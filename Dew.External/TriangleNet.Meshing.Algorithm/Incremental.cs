using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.Meshing.Algorithm;

public class Incremental : ITriangulator
{
	private Mesh mesh;

	public IMesh Triangulate(IList<Vertex> points, Configuration config)
	{
		mesh = new Mesh(config);
		mesh.TransferNodes(points);
		Otri starttri = default(Otri);
		GetBoundingBox();
		foreach (Vertex v in mesh.vertices.Values)
		{
			starttri.tri = mesh.dummytri;
			Osub tmp = default(Osub);
			if (mesh.InsertVertex(v, ref starttri, ref tmp, segmentflaws: false, triflaws: false) == InsertVertexResult.Duplicate)
			{
				if (Log.Verbose)
				{
					Log.Instance.Warning("A duplicate vertex appeared and was ignored.", "Incremental.Triangulate()");
				}
				v.type = VertexType.UndeadVertex;
				mesh.undeads++;
			}
		}
		mesh.hullsize = RemoveBox();
		return mesh;
	}

	private void GetBoundingBox()
	{
		Otri inftri = default(Otri);
		Rectangle box = mesh.bounds;
		double width = box.Width;
		if (box.Height > width)
		{
			width = box.Height;
		}
		if (width == 0.0)
		{
			width = 1.0;
		}
		mesh.infvertex1 = new Vertex(box.Left - 50.0 * width, box.Bottom - 40.0 * width);
		mesh.infvertex2 = new Vertex(box.Right + 50.0 * width, box.Bottom - 40.0 * width);
		mesh.infvertex3 = new Vertex(0.5 * (box.Left + box.Right), box.Top + 60.0 * width);
		mesh.MakeTriangle(ref inftri);
		inftri.SetOrg(mesh.infvertex1);
		inftri.SetDest(mesh.infvertex2);
		inftri.SetApex(mesh.infvertex3);
		mesh.dummytri.neighbors[0] = inftri;
	}

	private int RemoveBox()
	{
		Otri deadtriangle = default(Otri);
		Otri searchedge = default(Otri);
		Otri checkedge = default(Otri);
		Otri nextedge = default(Otri);
		Otri finaledge = default(Otri);
		Otri dissolveedge = default(Otri);
		bool noPoly = !mesh.behavior.Poly;
		nextedge.tri = mesh.dummytri;
		nextedge.orient = 0;
		nextedge.Sym();
		nextedge.Lprev(ref finaledge);
		nextedge.Lnext();
		nextedge.Sym();
		nextedge.Lprev(ref searchedge);
		searchedge.Sym();
		nextedge.Lnext(ref checkedge);
		checkedge.Sym();
		if (checkedge.tri.id == -1)
		{
			searchedge.Lprev();
			searchedge.Sym();
		}
		mesh.dummytri.neighbors[0] = searchedge;
		int hullsize = -2;
		while (!nextedge.Equals(finaledge))
		{
			hullsize++;
			nextedge.Lprev(ref dissolveedge);
			dissolveedge.Sym();
			if (noPoly && dissolveedge.tri.id != -1)
			{
				Vertex markorg = dissolveedge.Org();
				if (markorg.label == 0)
				{
					markorg.label = 1;
				}
			}
			dissolveedge.Dissolve(mesh.dummytri);
			nextedge.Lnext(ref deadtriangle);
			deadtriangle.Sym(ref nextedge);
			mesh.TriangleDealloc(deadtriangle.tri);
			if (nextedge.tri.id == -1)
			{
				dissolveedge.Copy(ref nextedge);
			}
		}
		mesh.TriangleDealloc(finaledge.tri);
		return hullsize;
	}
}
