using TriangleNet.Geometry;
using TriangleNet.Tools;
using TriangleNet.Topology.DCEL;

namespace TriangleNet.Voronoi;

public class BoundedVoronoi : VoronoiBase
{
	private int offset;

	public BoundedVoronoi(Mesh mesh)
		: this(mesh, new DefaultVoronoiFactory(), RobustPredicates.Default)
	{
	}

	public BoundedVoronoi(Mesh mesh, IVoronoiFactory factory, IPredicates predicates)
		: base(mesh, factory, predicates, generate: true)
	{
		offset = vertices.Count;
		vertices.Capacity = offset + mesh.hullsize;
		PostProcess();
		ResolveBoundaryEdges();
	}

	private void PostProcess()
	{
		foreach (HalfEdge edge in rays)
		{
			HalfEdge twin = edge.twin;
			global::TriangleNet.Geometry.Vertex v1 = (global::TriangleNet.Geometry.Vertex)edge.face.generator;
			global::TriangleNet.Geometry.Vertex v2 = (global::TriangleNet.Geometry.Vertex)twin.face.generator;
			if (predicates.CounterClockwise(v1, v2, edge.origin) <= 0.0)
			{
				HandleCase1(edge, v1, v2);
			}
			else
			{
				HandleCase2(edge, v1, v2);
			}
		}
	}

	private void HandleCase1(HalfEdge edge, global::TriangleNet.Geometry.Vertex v1, global::TriangleNet.Geometry.Vertex v2)
	{
		global::TriangleNet.Topology.DCEL.Vertex origin = edge.twin.origin;
		origin.x = (v1.x + v2.x) / 2.0;
		origin.y = (v1.y + v2.y) / 2.0;
		global::TriangleNet.Topology.DCEL.Vertex gen = factory.CreateVertex(v1.x, v1.y);
		HalfEdge h1 = factory.CreateHalfEdge(edge.twin.origin, edge.face);
		HalfEdge h2 = factory.CreateHalfEdge(gen, edge.face);
		edge.next = h1;
		h1.next = h2;
		h2.next = edge.face.edge;
		gen.leaving = h2;
		edge.face.edge = h2;
		edges.Add(h1);
		edges.Add(h2);
		h2.id = (h1.id = edges.Count) + 1;
		gen.id = offset++;
		vertices.Add(gen);
	}

	private void HandleCase2(HalfEdge edge, global::TriangleNet.Geometry.Vertex v1, global::TriangleNet.Geometry.Vertex v2)
	{
		Point p1 = edge.origin;
		Point p2 = edge.twin.origin;
		HalfEdge e1 = edge.twin.next;
		HalfEdge e2 = e1.twin.next;
		IntersectionHelper.IntersectSegments(v1, v2, e1.origin, e1.twin.origin, ref p2);
		IntersectionHelper.IntersectSegments(v1, v2, e2.origin, e2.twin.origin, ref p1);
		e1.twin.next = edge.twin;
		edge.twin.next = e2;
		edge.twin.face = e2.face;
		e1.origin = edge.twin.origin;
		edge.twin.twin = null;
		edge.twin = null;
		global::TriangleNet.Topology.DCEL.Vertex gen = factory.CreateVertex(v1.x, v1.y);
		HalfEdge he = (edge.next = factory.CreateHalfEdge(gen, edge.face));
		he.next = edge.face.edge;
		edge.face.edge = he;
		edges.Add(he);
		he.id = edges.Count;
		gen.id = offset++;
		vertices.Add(gen);
	}
}
