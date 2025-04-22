using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Logging;

namespace TriangleNet.Tools;

public static class PolygonValidator
{
	public static bool IsConsistent(IPolygon poly)
	{
		ILog<LogItem> logger = Log.Instance;
		List<Vertex> points = poly.Points;
		int horrors = 0;
		int i = 0;
		int count = points.Count;
		if (count < 3)
		{
			logger.Warning("Polygon must have at least 3 vertices.", "PolygonValidator.IsConsistent()");
			return false;
		}
		foreach (Vertex p in points)
		{
			if (p == null)
			{
				horrors++;
				logger.Warning($"Point {i} is null.", "PolygonValidator.IsConsistent()");
			}
			else if (double.IsNaN(p.x) || double.IsNaN(p.y))
			{
				horrors++;
				logger.Warning($"Point {i} has invalid coordinates.", "PolygonValidator.IsConsistent()");
			}
			else if (double.IsInfinity(p.x) || double.IsInfinity(p.y))
			{
				horrors++;
				logger.Warning($"Point {i} has invalid coordinates.", "PolygonValidator.IsConsistent()");
			}
			i++;
		}
		i = 0;
		foreach (ISegment seg in poly.Segments)
		{
			if (seg == null)
			{
				horrors++;
				logger.Warning($"Segment {i} is null.", "PolygonValidator.IsConsistent()");
				return false;
			}
			Vertex p2 = seg.GetVertex(0);
			Vertex q = seg.GetVertex(1);
			if (p2.x == q.x && p2.y == q.y)
			{
				horrors++;
				logger.Warning($"Endpoints of segment {i} are coincident (IDs {p2.id} / {q.id}).", "PolygonValidator.IsConsistent()");
			}
			i++;
		}
		horrors = ((points[0].id != points[1].id) ? (horrors + CheckDuplicateIDs(poly)) : (horrors + CheckVertexIDs(poly, count)));
		return horrors == 0;
	}

	public static bool HasDuplicateVertices(IPolygon poly)
	{
		ILog<LogItem> logger = Log.Instance;
		int horrors = 0;
		Vertex[] points = poly.Points.ToArray();
		VertexSorter.Sort(points);
		for (int i = 1; i < points.Length; i++)
		{
			if (points[i - 1] == points[i])
			{
				horrors++;
				logger.Warning($"Found duplicate point {points[i]}.", "PolygonValidator.HasDuplicateVertices()");
			}
		}
		return horrors > 0;
	}

	public static bool HasBadAngles(IPolygon poly, double threshold = 2E-12)
	{
		ILog<LogItem> logger = Log.Instance;
		int horrors = 0;
		int i = 0;
		Point p0 = null;
		Point p1 = null;
		_ = poly.Points.Count;
		foreach (ISegment segment in poly.Segments)
		{
			Point q0 = p0;
			Point q1 = p1;
			p0 = segment.GetVertex(0);
			p1 = segment.GetVertex(1);
			if (!(p0 == p1) && !(q0 == q1))
			{
				if (q0 != null && q1 != null && p0 == q1 && p1 != null && IsBadAngle(q0, p0, p1, threshold))
				{
					horrors++;
					logger.Warning($"Bad segment angle found at index {i}.", "PolygonValidator.HasBadAngles()");
				}
				i++;
			}
		}
		return horrors > 0;
	}

	private static bool IsBadAngle(Point a, Point b, Point c, double threshold = 0.0)
	{
		double x = DotProduct(a, b, c);
		return Math.Abs(Math.Atan2(CrossProductLength(a, b, c), x)) <= threshold;
	}

	private static double DotProduct(Point a, Point b, Point c)
	{
		return (a.x - b.x) * (c.x - b.x) + (a.y - b.y) * (c.y - b.y);
	}

	private static double CrossProductLength(Point a, Point b, Point c)
	{
		return (a.x - b.x) * (c.y - b.y) - (a.y - b.y) * (c.x - b.x);
	}

	private static int CheckVertexIDs(IPolygon poly, int count)
	{
		ILog<LogItem> logger = Log.Instance;
		int horrors = 0;
		int i = 0;
		foreach (ISegment segment in poly.Segments)
		{
			Vertex p = segment.GetVertex(0);
			Vertex q = segment.GetVertex(1);
			if (p.id < 0 || p.id >= count)
			{
				horrors++;
				logger.Warning($"Segment {i} has invalid startpoint.", "PolygonValidator.IsConsistent()");
			}
			if (q.id < 0 || q.id >= count)
			{
				horrors++;
				logger.Warning($"Segment {i} has invalid endpoint.", "PolygonValidator.IsConsistent()");
			}
			i++;
		}
		return horrors;
	}

	private static int CheckDuplicateIDs(IPolygon poly)
	{
		HashSet<int> ids = new HashSet<int>();
		foreach (Vertex p in poly.Points)
		{
			if (!ids.Add(p.id))
			{
				Log.Instance.Warning("Found duplicate vertex ids.", "PolygonValidator.IsConsistent()");
				return 1;
			}
		}
		return 0;
	}
}
