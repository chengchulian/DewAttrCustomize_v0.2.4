using System.Collections.Generic;
using TriangleNet.Geometry;

namespace TriangleNet.Topology.DCEL;

public class DcelMesh
{
	protected List<Vertex> vertices;

	protected List<HalfEdge> edges;

	protected List<Face> faces;

	public List<Vertex> Vertices => vertices;

	public List<HalfEdge> HalfEdges => edges;

	public List<Face> Faces => faces;

	public IEnumerable<IEdge> Edges => EnumerateEdges();

	public DcelMesh()
		: this(initialize: true)
	{
	}

	protected DcelMesh(bool initialize)
	{
		if (initialize)
		{
			vertices = new List<Vertex>();
			edges = new List<HalfEdge>();
			faces = new List<Face>();
		}
	}

	public virtual bool IsConsistent(bool closed = true, int depth = 0)
	{
		foreach (Vertex vertex in vertices)
		{
			if (vertex.id >= 0)
			{
				if (vertex.leaving == null)
				{
					return false;
				}
				if (vertex.Leaving.Origin.id != vertex.id)
				{
					return false;
				}
			}
		}
		foreach (Face face in faces)
		{
			if (face.ID >= 0)
			{
				if (face.edge == null)
				{
					return false;
				}
				if (face.id != face.edge.face.id)
				{
					return false;
				}
			}
		}
		foreach (HalfEdge edge in edges)
		{
			if (edge.id >= 0)
			{
				if (edge.twin == null)
				{
					return false;
				}
				if (edge.origin == null)
				{
					return false;
				}
				if (edge.face == null)
				{
					return false;
				}
				if (closed && edge.next == null)
				{
					return false;
				}
			}
		}
		foreach (HalfEdge edge2 in edges)
		{
			if (edge2.id < 0)
			{
				continue;
			}
			HalfEdge twin = edge2.twin;
			HalfEdge next = edge2.next;
			if (edge2.id != twin.twin.id)
			{
				return false;
			}
			if (closed)
			{
				if (next.origin.id != twin.origin.id)
				{
					return false;
				}
				if (next.twin.next.origin.id != edge2.twin.origin.id)
				{
					return false;
				}
			}
		}
		if (closed && depth > 0)
		{
			foreach (Face face2 in faces)
			{
				if (face2.id >= 0)
				{
					HalfEdge edge3 = face2.edge;
					HalfEdge next2 = edge3.next;
					int id = edge3.id;
					int k = 0;
					while (next2.id != id && k < depth)
					{
						next2 = next2.next;
						k++;
					}
					if (next2.id != id)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	public void ResolveBoundaryEdges()
	{
		Dictionary<int, HalfEdge> map = new Dictionary<int, HalfEdge>();
		foreach (HalfEdge edge in edges)
		{
			if (edge.twin == null)
			{
				HalfEdge twin = (edge.twin = new HalfEdge(edge.next.origin, Face.Empty));
				twin.twin = edge;
				map.Add(twin.origin.id, twin);
			}
		}
		int j = edges.Count;
		foreach (HalfEdge edge2 in map.Values)
		{
			edge2.id = j++;
			edge2.next = map[edge2.twin.origin.id];
			edges.Add(edge2);
		}
	}

	protected virtual IEnumerable<IEdge> EnumerateEdges()
	{
		List<IEdge> edges = new List<IEdge>(this.edges.Count / 2);
		foreach (HalfEdge edge in this.edges)
		{
			HalfEdge twin = edge.twin;
			if (edge.id < twin.id)
			{
				edges.Add(new Edge(edge.origin.id, twin.origin.id));
			}
		}
		return edges;
	}
}
