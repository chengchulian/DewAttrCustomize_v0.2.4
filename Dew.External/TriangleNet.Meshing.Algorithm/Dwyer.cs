using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Tools;
using TriangleNet.Topology;

namespace TriangleNet.Meshing.Algorithm;

public class Dwyer : ITriangulator
{
	private IPredicates predicates;

	public bool UseDwyer = true;

	private Vertex[] sortarray;

	private Mesh mesh;

	public IMesh Triangulate(IList<Vertex> points, Configuration config)
	{
		predicates = config.Predicates();
		mesh = new Mesh(config);
		mesh.TransferNodes(points);
		Otri hullleft = default(Otri);
		Otri hullright = default(Otri);
		int n = points.Count;
		sortarray = new Vertex[n];
		int i = 0;
		foreach (Vertex v in points)
		{
			sortarray[i++] = v;
		}
		VertexSorter.Sort(sortarray);
		i = 0;
		for (int j = 1; j < n; j++)
		{
			if (sortarray[i].x == sortarray[j].x && sortarray[i].y == sortarray[j].y)
			{
				if (Log.Verbose)
				{
					Log.Instance.Warning($"A duplicate vertex appeared and was ignored (ID {sortarray[j].id}).", "Dwyer.Triangulate()");
				}
				sortarray[j].type = VertexType.UndeadVertex;
				mesh.undeads++;
			}
			else
			{
				i++;
				sortarray[i] = sortarray[j];
			}
		}
		i++;
		if (UseDwyer)
		{
			VertexSorter.Alternate(sortarray, i);
		}
		DivconqRecurse(0, i - 1, 0, ref hullleft, ref hullright);
		mesh.hullsize = RemoveGhosts(ref hullleft);
		return mesh;
	}

	private void MergeHulls(ref Otri farleft, ref Otri innerleft, ref Otri innerright, ref Otri farright, int axis)
	{
		Otri leftcand = default(Otri);
		Otri rightcand = default(Otri);
		Otri nextedge = default(Otri);
		Otri sidecasing = default(Otri);
		Otri topcasing = default(Otri);
		Otri outercasing = default(Otri);
		Otri checkedge = default(Otri);
		Otri baseedge = default(Otri);
		Vertex innerleftdest = innerleft.Dest();
		Vertex innerleftapex = innerleft.Apex();
		Vertex innerrightorg = innerright.Org();
		Vertex innerrightapex = innerright.Apex();
		Vertex farleftpt;
		Vertex farrightpt;
		if (UseDwyer && axis == 1)
		{
			farleftpt = farleft.Org();
			Vertex farleftapex = farleft.Apex();
			farrightpt = farright.Dest();
			Vertex farrightapex = farright.Apex();
			while (farleftapex.y < farleftpt.y)
			{
				farleft.Lnext();
				farleft.Sym();
				farleftpt = farleftapex;
				farleftapex = farleft.Apex();
			}
			innerleft.Sym(ref checkedge);
			Vertex checkvertex = checkedge.Apex();
			while (checkvertex.y > innerleftdest.y)
			{
				checkedge.Lnext(ref innerleft);
				innerleftapex = innerleftdest;
				innerleftdest = checkvertex;
				innerleft.Sym(ref checkedge);
				checkvertex = checkedge.Apex();
			}
			while (innerrightapex.y < innerrightorg.y)
			{
				innerright.Lnext();
				innerright.Sym();
				innerrightorg = innerrightapex;
				innerrightapex = innerright.Apex();
			}
			farright.Sym(ref checkedge);
			checkvertex = checkedge.Apex();
			while (checkvertex.y > farrightpt.y)
			{
				checkedge.Lnext(ref farright);
				farrightapex = farrightpt;
				farrightpt = checkvertex;
				farright.Sym(ref checkedge);
				checkvertex = checkedge.Apex();
			}
		}
		bool changemade;
		do
		{
			changemade = false;
			if (predicates.CounterClockwise(innerleftdest, innerleftapex, innerrightorg) > 0.0)
			{
				innerleft.Lprev();
				innerleft.Sym();
				innerleftdest = innerleftapex;
				innerleftapex = innerleft.Apex();
				changemade = true;
			}
			if (predicates.CounterClockwise(innerrightapex, innerrightorg, innerleftdest) > 0.0)
			{
				innerright.Lnext();
				innerright.Sym();
				innerrightorg = innerrightapex;
				innerrightapex = innerright.Apex();
				changemade = true;
			}
		}
		while (changemade);
		innerleft.Sym(ref leftcand);
		innerright.Sym(ref rightcand);
		mesh.MakeTriangle(ref baseedge);
		baseedge.Bond(ref innerleft);
		baseedge.Lnext();
		baseedge.Bond(ref innerright);
		baseedge.Lnext();
		baseedge.SetOrg(innerrightorg);
		baseedge.SetDest(innerleftdest);
		farleftpt = farleft.Org();
		if (innerleftdest == farleftpt)
		{
			baseedge.Lnext(ref farleft);
		}
		farrightpt = farright.Dest();
		if (innerrightorg == farrightpt)
		{
			baseedge.Lprev(ref farright);
		}
		Vertex lowerleft = innerleftdest;
		Vertex lowerright = innerrightorg;
		Vertex upperleft = leftcand.Apex();
		Vertex upperright = rightcand.Apex();
		while (true)
		{
			bool leftfinished = predicates.CounterClockwise(upperleft, lowerleft, lowerright) <= 0.0;
			bool rightfinished = predicates.CounterClockwise(upperright, lowerleft, lowerright) <= 0.0;
			if (leftfinished && rightfinished)
			{
				break;
			}
			if (!leftfinished)
			{
				leftcand.Lprev(ref nextedge);
				nextedge.Sym();
				Vertex nextapex = nextedge.Apex();
				if (nextapex != null)
				{
					bool badedge = predicates.InCircle(lowerleft, lowerright, upperleft, nextapex) > 0.0;
					while (badedge)
					{
						nextedge.Lnext();
						nextedge.Sym(ref topcasing);
						nextedge.Lnext();
						nextedge.Sym(ref sidecasing);
						nextedge.Bond(ref topcasing);
						leftcand.Bond(ref sidecasing);
						leftcand.Lnext();
						leftcand.Sym(ref outercasing);
						nextedge.Lprev();
						nextedge.Bond(ref outercasing);
						leftcand.SetOrg(lowerleft);
						leftcand.SetDest(null);
						leftcand.SetApex(nextapex);
						nextedge.SetOrg(null);
						nextedge.SetDest(upperleft);
						nextedge.SetApex(nextapex);
						upperleft = nextapex;
						sidecasing.Copy(ref nextedge);
						nextapex = nextedge.Apex();
						badedge = nextapex != null && predicates.InCircle(lowerleft, lowerright, upperleft, nextapex) > 0.0;
					}
				}
			}
			if (!rightfinished)
			{
				rightcand.Lnext(ref nextedge);
				nextedge.Sym();
				Vertex nextapex = nextedge.Apex();
				if (nextapex != null)
				{
					bool badedge = predicates.InCircle(lowerleft, lowerright, upperright, nextapex) > 0.0;
					while (badedge)
					{
						nextedge.Lprev();
						nextedge.Sym(ref topcasing);
						nextedge.Lprev();
						nextedge.Sym(ref sidecasing);
						nextedge.Bond(ref topcasing);
						rightcand.Bond(ref sidecasing);
						rightcand.Lprev();
						rightcand.Sym(ref outercasing);
						nextedge.Lnext();
						nextedge.Bond(ref outercasing);
						rightcand.SetOrg(null);
						rightcand.SetDest(lowerright);
						rightcand.SetApex(nextapex);
						nextedge.SetOrg(upperright);
						nextedge.SetDest(null);
						nextedge.SetApex(nextapex);
						upperright = nextapex;
						sidecasing.Copy(ref nextedge);
						nextapex = nextedge.Apex();
						badedge = nextapex != null && predicates.InCircle(lowerleft, lowerright, upperright, nextapex) > 0.0;
					}
				}
			}
			if (leftfinished || (!rightfinished && predicates.InCircle(upperleft, lowerleft, lowerright, upperright) > 0.0))
			{
				baseedge.Bond(ref rightcand);
				rightcand.Lprev(ref baseedge);
				baseedge.SetDest(lowerleft);
				lowerright = upperright;
				baseedge.Sym(ref rightcand);
				upperright = rightcand.Apex();
			}
			else
			{
				baseedge.Bond(ref leftcand);
				leftcand.Lnext(ref baseedge);
				baseedge.SetOrg(lowerright);
				lowerleft = upperleft;
				baseedge.Sym(ref leftcand);
				upperleft = leftcand.Apex();
			}
		}
		mesh.MakeTriangle(ref nextedge);
		nextedge.SetOrg(lowerleft);
		nextedge.SetDest(lowerright);
		nextedge.Bond(ref baseedge);
		nextedge.Lnext();
		nextedge.Bond(ref rightcand);
		nextedge.Lnext();
		nextedge.Bond(ref leftcand);
		if (UseDwyer && axis == 1)
		{
			farleftpt = farleft.Org();
			Vertex farleftapex = farleft.Apex();
			farrightpt = farright.Dest();
			Vertex farrightapex = farright.Apex();
			farleft.Sym(ref checkedge);
			Vertex checkvertex = checkedge.Apex();
			while (checkvertex.x < farleftpt.x)
			{
				checkedge.Lprev(ref farleft);
				farleftapex = farleftpt;
				farleftpt = checkvertex;
				farleft.Sym(ref checkedge);
				checkvertex = checkedge.Apex();
			}
			while (farrightapex.x > farrightpt.x)
			{
				farright.Lprev();
				farright.Sym();
				farrightpt = farrightapex;
				farrightapex = farright.Apex();
			}
		}
	}

	private void DivconqRecurse(int left, int right, int axis, ref Otri farleft, ref Otri farright)
	{
		Otri midtri = default(Otri);
		Otri tri1 = default(Otri);
		Otri tri2 = default(Otri);
		Otri tri3 = default(Otri);
		Otri innerleft = default(Otri);
		Otri innerright = default(Otri);
		int vertices = right - left + 1;
		switch (vertices)
		{
		case 2:
			mesh.MakeTriangle(ref farleft);
			farleft.SetOrg(sortarray[left]);
			farleft.SetDest(sortarray[left + 1]);
			mesh.MakeTriangle(ref farright);
			farright.SetOrg(sortarray[left + 1]);
			farright.SetDest(sortarray[left]);
			farleft.Bond(ref farright);
			farleft.Lprev();
			farright.Lnext();
			farleft.Bond(ref farright);
			farleft.Lprev();
			farright.Lnext();
			farleft.Bond(ref farright);
			farright.Lprev(ref farleft);
			break;
		case 3:
		{
			mesh.MakeTriangle(ref midtri);
			mesh.MakeTriangle(ref tri1);
			mesh.MakeTriangle(ref tri2);
			mesh.MakeTriangle(ref tri3);
			double area = predicates.CounterClockwise(sortarray[left], sortarray[left + 1], sortarray[left + 2]);
			if (area == 0.0)
			{
				midtri.SetOrg(sortarray[left]);
				midtri.SetDest(sortarray[left + 1]);
				tri1.SetOrg(sortarray[left + 1]);
				tri1.SetDest(sortarray[left]);
				tri2.SetOrg(sortarray[left + 2]);
				tri2.SetDest(sortarray[left + 1]);
				tri3.SetOrg(sortarray[left + 1]);
				tri3.SetDest(sortarray[left + 2]);
				midtri.Bond(ref tri1);
				tri2.Bond(ref tri3);
				midtri.Lnext();
				tri1.Lprev();
				tri2.Lnext();
				tri3.Lprev();
				midtri.Bond(ref tri3);
				tri1.Bond(ref tri2);
				midtri.Lnext();
				tri1.Lprev();
				tri2.Lnext();
				tri3.Lprev();
				midtri.Bond(ref tri1);
				tri2.Bond(ref tri3);
				tri1.Copy(ref farleft);
				tri2.Copy(ref farright);
				break;
			}
			midtri.SetOrg(sortarray[left]);
			tri1.SetDest(sortarray[left]);
			tri3.SetOrg(sortarray[left]);
			if (area > 0.0)
			{
				midtri.SetDest(sortarray[left + 1]);
				tri1.SetOrg(sortarray[left + 1]);
				tri2.SetDest(sortarray[left + 1]);
				midtri.SetApex(sortarray[left + 2]);
				tri2.SetOrg(sortarray[left + 2]);
				tri3.SetDest(sortarray[left + 2]);
			}
			else
			{
				midtri.SetDest(sortarray[left + 2]);
				tri1.SetOrg(sortarray[left + 2]);
				tri2.SetDest(sortarray[left + 2]);
				midtri.SetApex(sortarray[left + 1]);
				tri2.SetOrg(sortarray[left + 1]);
				tri3.SetDest(sortarray[left + 1]);
			}
			midtri.Bond(ref tri1);
			midtri.Lnext();
			midtri.Bond(ref tri2);
			midtri.Lnext();
			midtri.Bond(ref tri3);
			tri1.Lprev();
			tri2.Lnext();
			tri1.Bond(ref tri2);
			tri1.Lprev();
			tri3.Lprev();
			tri1.Bond(ref tri3);
			tri2.Lnext();
			tri3.Lprev();
			tri2.Bond(ref tri3);
			tri1.Copy(ref farleft);
			if (area > 0.0)
			{
				tri2.Copy(ref farright);
			}
			else
			{
				farleft.Lnext(ref farright);
			}
			break;
		}
		default:
		{
			int divider = vertices >> 1;
			DivconqRecurse(left, left + divider - 1, 1 - axis, ref farleft, ref innerleft);
			DivconqRecurse(left + divider, right, 1 - axis, ref innerright, ref farright);
			MergeHulls(ref farleft, ref innerleft, ref innerright, ref farright, axis);
			break;
		}
		}
	}

	private int RemoveGhosts(ref Otri startghost)
	{
		Otri searchedge = default(Otri);
		Otri dissolveedge = default(Otri);
		Otri deadtriangle = default(Otri);
		bool noPoly = !mesh.behavior.Poly;
		startghost.Lprev(ref searchedge);
		searchedge.Sym();
		mesh.dummytri.neighbors[0] = searchedge;
		startghost.Copy(ref dissolveedge);
		int hullsize = 0;
		do
		{
			hullsize++;
			dissolveedge.Lnext(ref deadtriangle);
			dissolveedge.Lprev();
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
			deadtriangle.Sym(ref dissolveedge);
			mesh.TriangleDealloc(deadtriangle.tri);
		}
		while (!dissolveedge.Equals(startghost));
		return hullsize;
	}
}
