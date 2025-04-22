using System;
using TriangleNet.Geometry;
using TriangleNet.Topology.DCEL;
using TriangleNet.Voronoi;

namespace TriangleNet.Smoothing;

internal class VoronoiFactory : IVoronoiFactory
{
	private class ObjectPool<T> where T : class
	{
		private int index;

		private int count;

		private T[] pool;

		public int Count => count;

		public int Capacity
		{
			get
			{
				return pool.Length;
			}
			set
			{
				Resize(value);
			}
		}

		public ObjectPool(int capacity = 3)
		{
			index = 0;
			count = 0;
			pool = new T[capacity];
		}

		public ObjectPool(T[] pool)
		{
			index = 0;
			count = 0;
			this.pool = pool;
		}

		public bool TryGet(out T obj)
		{
			if (index < count)
			{
				obj = pool[index++];
				return true;
			}
			obj = null;
			return false;
		}

		public void Put(T obj)
		{
			int capacity = pool.Length;
			if (capacity <= count)
			{
				Resize(2 * capacity);
			}
			pool[count++] = obj;
			index++;
		}

		public void Release()
		{
			index = 0;
		}

		private void Resize(int size)
		{
			if (size > count)
			{
				Array.Resize(ref pool, size);
			}
		}
	}

	private ObjectPool<global::TriangleNet.Topology.DCEL.Vertex> vertices;

	private ObjectPool<HalfEdge> edges;

	private ObjectPool<Face> faces;

	public VoronoiFactory()
	{
		vertices = new ObjectPool<global::TriangleNet.Topology.DCEL.Vertex>();
		edges = new ObjectPool<HalfEdge>();
		faces = new ObjectPool<Face>();
	}

	public void Initialize(int vertexCount, int edgeCount, int faceCount)
	{
		vertices.Capacity = vertexCount;
		edges.Capacity = edgeCount;
		faces.Capacity = faceCount;
		for (int i = vertices.Count; i < vertexCount; i++)
		{
			vertices.Put(new global::TriangleNet.Topology.DCEL.Vertex(0.0, 0.0));
		}
		for (int j = edges.Count; j < edgeCount; j++)
		{
			edges.Put(new HalfEdge(null));
		}
		for (int k = faces.Count; k < faceCount; k++)
		{
			faces.Put(new Face(null));
		}
		Reset();
	}

	public void Reset()
	{
		vertices.Release();
		edges.Release();
		faces.Release();
	}

	public global::TriangleNet.Topology.DCEL.Vertex CreateVertex(double x, double y)
	{
		if (vertices.TryGet(out var vertex))
		{
			vertex.x = x;
			vertex.y = y;
			vertex.leaving = null;
			return vertex;
		}
		vertex = new global::TriangleNet.Topology.DCEL.Vertex(x, y);
		vertices.Put(vertex);
		return vertex;
	}

	public HalfEdge CreateHalfEdge(global::TriangleNet.Topology.DCEL.Vertex origin, Face face)
	{
		if (edges.TryGet(out var edge))
		{
			edge.origin = origin;
			edge.face = face;
			edge.next = null;
			edge.twin = null;
			if (face != null && face.edge == null)
			{
				face.edge = edge;
			}
			return edge;
		}
		edge = new HalfEdge(origin, face);
		edges.Put(edge);
		return edge;
	}

	public Face CreateFace(global::TriangleNet.Geometry.Vertex vertex)
	{
		if (faces.TryGet(out var face))
		{
			face.id = vertex.id;
			face.generator = vertex;
			face.edge = null;
			return face;
		}
		face = new Face(vertex);
		faces.Put(face);
		return face;
	}
}
