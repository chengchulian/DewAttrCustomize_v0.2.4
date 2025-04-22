using System;
using TriangleNet.Geometry;
using TriangleNet.Topology;

namespace TriangleNet.Tools;

public class QualityMeasure
{
	private class AreaMeasure
	{
		public double area_min = double.MaxValue;

		public double area_max = double.MinValue;

		public double area_total;

		public int area_zero;

		public void Reset()
		{
			area_min = double.MaxValue;
			area_max = double.MinValue;
			area_total = 0.0;
			area_zero = 0;
		}

		public double Measure(Point a, Point b, Point c)
		{
			double area = 0.5 * Math.Abs(a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y));
			area_min = Math.Min(area_min, area);
			area_max = Math.Max(area_max, area);
			area_total += area;
			if (area == 0.0)
			{
				area_zero++;
			}
			return area;
		}
	}

	private class AlphaMeasure
	{
		public double alpha_min;

		public double alpha_max;

		public double alpha_ave;

		public double alpha_area;

		public void Reset()
		{
			alpha_min = double.MaxValue;
			alpha_max = double.MinValue;
			alpha_ave = 0.0;
			alpha_area = 0.0;
		}

		private double acos(double c)
		{
			if (c <= -1.0)
			{
				return Math.PI;
			}
			if (1.0 <= c)
			{
				return 0.0;
			}
			return Math.Acos(c);
		}

		public double Measure(double ab, double bc, double ca, double area)
		{
			double alpha = double.MaxValue;
			double ab2 = ab * ab;
			double bc2 = bc * bc;
			double ca2 = ca * ca;
			double a_angle;
			double b_angle;
			double c_angle;
			if (ab != 0.0 || bc != 0.0 || ca != 0.0)
			{
				a_angle = ((ca != 0.0 && ab != 0.0) ? acos((ca2 + ab2 - bc2) / (2.0 * ca * ab)) : Math.PI);
				b_angle = ((ab != 0.0 && bc != 0.0) ? acos((ab2 + bc2 - ca2) / (2.0 * ab * bc)) : Math.PI);
				c_angle = ((bc != 0.0 && ca != 0.0) ? acos((bc2 + ca2 - ab2) / (2.0 * bc * ca)) : Math.PI);
			}
			else
			{
				a_angle = Math.PI * 2.0 / 3.0;
				b_angle = Math.PI * 2.0 / 3.0;
				c_angle = Math.PI * 2.0 / 3.0;
			}
			alpha = Math.Min(alpha, a_angle);
			alpha = Math.Min(alpha, b_angle);
			alpha = Math.Min(alpha, c_angle);
			alpha = alpha * 3.0 / Math.PI;
			alpha_ave += alpha;
			alpha_area += area * alpha;
			alpha_min = Math.Min(alpha, alpha_min);
			alpha_max = Math.Max(alpha, alpha_max);
			return alpha;
		}

		public void Normalize(int n, double area_total)
		{
			if (n > 0)
			{
				alpha_ave /= n;
			}
			else
			{
				alpha_ave = 0.0;
			}
			if (0.0 < area_total)
			{
				alpha_area /= area_total;
			}
			else
			{
				alpha_area = 0.0;
			}
		}
	}

	private class Q_Measure
	{
		public double q_min;

		public double q_max;

		public double q_ave;

		public double q_area;

		public void Reset()
		{
			q_min = double.MaxValue;
			q_max = double.MinValue;
			q_ave = 0.0;
			q_area = 0.0;
		}

		public double Measure(double ab, double bc, double ca, double area)
		{
			double q = (bc + ca - ab) * (ca + ab - bc) * (ab + bc - ca) / (ab * bc * ca);
			q_min = Math.Min(q_min, q);
			q_max = Math.Max(q_max, q);
			q_ave += q;
			q_area += q * area;
			return q;
		}

		public void Normalize(int n, double area_total)
		{
			if (n > 0)
			{
				q_ave /= n;
			}
			else
			{
				q_ave = 0.0;
			}
			if (area_total > 0.0)
			{
				q_area /= area_total;
			}
			else
			{
				q_area = 0.0;
			}
		}
	}

	private AreaMeasure areaMeasure;

	private AlphaMeasure alphaMeasure;

	private Q_Measure qMeasure;

	private Mesh mesh;

	public double AreaMinimum => areaMeasure.area_min;

	public double AreaMaximum => areaMeasure.area_max;

	public double AreaRatio => areaMeasure.area_max / areaMeasure.area_min;

	public double AlphaMinimum => alphaMeasure.alpha_min;

	public double AlphaMaximum => alphaMeasure.alpha_max;

	public double AlphaAverage => alphaMeasure.alpha_ave;

	public double AlphaArea => alphaMeasure.alpha_area;

	public double Q_Minimum => qMeasure.q_min;

	public double Q_Maximum => qMeasure.q_max;

	public double Q_Average => qMeasure.q_ave;

	public double Q_Area => qMeasure.q_area;

	public QualityMeasure()
	{
		areaMeasure = new AreaMeasure();
		alphaMeasure = new AlphaMeasure();
		qMeasure = new Q_Measure();
	}

	public void Update(Mesh mesh)
	{
		this.mesh = mesh;
		areaMeasure.Reset();
		alphaMeasure.Reset();
		qMeasure.Reset();
		Compute();
	}

	private void Compute()
	{
		int n = 0;
		foreach (Triangle triangle in mesh.triangles)
		{
			n++;
			Point a = triangle.vertices[0];
			Point b = triangle.vertices[1];
			Point c = triangle.vertices[2];
			double num = a.x - b.x;
			double ly = a.y - b.y;
			double ab = Math.Sqrt(num * num + ly * ly);
			double num2 = b.x - c.x;
			ly = b.y - c.y;
			double bc = Math.Sqrt(num2 * num2 + ly * ly);
			double num3 = c.x - a.x;
			ly = c.y - a.y;
			double ca = Math.Sqrt(num3 * num3 + ly * ly);
			double area = areaMeasure.Measure(a, b, c);
			alphaMeasure.Measure(ab, bc, ca, area);
			qMeasure.Measure(ab, bc, ca, area);
		}
		alphaMeasure.Normalize(n, areaMeasure.area_total);
		qMeasure.Normalize(n, areaMeasure.area_total);
	}

	public int Bandwidth()
	{
		if (mesh == null)
		{
			return 0;
		}
		int ml = 0;
		int mu = 0;
		foreach (Triangle tri in mesh.triangles)
		{
			for (int j = 0; j < 3; j++)
			{
				int gi = tri.GetVertex(j).id;
				for (int k = 0; k < 3; k++)
				{
					int gj = tri.GetVertex(k).id;
					mu = Math.Max(mu, gj - gi);
					ml = Math.Max(ml, gi - gj);
				}
			}
		}
		return ml + 1 + mu;
	}
}
