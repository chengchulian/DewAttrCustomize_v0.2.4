using System;

namespace TriangleNet.Tools;

public class CuthillMcKee
{
	private AdjacencyMatrix matrix;

	public int[] Renumber(Mesh mesh)
	{
		mesh.Renumber(NodeNumbering.Linear);
		return Renumber(new AdjacencyMatrix(mesh));
	}

	public int[] Renumber(AdjacencyMatrix matrix)
	{
		this.matrix = matrix;
		int bandwidth1 = matrix.Bandwidth();
		int[] pcol = matrix.ColumnPointers;
		Shift(pcol, up: true);
		int[] perm = GenerateRcm();
		int[] perm_inv = PermInverse(perm);
		int bandwidth2 = PermBandwidth(perm, perm_inv);
		if (Log.Verbose)
		{
			Log.Instance.Info($"Reverse Cuthill-McKee (Bandwidth: {bandwidth1} > {bandwidth2})");
		}
		Shift(pcol, up: false);
		return perm_inv;
	}

	private int[] GenerateRcm()
	{
		int n = matrix.N;
		int[] perm = new int[n];
		int iccsze = 0;
		int level_num = 0;
		int[] level_row = new int[n + 1];
		int[] mask = new int[n];
		for (int i = 0; i < n; i++)
		{
			mask[i] = 1;
		}
		int num = 1;
		for (int i = 0; i < n; i++)
		{
			if (mask[i] != 0)
			{
				int root = i;
				FindRoot(ref root, mask, ref level_num, level_row, perm, num - 1);
				Rcm(root, mask, perm, num - 1, ref iccsze);
				num += iccsze;
				if (n < num)
				{
					return perm;
				}
			}
		}
		return perm;
	}

	private void Rcm(int root, int[] mask, int[] perm, int offset, ref int iccsze)
	{
		int[] pcol = matrix.ColumnPointers;
		int[] irow = matrix.RowIndices;
		int[] deg = new int[matrix.N];
		Degree(root, mask, deg, ref iccsze, perm, offset);
		mask[root] = 0;
		if (iccsze <= 1)
		{
			return;
		}
		int lvlend = 0;
		int lnbr = 1;
		while (lvlend < lnbr)
		{
			int num = lvlend + 1;
			lvlend = lnbr;
			for (int i = num; i <= lvlend; i++)
			{
				int node = perm[offset + i - 1];
				int jstrt = pcol[node];
				int jstop = pcol[node + 1] - 1;
				int fnbr = lnbr + 1;
				for (int j = jstrt; j <= jstop; j++)
				{
					int nbr = irow[j - 1];
					if (mask[nbr] != 0)
					{
						lnbr++;
						mask[nbr] = 0;
						perm[offset + lnbr - 1] = nbr;
					}
				}
				if (lnbr <= fnbr)
				{
					continue;
				}
				int k = fnbr;
				while (k < lnbr)
				{
					int l = k;
					k++;
					int nbr = perm[offset + k - 1];
					while (fnbr < l)
					{
						int lperm = perm[offset + l - 1];
						if (deg[lperm - 1] <= deg[nbr - 1])
						{
							break;
						}
						perm[offset + l] = lperm;
						l--;
					}
					perm[offset + l] = nbr;
				}
			}
		}
		ReverseVector(perm, offset, iccsze);
	}

	private void FindRoot(ref int root, int[] mask, ref int level_num, int[] level_row, int[] level, int offset)
	{
		int[] pcol = matrix.ColumnPointers;
		int[] irow = matrix.RowIndices;
		int level_num2 = 0;
		GetLevelSet(ref root, mask, ref level_num, level_row, level, offset);
		int iccsze = level_row[level_num] - 1;
		if (level_num == 1 || level_num == iccsze)
		{
			return;
		}
		do
		{
			int mindeg = iccsze;
			int jstrt = level_row[level_num - 1];
			root = level[offset + jstrt - 1];
			if (jstrt < iccsze)
			{
				for (int j = jstrt; j <= iccsze; j++)
				{
					int node = level[offset + j - 1];
					int ndeg = 0;
					int kstrt = pcol[node - 1];
					int kstop = pcol[node] - 1;
					for (int k = kstrt; k <= kstop; k++)
					{
						int nghbor = irow[k - 1];
						if (mask[nghbor] > 0)
						{
							ndeg++;
						}
					}
					if (ndeg < mindeg)
					{
						root = node;
						mindeg = ndeg;
					}
				}
			}
			GetLevelSet(ref root, mask, ref level_num2, level_row, level, offset);
			if (level_num2 > level_num)
			{
				level_num = level_num2;
				continue;
			}
			break;
		}
		while (iccsze > level_num);
	}

	private void GetLevelSet(ref int root, int[] mask, ref int level_num, int[] level_row, int[] level, int offset)
	{
		int[] pcol = matrix.ColumnPointers;
		int[] irow = matrix.RowIndices;
		mask[root] = 0;
		level[offset] = root;
		level_num = 0;
		int lvlend = 0;
		int iccsze = 1;
		do
		{
			int lbegin = lvlend + 1;
			lvlend = iccsze;
			level_num++;
			level_row[level_num - 1] = lbegin;
			for (int i = lbegin; i <= lvlend; i++)
			{
				int node = level[offset + i - 1];
				int jstrt = pcol[node];
				int jstop = pcol[node + 1] - 1;
				for (int j = jstrt; j <= jstop; j++)
				{
					int nbr = irow[j - 1];
					if (mask[nbr] != 0)
					{
						iccsze++;
						level[offset + iccsze - 1] = nbr;
						mask[nbr] = 0;
					}
				}
			}
		}
		while (iccsze - lvlend > 0);
		level_row[level_num] = lvlend + 1;
		for (int i = 0; i < iccsze; i++)
		{
			mask[level[offset + i]] = 1;
		}
	}

	private void Degree(int root, int[] mask, int[] deg, ref int iccsze, int[] ls, int offset)
	{
		int[] pcol = matrix.ColumnPointers;
		int[] irow = matrix.RowIndices;
		int lvsize = 1;
		ls[offset] = root;
		pcol[root] = -pcol[root];
		int lvlend = 0;
		iccsze = 1;
		while (lvsize > 0)
		{
			int num = lvlend + 1;
			lvlend = iccsze;
			for (int i = num; i <= lvlend; i++)
			{
				int node = ls[offset + i - 1];
				int num2 = -pcol[node];
				int jstop = Math.Abs(pcol[node + 1]) - 1;
				int ideg = 0;
				for (int j = num2; j <= jstop; j++)
				{
					int nbr = irow[j - 1];
					if (mask[nbr] != 0)
					{
						ideg++;
						if (0 <= pcol[nbr])
						{
							pcol[nbr] = -pcol[nbr];
							iccsze++;
							ls[offset + iccsze - 1] = nbr;
						}
					}
				}
				deg[node] = ideg;
			}
			lvsize = iccsze - lvlend;
		}
		for (int i = 0; i < iccsze; i++)
		{
			int node = ls[offset + i];
			pcol[node] = -pcol[node];
		}
	}

	private int PermBandwidth(int[] perm, int[] perm_inv)
	{
		int[] pcol = matrix.ColumnPointers;
		int[] irow = matrix.RowIndices;
		int band_lo = 0;
		int band_hi = 0;
		int n = matrix.N;
		for (int i = 0; i < n; i++)
		{
			for (int j = pcol[perm[i]]; j < pcol[perm[i] + 1]; j++)
			{
				int col = perm_inv[irow[j - 1]];
				band_lo = Math.Max(band_lo, i - col);
				band_hi = Math.Max(band_hi, col - i);
			}
		}
		return band_lo + 1 + band_hi;
	}

	private int[] PermInverse(int[] perm)
	{
		int n = matrix.N;
		int[] perm_inv = new int[n];
		for (int i = 0; i < n; i++)
		{
			perm_inv[perm[i]] = i;
		}
		return perm_inv;
	}

	private void ReverseVector(int[] a, int offset, int size)
	{
		for (int i = 0; i < size / 2; i++)
		{
			int j = a[offset + i];
			a[offset + i] = a[offset + size - 1 - i];
			a[offset + size - 1 - i] = j;
		}
	}

	private void Shift(int[] a, bool up)
	{
		int length = a.Length;
		if (up)
		{
			for (int i = 0; i < length; i++)
			{
				a[i]++;
			}
		}
		else
		{
			for (int j = 0; j < length; j++)
			{
				a[j]--;
			}
		}
	}
}
