using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.Meshing.Iterators;

public class VertexCirculator
{
	private List<Otri> cache = new List<Otri>();

	public VertexCirculator(Mesh mesh)
	{
		mesh.MakeVertexMap();
	}

	public IEnumerable<Vertex> EnumerateVertices(Vertex vertex)
	{
		BuildCache(vertex, vertices: true);
		foreach (Otri item2 in cache)
		{
			yield return item2.Dest();
		}
	}

	public IEnumerable<ITriangle> EnumerateTriangles(Vertex vertex)
	{
		BuildCache(vertex, vertices: false);
		foreach (Otri item2 in cache)
		{
			yield return item2.tri;
		}
	}

	private void BuildCache(Vertex vertex, bool vertices)
	{
		cache.Clear();
		Otri init = vertex.tri;
		Otri next = default(Otri);
		Otri prev = default(Otri);
		init.Copy(ref next);
		while (next.tri.id != -1)
		{
			cache.Add(next);
			next.Copy(ref prev);
			next.Onext();
			if (next.Equals(init))
			{
				break;
			}
		}
		if (next.tri.id != -1)
		{
			return;
		}
		init.Copy(ref next);
		if (vertices)
		{
			prev.Lnext();
			cache.Add(prev);
		}
		next.Oprev();
		while (next.tri.id != -1)
		{
			cache.Insert(0, next);
			next.Oprev();
			if (next.Equals(init))
			{
				break;
			}
		}
	}
}
