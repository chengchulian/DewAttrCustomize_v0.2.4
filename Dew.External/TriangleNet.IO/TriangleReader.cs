using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TriangleNet.Geometry;

namespace TriangleNet.IO;

public class TriangleReader
{
	private static NumberFormatInfo nfi = NumberFormatInfo.InvariantInfo;

	private int startIndex;

	public static bool IsNullOrWhiteSpace(string value)
	{
		if (value == null)
		{
			return true;
		}
		return string.IsNullOrEmpty(value.Trim());
	}

	private bool TryReadLine(StreamReader reader, out string[] token)
	{
		token = null;
		if (reader.EndOfStream)
		{
			return false;
		}
		string line = reader.ReadLine().Trim();
		while (IsNullOrWhiteSpace(line) || line.StartsWith("#"))
		{
			if (reader.EndOfStream)
			{
				return false;
			}
			line = reader.ReadLine().Trim();
		}
		token = line.Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
		return true;
	}

	private void ReadVertex(List<Vertex> data, int index, string[] line, int attributes, int marks)
	{
		double x = double.Parse(line[1], nfi);
		double y = double.Parse(line[2], nfi);
		Vertex v = new Vertex(x, y);
		if (marks > 0 && line.Length > 3 + attributes)
		{
			v.Label = int.Parse(line[3 + attributes]);
		}
		_ = 0;
		data.Add(v);
	}

	public void Read(string filename, out Polygon polygon)
	{
		polygon = null;
		string path = Path.ChangeExtension(filename, ".poly");
		if (File.Exists(path))
		{
			polygon = ReadPolyFile(path);
			return;
		}
		path = Path.ChangeExtension(filename, ".node");
		polygon = ReadNodeFile(path);
	}

	public void Read(string filename, out Polygon geometry, out List<ITriangle> triangles)
	{
		triangles = null;
		Read(filename, out geometry);
		string path = Path.ChangeExtension(filename, ".ele");
		if (File.Exists(path) && geometry != null)
		{
			triangles = ReadEleFile(path);
		}
	}

	public IPolygon Read(string filename)
	{
		Polygon geometry = null;
		Read(filename, out geometry);
		return geometry;
	}

	public Polygon ReadNodeFile(string nodefilename)
	{
		return ReadNodeFile(nodefilename, readElements: false);
	}

	public Polygon ReadNodeFile(string nodefilename, bool readElements)
	{
		startIndex = 0;
		int invertices = 0;
		int attributes = 0;
		int nodemarkers = 0;
		Polygon data;
		using (StreamReader reader = new StreamReader(nodefilename))
		{
			if (!TryReadLine(reader, out var line))
			{
				throw new Exception("Can't read input file.");
			}
			invertices = int.Parse(line[0]);
			if (invertices < 3)
			{
				throw new Exception("Input must have at least three input vertices.");
			}
			if (line.Length > 1 && int.Parse(line[1]) != 2)
			{
				throw new Exception("Triangle only works with two-dimensional meshes.");
			}
			if (line.Length > 2)
			{
				attributes = int.Parse(line[2]);
			}
			if (line.Length > 3)
			{
				nodemarkers = int.Parse(line[3]);
			}
			data = new Polygon(invertices);
			if (invertices > 0)
			{
				for (int i = 0; i < invertices; i++)
				{
					if (!TryReadLine(reader, out line))
					{
						throw new Exception("Can't read input file (vertices).");
					}
					if (line.Length < 3)
					{
						throw new Exception("Invalid vertex.");
					}
					if (i == 0)
					{
						startIndex = int.Parse(line[0], nfi);
					}
					ReadVertex(data.Points, i, line, attributes, nodemarkers);
				}
			}
		}
		if (readElements)
		{
			string elefile = Path.ChangeExtension(nodefilename, ".ele");
			if (File.Exists(elefile))
			{
				ReadEleFile(elefile, readArea: true);
			}
		}
		return data;
	}

	public Polygon ReadPolyFile(string polyfilename)
	{
		return ReadPolyFile(polyfilename, readElements: false, readArea: false);
	}

	public Polygon ReadPolyFile(string polyfilename, bool readElements)
	{
		return ReadPolyFile(polyfilename, readElements, readArea: false);
	}

	public Polygon ReadPolyFile(string polyfilename, bool readElements, bool readArea)
	{
		startIndex = 0;
		int invertices = 0;
		int attributes = 0;
		int nodemarkers = 0;
		Polygon data;
		using (StreamReader reader = new StreamReader(polyfilename))
		{
			if (!TryReadLine(reader, out var line))
			{
				throw new Exception("Can't read input file.");
			}
			invertices = int.Parse(line[0]);
			if (line.Length > 1 && int.Parse(line[1]) != 2)
			{
				throw new Exception("Triangle only works with two-dimensional meshes.");
			}
			if (line.Length > 2)
			{
				attributes = int.Parse(line[2]);
			}
			if (line.Length > 3)
			{
				nodemarkers = int.Parse(line[3]);
			}
			if (invertices > 0)
			{
				data = new Polygon(invertices);
				for (int i = 0; i < invertices; i++)
				{
					if (!TryReadLine(reader, out line))
					{
						throw new Exception("Can't read input file (vertices).");
					}
					if (line.Length < 3)
					{
						throw new Exception("Invalid vertex.");
					}
					if (i == 0)
					{
						startIndex = int.Parse(line[0], nfi);
					}
					ReadVertex(data.Points, i, line, attributes, nodemarkers);
				}
			}
			else
			{
				data = ReadNodeFile(Path.ChangeExtension(polyfilename, ".node"));
				invertices = data.Points.Count;
			}
			List<Vertex> points = data.Points;
			if (points.Count == 0)
			{
				throw new Exception("No nodes available.");
			}
			if (!TryReadLine(reader, out line))
			{
				throw new Exception("Can't read input file (segments).");
			}
			int insegments = int.Parse(line[0]);
			int segmentmarkers = 0;
			if (line.Length > 1)
			{
				segmentmarkers = int.Parse(line[1]);
			}
			for (int j = 0; j < insegments; j++)
			{
				if (!TryReadLine(reader, out line))
				{
					throw new Exception("Can't read input file (segments).");
				}
				if (line.Length < 3)
				{
					throw new Exception("Segment has no endpoints.");
				}
				int end1 = int.Parse(line[1]) - startIndex;
				int end2 = int.Parse(line[2]) - startIndex;
				int mark = 0;
				if (segmentmarkers > 0 && line.Length > 3)
				{
					mark = int.Parse(line[3]);
				}
				if (end1 < 0 || end1 >= invertices)
				{
					if (Log.Verbose)
					{
						Log.Instance.Warning("Invalid first endpoint of segment.", "MeshReader.ReadPolyfile()");
					}
				}
				else if (end2 < 0 || end2 >= invertices)
				{
					if (Log.Verbose)
					{
						Log.Instance.Warning("Invalid second endpoint of segment.", "MeshReader.ReadPolyfile()");
					}
				}
				else
				{
					data.Add(new Segment(points[end1], points[end2], mark));
				}
			}
			if (!TryReadLine(reader, out line))
			{
				throw new Exception("Can't read input file (holes).");
			}
			int holes = int.Parse(line[0]);
			if (holes > 0)
			{
				for (int k = 0; k < holes; k++)
				{
					if (!TryReadLine(reader, out line))
					{
						throw new Exception("Can't read input file (holes).");
					}
					if (line.Length < 3)
					{
						throw new Exception("Invalid hole.");
					}
					data.Holes.Add(new Point(double.Parse(line[1], nfi), double.Parse(line[2], nfi)));
				}
			}
			if (TryReadLine(reader, out line))
			{
				int regions = int.Parse(line[0]);
				if (regions > 0)
				{
					for (int l = 0; l < regions; l++)
					{
						if (!TryReadLine(reader, out line))
						{
							throw new Exception("Can't read input file (region).");
						}
						if (line.Length < 4)
						{
							throw new Exception("Invalid region attributes.");
						}
						if (!int.TryParse(line[3], out var id))
						{
							id = l;
						}
						double area = 0.0;
						if (line.Length > 4)
						{
							double.TryParse(line[4], NumberStyles.Number, nfi, out area);
						}
						data.Regions.Add(new RegionPointer(double.Parse(line[1], nfi), double.Parse(line[2], nfi), id, area));
					}
				}
			}
		}
		if (readElements)
		{
			string elefile = Path.ChangeExtension(polyfilename, ".ele");
			if (File.Exists(elefile))
			{
				ReadEleFile(elefile, readArea);
			}
		}
		return data;
	}

	public List<ITriangle> ReadEleFile(string elefilename)
	{
		return ReadEleFile(elefilename, readArea: false);
	}

	private List<ITriangle> ReadEleFile(string elefilename, bool readArea)
	{
		int intriangles = 0;
		int attributes = 0;
		List<ITriangle> triangles;
		using (StreamReader reader = new StreamReader(elefilename))
		{
			bool validRegion = false;
			if (!TryReadLine(reader, out var line))
			{
				throw new Exception("Can't read input file (elements).");
			}
			intriangles = int.Parse(line[0]);
			attributes = 0;
			if (line.Length > 2)
			{
				attributes = int.Parse(line[2]);
				validRegion = true;
			}
			if (attributes > 1)
			{
				Log.Instance.Warning("Triangle attributes not supported.", "FileReader.Read");
			}
			triangles = new List<ITriangle>(intriangles);
			for (int i = 0; i < intriangles; i++)
			{
				if (!TryReadLine(reader, out line))
				{
					throw new Exception("Can't read input file (elements).");
				}
				if (line.Length < 4)
				{
					throw new Exception("Triangle has no nodes.");
				}
				InputTriangle tri = new InputTriangle(int.Parse(line[1]) - startIndex, int.Parse(line[2]) - startIndex, int.Parse(line[3]) - startIndex);
				if (attributes > 0 && validRegion)
				{
					int region = 0;
					validRegion = int.TryParse(line[4], out region);
					tri.label = region;
				}
				triangles.Add(tri);
			}
		}
		if (readArea)
		{
			string areafile = Path.ChangeExtension(elefilename, ".area");
			if (File.Exists(areafile))
			{
				ReadAreaFile(areafile, intriangles);
			}
		}
		return triangles;
	}

	private double[] ReadAreaFile(string areafilename, int intriangles)
	{
		double[] data = null;
		using StreamReader reader = new StreamReader(areafilename);
		if (!TryReadLine(reader, out var line))
		{
			throw new Exception("Can't read input file (area).");
		}
		if (int.Parse(line[0]) != intriangles)
		{
			Log.Instance.Warning("Number of area constraints doesn't match number of triangles.", "ReadAreaFile()");
			return null;
		}
		data = new double[intriangles];
		for (int i = 0; i < intriangles; i++)
		{
			if (!TryReadLine(reader, out line))
			{
				throw new Exception("Can't read input file (area).");
			}
			if (line.Length != 2)
			{
				throw new Exception("Triangle has no nodes.");
			}
			data[i] = double.Parse(line[1], nfi);
		}
		return data;
	}

	public List<Edge> ReadEdgeFile(string edgeFile, int invertices)
	{
		List<Edge> data = null;
		startIndex = 0;
		using StreamReader reader = new StreamReader(edgeFile);
		if (!TryReadLine(reader, out var line))
		{
			throw new Exception("Can't read input file (segments).");
		}
		int inedges = int.Parse(line[0]);
		int edgemarkers = 0;
		if (line.Length > 1)
		{
			edgemarkers = int.Parse(line[1]);
		}
		if (inedges > 0)
		{
			data = new List<Edge>(inedges);
		}
		for (int i = 0; i < inedges; i++)
		{
			if (!TryReadLine(reader, out line))
			{
				throw new Exception("Can't read input file (segments).");
			}
			if (line.Length < 3)
			{
				throw new Exception("Segment has no endpoints.");
			}
			int end1 = int.Parse(line[1]) - startIndex;
			int end2 = int.Parse(line[2]) - startIndex;
			int mark = 0;
			if (edgemarkers > 0 && line.Length > 3)
			{
				mark = int.Parse(line[3]);
			}
			if (end1 < 0 || end1 >= invertices)
			{
				if (Log.Verbose)
				{
					Log.Instance.Warning("Invalid first endpoint of segment.", "MeshReader.ReadPolyfile()");
				}
			}
			else if (end2 < 0 || end2 >= invertices)
			{
				if (Log.Verbose)
				{
					Log.Instance.Warning("Invalid second endpoint of segment.", "MeshReader.ReadPolyfile()");
				}
			}
			else
			{
				data.Add(new Edge(end1, end2, mark));
			}
		}
		return data;
	}
}
