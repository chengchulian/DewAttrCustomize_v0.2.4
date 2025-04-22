using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.IO;

internal class DebugWriter
{
	private static NumberFormatInfo nfi;

	private int iteration;

	private string session;

	private StreamWriter stream;

	private string tmpFile;

	private int[] vertices;

	private int triangles;

	private static readonly DebugWriter instance;

	public static DebugWriter Session => instance;

	static DebugWriter()
	{
		nfi = CultureInfo.InvariantCulture.NumberFormat;
		instance = new DebugWriter();
	}

	private DebugWriter()
	{
	}

	public void Start(string session)
	{
		iteration = 0;
		this.session = session;
		if (stream != null)
		{
			throw new Exception("A session is active. Finish before starting a new.");
		}
		tmpFile = Path.GetTempFileName();
		stream = new StreamWriter(tmpFile);
	}

	public void Write(Mesh mesh, bool skip = false)
	{
		WriteMesh(mesh, skip);
		triangles = mesh.Triangles.Count;
	}

	public void Finish()
	{
		Finish(session + ".mshx");
	}

	private void Finish(string path)
	{
		if (stream == null)
		{
			return;
		}
		stream.Flush();
		stream.Dispose();
		stream = null;
		string header = "#!N" + iteration + Environment.NewLine;
		using (FileStream gzFile = new FileStream(path, FileMode.Create))
		{
			using GZipStream gzStream = new GZipStream(gzFile, CompressionMode.Compress, leaveOpen: false);
			byte[] bytes = Encoding.UTF8.GetBytes(header);
			gzStream.Write(bytes, 0, bytes.Length);
			bytes = File.ReadAllBytes(tmpFile);
			gzStream.Write(bytes, 0, bytes.Length);
		}
		File.Delete(tmpFile);
	}

	private void WriteGeometry(IPolygon geometry)
	{
		stream.WriteLine("#!G{0}", iteration++);
	}

	private void WriteMesh(Mesh mesh, bool skip)
	{
		if (triangles == mesh.triangles.Count && skip)
		{
			return;
		}
		stream.WriteLine("#!M{0}", iteration++);
		if (VerticesChanged(mesh))
		{
			HashVertices(mesh);
			stream.WriteLine("{0}", mesh.vertices.Count);
			foreach (Vertex v in mesh.vertices.Values)
			{
				stream.WriteLine("{0} {1} {2} {3}", v.id, v.x.ToString(nfi), v.y.ToString(nfi), v.label);
			}
		}
		else
		{
			stream.WriteLine("0");
		}
		stream.WriteLine("{0}", mesh.subsegs.Count);
		Osub subseg = default(Osub);
		subseg.orient = 0;
		foreach (SubSegment item in mesh.subsegs.Values)
		{
			if (item.hash > 0)
			{
				subseg.seg = item;
				Vertex p1 = subseg.Org();
				Vertex p2 = subseg.Dest();
				stream.WriteLine("{0} {1} {2} {3}", subseg.seg.hash, p1.id, p2.id, subseg.seg.boundary);
			}
		}
		Otri tri = default(Otri);
		Otri trisym = default(Otri);
		tri.orient = 0;
		stream.WriteLine("{0}", mesh.triangles.Count);
		foreach (Triangle item2 in mesh.triangles)
		{
			tri.tri = item2;
			Vertex p1 = tri.Org();
			Vertex p2 = tri.Dest();
			Vertex p3 = tri.Apex();
			int h1 = ((p1 == null) ? (-1) : p1.id);
			int h2 = ((p2 == null) ? (-1) : p2.id);
			int h3 = ((p3 == null) ? (-1) : p3.id);
			stream.Write("{0} {1} {2} {3}", tri.tri.hash, h1, h2, h3);
			tri.orient = 1;
			tri.Sym(ref trisym);
			int n1 = trisym.tri.hash;
			tri.orient = 2;
			tri.Sym(ref trisym);
			int n2 = trisym.tri.hash;
			tri.orient = 0;
			tri.Sym(ref trisym);
			int n3 = trisym.tri.hash;
			stream.WriteLine(" {0} {1} {2}", n1, n2, n3);
		}
	}

	private bool VerticesChanged(Mesh mesh)
	{
		if (vertices == null || mesh.Vertices.Count != vertices.Length)
		{
			return true;
		}
		int i = 0;
		foreach (Vertex vertex in mesh.Vertices)
		{
			if (vertex.id != vertices[i++])
			{
				return true;
			}
		}
		return false;
	}

	private void HashVertices(Mesh mesh)
	{
		if (vertices == null || mesh.Vertices.Count != vertices.Length)
		{
			vertices = new int[mesh.Vertices.Count];
		}
		int i = 0;
		foreach (Vertex v in mesh.Vertices)
		{
			vertices[i++] = v.id;
		}
	}
}
