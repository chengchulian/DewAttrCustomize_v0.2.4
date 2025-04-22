using TriangleNet.Geometry;
using TriangleNet.Topology.DCEL;

namespace TriangleNet.Voronoi;

public class DefaultVoronoiFactory : IVoronoiFactory
{
	public void Initialize(int vertexCount, int edgeCount, int faceCount)
	{
	}

	public void Reset()
	{
	}

	public global::TriangleNet.Topology.DCEL.Vertex CreateVertex(double x, double y)
	{
		return new global::TriangleNet.Topology.DCEL.Vertex(x, y);
	}

	public HalfEdge CreateHalfEdge(global::TriangleNet.Topology.DCEL.Vertex origin, Face face)
	{
		return new HalfEdge(origin, face);
	}

	public Face CreateFace(global::TriangleNet.Geometry.Vertex vertex)
	{
		return new Face(vertex);
	}
}
