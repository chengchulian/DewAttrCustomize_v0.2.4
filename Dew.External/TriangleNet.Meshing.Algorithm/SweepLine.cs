using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Tools;
using TriangleNet.Topology;

namespace TriangleNet.Meshing.Algorithm;

public class SweepLine : ITriangulator
{
	private class SweepEvent
	{
		public double xkey;

		public double ykey;

		public Vertex vertexEvent;

		public Otri otriEvent;

		public int heapposition;
	}

	private class SweepEventVertex : Vertex
	{
		public SweepEvent evt;

		public SweepEventVertex(SweepEvent e)
		{
			evt = e;
		}
	}

	private class SplayNode
	{
		public Otri keyedge;

		public Vertex keydest;

		public SplayNode lchild;

		public SplayNode rchild;
	}

	private static int randomseed = 1;

	private static int SAMPLERATE = 10;

	private IPredicates predicates;

	private Mesh mesh;

	private double xminextreme;

	private List<SplayNode> splaynodes;

	private static int randomnation(int choices)
	{
		randomseed = (randomseed * 1366 + 150889) % 714025;
		return randomseed / (714025 / choices + 1);
	}

	public IMesh Triangulate(IList<Vertex> points, Configuration config)
	{
		predicates = config.Predicates();
		mesh = new Mesh(config);
		mesh.TransferNodes(points);
		xminextreme = 10.0 * mesh.bounds.Left - 9.0 * mesh.bounds.Right;
		Otri bottommost = default(Otri);
		Otri searchtri = default(Otri);
		Otri lefttri = default(Otri);
		Otri righttri = default(Otri);
		Otri farlefttri = default(Otri);
		Otri farrighttri = default(Otri);
		Otri inserttri = default(Otri);
		bool farrightflag = false;
		splaynodes = new List<SplayNode>();
		SplayNode splayroot = null;
		int heapsize = points.Count;
		CreateHeap(out var eventheap, heapsize);
		mesh.MakeTriangle(ref lefttri);
		mesh.MakeTriangle(ref righttri);
		lefttri.Bond(ref righttri);
		lefttri.Lnext();
		righttri.Lprev();
		lefttri.Bond(ref righttri);
		lefttri.Lnext();
		righttri.Lprev();
		lefttri.Bond(ref righttri);
		Vertex firstvertex = eventheap[0].vertexEvent;
		HeapDelete(eventheap, heapsize, 0);
		heapsize--;
		Vertex secondvertex;
		do
		{
			if (heapsize == 0)
			{
				Log.Instance.Error("Input vertices are all identical.", "SweepLine.Triangulate()");
				throw new Exception("Input vertices are all identical.");
			}
			secondvertex = eventheap[0].vertexEvent;
			HeapDelete(eventheap, heapsize, 0);
			heapsize--;
			if (firstvertex.x == secondvertex.x && firstvertex.y == secondvertex.y)
			{
				if (Log.Verbose)
				{
					Log.Instance.Warning("A duplicate vertex appeared and was ignored (ID " + secondvertex.id + ").", "SweepLine.Triangulate().1");
				}
				secondvertex.type = VertexType.UndeadVertex;
				mesh.undeads++;
			}
		}
		while (firstvertex.x == secondvertex.x && firstvertex.y == secondvertex.y);
		lefttri.SetOrg(firstvertex);
		lefttri.SetDest(secondvertex);
		righttri.SetOrg(secondvertex);
		righttri.SetDest(firstvertex);
		lefttri.Lprev(ref bottommost);
		Vertex lastvertex = secondvertex;
		while (heapsize > 0)
		{
			SweepEvent nextevent = eventheap[0];
			HeapDelete(eventheap, heapsize, 0);
			heapsize--;
			bool check4events = true;
			if (nextevent.xkey < mesh.bounds.Left)
			{
				Otri fliptri = nextevent.otriEvent;
				fliptri.Oprev(ref farlefttri);
				Check4DeadEvent(ref farlefttri, eventheap, ref heapsize);
				fliptri.Onext(ref farrighttri);
				Check4DeadEvent(ref farrighttri, eventheap, ref heapsize);
				if (farlefttri.Equals(bottommost))
				{
					fliptri.Lprev(ref bottommost);
				}
				mesh.Flip(ref fliptri);
				fliptri.SetApex(null);
				fliptri.Lprev(ref lefttri);
				fliptri.Lnext(ref righttri);
				lefttri.Sym(ref farlefttri);
				if (randomnation(SAMPLERATE) == 0)
				{
					fliptri.Sym();
					Vertex leftvertex = fliptri.Dest();
					Vertex midvertex = fliptri.Apex();
					Vertex rightvertex = fliptri.Org();
					splayroot = CircleTopInsert(splayroot, lefttri, leftvertex, midvertex, rightvertex, nextevent.ykey);
				}
			}
			else
			{
				Vertex nextvertex = nextevent.vertexEvent;
				if (nextvertex.x == lastvertex.x && nextvertex.y == lastvertex.y)
				{
					if (Log.Verbose)
					{
						Log.Instance.Warning("A duplicate vertex appeared and was ignored (ID " + nextvertex.id + ").", "SweepLine.Triangulate().2");
					}
					nextvertex.type = VertexType.UndeadVertex;
					mesh.undeads++;
					check4events = false;
				}
				else
				{
					lastvertex = nextvertex;
					splayroot = FrontLocate(splayroot, bottommost, nextvertex, ref searchtri, ref farrightflag);
					Check4DeadEvent(ref searchtri, eventheap, ref heapsize);
					searchtri.Copy(ref farrighttri);
					searchtri.Sym(ref farlefttri);
					mesh.MakeTriangle(ref lefttri);
					mesh.MakeTriangle(ref righttri);
					Vertex connectvertex = farrighttri.Dest();
					lefttri.SetOrg(connectvertex);
					lefttri.SetDest(nextvertex);
					righttri.SetOrg(nextvertex);
					righttri.SetDest(connectvertex);
					lefttri.Bond(ref righttri);
					lefttri.Lnext();
					righttri.Lprev();
					lefttri.Bond(ref righttri);
					lefttri.Lnext();
					righttri.Lprev();
					lefttri.Bond(ref farlefttri);
					righttri.Bond(ref farrighttri);
					if (!farrightflag && farrighttri.Equals(bottommost))
					{
						lefttri.Copy(ref bottommost);
					}
					if (randomnation(SAMPLERATE) == 0)
					{
						splayroot = SplayInsert(splayroot, lefttri, nextvertex);
					}
					else if (randomnation(SAMPLERATE) == 0)
					{
						righttri.Lnext(ref inserttri);
						splayroot = SplayInsert(splayroot, inserttri, nextvertex);
					}
				}
			}
			if (check4events)
			{
				Vertex leftvertex = farlefttri.Apex();
				Vertex midvertex = lefttri.Dest();
				Vertex rightvertex = lefttri.Apex();
				double lefttest = predicates.CounterClockwise(leftvertex, midvertex, rightvertex);
				if (lefttest > 0.0)
				{
					SweepEvent newevent = new SweepEvent();
					newevent.xkey = xminextreme;
					newevent.ykey = CircleTop(leftvertex, midvertex, rightvertex, lefttest);
					newevent.otriEvent = lefttri;
					HeapInsert(eventheap, heapsize, newevent);
					heapsize++;
					lefttri.SetOrg(new SweepEventVertex(newevent));
				}
				leftvertex = righttri.Apex();
				midvertex = righttri.Org();
				rightvertex = farrighttri.Apex();
				double righttest = predicates.CounterClockwise(leftvertex, midvertex, rightvertex);
				if (righttest > 0.0)
				{
					SweepEvent newevent = new SweepEvent();
					newevent.xkey = xminextreme;
					newevent.ykey = CircleTop(leftvertex, midvertex, rightvertex, righttest);
					newevent.otriEvent = farrighttri;
					HeapInsert(eventheap, heapsize, newevent);
					heapsize++;
					farrighttri.SetOrg(new SweepEventVertex(newevent));
				}
			}
		}
		splaynodes.Clear();
		bottommost.Lprev();
		mesh.hullsize = RemoveGhosts(ref bottommost);
		return mesh;
	}

	private void HeapInsert(SweepEvent[] heap, int heapsize, SweepEvent newevent)
	{
		double eventx = newevent.xkey;
		double eventy = newevent.ykey;
		int eventnum = heapsize;
		bool notdone = eventnum > 0;
		while (notdone)
		{
			int parent = eventnum - 1 >> 1;
			if (heap[parent].ykey < eventy || (heap[parent].ykey == eventy && heap[parent].xkey <= eventx))
			{
				notdone = false;
				continue;
			}
			heap[eventnum] = heap[parent];
			heap[eventnum].heapposition = eventnum;
			eventnum = parent;
			notdone = eventnum > 0;
		}
		heap[eventnum] = newevent;
		newevent.heapposition = eventnum;
	}

	private void Heapify(SweepEvent[] heap, int heapsize, int eventnum)
	{
		SweepEvent thisevent = heap[eventnum];
		double eventx = thisevent.xkey;
		double eventy = thisevent.ykey;
		int leftchild = 2 * eventnum + 1;
		bool notdone = leftchild < heapsize;
		while (notdone)
		{
			int smallest = ((!(heap[leftchild].ykey < eventy) && (heap[leftchild].ykey != eventy || !(heap[leftchild].xkey < eventx))) ? eventnum : leftchild);
			int rightchild = leftchild + 1;
			if (rightchild < heapsize && (heap[rightchild].ykey < heap[smallest].ykey || (heap[rightchild].ykey == heap[smallest].ykey && heap[rightchild].xkey < heap[smallest].xkey)))
			{
				smallest = rightchild;
			}
			if (smallest == eventnum)
			{
				notdone = false;
				continue;
			}
			heap[eventnum] = heap[smallest];
			heap[eventnum].heapposition = eventnum;
			heap[smallest] = thisevent;
			thisevent.heapposition = smallest;
			eventnum = smallest;
			leftchild = 2 * eventnum + 1;
			notdone = leftchild < heapsize;
		}
	}

	private void HeapDelete(SweepEvent[] heap, int heapsize, int eventnum)
	{
		SweepEvent moveevent = heap[heapsize - 1];
		if (eventnum > 0)
		{
			double eventx = moveevent.xkey;
			double eventy = moveevent.ykey;
			bool notdone;
			do
			{
				int parent = eventnum - 1 >> 1;
				if (heap[parent].ykey < eventy || (heap[parent].ykey == eventy && heap[parent].xkey <= eventx))
				{
					notdone = false;
					continue;
				}
				heap[eventnum] = heap[parent];
				heap[eventnum].heapposition = eventnum;
				eventnum = parent;
				notdone = eventnum > 0;
			}
			while (notdone);
		}
		heap[eventnum] = moveevent;
		moveevent.heapposition = eventnum;
		Heapify(heap, heapsize - 1, eventnum);
	}

	private void CreateHeap(out SweepEvent[] eventheap, int size)
	{
		int maxevents = 3 * size / 2;
		eventheap = new SweepEvent[maxevents];
		int i = 0;
		foreach (Vertex thisvertex in mesh.vertices.Values)
		{
			SweepEvent evt = new SweepEvent();
			evt.vertexEvent = thisvertex;
			evt.xkey = thisvertex.x;
			evt.ykey = thisvertex.y;
			HeapInsert(eventheap, i++, evt);
		}
	}

	private SplayNode Splay(SplayNode splaytree, Point searchpoint, ref Otri searchtri)
	{
		if (splaytree == null)
		{
			return null;
		}
		if (splaytree.keyedge.Dest() == splaytree.keydest)
		{
			bool rightofroot = RightOfHyperbola(ref splaytree.keyedge, searchpoint);
			SplayNode child;
			if (rightofroot)
			{
				splaytree.keyedge.Copy(ref searchtri);
				child = splaytree.rchild;
			}
			else
			{
				child = splaytree.lchild;
			}
			if (child == null)
			{
				return splaytree;
			}
			if (child.keyedge.Dest() != child.keydest)
			{
				child = Splay(child, searchpoint, ref searchtri);
				if (child == null)
				{
					if (rightofroot)
					{
						splaytree.rchild = null;
					}
					else
					{
						splaytree.lchild = null;
					}
					return splaytree;
				}
			}
			bool rightofchild = RightOfHyperbola(ref child.keyedge, searchpoint);
			SplayNode grandchild;
			if (!rightofchild)
			{
				grandchild = (child.lchild = Splay(child.lchild, searchpoint, ref searchtri));
			}
			else
			{
				child.keyedge.Copy(ref searchtri);
				grandchild = (child.rchild = Splay(child.rchild, searchpoint, ref searchtri));
			}
			if (grandchild == null)
			{
				if (rightofroot)
				{
					splaytree.rchild = child.lchild;
					child.lchild = splaytree;
				}
				else
				{
					splaytree.lchild = child.rchild;
					child.rchild = splaytree;
				}
				return child;
			}
			if (rightofchild)
			{
				if (rightofroot)
				{
					splaytree.rchild = child.lchild;
					child.lchild = splaytree;
				}
				else
				{
					splaytree.lchild = grandchild.rchild;
					grandchild.rchild = splaytree;
				}
				child.rchild = grandchild.lchild;
				grandchild.lchild = child;
			}
			else
			{
				if (rightofroot)
				{
					splaytree.rchild = grandchild.lchild;
					grandchild.lchild = splaytree;
				}
				else
				{
					splaytree.lchild = child.rchild;
					child.rchild = splaytree;
				}
				child.lchild = grandchild.rchild;
				grandchild.rchild = child;
			}
			return grandchild;
		}
		SplayNode lefttree = Splay(splaytree.lchild, searchpoint, ref searchtri);
		SplayNode righttree = Splay(splaytree.rchild, searchpoint, ref searchtri);
		splaynodes.Remove(splaytree);
		if (lefttree == null)
		{
			return righttree;
		}
		if (righttree == null)
		{
			return lefttree;
		}
		if (lefttree.rchild == null)
		{
			lefttree.rchild = righttree.lchild;
			righttree.lchild = lefttree;
			return righttree;
		}
		if (righttree.lchild == null)
		{
			righttree.lchild = lefttree.rchild;
			lefttree.rchild = righttree;
			return lefttree;
		}
		SplayNode leftright = lefttree.rchild;
		while (leftright.rchild != null)
		{
			leftright = leftright.rchild;
		}
		leftright.rchild = righttree;
		return lefttree;
	}

	private SplayNode SplayInsert(SplayNode splayroot, Otri newkey, Point searchpoint)
	{
		SplayNode newsplaynode = new SplayNode();
		splaynodes.Add(newsplaynode);
		newkey.Copy(ref newsplaynode.keyedge);
		newsplaynode.keydest = newkey.Dest();
		if (splayroot == null)
		{
			newsplaynode.lchild = null;
			newsplaynode.rchild = null;
		}
		else if (RightOfHyperbola(ref splayroot.keyedge, searchpoint))
		{
			newsplaynode.lchild = splayroot;
			newsplaynode.rchild = splayroot.rchild;
			splayroot.rchild = null;
		}
		else
		{
			newsplaynode.lchild = splayroot.lchild;
			newsplaynode.rchild = splayroot;
			splayroot.lchild = null;
		}
		return newsplaynode;
	}

	private SplayNode FrontLocate(SplayNode splayroot, Otri bottommost, Vertex searchvertex, ref Otri searchtri, ref bool farright)
	{
		bottommost.Copy(ref searchtri);
		splayroot = Splay(splayroot, searchvertex, ref searchtri);
		bool farrightflag = false;
		while (!farrightflag && RightOfHyperbola(ref searchtri, searchvertex))
		{
			searchtri.Onext();
			farrightflag = searchtri.Equals(bottommost);
		}
		farright = farrightflag;
		return splayroot;
	}

	private SplayNode CircleTopInsert(SplayNode splayroot, Otri newkey, Vertex pa, Vertex pb, Vertex pc, double topy)
	{
		Point searchpoint = new Point();
		Otri dummytri = default(Otri);
		double ccwabc = predicates.CounterClockwise(pa, pb, pc);
		double xac = pa.x - pc.x;
		double yac = pa.y - pc.y;
		double num = pb.x - pc.x;
		double ybc = pb.y - pc.y;
		double aclen2 = xac * xac + yac * yac;
		double bclen2 = num * num + ybc * ybc;
		searchpoint.x = pc.x - (yac * bclen2 - ybc * aclen2) / (2.0 * ccwabc);
		searchpoint.y = topy;
		return SplayInsert(Splay(splayroot, searchpoint, ref dummytri), newkey, searchpoint);
	}

	private bool RightOfHyperbola(ref Otri fronttri, Point newsite)
	{
		Statistic.HyperbolaCount++;
		Vertex leftvertex = fronttri.Dest();
		Vertex rightvertex = fronttri.Apex();
		if (leftvertex.y < rightvertex.y || (leftvertex.y == rightvertex.y && leftvertex.x < rightvertex.x))
		{
			if (newsite.x >= rightvertex.x)
			{
				return true;
			}
		}
		else if (newsite.x <= leftvertex.x)
		{
			return false;
		}
		double dxa = leftvertex.x - newsite.x;
		double dya = leftvertex.y - newsite.y;
		double dxb = rightvertex.x - newsite.x;
		double dyb = rightvertex.y - newsite.y;
		return dya * (dxb * dxb + dyb * dyb) > dyb * (dxa * dxa + dya * dya);
	}

	private double CircleTop(Vertex pa, Vertex pb, Vertex pc, double ccwabc)
	{
		Statistic.CircleTopCount++;
		double xac = pa.x - pc.x;
		double yac = pa.y - pc.y;
		double xbc = pb.x - pc.x;
		double ybc = pb.y - pc.y;
		double num = pa.x - pb.x;
		double yab = pa.y - pb.y;
		double aclen2 = xac * xac + yac * yac;
		double bclen2 = xbc * xbc + ybc * ybc;
		double ablen2 = num * num + yab * yab;
		return pc.y + (xac * bclen2 - xbc * aclen2 + Math.Sqrt(aclen2 * bclen2 * ablen2)) / (2.0 * ccwabc);
	}

	private void Check4DeadEvent(ref Otri checktri, SweepEvent[] eventheap, ref int heapsize)
	{
		int eventnum = -1;
		SweepEventVertex eventvertex = checktri.Org() as SweepEventVertex;
		if (eventvertex != null)
		{
			eventnum = eventvertex.evt.heapposition;
			HeapDelete(eventheap, heapsize, eventnum);
			heapsize--;
			checktri.SetOrg(null);
		}
	}

	private int RemoveGhosts(ref Otri startghost)
	{
		Otri searchedge = default(Otri);
		Otri dissolveedge = default(Otri);
		Otri deadtriangle = default(Otri);
		bool noPoly = !mesh.behavior.Poly;
		Triangle dummytri = mesh.dummytri;
		startghost.Lprev(ref searchedge);
		searchedge.Sym();
		dummytri.neighbors[0] = searchedge;
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
			dissolveedge.Dissolve(dummytri);
			deadtriangle.Sym(ref dissolveedge);
			mesh.TriangleDealloc(deadtriangle.tri);
		}
		while (!dissolveedge.Equals(startghost));
		return hullsize;
	}
}
