using TriangleNet.Geometry;

namespace TriangleNet.Tools;

public static class IntersectionHelper
{
	public static void IntersectSegments(Point p0, Point p1, Point q0, Point q1, ref Point c0)
	{
		double ux = p1.x - p0.x;
		double uy = p1.y - p0.y;
		double vx = q1.x - q0.x;
		double vy = q1.y - q0.y;
		double wx = p0.x - q0.x;
		double wy = p0.y - q0.y;
		double d = ux * vy - uy * vx;
		double s = (vx * wy - vy * wx) / d;
		c0.x = p0.X + s * ux;
		c0.y = p0.Y + s * uy;
	}

	public static bool LiangBarsky(Rectangle rect, Point p0, Point p1, ref Point c0, ref Point c1)
	{
		double xmin = rect.Left;
		double xmax = rect.Right;
		double ymin = rect.Bottom;
		double ymax = rect.Top;
		double x0 = p0.X;
		double y0 = p0.Y;
		double x1 = p1.X;
		double y1 = p1.Y;
		double t0 = 0.0;
		double t1 = 1.0;
		double dx = x1 - x0;
		double dy = y1 - y0;
		double p2 = 0.0;
		double q = 0.0;
		for (int edge = 0; edge < 4; edge++)
		{
			if (edge == 0)
			{
				p2 = 0.0 - dx;
				q = 0.0 - (xmin - x0);
			}
			if (edge == 1)
			{
				p2 = dx;
				q = xmax - x0;
			}
			if (edge == 2)
			{
				p2 = 0.0 - dy;
				q = 0.0 - (ymin - y0);
			}
			if (edge == 3)
			{
				p2 = dy;
				q = ymax - y0;
			}
			double r = q / p2;
			if (p2 == 0.0 && q < 0.0)
			{
				return false;
			}
			if (p2 < 0.0)
			{
				if (r > t1)
				{
					return false;
				}
				if (r > t0)
				{
					t0 = r;
				}
			}
			else if (p2 > 0.0)
			{
				if (r < t0)
				{
					return false;
				}
				if (r < t1)
				{
					t1 = r;
				}
			}
		}
		c0.X = x0 + t0 * dx;
		c0.Y = y0 + t0 * dy;
		c1.X = x0 + t1 * dx;
		c1.Y = y0 + t1 * dy;
		return true;
	}

	public static bool BoxRayIntersection(Rectangle rect, Point p0, Point p1, ref Point c1)
	{
		double x = p0.X;
		double y = p0.Y;
		double dx = p1.x - x;
		double dy = p1.y - y;
		double xmin = rect.Left;
		double xmax = rect.Right;
		double ymin = rect.Bottom;
		double ymax = rect.Top;
		if (x < xmin || x > xmax || y < ymin || y > ymax)
		{
			return false;
		}
		double t1;
		double x2;
		double y2;
		if (dx < 0.0)
		{
			t1 = (xmin - x) / dx;
			x2 = xmin;
			y2 = y + t1 * dy;
		}
		else if (dx > 0.0)
		{
			t1 = (xmax - x) / dx;
			x2 = xmax;
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
			t2 = (ymin - y) / dy;
			x3 = x + t2 * dx;
			y3 = ymin;
		}
		else if (dy > 0.0)
		{
			t2 = (ymax - y) / dy;
			x3 = x + t2 * dx;
			y3 = ymax;
		}
		else
		{
			t2 = double.MaxValue;
			x3 = (y3 = 0.0);
		}
		if (t1 < t2)
		{
			c1.x = x2;
			c1.y = y2;
		}
		else
		{
			c1.x = x3;
			c1.y = y3;
		}
		return true;
	}
}
