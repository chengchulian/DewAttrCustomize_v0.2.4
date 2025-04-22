using TriangleNet.Geometry;
using TriangleNet.Topology.DCEL;

namespace TriangleNet.Voronoi;

public interface IVoronoiFactory
{
	void Initialize(int vertexCount, int edgeCount, int faceCount);

	void Reset();

	global::TriangleNet.Topology.DCEL.Vertex CreateVertex(double x, double y);

	HalfEdge CreateHalfEdge(global::TriangleNet.Topology.DCEL.Vertex origin, Face face);

	Face CreateFace(global::TriangleNet.Geometry.Vertex vertex);
}
