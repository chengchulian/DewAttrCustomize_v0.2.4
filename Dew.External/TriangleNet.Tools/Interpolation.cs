using TriangleNet.Geometry;

namespace TriangleNet.Tools;

public static class Interpolation
{
	public static void InterpolateZ(Point p, ITriangle triangle)
	{
		Vertex org = triangle.GetVertex(0);
		Vertex dest = triangle.GetVertex(1);
		Vertex apex = triangle.GetVertex(2);
		double xdo = dest.x - org.x;
		double ydo = dest.y - org.y;
		double xao = apex.x - org.x;
		double yao = apex.y - org.y;
		double denominator = 0.5 / (xdo * yao - xao * ydo);
		double dx = p.x - org.x;
		double dy = p.y - org.y;
		double xi = (yao * dx - xao * dy) * (2.0 * denominator);
		double eta = (xdo * dy - ydo * dx) * (2.0 * denominator);
		p.z = org.z + xi * (dest.z - org.z) + eta * (apex.z - org.z);
	}
}
