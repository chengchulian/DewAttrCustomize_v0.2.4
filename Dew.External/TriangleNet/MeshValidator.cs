using TriangleNet.Geometry;
using TriangleNet.Logging;
using TriangleNet.Topology;

namespace TriangleNet;

public static class MeshValidator
{
	private static RobustPredicates predicates = RobustPredicates.Default;

	public static bool IsConsistent(Mesh mesh)
	{
		Otri tri = default(Otri);
		Otri oppotri = default(Otri);
		Otri oppooppotri = default(Otri);
		ILog<LogItem> logger = Log.Instance;
		bool saveexact = Behavior.NoExact;
		Behavior.NoExact = false;
		int horrors = 0;
		foreach (Triangle triangle in mesh.triangles)
		{
			Triangle t = (tri.tri = triangle);
			tri.orient = 0;
			while (tri.orient < 3)
			{
				Vertex org = tri.Org();
				Vertex dest = tri.Dest();
				if (tri.orient == 0)
				{
					Vertex apex = tri.Apex();
					if (predicates.CounterClockwise(org, dest, apex) <= 0.0)
					{
						if (Log.Verbose)
						{
							logger.Warning($"Triangle is flat or inverted (ID {t.id}).", "MeshValidator.IsConsistent()");
						}
						horrors++;
					}
				}
				tri.Sym(ref oppotri);
				if (oppotri.tri.id != -1)
				{
					oppotri.Sym(ref oppooppotri);
					if (tri.tri != oppooppotri.tri || tri.orient != oppooppotri.orient)
					{
						if (tri.tri == oppooppotri.tri && Log.Verbose)
						{
							logger.Warning("Asymmetric triangle-triangle bond: (Right triangle, wrong orientation)", "MeshValidator.IsConsistent()");
						}
						horrors++;
					}
					Vertex oppoorg = oppotri.Org();
					Vertex oppodest = oppotri.Dest();
					if (org != oppodest || dest != oppoorg)
					{
						if (Log.Verbose)
						{
							logger.Warning("Mismatched edge coordinates between two triangles.", "MeshValidator.IsConsistent()");
						}
						horrors++;
					}
				}
				tri.orient++;
			}
		}
		mesh.MakeVertexMap();
		foreach (Vertex v in mesh.vertices.Values)
		{
			if (v.tri.tri == null && Log.Verbose)
			{
				logger.Warning("Vertex (ID " + v.id + ") not connected to mesh (duplicate input vertex?)", "MeshValidator.IsConsistent()");
			}
		}
		Behavior.NoExact = saveexact;
		return horrors == 0;
	}

	public static bool IsDelaunay(Mesh mesh)
	{
		return IsDelaunay(mesh, constrained: false);
	}

	public static bool IsConstrainedDelaunay(Mesh mesh)
	{
		return IsDelaunay(mesh, constrained: true);
	}

	private static bool IsDelaunay(Mesh mesh, bool constrained)
	{
		Otri loop = default(Otri);
		Otri oppotri = default(Otri);
		Osub opposubseg = default(Osub);
		ILog<LogItem> logger = Log.Instance;
		bool saveexact = Behavior.NoExact;
		Behavior.NoExact = false;
		int horrors = 0;
		Vertex inf1 = mesh.infvertex1;
		Vertex inf2 = mesh.infvertex2;
		Vertex inf3 = mesh.infvertex3;
		foreach (Triangle tri in mesh.triangles)
		{
			loop.tri = tri;
			loop.orient = 0;
			while (loop.orient < 3)
			{
				Vertex org = loop.Org();
				Vertex dest = loop.Dest();
				Vertex apex = loop.Apex();
				loop.Sym(ref oppotri);
				Vertex oppoapex = oppotri.Apex();
				bool shouldbedelaunay = loop.tri.id < oppotri.tri.id && !Otri.IsDead(oppotri.tri) && oppotri.tri.id != -1 && org != inf1 && org != inf2 && org != inf3 && dest != inf1 && dest != inf2 && dest != inf3 && apex != inf1 && apex != inf2 && apex != inf3 && oppoapex != inf1 && oppoapex != inf2 && oppoapex != inf3;
				if (constrained && mesh.checksegments && shouldbedelaunay)
				{
					loop.Pivot(ref opposubseg);
					if (opposubseg.seg.hash != -1)
					{
						shouldbedelaunay = false;
					}
				}
				if (shouldbedelaunay && predicates.NonRegular(org, dest, apex, oppoapex) > 0.0)
				{
					if (Log.Verbose)
					{
						logger.Warning($"Non-regular pair of triangles found (IDs {loop.tri.id}/{oppotri.tri.id}).", "MeshValidator.IsDelaunay()");
					}
					horrors++;
				}
				loop.orient++;
			}
		}
		Behavior.NoExact = saveexact;
		return horrors == 0;
	}
}
