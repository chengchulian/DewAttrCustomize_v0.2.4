using System;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Topology;
using TriangleNet.Topology.DCEL;

namespace TriangleNet.Meshing;

public static class Converter
{
	public static Mesh ToMesh(Polygon polygon, IList<ITriangle> triangles)
	{
		return ToMesh(polygon, triangles.ToArray());
	}

	public static Mesh ToMesh(Polygon polygon, ITriangle[] triangles)
	{
		Otri tri = default(Otri);
		Osub subseg = default(Osub);
		int i = 0;
		int elements = ((triangles != null) ? triangles.Length : 0);
		int segments = polygon.Segments.Count;
		Mesh mesh = new Mesh(new Configuration());
		mesh.TransferNodes(polygon.Points);
		mesh.regions.AddRange(polygon.Regions);
		mesh.behavior.useRegions = polygon.Regions.Count > 0;
		if (polygon.Segments.Count > 0)
		{
			mesh.behavior.Poly = true;
			mesh.holes.AddRange(polygon.Holes);
		}
		for (i = 0; i < elements; i++)
		{
			mesh.MakeTriangle(ref tri);
		}
		if (mesh.behavior.Poly)
		{
			mesh.insegments = segments;
			for (i = 0; i < segments; i++)
			{
				mesh.MakeSegment(ref subseg);
			}
		}
		List<Otri>[] vertexarray = SetNeighbors(mesh, triangles);
		SetSegments(mesh, polygon, vertexarray);
		return mesh;
	}

	private static List<Otri>[] SetNeighbors(Mesh mesh, ITriangle[] triangles)
	{
		Otri tri = default(Otri);
		Otri triangleleft = default(Otri);
		Otri checktri = default(Otri);
		Otri checkleft = default(Otri);
		int[] corner = new int[3];
		List<Otri>[] vertexarray = new List<Otri>[mesh.vertices.Count];
		int i;
		for (i = 0; i < mesh.vertices.Count; i++)
		{
			Otri tmp = default(Otri);
			tmp.tri = mesh.dummytri;
			vertexarray[i] = new List<Otri>(3);
			vertexarray[i].Add(tmp);
		}
		i = 0;
		foreach (Triangle item in mesh.triangles)
		{
			tri.tri = item;
			for (int j = 0; j < 3; j++)
			{
				corner[j] = triangles[i].GetVertexID(j);
				if (corner[j] < 0 || corner[j] >= mesh.invertices)
				{
					Log.Instance.Error("Triangle has an invalid vertex index.", "MeshReader.Reconstruct()");
					throw new Exception("Triangle has an invalid vertex index.");
				}
			}
			tri.tri.label = triangles[i].Label;
			if (mesh.behavior.VarArea)
			{
				tri.tri.area = triangles[i].Area;
			}
			tri.orient = 0;
			tri.SetOrg(mesh.vertices[corner[0]]);
			tri.SetDest(mesh.vertices[corner[1]]);
			tri.SetApex(mesh.vertices[corner[2]]);
			tri.orient = 0;
			while (tri.orient < 3)
			{
				int aroundvertex = corner[tri.orient];
				int index = vertexarray[aroundvertex].Count - 1;
				Otri nexttri = vertexarray[aroundvertex][index];
				vertexarray[aroundvertex].Add(tri);
				checktri = nexttri;
				if (checktri.tri.id != -1)
				{
					global::TriangleNet.Geometry.Vertex tdest = tri.Dest();
					global::TriangleNet.Geometry.Vertex tapex = tri.Apex();
					do
					{
						global::TriangleNet.Geometry.Vertex checkdest = checktri.Dest();
						global::TriangleNet.Geometry.Vertex checkapex = checktri.Apex();
						if (tapex == checkdest)
						{
							tri.Lprev(ref triangleleft);
							triangleleft.Bond(ref checktri);
						}
						if (tdest == checkapex)
						{
							checktri.Lprev(ref checkleft);
							tri.Bond(ref checkleft);
						}
						index--;
						nexttri = vertexarray[aroundvertex][index];
						checktri = nexttri;
					}
					while (checktri.tri.id != -1);
				}
				tri.orient++;
			}
			i++;
		}
		return vertexarray;
	}

	private static void SetSegments(Mesh mesh, Polygon polygon, List<Otri>[] vertexarray)
	{
		Otri checktri = default(Otri);
		Otri checkneighbor = default(Otri);
		Osub subseg = default(Osub);
		int hullsize = 0;
		if (mesh.behavior.Poly)
		{
			int boundmarker = 0;
			int i = 0;
			foreach (SubSegment item in mesh.subsegs.Values)
			{
				subseg.seg = item;
				global::TriangleNet.Geometry.Vertex sorg = polygon.Segments[i].GetVertex(0);
				global::TriangleNet.Geometry.Vertex sdest = polygon.Segments[i].GetVertex(1);
				boundmarker = polygon.Segments[i].Label;
				if (sorg.id < 0 || sorg.id >= mesh.invertices || sdest.id < 0 || sdest.id >= mesh.invertices)
				{
					Log.Instance.Error("Segment has an invalid vertex index.", "MeshReader.Reconstruct()");
					throw new Exception("Segment has an invalid vertex index.");
				}
				subseg.orient = 0;
				subseg.SetOrg(sorg);
				subseg.SetDest(sdest);
				subseg.SetSegOrg(sorg);
				subseg.SetSegDest(sdest);
				subseg.seg.boundary = boundmarker;
				subseg.orient = 0;
				while (subseg.orient < 2)
				{
					int aroundvertex = ((subseg.orient == 1) ? sorg.id : sdest.id);
					int index = vertexarray[aroundvertex].Count - 1;
					Otri prevlink = vertexarray[aroundvertex][index];
					checktri = vertexarray[aroundvertex][index];
					global::TriangleNet.Geometry.Vertex tmp = subseg.Org();
					bool notfound = true;
					while (notfound && checktri.tri.id != -1)
					{
						global::TriangleNet.Geometry.Vertex checkdest = checktri.Dest();
						if (tmp == checkdest)
						{
							vertexarray[aroundvertex].Remove(prevlink);
							checktri.SegBond(ref subseg);
							checktri.Sym(ref checkneighbor);
							if (checkneighbor.tri.id == -1)
							{
								mesh.InsertSubseg(ref checktri, 1);
								hullsize++;
							}
							notfound = false;
						}
						index--;
						prevlink = vertexarray[aroundvertex][index];
						checktri = vertexarray[aroundvertex][index];
					}
					subseg.orient++;
				}
				i++;
			}
		}
		for (int i = 0; i < mesh.vertices.Count; i++)
		{
			int index2 = vertexarray[i].Count - 1;
			checktri = vertexarray[i][index2];
			while (checktri.tri.id != -1)
			{
				index2--;
				Otri otri = vertexarray[i][index2];
				checktri.SegDissolve(mesh.dummysub);
				checktri.Sym(ref checkneighbor);
				if (checkneighbor.tri.id == -1)
				{
					mesh.InsertSubseg(ref checktri, 1);
					hullsize++;
				}
				checktri = otri;
			}
		}
		mesh.hullsize = hullsize;
	}

	public static DcelMesh ToDCEL(Mesh mesh)
	{
		DcelMesh dcel = new DcelMesh();
		global::TriangleNet.Topology.DCEL.Vertex[] vertices = new global::TriangleNet.Topology.DCEL.Vertex[mesh.vertices.Count];
		Face[] faces = new Face[mesh.triangles.Count];
		dcel.HalfEdges.Capacity = 2 * mesh.NumberOfEdges;
		mesh.Renumber();
		foreach (global::TriangleNet.Geometry.Vertex v in mesh.vertices.Values)
		{
			global::TriangleNet.Topology.DCEL.Vertex vertex = new global::TriangleNet.Topology.DCEL.Vertex(v.x, v.y);
			vertex.id = v.id;
			vertex.label = v.label;
			vertices[v.id] = vertex;
		}
		List<HalfEdge>[] map = new List<HalfEdge>[mesh.triangles.Count];
		foreach (Triangle t in mesh.triangles)
		{
			Face face = new Face(null);
			face.id = t.id;
			faces[t.id] = face;
			map[t.id] = new List<HalfEdge>(3);
		}
		Otri tri = default(Otri);
		Otri neighbor = default(Otri);
		_ = mesh.triangles.Count;
		List<HalfEdge> edges = dcel.HalfEdges;
		int k = 0;
		Dictionary<int, HalfEdge> boundary = new Dictionary<int, HalfEdge>();
		foreach (Triangle t2 in mesh.triangles)
		{
			int id = t2.id;
			tri.tri = t2;
			for (int i = 0; i < 3; i++)
			{
				tri.orient = i;
				tri.Sym(ref neighbor);
				int nid = neighbor.tri.id;
				if (id < nid || nid < 0)
				{
					Face face = faces[id];
					global::TriangleNet.Geometry.Vertex org = tri.Org();
					global::TriangleNet.Geometry.Vertex dest = tri.Dest();
					HalfEdge edge = new HalfEdge(vertices[org.id], face);
					HalfEdge twin = new HalfEdge(vertices[dest.id], (nid < 0) ? Face.Empty : faces[nid]);
					map[id].Add(edge);
					if (nid >= 0)
					{
						map[nid].Add(twin);
					}
					else
					{
						boundary.Add(dest.id, twin);
					}
					edge.origin.leaving = edge;
					twin.origin.leaving = twin;
					edge.twin = twin;
					twin.twin = edge;
					edge.id = k++;
					twin.id = k++;
					edges.Add(edge);
					edges.Add(twin);
				}
			}
		}
		List<HalfEdge>[] array = map;
		foreach (List<HalfEdge> t3 in array)
		{
			HalfEdge edge = t3[0];
			HalfEdge next = t3[1];
			if (edge.twin.origin.id == next.origin.id)
			{
				edge.next = next;
				next.next = t3[2];
				t3[2].next = edge;
			}
			else
			{
				edge.next = t3[2];
				next.next = edge;
				t3[2].next = next;
			}
		}
		foreach (HalfEdge e in boundary.Values)
		{
			e.next = boundary[e.twin.origin.id];
		}
		dcel.Vertices.AddRange(vertices);
		dcel.Faces.AddRange(faces);
		return dcel;
	}
}
