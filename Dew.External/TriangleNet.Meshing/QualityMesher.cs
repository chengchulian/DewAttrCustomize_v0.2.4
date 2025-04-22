using System;
using System.Collections.Generic;
using TriangleNet.Geometry;
using TriangleNet.Logging;
using TriangleNet.Meshing.Data;
using TriangleNet.Tools;
using TriangleNet.Topology;

namespace TriangleNet.Meshing;

internal class QualityMesher
{
	private IPredicates predicates;

	private Queue<BadSubseg> badsubsegs;

	private BadTriQueue queue;

	private Mesh mesh;

	private Behavior behavior;

	private NewLocation newLocation;

	private ILog<LogItem> logger;

	private Triangle newvertex_tri;

	public QualityMesher(Mesh mesh, Configuration config)
	{
		logger = Log.Instance;
		badsubsegs = new Queue<BadSubseg>();
		queue = new BadTriQueue();
		this.mesh = mesh;
		predicates = config.Predicates();
		behavior = mesh.behavior;
		newLocation = new NewLocation(mesh, predicates);
		newvertex_tri = new Triangle();
	}

	public void Apply(QualityOptions quality, bool delaunay = false)
	{
		if (quality != null)
		{
			behavior.Quality = true;
			behavior.MinAngle = quality.MinimumAngle;
			behavior.MaxAngle = quality.MaximumAngle;
			behavior.MaxArea = quality.MaximumArea;
			behavior.UserTest = quality.UserTest;
			behavior.VarArea = quality.VariableArea;
			behavior.ConformingDelaunay = behavior.ConformingDelaunay || delaunay;
			mesh.steinerleft = ((quality.SteinerPoints == 0) ? (-1) : quality.SteinerPoints);
		}
		if (!behavior.Poly)
		{
			behavior.VarArea = false;
		}
		mesh.infvertex1 = null;
		mesh.infvertex2 = null;
		mesh.infvertex3 = null;
		if (behavior.useSegments)
		{
			mesh.checksegments = true;
		}
		if (behavior.Quality && mesh.triangles.Count > 0)
		{
			EnforceQuality();
		}
	}

	public void AddBadSubseg(BadSubseg badseg)
	{
		badsubsegs.Enqueue(badseg);
	}

	public int CheckSeg4Encroach(ref Osub testsubseg)
	{
		Otri neighbortri = default(Otri);
		Osub testsym = default(Osub);
		int encroached = 0;
		int sides = 0;
		Vertex eorg = testsubseg.Org();
		Vertex edest = testsubseg.Dest();
		testsubseg.Pivot(ref neighbortri);
		if (neighbortri.tri.id != -1)
		{
			sides++;
			Vertex eapex = neighbortri.Apex();
			double dotproduct = (eorg.x - eapex.x) * (edest.x - eapex.x) + (eorg.y - eapex.y) * (edest.y - eapex.y);
			if (dotproduct < 0.0 && (behavior.ConformingDelaunay || dotproduct * dotproduct >= (2.0 * behavior.goodAngle - 1.0) * (2.0 * behavior.goodAngle - 1.0) * ((eorg.x - eapex.x) * (eorg.x - eapex.x) + (eorg.y - eapex.y) * (eorg.y - eapex.y)) * ((edest.x - eapex.x) * (edest.x - eapex.x) + (edest.y - eapex.y) * (edest.y - eapex.y))))
			{
				encroached = 1;
			}
		}
		testsubseg.Sym(ref testsym);
		testsym.Pivot(ref neighbortri);
		if (neighbortri.tri.id != -1)
		{
			sides++;
			Vertex eapex = neighbortri.Apex();
			double dotproduct = (eorg.x - eapex.x) * (edest.x - eapex.x) + (eorg.y - eapex.y) * (edest.y - eapex.y);
			if (dotproduct < 0.0 && (behavior.ConformingDelaunay || dotproduct * dotproduct >= (2.0 * behavior.goodAngle - 1.0) * (2.0 * behavior.goodAngle - 1.0) * ((eorg.x - eapex.x) * (eorg.x - eapex.x) + (eorg.y - eapex.y) * (eorg.y - eapex.y)) * ((edest.x - eapex.x) * (edest.x - eapex.x) + (edest.y - eapex.y) * (edest.y - eapex.y))))
			{
				encroached += 2;
			}
		}
		if (encroached > 0 && (behavior.NoBisect == 0 || (behavior.NoBisect == 1 && sides == 2)))
		{
			BadSubseg encroachedseg = new BadSubseg();
			if (encroached == 1)
			{
				encroachedseg.subseg = testsubseg;
				encroachedseg.org = eorg;
				encroachedseg.dest = edest;
			}
			else
			{
				encroachedseg.subseg = testsym;
				encroachedseg.org = edest;
				encroachedseg.dest = eorg;
			}
			badsubsegs.Enqueue(encroachedseg);
		}
		return encroached;
	}

	public void TestTriangle(ref Otri testtri)
	{
		Otri tri1 = default(Otri);
		Otri tri2 = default(Otri);
		Osub testsub = default(Osub);
		Vertex torg = testtri.Org();
		Vertex tdest = testtri.Dest();
		Vertex tapex = testtri.Apex();
		double dxod = torg.x - tdest.x;
		double dyod = torg.y - tdest.y;
		double dxda = tdest.x - tapex.x;
		double dyda = tdest.y - tapex.y;
		double dxao = tapex.x - torg.x;
		double dyao = tapex.y - torg.y;
		double dxod2 = dxod * dxod;
		double dyod2 = dyod * dyod;
		double dxda2 = dxda * dxda;
		double dyda2 = dyda * dyda;
		double num = dxao * dxao;
		double dyao2 = dyao * dyao;
		double apexlen = dxod2 + dyod2;
		double orglen = dxda2 + dyda2;
		double destlen = num + dyao2;
		double minedge;
		Vertex base1;
		Vertex base2;
		double angle;
		if (apexlen < orglen && apexlen < destlen)
		{
			minedge = apexlen;
			angle = dxda * dxao + dyda * dyao;
			angle = angle * angle / (orglen * destlen);
			base1 = torg;
			base2 = tdest;
			testtri.Copy(ref tri1);
		}
		else if (orglen < destlen)
		{
			minedge = orglen;
			angle = dxod * dxao + dyod * dyao;
			angle = angle * angle / (apexlen * destlen);
			base1 = tdest;
			base2 = tapex;
			testtri.Lnext(ref tri1);
		}
		else
		{
			minedge = destlen;
			angle = dxod * dxda + dyod * dyda;
			angle = angle * angle / (apexlen * orglen);
			base1 = tapex;
			base2 = torg;
			testtri.Lprev(ref tri1);
		}
		if (behavior.VarArea || behavior.fixedArea || behavior.UserTest != null)
		{
			double area = 0.5 * (dxod * dyda - dyod * dxda);
			if (behavior.fixedArea && area > behavior.MaxArea)
			{
				queue.Enqueue(ref testtri, minedge, tapex, torg, tdest);
				return;
			}
			if (behavior.VarArea && area > testtri.tri.area && testtri.tri.area > 0.0)
			{
				queue.Enqueue(ref testtri, minedge, tapex, torg, tdest);
				return;
			}
			if (behavior.UserTest != null && behavior.UserTest(testtri.tri, area))
			{
				queue.Enqueue(ref testtri, minedge, tapex, torg, tdest);
				return;
			}
		}
		double maxangle = ((apexlen > orglen && apexlen > destlen) ? ((orglen + destlen - apexlen) / (2.0 * Math.Sqrt(orglen * destlen))) : ((!(orglen > destlen)) ? ((apexlen + orglen - destlen) / (2.0 * Math.Sqrt(apexlen * orglen))) : ((apexlen + destlen - orglen) / (2.0 * Math.Sqrt(apexlen * destlen)))));
		if (!(angle > behavior.goodAngle) && (!(maxangle < behavior.maxGoodAngle) || behavior.MaxAngle == 0.0))
		{
			return;
		}
		if (base1.type == VertexType.SegmentVertex && base2.type == VertexType.SegmentVertex)
		{
			tri1.Pivot(ref testsub);
			if (testsub.seg.hash == -1)
			{
				tri1.Copy(ref tri2);
				do
				{
					tri1.Oprev();
					tri1.Pivot(ref testsub);
				}
				while (testsub.seg.hash == -1);
				Vertex org1 = testsub.SegOrg();
				Vertex dest1 = testsub.SegDest();
				do
				{
					tri2.Dnext();
					tri2.Pivot(ref testsub);
				}
				while (testsub.seg.hash == -1);
				Vertex org2 = testsub.SegOrg();
				Vertex dest2 = testsub.SegDest();
				Vertex joinvertex = null;
				if (dest1.x == org2.x && dest1.y == org2.y)
				{
					joinvertex = dest1;
				}
				else if (org1.x == dest2.x && org1.y == dest2.y)
				{
					joinvertex = org1;
				}
				if (joinvertex != null)
				{
					double dist1 = (base1.x - joinvertex.x) * (base1.x - joinvertex.x) + (base1.y - joinvertex.y) * (base1.y - joinvertex.y);
					double dist2 = (base2.x - joinvertex.x) * (base2.x - joinvertex.x) + (base2.y - joinvertex.y) * (base2.y - joinvertex.y);
					if (dist1 < 1.001 * dist2 && dist1 > 0.999 * dist2)
					{
						return;
					}
				}
			}
		}
		queue.Enqueue(ref testtri, minedge, tapex, torg, tdest);
	}

	private void TallyEncs()
	{
		Osub subsegloop = default(Osub);
		subsegloop.orient = 0;
		foreach (SubSegment seg in mesh.subsegs.Values)
		{
			subsegloop.seg = seg;
			CheckSeg4Encroach(ref subsegloop);
		}
	}

	private void SplitEncSegs(bool triflaws)
	{
		Otri enctri = default(Otri);
		Otri testtri = default(Otri);
		Osub testsh = default(Osub);
		Osub currentenc = default(Osub);
		while (badsubsegs.Count > 0 && mesh.steinerleft != 0)
		{
			BadSubseg seg = badsubsegs.Dequeue();
			currentenc = seg.subseg;
			Vertex eorg = currentenc.Org();
			Vertex edest = currentenc.Dest();
			if (!Osub.IsDead(currentenc.seg) && eorg == seg.org && edest == seg.dest)
			{
				currentenc.Pivot(ref enctri);
				enctri.Lnext(ref testtri);
				testtri.Pivot(ref testsh);
				bool acuteorg = testsh.seg.hash != -1;
				testtri.Lnext();
				testtri.Pivot(ref testsh);
				bool acutedest = testsh.seg.hash != -1;
				if (!behavior.ConformingDelaunay && !acuteorg && !acutedest)
				{
					Vertex eapex = enctri.Apex();
					while (eapex.type == VertexType.FreeVertex && (eorg.x - eapex.x) * (edest.x - eapex.x) + (eorg.y - eapex.y) * (edest.y - eapex.y) < 0.0)
					{
						mesh.DeleteVertex(ref testtri);
						currentenc.Pivot(ref enctri);
						eapex = enctri.Apex();
						enctri.Lprev(ref testtri);
					}
				}
				enctri.Sym(ref testtri);
				if (testtri.tri.id != -1)
				{
					testtri.Lnext();
					testtri.Pivot(ref testsh);
					bool acutedest2 = testsh.seg.hash != -1;
					acutedest = acutedest || acutedest2;
					testtri.Lnext();
					testtri.Pivot(ref testsh);
					bool acuteorg2 = testsh.seg.hash != -1;
					acuteorg = acuteorg || acuteorg2;
					if (!behavior.ConformingDelaunay && !acuteorg2 && !acutedest2)
					{
						Vertex eapex = testtri.Org();
						while (eapex.type == VertexType.FreeVertex && (eorg.x - eapex.x) * (edest.x - eapex.x) + (eorg.y - eapex.y) * (edest.y - eapex.y) < 0.0)
						{
							mesh.DeleteVertex(ref testtri);
							enctri.Sym(ref testtri);
							eapex = testtri.Apex();
							testtri.Lprev();
						}
					}
				}
				double split;
				if (acuteorg || acutedest)
				{
					double segmentlength = Math.Sqrt((edest.x - eorg.x) * (edest.x - eorg.x) + (edest.y - eorg.y) * (edest.y - eorg.y));
					double nearestpoweroftwo = 1.0;
					while (segmentlength > 3.0 * nearestpoweroftwo)
					{
						nearestpoweroftwo *= 2.0;
					}
					while (segmentlength < 1.5 * nearestpoweroftwo)
					{
						nearestpoweroftwo *= 0.5;
					}
					split = nearestpoweroftwo / segmentlength;
					if (acutedest)
					{
						split = 1.0 - split;
					}
				}
				else
				{
					split = 0.5;
				}
				Vertex newvertex = new Vertex(eorg.x + split * (edest.x - eorg.x), eorg.y + split * (edest.y - eorg.y), currentenc.seg.boundary);
				newvertex.type = VertexType.SegmentVertex;
				newvertex.hash = mesh.hash_vtx++;
				newvertex.id = newvertex.hash;
				mesh.vertices.Add(newvertex.hash, newvertex);
				newvertex.z = eorg.z + split * (edest.z - eorg.z);
				if (!Behavior.NoExact)
				{
					double multiplier = predicates.CounterClockwise(eorg, edest, newvertex);
					double divisor = (eorg.x - edest.x) * (eorg.x - edest.x) + (eorg.y - edest.y) * (eorg.y - edest.y);
					if (multiplier != 0.0 && divisor != 0.0)
					{
						multiplier /= divisor;
						if (!double.IsNaN(multiplier))
						{
							newvertex.x += multiplier * (edest.y - eorg.y);
							newvertex.y += multiplier * (eorg.x - edest.x);
						}
					}
				}
				if ((newvertex.x == eorg.x && newvertex.y == eorg.y) || (newvertex.x == edest.x && newvertex.y == edest.y))
				{
					logger.Error("Ran out of precision: I attempted to split a segment to a smaller size than can be accommodated by the finite precision of floating point arithmetic.", "Quality.SplitEncSegs()");
					throw new Exception("Ran out of precision");
				}
				InsertVertexResult success = mesh.InsertVertex(newvertex, ref enctri, ref currentenc, segmentflaws: true, triflaws);
				if (success != 0 && success != InsertVertexResult.Encroaching)
				{
					logger.Error("Failure to split a segment.", "Quality.SplitEncSegs()");
					throw new Exception("Failure to split a segment.");
				}
				if (mesh.steinerleft > 0)
				{
					mesh.steinerleft--;
				}
				CheckSeg4Encroach(ref currentenc);
				currentenc.Next();
				CheckSeg4Encroach(ref currentenc);
			}
			seg.org = null;
		}
	}

	private void TallyFaces()
	{
		Otri triangleloop = default(Otri);
		triangleloop.orient = 0;
		foreach (Triangle tri in mesh.triangles)
		{
			triangleloop.tri = tri;
			TestTriangle(ref triangleloop);
		}
	}

	private void SplitTriangle(BadTriangle badtri)
	{
		Otri badotri = default(Otri);
		double xi = 0.0;
		double eta = 0.0;
		badotri = badtri.poortri;
		Vertex borg = badotri.Org();
		Vertex bdest = badotri.Dest();
		Vertex bapex = badotri.Apex();
		if (Otri.IsDead(badotri.tri) || !(borg == badtri.org) || !(bdest == badtri.dest) || !(bapex == badtri.apex))
		{
			return;
		}
		bool errorflag = false;
		Point newloc = ((!behavior.fixedArea && !behavior.VarArea) ? newLocation.FindLocation(borg, bdest, bapex, ref xi, ref eta, offcenter: true, badotri) : predicates.FindCircumcenter(borg, bdest, bapex, ref xi, ref eta, behavior.offconstant));
		if ((newloc.x == borg.x && newloc.y == borg.y) || (newloc.x == bdest.x && newloc.y == bdest.y) || (newloc.x == bapex.x && newloc.y == bapex.y))
		{
			if (Log.Verbose)
			{
				logger.Warning("New vertex falls on existing vertex.", "Quality.SplitTriangle()");
				errorflag = true;
			}
		}
		else
		{
			Vertex newvertex = new Vertex(newloc.x, newloc.y, 0);
			newvertex.type = VertexType.FreeVertex;
			if (eta < xi)
			{
				badotri.Lprev();
			}
			newvertex.tri.tri = newvertex_tri;
			Osub tmp = default(Osub);
			switch (mesh.InsertVertex(newvertex, ref badotri, ref tmp, segmentflaws: true, triflaws: true))
			{
			case InsertVertexResult.Successful:
				newvertex.hash = mesh.hash_vtx++;
				newvertex.id = newvertex.hash;
				Interpolation.InterpolateZ(newvertex, newvertex.tri.tri);
				mesh.vertices.Add(newvertex.hash, newvertex);
				if (mesh.steinerleft > 0)
				{
					mesh.steinerleft--;
				}
				break;
			case InsertVertexResult.Encroaching:
				mesh.UndoVertex();
				break;
			default:
				if (Log.Verbose)
				{
					logger.Warning("New vertex falls on existing vertex.", "Quality.SplitTriangle()");
					errorflag = true;
				}
				break;
			case InsertVertexResult.Violating:
				break;
			}
		}
		if (errorflag)
		{
			logger.Error("The new vertex is at the circumcenter of triangle: This probably means that I am trying to refine triangles to a smaller size than can be accommodated by the finite precision of floating point arithmetic.", "Quality.SplitTriangle()");
			throw new Exception("The new vertex is at the circumcenter of triangle.");
		}
	}

	private void EnforceQuality()
	{
		TallyEncs();
		SplitEncSegs(triflaws: false);
		if (behavior.MinAngle > 0.0 || behavior.VarArea || behavior.fixedArea || behavior.UserTest != null)
		{
			TallyFaces();
			mesh.checkquality = true;
			while (queue.Count > 0 && mesh.steinerleft != 0)
			{
				BadTriangle badtri = queue.Dequeue();
				SplitTriangle(badtri);
				if (badsubsegs.Count > 0)
				{
					queue.Enqueue(badtri);
					SplitEncSegs(triflaws: true);
				}
			}
		}
		if (Log.Verbose && behavior.ConformingDelaunay && badsubsegs.Count > 0 && mesh.steinerleft == 0)
		{
			logger.Warning("I ran out of Steiner points, but the mesh has encroached subsegments, and therefore might not be truly Delaunay. If the Delaunay property is important to you, try increasing the number of Steiner points.", "Quality.EnforceQuality()");
		}
	}
}
