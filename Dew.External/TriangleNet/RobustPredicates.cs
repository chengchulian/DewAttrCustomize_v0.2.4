using System;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace TriangleNet;

public class RobustPredicates : IPredicates
{
	private static readonly object creationLock;

	private static RobustPredicates _default;

	private static double epsilon;

	private static double splitter;

	private static double resulterrbound;

	private static double ccwerrboundA;

	private static double ccwerrboundB;

	private static double ccwerrboundC;

	private static double iccerrboundA;

	private static double iccerrboundB;

	private static double iccerrboundC;

	private double[] fin1;

	private double[] fin2;

	private double[] abdet;

	private double[] axbc;

	private double[] axxbc;

	private double[] aybc;

	private double[] ayybc;

	private double[] adet;

	private double[] bxca;

	private double[] bxxca;

	private double[] byca;

	private double[] byyca;

	private double[] bdet;

	private double[] cxab;

	private double[] cxxab;

	private double[] cyab;

	private double[] cyyab;

	private double[] cdet;

	private double[] temp8;

	private double[] temp16a;

	private double[] temp16b;

	private double[] temp16c;

	private double[] temp32a;

	private double[] temp32b;

	private double[] temp48;

	private double[] temp64;

	public static RobustPredicates Default
	{
		get
		{
			if (_default == null)
			{
				lock (creationLock)
				{
					if (_default == null)
					{
						_default = new RobustPredicates();
					}
				}
			}
			return _default;
		}
	}

	static RobustPredicates()
	{
		creationLock = new object();
		bool every_other = true;
		double half = 0.5;
		epsilon = 1.0;
		splitter = 1.0;
		double check = 1.0;
		double lastcheck;
		do
		{
			lastcheck = check;
			epsilon *= half;
			if (every_other)
			{
				splitter *= 2.0;
			}
			every_other = !every_other;
			check = 1.0 + epsilon;
		}
		while (check != 1.0 && check != lastcheck);
		splitter += 1.0;
		resulterrbound = (3.0 + 8.0 * epsilon) * epsilon;
		ccwerrboundA = (3.0 + 16.0 * epsilon) * epsilon;
		ccwerrboundB = (2.0 + 12.0 * epsilon) * epsilon;
		ccwerrboundC = (9.0 + 64.0 * epsilon) * epsilon * epsilon;
		iccerrboundA = (10.0 + 96.0 * epsilon) * epsilon;
		iccerrboundB = (4.0 + 48.0 * epsilon) * epsilon;
		iccerrboundC = (44.0 + 576.0 * epsilon) * epsilon * epsilon;
	}

	public RobustPredicates()
	{
		AllocateWorkspace();
	}

	public double CounterClockwise(Point pa, Point pb, Point pc)
	{
		Statistic.CounterClockwiseCount++;
		double detleft = (pa.x - pc.x) * (pb.y - pc.y);
		double detright = (pa.y - pc.y) * (pb.x - pc.x);
		double det = detleft - detright;
		if (Behavior.NoExact)
		{
			return det;
		}
		double detsum;
		if (detleft > 0.0)
		{
			if (detright <= 0.0)
			{
				return det;
			}
			detsum = detleft + detright;
		}
		else
		{
			if (!(detleft < 0.0))
			{
				return det;
			}
			if (detright >= 0.0)
			{
				return det;
			}
			detsum = 0.0 - detleft - detright;
		}
		double errbound = ccwerrboundA * detsum;
		if (det >= errbound || 0.0 - det >= errbound)
		{
			return det;
		}
		Statistic.CounterClockwiseAdaptCount++;
		return CounterClockwiseAdapt(pa, pb, pc, detsum);
	}

	public double InCircle(Point pa, Point pb, Point pc, Point pd)
	{
		Statistic.InCircleCount++;
		double adx = pa.x - pd.x;
		double num = pb.x - pd.x;
		double cdx = pc.x - pd.x;
		double ady = pa.y - pd.y;
		double bdy = pb.y - pd.y;
		double cdy = pc.y - pd.y;
		double bdxcdy = num * cdy;
		double cdxbdy = cdx * bdy;
		double alift = adx * adx + ady * ady;
		double cdxady = cdx * ady;
		double adxcdy = adx * cdy;
		double blift = num * num + bdy * bdy;
		double adxbdy = adx * bdy;
		double bdxady = num * ady;
		double clift = cdx * cdx + cdy * cdy;
		double det = alift * (bdxcdy - cdxbdy) + blift * (cdxady - adxcdy) + clift * (adxbdy - bdxady);
		if (Behavior.NoExact)
		{
			return det;
		}
		double permanent = (Math.Abs(bdxcdy) + Math.Abs(cdxbdy)) * alift + (Math.Abs(cdxady) + Math.Abs(adxcdy)) * blift + (Math.Abs(adxbdy) + Math.Abs(bdxady)) * clift;
		double errbound = iccerrboundA * permanent;
		if (det > errbound || 0.0 - det > errbound)
		{
			return det;
		}
		Statistic.InCircleAdaptCount++;
		return InCircleAdapt(pa, pb, pc, pd, permanent);
	}

	public double NonRegular(Point pa, Point pb, Point pc, Point pd)
	{
		return InCircle(pa, pb, pc, pd);
	}

	public Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta, double offconstant)
	{
		Statistic.CircumcenterCount++;
		double xdo = dest.x - org.x;
		double ydo = dest.y - org.y;
		double xao = apex.x - org.x;
		double yao = apex.y - org.y;
		double dodist = xdo * xdo + ydo * ydo;
		double aodist = xao * xao + yao * yao;
		double dadist = (dest.x - apex.x) * (dest.x - apex.x) + (dest.y - apex.y) * (dest.y - apex.y);
		double denominator;
		if (Behavior.NoExact)
		{
			denominator = 0.5 / (xdo * yao - xao * ydo);
		}
		else
		{
			denominator = 0.5 / CounterClockwise(dest, apex, org);
			Statistic.CounterClockwiseCount--;
		}
		double dx = (yao * dodist - ydo * aodist) * denominator;
		double dy = (xdo * aodist - xao * dodist) * denominator;
		if (dodist < aodist && dodist < dadist)
		{
			if (offconstant > 0.0)
			{
				double dxoff = 0.5 * xdo - offconstant * ydo;
				double dyoff = 0.5 * ydo + offconstant * xdo;
				if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
				{
					dx = dxoff;
					dy = dyoff;
				}
			}
		}
		else if (aodist < dadist)
		{
			if (offconstant > 0.0)
			{
				double dxoff = 0.5 * xao + offconstant * yao;
				double dyoff = 0.5 * yao - offconstant * xao;
				if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
				{
					dx = dxoff;
					dy = dyoff;
				}
			}
		}
		else if (offconstant > 0.0)
		{
			double dxoff = 0.5 * (apex.x - dest.x) - offconstant * (apex.y - dest.y);
			double dyoff = 0.5 * (apex.y - dest.y) + offconstant * (apex.x - dest.x);
			if (dxoff * dxoff + dyoff * dyoff < (dx - xdo) * (dx - xdo) + (dy - ydo) * (dy - ydo))
			{
				dx = xdo + dxoff;
				dy = ydo + dyoff;
			}
		}
		xi = (yao * dx - xao * dy) * (2.0 * denominator);
		eta = (xdo * dy - ydo * dx) * (2.0 * denominator);
		return new Point(org.x + dx, org.y + dy);
	}

	public Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta)
	{
		Statistic.CircumcenterCount++;
		double xdo = dest.x - org.x;
		double ydo = dest.y - org.y;
		double xao = apex.x - org.x;
		double yao = apex.y - org.y;
		double dodist = xdo * xdo + ydo * ydo;
		double aodist = xao * xao + yao * yao;
		double denominator;
		if (Behavior.NoExact)
		{
			denominator = 0.5 / (xdo * yao - xao * ydo);
		}
		else
		{
			denominator = 0.5 / CounterClockwise(dest, apex, org);
			Statistic.CounterClockwiseCount--;
		}
		double dx = (yao * dodist - ydo * aodist) * denominator;
		double dy = (xdo * aodist - xao * dodist) * denominator;
		xi = (yao * dx - xao * dy) * (2.0 * denominator);
		eta = (xdo * dy - ydo * dx) * (2.0 * denominator);
		return new Point(org.x + dx, org.y + dy);
	}

	private int FastExpansionSumZeroElim(int elen, double[] e, int flen, double[] f, double[] h)
	{
		double enow = e[0];
		double fnow = f[0];
		int findex;
		int eindex = (findex = 0);
		double Q;
		if (fnow > enow == fnow > 0.0 - enow)
		{
			Q = enow;
			enow = e[++eindex];
		}
		else
		{
			Q = fnow;
			fnow = f[++findex];
		}
		int hindex = 0;
		if (eindex < elen && findex < flen)
		{
			double Qnew;
			double hh;
			if (fnow > enow == fnow > 0.0 - enow)
			{
				Qnew = enow + Q;
				double bvirt = Qnew - enow;
				hh = Q - bvirt;
				enow = e[++eindex];
			}
			else
			{
				Qnew = fnow + Q;
				double bvirt = Qnew - fnow;
				hh = Q - bvirt;
				fnow = f[++findex];
			}
			Q = Qnew;
			if (hh != 0.0)
			{
				h[hindex++] = hh;
			}
			while (eindex < elen && findex < flen)
			{
				if (fnow > enow == fnow > 0.0 - enow)
				{
					Qnew = Q + enow;
					double bvirt = Qnew - Q;
					double avirt = Qnew - bvirt;
					double bround = enow - bvirt;
					hh = Q - avirt + bround;
					enow = e[++eindex];
				}
				else
				{
					Qnew = Q + fnow;
					double bvirt = Qnew - Q;
					double avirt = Qnew - bvirt;
					double bround = fnow - bvirt;
					hh = Q - avirt + bround;
					fnow = f[++findex];
				}
				Q = Qnew;
				if (hh != 0.0)
				{
					h[hindex++] = hh;
				}
			}
		}
		while (eindex < elen)
		{
			double Qnew = Q + enow;
			double bvirt = Qnew - Q;
			double avirt = Qnew - bvirt;
			double bround = enow - bvirt;
			double hh = Q - avirt + bround;
			enow = e[++eindex];
			Q = Qnew;
			if (hh != 0.0)
			{
				h[hindex++] = hh;
			}
		}
		while (findex < flen)
		{
			double Qnew = Q + fnow;
			double bvirt = Qnew - Q;
			double avirt = Qnew - bvirt;
			double bround = fnow - bvirt;
			double hh = Q - avirt + bround;
			fnow = f[++findex];
			Q = Qnew;
			if (hh != 0.0)
			{
				h[hindex++] = hh;
			}
		}
		if (Q != 0.0 || hindex == 0)
		{
			h[hindex++] = Q;
		}
		return hindex;
	}

	private int ScaleExpansionZeroElim(int elen, double[] e, double b, double[] h)
	{
		double num = splitter * b;
		double abig = num - b;
		double bhi = num - abig;
		double blo = b - bhi;
		double Q = e[0] * b;
		double num2 = splitter * e[0];
		abig = num2 - e[0];
		double ahi = num2 - abig;
		double alo = e[0] - ahi;
		double err3 = Q - ahi * bhi - alo * bhi - ahi * blo;
		double hh = alo * blo - err3;
		int hindex = 0;
		if (hh != 0.0)
		{
			h[hindex++] = hh;
		}
		for (int eindex = 1; eindex < elen; eindex++)
		{
			double enow = e[eindex];
			double product1 = enow * b;
			double num3 = splitter * enow;
			abig = num3 - enow;
			ahi = num3 - abig;
			alo = enow - ahi;
			err3 = product1 - ahi * bhi - alo * bhi - ahi * blo;
			double product2 = alo * blo - err3;
			double sum = Q + product2;
			double bvirt = sum - Q;
			double avirt = sum - bvirt;
			double bround = product2 - bvirt;
			hh = Q - avirt + bround;
			if (hh != 0.0)
			{
				h[hindex++] = hh;
			}
			Q = product1 + sum;
			bvirt = Q - product1;
			hh = sum - bvirt;
			if (hh != 0.0)
			{
				h[hindex++] = hh;
			}
		}
		if (Q != 0.0 || hindex == 0)
		{
			h[hindex++] = Q;
		}
		return hindex;
	}

	private double Estimate(int elen, double[] e)
	{
		double Q = e[0];
		for (int eindex = 1; eindex < elen; eindex++)
		{
			Q += e[eindex];
		}
		return Q;
	}

	private double CounterClockwiseAdapt(Point pa, Point pb, Point pc, double detsum)
	{
		double[] B = new double[5];
		double[] u = new double[5];
		double[] C1 = new double[8];
		double[] C2 = new double[12];
		double[] D = new double[16];
		double acx = pa.x - pc.x;
		double bcx = pb.x - pc.x;
		double acy = pa.y - pc.y;
		double bcy = pb.y - pc.y;
		double detleft = acx * bcy;
		double num = splitter * acx;
		double abig = num - acx;
		double ahi = num - abig;
		double alo = acx - ahi;
		double num2 = splitter * bcy;
		abig = num2 - bcy;
		double bhi = num2 - abig;
		double blo = bcy - bhi;
		double err3 = detleft - ahi * bhi - alo * bhi - ahi * blo;
		double num3 = alo * blo - err3;
		double detright = acy * bcx;
		double num4 = splitter * acy;
		abig = num4 - acy;
		ahi = num4 - abig;
		alo = acy - ahi;
		double num5 = splitter * bcx;
		abig = num5 - bcx;
		bhi = num5 - abig;
		blo = bcx - bhi;
		err3 = detright - ahi * bhi - alo * bhi - ahi * blo;
		double detrighttail = alo * blo - err3;
		double _i = num3 - detrighttail;
		double bvirt = num3 - _i;
		double avirt = _i + bvirt;
		double bround = bvirt - detrighttail;
		double around = num3 - avirt;
		B[0] = around + bround;
		double _j = detleft + _i;
		bvirt = _j - detleft;
		avirt = _j - bvirt;
		bround = _i - bvirt;
		around = detleft - avirt;
		double num6 = around + bround;
		_i = num6 - detright;
		bvirt = num6 - _i;
		avirt = _i + bvirt;
		bround = bvirt - detright;
		around = num6 - avirt;
		B[1] = around + bround;
		double B3 = _j + _i;
		bvirt = B3 - _j;
		avirt = B3 - bvirt;
		bround = _i - bvirt;
		around = _j - avirt;
		B[2] = around + bround;
		B[3] = B3;
		double det = Estimate(4, B);
		double errbound = ccwerrboundB * detsum;
		if (det >= errbound || 0.0 - det >= errbound)
		{
			return det;
		}
		bvirt = pa.x - acx;
		avirt = acx + bvirt;
		bround = bvirt - pc.x;
		around = pa.x - avirt;
		double acxtail = around + bround;
		bvirt = pb.x - bcx;
		avirt = bcx + bvirt;
		bround = bvirt - pc.x;
		around = pb.x - avirt;
		double bcxtail = around + bround;
		bvirt = pa.y - acy;
		avirt = acy + bvirt;
		bround = bvirt - pc.y;
		around = pa.y - avirt;
		double acytail = around + bround;
		bvirt = pb.y - bcy;
		avirt = bcy + bvirt;
		bround = bvirt - pc.y;
		around = pb.y - avirt;
		double bcytail = around + bround;
		if (acxtail == 0.0 && acytail == 0.0 && bcxtail == 0.0 && bcytail == 0.0)
		{
			return det;
		}
		errbound = ccwerrboundC * detsum + resulterrbound * ((det >= 0.0) ? det : (0.0 - det));
		det += acx * bcytail + bcy * acxtail - (acy * bcxtail + bcx * acytail);
		if (det >= errbound || 0.0 - det >= errbound)
		{
			return det;
		}
		double s1 = acxtail * bcy;
		double num7 = splitter * acxtail;
		abig = num7 - acxtail;
		ahi = num7 - abig;
		alo = acxtail - ahi;
		double num8 = splitter * bcy;
		abig = num8 - bcy;
		bhi = num8 - abig;
		blo = bcy - bhi;
		err3 = s1 - ahi * bhi - alo * bhi - ahi * blo;
		double num9 = alo * blo - err3;
		double t1 = acytail * bcx;
		double num10 = splitter * acytail;
		abig = num10 - acytail;
		ahi = num10 - abig;
		alo = acytail - ahi;
		double num11 = splitter * bcx;
		abig = num11 - bcx;
		bhi = num11 - abig;
		blo = bcx - bhi;
		err3 = t1 - ahi * bhi - alo * bhi - ahi * blo;
		double t2 = alo * blo - err3;
		_i = num9 - t2;
		bvirt = num9 - _i;
		avirt = _i + bvirt;
		bround = bvirt - t2;
		around = num9 - avirt;
		u[0] = around + bround;
		_j = s1 + _i;
		bvirt = _j - s1;
		avirt = _j - bvirt;
		bround = _i - bvirt;
		around = s1 - avirt;
		double num12 = around + bround;
		_i = num12 - t1;
		bvirt = num12 - _i;
		avirt = _i + bvirt;
		bround = bvirt - t1;
		around = num12 - avirt;
		u[1] = around + bround;
		double u3 = _j + _i;
		bvirt = u3 - _j;
		avirt = u3 - bvirt;
		bround = _i - bvirt;
		around = _j - avirt;
		u[2] = around + bround;
		u[3] = u3;
		int C1length = FastExpansionSumZeroElim(4, B, 4, u, C1);
		s1 = acx * bcytail;
		double num13 = splitter * acx;
		abig = num13 - acx;
		ahi = num13 - abig;
		alo = acx - ahi;
		double num14 = splitter * bcytail;
		abig = num14 - bcytail;
		bhi = num14 - abig;
		blo = bcytail - bhi;
		err3 = s1 - ahi * bhi - alo * bhi - ahi * blo;
		double num15 = alo * blo - err3;
		t1 = acy * bcxtail;
		double num16 = splitter * acy;
		abig = num16 - acy;
		ahi = num16 - abig;
		alo = acy - ahi;
		double num17 = splitter * bcxtail;
		abig = num17 - bcxtail;
		bhi = num17 - abig;
		blo = bcxtail - bhi;
		err3 = t1 - ahi * bhi - alo * bhi - ahi * blo;
		t2 = alo * blo - err3;
		_i = num15 - t2;
		bvirt = num15 - _i;
		avirt = _i + bvirt;
		bround = bvirt - t2;
		around = num15 - avirt;
		u[0] = around + bround;
		_j = s1 + _i;
		bvirt = _j - s1;
		avirt = _j - bvirt;
		bround = _i - bvirt;
		around = s1 - avirt;
		double num18 = around + bround;
		_i = num18 - t1;
		bvirt = num18 - _i;
		avirt = _i + bvirt;
		bround = bvirt - t1;
		around = num18 - avirt;
		u[1] = around + bround;
		u3 = _j + _i;
		bvirt = u3 - _j;
		avirt = u3 - bvirt;
		bround = _i - bvirt;
		around = _j - avirt;
		u[2] = around + bround;
		u[3] = u3;
		int C2length = FastExpansionSumZeroElim(C1length, C1, 4, u, C2);
		s1 = acxtail * bcytail;
		double num19 = splitter * acxtail;
		abig = num19 - acxtail;
		ahi = num19 - abig;
		alo = acxtail - ahi;
		double num20 = splitter * bcytail;
		abig = num20 - bcytail;
		bhi = num20 - abig;
		blo = bcytail - bhi;
		err3 = s1 - ahi * bhi - alo * bhi - ahi * blo;
		double num21 = alo * blo - err3;
		t1 = acytail * bcxtail;
		double num22 = splitter * acytail;
		abig = num22 - acytail;
		ahi = num22 - abig;
		alo = acytail - ahi;
		double num23 = splitter * bcxtail;
		abig = num23 - bcxtail;
		bhi = num23 - abig;
		blo = bcxtail - bhi;
		err3 = t1 - ahi * bhi - alo * bhi - ahi * blo;
		t2 = alo * blo - err3;
		_i = num21 - t2;
		bvirt = num21 - _i;
		avirt = _i + bvirt;
		bround = bvirt - t2;
		around = num21 - avirt;
		u[0] = around + bround;
		_j = s1 + _i;
		bvirt = _j - s1;
		avirt = _j - bvirt;
		bround = _i - bvirt;
		around = s1 - avirt;
		double num24 = around + bround;
		_i = num24 - t1;
		bvirt = num24 - _i;
		avirt = _i + bvirt;
		bround = bvirt - t1;
		around = num24 - avirt;
		u[1] = around + bround;
		u3 = _j + _i;
		bvirt = u3 - _j;
		avirt = u3 - bvirt;
		bround = _i - bvirt;
		around = _j - avirt;
		u[2] = around + bround;
		u[3] = u3;
		int Dlength = FastExpansionSumZeroElim(C2length, C2, 4, u, D);
		return D[Dlength - 1];
	}

	private double InCircleAdapt(Point pa, Point pb, Point pc, Point pd, double permanent)
	{
		double[] bc = new double[4];
		double[] ca = new double[4];
		double[] ab = new double[4];
		double[] aa = new double[4];
		double[] bb = new double[4];
		double[] cc = new double[4];
		double[] u = new double[5];
		double[] v = new double[5];
		double[] axtbb = new double[8];
		double[] axtcc = new double[8];
		double[] aytbb = new double[8];
		double[] aytcc = new double[8];
		double[] bxtaa = new double[8];
		double[] bxtcc = new double[8];
		double[] bytaa = new double[8];
		double[] bytcc = new double[8];
		double[] cxtaa = new double[8];
		double[] cxtbb = new double[8];
		double[] cytaa = new double[8];
		double[] cytbb = new double[8];
		double[] axtbc = new double[8];
		double[] aytbc = new double[8];
		double[] bxtca = new double[8];
		double[] bytca = new double[8];
		double[] cxtab = new double[8];
		double[] cytab = new double[8];
		int axtbclen = 0;
		int aytbclen = 0;
		int bxtcalen = 0;
		int bytcalen = 0;
		int cxtablen = 0;
		int cytablen = 0;
		double[] axtbct = new double[16];
		double[] aytbct = new double[16];
		double[] bxtcat = new double[16];
		double[] bytcat = new double[16];
		double[] cxtabt = new double[16];
		double[] cytabt = new double[16];
		double[] axtbctt = new double[8];
		double[] aytbctt = new double[8];
		double[] bxtcatt = new double[8];
		double[] bytcatt = new double[8];
		double[] cxtabtt = new double[8];
		double[] cytabtt = new double[8];
		double[] abt = new double[8];
		double[] bct = new double[8];
		double[] cat = new double[8];
		double[] abtt = new double[4];
		double[] bctt = new double[4];
		double[] catt = new double[4];
		double adx = pa.x - pd.x;
		double bdx = pb.x - pd.x;
		double cdx = pc.x - pd.x;
		double ady = pa.y - pd.y;
		double bdy = pb.y - pd.y;
		double cdy = pc.y - pd.y;
		adx = pa.x - pd.x;
		bdx = pb.x - pd.x;
		cdx = pc.x - pd.x;
		ady = pa.y - pd.y;
		bdy = pb.y - pd.y;
		cdy = pc.y - pd.y;
		double bdxcdy1 = bdx * cdy;
		double num = splitter * bdx;
		double abig = num - bdx;
		double ahi = num - abig;
		double alo = bdx - ahi;
		double num2 = splitter * cdy;
		abig = num2 - cdy;
		double bhi = num2 - abig;
		double blo = cdy - bhi;
		double err3 = bdxcdy1 - ahi * bhi - alo * bhi - ahi * blo;
		double num3 = alo * blo - err3;
		double cdxbdy1 = cdx * bdy;
		double num4 = splitter * cdx;
		abig = num4 - cdx;
		ahi = num4 - abig;
		alo = cdx - ahi;
		double num5 = splitter * bdy;
		abig = num5 - bdy;
		bhi = num5 - abig;
		blo = bdy - bhi;
		err3 = cdxbdy1 - ahi * bhi - alo * bhi - ahi * blo;
		double cdxbdy2 = alo * blo - err3;
		double _i = num3 - cdxbdy2;
		double bvirt = num3 - _i;
		double avirt = _i + bvirt;
		double bround = bvirt - cdxbdy2;
		double around = num3 - avirt;
		bc[0] = around + bround;
		double _j = bdxcdy1 + _i;
		bvirt = _j - bdxcdy1;
		avirt = _j - bvirt;
		bround = _i - bvirt;
		around = bdxcdy1 - avirt;
		double _0 = around + bround;
		_i = _0 - cdxbdy1;
		bvirt = _0 - _i;
		avirt = _i + bvirt;
		bround = bvirt - cdxbdy1;
		around = _0 - avirt;
		bc[1] = around + bround;
		double bc3 = _j + _i;
		bvirt = bc3 - _j;
		avirt = bc3 - bvirt;
		bround = _i - bvirt;
		around = _j - avirt;
		bc[2] = around + bround;
		bc[3] = bc3;
		int axbclen = ScaleExpansionZeroElim(4, bc, adx, axbc);
		int axxbclen = ScaleExpansionZeroElim(axbclen, axbc, adx, axxbc);
		int aybclen = ScaleExpansionZeroElim(4, bc, ady, aybc);
		int ayybclen = ScaleExpansionZeroElim(aybclen, aybc, ady, ayybc);
		int alen = FastExpansionSumZeroElim(axxbclen, axxbc, ayybclen, ayybc, adet);
		double cdxady1 = cdx * ady;
		double num6 = splitter * cdx;
		abig = num6 - cdx;
		ahi = num6 - abig;
		alo = cdx - ahi;
		double num7 = splitter * ady;
		abig = num7 - ady;
		bhi = num7 - abig;
		blo = ady - bhi;
		err3 = cdxady1 - ahi * bhi - alo * bhi - ahi * blo;
		double num8 = alo * blo - err3;
		double adxcdy1 = adx * cdy;
		double num9 = splitter * adx;
		abig = num9 - adx;
		ahi = num9 - abig;
		alo = adx - ahi;
		double num10 = splitter * cdy;
		abig = num10 - cdy;
		bhi = num10 - abig;
		blo = cdy - bhi;
		err3 = adxcdy1 - ahi * bhi - alo * bhi - ahi * blo;
		double adxcdy2 = alo * blo - err3;
		_i = num8 - adxcdy2;
		bvirt = num8 - _i;
		avirt = _i + bvirt;
		bround = bvirt - adxcdy2;
		around = num8 - avirt;
		ca[0] = around + bround;
		_j = cdxady1 + _i;
		bvirt = _j - cdxady1;
		avirt = _j - bvirt;
		bround = _i - bvirt;
		around = cdxady1 - avirt;
		_0 = around + bround;
		_i = _0 - adxcdy1;
		bvirt = _0 - _i;
		avirt = _i + bvirt;
		bround = bvirt - adxcdy1;
		around = _0 - avirt;
		ca[1] = around + bround;
		double ca3 = _j + _i;
		bvirt = ca3 - _j;
		avirt = ca3 - bvirt;
		bround = _i - bvirt;
		around = _j - avirt;
		ca[2] = around + bround;
		ca[3] = ca3;
		int bxcalen = ScaleExpansionZeroElim(4, ca, bdx, bxca);
		int bxxcalen = ScaleExpansionZeroElim(bxcalen, bxca, bdx, bxxca);
		int bycalen = ScaleExpansionZeroElim(4, ca, bdy, byca);
		int byycalen = ScaleExpansionZeroElim(bycalen, byca, bdy, byyca);
		int blen = FastExpansionSumZeroElim(bxxcalen, bxxca, byycalen, byyca, bdet);
		double adxbdy1 = adx * bdy;
		double num11 = splitter * adx;
		abig = num11 - adx;
		ahi = num11 - abig;
		alo = adx - ahi;
		double num12 = splitter * bdy;
		abig = num12 - bdy;
		bhi = num12 - abig;
		blo = bdy - bhi;
		err3 = adxbdy1 - ahi * bhi - alo * bhi - ahi * blo;
		double num13 = alo * blo - err3;
		double bdxady1 = bdx * ady;
		double num14 = splitter * bdx;
		abig = num14 - bdx;
		ahi = num14 - abig;
		alo = bdx - ahi;
		double num15 = splitter * ady;
		abig = num15 - ady;
		bhi = num15 - abig;
		blo = ady - bhi;
		err3 = bdxady1 - ahi * bhi - alo * bhi - ahi * blo;
		double bdxady2 = alo * blo - err3;
		_i = num13 - bdxady2;
		bvirt = num13 - _i;
		avirt = _i + bvirt;
		bround = bvirt - bdxady2;
		around = num13 - avirt;
		ab[0] = around + bround;
		_j = adxbdy1 + _i;
		bvirt = _j - adxbdy1;
		avirt = _j - bvirt;
		bround = _i - bvirt;
		around = adxbdy1 - avirt;
		_0 = around + bround;
		_i = _0 - bdxady1;
		bvirt = _0 - _i;
		avirt = _i + bvirt;
		bround = bvirt - bdxady1;
		around = _0 - avirt;
		ab[1] = around + bround;
		double ab3 = _j + _i;
		bvirt = ab3 - _j;
		avirt = ab3 - bvirt;
		bround = _i - bvirt;
		around = _j - avirt;
		ab[2] = around + bround;
		ab[3] = ab3;
		int cxablen = ScaleExpansionZeroElim(4, ab, cdx, cxab);
		int cxxablen = ScaleExpansionZeroElim(cxablen, cxab, cdx, cxxab);
		int cyablen = ScaleExpansionZeroElim(4, ab, cdy, cyab);
		int cyyablen = ScaleExpansionZeroElim(cyablen, cyab, cdy, cyyab);
		int clen = FastExpansionSumZeroElim(cxxablen, cxxab, cyyablen, cyyab, cdet);
		int ablen = FastExpansionSumZeroElim(alen, adet, blen, bdet, abdet);
		int finlength = FastExpansionSumZeroElim(ablen, abdet, clen, cdet, fin1);
		double det = Estimate(finlength, fin1);
		double errbound = iccerrboundB * permanent;
		if (det >= errbound || 0.0 - det >= errbound)
		{
			return det;
		}
		bvirt = pa.x - adx;
		avirt = adx + bvirt;
		bround = bvirt - pd.x;
		around = pa.x - avirt;
		double adxtail = around + bround;
		bvirt = pa.y - ady;
		avirt = ady + bvirt;
		bround = bvirt - pd.y;
		around = pa.y - avirt;
		double adytail = around + bround;
		bvirt = pb.x - bdx;
		avirt = bdx + bvirt;
		bround = bvirt - pd.x;
		around = pb.x - avirt;
		double bdxtail = around + bround;
		bvirt = pb.y - bdy;
		avirt = bdy + bvirt;
		bround = bvirt - pd.y;
		around = pb.y - avirt;
		double bdytail = around + bround;
		bvirt = pc.x - cdx;
		avirt = cdx + bvirt;
		bround = bvirt - pd.x;
		around = pc.x - avirt;
		double cdxtail = around + bround;
		bvirt = pc.y - cdy;
		avirt = cdy + bvirt;
		bround = bvirt - pd.y;
		around = pc.y - avirt;
		double cdytail = around + bround;
		if (adxtail == 0.0 && bdxtail == 0.0 && cdxtail == 0.0 && adytail == 0.0 && bdytail == 0.0 && cdytail == 0.0)
		{
			return det;
		}
		errbound = iccerrboundC * permanent + resulterrbound * ((det >= 0.0) ? det : (0.0 - det));
		det += (adx * adx + ady * ady) * (bdx * cdytail + cdy * bdxtail - (bdy * cdxtail + cdx * bdytail)) + 2.0 * (adx * adxtail + ady * adytail) * (bdx * cdy - bdy * cdx) + ((bdx * bdx + bdy * bdy) * (cdx * adytail + ady * cdxtail - (cdy * adxtail + adx * cdytail)) + 2.0 * (bdx * bdxtail + bdy * bdytail) * (cdx * ady - cdy * adx)) + ((cdx * cdx + cdy * cdy) * (adx * bdytail + bdy * adxtail - (ady * bdxtail + bdx * adytail)) + 2.0 * (cdx * cdxtail + cdy * cdytail) * (adx * bdy - ady * bdx));
		if (det >= errbound || 0.0 - det >= errbound)
		{
			return det;
		}
		double[] finnow = fin1;
		double[] finother = fin2;
		if (bdxtail != 0.0 || bdytail != 0.0 || cdxtail != 0.0 || cdytail != 0.0)
		{
			double adxadx1 = adx * adx;
			double num16 = splitter * adx;
			abig = num16 - adx;
			ahi = num16 - abig;
			alo = adx - ahi;
			err3 = adxadx1 - ahi * ahi - (ahi + ahi) * alo;
			double adxadx2 = alo * alo - err3;
			double adyady1 = ady * ady;
			double num17 = splitter * ady;
			abig = num17 - ady;
			ahi = num17 - abig;
			alo = ady - ahi;
			err3 = adyady1 - ahi * ahi - (ahi + ahi) * alo;
			double adyady2 = alo * alo - err3;
			_i = adxadx2 + adyady2;
			bvirt = _i - adxadx2;
			avirt = _i - bvirt;
			bround = adyady2 - bvirt;
			around = adxadx2 - avirt;
			aa[0] = around + bround;
			_j = adxadx1 + _i;
			bvirt = _j - adxadx1;
			avirt = _j - bvirt;
			bround = _i - bvirt;
			around = adxadx1 - avirt;
			_0 = around + bround;
			_i = _0 + adyady1;
			bvirt = _i - _0;
			avirt = _i - bvirt;
			bround = adyady1 - bvirt;
			around = _0 - avirt;
			aa[1] = around + bround;
			double aa3 = _j + _i;
			bvirt = aa3 - _j;
			avirt = aa3 - bvirt;
			bround = _i - bvirt;
			around = _j - avirt;
			aa[2] = around + bround;
			aa[3] = aa3;
		}
		if (cdxtail != 0.0 || cdytail != 0.0 || adxtail != 0.0 || adytail != 0.0)
		{
			double bdxbdx1 = bdx * bdx;
			double num18 = splitter * bdx;
			abig = num18 - bdx;
			ahi = num18 - abig;
			alo = bdx - ahi;
			err3 = bdxbdx1 - ahi * ahi - (ahi + ahi) * alo;
			double bdxbdx2 = alo * alo - err3;
			double bdybdy1 = bdy * bdy;
			double num19 = splitter * bdy;
			abig = num19 - bdy;
			ahi = num19 - abig;
			alo = bdy - ahi;
			err3 = bdybdy1 - ahi * ahi - (ahi + ahi) * alo;
			double bdybdy2 = alo * alo - err3;
			_i = bdxbdx2 + bdybdy2;
			bvirt = _i - bdxbdx2;
			avirt = _i - bvirt;
			bround = bdybdy2 - bvirt;
			around = bdxbdx2 - avirt;
			bb[0] = around + bround;
			_j = bdxbdx1 + _i;
			bvirt = _j - bdxbdx1;
			avirt = _j - bvirt;
			bround = _i - bvirt;
			around = bdxbdx1 - avirt;
			_0 = around + bround;
			_i = _0 + bdybdy1;
			bvirt = _i - _0;
			avirt = _i - bvirt;
			bround = bdybdy1 - bvirt;
			around = _0 - avirt;
			bb[1] = around + bround;
			double bb3 = _j + _i;
			bvirt = bb3 - _j;
			avirt = bb3 - bvirt;
			bround = _i - bvirt;
			around = _j - avirt;
			bb[2] = around + bround;
			bb[3] = bb3;
		}
		if (adxtail != 0.0 || adytail != 0.0 || bdxtail != 0.0 || bdytail != 0.0)
		{
			double cdxcdx1 = cdx * cdx;
			double num20 = splitter * cdx;
			abig = num20 - cdx;
			ahi = num20 - abig;
			alo = cdx - ahi;
			err3 = cdxcdx1 - ahi * ahi - (ahi + ahi) * alo;
			double cdxcdx2 = alo * alo - err3;
			double cdycdy1 = cdy * cdy;
			double num21 = splitter * cdy;
			abig = num21 - cdy;
			ahi = num21 - abig;
			alo = cdy - ahi;
			err3 = cdycdy1 - ahi * ahi - (ahi + ahi) * alo;
			double cdycdy2 = alo * alo - err3;
			_i = cdxcdx2 + cdycdy2;
			bvirt = _i - cdxcdx2;
			avirt = _i - bvirt;
			bround = cdycdy2 - bvirt;
			around = cdxcdx2 - avirt;
			cc[0] = around + bround;
			_j = cdxcdx1 + _i;
			bvirt = _j - cdxcdx1;
			avirt = _j - bvirt;
			bround = _i - bvirt;
			around = cdxcdx1 - avirt;
			_0 = around + bround;
			_i = _0 + cdycdy1;
			bvirt = _i - _0;
			avirt = _i - bvirt;
			bround = cdycdy1 - bvirt;
			around = _0 - avirt;
			cc[1] = around + bround;
			double cc3 = _j + _i;
			bvirt = cc3 - _j;
			avirt = cc3 - bvirt;
			bround = _i - bvirt;
			around = _j - avirt;
			cc[2] = around + bround;
			cc[3] = cc3;
		}
		if (adxtail != 0.0)
		{
			axtbclen = ScaleExpansionZeroElim(4, bc, adxtail, axtbc);
			int temp16alen = ScaleExpansionZeroElim(axtbclen, axtbc, 2.0 * adx, temp16a);
			int axtcclen = ScaleExpansionZeroElim(4, cc, adxtail, axtcc);
			int temp16blen = ScaleExpansionZeroElim(axtcclen, axtcc, bdy, temp16b);
			int axtbblen = ScaleExpansionZeroElim(4, bb, adxtail, axtbb);
			int temp16clen = ScaleExpansionZeroElim(axtbblen, axtbb, 0.0 - cdy, temp16c);
			int temp32alen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32a);
			int temp48len = FastExpansionSumZeroElim(temp16clen, temp16c, temp32alen, temp32a, temp48);
			finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
			double[] array = finnow;
			finnow = finother;
			finother = array;
		}
		if (adytail != 0.0)
		{
			aytbclen = ScaleExpansionZeroElim(4, bc, adytail, aytbc);
			int temp16alen = ScaleExpansionZeroElim(aytbclen, aytbc, 2.0 * ady, temp16a);
			int aytbblen = ScaleExpansionZeroElim(4, bb, adytail, aytbb);
			int temp16blen = ScaleExpansionZeroElim(aytbblen, aytbb, cdx, temp16b);
			int aytcclen = ScaleExpansionZeroElim(4, cc, adytail, aytcc);
			int temp16clen = ScaleExpansionZeroElim(aytcclen, aytcc, 0.0 - bdx, temp16c);
			int temp32alen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32a);
			int temp48len = FastExpansionSumZeroElim(temp16clen, temp16c, temp32alen, temp32a, temp48);
			finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
			double[] array2 = finnow;
			finnow = finother;
			finother = array2;
		}
		if (bdxtail != 0.0)
		{
			bxtcalen = ScaleExpansionZeroElim(4, ca, bdxtail, bxtca);
			int temp16alen = ScaleExpansionZeroElim(bxtcalen, bxtca, 2.0 * bdx, temp16a);
			int bxtaalen = ScaleExpansionZeroElim(4, aa, bdxtail, bxtaa);
			int temp16blen = ScaleExpansionZeroElim(bxtaalen, bxtaa, cdy, temp16b);
			int bxtcclen = ScaleExpansionZeroElim(4, cc, bdxtail, bxtcc);
			int temp16clen = ScaleExpansionZeroElim(bxtcclen, bxtcc, 0.0 - ady, temp16c);
			int temp32alen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32a);
			int temp48len = FastExpansionSumZeroElim(temp16clen, temp16c, temp32alen, temp32a, temp48);
			finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
			double[] array3 = finnow;
			finnow = finother;
			finother = array3;
		}
		if (bdytail != 0.0)
		{
			bytcalen = ScaleExpansionZeroElim(4, ca, bdytail, bytca);
			int temp16alen = ScaleExpansionZeroElim(bytcalen, bytca, 2.0 * bdy, temp16a);
			int bytcclen = ScaleExpansionZeroElim(4, cc, bdytail, bytcc);
			int temp16blen = ScaleExpansionZeroElim(bytcclen, bytcc, adx, temp16b);
			int bytaalen = ScaleExpansionZeroElim(4, aa, bdytail, bytaa);
			int temp16clen = ScaleExpansionZeroElim(bytaalen, bytaa, 0.0 - cdx, temp16c);
			int temp32alen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32a);
			int temp48len = FastExpansionSumZeroElim(temp16clen, temp16c, temp32alen, temp32a, temp48);
			finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
			double[] array4 = finnow;
			finnow = finother;
			finother = array4;
		}
		if (cdxtail != 0.0)
		{
			cxtablen = ScaleExpansionZeroElim(4, ab, cdxtail, cxtab);
			int temp16alen = ScaleExpansionZeroElim(cxtablen, cxtab, 2.0 * cdx, temp16a);
			int cxtbblen = ScaleExpansionZeroElim(4, bb, cdxtail, cxtbb);
			int temp16blen = ScaleExpansionZeroElim(cxtbblen, cxtbb, ady, temp16b);
			int cxtaalen = ScaleExpansionZeroElim(4, aa, cdxtail, cxtaa);
			int temp16clen = ScaleExpansionZeroElim(cxtaalen, cxtaa, 0.0 - bdy, temp16c);
			int temp32alen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32a);
			int temp48len = FastExpansionSumZeroElim(temp16clen, temp16c, temp32alen, temp32a, temp48);
			finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
			double[] array5 = finnow;
			finnow = finother;
			finother = array5;
		}
		if (cdytail != 0.0)
		{
			cytablen = ScaleExpansionZeroElim(4, ab, cdytail, cytab);
			int temp16alen = ScaleExpansionZeroElim(cytablen, cytab, 2.0 * cdy, temp16a);
			int cytaalen = ScaleExpansionZeroElim(4, aa, cdytail, cytaa);
			int temp16blen = ScaleExpansionZeroElim(cytaalen, cytaa, bdx, temp16b);
			int cytbblen = ScaleExpansionZeroElim(4, bb, cdytail, cytbb);
			int temp16clen = ScaleExpansionZeroElim(cytbblen, cytbb, 0.0 - adx, temp16c);
			int temp32alen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32a);
			int temp48len = FastExpansionSumZeroElim(temp16clen, temp16c, temp32alen, temp32a, temp48);
			finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
			double[] array6 = finnow;
			finnow = finother;
			finother = array6;
		}
		if (adxtail != 0.0 || adytail != 0.0)
		{
			int bctlen;
			int bcttlen;
			if (bdxtail != 0.0 || bdytail != 0.0 || cdxtail != 0.0 || cdytail != 0.0)
			{
				double ti1 = bdxtail * cdy;
				double num22 = splitter * bdxtail;
				abig = num22 - bdxtail;
				ahi = num22 - abig;
				alo = bdxtail - ahi;
				double num23 = splitter * cdy;
				abig = num23 - cdy;
				bhi = num23 - abig;
				blo = cdy - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				double ti2 = alo * blo - err3;
				double tj1 = bdx * cdytail;
				double num24 = splitter * bdx;
				abig = num24 - bdx;
				ahi = num24 - abig;
				alo = bdx - ahi;
				double num25 = splitter * cdytail;
				abig = num25 - cdytail;
				bhi = num25 - abig;
				blo = cdytail - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				double tj2 = alo * blo - err3;
				_i = ti2 + tj2;
				bvirt = _i - ti2;
				avirt = _i - bvirt;
				bround = tj2 - bvirt;
				around = ti2 - avirt;
				u[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 + tj1;
				bvirt = _i - _0;
				avirt = _i - bvirt;
				bround = tj1 - bvirt;
				around = _0 - avirt;
				u[1] = around + bround;
				double u3 = _j + _i;
				bvirt = u3 - _j;
				avirt = u3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				u[2] = around + bround;
				u[3] = u3;
				double negate = 0.0 - bdy;
				ti1 = cdxtail * negate;
				double num26 = splitter * cdxtail;
				abig = num26 - cdxtail;
				ahi = num26 - abig;
				alo = cdxtail - ahi;
				double num27 = splitter * negate;
				abig = num27 - negate;
				bhi = num27 - abig;
				blo = negate - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				ti2 = alo * blo - err3;
				negate = 0.0 - bdytail;
				tj1 = cdx * negate;
				double num28 = splitter * cdx;
				abig = num28 - cdx;
				ahi = num28 - abig;
				alo = cdx - ahi;
				double num29 = splitter * negate;
				abig = num29 - negate;
				bhi = num29 - abig;
				blo = negate - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				tj2 = alo * blo - err3;
				_i = ti2 + tj2;
				bvirt = _i - ti2;
				avirt = _i - bvirt;
				bround = tj2 - bvirt;
				around = ti2 - avirt;
				v[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 + tj1;
				bvirt = _i - _0;
				avirt = _i - bvirt;
				bround = tj1 - bvirt;
				around = _0 - avirt;
				v[1] = around + bround;
				double v3 = _j + _i;
				bvirt = v3 - _j;
				avirt = v3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				v[2] = around + bround;
				v[3] = v3;
				bctlen = FastExpansionSumZeroElim(4, u, 4, v, bct);
				ti1 = bdxtail * cdytail;
				double num30 = splitter * bdxtail;
				abig = num30 - bdxtail;
				ahi = num30 - abig;
				alo = bdxtail - ahi;
				double num31 = splitter * cdytail;
				abig = num31 - cdytail;
				bhi = num31 - abig;
				blo = cdytail - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				ti2 = alo * blo - err3;
				tj1 = cdxtail * bdytail;
				double num32 = splitter * cdxtail;
				abig = num32 - cdxtail;
				ahi = num32 - abig;
				alo = cdxtail - ahi;
				double num33 = splitter * bdytail;
				abig = num33 - bdytail;
				bhi = num33 - abig;
				blo = bdytail - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				tj2 = alo * blo - err3;
				_i = ti2 - tj2;
				bvirt = ti2 - _i;
				avirt = _i + bvirt;
				bround = bvirt - tj2;
				around = ti2 - avirt;
				bctt[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 - tj1;
				bvirt = _0 - _i;
				avirt = _i + bvirt;
				bround = bvirt - tj1;
				around = _0 - avirt;
				bctt[1] = around + bround;
				double bctt3 = _j + _i;
				bvirt = bctt3 - _j;
				avirt = bctt3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				bctt[2] = around + bround;
				bctt[3] = bctt3;
				bcttlen = 4;
			}
			else
			{
				bct[0] = 0.0;
				bctlen = 1;
				bctt[0] = 0.0;
				bcttlen = 1;
			}
			if (adxtail != 0.0)
			{
				int temp16alen = ScaleExpansionZeroElim(axtbclen, axtbc, adxtail, temp16a);
				int axtbctlen = ScaleExpansionZeroElim(bctlen, bct, adxtail, axtbct);
				int temp32alen = ScaleExpansionZeroElim(axtbctlen, axtbct, 2.0 * adx, temp32a);
				int temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
				double[] array7 = finnow;
				finnow = finother;
				finother = array7;
				if (bdytail != 0.0)
				{
					int temp8len = ScaleExpansionZeroElim(4, cc, adxtail, temp8);
					temp16alen = ScaleExpansionZeroElim(temp8len, temp8, bdytail, temp16a);
					finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
					double[] array8 = finnow;
					finnow = finother;
					finother = array8;
				}
				if (cdytail != 0.0)
				{
					int temp8len = ScaleExpansionZeroElim(4, bb, 0.0 - adxtail, temp8);
					temp16alen = ScaleExpansionZeroElim(temp8len, temp8, cdytail, temp16a);
					finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
					double[] array9 = finnow;
					finnow = finother;
					finother = array9;
				}
				temp32alen = ScaleExpansionZeroElim(axtbctlen, axtbct, adxtail, temp32a);
				int axtbcttlen = ScaleExpansionZeroElim(bcttlen, bctt, adxtail, axtbctt);
				temp16alen = ScaleExpansionZeroElim(axtbcttlen, axtbctt, 2.0 * adx, temp16a);
				int temp16blen = ScaleExpansionZeroElim(axtbcttlen, axtbctt, adxtail, temp16b);
				int temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
				int temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
				double[] array10 = finnow;
				finnow = finother;
				finother = array10;
			}
			if (adytail != 0.0)
			{
				int temp16alen = ScaleExpansionZeroElim(aytbclen, aytbc, adytail, temp16a);
				int aytbctlen = ScaleExpansionZeroElim(bctlen, bct, adytail, aytbct);
				int temp32alen = ScaleExpansionZeroElim(aytbctlen, aytbct, 2.0 * ady, temp32a);
				int temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
				double[] array11 = finnow;
				finnow = finother;
				finother = array11;
				temp32alen = ScaleExpansionZeroElim(aytbctlen, aytbct, adytail, temp32a);
				int aytbcttlen = ScaleExpansionZeroElim(bcttlen, bctt, adytail, aytbctt);
				temp16alen = ScaleExpansionZeroElim(aytbcttlen, aytbctt, 2.0 * ady, temp16a);
				int temp16blen = ScaleExpansionZeroElim(aytbcttlen, aytbctt, adytail, temp16b);
				int temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
				int temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
				double[] array12 = finnow;
				finnow = finother;
				finother = array12;
			}
		}
		if (bdxtail != 0.0 || bdytail != 0.0)
		{
			int catlen;
			int cattlen;
			if (cdxtail != 0.0 || cdytail != 0.0 || adxtail != 0.0 || adytail != 0.0)
			{
				double ti1 = cdxtail * ady;
				double num34 = splitter * cdxtail;
				abig = num34 - cdxtail;
				ahi = num34 - abig;
				alo = cdxtail - ahi;
				double num35 = splitter * ady;
				abig = num35 - ady;
				bhi = num35 - abig;
				blo = ady - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				double ti2 = alo * blo - err3;
				double tj1 = cdx * adytail;
				double num36 = splitter * cdx;
				abig = num36 - cdx;
				ahi = num36 - abig;
				alo = cdx - ahi;
				double num37 = splitter * adytail;
				abig = num37 - adytail;
				bhi = num37 - abig;
				blo = adytail - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				double tj2 = alo * blo - err3;
				_i = ti2 + tj2;
				bvirt = _i - ti2;
				avirt = _i - bvirt;
				bround = tj2 - bvirt;
				around = ti2 - avirt;
				u[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 + tj1;
				bvirt = _i - _0;
				avirt = _i - bvirt;
				bround = tj1 - bvirt;
				around = _0 - avirt;
				u[1] = around + bround;
				double u3 = _j + _i;
				bvirt = u3 - _j;
				avirt = u3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				u[2] = around + bround;
				u[3] = u3;
				double negate = 0.0 - cdy;
				ti1 = adxtail * negate;
				double num38 = splitter * adxtail;
				abig = num38 - adxtail;
				ahi = num38 - abig;
				alo = adxtail - ahi;
				double num39 = splitter * negate;
				abig = num39 - negate;
				bhi = num39 - abig;
				blo = negate - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				ti2 = alo * blo - err3;
				negate = 0.0 - cdytail;
				tj1 = adx * negate;
				double num40 = splitter * adx;
				abig = num40 - adx;
				ahi = num40 - abig;
				alo = adx - ahi;
				double num41 = splitter * negate;
				abig = num41 - negate;
				bhi = num41 - abig;
				blo = negate - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				tj2 = alo * blo - err3;
				_i = ti2 + tj2;
				bvirt = _i - ti2;
				avirt = _i - bvirt;
				bround = tj2 - bvirt;
				around = ti2 - avirt;
				v[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 + tj1;
				bvirt = _i - _0;
				avirt = _i - bvirt;
				bround = tj1 - bvirt;
				around = _0 - avirt;
				v[1] = around + bround;
				double v3 = _j + _i;
				bvirt = v3 - _j;
				avirt = v3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				v[2] = around + bround;
				v[3] = v3;
				catlen = FastExpansionSumZeroElim(4, u, 4, v, cat);
				ti1 = cdxtail * adytail;
				double num42 = splitter * cdxtail;
				abig = num42 - cdxtail;
				ahi = num42 - abig;
				alo = cdxtail - ahi;
				double num43 = splitter * adytail;
				abig = num43 - adytail;
				bhi = num43 - abig;
				blo = adytail - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				ti2 = alo * blo - err3;
				tj1 = adxtail * cdytail;
				double num44 = splitter * adxtail;
				abig = num44 - adxtail;
				ahi = num44 - abig;
				alo = adxtail - ahi;
				double num45 = splitter * cdytail;
				abig = num45 - cdytail;
				bhi = num45 - abig;
				blo = cdytail - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				tj2 = alo * blo - err3;
				_i = ti2 - tj2;
				bvirt = ti2 - _i;
				avirt = _i + bvirt;
				bround = bvirt - tj2;
				around = ti2 - avirt;
				catt[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 - tj1;
				bvirt = _0 - _i;
				avirt = _i + bvirt;
				bround = bvirt - tj1;
				around = _0 - avirt;
				catt[1] = around + bround;
				double catt3 = _j + _i;
				bvirt = catt3 - _j;
				avirt = catt3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				catt[2] = around + bround;
				catt[3] = catt3;
				cattlen = 4;
			}
			else
			{
				cat[0] = 0.0;
				catlen = 1;
				catt[0] = 0.0;
				cattlen = 1;
			}
			if (bdxtail != 0.0)
			{
				int temp16alen = ScaleExpansionZeroElim(bxtcalen, bxtca, bdxtail, temp16a);
				int bxtcatlen = ScaleExpansionZeroElim(catlen, cat, bdxtail, bxtcat);
				int temp32alen = ScaleExpansionZeroElim(bxtcatlen, bxtcat, 2.0 * bdx, temp32a);
				int temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
				double[] array13 = finnow;
				finnow = finother;
				finother = array13;
				if (cdytail != 0.0)
				{
					int temp8len = ScaleExpansionZeroElim(4, aa, bdxtail, temp8);
					temp16alen = ScaleExpansionZeroElim(temp8len, temp8, cdytail, temp16a);
					finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
					double[] array14 = finnow;
					finnow = finother;
					finother = array14;
				}
				if (adytail != 0.0)
				{
					int temp8len = ScaleExpansionZeroElim(4, cc, 0.0 - bdxtail, temp8);
					temp16alen = ScaleExpansionZeroElim(temp8len, temp8, adytail, temp16a);
					finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
					double[] array15 = finnow;
					finnow = finother;
					finother = array15;
				}
				temp32alen = ScaleExpansionZeroElim(bxtcatlen, bxtcat, bdxtail, temp32a);
				int bxtcattlen = ScaleExpansionZeroElim(cattlen, catt, bdxtail, bxtcatt);
				temp16alen = ScaleExpansionZeroElim(bxtcattlen, bxtcatt, 2.0 * bdx, temp16a);
				int temp16blen = ScaleExpansionZeroElim(bxtcattlen, bxtcatt, bdxtail, temp16b);
				int temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
				int temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
				double[] array16 = finnow;
				finnow = finother;
				finother = array16;
			}
			if (bdytail != 0.0)
			{
				int temp16alen = ScaleExpansionZeroElim(bytcalen, bytca, bdytail, temp16a);
				int bytcatlen = ScaleExpansionZeroElim(catlen, cat, bdytail, bytcat);
				int temp32alen = ScaleExpansionZeroElim(bytcatlen, bytcat, 2.0 * bdy, temp32a);
				int temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
				double[] array17 = finnow;
				finnow = finother;
				finother = array17;
				temp32alen = ScaleExpansionZeroElim(bytcatlen, bytcat, bdytail, temp32a);
				int bytcattlen = ScaleExpansionZeroElim(cattlen, catt, bdytail, bytcatt);
				temp16alen = ScaleExpansionZeroElim(bytcattlen, bytcatt, 2.0 * bdy, temp16a);
				int temp16blen = ScaleExpansionZeroElim(bytcattlen, bytcatt, bdytail, temp16b);
				int temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
				int temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
				double[] array18 = finnow;
				finnow = finother;
				finother = array18;
			}
		}
		if (cdxtail != 0.0 || cdytail != 0.0)
		{
			int abtlen;
			int abttlen;
			if (adxtail != 0.0 || adytail != 0.0 || bdxtail != 0.0 || bdytail != 0.0)
			{
				double ti1 = adxtail * bdy;
				double num46 = splitter * adxtail;
				abig = num46 - adxtail;
				ahi = num46 - abig;
				alo = adxtail - ahi;
				double num47 = splitter * bdy;
				abig = num47 - bdy;
				bhi = num47 - abig;
				blo = bdy - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				double ti2 = alo * blo - err3;
				double tj1 = adx * bdytail;
				double num48 = splitter * adx;
				abig = num48 - adx;
				ahi = num48 - abig;
				alo = adx - ahi;
				double num49 = splitter * bdytail;
				abig = num49 - bdytail;
				bhi = num49 - abig;
				blo = bdytail - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				double tj2 = alo * blo - err3;
				_i = ti2 + tj2;
				bvirt = _i - ti2;
				avirt = _i - bvirt;
				bround = tj2 - bvirt;
				around = ti2 - avirt;
				u[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 + tj1;
				bvirt = _i - _0;
				avirt = _i - bvirt;
				bround = tj1 - bvirt;
				around = _0 - avirt;
				u[1] = around + bround;
				double u3 = _j + _i;
				bvirt = u3 - _j;
				avirt = u3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				u[2] = around + bround;
				u[3] = u3;
				double negate = 0.0 - ady;
				ti1 = bdxtail * negate;
				double num50 = splitter * bdxtail;
				abig = num50 - bdxtail;
				ahi = num50 - abig;
				alo = bdxtail - ahi;
				double num51 = splitter * negate;
				abig = num51 - negate;
				bhi = num51 - abig;
				blo = negate - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				ti2 = alo * blo - err3;
				negate = 0.0 - adytail;
				tj1 = bdx * negate;
				double num52 = splitter * bdx;
				abig = num52 - bdx;
				ahi = num52 - abig;
				alo = bdx - ahi;
				double num53 = splitter * negate;
				abig = num53 - negate;
				bhi = num53 - abig;
				blo = negate - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				tj2 = alo * blo - err3;
				_i = ti2 + tj2;
				bvirt = _i - ti2;
				avirt = _i - bvirt;
				bround = tj2 - bvirt;
				around = ti2 - avirt;
				v[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 + tj1;
				bvirt = _i - _0;
				avirt = _i - bvirt;
				bround = tj1 - bvirt;
				around = _0 - avirt;
				v[1] = around + bround;
				double v3 = _j + _i;
				bvirt = v3 - _j;
				avirt = v3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				v[2] = around + bround;
				v[3] = v3;
				abtlen = FastExpansionSumZeroElim(4, u, 4, v, abt);
				ti1 = adxtail * bdytail;
				double num54 = splitter * adxtail;
				abig = num54 - adxtail;
				ahi = num54 - abig;
				alo = adxtail - ahi;
				double num55 = splitter * bdytail;
				abig = num55 - bdytail;
				bhi = num55 - abig;
				blo = bdytail - bhi;
				err3 = ti1 - ahi * bhi - alo * bhi - ahi * blo;
				ti2 = alo * blo - err3;
				tj1 = bdxtail * adytail;
				double num56 = splitter * bdxtail;
				abig = num56 - bdxtail;
				ahi = num56 - abig;
				alo = bdxtail - ahi;
				double num57 = splitter * adytail;
				abig = num57 - adytail;
				bhi = num57 - abig;
				blo = adytail - bhi;
				err3 = tj1 - ahi * bhi - alo * bhi - ahi * blo;
				tj2 = alo * blo - err3;
				_i = ti2 - tj2;
				bvirt = ti2 - _i;
				avirt = _i + bvirt;
				bround = bvirt - tj2;
				around = ti2 - avirt;
				abtt[0] = around + bround;
				_j = ti1 + _i;
				bvirt = _j - ti1;
				avirt = _j - bvirt;
				bround = _i - bvirt;
				around = ti1 - avirt;
				_0 = around + bround;
				_i = _0 - tj1;
				bvirt = _0 - _i;
				avirt = _i + bvirt;
				bround = bvirt - tj1;
				around = _0 - avirt;
				abtt[1] = around + bround;
				double abtt3 = _j + _i;
				bvirt = abtt3 - _j;
				avirt = abtt3 - bvirt;
				bround = _i - bvirt;
				around = _j - avirt;
				abtt[2] = around + bround;
				abtt[3] = abtt3;
				abttlen = 4;
			}
			else
			{
				abt[0] = 0.0;
				abtlen = 1;
				abtt[0] = 0.0;
				abttlen = 1;
			}
			if (cdxtail != 0.0)
			{
				int temp16alen = ScaleExpansionZeroElim(cxtablen, cxtab, cdxtail, temp16a);
				int cxtabtlen = ScaleExpansionZeroElim(abtlen, abt, cdxtail, cxtabt);
				int temp32alen = ScaleExpansionZeroElim(cxtabtlen, cxtabt, 2.0 * cdx, temp32a);
				int temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
				double[] array19 = finnow;
				finnow = finother;
				finother = array19;
				if (adytail != 0.0)
				{
					int temp8len = ScaleExpansionZeroElim(4, bb, cdxtail, temp8);
					temp16alen = ScaleExpansionZeroElim(temp8len, temp8, adytail, temp16a);
					finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
					double[] array20 = finnow;
					finnow = finother;
					finother = array20;
				}
				if (bdytail != 0.0)
				{
					int temp8len = ScaleExpansionZeroElim(4, aa, 0.0 - cdxtail, temp8);
					temp16alen = ScaleExpansionZeroElim(temp8len, temp8, bdytail, temp16a);
					finlength = FastExpansionSumZeroElim(finlength, finnow, temp16alen, temp16a, finother);
					double[] array21 = finnow;
					finnow = finother;
					finother = array21;
				}
				temp32alen = ScaleExpansionZeroElim(cxtabtlen, cxtabt, cdxtail, temp32a);
				int cxtabttlen = ScaleExpansionZeroElim(abttlen, abtt, cdxtail, cxtabtt);
				temp16alen = ScaleExpansionZeroElim(cxtabttlen, cxtabtt, 2.0 * cdx, temp16a);
				int temp16blen = ScaleExpansionZeroElim(cxtabttlen, cxtabtt, cdxtail, temp16b);
				int temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
				int temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
				double[] array22 = finnow;
				finnow = finother;
				finother = array22;
			}
			if (cdytail != 0.0)
			{
				int temp16alen = ScaleExpansionZeroElim(cytablen, cytab, cdytail, temp16a);
				int cytabtlen = ScaleExpansionZeroElim(abtlen, abt, cdytail, cytabt);
				int temp32alen = ScaleExpansionZeroElim(cytabtlen, cytabt, 2.0 * cdy, temp32a);
				int temp48len = FastExpansionSumZeroElim(temp16alen, temp16a, temp32alen, temp32a, temp48);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp48len, temp48, finother);
				double[] array23 = finnow;
				finnow = finother;
				finother = array23;
				temp32alen = ScaleExpansionZeroElim(cytabtlen, cytabt, cdytail, temp32a);
				int cytabttlen = ScaleExpansionZeroElim(abttlen, abtt, cdytail, cytabtt);
				temp16alen = ScaleExpansionZeroElim(cytabttlen, cytabtt, 2.0 * cdy, temp16a);
				int temp16blen = ScaleExpansionZeroElim(cytabttlen, cytabtt, cdytail, temp16b);
				int temp32blen = FastExpansionSumZeroElim(temp16alen, temp16a, temp16blen, temp16b, temp32b);
				int temp64len = FastExpansionSumZeroElim(temp32alen, temp32a, temp32blen, temp32b, temp64);
				finlength = FastExpansionSumZeroElim(finlength, finnow, temp64len, temp64, finother);
				double[] array24 = finnow;
				finnow = finother;
				finother = array24;
			}
		}
		return finnow[finlength - 1];
	}

	private void AllocateWorkspace()
	{
		fin1 = new double[1152];
		fin2 = new double[1152];
		abdet = new double[64];
		axbc = new double[8];
		axxbc = new double[16];
		aybc = new double[8];
		ayybc = new double[16];
		adet = new double[32];
		bxca = new double[8];
		bxxca = new double[16];
		byca = new double[8];
		byyca = new double[16];
		bdet = new double[32];
		cxab = new double[8];
		cxxab = new double[16];
		cyab = new double[8];
		cyyab = new double[16];
		cdet = new double[32];
		temp8 = new double[8];
		temp16a = new double[16];
		temp16b = new double[16];
		temp16c = new double[16];
		temp32a = new double[32];
		temp32b = new double[32];
		temp48 = new double[48];
		temp64 = new double[64];
	}

	private void ClearWorkspace()
	{
	}
}
