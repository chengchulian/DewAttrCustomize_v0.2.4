using System;
using TriangleNet.Topology;

namespace TriangleNet.Tools;

public class AdjacencyMatrix
{
	private int nnz;

	private int[] pcol;

	private int[] irow;

	public readonly int N;

	public int[] ColumnPointers => pcol;

	public int[] RowIndices => irow;

	public AdjacencyMatrix(Mesh mesh)
	{
		N = mesh.vertices.Count;
		pcol = AdjacencyCount(mesh);
		nnz = pcol[N];
		irow = AdjacencySet(mesh, pcol);
		SortIndices();
	}

	public AdjacencyMatrix(int[] pcol, int[] irow)
	{
		N = pcol.Length - 1;
		nnz = pcol[N];
		this.pcol = pcol;
		this.irow = irow;
		if (pcol[0] != 0)
		{
			throw new ArgumentException("Expected 0-based indexing.", "pcol");
		}
		if (irow.Length < nnz)
		{
			throw new ArgumentException();
		}
	}

	public int Bandwidth()
	{
		int band_lo = 0;
		int band_hi = 0;
		for (int i = 0; i < N; i++)
		{
			for (int j = pcol[i]; j < pcol[i + 1]; j++)
			{
				int col = irow[j];
				band_lo = Math.Max(band_lo, i - col);
				band_hi = Math.Max(band_hi, col - i);
			}
		}
		return band_lo + 1 + band_hi;
	}

	private int[] AdjacencyCount(Mesh mesh)
	{
		int n = N;
		int[] pcol = new int[n + 1];
		for (int i = 0; i < n; i++)
		{
			pcol[i] = 1;
		}
		foreach (Triangle triangle in mesh.triangles)
		{
			int tid = triangle.id;
			int n2 = triangle.vertices[0].id;
			int n3 = triangle.vertices[1].id;
			int n4 = triangle.vertices[2].id;
			int nid = triangle.neighbors[2].tri.id;
			if (nid < 0 || tid < nid)
			{
				pcol[n2]++;
				pcol[n3]++;
			}
			nid = triangle.neighbors[0].tri.id;
			if (nid < 0 || tid < nid)
			{
				pcol[n3]++;
				pcol[n4]++;
			}
			nid = triangle.neighbors[1].tri.id;
			if (nid < 0 || tid < nid)
			{
				pcol[n4]++;
				pcol[n2]++;
			}
		}
		for (int i2 = n; i2 > 0; i2--)
		{
			pcol[i2] = pcol[i2 - 1];
		}
		pcol[0] = 0;
		for (int j = 1; j <= n; j++)
		{
			pcol[j] = pcol[j - 1] + pcol[j];
		}
		return pcol;
	}

	private int[] AdjacencySet(Mesh mesh, int[] pcol)
	{
		int n = N;
		int[] col = new int[n];
		Array.Copy(pcol, col, n);
		int[] list = new int[pcol[n]];
		for (int i = 0; i < n; i++)
		{
			list[col[i]] = i;
			col[i]++;
		}
		foreach (Triangle triangle in mesh.triangles)
		{
			int tid = triangle.id;
			int n2 = triangle.vertices[0].id;
			int n3 = triangle.vertices[1].id;
			int n4 = triangle.vertices[2].id;
			int nid = triangle.neighbors[2].tri.id;
			if (nid < 0 || tid < nid)
			{
				list[col[n2]++] = n3;
				list[col[n3]++] = n2;
			}
			nid = triangle.neighbors[0].tri.id;
			if (nid < 0 || tid < nid)
			{
				list[col[n3]++] = n4;
				list[col[n4]++] = n3;
			}
			nid = triangle.neighbors[1].tri.id;
			if (nid < 0 || tid < nid)
			{
				list[col[n2]++] = n4;
				list[col[n4]++] = n2;
			}
		}
		return list;
	}

	public void SortIndices()
	{
		int n = N;
		int[] list = irow;
		for (int i = 0; i < n; i++)
		{
			int k1 = pcol[i];
			int k2 = pcol[i + 1];
			Array.Sort(list, k1, k2 - k1);
		}
	}
}
