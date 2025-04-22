using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.IO;

public class TriangleWriter
{
	private static NumberFormatInfo nfi = NumberFormatInfo.InvariantInfo;

	public void Write(Mesh mesh, string filename)
	{
		WritePoly(mesh, Path.ChangeExtension(filename, ".poly"));
		WriteElements(mesh, Path.ChangeExtension(filename, ".ele"));
	}

	public void WriteNodes(Mesh mesh, string filename)
	{
		using StreamWriter writer = new StreamWriter(filename);
		WriteNodes(writer, mesh);
	}

	private void WriteNodes(StreamWriter writer, Mesh mesh)
	{
		int outvertices = mesh.vertices.Count;
		int nextras = mesh.nextras;
		Behavior behavior = mesh.behavior;
		if (behavior.Jettison)
		{
			outvertices = mesh.vertices.Count - mesh.undeads;
		}
		if (writer == null)
		{
			return;
		}
		writer.WriteLine("{0} {1} {2} {3}", outvertices, mesh.mesh_dim, nextras, behavior.UseBoundaryMarkers ? "1" : "0");
		if (mesh.numbering == NodeNumbering.None)
		{
			mesh.Renumber();
		}
		if (mesh.numbering == NodeNumbering.Linear)
		{
			WriteNodes(writer, mesh.vertices.Values, behavior.UseBoundaryMarkers, nextras, behavior.Jettison);
			return;
		}
		Vertex[] nodes = new Vertex[mesh.vertices.Count];
		foreach (Vertex node in mesh.vertices.Values)
		{
			nodes[node.id] = node;
		}
		WriteNodes(writer, nodes, behavior.UseBoundaryMarkers, nextras, behavior.Jettison);
	}

	private void WriteNodes(StreamWriter writer, IEnumerable<Vertex> nodes, bool markers, int attribs, bool jettison)
	{
		int index = 0;
		foreach (Vertex vertex in nodes)
		{
			if (!jettison || vertex.type != VertexType.UndeadVertex)
			{
				writer.Write("{0} {1} {2}", index, vertex.x.ToString(nfi), vertex.y.ToString(nfi));
				if (markers)
				{
					writer.Write(" {0}", vertex.label);
				}
				writer.WriteLine();
				index++;
			}
		}
	}

	public void WriteElements(Mesh mesh, string filename)
	{
		Otri tri = default(Otri);
		bool regions = mesh.behavior.useRegions;
		int j = 0;
		tri.orient = 0;
		using StreamWriter writer = new StreamWriter(filename);
		writer.WriteLine("{0} 3 {1}", mesh.triangles.Count, regions ? 1 : 0);
		foreach (Triangle triangle in mesh.triangles)
		{
			Triangle item = (tri.tri = triangle);
			Vertex p1 = tri.Org();
			Vertex p2 = tri.Dest();
			Vertex p3 = tri.Apex();
			writer.Write("{0} {1} {2} {3}", j, p1.id, p2.id, p3.id);
			if (regions)
			{
				writer.Write(" {0}", tri.tri.label);
			}
			writer.WriteLine();
			item.id = j++;
		}
	}

	public void WritePoly(IPolygon polygon, string filename)
	{
		bool hasMarkers = polygon.HasSegmentMarkers;
		using StreamWriter writer = new StreamWriter(filename);
		writer.WriteLine("{0} 2 0 {1}", polygon.Points.Count, polygon.HasPointMarkers ? "1" : "0");
		WriteNodes(writer, polygon.Points, polygon.HasPointMarkers, 0, jettison: false);
		writer.WriteLine("{0} {1}", polygon.Segments.Count, hasMarkers ? "1" : "0");
		int j = 0;
		foreach (ISegment seg in polygon.Segments)
		{
			Vertex p = seg.GetVertex(0);
			Vertex q = seg.GetVertex(1);
			if (hasMarkers)
			{
				writer.WriteLine("{0} {1} {2} {3}", j, p.ID, q.ID, seg.Label);
			}
			else
			{
				writer.WriteLine("{0} {1} {2}", j, p.ID, q.ID);
			}
			j++;
		}
		j = 0;
		writer.WriteLine("{0}", polygon.Holes.Count);
		foreach (Point hole in polygon.Holes)
		{
			writer.WriteLine("{0} {1} {2}", j++, hole.X.ToString(nfi), hole.Y.ToString(nfi));
		}
		if (polygon.Regions.Count <= 0)
		{
			return;
		}
		j = 0;
		writer.WriteLine("{0}", polygon.Regions.Count);
		foreach (RegionPointer region in polygon.Regions)
		{
			writer.WriteLine("{0} {1} {2} {3}", j, region.point.X.ToString(nfi), region.point.Y.ToString(nfi), region.id);
			j++;
		}
	}

	public void WritePoly(Mesh mesh, string filename)
	{
		WritePoly(mesh, filename, writeNodes: true);
	}

	public void WritePoly(Mesh mesh, string filename, bool writeNodes)
	{
		Osub subseg = default(Osub);
		bool useBoundaryMarkers = mesh.behavior.UseBoundaryMarkers;
		using StreamWriter writer = new StreamWriter(filename);
		if (writeNodes)
		{
			WriteNodes(writer, mesh);
		}
		else
		{
			writer.WriteLine("0 {0} {1} {2}", mesh.mesh_dim, mesh.nextras, useBoundaryMarkers ? "1" : "0");
		}
		writer.WriteLine("{0} {1}", mesh.subsegs.Count, useBoundaryMarkers ? "1" : "0");
		subseg.orient = 0;
		int j = 0;
		foreach (SubSegment item in mesh.subsegs.Values)
		{
			subseg.seg = item;
			Vertex pt1 = subseg.Org();
			Vertex pt2 = subseg.Dest();
			if (useBoundaryMarkers)
			{
				writer.WriteLine("{0} {1} {2} {3}", j, pt1.id, pt2.id, subseg.seg.boundary);
			}
			else
			{
				writer.WriteLine("{0} {1} {2}", j, pt1.id, pt2.id);
			}
			j++;
		}
		j = 0;
		writer.WriteLine("{0}", mesh.holes.Count);
		foreach (Point hole in mesh.holes)
		{
			writer.WriteLine("{0} {1} {2}", j++, hole.X.ToString(nfi), hole.Y.ToString(nfi));
		}
		if (mesh.regions.Count <= 0)
		{
			return;
		}
		j = 0;
		writer.WriteLine("{0}", mesh.regions.Count);
		foreach (RegionPointer region in mesh.regions)
		{
			writer.WriteLine("{0} {1} {2} {3}", j, region.point.X.ToString(nfi), region.point.Y.ToString(nfi), region.id);
			j++;
		}
	}

	public void WriteEdges(Mesh mesh, string filename)
	{
		Otri tri = default(Otri);
		Otri trisym = default(Otri);
		Osub checkmark = default(Osub);
		Behavior behavior = mesh.behavior;
		using StreamWriter writer = new StreamWriter(filename);
		writer.WriteLine("{0} {1}", mesh.NumberOfEdges, behavior.UseBoundaryMarkers ? "1" : "0");
		long index = 0L;
		foreach (Triangle item in mesh.triangles)
		{
			tri.tri = item;
			tri.orient = 0;
			while (tri.orient < 3)
			{
				tri.Sym(ref trisym);
				if (tri.tri.id < trisym.tri.id || trisym.tri.id == -1)
				{
					Vertex p1 = tri.Org();
					Vertex p2 = tri.Dest();
					if (behavior.UseBoundaryMarkers)
					{
						if (behavior.useSegments)
						{
							tri.Pivot(ref checkmark);
							if (checkmark.seg.hash == -1)
							{
								writer.WriteLine("{0} {1} {2} {3}", index, p1.id, p2.id, 0);
							}
							else
							{
								writer.WriteLine("{0} {1} {2} {3}", index, p1.id, p2.id, checkmark.seg.boundary);
							}
						}
						else
						{
							writer.WriteLine("{0} {1} {2} {3}", index, p1.id, p2.id, (trisym.tri.id == -1) ? "1" : "0");
						}
					}
					else
					{
						writer.WriteLine("{0} {1} {2}", index, p1.id, p2.id);
					}
					index++;
				}
				tri.orient++;
			}
		}
	}

	public void WriteNeighbors(Mesh mesh, string filename)
	{
		Otri tri = default(Otri);
		Otri trisym = default(Otri);
		int i = 0;
		using StreamWriter writer = new StreamWriter(filename);
		writer.WriteLine("{0} 3", mesh.triangles.Count);
		foreach (Triangle item in mesh.triangles)
		{
			tri.tri = item;
			tri.orient = 1;
			tri.Sym(ref trisym);
			int n1 = trisym.tri.id;
			tri.orient = 2;
			tri.Sym(ref trisym);
			int n2 = trisym.tri.id;
			tri.orient = 0;
			tri.Sym(ref trisym);
			int n3 = trisym.tri.id;
			writer.WriteLine("{0} {1} {2} {3}", i++, n1, n2, n3);
		}
	}
}
