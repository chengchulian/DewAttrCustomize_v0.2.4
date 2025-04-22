using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Logging;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Data;
using TriangleNet.Meshing.Iterators;
using TriangleNet.Tools;
using TriangleNet.Topology;

namespace TriangleNet;

public class Mesh : IMesh
{
	private IPredicates predicates;

	private ILog<LogItem> logger;

	private QualityMesher qualityMesher;

	private Stack<Otri> flipstack;

	internal TrianglePool triangles;

	internal Dictionary<int, SubSegment> subsegs;

	internal Dictionary<int, Vertex> vertices;

	internal int hash_vtx;

	internal int hash_seg;

	internal int hash_tri;

	internal List<Point> holes;

	internal List<RegionPointer> regions;

	internal Rectangle bounds;

	internal int invertices;

	internal int insegments;

	internal int undeads;

	internal int mesh_dim;

	internal int nextras;

	internal int hullsize;

	internal int steinerleft;

	internal bool checksegments;

	internal bool checkquality;

	internal Vertex infvertex1;

	internal Vertex infvertex2;

	internal Vertex infvertex3;

	internal TriangleLocator locator;

	internal Behavior behavior;

	internal NodeNumbering numbering;

	internal const int DUMMY = -1;

	internal Triangle dummytri;

	internal SubSegment dummysub;

	public Rectangle Bounds => bounds;

	public ICollection<Vertex> Vertices => vertices.Values;

	public IList<Point> Holes => holes;

	public ICollection<Triangle> Triangles => triangles;

	public ICollection<SubSegment> Segments => subsegs.Values;

	public IEnumerable<Edge> Edges
	{
		get
		{
			EdgeIterator e = new EdgeIterator(this);
			while (e.MoveNext())
			{
				yield return e.Current;
			}
		}
	}

	public int NumberOfInputPoints => invertices;

	public int NumberOfEdges => (3 * triangles.Count + hullsize) / 2;

	public bool IsPolygon => insegments > 0;

	public NodeNumbering CurrentNumbering => numbering;

	private void Initialize()
	{
		dummysub = new SubSegment();
		dummysub.hash = -1;
		dummysub.subsegs[0].seg = dummysub;
		dummysub.subsegs[1].seg = dummysub;
		dummytri = new Triangle();
		dummytri.hash = (dummytri.id = -1);
		dummytri.neighbors[0].tri = dummytri;
		dummytri.neighbors[1].tri = dummytri;
		dummytri.neighbors[2].tri = dummytri;
		dummytri.subsegs[0].seg = dummysub;
		dummytri.subsegs[1].seg = dummysub;
		dummytri.subsegs[2].seg = dummysub;
	}

	public Mesh(Configuration config)
	{
		Initialize();
		logger = Log.Instance;
		behavior = new Behavior();
		vertices = new Dictionary<int, Vertex>();
		subsegs = new Dictionary<int, SubSegment>();
		triangles = config.TrianglePool();
		flipstack = new Stack<Otri>();
		holes = new List<Point>();
		regions = new List<RegionPointer>();
		steinerleft = -1;
		predicates = config.Predicates();
		locator = new TriangleLocator(this, predicates);
	}

	public void Refine(QualityOptions quality, bool delaunay = false)
	{
		invertices = vertices.Count;
		if (behavior.Poly)
		{
			insegments = (behavior.useSegments ? subsegs.Count : hullsize);
		}
		Reset();
		if (qualityMesher == null)
		{
			qualityMesher = new QualityMesher(this, new Configuration());
		}
		qualityMesher.Apply(quality, delaunay);
	}

	public void Renumber()
	{
		Renumber(NodeNumbering.Linear);
	}

	public void Renumber(NodeNumbering num)
	{
		if (num == numbering)
		{
			return;
		}
		int id;
		switch (num)
		{
		case NodeNumbering.Linear:
			id = 0;
			foreach (Vertex value in vertices.Values)
			{
				value.id = id++;
			}
			break;
		case NodeNumbering.CuthillMcKee:
		{
			int[] iperm = new CuthillMcKee().Renumber(this);
			foreach (Vertex node in vertices.Values)
			{
				node.id = iperm[node.id];
			}
			break;
		}
		}
		numbering = num;
		id = 0;
		foreach (Triangle triangle in triangles)
		{
			triangle.id = id++;
		}
	}

	internal void SetQualityMesher(QualityMesher qmesher)
	{
		qualityMesher = qmesher;
	}

	internal void CopyTo(Mesh target)
	{
		target.vertices = vertices;
		target.triangles = triangles;
		target.subsegs = subsegs;
		target.holes = holes;
		target.regions = regions;
		target.hash_vtx = hash_vtx;
		target.hash_seg = hash_seg;
		target.hash_tri = hash_tri;
		target.numbering = numbering;
		target.hullsize = hullsize;
	}

	private void ResetData()
	{
		vertices.Clear();
		triangles.Restart();
		subsegs.Clear();
		holes.Clear();
		regions.Clear();
		hash_vtx = 0;
		hash_seg = 0;
		hash_tri = 0;
		flipstack.Clear();
		hullsize = 0;
		Reset();
		locator.Reset();
	}

	private void Reset()
	{
		numbering = NodeNumbering.None;
		undeads = 0;
		checksegments = false;
		checkquality = false;
		Statistic.InCircleCount = 0L;
		Statistic.CounterClockwiseCount = 0L;
		Statistic.InCircleAdaptCount = 0L;
		Statistic.CounterClockwiseAdaptCount = 0L;
		Statistic.Orient3dCount = 0L;
		Statistic.HyperbolaCount = 0L;
		Statistic.CircleTopCount = 0L;
		Statistic.CircumcenterCount = 0L;
	}

	internal void TransferNodes(IList<Vertex> points)
	{
		invertices = points.Count;
		mesh_dim = 2;
		bounds = new Rectangle();
		if (invertices < 3)
		{
			logger.Error("Input must have at least three input vertices.", "Mesh.TransferNodes()");
			throw new Exception("Input must have at least three input vertices.");
		}
		Vertex vertex = points[0];
		int test = nextras;
		nextras = test;
		bool userId = vertex.id != points[1].id;
		foreach (Vertex p in points)
		{
			if (userId)
			{
				p.hash = p.id;
				hash_vtx = Math.Max(p.hash + 1, hash_vtx);
			}
			else
			{
				p.hash = (p.id = hash_vtx++);
			}
			vertices.Add(p.hash, p);
			bounds.Expand(p);
		}
	}

	internal void MakeVertexMap()
	{
		Otri tri = default(Otri);
		foreach (Triangle t in triangles)
		{
			tri.tri = t;
			tri.orient = 0;
			while (tri.orient < 3)
			{
				tri.Org().tri = tri;
				tri.orient++;
			}
		}
	}

	internal void MakeTriangle(ref Otri newotri)
	{
		Triangle tri = triangles.Get();
		tri.subsegs[0].seg = dummysub;
		tri.subsegs[1].seg = dummysub;
		tri.subsegs[2].seg = dummysub;
		tri.neighbors[0].tri = dummytri;
		tri.neighbors[1].tri = dummytri;
		tri.neighbors[2].tri = dummytri;
		newotri.tri = tri;
		newotri.orient = 0;
	}

	internal void MakeSegment(ref Osub newsubseg)
	{
		SubSegment seg = new SubSegment();
		seg.hash = hash_seg++;
		seg.subsegs[0].seg = dummysub;
		seg.subsegs[1].seg = dummysub;
		seg.triangles[0].tri = dummytri;
		seg.triangles[1].tri = dummytri;
		newsubseg.seg = seg;
		newsubseg.orient = 0;
		subsegs.Add(seg.hash, seg);
	}

	internal InsertVertexResult InsertVertex(Vertex newvertex, ref Otri searchtri, ref Osub splitseg, bool segmentflaws, bool triflaws)
	{
		Otri horiz = default(Otri);
		Otri top = default(Otri);
		Otri botleft = default(Otri);
		Otri botright = default(Otri);
		Otri topleft = default(Otri);
		Otri topright = default(Otri);
		Otri newbotleft = default(Otri);
		Otri newbotright = default(Otri);
		Otri newtopright = default(Otri);
		Otri botlcasing = default(Otri);
		Otri botrcasing = default(Otri);
		Otri toplcasing = default(Otri);
		Otri toprcasing = default(Otri);
		Otri testtri = default(Otri);
		Osub botlsubseg = default(Osub);
		Osub botrsubseg = default(Osub);
		Osub toplsubseg = default(Osub);
		Osub toprsubseg = default(Osub);
		Osub brokensubseg = default(Osub);
		Osub checksubseg = default(Osub);
		Osub rightsubseg = default(Osub);
		Osub newsubseg = default(Osub);
		LocateResult intersect;
		if (splitseg.seg == null)
		{
			if (searchtri.tri.id == -1)
			{
				horiz.tri = dummytri;
				horiz.orient = 0;
				horiz.Sym();
				intersect = locator.Locate(newvertex, ref horiz);
			}
			else
			{
				searchtri.Copy(ref horiz);
				intersect = locator.PreciseLocate(newvertex, ref horiz, stopatsubsegment: true);
			}
		}
		else
		{
			searchtri.Copy(ref horiz);
			intersect = LocateResult.OnEdge;
		}
		Vertex botvertex;
		Vertex rightvertex;
		Vertex leftvertex;
		switch (intersect)
		{
		case LocateResult.OnVertex:
			horiz.Copy(ref searchtri);
			locator.Update(ref horiz);
			return InsertVertexResult.Duplicate;
		case LocateResult.OnEdge:
		case LocateResult.Outside:
		{
			if (checksegments && splitseg.seg == null)
			{
				horiz.Pivot(ref brokensubseg);
				if (brokensubseg.seg.hash != -1)
				{
					if (segmentflaws)
					{
						bool enq = behavior.NoBisect != 2;
						if (enq && behavior.NoBisect == 1)
						{
							horiz.Sym(ref testtri);
							enq = testtri.tri.id != -1;
						}
						if (enq)
						{
							BadSubseg encroached = new BadSubseg();
							encroached.subseg = brokensubseg;
							encroached.org = brokensubseg.Org();
							encroached.dest = brokensubseg.Dest();
							qualityMesher.AddBadSubseg(encroached);
						}
					}
					horiz.Copy(ref searchtri);
					locator.Update(ref horiz);
					return InsertVertexResult.Violating;
				}
			}
			horiz.Lprev(ref botright);
			botright.Sym(ref botrcasing);
			horiz.Sym(ref topright);
			bool mirrorflag = topright.tri.id != -1;
			if (mirrorflag)
			{
				topright.Lnext();
				topright.Sym(ref toprcasing);
				MakeTriangle(ref newtopright);
			}
			else
			{
				hullsize++;
			}
			MakeTriangle(ref newbotright);
			rightvertex = horiz.Org();
			leftvertex = horiz.Dest();
			botvertex = horiz.Apex();
			newbotright.SetOrg(botvertex);
			newbotright.SetDest(rightvertex);
			newbotright.SetApex(newvertex);
			horiz.SetOrg(newvertex);
			newbotright.tri.label = botright.tri.label;
			if (behavior.VarArea)
			{
				newbotright.tri.area = botright.tri.area;
			}
			if (mirrorflag)
			{
				Vertex topvertex = topright.Dest();
				newtopright.SetOrg(rightvertex);
				newtopright.SetDest(topvertex);
				newtopright.SetApex(newvertex);
				topright.SetOrg(newvertex);
				newtopright.tri.label = topright.tri.label;
				if (behavior.VarArea)
				{
					newtopright.tri.area = topright.tri.area;
				}
			}
			if (checksegments)
			{
				botright.Pivot(ref botrsubseg);
				if (botrsubseg.seg.hash != -1)
				{
					botright.SegDissolve(dummysub);
					newbotright.SegBond(ref botrsubseg);
				}
				if (mirrorflag)
				{
					topright.Pivot(ref toprsubseg);
					if (toprsubseg.seg.hash != -1)
					{
						topright.SegDissolve(dummysub);
						newtopright.SegBond(ref toprsubseg);
					}
				}
			}
			newbotright.Bond(ref botrcasing);
			newbotright.Lprev();
			newbotright.Bond(ref botright);
			newbotright.Lprev();
			if (mirrorflag)
			{
				newtopright.Bond(ref toprcasing);
				newtopright.Lnext();
				newtopright.Bond(ref topright);
				newtopright.Lnext();
				newtopright.Bond(ref newbotright);
			}
			if (splitseg.seg != null)
			{
				splitseg.SetDest(newvertex);
				Vertex segmentorg = splitseg.SegOrg();
				Vertex segmentdest = splitseg.SegDest();
				splitseg.Sym();
				splitseg.Pivot(ref rightsubseg);
				InsertSubseg(ref newbotright, splitseg.seg.boundary);
				newbotright.Pivot(ref newsubseg);
				newsubseg.SetSegOrg(segmentorg);
				newsubseg.SetSegDest(segmentdest);
				splitseg.Bond(ref newsubseg);
				newsubseg.Sym();
				newsubseg.Bond(ref rightsubseg);
				splitseg.Sym();
				if (newvertex.label == 0)
				{
					newvertex.label = splitseg.seg.boundary;
				}
			}
			if (checkquality)
			{
				flipstack.Clear();
				flipstack.Push(default(Otri));
				flipstack.Push(horiz);
			}
			horiz.Lnext();
			break;
		}
		default:
			horiz.Lnext(ref botleft);
			horiz.Lprev(ref botright);
			botleft.Sym(ref botlcasing);
			botright.Sym(ref botrcasing);
			MakeTriangle(ref newbotleft);
			MakeTriangle(ref newbotright);
			rightvertex = horiz.Org();
			leftvertex = horiz.Dest();
			botvertex = horiz.Apex();
			newbotleft.SetOrg(leftvertex);
			newbotleft.SetDest(botvertex);
			newbotleft.SetApex(newvertex);
			newbotright.SetOrg(botvertex);
			newbotright.SetDest(rightvertex);
			newbotright.SetApex(newvertex);
			horiz.SetApex(newvertex);
			newbotleft.tri.label = horiz.tri.label;
			newbotright.tri.label = horiz.tri.label;
			if (behavior.VarArea)
			{
				double area = horiz.tri.area;
				newbotleft.tri.area = area;
				newbotright.tri.area = area;
			}
			if (checksegments)
			{
				botleft.Pivot(ref botlsubseg);
				if (botlsubseg.seg.hash != -1)
				{
					botleft.SegDissolve(dummysub);
					newbotleft.SegBond(ref botlsubseg);
				}
				botright.Pivot(ref botrsubseg);
				if (botrsubseg.seg.hash != -1)
				{
					botright.SegDissolve(dummysub);
					newbotright.SegBond(ref botrsubseg);
				}
			}
			newbotleft.Bond(ref botlcasing);
			newbotright.Bond(ref botrcasing);
			newbotleft.Lnext();
			newbotright.Lprev();
			newbotleft.Bond(ref newbotright);
			newbotleft.Lnext();
			botleft.Bond(ref newbotleft);
			newbotright.Lprev();
			botright.Bond(ref newbotright);
			if (checkquality)
			{
				flipstack.Clear();
				flipstack.Push(horiz);
			}
			break;
		}
		InsertVertexResult success = InsertVertexResult.Successful;
		if (newvertex.tri.tri != null)
		{
			newvertex.tri.SetOrg(rightvertex);
			newvertex.tri.SetDest(leftvertex);
			newvertex.tri.SetApex(botvertex);
		}
		Vertex first = horiz.Org();
		rightvertex = first;
		leftvertex = horiz.Dest();
		while (true)
		{
			bool doflip = true;
			if (checksegments)
			{
				horiz.Pivot(ref checksubseg);
				if (checksubseg.seg.hash != -1)
				{
					doflip = false;
					if (segmentflaws && qualityMesher.CheckSeg4Encroach(ref checksubseg) > 0)
					{
						success = InsertVertexResult.Encroaching;
					}
				}
			}
			if (doflip)
			{
				horiz.Sym(ref top);
				if (top.tri.id == -1)
				{
					doflip = false;
				}
				else
				{
					Vertex farvertex = top.Apex();
					doflip = ((!(leftvertex == infvertex1) && !(leftvertex == infvertex2) && !(leftvertex == infvertex3)) ? ((!(rightvertex == infvertex1) && !(rightvertex == infvertex2) && !(rightvertex == infvertex3)) ? (!(farvertex == infvertex1) && !(farvertex == infvertex2) && !(farvertex == infvertex3) && predicates.InCircle(leftvertex, newvertex, rightvertex, farvertex) > 0.0) : (predicates.CounterClockwise(farvertex, leftvertex, newvertex) > 0.0)) : (predicates.CounterClockwise(newvertex, rightvertex, farvertex) > 0.0));
					if (doflip)
					{
						top.Lprev(ref topleft);
						topleft.Sym(ref toplcasing);
						top.Lnext(ref topright);
						topright.Sym(ref toprcasing);
						horiz.Lnext(ref botleft);
						botleft.Sym(ref botlcasing);
						horiz.Lprev(ref botright);
						botright.Sym(ref botrcasing);
						topleft.Bond(ref botlcasing);
						botleft.Bond(ref botrcasing);
						botright.Bond(ref toprcasing);
						topright.Bond(ref toplcasing);
						if (checksegments)
						{
							topleft.Pivot(ref toplsubseg);
							botleft.Pivot(ref botlsubseg);
							botright.Pivot(ref botrsubseg);
							topright.Pivot(ref toprsubseg);
							if (toplsubseg.seg.hash == -1)
							{
								topright.SegDissolve(dummysub);
							}
							else
							{
								topright.SegBond(ref toplsubseg);
							}
							if (botlsubseg.seg.hash == -1)
							{
								topleft.SegDissolve(dummysub);
							}
							else
							{
								topleft.SegBond(ref botlsubseg);
							}
							if (botrsubseg.seg.hash == -1)
							{
								botleft.SegDissolve(dummysub);
							}
							else
							{
								botleft.SegBond(ref botrsubseg);
							}
							if (toprsubseg.seg.hash == -1)
							{
								botright.SegDissolve(dummysub);
							}
							else
							{
								botright.SegBond(ref toprsubseg);
							}
						}
						horiz.SetOrg(farvertex);
						horiz.SetDest(newvertex);
						horiz.SetApex(rightvertex);
						top.SetOrg(newvertex);
						top.SetDest(farvertex);
						top.SetApex(leftvertex);
						int region = Math.Min(top.tri.label, horiz.tri.label);
						top.tri.label = region;
						horiz.tri.label = region;
						if (behavior.VarArea)
						{
							double area = ((!(top.tri.area <= 0.0) && !(horiz.tri.area <= 0.0)) ? (0.5 * (top.tri.area + horiz.tri.area)) : (-1.0));
							top.tri.area = area;
							horiz.tri.area = area;
						}
						if (checkquality)
						{
							flipstack.Push(horiz);
						}
						horiz.Lprev();
						leftvertex = farvertex;
					}
				}
			}
			if (!doflip)
			{
				if (triflaws)
				{
					qualityMesher.TestTriangle(ref horiz);
				}
				horiz.Lnext();
				horiz.Sym(ref testtri);
				if (leftvertex == first || testtri.tri.id == -1)
				{
					break;
				}
				testtri.Lnext(ref horiz);
				rightvertex = leftvertex;
				leftvertex = horiz.Dest();
			}
		}
		horiz.Lnext(ref searchtri);
		Otri recenttri = default(Otri);
		horiz.Lnext(ref recenttri);
		locator.Update(ref recenttri);
		return success;
	}

	internal void InsertSubseg(ref Otri tri, int subsegmark)
	{
		Otri oppotri = default(Otri);
		Osub newsubseg = default(Osub);
		Vertex triorg = tri.Org();
		Vertex tridest = tri.Dest();
		if (triorg.label == 0)
		{
			triorg.label = subsegmark;
		}
		if (tridest.label == 0)
		{
			tridest.label = subsegmark;
		}
		tri.Pivot(ref newsubseg);
		if (newsubseg.seg.hash == -1)
		{
			MakeSegment(ref newsubseg);
			newsubseg.SetOrg(tridest);
			newsubseg.SetDest(triorg);
			newsubseg.SetSegOrg(tridest);
			newsubseg.SetSegDest(triorg);
			tri.SegBond(ref newsubseg);
			tri.Sym(ref oppotri);
			newsubseg.Sym();
			oppotri.SegBond(ref newsubseg);
			newsubseg.seg.boundary = subsegmark;
		}
		else if (newsubseg.seg.boundary == 0)
		{
			newsubseg.seg.boundary = subsegmark;
		}
	}

	internal void Flip(ref Otri flipedge)
	{
		Otri botleft = default(Otri);
		Otri botright = default(Otri);
		Otri topleft = default(Otri);
		Otri topright = default(Otri);
		Otri top = default(Otri);
		Otri botlcasing = default(Otri);
		Otri botrcasing = default(Otri);
		Otri toplcasing = default(Otri);
		Otri toprcasing = default(Otri);
		Osub botlsubseg = default(Osub);
		Osub botrsubseg = default(Osub);
		Osub toplsubseg = default(Osub);
		Osub toprsubseg = default(Osub);
		Vertex rightvertex = flipedge.Org();
		Vertex leftvertex = flipedge.Dest();
		Vertex botvertex = flipedge.Apex();
		flipedge.Sym(ref top);
		Vertex farvertex = top.Apex();
		top.Lprev(ref topleft);
		topleft.Sym(ref toplcasing);
		top.Lnext(ref topright);
		topright.Sym(ref toprcasing);
		flipedge.Lnext(ref botleft);
		botleft.Sym(ref botlcasing);
		flipedge.Lprev(ref botright);
		botright.Sym(ref botrcasing);
		topleft.Bond(ref botlcasing);
		botleft.Bond(ref botrcasing);
		botright.Bond(ref toprcasing);
		topright.Bond(ref toplcasing);
		if (checksegments)
		{
			topleft.Pivot(ref toplsubseg);
			botleft.Pivot(ref botlsubseg);
			botright.Pivot(ref botrsubseg);
			topright.Pivot(ref toprsubseg);
			if (toplsubseg.seg.hash == -1)
			{
				topright.SegDissolve(dummysub);
			}
			else
			{
				topright.SegBond(ref toplsubseg);
			}
			if (botlsubseg.seg.hash == -1)
			{
				topleft.SegDissolve(dummysub);
			}
			else
			{
				topleft.SegBond(ref botlsubseg);
			}
			if (botrsubseg.seg.hash == -1)
			{
				botleft.SegDissolve(dummysub);
			}
			else
			{
				botleft.SegBond(ref botrsubseg);
			}
			if (toprsubseg.seg.hash == -1)
			{
				botright.SegDissolve(dummysub);
			}
			else
			{
				botright.SegBond(ref toprsubseg);
			}
		}
		flipedge.SetOrg(farvertex);
		flipedge.SetDest(botvertex);
		flipedge.SetApex(rightvertex);
		top.SetOrg(botvertex);
		top.SetDest(farvertex);
		top.SetApex(leftvertex);
	}

	internal void Unflip(ref Otri flipedge)
	{
		Otri botleft = default(Otri);
		Otri botright = default(Otri);
		Otri topleft = default(Otri);
		Otri topright = default(Otri);
		Otri top = default(Otri);
		Otri botlcasing = default(Otri);
		Otri botrcasing = default(Otri);
		Otri toplcasing = default(Otri);
		Otri toprcasing = default(Otri);
		Osub botlsubseg = default(Osub);
		Osub botrsubseg = default(Osub);
		Osub toplsubseg = default(Osub);
		Osub toprsubseg = default(Osub);
		Vertex rightvertex = flipedge.Org();
		Vertex leftvertex = flipedge.Dest();
		Vertex botvertex = flipedge.Apex();
		flipedge.Sym(ref top);
		Vertex farvertex = top.Apex();
		top.Lprev(ref topleft);
		topleft.Sym(ref toplcasing);
		top.Lnext(ref topright);
		topright.Sym(ref toprcasing);
		flipedge.Lnext(ref botleft);
		botleft.Sym(ref botlcasing);
		flipedge.Lprev(ref botright);
		botright.Sym(ref botrcasing);
		topleft.Bond(ref toprcasing);
		botleft.Bond(ref toplcasing);
		botright.Bond(ref botlcasing);
		topright.Bond(ref botrcasing);
		if (checksegments)
		{
			topleft.Pivot(ref toplsubseg);
			botleft.Pivot(ref botlsubseg);
			botright.Pivot(ref botrsubseg);
			topright.Pivot(ref toprsubseg);
			if (toplsubseg.seg.hash == -1)
			{
				botleft.SegDissolve(dummysub);
			}
			else
			{
				botleft.SegBond(ref toplsubseg);
			}
			if (botlsubseg.seg.hash == -1)
			{
				botright.SegDissolve(dummysub);
			}
			else
			{
				botright.SegBond(ref botlsubseg);
			}
			if (botrsubseg.seg.hash == -1)
			{
				topright.SegDissolve(dummysub);
			}
			else
			{
				topright.SegBond(ref botrsubseg);
			}
			if (toprsubseg.seg.hash == -1)
			{
				topleft.SegDissolve(dummysub);
			}
			else
			{
				topleft.SegBond(ref toprsubseg);
			}
		}
		flipedge.SetOrg(botvertex);
		flipedge.SetDest(farvertex);
		flipedge.SetApex(leftvertex);
		top.SetOrg(farvertex);
		top.SetDest(botvertex);
		top.SetApex(rightvertex);
	}

	private void TriangulatePolygon(Otri firstedge, Otri lastedge, int edgecount, bool doflip, bool triflaws)
	{
		Otri testtri = default(Otri);
		Otri besttri = default(Otri);
		Otri tempedge = default(Otri);
		int bestnumber = 1;
		Vertex leftbasevertex = lastedge.Apex();
		Vertex rightbasevertex = firstedge.Dest();
		firstedge.Onext(ref besttri);
		Vertex bestvertex = besttri.Dest();
		besttri.Copy(ref testtri);
		for (int i = 2; i <= edgecount - 2; i++)
		{
			testtri.Onext();
			Vertex testvertex = testtri.Dest();
			if (predicates.InCircle(leftbasevertex, rightbasevertex, bestvertex, testvertex) > 0.0)
			{
				testtri.Copy(ref besttri);
				bestvertex = testvertex;
				bestnumber = i;
			}
		}
		if (bestnumber > 1)
		{
			besttri.Oprev(ref tempedge);
			TriangulatePolygon(firstedge, tempedge, bestnumber + 1, doflip: true, triflaws);
		}
		if (bestnumber < edgecount - 2)
		{
			besttri.Sym(ref tempedge);
			TriangulatePolygon(besttri, lastedge, edgecount - bestnumber, doflip: true, triflaws);
			tempedge.Sym(ref besttri);
		}
		if (doflip)
		{
			Flip(ref besttri);
			if (triflaws)
			{
				besttri.Sym(ref testtri);
				qualityMesher.TestTriangle(ref testtri);
			}
		}
		besttri.Copy(ref lastedge);
	}

	internal void DeleteVertex(ref Otri deltri)
	{
		Otri countingtri = default(Otri);
		Otri firstedge = default(Otri);
		Otri lastedge = default(Otri);
		Otri deltriright = default(Otri);
		Otri lefttri = default(Otri);
		Otri righttri = default(Otri);
		Otri leftcasing = default(Otri);
		Otri rightcasing = default(Otri);
		Osub leftsubseg = default(Osub);
		Osub rightsubseg = default(Osub);
		Vertex delvertex = deltri.Org();
		VertexDealloc(delvertex);
		deltri.Onext(ref countingtri);
		int edgecount = 1;
		while (!deltri.Equals(countingtri))
		{
			edgecount++;
			countingtri.Onext();
		}
		if (edgecount > 3)
		{
			deltri.Onext(ref firstedge);
			deltri.Oprev(ref lastedge);
			TriangulatePolygon(firstedge, lastedge, edgecount, doflip: false, behavior.NoBisect == 0);
		}
		deltri.Lprev(ref deltriright);
		deltri.Dnext(ref lefttri);
		lefttri.Sym(ref leftcasing);
		deltriright.Oprev(ref righttri);
		righttri.Sym(ref rightcasing);
		deltri.Bond(ref leftcasing);
		deltriright.Bond(ref rightcasing);
		lefttri.Pivot(ref leftsubseg);
		if (leftsubseg.seg.hash != -1)
		{
			deltri.SegBond(ref leftsubseg);
		}
		righttri.Pivot(ref rightsubseg);
		if (rightsubseg.seg.hash != -1)
		{
			deltriright.SegBond(ref rightsubseg);
		}
		Vertex neworg = lefttri.Org();
		deltri.SetOrg(neworg);
		if (behavior.NoBisect == 0)
		{
			qualityMesher.TestTriangle(ref deltri);
		}
		TriangleDealloc(lefttri.tri);
		TriangleDealloc(righttri.tri);
	}

	internal void UndoVertex()
	{
		Otri botleft = default(Otri);
		Otri botright = default(Otri);
		Otri topright = default(Otri);
		Otri botlcasing = default(Otri);
		Otri botrcasing = default(Otri);
		Otri toprcasing = default(Otri);
		Otri gluetri = default(Otri);
		Osub botlsubseg = default(Osub);
		Osub botrsubseg = default(Osub);
		Osub toprsubseg = default(Osub);
		while (flipstack.Count > 0)
		{
			Otri fliptri = flipstack.Pop();
			if (flipstack.Count == 0)
			{
				fliptri.Dprev(ref botleft);
				botleft.Lnext();
				fliptri.Onext(ref botright);
				botright.Lprev();
				botleft.Sym(ref botlcasing);
				botright.Sym(ref botrcasing);
				Vertex botvertex = botleft.Dest();
				fliptri.SetApex(botvertex);
				fliptri.Lnext();
				fliptri.Bond(ref botlcasing);
				botleft.Pivot(ref botlsubseg);
				fliptri.SegBond(ref botlsubseg);
				fliptri.Lnext();
				fliptri.Bond(ref botrcasing);
				botright.Pivot(ref botrsubseg);
				fliptri.SegBond(ref botrsubseg);
				TriangleDealloc(botleft.tri);
				TriangleDealloc(botright.tri);
			}
			else if (flipstack.Peek().tri == null)
			{
				fliptri.Lprev(ref gluetri);
				gluetri.Sym(ref botright);
				botright.Lnext();
				botright.Sym(ref botrcasing);
				Vertex rightvertex = botright.Dest();
				fliptri.SetOrg(rightvertex);
				gluetri.Bond(ref botrcasing);
				botright.Pivot(ref botrsubseg);
				gluetri.SegBond(ref botrsubseg);
				TriangleDealloc(botright.tri);
				fliptri.Sym(ref gluetri);
				if (gluetri.tri.id != -1)
				{
					gluetri.Lnext();
					gluetri.Dnext(ref topright);
					topright.Sym(ref toprcasing);
					gluetri.SetOrg(rightvertex);
					gluetri.Bond(ref toprcasing);
					topright.Pivot(ref toprsubseg);
					gluetri.SegBond(ref toprsubseg);
					TriangleDealloc(topright.tri);
				}
				flipstack.Clear();
			}
			else
			{
				Unflip(ref fliptri);
			}
		}
	}

	internal void TriangleDealloc(Triangle dyingtriangle)
	{
		Otri.Kill(dyingtriangle);
		triangles.Release(dyingtriangle);
	}

	internal void VertexDealloc(Vertex dyingvertex)
	{
		dyingvertex.type = VertexType.DeadVertex;
		vertices.Remove(dyingvertex.hash);
	}

	internal void SubsegDealloc(SubSegment dyingsubseg)
	{
		Osub.Kill(dyingsubseg);
		subsegs.Remove(dyingsubseg.hash);
	}
}
