using System;
using System.Collections.Generic;
using TriangleNet.Topology;

namespace TriangleNet.Meshing.Iterators;

public class RegionIterator
{
	private List<Triangle> region;

	public RegionIterator(Mesh mesh)
	{
		region = new List<Triangle>();
	}

	public void Process(Triangle triangle, int boundary = 0)
	{
		Process(triangle, delegate(Triangle tri)
		{
			tri.label = triangle.label;
			tri.area = triangle.area;
		}, boundary);
	}

	public void Process(Triangle triangle, Action<Triangle> action, int boundary = 0)
	{
		if (triangle.id == -1 || Otri.IsDead(triangle))
		{
			return;
		}
		region.Add(triangle);
		triangle.infected = true;
		if (boundary == 0)
		{
			ProcessRegion(action, (SubSegment seg) => seg.hash == -1);
		}
		else
		{
			ProcessRegion(action, (SubSegment seg) => seg.boundary != boundary);
		}
		region.Clear();
	}

	private void ProcessRegion(Action<Triangle> action, Func<SubSegment, bool> protector)
	{
		Otri testtri = default(Otri);
		Otri neighbor = default(Otri);
		Osub neighborsubseg = default(Osub);
		for (int i = 0; i < region.Count; i++)
		{
			testtri.tri = region[i];
			action(testtri.tri);
			testtri.orient = 0;
			while (testtri.orient < 3)
			{
				testtri.Sym(ref neighbor);
				testtri.Pivot(ref neighborsubseg);
				if (neighbor.tri.id != -1 && !neighbor.IsInfected() && protector(neighborsubseg.seg))
				{
					neighbor.Infect();
					region.Add(neighbor.tri);
				}
				testtri.orient++;
			}
		}
		foreach (Triangle item in region)
		{
			item.infected = false;
		}
		region.Clear();
	}
}
