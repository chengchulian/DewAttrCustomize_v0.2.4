using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.Voronoi.Legacy;

[Obsolete("Use TriangleNet.Voronoi.StandardVoronoi class instead.")]
public class SimpleVoronoi : IVoronoi
{
	private IPredicates predicates = RobustPredicates.Default;

	private Mesh mesh;

	private Point[] points;

	private Dictionary<int, VoronoiRegion> regions;

	private Dictionary<int, Point> rayPoints;

	private int rayIndex;

	private Rectangle bounds;

	public Point[] Points => points;

	public ICollection<VoronoiRegion> Regions => regions.Values;

	public IEnumerable<IEdge> Edges => EnumerateEdges();

	public SimpleVoronoi(Mesh mesh)
	{
		this.mesh = mesh;
		Generate();
	}

	private void Generate()
	{
		mesh.Renumber();
		mesh.MakeVertexMap();
		points = new Point[mesh.triangles.Count + mesh.hullsize];
		regions = new Dictionary<int, VoronoiRegion>(mesh.vertices.Count);
		rayPoints = new Dictionary<int, Point>();
		rayIndex = 0;
		bounds = new Rectangle();
		ComputeCircumCenters();
		foreach (Vertex vertex in mesh.vertices.Values)
		{
			regions.Add(vertex.id, new VoronoiRegion(vertex));
		}
		foreach (VoronoiRegion region in regions.Values)
		{
			ConstructCell(region);
		}
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
			bounds.Expand(pt);
		}
		double ds = Math.Max(bounds.Width, bounds.Height);
		bounds.Resize(ds, ds);
	}

	private void ConstructCell(VoronoiRegion region)
	{
		Vertex obj = region.Generator as Vertex;
		List<Point> vpoints = new List<Point>();
		Otri f = default(Otri);
		Otri f_init = default(Otri);
		Otri f_next = default(Otri);
		Otri f_prev = default(Otri);
		Osub sub = default(Osub);
		obj.tri.Copy(ref f_init);
		f_init.Copy(ref f);
		f_init.Onext(ref f_next);
		if (f_next.tri.id == -1)
		{
			f_init.Oprev(ref f_prev);
			if (f_prev.tri.id != -1)
			{
				f_init.Copy(ref f_next);
				f_init.Oprev();
				f_init.Copy(ref f);
			}
		}
		while (f_next.tri.id != -1)
		{
			vpoints.Add(points[f.tri.id]);
			region.AddNeighbor(f.tri.id, regions[f.Apex().id]);
			if (f_next.Equals(f_init))
			{
				region.Add(vpoints);
				return;
			}
			f_next.Copy(ref f);
			f_next.Onext();
		}
		region.Bounded = false;
		int n = mesh.triangles.Count;
		f.Lprev(ref f_next);
		f_next.Pivot(ref sub);
		int sid = sub.seg.hash;
		vpoints.Add(points[f.tri.id]);
		region.AddNeighbor(f.tri.id, regions[f.Apex().id]);
		if (!rayPoints.TryGetValue(sid, out var intersection))
		{
			Vertex torg = f.Org();
			Vertex tapex = f.Apex();
			BoxRayIntersection(points[f.tri.id], torg.y - tapex.y, tapex.x - torg.x, out intersection);
			intersection.id = n + rayIndex;
			points[n + rayIndex] = intersection;
			rayIndex++;
			rayPoints.Add(sid, intersection);
		}
		vpoints.Add(intersection);
		vpoints.Reverse();
		f_init.Copy(ref f);
		f.Oprev(ref f_prev);
		while (f_prev.tri.id != -1)
		{
			vpoints.Add(points[f_prev.tri.id]);
			region.AddNeighbor(f_prev.tri.id, regions[f_prev.Apex().id]);
			f_prev.Copy(ref f);
			f_prev.Oprev();
		}
		f.Pivot(ref sub);
		sid = sub.seg.hash;
		if (!rayPoints.TryGetValue(sid, out intersection))
		{
			Vertex torg = f.Org();
			Vertex tdest = f.Dest();
			BoxRayIntersection(points[f.tri.id], tdest.y - torg.y, torg.x - tdest.x, out intersection);
			intersection.id = n + rayIndex;
			rayPoints.Add(sid, intersection);
			points[n + rayIndex] = intersection;
			rayIndex++;
		}
		vpoints.Add(intersection);
		region.AddNeighbor(intersection.id, regions[f.Dest().id]);
		vpoints.Reverse();
		region.Add(vpoints);
	}

	private bool BoxRayIntersection(Point pt, double dx, double dy, out Point intersect)
	{
		double x = pt.x;
		double y = pt.y;
		double minX = bounds.Left;
		double maxX = bounds.Right;
		double minY = bounds.Bottom;
		double maxY = bounds.Top;
		if (x < minX || x > maxX || y < minY || y > maxY)
		{
			intersect = null;
			return false;
		}
		double t1;
		double x2;
		double y2;
		if (dx < 0.0)
		{
			t1 = (minX - x) / dx;
			x2 = minX;
			y2 = y + t1 * dy;
		}
		else if (dx > 0.0)
		{
			t1 = (maxX - x) / dx;
			x2 = maxX;
			y2 = y + t1 * dy;
		}
		else
		{
			t1 = double.MaxValue;
			x2 = (y2 = 0.0);
		}
		double t2;
		double x3;
		double y3;
		if (dy < 0.0)
		{
			t2 = (minY - y) / dy;
			x3 = x + t2 * dx;
			y3 = minY;
		}
		else if (dy > 0.0)
		{
			t2 = (maxY - y) / dy;
			x3 = x + t2 * dx;
			y3 = maxY;
		}
		else
		{
			t2 = double.MaxValue;
			x3 = (y3 = 0.0);
		}
		if (t1 < t2)
		{
			intersect = new Point(x2, y2);
		}
		else
		{
			intersect = new Point(x3, y3);
		}
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
