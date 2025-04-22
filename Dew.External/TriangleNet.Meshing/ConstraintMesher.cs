using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Logging;
using TriangleNet.Meshing.Iterators;
using TriangleNet.Topology;

namespace TriangleNet.Meshing;

internal class ConstraintMesher
{
	private IPredicates predicates;

	private Mesh mesh;

	private Behavior behavior;

	private TriangleLocator locator;

	private List<Triangle> viri;

	private ILog<LogItem> logger;

	public ConstraintMesher(Mesh mesh, Configuration config)
	{
		this.mesh = mesh;
		predicates = config.Predicates();
		behavior = mesh.behavior;
		locator = mesh.locator;
		viri = new List<Triangle>();
		logger = Log.Instance;
	}

	public void Apply(IPolygon input, ConstraintOptions options)
	{
		behavior.Poly = input.Segments.Count > 0;
		if (options != null)
		{
			behavior.ConformingDelaunay = options.ConformingDelaunay;
			behavior.Convex = options.Convex;
			behavior.NoBisect = options.SegmentSplitting;
			if (behavior.ConformingDelaunay)
			{
				behavior.Quality = true;
			}
		}
		behavior.useRegions = input.Regions.Count > 0;
		mesh.infvertex1 = null;
		mesh.infvertex2 = null;
		mesh.infvertex3 = null;
		if (behavior.useSegments)
		{
			mesh.checksegments = true;
			FormSkeleton(input);
		}
		if (behavior.Poly && mesh.triangles.Count > 0)
		{
			mesh.holes.AddRange(input.Holes);
			mesh.regions.AddRange(input.Regions);
			CarveHoles();
		}
	}

	private void CarveHoles()
	{
		Otri searchtri = default(Otri);
		Triangle[] regionTris = null;
		Triangle dummytri = mesh.dummytri;
		if (!mesh.behavior.Convex)
		{
			InfectHull();
		}
		if (!mesh.behavior.NoHoles)
		{
			foreach (Point hole in mesh.holes)
			{
				if (mesh.bounds.Contains(hole))
				{
					searchtri.tri = dummytri;
					searchtri.orient = 0;
					searchtri.Sym();
					Vertex searchorg = searchtri.Org();
					Vertex searchdest = searchtri.Dest();
					if (predicates.CounterClockwise(searchorg, searchdest, hole) > 0.0 && mesh.locator.Locate(hole, ref searchtri) != LocateResult.Outside && !searchtri.IsInfected())
					{
						searchtri.Infect();
						viri.Add(searchtri.tri);
					}
				}
			}
		}
		if (mesh.regions.Count > 0)
		{
			int i = 0;
			regionTris = new Triangle[mesh.regions.Count];
			foreach (RegionPointer region in mesh.regions)
			{
				regionTris[i] = dummytri;
				if (mesh.bounds.Contains(region.point))
				{
					searchtri.tri = dummytri;
					searchtri.orient = 0;
					searchtri.Sym();
					Vertex searchorg = searchtri.Org();
					Vertex searchdest = searchtri.Dest();
					if (predicates.CounterClockwise(searchorg, searchdest, region.point) > 0.0 && mesh.locator.Locate(region.point, ref searchtri) != LocateResult.Outside && !searchtri.IsInfected())
					{
						regionTris[i] = searchtri.tri;
						regionTris[i].label = region.id;
						regionTris[i].area = region.area;
					}
				}
				i++;
			}
		}
		if (viri.Count > 0)
		{
			Plague();
		}
		if (regionTris != null)
		{
			RegionIterator iterator = new RegionIterator(mesh);
			for (int j = 0; j < regionTris.Length; j++)
			{
				if (regionTris[j].id != -1 && !Otri.IsDead(regionTris[j]))
				{
					iterator.Process(regionTris[j]);
				}
			}
		}
		viri.Clear();
	}

	private void FormSkeleton(IPolygon input)
	{
		mesh.insegments = 0;
		if (behavior.Poly)
		{
			if (mesh.triangles.Count == 0)
			{
				return;
			}
			if (input.Segments.Count > 0)
			{
				mesh.MakeVertexMap();
			}
			foreach (ISegment seg in input.Segments)
			{
				mesh.insegments++;
				Vertex p = seg.GetVertex(0);
				Vertex q = seg.GetVertex(1);
				if (p.x == q.x && p.y == q.y)
				{
					if (Log.Verbose)
					{
						logger.Warning("Endpoints of segment (IDs " + p.id + "/" + q.id + ") are coincident.", "Mesh.FormSkeleton()");
					}
				}
				else
				{
					InsertSegment(p, q, seg.Label);
				}
			}
		}
		if (behavior.Convex || !behavior.Poly)
		{
			MarkHull();
		}
	}

	private void InfectHull()
	{
		Otri hulltri = default(Otri);
		Otri nexttri = default(Otri);
		Otri starttri = default(Otri);
		Osub hullsubseg = default(Osub);
		Triangle dummytri = mesh.dummytri;
		hulltri.tri = dummytri;
		hulltri.orient = 0;
		hulltri.Sym();
		hulltri.Copy(ref starttri);
		do
		{
			if (!hulltri.IsInfected())
			{
				hulltri.Pivot(ref hullsubseg);
				if (hullsubseg.seg.hash == -1)
				{
					if (!hulltri.IsInfected())
					{
						hulltri.Infect();
						viri.Add(hulltri.tri);
					}
				}
				else if (hullsubseg.seg.boundary == 0)
				{
					hullsubseg.seg.boundary = 1;
					Vertex horg = hulltri.Org();
					Vertex hdest = hulltri.Dest();
					if (horg.label == 0)
					{
						horg.label = 1;
					}
					if (hdest.label == 0)
					{
						hdest.label = 1;
					}
				}
			}
			hulltri.Lnext();
			hulltri.Oprev(ref nexttri);
			while (nexttri.tri.id != -1)
			{
				nexttri.Copy(ref hulltri);
				hulltri.Oprev(ref nexttri);
			}
		}
		while (!hulltri.Equals(starttri));
	}

	private void Plague()
	{
		Otri testtri = default(Otri);
		Otri neighbor = default(Otri);
		Osub neighborsubseg = default(Osub);
		SubSegment dummysub = mesh.dummysub;
		Triangle dummytri = mesh.dummytri;
		for (int i = 0; i < viri.Count; i++)
		{
			testtri.tri = viri[i];
			testtri.Uninfect();
			testtri.orient = 0;
			while (testtri.orient < 3)
			{
				testtri.Sym(ref neighbor);
				testtri.Pivot(ref neighborsubseg);
				if (neighbor.tri.id == -1 || neighbor.IsInfected())
				{
					if (neighborsubseg.seg.hash != -1)
					{
						mesh.SubsegDealloc(neighborsubseg.seg);
						if (neighbor.tri.id != -1)
						{
							neighbor.Uninfect();
							neighbor.SegDissolve(dummysub);
							neighbor.Infect();
						}
					}
				}
				else if (neighborsubseg.seg.hash == -1)
				{
					neighbor.Infect();
					viri.Add(neighbor.tri);
				}
				else
				{
					neighborsubseg.TriDissolve(dummytri);
					if (neighborsubseg.seg.boundary == 0)
					{
						neighborsubseg.seg.boundary = 1;
					}
					Vertex norg = neighbor.Org();
					Vertex ndest = neighbor.Dest();
					if (norg.label == 0)
					{
						norg.label = 1;
					}
					if (ndest.label == 0)
					{
						ndest.label = 1;
					}
				}
				testtri.orient++;
			}
			testtri.Infect();
		}
		foreach (Triangle virus in viri)
		{
			testtri.tri = virus;
			testtri.orient = 0;
			while (testtri.orient < 3)
			{
				Vertex testvertex = testtri.Org();
				if (testvertex != null)
				{
					bool killorg = true;
					testtri.SetOrg(null);
					testtri.Onext(ref neighbor);
					while (neighbor.tri.id != -1 && !neighbor.Equals(testtri))
					{
						if (neighbor.IsInfected())
						{
							neighbor.SetOrg(null);
						}
						else
						{
							killorg = false;
						}
						neighbor.Onext();
					}
					if (neighbor.tri.id == -1)
					{
						testtri.Oprev(ref neighbor);
						while (neighbor.tri.id != -1)
						{
							if (neighbor.IsInfected())
							{
								neighbor.SetOrg(null);
							}
							else
							{
								killorg = false;
							}
							neighbor.Oprev();
						}
					}
					if (killorg)
					{
						testvertex.type = VertexType.UndeadVertex;
						mesh.undeads++;
					}
				}
				testtri.orient++;
			}
			testtri.orient = 0;
			while (testtri.orient < 3)
			{
				testtri.Sym(ref neighbor);
				if (neighbor.tri.id == -1)
				{
					mesh.hullsize--;
				}
				else
				{
					neighbor.Dissolve(dummytri);
					mesh.hullsize++;
				}
				testtri.orient++;
			}
			mesh.TriangleDealloc(testtri.tri);
		}
		viri.Clear();
	}

	private FindDirectionResult FindDirection(ref Otri searchtri, Vertex searchpoint)
	{
		Otri checktri = default(Otri);
		Vertex startvertex = searchtri.Org();
		Vertex rightvertex = searchtri.Dest();
		Vertex leftvertex = searchtri.Apex();
		double leftccw = predicates.CounterClockwise(searchpoint, startvertex, leftvertex);
		bool leftflag = leftccw > 0.0;
		double rightccw = predicates.CounterClockwise(startvertex, searchpoint, rightvertex);
		bool rightflag = rightccw > 0.0;
		if (leftflag && rightflag)
		{
			searchtri.Onext(ref checktri);
			if (checktri.tri.id == -1)
			{
				leftflag = false;
			}
			else
			{
				rightflag = false;
			}
		}
		while (leftflag)
		{
			searchtri.Onext();
			if (searchtri.tri.id == -1)
			{
				logger.Error("Unable to find a triangle on path.", "Mesh.FindDirection().1");
				throw new Exception("Unable to find a triangle on path.");
			}
			leftvertex = searchtri.Apex();
			rightccw = leftccw;
			leftccw = predicates.CounterClockwise(searchpoint, startvertex, leftvertex);
			leftflag = leftccw > 0.0;
		}
		while (rightflag)
		{
			searchtri.Oprev();
			if (searchtri.tri.id == -1)
			{
				logger.Error("Unable to find a triangle on path.", "Mesh.FindDirection().2");
				throw new Exception("Unable to find a triangle on path.");
			}
			rightvertex = searchtri.Dest();
			leftccw = rightccw;
			rightccw = predicates.CounterClockwise(startvertex, searchpoint, rightvertex);
			rightflag = rightccw > 0.0;
		}
		if (leftccw == 0.0)
		{
			return FindDirectionResult.Leftcollinear;
		}
		if (rightccw == 0.0)
		{
			return FindDirectionResult.Rightcollinear;
		}
		return FindDirectionResult.Within;
	}

	private void SegmentIntersection(ref Otri splittri, ref Osub splitsubseg, Vertex endpoint2)
	{
		Osub opposubseg = default(Osub);
		SubSegment dummysub = mesh.dummysub;
		Vertex endpoint3 = splittri.Apex();
		Vertex torg = splittri.Org();
		Vertex tdest = splittri.Dest();
		double tx = tdest.x - torg.x;
		double num = tdest.y - torg.y;
		double ex = endpoint2.x - endpoint3.x;
		double ey = endpoint2.y - endpoint3.y;
		double etx = torg.x - endpoint2.x;
		double ety = torg.y - endpoint2.y;
		double denom = num * ex - tx * ey;
		if (denom == 0.0)
		{
			logger.Error("Attempt to find intersection of parallel segments.", "Mesh.SegmentIntersection()");
			throw new Exception("Attempt to find intersection of parallel segments.");
		}
		double split = (ey * etx - ex * ety) / denom;
		Vertex newvertex = new Vertex(torg.x + split * (tdest.x - torg.x), torg.y + split * (tdest.y - torg.y), splitsubseg.seg.boundary);
		newvertex.hash = mesh.hash_vtx++;
		newvertex.id = newvertex.hash;
		newvertex.z = torg.z + split * (tdest.z - torg.z);
		mesh.vertices.Add(newvertex.hash, newvertex);
		if (mesh.InsertVertex(newvertex, ref splittri, ref splitsubseg, segmentflaws: false, triflaws: false) != 0)
		{
			logger.Error("Failure to split a segment.", "Mesh.SegmentIntersection()");
			throw new Exception("Failure to split a segment.");
		}
		newvertex.tri = splittri;
		if (mesh.steinerleft > 0)
		{
			mesh.steinerleft--;
		}
		splitsubseg.Sym();
		splitsubseg.Pivot(ref opposubseg);
		splitsubseg.Dissolve(dummysub);
		opposubseg.Dissolve(dummysub);
		do
		{
			splitsubseg.SetSegOrg(newvertex);
			splitsubseg.Next();
		}
		while (splitsubseg.seg.hash != -1);
		do
		{
			opposubseg.SetSegOrg(newvertex);
			opposubseg.Next();
		}
		while (opposubseg.seg.hash != -1);
		FindDirection(ref splittri, endpoint3);
		Vertex rightvertex = splittri.Dest();
		Vertex leftvertex = splittri.Apex();
		if (leftvertex.x == endpoint3.x && leftvertex.y == endpoint3.y)
		{
			splittri.Onext();
		}
		else if (rightvertex.x != endpoint3.x || rightvertex.y != endpoint3.y)
		{
			logger.Error("Topological inconsistency after splitting a segment.", "Mesh.SegmentIntersection()");
			throw new Exception("Topological inconsistency after splitting a segment.");
		}
	}

	private bool ScoutSegment(ref Otri searchtri, Vertex endpoint2, int newmark)
	{
		Otri crosstri = default(Otri);
		Osub crosssubseg = default(Osub);
		FindDirectionResult collinear = FindDirection(ref searchtri, endpoint2);
		Vertex rightvertex = searchtri.Dest();
		Vertex leftvertex = searchtri.Apex();
		if ((leftvertex.x == endpoint2.x && leftvertex.y == endpoint2.y) || (rightvertex.x == endpoint2.x && rightvertex.y == endpoint2.y))
		{
			if (leftvertex.x == endpoint2.x && leftvertex.y == endpoint2.y)
			{
				searchtri.Lprev();
			}
			mesh.InsertSubseg(ref searchtri, newmark);
			return true;
		}
		switch (collinear)
		{
		case FindDirectionResult.Leftcollinear:
			searchtri.Lprev();
			mesh.InsertSubseg(ref searchtri, newmark);
			return ScoutSegment(ref searchtri, endpoint2, newmark);
		case FindDirectionResult.Rightcollinear:
			mesh.InsertSubseg(ref searchtri, newmark);
			searchtri.Lnext();
			return ScoutSegment(ref searchtri, endpoint2, newmark);
		default:
			searchtri.Lnext(ref crosstri);
			crosstri.Pivot(ref crosssubseg);
			if (crosssubseg.seg.hash == -1)
			{
				return false;
			}
			SegmentIntersection(ref crosstri, ref crosssubseg, endpoint2);
			crosstri.Copy(ref searchtri);
			mesh.InsertSubseg(ref searchtri, newmark);
			return ScoutSegment(ref searchtri, endpoint2, newmark);
		}
	}

	private void DelaunayFixup(ref Otri fixuptri, bool leftside)
	{
		Otri neartri = default(Otri);
		Otri fartri = default(Otri);
		Osub faredge = default(Osub);
		fixuptri.Lnext(ref neartri);
		neartri.Sym(ref fartri);
		if (fartri.tri.id == -1)
		{
			return;
		}
		neartri.Pivot(ref faredge);
		if (faredge.seg.hash != -1)
		{
			return;
		}
		Vertex nearvertex = neartri.Apex();
		Vertex leftvertex = neartri.Org();
		Vertex rightvertex = neartri.Dest();
		Vertex farvertex = fartri.Apex();
		if (leftside)
		{
			if (predicates.CounterClockwise(nearvertex, leftvertex, farvertex) <= 0.0)
			{
				return;
			}
		}
		else if (predicates.CounterClockwise(farvertex, rightvertex, nearvertex) <= 0.0)
		{
			return;
		}
		if (!(predicates.CounterClockwise(rightvertex, leftvertex, farvertex) > 0.0) || !(predicates.InCircle(leftvertex, farvertex, rightvertex, nearvertex) <= 0.0))
		{
			mesh.Flip(ref neartri);
			fixuptri.Lprev();
			DelaunayFixup(ref fixuptri, leftside);
			DelaunayFixup(ref fartri, leftside);
		}
	}

	private void ConstrainedEdge(ref Otri starttri, Vertex endpoint2, int newmark)
	{
		Otri fixuptri = default(Otri);
		Otri fixuptri2 = default(Otri);
		Osub crosssubseg = default(Osub);
		Vertex endpoint3 = starttri.Org();
		starttri.Lnext(ref fixuptri);
		mesh.Flip(ref fixuptri);
		bool collision = false;
		bool done = false;
		do
		{
			Vertex farvertex = fixuptri.Org();
			if (farvertex.x == endpoint2.x && farvertex.y == endpoint2.y)
			{
				fixuptri.Oprev(ref fixuptri2);
				DelaunayFixup(ref fixuptri, leftside: false);
				DelaunayFixup(ref fixuptri2, leftside: true);
				done = true;
				continue;
			}
			double area = predicates.CounterClockwise(endpoint3, endpoint2, farvertex);
			if (area == 0.0)
			{
				collision = true;
				fixuptri.Oprev(ref fixuptri2);
				DelaunayFixup(ref fixuptri, leftside: false);
				DelaunayFixup(ref fixuptri2, leftside: true);
				done = true;
				continue;
			}
			if (area > 0.0)
			{
				fixuptri.Oprev(ref fixuptri2);
				DelaunayFixup(ref fixuptri2, leftside: true);
				fixuptri.Lprev();
			}
			else
			{
				DelaunayFixup(ref fixuptri, leftside: false);
				fixuptri.Oprev();
			}
			fixuptri.Pivot(ref crosssubseg);
			if (crosssubseg.seg.hash == -1)
			{
				mesh.Flip(ref fixuptri);
				continue;
			}
			collision = true;
			SegmentIntersection(ref fixuptri, ref crosssubseg, endpoint2);
			done = true;
		}
		while (!done);
		mesh.InsertSubseg(ref fixuptri, newmark);
		if (collision && !ScoutSegment(ref fixuptri, endpoint2, newmark))
		{
			ConstrainedEdge(ref fixuptri, endpoint2, newmark);
		}
	}

	private void InsertSegment(Vertex endpoint1, Vertex endpoint2, int newmark)
	{
		Otri searchtri1 = default(Otri);
		Otri searchtri2 = default(Otri);
		Vertex checkvertex = null;
		Triangle dummytri = mesh.dummytri;
		searchtri1 = endpoint1.tri;
		if (searchtri1.tri != null)
		{
			checkvertex = searchtri1.Org();
		}
		if (checkvertex != endpoint1)
		{
			searchtri1.tri = dummytri;
			searchtri1.orient = 0;
			searchtri1.Sym();
			if (locator.Locate(endpoint1, ref searchtri1) != LocateResult.OnVertex)
			{
				logger.Error("Unable to locate PSLG vertex in triangulation.", "Mesh.InsertSegment().1");
				throw new Exception("Unable to locate PSLG vertex in triangulation.");
			}
		}
		locator.Update(ref searchtri1);
		if (ScoutSegment(ref searchtri1, endpoint2, newmark))
		{
			return;
		}
		endpoint1 = searchtri1.Org();
		checkvertex = null;
		searchtri2 = endpoint2.tri;
		if (searchtri2.tri != null)
		{
			checkvertex = searchtri2.Org();
		}
		if (checkvertex != endpoint2)
		{
			searchtri2.tri = dummytri;
			searchtri2.orient = 0;
			searchtri2.Sym();
			if (locator.Locate(endpoint2, ref searchtri2) != LocateResult.OnVertex)
			{
				logger.Error("Unable to locate PSLG vertex in triangulation.", "Mesh.InsertSegment().2");
				throw new Exception("Unable to locate PSLG vertex in triangulation.");
			}
		}
		locator.Update(ref searchtri2);
		if (!ScoutSegment(ref searchtri2, endpoint1, newmark))
		{
			endpoint2 = searchtri2.Org();
			ConstrainedEdge(ref searchtri1, endpoint2, newmark);
		}
	}

	private void MarkHull()
	{
		Otri hulltri = default(Otri);
		Otri nexttri = default(Otri);
		Otri starttri = default(Otri);
		hulltri.tri = mesh.dummytri;
		hulltri.orient = 0;
		hulltri.Sym();
		hulltri.Copy(ref starttri);
		do
		{
			mesh.InsertSubseg(ref hulltri, 1);
			hulltri.Lnext();
			hulltri.Oprev(ref nexttri);
			while (nexttri.tri.id != -1)
			{
				nexttri.Copy(ref hulltri);
				hulltri.Oprev(ref nexttri);
			}
		}
		while (!hulltri.Equals(starttri));
	}
}
