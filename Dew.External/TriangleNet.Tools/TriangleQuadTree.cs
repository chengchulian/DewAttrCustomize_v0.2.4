using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;

namespace TriangleNet.Tools;

public class TriangleQuadTree
{
	private class QuadNode
	{
		private const int SW = 0;

		private const int SE = 1;

		private const int NW = 2;

		private const int NE = 3;

		private const double EPS = 1E-06;

		private static readonly byte[] BITVECTOR = new byte[4] { 1, 2, 4, 8 };

		private Rectangle bounds;

		private Point pivot;

		private TriangleQuadTree tree;

		private QuadNode[] regions;

		private List<int> triangles;

		private byte bitRegions;

		public QuadNode(Rectangle box, TriangleQuadTree tree)
			: this(box, tree, init: false)
		{
		}

		public QuadNode(Rectangle box, TriangleQuadTree tree, bool init)
		{
			this.tree = tree;
			bounds = new Rectangle(box.Left, box.Bottom, box.Width, box.Height);
			pivot = new Point((box.Left + box.Right) / 2.0, (box.Bottom + box.Top) / 2.0);
			bitRegions = 0;
			regions = new QuadNode[4];
			triangles = new List<int>();
			if (init)
			{
				int count = tree.triangles.Length;
				triangles.Capacity = count;
				for (int i = 0; i < count; i++)
				{
					triangles.Add(i);
				}
			}
		}

		public List<int> FindTriangles(Point searchPoint)
		{
			int region = FindRegion(searchPoint);
			if (regions[region] == null)
			{
				return triangles;
			}
			return regions[region].FindTriangles(searchPoint);
		}

		public void CreateSubRegion(int currentDepth)
		{
			double width = bounds.Right - pivot.x;
			double height = bounds.Top - pivot.y;
			Rectangle box = new Rectangle(bounds.Left, bounds.Bottom, width, height);
			regions[0] = new QuadNode(box, tree);
			box = new Rectangle(pivot.x, bounds.Bottom, width, height);
			regions[1] = new QuadNode(box, tree);
			box = new Rectangle(bounds.Left, pivot.y, width, height);
			regions[2] = new QuadNode(box, tree);
			box = new Rectangle(pivot.x, pivot.y, width, height);
			regions[3] = new QuadNode(box, tree);
			Point[] triangle = new Point[3];
			foreach (int index in triangles)
			{
				ITriangle tri = tree.triangles[index];
				triangle[0] = tri.GetVertex(0);
				triangle[1] = tri.GetVertex(1);
				triangle[2] = tri.GetVertex(2);
				AddTriangleToRegion(triangle, index);
			}
			for (int i = 0; i < 4; i++)
			{
				if (regions[i].triangles.Count > tree.sizeBound && currentDepth < tree.maxDepth)
				{
					regions[i].CreateSubRegion(currentDepth + 1);
				}
			}
		}

		private void AddTriangleToRegion(Point[] triangle, int index)
		{
			bitRegions = 0;
			if (IsPointInTriangle(pivot, triangle[0], triangle[1], triangle[2]))
			{
				AddToRegion(index, 0);
				AddToRegion(index, 1);
				AddToRegion(index, 2);
				AddToRegion(index, 3);
				return;
			}
			FindTriangleIntersections(triangle, index);
			if (bitRegions == 0)
			{
				int region = FindRegion(triangle[0]);
				regions[region].triangles.Add(index);
			}
		}

		private void FindTriangleIntersections(Point[] triangle, int index)
		{
			int k = 2;
			int i = 0;
			while (i < 3)
			{
				double dx = triangle[i].x - triangle[k].x;
				double dy = triangle[i].y - triangle[k].y;
				if (dx != 0.0)
				{
					FindIntersectionsWithX(dx, dy, triangle, index, k);
				}
				if (dy != 0.0)
				{
					FindIntersectionsWithY(dx, dy, triangle, index, k);
				}
				k = i++;
			}
		}

		private void FindIntersectionsWithX(double dx, double dy, Point[] triangle, int index, int k)
		{
			double t = (pivot.x - triangle[k].x) / dx;
			if (t < 1.000001 && t > -1E-06)
			{
				double yComponent = triangle[k].y + t * dy;
				if (yComponent < pivot.y && yComponent >= bounds.Bottom)
				{
					AddToRegion(index, 0);
					AddToRegion(index, 1);
				}
				else if (yComponent <= bounds.Top)
				{
					AddToRegion(index, 2);
					AddToRegion(index, 3);
				}
			}
			t = (bounds.Left - triangle[k].x) / dx;
			if (t < 1.000001 && t > -1E-06)
			{
				double yComponent2 = triangle[k].y + t * dy;
				if (yComponent2 < pivot.y && yComponent2 >= bounds.Bottom)
				{
					AddToRegion(index, 0);
				}
				else if (yComponent2 <= bounds.Top)
				{
					AddToRegion(index, 2);
				}
			}
			t = (bounds.Right - triangle[k].x) / dx;
			if (t < 1.000001 && t > -1E-06)
			{
				double yComponent3 = triangle[k].y + t * dy;
				if (yComponent3 < pivot.y && yComponent3 >= bounds.Bottom)
				{
					AddToRegion(index, 1);
				}
				else if (yComponent3 <= bounds.Top)
				{
					AddToRegion(index, 3);
				}
			}
		}

		private void FindIntersectionsWithY(double dx, double dy, Point[] triangle, int index, int k)
		{
			double t = (pivot.y - triangle[k].y) / dy;
			if (t < 1.000001 && t > -1E-06)
			{
				double xComponent = triangle[k].x + t * dx;
				if (xComponent > pivot.x && xComponent <= bounds.Right)
				{
					AddToRegion(index, 1);
					AddToRegion(index, 3);
				}
				else if (xComponent >= bounds.Left)
				{
					AddToRegion(index, 0);
					AddToRegion(index, 2);
				}
			}
			t = (bounds.Bottom - triangle[k].y) / dy;
			if (t < 1.000001 && t > -1E-06)
			{
				double xComponent = triangle[k].x + t * dx;
				if (xComponent > pivot.x && xComponent <= bounds.Right)
				{
					AddToRegion(index, 1);
				}
				else if (xComponent >= bounds.Left)
				{
					AddToRegion(index, 0);
				}
			}
			t = (bounds.Top - triangle[k].y) / dy;
			if (t < 1.000001 && t > -1E-06)
			{
				double xComponent = triangle[k].x + t * dx;
				if (xComponent > pivot.x && xComponent <= bounds.Right)
				{
					AddToRegion(index, 3);
				}
				else if (xComponent >= bounds.Left)
				{
					AddToRegion(index, 2);
				}
			}
		}

		private int FindRegion(Point point)
		{
			int b = 2;
			if (point.y < pivot.y)
			{
				b = 0;
			}
			if (point.x > pivot.x)
			{
				b++;
			}
			return b;
		}

		private void AddToRegion(int index, int region)
		{
			if ((bitRegions & BITVECTOR[region]) == 0)
			{
				regions[region].triangles.Add(index);
				bitRegions |= BITVECTOR[region];
			}
		}
	}

	private QuadNode root;

	internal ITriangle[] triangles;

	internal int sizeBound;

	internal int maxDepth;

	public TriangleQuadTree(Mesh mesh, int maxDepth = 10, int sizeBound = 10)
	{
		this.maxDepth = maxDepth;
		this.sizeBound = sizeBound;
		ITriangle[] array = mesh.Triangles.ToArray();
		triangles = array;
		int currentDepth = 0;
		root = new QuadNode(mesh.Bounds, this, init: true);
		root.CreateSubRegion(++currentDepth);
	}

	public ITriangle Query(double x, double y)
	{
		Point point = new Point(x, y);
		foreach (int i in root.FindTriangles(point))
		{
			ITriangle tri = triangles[i];
			if (IsPointInTriangle(point, tri.GetVertex(0), tri.GetVertex(1), tri.GetVertex(2)))
			{
				return tri;
			}
		}
		return null;
	}

	internal static bool IsPointInTriangle(Point p, Point t0, Point t1, Point t2)
	{
		Point d0 = new Point(t1.x - t0.x, t1.y - t0.y);
		Point d1 = new Point(t2.x - t0.x, t2.y - t0.y);
		Point p2 = new Point(p.x - t0.x, p.y - t0.y);
		Point c0 = new Point(0.0 - d0.y, d0.x);
		Point c1 = new Point(0.0 - d1.y, d1.x);
		double s = DotProduct(p2, c1) / DotProduct(d0, c1);
		double v = DotProduct(p2, c0) / DotProduct(d1, c0);
		if (s >= 0.0 && v >= 0.0 && s + v <= 1.0)
		{
			return true;
		}
		return false;
	}

	internal static double DotProduct(Point p, Point q)
	{
		return p.x * q.x + p.y * q.y;
	}
}
