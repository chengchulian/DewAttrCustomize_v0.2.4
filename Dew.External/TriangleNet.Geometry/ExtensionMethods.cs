using TriangleNet.Meshing;

namespace TriangleNet.Geometry;

public static class ExtensionMethods
{
	public static IMesh Triangulate(this IPolygon polygon)
	{
		return new GenericMesher().Triangulate(polygon, null, null);
	}

	public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options)
	{
		return new GenericMesher().Triangulate(polygon, options, null);
	}

	public static IMesh Triangulate(this IPolygon polygon, QualityOptions quality)
	{
		return new GenericMesher().Triangulate(polygon, null, quality);
	}

	public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options, QualityOptions quality)
	{
		return new GenericMesher().Triangulate(polygon, options, quality);
	}

	public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options, QualityOptions quality, ITriangulator triangulator)
	{
		return new GenericMesher(triangulator).Triangulate(polygon, options, quality);
	}

	public static bool Contains(this ITriangle triangle, Point p)
	{
		return triangle.Contains(p.X, p.Y);
	}

	public static bool Contains(this ITriangle triangle, double x, double y)
	{
		Vertex t0 = triangle.GetVertex(0);
		Vertex t1 = triangle.GetVertex(1);
		Vertex t2 = triangle.GetVertex(2);
		Point d0 = new Point(t1.X - t0.X, t1.Y - t0.Y);
		Point d1 = new Point(t2.X - t0.X, t2.Y - t0.Y);
		Point p = new Point(x - t0.X, y - t0.Y);
		Point c0 = new Point(0.0 - d0.Y, d0.X);
		Point c1 = new Point(0.0 - d1.Y, d1.X);
		double s = DotProduct(p, c1) / DotProduct(d0, c1);
		double v = DotProduct(p, c0) / DotProduct(d1, c0);
		if (s >= 0.0 && v >= 0.0 && s + v <= 1.0)
		{
			return true;
		}
		return false;
	}

	public static Rectangle Bounds(this ITriangle triangle)
	{
		Rectangle bounds = new Rectangle();
		for (int i = 0; i < 3; i++)
		{
			bounds.Expand(triangle.GetVertex(i));
		}
		return bounds;
	}

	internal static double DotProduct(Point p, Point q)
	{
		return p.X * q.X + p.Y * q.Y;
	}
}
