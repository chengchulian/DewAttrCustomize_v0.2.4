using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.Voronoi.Legacy;

[Obsolete("Use TriangleNet.Voronoi.BoundedVoronoi class instead.")]
public class BoundedVoronoiLegacy : IVoronoi
{
	private IPredicates predicates = RobustPredicates.Default;

	private Mesh mesh;

	private Point[] points;

	private List<VoronoiRegion> regions;

	private List<Point> segPoints;

	private int segIndex;

	private Dictionary<int, SubSegment> subsegMap;

	private bool includeBoundary = true;

	public Point[] Points => points;

	public ICollection<VoronoiRegion> Regions => regions;

	public IEnumerable<IEdge> Edges => EnumerateEdges();

	public BoundedVoronoiLegacy(Mesh mesh)
		: this(mesh, includeBoundary: true)
	{
	}

	public BoundedVoronoiLegacy(Mesh mesh, bool includeBoundary)
	{
		this.mesh = mesh;
		this.includeBoundary = includeBoundary;
		Generate();
	}

	private void Generate()
	{
		mesh.Renumber();
		mesh.MakeVertexMap();
		regions = new List<VoronoiRegion>(mesh.vertices.Count);
		points = new Point[mesh.triangles.Count];
		segPoints = new List<Point>(mesh.subsegs.Count * 4);
		ComputeCircumCenters();
		TagBlindTriangles();
		foreach (Vertex v in mesh.vertices.Values)
		{
			if (v.type == VertexType.FreeVertex || v.label == 0)
			{
				ConstructCell(v);
			}
			else if (includeBoundary)
			{
				ConstructBoundaryCell(v);
			}
		}
		int length = points.Length;
		Array.Resize(ref points, length + segPoints.Count);
		for (int i = 0; i < segPoints.Count; i++)
		{
			points[length + i] = segPoints[i];
		}
		segPoints.Clear();
		segPoints = null;
	}

	private void ComputeCircumCenters()
	{
		Otri tri = default(Otri);
		double xi = 0.0;
		double eta = 0.0;
		foreach (Triangle triangle in mesh.triangles)
		{
			Triangle item = (tri.tri = triangle);
			Point pt = predicates.FindCircumcenter(tri.Org(), tri.Dest(), tri.Apex(), ref xi, ref eta);
			pt.id = item.id;
			points[item.id] = pt;
		}
	}

	private void TagBlindTriangles()
	{
		int blinded = 0;
		subsegMap = new Dictionary<int, SubSegment>();
		Otri f = default(Otri);
		Otri f2 = default(Otri);
		Osub e = default(Osub);
		Osub sub1 = default(Osub);
		foreach (Triangle triangle in mesh.triangles)
		{
			triangle.infected = false;
		}
		foreach (SubSegment ss in mesh.subsegs.Values)
		{
			Stack<Triangle> triangles = new Stack<Triangle>();
			e.seg = ss;
			e.orient = 0;
			e.Pivot(ref f);
			if (f.tri.id != -1 && !f.tri.infected)
			{
				triangles.Push(f.tri);
			}
			e.Sym();
			e.Pivot(ref f);
			if (f.tri.id != -1 && !f.tri.infected)
			{
				triangles.Push(f.tri);
			}
			while (triangles.Count > 0)
			{
				f.tri = triangles.Pop();
				f.orient = 0;
				if (!TriangleIsBlinded(ref f, ref e))
				{
					continue;
				}
				f.tri.infected = true;
				blinded++;
				subsegMap.Add(f.tri.hash, e.seg);
				f.orient = 0;
				while (f.orient < 3)
				{
					f.Sym(ref f2);
					f2.Pivot(ref sub1);
					if (f2.tri.id != -1 && !f2.tri.infected && sub1.seg.hash == -1)
					{
						triangles.Push(f2.tri);
					}
					f.orient++;
				}
			}
		}
		blinded = 0;
	}

	private bool TriangleIsBlinded(ref Otri tri, ref Osub seg)
	{
		Vertex torg = tri.Org();
		Vertex tdest = tri.Dest();
		Vertex tapex = tri.Apex();
		Vertex sorg = seg.Org();
		Vertex sdest = seg.Dest();
		Point c = points[tri.tri.id];
		if (SegmentsIntersect(sorg, sdest, c, torg, out var pt, strictIntersect: true))
		{
			return true;
		}
		if (SegmentsIntersect(sorg, sdest, c, tdest, out pt, strictIntersect: true))
		{
			return true;
		}
		if (SegmentsIntersect(sorg, sdest, c, tapex, out pt, strictIntersect: true))
		{
			return true;
		}
		return false;
	}

	private void ConstructCell(Vertex vertex)
	{
		VoronoiRegion region = new VoronoiRegion(vertex);
		regions.Add(region);
		Otri f = default(Otri);
		Otri f_init = default(Otri);
		Otri f_next = default(Otri);
		Osub sf = default(Osub);
		Osub sfn = default(Osub);
		int n = mesh.triangles.Count;
		List<Point> vpoints = new List<Point>();
		vertex.tri.Copy(ref f_init);
		if (f_init.Org() != vertex)
		{
			throw new Exception("ConstructCell: inconsistent topology.");
		}
		f_init.Copy(ref f);
		f_init.Onext(ref f_next);
		do
		{
			Point cc_f = points[f.tri.id];
			Point cc_f_next = points[f_next.tri.id];
			Point p;
			if (!f.tri.infected)
			{
				vpoints.Add(cc_f);
				if (f_next.tri.infected)
				{
					sfn.seg = subsegMap[f_next.tri.hash];
					if (SegmentsIntersect(sfn.Org(), sfn.Dest(), cc_f, cc_f_next, out p, strictIntersect: true))
					{
						p.id = n + segIndex++;
						segPoints.Add(p);
						vpoints.Add(p);
					}
				}
			}
			else
			{
				sf.seg = subsegMap[f.tri.hash];
				if (!f_next.tri.infected)
				{
					if (SegmentsIntersect(sf.Org(), sf.Dest(), cc_f, cc_f_next, out p, strictIntersect: true))
					{
						p.id = n + segIndex++;
						segPoints.Add(p);
						vpoints.Add(p);
					}
				}
				else
				{
					sfn.seg = subsegMap[f_next.tri.hash];
					if (!sf.Equal(sfn))
					{
						if (SegmentsIntersect(sf.Org(), sf.Dest(), cc_f, cc_f_next, out p, strictIntersect: true))
						{
							p.id = n + segIndex++;
							segPoints.Add(p);
							vpoints.Add(p);
						}
						if (SegmentsIntersect(sfn.Org(), sfn.Dest(), cc_f, cc_f_next, out p, strictIntersect: true))
						{
							p.id = n + segIndex++;
							segPoints.Add(p);
							vpoints.Add(p);
						}
					}
				}
			}
			f_next.Copy(ref f);
			f_next.Onext();
		}
		while (!f.Equals(f_init));
		region.Add(vpoints);
	}

	private void ConstructBoundaryCell(Vertex vertex)
	{
		VoronoiRegion region = new VoronoiRegion(vertex);
		regions.Add(region);
		Otri f = default(Otri);
		Otri f_init = default(Otri);
		Otri f_next = default(Otri);
		Otri f_prev = default(Otri);
		Osub sf = default(Osub);
		Osub sfn = default(Osub);
		int n = mesh.triangles.Count;
		List<Point> vpoints = new List<Point>();
		vertex.tri.Copy(ref f_init);
		if (f_init.Org() != vertex)
		{
			throw new Exception("ConstructBoundaryCell: inconsistent topology.");
		}
		f_init.Copy(ref f);
		f_init.Onext(ref f_next);
		f_init.Oprev(ref f_prev);
		if (f_prev.tri.id != -1)
		{
			while (f_prev.tri.id != -1 && !f_prev.Equals(f_init))
			{
				f_prev.Copy(ref f);
				f_prev.Oprev();
			}
			f.Copy(ref f_init);
			f.Onext(ref f_next);
		}
		Point p;
		if (f_prev.tri.id == -1)
		{
			p = new Point(vertex.x, vertex.y);
			p.id = n + segIndex++;
			segPoints.Add(p);
			vpoints.Add(p);
		}
		Vertex torg = f.Org();
		Vertex tdest = f.Dest();
		p = new Point((torg.x + tdest.x) / 2.0, (torg.y + tdest.y) / 2.0);
		p.id = n + segIndex++;
		segPoints.Add(p);
		vpoints.Add(p);
		do
		{
			Point cc_f = points[f.tri.id];
			if (f_next.tri.id == -1)
			{
				if (!f.tri.infected)
				{
					vpoints.Add(cc_f);
				}
				torg = f.Org();
				Vertex tapex = f.Apex();
				p = new Point((torg.x + tapex.x) / 2.0, (torg.y + tapex.y) / 2.0);
				p.id = n + segIndex++;
				segPoints.Add(p);
				vpoints.Add(p);
				break;
			}
			Point cc_f_next = points[f_next.tri.id];
			if (!f.tri.infected)
			{
				vpoints.Add(cc_f);
				if (f_next.tri.infected)
				{
					sfn.seg = subsegMap[f_next.tri.hash];
					if (SegmentsIntersect(sfn.Org(), sfn.Dest(), cc_f, cc_f_next, out p, strictIntersect: true))
					{
						p.id = n + segIndex++;
						segPoints.Add(p);
						vpoints.Add(p);
					}
				}
			}
			else
			{
				sf.seg = subsegMap[f.tri.hash];
				Vertex sorg = sf.Org();
				Vertex sdest = sf.Dest();
				if (!f_next.tri.infected)
				{
					tdest = f.Dest();
					Vertex tapex = f.Apex();
					Point bisec = new Point((tdest.x + tapex.x) / 2.0, (tdest.y + tapex.y) / 2.0);
					if (SegmentsIntersect(sorg, sdest, bisec, cc_f, out p, strictIntersect: false))
					{
						p.id = n + segIndex++;
						segPoints.Add(p);
						vpoints.Add(p);
					}
					if (SegmentsIntersect(sorg, sdest, cc_f, cc_f_next, out p, strictIntersect: true))
					{
						p.id = n + segIndex++;
						segPoints.Add(p);
						vpoints.Add(p);
					}
				}
				else
				{
					sfn.seg = subsegMap[f_next.tri.hash];
					if (!sf.Equal(sfn))
					{
						if (SegmentsIntersect(sorg, sdest, cc_f, cc_f_next, out p, strictIntersect: true))
						{
							p.id = n + segIndex++;
							segPoints.Add(p);
							vpoints.Add(p);
						}
						if (SegmentsIntersect(sfn.Org(), sfn.Dest(), cc_f, cc_f_next, out p, strictIntersect: true))
						{
							p.id = n + segIndex++;
							segPoints.Add(p);
							vpoints.Add(p);
						}
					}
					else
					{
						Point bisec2 = new Point((torg.x + tdest.x) / 2.0, (torg.y + tdest.y) / 2.0);
						if (SegmentsIntersect(sorg, sdest, bisec2, cc_f_next, out p, strictIntersect: false))
						{
							p.id = n + segIndex++;
							segPoints.Add(p);
							vpoints.Add(p);
						}
					}
				}
			}
			f_next.Copy(ref f);
			f_next.Onext();
		}
		while (!f.Equals(f_init));
		region.Add(vpoints);
	}

	private bool SegmentsIntersect(Point p1, Point p2, Point p3, Point p4, out Point p, bool strictIntersect)
	{
		p = null;
		double Ax = p1.x;
		double Ay = p1.y;
		double Bx = p2.x;
		double By = p2.y;
		double Cx = p3.x;
		double Cy = p3.y;
		double Dx = p4.x;
		double Dy = p4.y;
		if ((Ax == Bx && Ay == By) || (Cx == Dx && Cy == Dy))
		{
			return false;
		}
		if ((Ax == Cx && Ay == Cy) || (Bx == Cx && By == Cy) || (Ax == Dx && Ay == Dy) || (Bx == Dx && By == Dy))
		{
			return false;
		}
		Bx -= Ax;
		By -= Ay;
		Cx -= Ax;
		Cy -= Ay;
		Dx -= Ax;
		Dy -= Ay;
		double distAB = Math.Sqrt(Bx * Bx + By * By);
		double theCos = Bx / distAB;
		double theSin = By / distAB;
		double num = Cx * theCos + Cy * theSin;
		Cy = Cy * theCos - Cx * theSin;
		Cx = num;
		double num2 = Dx * theCos + Dy * theSin;
		Dy = Dy * theCos - Dx * theSin;
		Dx = num2;
		if ((Cy < 0.0 && Dy < 0.0) || (Cy >= 0.0 && Dy >= 0.0 && strictIntersect))
		{
			return false;
		}
		double ABpos = Dx + (Cx - Dx) * Dy / (Dy - Cy);
		if (ABpos < 0.0 || (ABpos > distAB && strictIntersect))
		{
			return false;
		}
		p = new Point(Ax + ABpos * theCos, Ay + ABpos * theSin);
		return true;
	}

	private IEnumerable<IEdge> EnumerateEdges()
	{
		List<IEdge> edges = new List<IEdge>(Regions.Count * 2);
		foreach (VoronoiRegion region in Regions)
		{
			Point first = null;
			Point last = null;
			foreach (Point pt in region.Vertices)
			{
				if (first == null)
				{
					first = pt;
					last = pt;
				}
				else
				{
					edges.Add(new Edge(last.id, pt.id));
					last = pt;
				}
			}
			if (region.Bounded && first != null)
			{
				edges.Add(new Edge(last.id, first.id));
			}
		}
		return edges;
	}
}
