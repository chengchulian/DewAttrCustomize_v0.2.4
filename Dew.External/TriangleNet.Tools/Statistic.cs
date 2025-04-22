using System;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.Tools;

public class Statistic
{
	public static long InCircleCount = 0L;

	public static long InCircleAdaptCount = 0L;

	public static long CounterClockwiseCount = 0L;

	public static long CounterClockwiseAdaptCount = 0L;

	public static long Orient3dCount = 0L;

	public static long HyperbolaCount = 0L;

	public static long CircumcenterCount = 0L;

	public static long CircleTopCount = 0L;

	public static long RelocationCount = 0L;

	private double minEdge;

	private double maxEdge;

	private double minAspect;

	private double maxAspect;

	private double minArea;

	private double maxArea;

	private double minAngle;

	private double maxAngle;

	private int[] angleTable;

	private int[] minAngles;

	private int[] maxAngles;

	private static readonly int[] plus1Mod3 = new int[3] { 1, 2, 0 };

	private static readonly int[] minus1Mod3 = new int[3] { 2, 0, 1 };

	public double ShortestEdge => minEdge;

	public double LongestEdge => maxEdge;

	public double ShortestAltitude => minAspect;

	public double LargestAspectRatio => maxAspect;

	public double SmallestArea => minArea;

	public double LargestArea => maxArea;

	public double SmallestAngle => minAngle;

	public double LargestAngle => maxAngle;

	public int[] AngleHistogram => angleTable;

	public int[] MinAngleHistogram => minAngles;

	public int[] MaxAngleHistogram => maxAngles;

	private void GetAspectHistogram(Mesh mesh)
	{
		int[] aspecttable = new int[16];
		double[] ratiotable = new double[16]
		{
			1.5, 2.0, 2.5, 3.0, 4.0, 6.0, 10.0, 15.0, 25.0, 50.0,
			100.0, 300.0, 1000.0, 10000.0, 100000.0, 0.0
		};
		Otri tri = default(Otri);
		Vertex[] p = new Vertex[3];
		double[] dx = new double[3];
		double[] dy = new double[3];
		double[] edgelength = new double[3];
		tri.orient = 0;
		foreach (Triangle t in mesh.triangles)
		{
			tri.tri = t;
			p[0] = tri.Org();
			p[1] = tri.Dest();
			p[2] = tri.Apex();
			double trilongest2 = 0.0;
			for (int i = 0; i < 3; i++)
			{
				int j = plus1Mod3[i];
				int k = minus1Mod3[i];
				dx[i] = p[j].x - p[k].x;
				dy[i] = p[j].y - p[k].y;
				edgelength[i] = dx[i] * dx[i] + dy[i] * dy[i];
				if (edgelength[i] > trilongest2)
				{
					trilongest2 = edgelength[i];
				}
			}
			double num = Math.Abs((p[2].x - p[0].x) * (p[1].y - p[0].y) - (p[1].x - p[0].x) * (p[2].y - p[0].y)) / 2.0;
			double triminaltitude2 = num * num / trilongest2;
			double triaspect2 = trilongest2 / triminaltitude2;
			int aspectindex;
			for (aspectindex = 0; triaspect2 > ratiotable[aspectindex] * ratiotable[aspectindex] && aspectindex < 15; aspectindex++)
			{
			}
			aspecttable[aspectindex]++;
		}
	}

	public void Update(Mesh mesh, int sampleDegrees)
	{
		Point[] p = new Point[3];
		sampleDegrees = 60;
		double[] cosSquareTable = new double[sampleDegrees / 2 - 1];
		double[] dx = new double[3];
		double[] dy = new double[3];
		double[] edgeLength = new double[3];
		double radconst = Math.PI / (double)sampleDegrees;
		double degconst = 180.0 / Math.PI;
		angleTable = new int[sampleDegrees];
		minAngles = new int[sampleDegrees];
		maxAngles = new int[sampleDegrees];
		for (int i = 0; i < sampleDegrees / 2 - 1; i++)
		{
			cosSquareTable[i] = Math.Cos(radconst * (double)(i + 1));
			cosSquareTable[i] *= cosSquareTable[i];
		}
		for (int j = 0; j < sampleDegrees; j++)
		{
			angleTable[j] = 0;
		}
		minAspect = mesh.bounds.Width + mesh.bounds.Height;
		minAspect *= minAspect;
		maxAspect = 0.0;
		minEdge = minAspect;
		maxEdge = 0.0;
		minArea = minAspect;
		maxArea = 0.0;
		minAngle = 0.0;
		maxAngle = 2.0;
		bool acuteBiggest = true;
		bool acuteBiggestTri = true;
		double triMaxAngle = 1.0;
		foreach (Triangle tri in mesh.triangles)
		{
			double triMinAngle = 0.0;
			triMaxAngle = 1.0;
			p[0] = tri.vertices[0];
			p[1] = tri.vertices[1];
			p[2] = tri.vertices[2];
			double triLongest2 = 0.0;
			for (int k = 0; k < 3; k++)
			{
				int k2 = plus1Mod3[k];
				int k3 = minus1Mod3[k];
				dx[k] = p[k2].x - p[k3].x;
				dy[k] = p[k2].y - p[k3].y;
				edgeLength[k] = dx[k] * dx[k] + dy[k] * dy[k];
				if (edgeLength[k] > triLongest2)
				{
					triLongest2 = edgeLength[k];
				}
				if (edgeLength[k] > maxEdge)
				{
					maxEdge = edgeLength[k];
				}
				if (edgeLength[k] < minEdge)
				{
					minEdge = edgeLength[k];
				}
			}
			double triArea = Math.Abs((p[2].x - p[0].x) * (p[1].y - p[0].y) - (p[1].x - p[0].x) * (p[2].y - p[0].y));
			if (triArea < minArea)
			{
				minArea = triArea;
			}
			if (triArea > maxArea)
			{
				maxArea = triArea;
			}
			double triMinAltitude2 = triArea * triArea / triLongest2;
			if (triMinAltitude2 < minAspect)
			{
				minAspect = triMinAltitude2;
			}
			double triAspect2 = triLongest2 / triMinAltitude2;
			if (triAspect2 > maxAspect)
			{
				maxAspect = triAspect2;
			}
			int degreeStep;
			for (int l = 0; l < 3; l++)
			{
				int k2 = plus1Mod3[l];
				int k3 = minus1Mod3[l];
				double dotProduct = dx[k2] * dx[k3] + dy[k2] * dy[k3];
				double cosSquare = dotProduct * dotProduct / (edgeLength[k2] * edgeLength[k3]);
				degreeStep = sampleDegrees / 2 - 1;
				for (int j2 = degreeStep - 1; j2 >= 0; j2--)
				{
					if (cosSquare > cosSquareTable[j2])
					{
						degreeStep = j2;
					}
				}
				if (dotProduct <= 0.0)
				{
					angleTable[degreeStep]++;
					if (cosSquare > minAngle)
					{
						minAngle = cosSquare;
					}
					if (acuteBiggest && cosSquare < maxAngle)
					{
						maxAngle = cosSquare;
					}
					if (cosSquare > triMinAngle)
					{
						triMinAngle = cosSquare;
					}
					if (acuteBiggestTri && cosSquare < triMaxAngle)
					{
						triMaxAngle = cosSquare;
					}
				}
				else
				{
					angleTable[sampleDegrees - degreeStep - 1]++;
					if (acuteBiggest || cosSquare > maxAngle)
					{
						maxAngle = cosSquare;
						acuteBiggest = false;
					}
					if (acuteBiggestTri || cosSquare > triMaxAngle)
					{
						triMaxAngle = cosSquare;
						acuteBiggestTri = false;
					}
				}
			}
			degreeStep = sampleDegrees / 2 - 1;
			for (int j3 = degreeStep - 1; j3 >= 0; j3--)
			{
				if (triMinAngle > cosSquareTable[j3])
				{
					degreeStep = j3;
				}
			}
			minAngles[degreeStep]++;
			degreeStep = sampleDegrees / 2 - 1;
			for (int j4 = degreeStep - 1; j4 >= 0; j4--)
			{
				if (triMaxAngle > cosSquareTable[j4])
				{
					degreeStep = j4;
				}
			}
			if (acuteBiggestTri)
			{
				maxAngles[degreeStep]++;
			}
			else
			{
				maxAngles[sampleDegrees - degreeStep - 1]++;
			}
			acuteBiggestTri = true;
		}
		minEdge = Math.Sqrt(minEdge);
		maxEdge = Math.Sqrt(maxEdge);
		minAspect = Math.Sqrt(minAspect);
		maxAspect = Math.Sqrt(maxAspect);
		minArea *= 0.5;
		maxArea *= 0.5;
		if (minAngle >= 1.0)
		{
			minAngle = 0.0;
		}
		else
		{
			minAngle = degconst * Math.Acos(Math.Sqrt(minAngle));
		}
		if (maxAngle >= 1.0)
		{
			maxAngle = 180.0;
		}
		else if (acuteBiggest)
		{
			maxAngle = degconst * Math.Acos(Math.Sqrt(maxAngle));
		}
		else
		{
			maxAngle = 180.0 - degconst * Math.Acos(Math.Sqrt(maxAngle));
		}
	}
}
