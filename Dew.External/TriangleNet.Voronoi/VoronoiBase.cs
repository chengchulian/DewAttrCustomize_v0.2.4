using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Topology;
using TriangleNet.Topology.DCEL;

namespace TriangleNet.Voronoi;

public abstract class VoronoiBase : DcelMesh
{
	protected IPredicates predicates;

	protected IVoronoiFactory factory;

	protected List<HalfEdge> rays;

	protected VoronoiBase(Mesh mesh, IVoronoiFactory factory, IPredicates predicates, bool generate)
		: base(initialize: false)
	{
		this.factory = factory;
		this.predicates = predicates;
		if (generate)
		{
			Generate(mesh);
		}
	}

	protected void Generate(Mesh mesh)
	{
		mesh.Renumber();
		edges = new List<HalfEdge>();
		rays = new List<HalfEdge>();
		global::TriangleNet.Topology.DCEL.Vertex[] vertices = new global::TriangleNet.Topology.DCEL.Vertex[mesh.triangles.Count + mesh.hullsize];
		Face[] faces = new Face[mesh.vertices.Count];
		if (factory == null)
		{
			factory = new DefaultVoronoiFactory();
		}
		factory.Initialize(vertices.Length, 2 * mesh.NumberOfEdges, faces.Length);
		List<HalfEdge>[] map = ComputeVertices(mesh, vertices);
		foreach (global::TriangleNet.Geometry.Vertex vertex in mesh.vertices.Values)
		{
			faces[vertex.id] = factory.CreateFace(vertex);
		}
		ComputeEdges(mesh, vertices, faces, map);
		ConnectEdges(map);
		base.vertices = new List<global::TriangleNet.Topology.DCEL.Vertex>(vertices);
		base.faces = new List<Face>(faces);
	}

	protected List<HalfEdge>[] ComputeVertices(Mesh mesh, global::TriangleNet.Topology.DCEL.Vertex[] vertices)
	{
		Otri tri = default(Otri);
		double xi = 0.0;
		double eta = 0.0;
		List<HalfEdge>[] map = new List<HalfEdge>[mesh.triangles.Count];
		foreach (Triangle t in mesh.triangles)
		{
			int id = t.id;
			tri.tri = t;
			Point pt = predicates.FindCircumcenter(tri.Org(), tri.Dest(), tri.Apex(), ref xi, ref eta);
			global::TriangleNet.Topology.DCEL.Vertex vertex = factory.CreateVertex(pt.x, pt.y);
			vertex.id = id;
			vertices[id] = vertex;
			map[id] = new List<HalfEdge>();
		}
		return map;
	}

	protected void ComputeEdges(Mesh mesh, global::TriangleNet.Topology.DCEL.Vertex[] vertices, Face[] faces, List<HalfEdge>[] map)
	{
		Otri neighbor = default(Otri);
		int count = mesh.triangles.Count;
		int j = 0;
		int k = 0;
		Otri tri = default(Otri);
		foreach (Triangle t in mesh.triangles)
		{
			int id = t.id;
			tri.tri = t;
			for (int i = 0; i < 3; i++)
			{
				tri.orient = i;
				tri.Sym(ref neighbor);
				int nid = neighbor.tri.id;
				if (id < nid || nid < 0)
				{
					global::TriangleNet.Geometry.Vertex org = tri.Org();
					global::TriangleNet.Geometry.Vertex dest = tri.Dest();
					Face face = faces[org.id];
					Face neighborFace = faces[dest.id];
					global::TriangleNet.Topology.DCEL.Vertex vertex = vertices[id];
					global::TriangleNet.Topology.DCEL.Vertex end;
					HalfEdge edge;
					HalfEdge twin;
					if (nid < 0)
					{
						double px = dest.y - org.y;
						double py = org.x - dest.x;
						end = factory.CreateVertex(vertex.x + px, vertex.y + py);
						end.id = count + j++;
						vertices[end.id] = end;
						edge = factory.CreateHalfEdge(end, face);
						twin = factory.CreateHalfEdge(vertex, neighborFace);
						face.edge = edge;
						face.bounded = false;
						map[id].Add(twin);
						rays.Add(twin);
					}
					else
					{
						end = vertices[nid];
						edge = factory.CreateHalfEdge(end, face);
						twin = factory.CreateHalfEdge(vertex, neighborFace);
						map[nid].Add(edge);
						map[id].Add(twin);
					}
					vertex.leaving = twin;
					end.leaving = edge;
					edge.twin = twin;
					twin.twin = edge;
					edge.id = k++;
					twin.id = k++;
					edges.Add(edge);
					edges.Add(twin);
				}
			}
		}
	}

	protected virtual void ConnectEdges(List<HalfEdge>[] map)
	{
		int length = map.Length;
		foreach (HalfEdge edge in edges)
		{
			int face = edge.face.generator.id;
			int id = edge.twin.origin.id;
			if (id >= length)
			{
				continue;
			}
			foreach (HalfEdge next in map[id])
			{
				if (next.face.generator.id == face)
				{
					edge.next = next;
					break;
				}
			}
		}
	}

	protected override IEnumerable<IEdge> EnumerateEdges()
	{
		List<IEdge> edges = new List<IEdge>(base.edges.Count / 2);
		foreach (HalfEdge edge in base.edges)
		{
			HalfEdge twin = edge.twin;
			if (twin == null)
			{
				edges.Add(new Edge(edge.origin.id, edge.next.origin.id));
			}
			else if (edge.id < twin.id)
			{
				edges.Add(new Edge(edge.origin.id, twin.origin.id));
			}
		}
		return edges;
	}
}
