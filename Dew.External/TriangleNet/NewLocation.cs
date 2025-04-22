using System;
using TriangleNet.Geometry;
using TriangleNet.Tools;
using TriangleNet.Topology;

namespace TriangleNet;

internal class NewLocation
{
	private const double EPS = 1E-50;

	private IPredicates predicates;

	private Mesh mesh;

	private Behavior behavior;

	private double[] petalx = new double[20];

	private double[] petaly = new double[20];

	private double[] petalr = new double[20];

	private double[] wedges = new double[500];

	private double[] initialConvexPoly = new double[500];

	private double[] points_p = new double[500];

	private double[] points_q = new double[500];

	private double[] points_r = new double[500];

	private double[] poly1 = new double[100];

	private double[] poly2 = new double[100];

	private double[][] polys = new double[3][];

	public NewLocation(Mesh mesh, IPredicates predicates)
	{
		this.mesh = mesh;
		this.predicates = predicates;
		behavior = mesh.behavior;
	}

	public Point FindLocation(Vertex org, Vertex dest, Vertex apex, ref double xi, ref double eta, bool offcenter, Otri badotri)
	{
		if (behavior.MaxAngle == 0.0)
		{
			return FindNewLocationWithoutMaxAngle(org, dest, apex, ref xi, ref eta, offcenter: true, badotri);
		}
		return FindNewLocation(org, dest, apex, ref xi, ref eta, offcenter: true, badotri);
	}

	private Point FindNewLocationWithoutMaxAngle(Vertex torg, Vertex tdest, Vertex tapex, ref double xi, ref double eta, bool offcenter, Otri badotri)
	{
		double offconstant = behavior.offconstant;
		double xShortestEdge = 0.0;
		double yShortestEdge = 0.0;
		double shortestEdgeDist = 0.0;
		double middleEdgeDist = 0.0;
		double longestEdgeDist = 0.0;
		int orientation = 0;
		int almostGood = 0;
		Otri neighborotri = default(Otri);
		double[] thirdPoint = new double[2];
		double xi_tmp = 0.0;
		double eta_tmp = 0.0;
		double[] p = new double[5];
		double[] voronoiOrInter = new double[4];
		double pertConst = 0.06;
		double lengthConst = 1.0;
		double justAcute = 1.0;
		int relocated = 0;
		double[] newloc = new double[2];
		double origin_x = 0.0;
		double origin_y = 0.0;
		Statistic.CircumcenterCount++;
		double xdo = tdest.x - torg.x;
		double ydo = tdest.y - torg.y;
		double xao = tapex.x - torg.x;
		double yao = tapex.y - torg.y;
		double xda = tapex.x - tdest.x;
		double yda = tapex.y - tdest.y;
		double dodist = xdo * xdo + ydo * ydo;
		double aodist = xao * xao + yao * yao;
		double dadist = (tdest.x - tapex.x) * (tdest.x - tapex.x) + (tdest.y - tapex.y) * (tdest.y - tapex.y);
		double denominator;
		if (Behavior.NoExact)
		{
			denominator = 0.5 / (xdo * yao - xao * ydo);
		}
		else
		{
			denominator = 0.5 / predicates.CounterClockwise(tdest, tapex, torg);
			Statistic.CounterClockwiseCount--;
		}
		double dx = (yao * dodist - ydo * aodist) * denominator;
		double dy = (xdo * aodist - xao * dodist) * denominator;
		Point myCircumcenter = new Point(torg.x + dx, torg.y + dy);
		Otri delotri = badotri;
		orientation = LongestShortestEdge(aodist, dadist, dodist);
		Point smallestAngleCorner;
		Point middleAngleCorner;
		Point largestAngleCorner;
		switch (orientation)
		{
		case 123:
			xShortestEdge = xao;
			yShortestEdge = yao;
			shortestEdgeDist = aodist;
			middleEdgeDist = dadist;
			longestEdgeDist = dodist;
			smallestAngleCorner = tdest;
			middleAngleCorner = torg;
			largestAngleCorner = tapex;
			break;
		case 132:
			xShortestEdge = xao;
			yShortestEdge = yao;
			shortestEdgeDist = aodist;
			middleEdgeDist = dodist;
			longestEdgeDist = dadist;
			smallestAngleCorner = tdest;
			middleAngleCorner = tapex;
			largestAngleCorner = torg;
			break;
		case 213:
			xShortestEdge = xda;
			yShortestEdge = yda;
			shortestEdgeDist = dadist;
			middleEdgeDist = aodist;
			longestEdgeDist = dodist;
			smallestAngleCorner = torg;
			middleAngleCorner = tdest;
			largestAngleCorner = tapex;
			break;
		case 231:
			xShortestEdge = xda;
			yShortestEdge = yda;
			shortestEdgeDist = dadist;
			middleEdgeDist = dodist;
			longestEdgeDist = aodist;
			smallestAngleCorner = torg;
			middleAngleCorner = tapex;
			largestAngleCorner = tdest;
			break;
		case 312:
			xShortestEdge = xdo;
			yShortestEdge = ydo;
			shortestEdgeDist = dodist;
			middleEdgeDist = aodist;
			longestEdgeDist = dadist;
			smallestAngleCorner = tapex;
			middleAngleCorner = tdest;
			largestAngleCorner = torg;
			break;
		default:
			xShortestEdge = xdo;
			yShortestEdge = ydo;
			shortestEdgeDist = dodist;
			middleEdgeDist = dadist;
			longestEdgeDist = aodist;
			smallestAngleCorner = tapex;
			middleAngleCorner = torg;
			largestAngleCorner = tdest;
			break;
		}
		if (offcenter && offconstant > 0.0)
		{
			switch (orientation)
			{
			case 213:
			case 231:
			{
				double dxoff = 0.5 * xShortestEdge - offconstant * yShortestEdge;
				double dyoff = 0.5 * yShortestEdge + offconstant * xShortestEdge;
				if (dxoff * dxoff + dyoff * dyoff < (dx - xdo) * (dx - xdo) + (dy - ydo) * (dy - ydo))
				{
					dx = xdo + dxoff;
					dy = ydo + dyoff;
				}
				else
				{
					almostGood = 1;
				}
				break;
			}
			case 123:
			case 132:
			{
				double dxoff = 0.5 * xShortestEdge + offconstant * yShortestEdge;
				double dyoff = 0.5 * yShortestEdge - offconstant * xShortestEdge;
				if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
				{
					dx = dxoff;
					dy = dyoff;
				}
				else
				{
					almostGood = 1;
				}
				break;
			}
			default:
			{
				double dxoff = 0.5 * xShortestEdge - offconstant * yShortestEdge;
				double dyoff = 0.5 * yShortestEdge + offconstant * xShortestEdge;
				if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
				{
					dx = dxoff;
					dy = dyoff;
				}
				else
				{
					almostGood = 1;
				}
				break;
			}
			}
		}
		if (almostGood == 1)
		{
			double cosMaxAngle = (middleEdgeDist + shortestEdgeDist - longestEdgeDist) / (2.0 * Math.Sqrt(middleEdgeDist) * Math.Sqrt(shortestEdgeDist));
			bool isObtuse = cosMaxAngle < 0.0 || Math.Abs(cosMaxAngle - 0.0) <= 1E-50;
			relocated = DoSmoothing(delotri, torg, tdest, tapex, ref newloc);
			if (relocated > 0)
			{
				Statistic.RelocationCount++;
				dx = newloc[0] - torg.x;
				dy = newloc[1] - torg.y;
				origin_x = torg.x;
				origin_y = torg.y;
				switch (relocated)
				{
				case 1:
					mesh.DeleteVertex(ref delotri);
					break;
				case 2:
					delotri.Lnext();
					mesh.DeleteVertex(ref delotri);
					break;
				case 3:
					delotri.Lprev();
					mesh.DeleteVertex(ref delotri);
					break;
				}
			}
			else
			{
				double petalRadius = Math.Sqrt(shortestEdgeDist) / (2.0 * Math.Sin(behavior.MinAngle * Math.PI / 180.0));
				double num = (middleAngleCorner.x + largestAngleCorner.x) / 2.0;
				double yMidOfShortestEdge = (middleAngleCorner.y + largestAngleCorner.y) / 2.0;
				double xPetalCtr_1 = num + Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (middleAngleCorner.y - largestAngleCorner.y) / Math.Sqrt(shortestEdgeDist);
				double yPetalCtr_1 = yMidOfShortestEdge + Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (largestAngleCorner.x - middleAngleCorner.x) / Math.Sqrt(shortestEdgeDist);
				double xPetalCtr_2 = num - Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (middleAngleCorner.y - largestAngleCorner.y) / Math.Sqrt(shortestEdgeDist);
				double yPetalCtr_2 = yMidOfShortestEdge - Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (largestAngleCorner.x - middleAngleCorner.x) / Math.Sqrt(shortestEdgeDist);
				double num2 = (xPetalCtr_1 - smallestAngleCorner.x) * (xPetalCtr_1 - smallestAngleCorner.x);
				double dycenter1 = (yPetalCtr_1 - smallestAngleCorner.y) * (yPetalCtr_1 - smallestAngleCorner.y);
				double dxcenter2 = (xPetalCtr_2 - smallestAngleCorner.x) * (xPetalCtr_2 - smallestAngleCorner.x);
				double dycenter2 = (yPetalCtr_2 - smallestAngleCorner.y) * (yPetalCtr_2 - smallestAngleCorner.y);
				double xPetalCtr;
				double yPetalCtr;
				if (num2 + dycenter1 <= dxcenter2 + dycenter2)
				{
					xPetalCtr = xPetalCtr_1;
					yPetalCtr = yPetalCtr_1;
				}
				else
				{
					xPetalCtr = xPetalCtr_2;
					yPetalCtr = yPetalCtr_2;
				}
				bool neighborsVertex = GetNeighborsVertex(badotri, middleAngleCorner.x, middleAngleCorner.y, smallestAngleCorner.x, smallestAngleCorner.y, ref thirdPoint, ref neighborotri);
				double dxFirstSuggestion = dx;
				double dyFirstSuggestion = dy;
				if (!neighborsVertex)
				{
					Vertex neighborvertex_1 = neighborotri.Org();
					Vertex neighborvertex_2 = neighborotri.Dest();
					Vertex neighborvertex_3 = neighborotri.Apex();
					Point neighborCircumcenter = predicates.FindCircumcenter(neighborvertex_1, neighborvertex_2, neighborvertex_3, ref xi_tmp, ref eta_tmp);
					double vector_x = middleAngleCorner.y - smallestAngleCorner.y;
					double vector_y = smallestAngleCorner.x - middleAngleCorner.x;
					vector_x = myCircumcenter.x + vector_x;
					vector_y = myCircumcenter.y + vector_y;
					CircleLineIntersection(myCircumcenter.x, myCircumcenter.y, vector_x, vector_y, xPetalCtr, yPetalCtr, petalRadius, ref p);
					double xMidOfLongestEdge = (middleAngleCorner.x + smallestAngleCorner.x) / 2.0;
					double yMidOfLongestEdge = (middleAngleCorner.y + smallestAngleCorner.y) / 2.0;
					double inter_x;
					double inter_y;
					if (ChooseCorrectPoint(xMidOfLongestEdge, yMidOfLongestEdge, p[3], p[4], myCircumcenter.x, myCircumcenter.y, isObtuse))
					{
						inter_x = p[3];
						inter_y = p[4];
					}
					else
					{
						inter_x = p[1];
						inter_y = p[2];
					}
					PointBetweenPoints(inter_x, inter_y, myCircumcenter.x, myCircumcenter.y, neighborCircumcenter.x, neighborCircumcenter.y, ref voronoiOrInter);
					if (p[0] > 0.0)
					{
						if (Math.Abs(voronoiOrInter[0] - 1.0) <= 1E-50)
						{
							if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, neighborCircumcenter.x, neighborCircumcenter.y))
							{
								dxFirstSuggestion = dx;
								dyFirstSuggestion = dy;
							}
							else
							{
								dxFirstSuggestion = voronoiOrInter[2] - torg.x;
								dyFirstSuggestion = voronoiOrInter[3] - torg.y;
							}
						}
						else if (IsBadTriangleAngle(largestAngleCorner.x, largestAngleCorner.y, middleAngleCorner.x, middleAngleCorner.y, inter_x, inter_y))
						{
							double d = Math.Sqrt((inter_x - myCircumcenter.x) * (inter_x - myCircumcenter.x) + (inter_y - myCircumcenter.y) * (inter_y - myCircumcenter.y));
							double ax = myCircumcenter.x - inter_x;
							double ay = myCircumcenter.y - inter_y;
							ax /= d;
							ay /= d;
							inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
							inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
							if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, inter_x, inter_y))
							{
								dxFirstSuggestion = dx;
								dyFirstSuggestion = dy;
							}
							else
							{
								dxFirstSuggestion = inter_x - torg.x;
								dyFirstSuggestion = inter_y - torg.y;
							}
						}
						else
						{
							dxFirstSuggestion = inter_x - torg.x;
							dyFirstSuggestion = inter_y - torg.y;
						}
						if ((smallestAngleCorner.x - myCircumcenter.x) * (smallestAngleCorner.x - myCircumcenter.x) + (smallestAngleCorner.y - myCircumcenter.y) * (smallestAngleCorner.y - myCircumcenter.y) > lengthConst * ((smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) * (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) + (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)) * (smallestAngleCorner.y - (dyFirstSuggestion + torg.y))))
						{
							dxFirstSuggestion = dx;
							dyFirstSuggestion = dy;
						}
					}
				}
				bool neighborsVertex2 = GetNeighborsVertex(badotri, largestAngleCorner.x, largestAngleCorner.y, smallestAngleCorner.x, smallestAngleCorner.y, ref thirdPoint, ref neighborotri);
				double dxSecondSuggestion = dx;
				double dySecondSuggestion = dy;
				if (!neighborsVertex2)
				{
					Vertex neighborvertex_1 = neighborotri.Org();
					Vertex neighborvertex_2 = neighborotri.Dest();
					Vertex neighborvertex_3 = neighborotri.Apex();
					Point neighborCircumcenter = predicates.FindCircumcenter(neighborvertex_1, neighborvertex_2, neighborvertex_3, ref xi_tmp, ref eta_tmp);
					double vector_x = largestAngleCorner.y - smallestAngleCorner.y;
					double vector_y = smallestAngleCorner.x - largestAngleCorner.x;
					vector_x = myCircumcenter.x + vector_x;
					vector_y = myCircumcenter.y + vector_y;
					CircleLineIntersection(myCircumcenter.x, myCircumcenter.y, vector_x, vector_y, xPetalCtr, yPetalCtr, petalRadius, ref p);
					double xMidOfMiddleEdge = (largestAngleCorner.x + smallestAngleCorner.x) / 2.0;
					double yMidOfMiddleEdge = (largestAngleCorner.y + smallestAngleCorner.y) / 2.0;
					double inter_x;
					double inter_y;
					if (ChooseCorrectPoint(xMidOfMiddleEdge, yMidOfMiddleEdge, p[3], p[4], myCircumcenter.x, myCircumcenter.y, isObtuse: false))
					{
						inter_x = p[3];
						inter_y = p[4];
					}
					else
					{
						inter_x = p[1];
						inter_y = p[2];
					}
					PointBetweenPoints(inter_x, inter_y, myCircumcenter.x, myCircumcenter.y, neighborCircumcenter.x, neighborCircumcenter.y, ref voronoiOrInter);
					if (p[0] > 0.0)
					{
						if (Math.Abs(voronoiOrInter[0] - 1.0) <= 1E-50)
						{
							if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, neighborCircumcenter.x, neighborCircumcenter.y))
							{
								dxSecondSuggestion = dx;
								dySecondSuggestion = dy;
							}
							else
							{
								dxSecondSuggestion = voronoiOrInter[2] - torg.x;
								dySecondSuggestion = voronoiOrInter[3] - torg.y;
							}
						}
						else if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, inter_x, inter_y))
						{
							double d = Math.Sqrt((inter_x - myCircumcenter.x) * (inter_x - myCircumcenter.x) + (inter_y - myCircumcenter.y) * (inter_y - myCircumcenter.y));
							double ax = myCircumcenter.x - inter_x;
							double ay = myCircumcenter.y - inter_y;
							ax /= d;
							ay /= d;
							inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
							inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
							if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, inter_x, inter_y))
							{
								dxSecondSuggestion = dx;
								dySecondSuggestion = dy;
							}
							else
							{
								dxSecondSuggestion = inter_x - torg.x;
								dySecondSuggestion = inter_y - torg.y;
							}
						}
						else
						{
							dxSecondSuggestion = inter_x - torg.x;
							dySecondSuggestion = inter_y - torg.y;
						}
						if ((smallestAngleCorner.x - myCircumcenter.x) * (smallestAngleCorner.x - myCircumcenter.x) + (smallestAngleCorner.y - myCircumcenter.y) * (smallestAngleCorner.y - myCircumcenter.y) > lengthConst * ((smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) * (smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) + (smallestAngleCorner.y - (dySecondSuggestion + torg.y)) * (smallestAngleCorner.y - (dySecondSuggestion + torg.y))))
						{
							dxSecondSuggestion = dx;
							dySecondSuggestion = dy;
						}
					}
				}
				if (isObtuse)
				{
					dx = dxFirstSuggestion;
					dy = dyFirstSuggestion;
				}
				else if (justAcute * ((smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) * (smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) + (smallestAngleCorner.y - (dySecondSuggestion + torg.y)) * (smallestAngleCorner.y - (dySecondSuggestion + torg.y))) > (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) * (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) + (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)) * (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)))
				{
					dx = dxSecondSuggestion;
					dy = dySecondSuggestion;
				}
				else
				{
					dx = dxFirstSuggestion;
					dy = dyFirstSuggestion;
				}
			}
		}
		Point circumcenter = new Point();
		if (relocated <= 0)
		{
			circumcenter.x = torg.x + dx;
			circumcenter.y = torg.y + dy;
		}
		else
		{
			circumcenter.x = origin_x + dx;
			circumcenter.y = origin_y + dy;
		}
		xi = (yao * dx - xao * dy) * (2.0 * denominator);
		eta = (xdo * dy - ydo * dx) * (2.0 * denominator);
		return circumcenter;
	}

	private Point FindNewLocation(Vertex torg, Vertex tdest, Vertex tapex, ref double xi, ref double eta, bool offcenter, Otri badotri)
	{
		double offconstant = behavior.offconstant;
		double xShortestEdge = 0.0;
		double yShortestEdge = 0.0;
		double shortestEdgeDist = 0.0;
		double middleEdgeDist = 0.0;
		double longestEdgeDist = 0.0;
		int orientation = 0;
		int almostGood = 0;
		Otri neighborotri = default(Otri);
		double[] thirdPoint = new double[2];
		double xi_tmp = 0.0;
		double eta_tmp = 0.0;
		double[] p = new double[5];
		double[] voronoiOrInter = new double[4];
		double pertConst = 0.06;
		double lengthConst = 1.0;
		double justAcute = 1.0;
		int relocated = 0;
		double[] newloc = new double[2];
		double origin_x = 0.0;
		double origin_y = 0.0;
		double line_inter_x = 0.0;
		double line_inter_y = 0.0;
		double[] line_p = new double[3];
		double[] line_result = new double[4];
		Statistic.CircumcenterCount++;
		double xdo = tdest.x - torg.x;
		double ydo = tdest.y - torg.y;
		double xao = tapex.x - torg.x;
		double yao = tapex.y - torg.y;
		double xda = tapex.x - tdest.x;
		double yda = tapex.y - tdest.y;
		double dodist = xdo * xdo + ydo * ydo;
		double aodist = xao * xao + yao * yao;
		double dadist = (tdest.x - tapex.x) * (tdest.x - tapex.x) + (tdest.y - tapex.y) * (tdest.y - tapex.y);
		double denominator;
		if (Behavior.NoExact)
		{
			denominator = 0.5 / (xdo * yao - xao * ydo);
		}
		else
		{
			denominator = 0.5 / predicates.CounterClockwise(tdest, tapex, torg);
			Statistic.CounterClockwiseCount--;
		}
		double dx = (yao * dodist - ydo * aodist) * denominator;
		double dy = (xdo * aodist - xao * dodist) * denominator;
		Point myCircumcenter = new Point(torg.x + dx, torg.y + dy);
		Otri delotri = badotri;
		orientation = LongestShortestEdge(aodist, dadist, dodist);
		Point smallestAngleCorner;
		Point middleAngleCorner;
		Point largestAngleCorner;
		switch (orientation)
		{
		case 123:
			xShortestEdge = xao;
			yShortestEdge = yao;
			shortestEdgeDist = aodist;
			middleEdgeDist = dadist;
			longestEdgeDist = dodist;
			smallestAngleCorner = tdest;
			middleAngleCorner = torg;
			largestAngleCorner = tapex;
			break;
		case 132:
			xShortestEdge = xao;
			yShortestEdge = yao;
			shortestEdgeDist = aodist;
			middleEdgeDist = dodist;
			longestEdgeDist = dadist;
			smallestAngleCorner = tdest;
			middleAngleCorner = tapex;
			largestAngleCorner = torg;
			break;
		case 213:
			xShortestEdge = xda;
			yShortestEdge = yda;
			shortestEdgeDist = dadist;
			middleEdgeDist = aodist;
			longestEdgeDist = dodist;
			smallestAngleCorner = torg;
			middleAngleCorner = tdest;
			largestAngleCorner = tapex;
			break;
		case 231:
			xShortestEdge = xda;
			yShortestEdge = yda;
			shortestEdgeDist = dadist;
			middleEdgeDist = dodist;
			longestEdgeDist = aodist;
			smallestAngleCorner = torg;
			middleAngleCorner = tapex;
			largestAngleCorner = tdest;
			break;
		case 312:
			xShortestEdge = xdo;
			yShortestEdge = ydo;
			shortestEdgeDist = dodist;
			middleEdgeDist = aodist;
			longestEdgeDist = dadist;
			smallestAngleCorner = tapex;
			middleAngleCorner = tdest;
			largestAngleCorner = torg;
			break;
		default:
			xShortestEdge = xdo;
			yShortestEdge = ydo;
			shortestEdgeDist = dodist;
			middleEdgeDist = dadist;
			longestEdgeDist = aodist;
			smallestAngleCorner = tapex;
			middleAngleCorner = torg;
			largestAngleCorner = tdest;
			break;
		}
		if (offcenter && offconstant > 0.0)
		{
			switch (orientation)
			{
			case 213:
			case 231:
			{
				double dxoff = 0.5 * xShortestEdge - offconstant * yShortestEdge;
				double dyoff = 0.5 * yShortestEdge + offconstant * xShortestEdge;
				if (dxoff * dxoff + dyoff * dyoff < (dx - xdo) * (dx - xdo) + (dy - ydo) * (dy - ydo))
				{
					dx = xdo + dxoff;
					dy = ydo + dyoff;
				}
				else
				{
					almostGood = 1;
				}
				break;
			}
			case 123:
			case 132:
			{
				double dxoff = 0.5 * xShortestEdge + offconstant * yShortestEdge;
				double dyoff = 0.5 * yShortestEdge - offconstant * xShortestEdge;
				if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
				{
					dx = dxoff;
					dy = dyoff;
				}
				else
				{
					almostGood = 1;
				}
				break;
			}
			default:
			{
				double dxoff = 0.5 * xShortestEdge - offconstant * yShortestEdge;
				double dyoff = 0.5 * yShortestEdge + offconstant * xShortestEdge;
				if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
				{
					dx = dxoff;
					dy = dyoff;
				}
				else
				{
					almostGood = 1;
				}
				break;
			}
			}
		}
		if (almostGood == 1)
		{
			double cosMaxAngle = (middleEdgeDist + shortestEdgeDist - longestEdgeDist) / (2.0 * Math.Sqrt(middleEdgeDist) * Math.Sqrt(shortestEdgeDist));
			bool isObtuse = cosMaxAngle < 0.0 || Math.Abs(cosMaxAngle - 0.0) <= 1E-50;
			relocated = DoSmoothing(delotri, torg, tdest, tapex, ref newloc);
			if (relocated > 0)
			{
				Statistic.RelocationCount++;
				dx = newloc[0] - torg.x;
				dy = newloc[1] - torg.y;
				origin_x = torg.x;
				origin_y = torg.y;
				switch (relocated)
				{
				case 1:
					mesh.DeleteVertex(ref delotri);
					break;
				case 2:
					delotri.Lnext();
					mesh.DeleteVertex(ref delotri);
					break;
				case 3:
					delotri.Lprev();
					mesh.DeleteVertex(ref delotri);
					break;
				}
			}
			else
			{
				double minangle = Math.Acos((middleEdgeDist + longestEdgeDist - shortestEdgeDist) / (2.0 * Math.Sqrt(middleEdgeDist) * Math.Sqrt(longestEdgeDist))) * 180.0 / Math.PI;
				minangle = ((!(behavior.MinAngle > minangle)) ? (minangle + 0.5) : behavior.MinAngle);
				double petalRadius = Math.Sqrt(shortestEdgeDist) / (2.0 * Math.Sin(minangle * Math.PI / 180.0));
				double xMidOfShortestEdge = (middleAngleCorner.x + largestAngleCorner.x) / 2.0;
				double yMidOfShortestEdge = (middleAngleCorner.y + largestAngleCorner.y) / 2.0;
				double xPetalCtr_1 = xMidOfShortestEdge + Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (middleAngleCorner.y - largestAngleCorner.y) / Math.Sqrt(shortestEdgeDist);
				double yPetalCtr_1 = yMidOfShortestEdge + Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (largestAngleCorner.x - middleAngleCorner.x) / Math.Sqrt(shortestEdgeDist);
				double xPetalCtr_2 = xMidOfShortestEdge - Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (middleAngleCorner.y - largestAngleCorner.y) / Math.Sqrt(shortestEdgeDist);
				double yPetalCtr_2 = yMidOfShortestEdge - Math.Sqrt(petalRadius * petalRadius - shortestEdgeDist / 4.0) * (largestAngleCorner.x - middleAngleCorner.x) / Math.Sqrt(shortestEdgeDist);
				double num = (xPetalCtr_1 - smallestAngleCorner.x) * (xPetalCtr_1 - smallestAngleCorner.x);
				double dycenter1 = (yPetalCtr_1 - smallestAngleCorner.y) * (yPetalCtr_1 - smallestAngleCorner.y);
				double dxcenter2 = (xPetalCtr_2 - smallestAngleCorner.x) * (xPetalCtr_2 - smallestAngleCorner.x);
				double dycenter2 = (yPetalCtr_2 - smallestAngleCorner.y) * (yPetalCtr_2 - smallestAngleCorner.y);
				double xPetalCtr;
				double yPetalCtr;
				if (num + dycenter1 <= dxcenter2 + dycenter2)
				{
					xPetalCtr = xPetalCtr_1;
					yPetalCtr = yPetalCtr_1;
				}
				else
				{
					xPetalCtr = xPetalCtr_2;
					yPetalCtr = yPetalCtr_2;
				}
				bool neighborNotFound_first = GetNeighborsVertex(badotri, middleAngleCorner.x, middleAngleCorner.y, smallestAngleCorner.x, smallestAngleCorner.y, ref thirdPoint, ref neighborotri);
				double dxFirstSuggestion = dx;
				double dyFirstSuggestion = dy;
				double dist = Math.Sqrt((xPetalCtr - xMidOfShortestEdge) * (xPetalCtr - xMidOfShortestEdge) + (yPetalCtr - yMidOfShortestEdge) * (yPetalCtr - yMidOfShortestEdge));
				double line_vector_x = (xPetalCtr - xMidOfShortestEdge) / dist;
				double line_vector_y = (yPetalCtr - yMidOfShortestEdge) / dist;
				double num2 = xPetalCtr + line_vector_x * petalRadius;
				double petal_bisector_y = yPetalCtr + line_vector_y * petalRadius;
				double alpha = (2.0 * behavior.MaxAngle + minangle - 180.0) * Math.PI / 180.0;
				double x_1 = num2 * Math.Cos(alpha) + petal_bisector_y * Math.Sin(alpha) + xPetalCtr - xPetalCtr * Math.Cos(alpha) - yPetalCtr * Math.Sin(alpha);
				double y_1 = (0.0 - num2) * Math.Sin(alpha) + petal_bisector_y * Math.Cos(alpha) + yPetalCtr + xPetalCtr * Math.Sin(alpha) - yPetalCtr * Math.Cos(alpha);
				double x_2 = num2 * Math.Cos(alpha) - petal_bisector_y * Math.Sin(alpha) + xPetalCtr - xPetalCtr * Math.Cos(alpha) + yPetalCtr * Math.Sin(alpha);
				double y_2 = num2 * Math.Sin(alpha) + petal_bisector_y * Math.Cos(alpha) + yPetalCtr - xPetalCtr * Math.Sin(alpha) - yPetalCtr * Math.Cos(alpha);
				double petal_slab_inter_x_first;
				double petal_slab_inter_y_first;
				double petal_slab_inter_x_second;
				double petal_slab_inter_y_second;
				if (ChooseCorrectPoint(x_2, y_2, middleAngleCorner.x, middleAngleCorner.y, x_1, y_1, isObtuse: true))
				{
					petal_slab_inter_x_first = x_1;
					petal_slab_inter_y_first = y_1;
					petal_slab_inter_x_second = x_2;
					petal_slab_inter_y_second = y_2;
				}
				else
				{
					petal_slab_inter_x_first = x_2;
					petal_slab_inter_y_first = y_2;
					petal_slab_inter_x_second = x_1;
					petal_slab_inter_y_second = y_1;
				}
				double xMidOfLongestEdge = (middleAngleCorner.x + smallestAngleCorner.x) / 2.0;
				double yMidOfLongestEdge = (middleAngleCorner.y + smallestAngleCorner.y) / 2.0;
				if (!neighborNotFound_first)
				{
					Vertex neighborvertex_1 = neighborotri.Org();
					Vertex neighborvertex_2 = neighborotri.Dest();
					Vertex neighborvertex_3 = neighborotri.Apex();
					Point neighborCircumcenter = predicates.FindCircumcenter(neighborvertex_1, neighborvertex_2, neighborvertex_3, ref xi_tmp, ref eta_tmp);
					double vector_x = middleAngleCorner.y - smallestAngleCorner.y;
					double vector_y = smallestAngleCorner.x - middleAngleCorner.x;
					vector_x = myCircumcenter.x + vector_x;
					vector_y = myCircumcenter.y + vector_y;
					CircleLineIntersection(myCircumcenter.x, myCircumcenter.y, vector_x, vector_y, xPetalCtr, yPetalCtr, petalRadius, ref p);
					double inter_x;
					double inter_y;
					if (ChooseCorrectPoint(xMidOfLongestEdge, yMidOfLongestEdge, p[3], p[4], myCircumcenter.x, myCircumcenter.y, isObtuse))
					{
						inter_x = p[3];
						inter_y = p[4];
					}
					else
					{
						inter_x = p[1];
						inter_y = p[2];
					}
					double linepnt1_x = middleAngleCorner.x;
					double linepnt1_y = middleAngleCorner.y;
					line_vector_x = largestAngleCorner.x - middleAngleCorner.x;
					line_vector_y = largestAngleCorner.y - middleAngleCorner.y;
					double linepnt2_x = petal_slab_inter_x_first;
					double linepnt2_y = petal_slab_inter_y_first;
					LineLineIntersection(myCircumcenter.x, myCircumcenter.y, vector_x, vector_y, linepnt1_x, linepnt1_y, linepnt2_x, linepnt2_y, ref line_p);
					if (line_p[0] > 0.0)
					{
						line_inter_x = line_p[1];
						line_inter_y = line_p[2];
					}
					PointBetweenPoints(inter_x, inter_y, myCircumcenter.x, myCircumcenter.y, neighborCircumcenter.x, neighborCircumcenter.y, ref voronoiOrInter);
					if (p[0] > 0.0)
					{
						if (Math.Abs(voronoiOrInter[0] - 1.0) <= 1E-50)
						{
							PointBetweenPoints(voronoiOrInter[2], voronoiOrInter[3], myCircumcenter.x, myCircumcenter.y, line_inter_x, line_inter_y, ref line_result);
							if (Math.Abs(line_result[0] - 1.0) <= 1E-50 && line_p[0] > 0.0)
							{
								if ((smallestAngleCorner.x - petal_slab_inter_x_first) * (smallestAngleCorner.x - petal_slab_inter_x_first) + (smallestAngleCorner.y - petal_slab_inter_y_first) * (smallestAngleCorner.y - petal_slab_inter_y_first) > lengthConst * ((smallestAngleCorner.x - line_inter_x) * (smallestAngleCorner.x - line_inter_x) + (smallestAngleCorner.y - line_inter_y) * (smallestAngleCorner.y - line_inter_y)) && IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, petal_slab_inter_x_first, petal_slab_inter_y_first) && MinDistanceToNeighbor(petal_slab_inter_x_first, petal_slab_inter_y_first, ref neighborotri) > MinDistanceToNeighbor(line_inter_x, line_inter_y, ref neighborotri))
								{
									dxFirstSuggestion = petal_slab_inter_x_first - torg.x;
									dyFirstSuggestion = petal_slab_inter_y_first - torg.y;
								}
								else if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, line_inter_x, line_inter_y))
								{
									double d = Math.Sqrt((line_inter_x - myCircumcenter.x) * (line_inter_x - myCircumcenter.x) + (line_inter_y - myCircumcenter.y) * (line_inter_y - myCircumcenter.y));
									double ax = myCircumcenter.x - line_inter_x;
									double ay = myCircumcenter.y - line_inter_y;
									ax /= d;
									ay /= d;
									line_inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
									line_inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
									if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, line_inter_x, line_inter_y))
									{
										dxFirstSuggestion = dx;
										dyFirstSuggestion = dy;
									}
									else
									{
										dxFirstSuggestion = line_inter_x - torg.x;
										dyFirstSuggestion = line_inter_y - torg.y;
									}
								}
								else
								{
									dxFirstSuggestion = line_result[2] - torg.x;
									dyFirstSuggestion = line_result[3] - torg.y;
								}
							}
							else if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, neighborCircumcenter.x, neighborCircumcenter.y))
							{
								dxFirstSuggestion = dx;
								dyFirstSuggestion = dy;
							}
							else
							{
								dxFirstSuggestion = voronoiOrInter[2] - torg.x;
								dyFirstSuggestion = voronoiOrInter[3] - torg.y;
							}
						}
						else
						{
							PointBetweenPoints(inter_x, inter_y, myCircumcenter.x, myCircumcenter.y, line_inter_x, line_inter_y, ref line_result);
							if (Math.Abs(line_result[0] - 1.0) <= 1E-50 && line_p[0] > 0.0)
							{
								if ((smallestAngleCorner.x - petal_slab_inter_x_first) * (smallestAngleCorner.x - petal_slab_inter_x_first) + (smallestAngleCorner.y - petal_slab_inter_y_first) * (smallestAngleCorner.y - petal_slab_inter_y_first) > lengthConst * ((smallestAngleCorner.x - line_inter_x) * (smallestAngleCorner.x - line_inter_x) + (smallestAngleCorner.y - line_inter_y) * (smallestAngleCorner.y - line_inter_y)) && IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, petal_slab_inter_x_first, petal_slab_inter_y_first) && MinDistanceToNeighbor(petal_slab_inter_x_first, petal_slab_inter_y_first, ref neighborotri) > MinDistanceToNeighbor(line_inter_x, line_inter_y, ref neighborotri))
								{
									dxFirstSuggestion = petal_slab_inter_x_first - torg.x;
									dyFirstSuggestion = petal_slab_inter_y_first - torg.y;
								}
								else if (IsBadTriangleAngle(largestAngleCorner.x, largestAngleCorner.y, middleAngleCorner.x, middleAngleCorner.y, line_inter_x, line_inter_y))
								{
									double d = Math.Sqrt((line_inter_x - myCircumcenter.x) * (line_inter_x - myCircumcenter.x) + (line_inter_y - myCircumcenter.y) * (line_inter_y - myCircumcenter.y));
									double ax = myCircumcenter.x - line_inter_x;
									double ay = myCircumcenter.y - line_inter_y;
									ax /= d;
									ay /= d;
									line_inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
									line_inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
									if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, line_inter_x, line_inter_y))
									{
										dxFirstSuggestion = dx;
										dyFirstSuggestion = dy;
									}
									else
									{
										dxFirstSuggestion = line_inter_x - torg.x;
										dyFirstSuggestion = line_inter_y - torg.y;
									}
								}
								else
								{
									dxFirstSuggestion = line_result[2] - torg.x;
									dyFirstSuggestion = line_result[3] - torg.y;
								}
							}
							else if (IsBadTriangleAngle(largestAngleCorner.x, largestAngleCorner.y, middleAngleCorner.x, middleAngleCorner.y, inter_x, inter_y))
							{
								double d = Math.Sqrt((inter_x - myCircumcenter.x) * (inter_x - myCircumcenter.x) + (inter_y - myCircumcenter.y) * (inter_y - myCircumcenter.y));
								double ax = myCircumcenter.x - inter_x;
								double ay = myCircumcenter.y - inter_y;
								ax /= d;
								ay /= d;
								inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
								inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
								if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, inter_x, inter_y))
								{
									dxFirstSuggestion = dx;
									dyFirstSuggestion = dy;
								}
								else
								{
									dxFirstSuggestion = inter_x - torg.x;
									dyFirstSuggestion = inter_y - torg.y;
								}
							}
							else
							{
								dxFirstSuggestion = inter_x - torg.x;
								dyFirstSuggestion = inter_y - torg.y;
							}
						}
						if ((smallestAngleCorner.x - myCircumcenter.x) * (smallestAngleCorner.x - myCircumcenter.x) + (smallestAngleCorner.y - myCircumcenter.y) * (smallestAngleCorner.y - myCircumcenter.y) > lengthConst * ((smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) * (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) + (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)) * (smallestAngleCorner.y - (dyFirstSuggestion + torg.y))))
						{
							dxFirstSuggestion = dx;
							dyFirstSuggestion = dy;
						}
					}
				}
				bool neighborNotFound_second = GetNeighborsVertex(badotri, largestAngleCorner.x, largestAngleCorner.y, smallestAngleCorner.x, smallestAngleCorner.y, ref thirdPoint, ref neighborotri);
				double dxSecondSuggestion = dx;
				double dySecondSuggestion = dy;
				double xMidOfMiddleEdge = (largestAngleCorner.x + smallestAngleCorner.x) / 2.0;
				double yMidOfMiddleEdge = (largestAngleCorner.y + smallestAngleCorner.y) / 2.0;
				if (!neighborNotFound_second)
				{
					Vertex neighborvertex_1 = neighborotri.Org();
					Vertex neighborvertex_2 = neighborotri.Dest();
					Vertex neighborvertex_3 = neighborotri.Apex();
					Point neighborCircumcenter = predicates.FindCircumcenter(neighborvertex_1, neighborvertex_2, neighborvertex_3, ref xi_tmp, ref eta_tmp);
					double vector_x = largestAngleCorner.y - smallestAngleCorner.y;
					double vector_y = smallestAngleCorner.x - largestAngleCorner.x;
					vector_x = myCircumcenter.x + vector_x;
					vector_y = myCircumcenter.y + vector_y;
					CircleLineIntersection(myCircumcenter.x, myCircumcenter.y, vector_x, vector_y, xPetalCtr, yPetalCtr, petalRadius, ref p);
					double inter_x;
					double inter_y;
					if (ChooseCorrectPoint(xMidOfMiddleEdge, yMidOfMiddleEdge, p[3], p[4], myCircumcenter.x, myCircumcenter.y, isObtuse: false))
					{
						inter_x = p[3];
						inter_y = p[4];
					}
					else
					{
						inter_x = p[1];
						inter_y = p[2];
					}
					double linepnt1_x = largestAngleCorner.x;
					double linepnt1_y = largestAngleCorner.y;
					line_vector_x = middleAngleCorner.x - largestAngleCorner.x;
					line_vector_y = middleAngleCorner.y - largestAngleCorner.y;
					double linepnt2_x = petal_slab_inter_x_second;
					double linepnt2_y = petal_slab_inter_y_second;
					LineLineIntersection(myCircumcenter.x, myCircumcenter.y, vector_x, vector_y, linepnt1_x, linepnt1_y, linepnt2_x, linepnt2_y, ref line_p);
					if (line_p[0] > 0.0)
					{
						line_inter_x = line_p[1];
						line_inter_y = line_p[2];
					}
					PointBetweenPoints(inter_x, inter_y, myCircumcenter.x, myCircumcenter.y, neighborCircumcenter.x, neighborCircumcenter.y, ref voronoiOrInter);
					if (p[0] > 0.0)
					{
						if (Math.Abs(voronoiOrInter[0] - 1.0) <= 1E-50)
						{
							PointBetweenPoints(voronoiOrInter[2], voronoiOrInter[3], myCircumcenter.x, myCircumcenter.y, line_inter_x, line_inter_y, ref line_result);
							if (Math.Abs(line_result[0] - 1.0) <= 1E-50 && line_p[0] > 0.0)
							{
								if ((smallestAngleCorner.x - petal_slab_inter_x_second) * (smallestAngleCorner.x - petal_slab_inter_x_second) + (smallestAngleCorner.y - petal_slab_inter_y_second) * (smallestAngleCorner.y - petal_slab_inter_y_second) > lengthConst * ((smallestAngleCorner.x - line_inter_x) * (smallestAngleCorner.x - line_inter_x) + (smallestAngleCorner.y - line_inter_y) * (smallestAngleCorner.y - line_inter_y)) && IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, petal_slab_inter_x_second, petal_slab_inter_y_second) && MinDistanceToNeighbor(petal_slab_inter_x_second, petal_slab_inter_y_second, ref neighborotri) > MinDistanceToNeighbor(line_inter_x, line_inter_y, ref neighborotri))
								{
									dxSecondSuggestion = petal_slab_inter_x_second - torg.x;
									dySecondSuggestion = petal_slab_inter_y_second - torg.y;
								}
								else if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, line_inter_x, line_inter_y))
								{
									double d = Math.Sqrt((line_inter_x - myCircumcenter.x) * (line_inter_x - myCircumcenter.x) + (line_inter_y - myCircumcenter.y) * (line_inter_y - myCircumcenter.y));
									double ax = myCircumcenter.x - line_inter_x;
									double ay = myCircumcenter.y - line_inter_y;
									ax /= d;
									ay /= d;
									line_inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
									line_inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
									if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, line_inter_x, line_inter_y))
									{
										dxSecondSuggestion = dx;
										dySecondSuggestion = dy;
									}
									else
									{
										dxSecondSuggestion = line_inter_x - torg.x;
										dySecondSuggestion = line_inter_y - torg.y;
									}
								}
								else
								{
									dxSecondSuggestion = line_result[2] - torg.x;
									dySecondSuggestion = line_result[3] - torg.y;
								}
							}
							else if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, neighborCircumcenter.x, neighborCircumcenter.y))
							{
								dxSecondSuggestion = dx;
								dySecondSuggestion = dy;
							}
							else
							{
								dxSecondSuggestion = voronoiOrInter[2] - torg.x;
								dySecondSuggestion = voronoiOrInter[3] - torg.y;
							}
						}
						else
						{
							PointBetweenPoints(inter_x, inter_y, myCircumcenter.x, myCircumcenter.y, line_inter_x, line_inter_y, ref line_result);
							if (Math.Abs(line_result[0] - 1.0) <= 1E-50 && line_p[0] > 0.0)
							{
								if ((smallestAngleCorner.x - petal_slab_inter_x_second) * (smallestAngleCorner.x - petal_slab_inter_x_second) + (smallestAngleCorner.y - petal_slab_inter_y_second) * (smallestAngleCorner.y - petal_slab_inter_y_second) > lengthConst * ((smallestAngleCorner.x - line_inter_x) * (smallestAngleCorner.x - line_inter_x) + (smallestAngleCorner.y - line_inter_y) * (smallestAngleCorner.y - line_inter_y)) && IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, petal_slab_inter_x_second, petal_slab_inter_y_second) && MinDistanceToNeighbor(petal_slab_inter_x_second, petal_slab_inter_y_second, ref neighborotri) > MinDistanceToNeighbor(line_inter_x, line_inter_y, ref neighborotri))
								{
									dxSecondSuggestion = petal_slab_inter_x_second - torg.x;
									dySecondSuggestion = petal_slab_inter_y_second - torg.y;
								}
								else if (IsBadTriangleAngle(largestAngleCorner.x, largestAngleCorner.y, middleAngleCorner.x, middleAngleCorner.y, line_inter_x, line_inter_y))
								{
									double d = Math.Sqrt((line_inter_x - myCircumcenter.x) * (line_inter_x - myCircumcenter.x) + (line_inter_y - myCircumcenter.y) * (line_inter_y - myCircumcenter.y));
									double ax = myCircumcenter.x - line_inter_x;
									double ay = myCircumcenter.y - line_inter_y;
									ax /= d;
									ay /= d;
									line_inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
									line_inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
									if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, line_inter_x, line_inter_y))
									{
										dxSecondSuggestion = dx;
										dySecondSuggestion = dy;
									}
									else
									{
										dxSecondSuggestion = line_inter_x - torg.x;
										dySecondSuggestion = line_inter_y - torg.y;
									}
								}
								else
								{
									dxSecondSuggestion = line_result[2] - torg.x;
									dySecondSuggestion = line_result[3] - torg.y;
								}
							}
							else if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, inter_x, inter_y))
							{
								double d = Math.Sqrt((inter_x - myCircumcenter.x) * (inter_x - myCircumcenter.x) + (inter_y - myCircumcenter.y) * (inter_y - myCircumcenter.y));
								double ax = myCircumcenter.x - inter_x;
								double ay = myCircumcenter.y - inter_y;
								ax /= d;
								ay /= d;
								inter_x += ax * pertConst * Math.Sqrt(shortestEdgeDist);
								inter_y += ay * pertConst * Math.Sqrt(shortestEdgeDist);
								if (IsBadTriangleAngle(middleAngleCorner.x, middleAngleCorner.y, largestAngleCorner.x, largestAngleCorner.y, inter_x, inter_y))
								{
									dxSecondSuggestion = dx;
									dySecondSuggestion = dy;
								}
								else
								{
									dxSecondSuggestion = inter_x - torg.x;
									dySecondSuggestion = inter_y - torg.y;
								}
							}
							else
							{
								dxSecondSuggestion = inter_x - torg.x;
								dySecondSuggestion = inter_y - torg.y;
							}
						}
						if ((smallestAngleCorner.x - myCircumcenter.x) * (smallestAngleCorner.x - myCircumcenter.x) + (smallestAngleCorner.y - myCircumcenter.y) * (smallestAngleCorner.y - myCircumcenter.y) > lengthConst * ((smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) * (smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) + (smallestAngleCorner.y - (dySecondSuggestion + torg.y)) * (smallestAngleCorner.y - (dySecondSuggestion + torg.y))))
						{
							dxSecondSuggestion = dx;
							dySecondSuggestion = dy;
						}
					}
				}
				if (isObtuse)
				{
					if (neighborNotFound_first && neighborNotFound_second)
					{
						if (justAcute * ((smallestAngleCorner.x - xMidOfMiddleEdge) * (smallestAngleCorner.x - xMidOfMiddleEdge) + (smallestAngleCorner.y - yMidOfMiddleEdge) * (smallestAngleCorner.y - yMidOfMiddleEdge)) > (smallestAngleCorner.x - xMidOfLongestEdge) * (smallestAngleCorner.x - xMidOfLongestEdge) + (smallestAngleCorner.y - yMidOfLongestEdge) * (smallestAngleCorner.y - yMidOfLongestEdge))
						{
							dx = dxSecondSuggestion;
							dy = dySecondSuggestion;
						}
						else
						{
							dx = dxFirstSuggestion;
							dy = dyFirstSuggestion;
						}
					}
					else if (neighborNotFound_first)
					{
						if (justAcute * ((smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) * (smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) + (smallestAngleCorner.y - (dySecondSuggestion + torg.y)) * (smallestAngleCorner.y - (dySecondSuggestion + torg.y))) > (smallestAngleCorner.x - xMidOfLongestEdge) * (smallestAngleCorner.x - xMidOfLongestEdge) + (smallestAngleCorner.y - yMidOfLongestEdge) * (smallestAngleCorner.y - yMidOfLongestEdge))
						{
							dx = dxSecondSuggestion;
							dy = dySecondSuggestion;
						}
						else
						{
							dx = dxFirstSuggestion;
							dy = dyFirstSuggestion;
						}
					}
					else if (neighborNotFound_second)
					{
						if (justAcute * ((smallestAngleCorner.x - xMidOfMiddleEdge) * (smallestAngleCorner.x - xMidOfMiddleEdge) + (smallestAngleCorner.y - yMidOfMiddleEdge) * (smallestAngleCorner.y - yMidOfMiddleEdge)) > (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) * (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) + (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)) * (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)))
						{
							dx = dxSecondSuggestion;
							dy = dySecondSuggestion;
						}
						else
						{
							dx = dxFirstSuggestion;
							dy = dyFirstSuggestion;
						}
					}
					else if (justAcute * ((smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) * (smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) + (smallestAngleCorner.y - (dySecondSuggestion + torg.y)) * (smallestAngleCorner.y - (dySecondSuggestion + torg.y))) > (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) * (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) + (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)) * (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)))
					{
						dx = dxSecondSuggestion;
						dy = dySecondSuggestion;
					}
					else
					{
						dx = dxFirstSuggestion;
						dy = dyFirstSuggestion;
					}
				}
				else if (neighborNotFound_first && neighborNotFound_second)
				{
					if (justAcute * ((smallestAngleCorner.x - xMidOfMiddleEdge) * (smallestAngleCorner.x - xMidOfMiddleEdge) + (smallestAngleCorner.y - yMidOfMiddleEdge) * (smallestAngleCorner.y - yMidOfMiddleEdge)) > (smallestAngleCorner.x - xMidOfLongestEdge) * (smallestAngleCorner.x - xMidOfLongestEdge) + (smallestAngleCorner.y - yMidOfLongestEdge) * (smallestAngleCorner.y - yMidOfLongestEdge))
					{
						dx = dxSecondSuggestion;
						dy = dySecondSuggestion;
					}
					else
					{
						dx = dxFirstSuggestion;
						dy = dyFirstSuggestion;
					}
				}
				else if (neighborNotFound_first)
				{
					if (justAcute * ((smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) * (smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) + (smallestAngleCorner.y - (dySecondSuggestion + torg.y)) * (smallestAngleCorner.y - (dySecondSuggestion + torg.y))) > (smallestAngleCorner.x - xMidOfLongestEdge) * (smallestAngleCorner.x - xMidOfLongestEdge) + (smallestAngleCorner.y - yMidOfLongestEdge) * (smallestAngleCorner.y - yMidOfLongestEdge))
					{
						dx = dxSecondSuggestion;
						dy = dySecondSuggestion;
					}
					else
					{
						dx = dxFirstSuggestion;
						dy = dyFirstSuggestion;
					}
				}
				else if (neighborNotFound_second)
				{
					if (justAcute * ((smallestAngleCorner.x - xMidOfMiddleEdge) * (smallestAngleCorner.x - xMidOfMiddleEdge) + (smallestAngleCorner.y - yMidOfMiddleEdge) * (smallestAngleCorner.y - yMidOfMiddleEdge)) > (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) * (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) + (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)) * (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)))
					{
						dx = dxSecondSuggestion;
						dy = dySecondSuggestion;
					}
					else
					{
						dx = dxFirstSuggestion;
						dy = dyFirstSuggestion;
					}
				}
				else if (justAcute * ((smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) * (smallestAngleCorner.x - (dxSecondSuggestion + torg.x)) + (smallestAngleCorner.y - (dySecondSuggestion + torg.y)) * (smallestAngleCorner.y - (dySecondSuggestion + torg.y))) > (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) * (smallestAngleCorner.x - (dxFirstSuggestion + torg.x)) + (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)) * (smallestAngleCorner.y - (dyFirstSuggestion + torg.y)))
				{
					dx = dxSecondSuggestion;
					dy = dySecondSuggestion;
				}
				else
				{
					dx = dxFirstSuggestion;
					dy = dyFirstSuggestion;
				}
			}
		}
		Point circumcenter = new Point();
		if (relocated <= 0)
		{
			circumcenter.x = torg.x + dx;
			circumcenter.y = torg.y + dy;
		}
		else
		{
			circumcenter.x = origin_x + dx;
			circumcenter.y = origin_y + dy;
		}
		xi = (yao * dx - xao * dy) * (2.0 * denominator);
		eta = (xdo * dy - ydo * dx) * (2.0 * denominator);
		return circumcenter;
	}

	private int LongestShortestEdge(double aodist, double dadist, double dodist)
	{
		int max = 0;
		int min = 0;
		int mid = 0;
		if (dodist < aodist && dodist < dadist)
		{
			min = 3;
			if (aodist < dadist)
			{
				max = 2;
				mid = 1;
			}
			else
			{
				max = 1;
				mid = 2;
			}
		}
		else if (aodist < dadist)
		{
			min = 1;
			if (dodist < dadist)
			{
				max = 2;
				mid = 3;
			}
			else
			{
				max = 3;
				mid = 2;
			}
		}
		else
		{
			min = 2;
			if (aodist < dodist)
			{
				max = 3;
				mid = 1;
			}
			else
			{
				max = 1;
				mid = 3;
			}
		}
		return min * 100 + mid * 10 + max;
	}

	private int DoSmoothing(Otri badotri, Vertex torg, Vertex tdest, Vertex tapex, ref double[] newloc)
	{
		int numpoints_p = 0;
		int numpoints_q = 0;
		int numpoints_r = 0;
		double[] possibilities = new double[6];
		int num_pos = 0;
		int flag1 = 0;
		int flag2 = 0;
		int flag3 = 0;
		bool newLocFound = false;
		numpoints_p = GetStarPoints(badotri, torg, tdest, tapex, 1, ref points_p);
		if (torg.type == VertexType.FreeVertex && numpoints_p != 0 && ValidPolygonAngles(numpoints_p, points_p) && ((behavior.MaxAngle != 0.0) ? GetWedgeIntersection(numpoints_p, points_p, ref newloc) : GetWedgeIntersectionWithoutMaxAngle(numpoints_p, points_p, ref newloc)))
		{
			possibilities[0] = newloc[0];
			possibilities[1] = newloc[1];
			num_pos++;
			flag1 = 1;
		}
		numpoints_q = GetStarPoints(badotri, torg, tdest, tapex, 2, ref points_q);
		if (tdest.type == VertexType.FreeVertex && numpoints_q != 0 && ValidPolygonAngles(numpoints_q, points_q) && ((behavior.MaxAngle != 0.0) ? GetWedgeIntersection(numpoints_q, points_q, ref newloc) : GetWedgeIntersectionWithoutMaxAngle(numpoints_q, points_q, ref newloc)))
		{
			possibilities[2] = newloc[0];
			possibilities[3] = newloc[1];
			num_pos++;
			flag2 = 2;
		}
		numpoints_r = GetStarPoints(badotri, torg, tdest, tapex, 3, ref points_r);
		if (tapex.type == VertexType.FreeVertex && numpoints_r != 0 && ValidPolygonAngles(numpoints_r, points_r) && ((behavior.MaxAngle != 0.0) ? GetWedgeIntersection(numpoints_r, points_r, ref newloc) : GetWedgeIntersectionWithoutMaxAngle(numpoints_r, points_r, ref newloc)))
		{
			possibilities[4] = newloc[0];
			possibilities[5] = newloc[1];
			num_pos++;
			flag3 = 3;
		}
		if (num_pos > 0)
		{
			if (flag1 > 0)
			{
				newloc[0] = possibilities[0];
				newloc[1] = possibilities[1];
				return flag1;
			}
			if (flag2 > 0)
			{
				newloc[0] = possibilities[2];
				newloc[1] = possibilities[3];
				return flag2;
			}
			if (flag3 > 0)
			{
				newloc[0] = possibilities[4];
				newloc[1] = possibilities[5];
				return flag3;
			}
		}
		return 0;
	}

	private int GetStarPoints(Otri badotri, Vertex p, Vertex q, Vertex r, int whichPoint, ref double[] points)
	{
		Otri neighotri = default(Otri);
		double first_x = 0.0;
		double first_y = 0.0;
		double second_x = 0.0;
		double second_y = 0.0;
		double third_x = 0.0;
		double third_y = 0.0;
		double[] returnPoint = new double[2];
		int numvertices = 0;
		switch (whichPoint)
		{
		case 1:
			first_x = p.x;
			first_y = p.y;
			second_x = r.x;
			second_y = r.y;
			third_x = q.x;
			third_y = q.y;
			break;
		case 2:
			first_x = q.x;
			first_y = q.y;
			second_x = p.x;
			second_y = p.y;
			third_x = r.x;
			third_y = r.y;
			break;
		case 3:
			first_x = r.x;
			first_y = r.y;
			second_x = q.x;
			second_y = q.y;
			third_x = p.x;
			third_y = p.y;
			break;
		}
		Otri tempotri = badotri;
		points[numvertices] = second_x;
		numvertices++;
		points[numvertices] = second_y;
		numvertices++;
		returnPoint[0] = second_x;
		returnPoint[1] = second_y;
		do
		{
			if (!GetNeighborsVertex(tempotri, first_x, first_y, second_x, second_y, ref returnPoint, ref neighotri))
			{
				tempotri = neighotri;
				second_x = returnPoint[0];
				second_y = returnPoint[1];
				points[numvertices] = returnPoint[0];
				numvertices++;
				points[numvertices] = returnPoint[1];
				numvertices++;
				continue;
			}
			numvertices = 0;
			break;
		}
		while (!(Math.Abs(returnPoint[0] - third_x) <= 1E-50) || !(Math.Abs(returnPoint[1] - third_y) <= 1E-50));
		return numvertices / 2;
	}

	private bool GetNeighborsVertex(Otri badotri, double first_x, double first_y, double second_x, double second_y, ref double[] thirdpoint, ref Otri neighotri)
	{
		Otri neighbor = default(Otri);
		bool notFound = false;
		Vertex neighborvertex_1 = null;
		Vertex neighborvertex_2 = null;
		Vertex neighborvertex_3 = null;
		int firstVertexMatched = 0;
		int secondVertexMatched = 0;
		badotri.orient = 0;
		while (badotri.orient < 3)
		{
			badotri.Sym(ref neighbor);
			if (neighbor.tri.id != -1)
			{
				neighborvertex_1 = neighbor.Org();
				neighborvertex_2 = neighbor.Dest();
				neighborvertex_3 = neighbor.Apex();
				if ((neighborvertex_1.x != neighborvertex_2.x || neighborvertex_1.y != neighborvertex_2.y) && (neighborvertex_2.x != neighborvertex_3.x || neighborvertex_2.y != neighborvertex_3.y) && (neighborvertex_1.x != neighborvertex_3.x || neighborvertex_1.y != neighborvertex_3.y))
				{
					firstVertexMatched = 0;
					if (Math.Abs(first_x - neighborvertex_1.x) < 1E-50 && Math.Abs(first_y - neighborvertex_1.y) < 1E-50)
					{
						firstVertexMatched = 11;
					}
					else if (Math.Abs(first_x - neighborvertex_2.x) < 1E-50 && Math.Abs(first_y - neighborvertex_2.y) < 1E-50)
					{
						firstVertexMatched = 12;
					}
					else if (Math.Abs(first_x - neighborvertex_3.x) < 1E-50 && Math.Abs(first_y - neighborvertex_3.y) < 1E-50)
					{
						firstVertexMatched = 13;
					}
					secondVertexMatched = 0;
					if (Math.Abs(second_x - neighborvertex_1.x) < 1E-50 && Math.Abs(second_y - neighborvertex_1.y) < 1E-50)
					{
						secondVertexMatched = 21;
					}
					else if (Math.Abs(second_x - neighborvertex_2.x) < 1E-50 && Math.Abs(second_y - neighborvertex_2.y) < 1E-50)
					{
						secondVertexMatched = 22;
					}
					else if (Math.Abs(second_x - neighborvertex_3.x) < 1E-50 && Math.Abs(second_y - neighborvertex_3.y) < 1E-50)
					{
						secondVertexMatched = 23;
					}
				}
			}
			if ((firstVertexMatched == 11 && (secondVertexMatched == 22 || secondVertexMatched == 23)) || (firstVertexMatched == 12 && (secondVertexMatched == 21 || secondVertexMatched == 23)) || (firstVertexMatched == 13 && (secondVertexMatched == 21 || secondVertexMatched == 22)))
			{
				break;
			}
			badotri.orient++;
		}
		switch (firstVertexMatched)
		{
		case 0:
			notFound = true;
			break;
		case 11:
			switch (secondVertexMatched)
			{
			case 22:
				thirdpoint[0] = neighborvertex_3.x;
				thirdpoint[1] = neighborvertex_3.y;
				break;
			case 23:
				thirdpoint[0] = neighborvertex_2.x;
				thirdpoint[1] = neighborvertex_2.y;
				break;
			default:
				notFound = true;
				break;
			}
			break;
		case 12:
			switch (secondVertexMatched)
			{
			case 21:
				thirdpoint[0] = neighborvertex_3.x;
				thirdpoint[1] = neighborvertex_3.y;
				break;
			case 23:
				thirdpoint[0] = neighborvertex_1.x;
				thirdpoint[1] = neighborvertex_1.y;
				break;
			default:
				notFound = true;
				break;
			}
			break;
		case 13:
			switch (secondVertexMatched)
			{
			case 21:
				thirdpoint[0] = neighborvertex_2.x;
				thirdpoint[1] = neighborvertex_2.y;
				break;
			case 22:
				thirdpoint[0] = neighborvertex_1.x;
				thirdpoint[1] = neighborvertex_1.y;
				break;
			default:
				notFound = true;
				break;
			}
			break;
		default:
			if (secondVertexMatched == 0)
			{
				notFound = true;
			}
			break;
		}
		neighotri = neighbor;
		return notFound;
	}

	private bool GetWedgeIntersectionWithoutMaxAngle(int numpoints, double[] points, ref double[] newloc)
	{
		if (2 * numpoints > petalx.Length)
		{
			petalx = new double[2 * numpoints];
			petaly = new double[2 * numpoints];
			petalr = new double[2 * numpoints];
			wedges = new double[2 * numpoints * 16 + 36];
		}
		double[] p1 = new double[3];
		int numpolypoints = 0;
		double x0 = points[2 * numpoints - 4];
		double y0 = points[2 * numpoints - 3];
		double x1 = points[2 * numpoints - 2];
		double y1 = points[2 * numpoints - 1];
		double alpha = behavior.MinAngle * Math.PI / 180.0;
		double petalcenterconstant;
		double petalradiusconstant;
		if (behavior.goodAngle == 1.0)
		{
			petalcenterconstant = 0.0;
			petalradiusconstant = 0.0;
		}
		else
		{
			petalcenterconstant = 0.5 / Math.Tan(alpha);
			petalradiusconstant = 0.5 / Math.Sin(alpha);
		}
		for (int i = 0; i < numpoints * 2; i += 2)
		{
			double x2 = points[i];
			double y2 = points[i + 1];
			double x3 = x1 - x0;
			double y3 = y1 - y0;
			double d01 = Math.Sqrt(x3 * x3 + y3 * y3);
			petalx[i / 2] = x0 + 0.5 * x3 - petalcenterconstant * y3;
			petaly[i / 2] = y0 + 0.5 * y3 + petalcenterconstant * x3;
			petalr[i / 2] = petalradiusconstant * d01;
			petalx[numpoints + i / 2] = petalx[i / 2];
			petaly[numpoints + i / 2] = petaly[i / 2];
			petalr[numpoints + i / 2] = petalr[i / 2];
			double xmid = (x0 + x1) / 2.0;
			double ymid = (y0 + y1) / 2.0;
			double dist = Math.Sqrt((petalx[i / 2] - xmid) * (petalx[i / 2] - xmid) + (petaly[i / 2] - ymid) * (petaly[i / 2] - ymid));
			double ux = (petalx[i / 2] - xmid) / dist;
			double uy = (petaly[i / 2] - ymid) / dist;
			double x4 = petalx[i / 2] + ux * petalr[i / 2];
			double y4 = petaly[i / 2] + uy * petalr[i / 2];
			ux = x1 - x0;
			uy = y1 - y0;
			double x_1 = x1 * Math.Cos(alpha) - y1 * Math.Sin(alpha) + x0 - x0 * Math.Cos(alpha) + y0 * Math.Sin(alpha);
			double y_1 = x1 * Math.Sin(alpha) + y1 * Math.Cos(alpha) + y0 - x0 * Math.Sin(alpha) - y0 * Math.Cos(alpha);
			wedges[i * 16] = x0;
			wedges[i * 16 + 1] = y0;
			wedges[i * 16 + 2] = x_1;
			wedges[i * 16 + 3] = y_1;
			ux = x0 - x1;
			uy = y0 - y1;
			double x_2 = x0 * Math.Cos(alpha) + y0 * Math.Sin(alpha) + x1 - x1 * Math.Cos(alpha) - y1 * Math.Sin(alpha);
			double y_2 = (0.0 - x0) * Math.Sin(alpha) + y0 * Math.Cos(alpha) + y1 + x1 * Math.Sin(alpha) - y1 * Math.Cos(alpha);
			wedges[i * 16 + 4] = x_2;
			wedges[i * 16 + 5] = y_2;
			wedges[i * 16 + 6] = x1;
			wedges[i * 16 + 7] = y1;
			ux = x4 - petalx[i / 2];
			uy = y4 - petaly[i / 2];
			double tempx = x4;
			double tempy = y4;
			for (int j = 1; j < 4; j++)
			{
				double x_3 = x4 * Math.Cos((Math.PI / 3.0 - alpha) * (double)j) + y4 * Math.Sin((Math.PI / 3.0 - alpha) * (double)j) + petalx[i / 2] - petalx[i / 2] * Math.Cos((Math.PI / 3.0 - alpha) * (double)j) - petaly[i / 2] * Math.Sin((Math.PI / 3.0 - alpha) * (double)j);
				double y_3 = (0.0 - x4) * Math.Sin((Math.PI / 3.0 - alpha) * (double)j) + y4 * Math.Cos((Math.PI / 3.0 - alpha) * (double)j) + petaly[i / 2] + petalx[i / 2] * Math.Sin((Math.PI / 3.0 - alpha) * (double)j) - petaly[i / 2] * Math.Cos((Math.PI / 3.0 - alpha) * (double)j);
				wedges[i * 16 + 8 + 4 * (j - 1)] = x_3;
				wedges[i * 16 + 9 + 4 * (j - 1)] = y_3;
				wedges[i * 16 + 10 + 4 * (j - 1)] = tempx;
				wedges[i * 16 + 11 + 4 * (j - 1)] = tempy;
				tempx = x_3;
				tempy = y_3;
			}
			tempx = x4;
			tempy = y4;
			for (int j = 1; j < 4; j++)
			{
				double x_4 = x4 * Math.Cos((Math.PI / 3.0 - alpha) * (double)j) - y4 * Math.Sin((Math.PI / 3.0 - alpha) * (double)j) + petalx[i / 2] - petalx[i / 2] * Math.Cos((Math.PI / 3.0 - alpha) * (double)j) + petaly[i / 2] * Math.Sin((Math.PI / 3.0 - alpha) * (double)j);
				double y_4 = x4 * Math.Sin((Math.PI / 3.0 - alpha) * (double)j) + y4 * Math.Cos((Math.PI / 3.0 - alpha) * (double)j) + petaly[i / 2] - petalx[i / 2] * Math.Sin((Math.PI / 3.0 - alpha) * (double)j) - petaly[i / 2] * Math.Cos((Math.PI / 3.0 - alpha) * (double)j);
				wedges[i * 16 + 20 + 4 * (j - 1)] = tempx;
				wedges[i * 16 + 21 + 4 * (j - 1)] = tempy;
				wedges[i * 16 + 22 + 4 * (j - 1)] = x_4;
				wedges[i * 16 + 23 + 4 * (j - 1)] = y_4;
				tempx = x_4;
				tempy = y_4;
			}
			if (i == 0)
			{
				LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_2, y_2, ref p1);
				if (p1[0] == 1.0)
				{
					initialConvexPoly[0] = p1[1];
					initialConvexPoly[1] = p1[2];
					initialConvexPoly[2] = wedges[i * 16 + 16];
					initialConvexPoly[3] = wedges[i * 16 + 17];
					initialConvexPoly[4] = wedges[i * 16 + 12];
					initialConvexPoly[5] = wedges[i * 16 + 13];
					initialConvexPoly[6] = wedges[i * 16 + 8];
					initialConvexPoly[7] = wedges[i * 16 + 9];
					initialConvexPoly[8] = x4;
					initialConvexPoly[9] = y4;
					initialConvexPoly[10] = wedges[i * 16 + 22];
					initialConvexPoly[11] = wedges[i * 16 + 23];
					initialConvexPoly[12] = wedges[i * 16 + 26];
					initialConvexPoly[13] = wedges[i * 16 + 27];
					initialConvexPoly[14] = wedges[i * 16 + 30];
					initialConvexPoly[15] = wedges[i * 16 + 31];
				}
			}
			x0 = x1;
			y0 = y1;
			x1 = x2;
			y1 = y2;
		}
		if (numpoints != 0)
		{
			int s = (numpoints - 1) / 2 + 1;
			int flag = 0;
			int count = 0;
			int i = 1;
			int num = 8;
			for (int j = 0; j < 32; j += 4)
			{
				numpolypoints = HalfPlaneIntersection(num, ref initialConvexPoly, wedges[32 * s + j], wedges[32 * s + 1 + j], wedges[32 * s + 2 + j], wedges[32 * s + 3 + j]);
				if (numpolypoints == 0)
				{
					return false;
				}
				num = numpolypoints;
			}
			for (count++; count < numpoints - 1; count++)
			{
				for (int j = 0; j < 32; j += 4)
				{
					numpolypoints = HalfPlaneIntersection(num, ref initialConvexPoly, wedges[32 * (i + s * flag) + j], wedges[32 * (i + s * flag) + 1 + j], wedges[32 * (i + s * flag) + 2 + j], wedges[32 * (i + s * flag) + 3 + j]);
					if (numpolypoints == 0)
					{
						return false;
					}
					num = numpolypoints;
				}
				i += flag;
				flag = (flag + 1) % 2;
			}
			FindPolyCentroid(numpolypoints, initialConvexPoly, ref newloc);
			if (!behavior.fixedArea)
			{
				return true;
			}
		}
		return false;
	}

	private bool GetWedgeIntersection(int numpoints, double[] points, ref double[] newloc)
	{
		if (2 * numpoints > petalx.Length)
		{
			petalx = new double[2 * numpoints];
			petaly = new double[2 * numpoints];
			petalr = new double[2 * numpoints];
			wedges = new double[2 * numpoints * 20 + 40];
		}
		double[] p1 = new double[3];
		double[] p2 = new double[3];
		double[] p3 = new double[3];
		double[] p4 = new double[3];
		int numpolypoints = 0;
		int howManyPoints = 0;
		double line345 = 4.0;
		double line789 = 4.0;
		double x0 = points[2 * numpoints - 4];
		double y0 = points[2 * numpoints - 3];
		double x1 = points[2 * numpoints - 2];
		double y1 = points[2 * numpoints - 1];
		double alpha = behavior.MinAngle * Math.PI / 180.0;
		double sinAlpha = Math.Sin(alpha);
		double cosAlpha = Math.Cos(alpha);
		double num = behavior.MaxAngle * Math.PI / 180.0;
		double sinBeta = Math.Sin(num);
		double cosBeta = Math.Cos(num);
		double petalcenterconstant;
		double petalradiusconstant;
		if (behavior.goodAngle == 1.0)
		{
			petalcenterconstant = 0.0;
			petalradiusconstant = 0.0;
		}
		else
		{
			petalcenterconstant = 0.5 / Math.Tan(alpha);
			petalradiusconstant = 0.5 / Math.Sin(alpha);
		}
		for (int i = 0; i < numpoints * 2; i += 2)
		{
			double x2 = points[i];
			double y2 = points[i + 1];
			double x3 = x1 - x0;
			double y3 = y1 - y0;
			double d01 = Math.Sqrt(x3 * x3 + y3 * y3);
			petalx[i / 2] = x0 + 0.5 * x3 - petalcenterconstant * y3;
			petaly[i / 2] = y0 + 0.5 * y3 + petalcenterconstant * x3;
			petalr[i / 2] = petalradiusconstant * d01;
			petalx[numpoints + i / 2] = petalx[i / 2];
			petaly[numpoints + i / 2] = petaly[i / 2];
			petalr[numpoints + i / 2] = petalr[i / 2];
			double xmid = (x0 + x1) / 2.0;
			double ymid = (y0 + y1) / 2.0;
			double dist = Math.Sqrt((petalx[i / 2] - xmid) * (petalx[i / 2] - xmid) + (petaly[i / 2] - ymid) * (petaly[i / 2] - ymid));
			double ux = (petalx[i / 2] - xmid) / dist;
			double uy = (petaly[i / 2] - ymid) / dist;
			double x4 = petalx[i / 2] + ux * petalr[i / 2];
			double y4 = petaly[i / 2] + uy * petalr[i / 2];
			ux = x1 - x0;
			uy = y1 - y0;
			double x_1 = x1 * cosAlpha - y1 * sinAlpha + x0 - x0 * cosAlpha + y0 * sinAlpha;
			double y_1 = x1 * sinAlpha + y1 * cosAlpha + y0 - x0 * sinAlpha - y0 * cosAlpha;
			wedges[i * 20] = x0;
			wedges[i * 20 + 1] = y0;
			wedges[i * 20 + 2] = x_1;
			wedges[i * 20 + 3] = y_1;
			ux = x0 - x1;
			uy = y0 - y1;
			double x_2 = x0 * cosAlpha + y0 * sinAlpha + x1 - x1 * cosAlpha - y1 * sinAlpha;
			double y_2 = (0.0 - x0) * sinAlpha + y0 * cosAlpha + y1 + x1 * sinAlpha - y1 * cosAlpha;
			wedges[i * 20 + 4] = x_2;
			wedges[i * 20 + 5] = y_2;
			wedges[i * 20 + 6] = x1;
			wedges[i * 20 + 7] = y1;
			ux = x4 - petalx[i / 2];
			uy = y4 - petaly[i / 2];
			double tempx = x4;
			double tempy = y4;
			alpha = 2.0 * behavior.MaxAngle + behavior.MinAngle - 180.0;
			if (alpha <= 0.0)
			{
				howManyPoints = 4;
				line345 = 1.0;
				line789 = 1.0;
			}
			else if (alpha <= 5.0)
			{
				howManyPoints = 6;
				line345 = 2.0;
				line789 = 2.0;
			}
			else if (alpha <= 10.0)
			{
				howManyPoints = 8;
				line345 = 3.0;
				line789 = 3.0;
			}
			else
			{
				howManyPoints = 10;
				line345 = 4.0;
				line789 = 4.0;
			}
			alpha = alpha * Math.PI / 180.0;
			for (int j = 1; (double)j < line345; j++)
			{
				if (line345 != 1.0)
				{
					double x_3 = x4 * Math.Cos(alpha / (line345 - 1.0) * (double)j) + y4 * Math.Sin(alpha / (line345 - 1.0) * (double)j) + petalx[i / 2] - petalx[i / 2] * Math.Cos(alpha / (line345 - 1.0) * (double)j) - petaly[i / 2] * Math.Sin(alpha / (line345 - 1.0) * (double)j);
					double y_3 = (0.0 - x4) * Math.Sin(alpha / (line345 - 1.0) * (double)j) + y4 * Math.Cos(alpha / (line345 - 1.0) * (double)j) + petaly[i / 2] + petalx[i / 2] * Math.Sin(alpha / (line345 - 1.0) * (double)j) - petaly[i / 2] * Math.Cos(alpha / (line345 - 1.0) * (double)j);
					wedges[i * 20 + 8 + 4 * (j - 1)] = x_3;
					wedges[i * 20 + 9 + 4 * (j - 1)] = y_3;
					wedges[i * 20 + 10 + 4 * (j - 1)] = tempx;
					wedges[i * 20 + 11 + 4 * (j - 1)] = tempy;
					tempx = x_3;
					tempy = y_3;
				}
			}
			ux = x0 - x1;
			uy = y0 - y1;
			double x_5 = x0 * cosBeta + y0 * sinBeta + x1 - x1 * cosBeta - y1 * sinBeta;
			double y_5 = (0.0 - x0) * sinBeta + y0 * cosBeta + y1 + x1 * sinBeta - y1 * cosBeta;
			wedges[i * 20 + 20] = x1;
			wedges[i * 20 + 21] = y1;
			wedges[i * 20 + 22] = x_5;
			wedges[i * 20 + 23] = y_5;
			tempx = x4;
			tempy = y4;
			for (int j = 1; (double)j < line789; j++)
			{
				if (line789 != 1.0)
				{
					double x_6 = x4 * Math.Cos(alpha / (line789 - 1.0) * (double)j) - y4 * Math.Sin(alpha / (line789 - 1.0) * (double)j) + petalx[i / 2] - petalx[i / 2] * Math.Cos(alpha / (line789 - 1.0) * (double)j) + petaly[i / 2] * Math.Sin(alpha / (line789 - 1.0) * (double)j);
					double y_6 = x4 * Math.Sin(alpha / (line789 - 1.0) * (double)j) + y4 * Math.Cos(alpha / (line789 - 1.0) * (double)j) + petaly[i / 2] - petalx[i / 2] * Math.Sin(alpha / (line789 - 1.0) * (double)j) - petaly[i / 2] * Math.Cos(alpha / (line789 - 1.0) * (double)j);
					wedges[i * 20 + 24 + 4 * (j - 1)] = tempx;
					wedges[i * 20 + 25 + 4 * (j - 1)] = tempy;
					wedges[i * 20 + 26 + 4 * (j - 1)] = x_6;
					wedges[i * 20 + 27 + 4 * (j - 1)] = y_6;
					tempx = x_6;
					tempy = y_6;
				}
			}
			ux = x1 - x0;
			uy = y1 - y0;
			double x_7 = x1 * cosBeta - y1 * sinBeta + x0 - x0 * cosBeta + y0 * sinBeta;
			double y_7 = x1 * sinBeta + y1 * cosBeta + y0 - x0 * sinBeta - y0 * cosBeta;
			wedges[i * 20 + 36] = x_7;
			wedges[i * 20 + 37] = y_7;
			wedges[i * 20 + 38] = x0;
			wedges[i * 20 + 39] = y0;
			if (i == 0)
			{
				switch (howManyPoints)
				{
				case 4:
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_2, y_2, ref p1);
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_5, y_5, ref p2);
					LineLineIntersection(x0, y0, x_7, y_7, x1, y1, x_5, y_5, ref p3);
					LineLineIntersection(x0, y0, x_7, y_7, x1, y1, x_2, y_2, ref p4);
					if (p1[0] == 1.0 && p2[0] == 1.0 && p3[0] == 1.0 && p4[0] == 1.0)
					{
						initialConvexPoly[0] = p1[1];
						initialConvexPoly[1] = p1[2];
						initialConvexPoly[2] = p2[1];
						initialConvexPoly[3] = p2[2];
						initialConvexPoly[4] = p3[1];
						initialConvexPoly[5] = p3[2];
						initialConvexPoly[6] = p4[1];
						initialConvexPoly[7] = p4[2];
					}
					break;
				case 6:
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_2, y_2, ref p1);
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_5, y_5, ref p2);
					LineLineIntersection(x0, y0, x_7, y_7, x1, y1, x_2, y_2, ref p3);
					if (p1[0] == 1.0 && p2[0] == 1.0 && p3[0] == 1.0)
					{
						initialConvexPoly[0] = p1[1];
						initialConvexPoly[1] = p1[2];
						initialConvexPoly[2] = p2[1];
						initialConvexPoly[3] = p2[2];
						initialConvexPoly[4] = wedges[i * 20 + 8];
						initialConvexPoly[5] = wedges[i * 20 + 9];
						initialConvexPoly[6] = x4;
						initialConvexPoly[7] = y4;
						initialConvexPoly[8] = wedges[i * 20 + 26];
						initialConvexPoly[9] = wedges[i * 20 + 27];
						initialConvexPoly[10] = p3[1];
						initialConvexPoly[11] = p3[2];
					}
					break;
				case 8:
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_2, y_2, ref p1);
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_5, y_5, ref p2);
					LineLineIntersection(x0, y0, x_7, y_7, x1, y1, x_2, y_2, ref p3);
					if (p1[0] == 1.0 && p2[0] == 1.0 && p3[0] == 1.0)
					{
						initialConvexPoly[0] = p1[1];
						initialConvexPoly[1] = p1[2];
						initialConvexPoly[2] = p2[1];
						initialConvexPoly[3] = p2[2];
						initialConvexPoly[4] = wedges[i * 20 + 12];
						initialConvexPoly[5] = wedges[i * 20 + 13];
						initialConvexPoly[6] = wedges[i * 20 + 8];
						initialConvexPoly[7] = wedges[i * 20 + 9];
						initialConvexPoly[8] = x4;
						initialConvexPoly[9] = y4;
						initialConvexPoly[10] = wedges[i * 20 + 26];
						initialConvexPoly[11] = wedges[i * 20 + 27];
						initialConvexPoly[12] = wedges[i * 20 + 30];
						initialConvexPoly[13] = wedges[i * 20 + 31];
						initialConvexPoly[14] = p3[1];
						initialConvexPoly[15] = p3[2];
					}
					break;
				case 10:
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_2, y_2, ref p1);
					LineLineIntersection(x0, y0, x_1, y_1, x1, y1, x_5, y_5, ref p2);
					LineLineIntersection(x0, y0, x_7, y_7, x1, y1, x_2, y_2, ref p3);
					if (p1[0] == 1.0 && p2[0] == 1.0 && p3[0] == 1.0)
					{
						initialConvexPoly[0] = p1[1];
						initialConvexPoly[1] = p1[2];
						initialConvexPoly[2] = p2[1];
						initialConvexPoly[3] = p2[2];
						initialConvexPoly[4] = wedges[i * 20 + 16];
						initialConvexPoly[5] = wedges[i * 20 + 17];
						initialConvexPoly[6] = wedges[i * 20 + 12];
						initialConvexPoly[7] = wedges[i * 20 + 13];
						initialConvexPoly[8] = wedges[i * 20 + 8];
						initialConvexPoly[9] = wedges[i * 20 + 9];
						initialConvexPoly[10] = x4;
						initialConvexPoly[11] = y4;
						initialConvexPoly[12] = wedges[i * 20 + 28];
						initialConvexPoly[13] = wedges[i * 20 + 29];
						initialConvexPoly[14] = wedges[i * 20 + 32];
						initialConvexPoly[15] = wedges[i * 20 + 33];
						initialConvexPoly[16] = wedges[i * 20 + 34];
						initialConvexPoly[17] = wedges[i * 20 + 35];
						initialConvexPoly[18] = p3[1];
						initialConvexPoly[19] = p3[2];
					}
					break;
				}
			}
			x0 = x1;
			y0 = y1;
			x1 = x2;
			y1 = y2;
		}
		if (numpoints != 0)
		{
			int s = (numpoints - 1) / 2 + 1;
			int flag = 0;
			int count = 0;
			int i = 1;
			int num2 = howManyPoints;
			for (int j = 0; j < 40; j += 4)
			{
				if ((howManyPoints != 4 || (j != 8 && j != 12 && j != 16 && j != 24 && j != 28 && j != 32)) && (howManyPoints != 6 || (j != 12 && j != 16 && j != 28 && j != 32)) && (howManyPoints != 8 || (j != 16 && j != 32)))
				{
					numpolypoints = HalfPlaneIntersection(num2, ref initialConvexPoly, wedges[40 * s + j], wedges[40 * s + 1 + j], wedges[40 * s + 2 + j], wedges[40 * s + 3 + j]);
					if (numpolypoints == 0)
					{
						return false;
					}
					num2 = numpolypoints;
				}
			}
			for (count++; count < numpoints - 1; count++)
			{
				for (int j = 0; j < 40; j += 4)
				{
					if ((howManyPoints != 4 || (j != 8 && j != 12 && j != 16 && j != 24 && j != 28 && j != 32)) && (howManyPoints != 6 || (j != 12 && j != 16 && j != 28 && j != 32)) && (howManyPoints != 8 || (j != 16 && j != 32)))
					{
						numpolypoints = HalfPlaneIntersection(num2, ref initialConvexPoly, wedges[40 * (i + s * flag) + j], wedges[40 * (i + s * flag) + 1 + j], wedges[40 * (i + s * flag) + 2 + j], wedges[40 * (i + s * flag) + 3 + j]);
						if (numpolypoints == 0)
						{
							return false;
						}
						num2 = numpolypoints;
					}
				}
				i += flag;
				flag = (flag + 1) % 2;
			}
			FindPolyCentroid(numpolypoints, initialConvexPoly, ref newloc);
			if (behavior.MaxAngle == 0.0)
			{
				return true;
			}
			int numBadTriangle = 0;
			for (int j = 0; j < numpoints * 2 - 2; j += 2)
			{
				if (IsBadTriangleAngle(newloc[0], newloc[1], points[j], points[j + 1], points[j + 2], points[j + 3]))
				{
					numBadTriangle++;
				}
			}
			if (IsBadTriangleAngle(newloc[0], newloc[1], points[0], points[1], points[numpoints * 2 - 2], points[numpoints * 2 - 1]))
			{
				numBadTriangle++;
			}
			if (numBadTriangle == 0)
			{
				return true;
			}
			int n = ((numpoints <= 2) ? 20 : 30);
			for (int k = 0; k < 2 * numpoints; k += 2)
			{
				for (int e = 1; e < n; e++)
				{
					newloc[0] = 0.0;
					newloc[1] = 0.0;
					for (i = 0; i < 2 * numpoints; i += 2)
					{
						double weight = 1.0 / (double)numpoints;
						if (i == k)
						{
							newloc[0] = newloc[0] + 0.1 * (double)e * weight * points[i];
							newloc[1] = newloc[1] + 0.1 * (double)e * weight * points[i + 1];
						}
						else
						{
							weight = (1.0 - 0.1 * (double)e * weight) / ((double)numpoints - 1.0);
							newloc[0] = newloc[0] + weight * points[i];
							newloc[1] = newloc[1] + weight * points[i + 1];
						}
					}
					numBadTriangle = 0;
					for (int j = 0; j < numpoints * 2 - 2; j += 2)
					{
						if (IsBadTriangleAngle(newloc[0], newloc[1], points[j], points[j + 1], points[j + 2], points[j + 3]))
						{
							numBadTriangle++;
						}
					}
					if (IsBadTriangleAngle(newloc[0], newloc[1], points[0], points[1], points[numpoints * 2 - 2], points[numpoints * 2 - 1]))
					{
						numBadTriangle++;
					}
					if (numBadTriangle == 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool ValidPolygonAngles(int numpoints, double[] points)
	{
		for (int i = 0; i < numpoints; i++)
		{
			if (i == numpoints - 1)
			{
				if (IsBadPolygonAngle(points[i * 2], points[i * 2 + 1], points[0], points[1], points[2], points[3]))
				{
					return false;
				}
			}
			else if (i == numpoints - 2)
			{
				if (IsBadPolygonAngle(points[i * 2], points[i * 2 + 1], points[(i + 1) * 2], points[(i + 1) * 2 + 1], points[0], points[1]))
				{
					return false;
				}
			}
			else if (IsBadPolygonAngle(points[i * 2], points[i * 2 + 1], points[(i + 1) * 2], points[(i + 1) * 2 + 1], points[(i + 2) * 2], points[(i + 2) * 2 + 1]))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsBadPolygonAngle(double x1, double y1, double x2, double y2, double x3, double y3)
	{
		double dx12 = x1 - x2;
		double dy12 = y1 - y2;
		double dx23 = x2 - x3;
		double dy23 = y2 - y3;
		double num = x3 - x1;
		double dy31 = y3 - y1;
		double dist12 = dx12 * dx12 + dy12 * dy12;
		double dist23 = dx23 * dx23 + dy23 * dy23;
		double dist31 = num * num + dy31 * dy31;
		if (Math.Acos((dist12 + dist23 - dist31) / (2.0 * Math.Sqrt(dist12) * Math.Sqrt(dist23))) < 2.0 * Math.Acos(Math.Sqrt(behavior.goodAngle)))
		{
			return true;
		}
		return false;
	}

	private void LineLineIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, ref double[] p)
	{
		double denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
		double u_a = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
		double u_b = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
		if (Math.Abs(denom - 0.0) < 1E-50 && Math.Abs(u_b - 0.0) < 1E-50 && Math.Abs(u_a - 0.0) < 1E-50)
		{
			p[0] = 0.0;
			return;
		}
		if (Math.Abs(denom - 0.0) < 1E-50)
		{
			p[0] = 0.0;
			return;
		}
		p[0] = 1.0;
		u_a /= denom;
		u_b /= denom;
		p[1] = x1 + u_a * (x2 - x1);
		p[2] = y1 + u_a * (y2 - y1);
	}

	private int HalfPlaneIntersection(int numvertices, ref double[] convexPoly, double x1, double y1, double x2, double y2)
	{
		double[] res = null;
		int count = 0;
		int intFound = 0;
		double dx = x2 - x1;
		double dy = y2 - y1;
		int numpolys = SplitConvexPolygon(numvertices, convexPoly, x1, y1, x2, y2, polys);
		if (numpolys == 3)
		{
			count = numvertices;
		}
		else
		{
			for (int i = 0; i < numpolys; i++)
			{
				double min = double.MaxValue;
				double max = double.MinValue;
				double z;
				for (int j = 1; (double)j <= 2.0 * polys[i][0] - 1.0; j += 2)
				{
					z = dx * (polys[i][j + 1] - y1) - dy * (polys[i][j] - x1);
					min = ((z < min) ? z : min);
					max = ((z > max) ? z : max);
				}
				z = ((Math.Abs(min) > Math.Abs(max)) ? min : max);
				if (z > 0.0)
				{
					res = polys[i];
					intFound = 1;
					break;
				}
			}
			if (intFound == 1)
			{
				for (; (double)count < res[0]; count++)
				{
					convexPoly[2 * count] = res[2 * count + 1];
					convexPoly[2 * count + 1] = res[2 * count + 2];
				}
			}
		}
		return count;
	}

	private int SplitConvexPolygon(int numvertices, double[] convexPoly, double x1, double y1, double x2, double y2, double[][] polys)
	{
		int state = 0;
		double[] p = new double[3];
		int poly1counter = 0;
		int poly2counter = 0;
		double compConst = 1E-12;
		int case1 = 0;
		int case2 = 0;
		int case3 = 0;
		int case31 = 0;
		int case32 = 0;
		int case33 = 0;
		int case311 = 0;
		int case3111 = 0;
		for (int i = 0; i < 2 * numvertices; i += 2)
		{
			int j = ((i + 2 < 2 * numvertices) ? (i + 2) : 0);
			LineLineSegmentIntersection(x1, y1, x2, y2, convexPoly[i], convexPoly[i + 1], convexPoly[j], convexPoly[j + 1], ref p);
			if (Math.Abs(p[0] - 0.0) <= compConst)
			{
				if (state == 1)
				{
					poly2counter++;
					poly2[2 * poly2counter - 1] = convexPoly[j];
					poly2[2 * poly2counter] = convexPoly[j + 1];
				}
				else
				{
					poly1counter++;
					poly1[2 * poly1counter - 1] = convexPoly[j];
					poly1[2 * poly1counter] = convexPoly[j + 1];
				}
				case1++;
				continue;
			}
			if (Math.Abs(p[0] - 2.0) <= compConst)
			{
				poly1counter++;
				poly1[2 * poly1counter - 1] = convexPoly[j];
				poly1[2 * poly1counter] = convexPoly[j + 1];
				case2++;
				continue;
			}
			case3++;
			if (Math.Abs(p[1] - convexPoly[j]) <= compConst && Math.Abs(p[2] - convexPoly[j + 1]) <= compConst)
			{
				case31++;
				switch (state)
				{
				case 1:
					poly2counter++;
					poly2[2 * poly2counter - 1] = convexPoly[j];
					poly2[2 * poly2counter] = convexPoly[j + 1];
					poly1counter++;
					poly1[2 * poly1counter - 1] = convexPoly[j];
					poly1[2 * poly1counter] = convexPoly[j + 1];
					state++;
					break;
				case 0:
					case311++;
					poly1counter++;
					poly1[2 * poly1counter - 1] = convexPoly[j];
					poly1[2 * poly1counter] = convexPoly[j + 1];
					if (i + 4 < 2 * numvertices)
					{
						int s1 = LinePointLocation(x1, y1, x2, y2, convexPoly[i], convexPoly[i + 1]);
						int s2 = LinePointLocation(x1, y1, x2, y2, convexPoly[i + 4], convexPoly[i + 5]);
						if (s1 != s2 && s1 != 0 && s2 != 0)
						{
							case3111++;
							poly2counter++;
							poly2[2 * poly2counter - 1] = convexPoly[j];
							poly2[2 * poly2counter] = convexPoly[j + 1];
							state++;
						}
					}
					break;
				}
			}
			else if (!(Math.Abs(p[1] - convexPoly[i]) <= compConst) || !(Math.Abs(p[2] - convexPoly[i + 1]) <= compConst))
			{
				case32++;
				poly1counter++;
				poly1[2 * poly1counter - 1] = p[1];
				poly1[2 * poly1counter] = p[2];
				poly2counter++;
				poly2[2 * poly2counter - 1] = p[1];
				poly2[2 * poly2counter] = p[2];
				switch (state)
				{
				case 1:
					poly1counter++;
					poly1[2 * poly1counter - 1] = convexPoly[j];
					poly1[2 * poly1counter] = convexPoly[j + 1];
					break;
				case 0:
					poly2counter++;
					poly2[2 * poly2counter - 1] = convexPoly[j];
					poly2[2 * poly2counter] = convexPoly[j + 1];
					break;
				}
				state++;
			}
			else
			{
				case33++;
				if (state == 1)
				{
					poly2counter++;
					poly2[2 * poly2counter - 1] = convexPoly[j];
					poly2[2 * poly2counter] = convexPoly[j + 1];
				}
				else
				{
					poly1counter++;
					poly1[2 * poly1counter - 1] = convexPoly[j];
					poly1[2 * poly1counter] = convexPoly[j + 1];
				}
			}
		}
		int numpolys;
		if (state != 0 && state != 2)
		{
			numpolys = 3;
		}
		else
		{
			numpolys = ((state == 0) ? 1 : 2);
			poly1[0] = poly1counter;
			poly2[0] = poly2counter;
			polys[0] = poly1;
			if (state == 2)
			{
				polys[1] = poly2;
			}
		}
		return numpolys;
	}

	private int LinePointLocation(double x1, double y1, double x2, double y2, double x, double y)
	{
		if (Math.Atan((y2 - y1) / (x2 - x1)) * 180.0 / Math.PI == 90.0)
		{
			if (Math.Abs(x1 - x) <= 1E-11)
			{
				return 0;
			}
		}
		else if (Math.Abs(y1 + (y2 - y1) * (x - x1) / (x2 - x1) - y) <= 1E-50)
		{
			return 0;
		}
		double z = (x2 - x1) * (y - y1) - (y2 - y1) * (x - x1);
		if (Math.Abs(z - 0.0) <= 1E-11)
		{
			return 0;
		}
		if (z > 0.0)
		{
			return 1;
		}
		return 2;
	}

	private void LineLineSegmentIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, ref double[] p)
	{
		double compConst = 1E-13;
		double denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
		double u_a = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
		double u_b = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
		if (Math.Abs(denom - 0.0) < compConst)
		{
			if (Math.Abs(u_b - 0.0) < compConst && Math.Abs(u_a - 0.0) < compConst)
			{
				p[0] = 2.0;
			}
			else
			{
				p[0] = 0.0;
			}
			return;
		}
		u_b /= denom;
		u_a /= denom;
		if (u_b < 0.0 - compConst || u_b > 1.0 + compConst)
		{
			p[0] = 0.0;
			return;
		}
		p[0] = 1.0;
		p[1] = x1 + u_a * (x2 - x1);
		p[2] = y1 + u_a * (y2 - y1);
	}

	private void FindPolyCentroid(int numpoints, double[] points, ref double[] centroid)
	{
		centroid[0] = 0.0;
		centroid[1] = 0.0;
		for (int i = 0; i < 2 * numpoints; i += 2)
		{
			centroid[0] = centroid[0] + points[i];
			centroid[1] = centroid[1] + points[i + 1];
		}
		centroid[0] = centroid[0] / (double)numpoints;
		centroid[1] = centroid[1] / (double)numpoints;
	}

	private void CircleLineIntersection(double x1, double y1, double x2, double y2, double x3, double y3, double r, ref double[] p)
	{
		double a = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
		double b = 2.0 * ((x2 - x1) * (x1 - x3) + (y2 - y1) * (y1 - y3));
		double c = x3 * x3 + y3 * y3 + x1 * x1 + y1 * y1 - 2.0 * (x3 * x1 + y3 * y1) - r * r;
		double i = b * b - 4.0 * a * c;
		if (i < 0.0)
		{
			p[0] = 0.0;
		}
		else if (Math.Abs(i - 0.0) < 1E-50)
		{
			p[0] = 1.0;
			double mu = (0.0 - b) / (2.0 * a);
			p[1] = x1 + mu * (x2 - x1);
			p[2] = y1 + mu * (y2 - y1);
		}
		else if (i > 0.0 && !(Math.Abs(a - 0.0) < 1E-50))
		{
			p[0] = 2.0;
			double mu = (0.0 - b + Math.Sqrt(i)) / (2.0 * a);
			p[1] = x1 + mu * (x2 - x1);
			p[2] = y1 + mu * (y2 - y1);
			mu = (0.0 - b - Math.Sqrt(i)) / (2.0 * a);
			p[3] = x1 + mu * (x2 - x1);
			p[4] = y1 + mu * (y2 - y1);
		}
		else
		{
			p[0] = 0.0;
		}
	}

	private bool ChooseCorrectPoint(double x1, double y1, double x2, double y2, double x3, double y3, bool isObtuse)
	{
		double d1 = (x2 - x3) * (x2 - x3) + (y2 - y3) * (y2 - y3);
		double d2 = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
		if (isObtuse)
		{
			if (d2 >= d1)
			{
				return true;
			}
			return false;
		}
		if (d2 < d1)
		{
			return true;
		}
		return false;
	}

	private void PointBetweenPoints(double x1, double y1, double x2, double y2, double x, double y, ref double[] p)
	{
		if ((x2 - x) * (x2 - x) + (y2 - y) * (y2 - y) < (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1))
		{
			p[0] = 1.0;
			p[1] = (x - x2) * (x - x2) + (y - y2) * (y - y2);
			p[2] = x;
			p[3] = y;
		}
		else
		{
			p[0] = 0.0;
			p[1] = 0.0;
			p[2] = 0.0;
			p[3] = 0.0;
		}
	}

	private bool IsBadTriangleAngle(double x1, double y1, double x2, double y2, double x3, double y3)
	{
		double dxod = x1 - x2;
		double dyod = y1 - y2;
		double dxda = x2 - x3;
		double dyda = y2 - y3;
		double dxao = x3 - x1;
		double dyao = y3 - y1;
		double dxod2 = dxod * dxod;
		double dyod2 = dyod * dyod;
		double dxda2 = dxda * dxda;
		double dyda2 = dyda * dyda;
		double num = dxao * dxao;
		double dyao2 = dyao * dyao;
		double apexlen = dxod2 + dyod2;
		double orglen = dxda2 + dyda2;
		double destlen = num + dyao2;
		double angle;
		if (apexlen < orglen && apexlen < destlen)
		{
			angle = dxda * dxao + dyda * dyao;
			angle = angle * angle / (orglen * destlen);
		}
		else if (orglen < destlen)
		{
			angle = dxod * dxao + dyod * dyao;
			angle = angle * angle / (apexlen * destlen);
		}
		else
		{
			angle = dxod * dxda + dyod * dyda;
			angle = angle * angle / (apexlen * orglen);
		}
		double maxangle = ((apexlen > orglen && apexlen > destlen) ? ((orglen + destlen - apexlen) / (2.0 * Math.Sqrt(orglen * destlen))) : ((!(orglen > destlen)) ? ((apexlen + orglen - destlen) / (2.0 * Math.Sqrt(apexlen * orglen))) : ((apexlen + destlen - orglen) / (2.0 * Math.Sqrt(apexlen * destlen)))));
		if (angle > behavior.goodAngle || (behavior.MaxAngle != 0.0 && maxangle < behavior.maxGoodAngle))
		{
			return true;
		}
		return false;
	}

	private double MinDistanceToNeighbor(double newlocX, double newlocY, ref Otri searchtri)
	{
		Otri horiz = default(Otri);
		LocateResult intersect = LocateResult.Outside;
		Point newvertex = new Point(newlocX, newlocY);
		Vertex torg = searchtri.Org();
		Vertex tdest = searchtri.Dest();
		if (torg.x == newvertex.x && torg.y == newvertex.y)
		{
			intersect = LocateResult.OnVertex;
			searchtri.Copy(ref horiz);
		}
		else if (tdest.x == newvertex.x && tdest.y == newvertex.y)
		{
			searchtri.Lnext();
			intersect = LocateResult.OnVertex;
			searchtri.Copy(ref horiz);
		}
		else
		{
			double ahead = predicates.CounterClockwise(torg, tdest, newvertex);
			if (ahead < 0.0)
			{
				searchtri.Sym();
				searchtri.Copy(ref horiz);
				intersect = mesh.locator.PreciseLocate(newvertex, ref horiz, stopatsubsegment: false);
			}
			else if (ahead == 0.0)
			{
				if (torg.x < newvertex.x == newvertex.x < tdest.x && torg.y < newvertex.y == newvertex.y < tdest.y)
				{
					intersect = LocateResult.OnEdge;
					searchtri.Copy(ref horiz);
				}
			}
			else
			{
				searchtri.Copy(ref horiz);
				intersect = mesh.locator.PreciseLocate(newvertex, ref horiz, stopatsubsegment: false);
			}
		}
		if (intersect == LocateResult.OnVertex || intersect == LocateResult.Outside)
		{
			return 0.0;
		}
		Vertex v1 = horiz.Org();
		Vertex v2 = horiz.Dest();
		Vertex v3 = horiz.Apex();
		double d1 = (v1.x - newvertex.x) * (v1.x - newvertex.x) + (v1.y - newvertex.y) * (v1.y - newvertex.y);
		double d2 = (v2.x - newvertex.x) * (v2.x - newvertex.x) + (v2.y - newvertex.y) * (v2.y - newvertex.y);
		double d3 = (v3.x - newvertex.x) * (v3.x - newvertex.x) + (v3.y - newvertex.y) * (v3.y - newvertex.y);
		if (d1 <= d2 && d1 <= d3)
		{
			return d1;
		}
		if (d2 <= d3)
		{
			return d2;
		}
		return d3;
	}
}
