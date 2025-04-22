using System;
using System.Collections.Generic;

namespace TriangleNet.Geometry;

public class Contour
{
	private int marker;

	private bool convex;

	public List<Vertex> Points { get; set; }

	public Contour(IEnumerable<Vertex> points)
		: this(points, 0, convex: false)
	{
	}

	public Contour(IEnumerable<Vertex> points, int marker)
		: this(points, marker, convex: false)
	{
	}

	public Contour(IEnumerable<Vertex> points, int marker, bool convex)
	{
		AddPoints(points);
		this.marker = marker;
		this.convex = convex;
	}

	public List<ISegment> GetSegments()
	{
		List<ISegment> segments = new List<ISegment>();
		List<Vertex> p = Points;
		int count = p.Count - 1;
		for (int i = 0; i < count; i++)
		{
			segments.Add(new Segment(p[i], p[i + 1], marker));
		}
		segments.Add(new Segment(p[count], p[0], marker));
		return segments;
	}

	public Point FindInteriorPoint(int limit = 5, double eps = 2E-05)
	{
		if (convex)
		{
			int count = Points.Count;
			Point point = new Point(0.0, 0.0);
			for (int i = 0; i < count; i++)
			{
				point.x += Points[i].x;
				point.y += Points[i].y;
			}
			point.x /= count;
			point.y /= count;
			return point;
		}
		return FindPointInPolygon(Points, limit, eps);
	}

	private void AddPoints(IEnumerable<Vertex> points)
	{
		Points = new List<Vertex>(points);
		int count = Points.Count - 1;
		if (Points[0] == Points[count])
		{
			Points.RemoveAt(count);
		}
	}

	private static Point FindPointInPolygon(List<Vertex> contour, int limit, double eps)
	{
		List<Point> points = contour.ConvertAll((Converter<Vertex, Point>)((Vertex x) => x));
		Rectangle bounds = new Rectangle();
		bounds.Expand(points);
		int length = contour.Count;
		Point test = new Point();
		RobustPredicates predicates = new RobustPredicates();
		Point a = contour[0];
		Point b = contour[1];
		for (int i = 0; i < length; i++)
		{
			Point c = contour[(i + 2) % length];
			double bx = b.x;
			double by = b.y;
			double h = predicates.CounterClockwise(a, b, c);
			double dx;
			double dy;
			if (Math.Abs(h) < eps)
			{
				dx = (c.y - a.y) / 2.0;
				dy = (a.x - c.x) / 2.0;
			}
			else
			{
				dx = (a.x + c.x) / 2.0 - bx;
				dy = (a.y + c.y) / 2.0 - by;
			}
			a = b;
			b = c;
			h = 1.0;
			for (int j = 0; j < limit; j++)
			{
				test.x = bx + dx * h;
				test.y = by + dy * h;
				if (bounds.Contains(test) && IsPointInPolygon(test, contour))
				{
					return test;
				}
				test.x = bx - dx * h;
				test.y = by - dy * h;
				if (bounds.Contains(test) && IsPointInPolygon(test, contour))
				{
					return test;
				}
				h /= 2.0;
			}
		}
		throw new Exception();
	}

	private static bool IsPointInPolygon(Point point, List<Vertex> poly)
	{
		bool inside = false;
		double x = point.x;
		double y = point.y;
		int count = poly.Count;
		int i = 0;
		int j = count - 1;
		for (; i < count; i++)
		{
			if (((poly[i].y < y && poly[j].y >= y) || (poly[j].y < y && poly[i].y >= y)) && (poly[i].x <= x || poly[j].x <= x))
			{
				inside ^= poly[i].x + (y - poly[i].y) / (poly[j].y - poly[i].y) * (poly[j].x - poly[i].x) < x;
			}
			j = i;
		}
		return inside;
	}
}
