using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.IO;
using TriangleNet.Meshing.Algorithm;

namespace TriangleNet.Meshing;

public class GenericMesher
{
	private Configuration config;

	private ITriangulator triangulator;

	public GenericMesher()
		: this(new Dwyer(), new Configuration())
	{
	}

	public GenericMesher(ITriangulator triangulator)
		: this(triangulator, new Configuration())
	{
	}

	public GenericMesher(Configuration config)
		: this(new Dwyer(), config)
	{
	}

	public GenericMesher(ITriangulator triangulator, Configuration config)
	{
		this.config = config;
		this.triangulator = triangulator;
	}

	public IMesh Triangulate(IList<Vertex> points)
	{
		return triangulator.Triangulate(points, config);
	}

	public IMesh Triangulate(IPolygon polygon)
	{
		return Triangulate(polygon, null, null);
	}

	public IMesh Triangulate(IPolygon polygon, ConstraintOptions options)
	{
		return Triangulate(polygon, options, null);
	}

	public IMesh Triangulate(IPolygon polygon, QualityOptions quality)
	{
		return Triangulate(polygon, null, quality);
	}

	public IMesh Triangulate(IPolygon polygon, ConstraintOptions options, QualityOptions quality)
	{
		Mesh obj = (Mesh)triangulator.Triangulate(polygon.Points, config);
		ConstraintMesher cmesher = new ConstraintMesher(obj, config);
		QualityMesher qmesher = new QualityMesher(obj, config);
		obj.SetQualityMesher(qmesher);
		cmesher.Apply(polygon, options);
		qmesher.Apply(quality);
		return obj;
	}

	public static IMesh StructuredMesh(double width, double height, int nx, int ny)
	{
		if (width <= 0.0)
		{
			throw new ArgumentException("width");
		}
		if (height <= 0.0)
		{
			throw new ArgumentException("height");
		}
		return StructuredMesh(new Rectangle(0.0, 0.0, width, height), nx, ny);
	}

	public static IMesh StructuredMesh(Rectangle bounds, int nx, int ny)
	{
		Polygon polygon = new Polygon((nx + 1) * (ny + 1));
		double dx = bounds.Width / (double)nx;
		double dy = bounds.Height / (double)ny;
		double left = bounds.Left;
		double bottom = bounds.Bottom;
		int n = 0;
		Vertex[] points = new Vertex[(nx + 1) * (ny + 1)];
		for (int i = 0; i <= nx; i++)
		{
			double x = left + (double)i * dx;
			for (int j = 0; j <= ny; j++)
			{
				double y = bottom + (double)j * dy;
				points[n++] = new Vertex(x, y);
			}
		}
		polygon.Points.AddRange(points);
		n = 0;
		Vertex[] array = points;
		foreach (Vertex obj in array)
		{
			obj.hash = (obj.id = n++);
		}
		List<ISegment> segments = polygon.Segments;
		segments.Capacity = 2 * (nx + ny);
		for (int j = 0; j < ny; j++)
		{
			Vertex a = points[j];
			Vertex b = points[j + 1];
			segments.Add(new Segment(a, b, 1));
			Vertex vertex = a;
			int k = (b.Label = 1);
			vertex.Label = k;
			a = points[nx * (ny + 1) + j];
			b = points[nx * (ny + 1) + (j + 1)];
			segments.Add(new Segment(a, b, 1));
			Vertex vertex2 = a;
			k = (b.Label = 1);
			vertex2.Label = k;
		}
		for (int i = 0; i < nx; i++)
		{
			Vertex a = points[(ny + 1) * i];
			Vertex b = points[(ny + 1) * (i + 1)];
			segments.Add(new Segment(a, b, 1));
			Vertex vertex3 = a;
			int k = (b.Label = 1);
			vertex3.Label = k;
			a = points[ny + (ny + 1) * i];
			b = points[ny + (ny + 1) * (i + 1)];
			segments.Add(new Segment(a, b, 1));
			Vertex vertex4 = a;
			k = (b.Label = 1);
			vertex4.Label = k;
		}
		InputTriangle[] triangles = new InputTriangle[2 * nx * ny];
		n = 0;
		for (int i = 0; i < nx; i++)
		{
			for (int j = 0; j < ny; j++)
			{
				int k2 = j + (ny + 1) * i;
				int l = j + (ny + 1) * (i + 1);
				if ((i + j) % 2 == 0)
				{
					triangles[n++] = new InputTriangle(k2, l, l + 1);
					triangles[n++] = new InputTriangle(k2, l + 1, k2 + 1);
				}
				else
				{
					triangles[n++] = new InputTriangle(k2, l, k2 + 1);
					triangles[n++] = new InputTriangle(l, l + 1, k2 + 1);
				}
			}
		}
		ITriangle[] triangles2 = triangles;
		return Converter.ToMesh(polygon, triangles2);
	}
}
