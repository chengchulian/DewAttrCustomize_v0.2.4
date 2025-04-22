using System;
using System.IO;
using TriangleNet.Geometry;
using TriangleNet.Meshing;

namespace TriangleNet.IO;

public class TriangleFormat : IPolygonFormat, IFileFormat, IMeshFormat
{
	public bool IsSupported(string file)
	{
		switch (Path.GetExtension(file).ToLower())
		{
		case ".node":
		case ".poly":
		case ".ele":
			return true;
		default:
			return false;
		}
	}

	public IMesh Import(string filename)
	{
		switch (Path.GetExtension(filename))
		{
		case ".node":
		case ".poly":
		case ".ele":
		{
			new TriangleReader().Read(filename, out var geometry, out var triangles);
			if (geometry != null && triangles != null)
			{
				return Converter.ToMesh(geometry, triangles.ToArray());
			}
			break;
		}
		}
		throw new NotSupportedException("Could not load '" + filename + "' file.");
	}

	public void Write(IMesh mesh, string filename)
	{
		TriangleWriter triangleWriter = new TriangleWriter();
		triangleWriter.WritePoly((Mesh)mesh, Path.ChangeExtension(filename, ".poly"));
		triangleWriter.WriteElements((Mesh)mesh, Path.ChangeExtension(filename, ".ele"));
	}

	public void Write(IMesh mesh, Stream stream)
	{
		throw new NotImplementedException();
	}

	public IPolygon Read(string filename)
	{
		string ext = Path.GetExtension(filename);
		if (ext == ".node")
		{
			return new TriangleReader().ReadNodeFile(filename);
		}
		if (ext == ".poly")
		{
			return new TriangleReader().ReadPolyFile(filename);
		}
		throw new NotSupportedException("File format '" + ext + "' not supported.");
	}

	public void Write(IPolygon polygon, string filename)
	{
		new TriangleWriter().WritePoly(polygon, filename);
	}

	public void Write(IPolygon polygon, Stream stream)
	{
		throw new NotImplementedException();
	}
}
