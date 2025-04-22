using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet;

public class TriangleLocator
{
	private TriangleSampler sampler;

	private Mesh mesh;

	private IPredicates predicates;

	internal Otri recenttri;

	public TriangleLocator(Mesh mesh)
		: this(mesh, RobustPredicates.Default)
	{
	}

	public TriangleLocator(Mesh mesh, IPredicates predicates)
	{
		this.mesh = mesh;
		this.predicates = predicates;
		sampler = new TriangleSampler(mesh);
	}

	public void Update(ref Otri otri)
	{
		otri.Copy(ref recenttri);
	}

	public void Reset()
	{
		sampler.Reset();
		recenttri.tri = null;
	}

	public LocateResult PreciseLocate(Point searchpoint, ref Otri searchtri, bool stopatsubsegment)
	{
		Otri backtracktri = default(Otri);
		Osub checkedge = default(Osub);
		Vertex forg = searchtri.Org();
		Vertex fdest = searchtri.Dest();
		Vertex fapex = searchtri.Apex();
		while (true)
		{
			if (fapex.x == searchpoint.x && fapex.y == searchpoint.y)
			{
				searchtri.Lprev();
				return LocateResult.OnVertex;
			}
			double destorient = predicates.CounterClockwise(forg, fapex, searchpoint);
			double orgorient = predicates.CounterClockwise(fapex, fdest, searchpoint);
			bool moveleft;
			if (destorient > 0.0)
			{
				moveleft = !(orgorient > 0.0) || (fapex.x - searchpoint.x) * (fdest.x - forg.x) + (fapex.y - searchpoint.y) * (fdest.y - forg.y) > 0.0;
			}
			else
			{
				if (!(orgorient > 0.0))
				{
					if (destorient == 0.0)
					{
						searchtri.Lprev();
						return LocateResult.OnEdge;
					}
					if (orgorient == 0.0)
					{
						searchtri.Lnext();
						return LocateResult.OnEdge;
					}
					return LocateResult.InTriangle;
				}
				moveleft = false;
			}
			if (moveleft)
			{
				searchtri.Lprev(ref backtracktri);
				fdest = fapex;
			}
			else
			{
				searchtri.Lnext(ref backtracktri);
				forg = fapex;
			}
			backtracktri.Sym(ref searchtri);
			if (mesh.checksegments && stopatsubsegment)
			{
				backtracktri.Pivot(ref checkedge);
				if (checkedge.seg.hash != -1)
				{
					backtracktri.Copy(ref searchtri);
					return LocateResult.Outside;
				}
			}
			if (searchtri.tri.id == -1)
			{
				break;
			}
			fapex = searchtri.Apex();
		}
		backtracktri.Copy(ref searchtri);
		return LocateResult.Outside;
	}

	public LocateResult Locate(Point searchpoint, ref Otri searchtri)
	{
		Otri sampletri = default(Otri);
		Vertex torg = searchtri.Org();
		double searchdist = (searchpoint.x - torg.x) * (searchpoint.x - torg.x) + (searchpoint.y - torg.y) * (searchpoint.y - torg.y);
		if (recenttri.tri != null && !Otri.IsDead(recenttri.tri))
		{
			torg = recenttri.Org();
			if (torg.x == searchpoint.x && torg.y == searchpoint.y)
			{
				recenttri.Copy(ref searchtri);
				return LocateResult.OnVertex;
			}
			double dist = (searchpoint.x - torg.x) * (searchpoint.x - torg.x) + (searchpoint.y - torg.y) * (searchpoint.y - torg.y);
			if (dist < searchdist)
			{
				recenttri.Copy(ref searchtri);
				searchdist = dist;
			}
		}
		sampler.Update();
		foreach (Triangle t in sampler)
		{
			sampletri.tri = t;
			if (!Otri.IsDead(sampletri.tri))
			{
				torg = sampletri.Org();
				double dist = (searchpoint.x - torg.x) * (searchpoint.x - torg.x) + (searchpoint.y - torg.y) * (searchpoint.y - torg.y);
				if (dist < searchdist)
				{
					sampletri.Copy(ref searchtri);
					searchdist = dist;
				}
			}
		}
		torg = searchtri.Org();
		Vertex tdest = searchtri.Dest();
		if (torg.x == searchpoint.x && torg.y == searchpoint.y)
		{
			return LocateResult.OnVertex;
		}
		if (tdest.x == searchpoint.x && tdest.y == searchpoint.y)
		{
			searchtri.Lnext();
			return LocateResult.OnVertex;
		}
		double ahead = predicates.CounterClockwise(torg, tdest, searchpoint);
		if (ahead < 0.0)
		{
			searchtri.Sym();
		}
		else if (ahead == 0.0 && torg.x < searchpoint.x == searchpoint.x < tdest.x && torg.y < searchpoint.y == searchpoint.y < tdest.y)
		{
			return LocateResult.OnEdge;
		}
		return PreciseLocate(searchpoint, ref searchtri, stopatsubsegment: false);
	}
}
