using System;
using TriangleNet.Geometry;

namespace TriangleNet.Tools;

public class VertexSorter
{
	private const int RANDOM_SEED = 57113;

	private Random rand;

	private Vertex[] points;

	private VertexSorter(Vertex[] points, int seed)
	{
		this.points = points;
		rand = new Random(seed);
	}

	public static void Sort(Vertex[] array, int seed = 57113)
	{
		new VertexSorter(array, seed).QuickSort(0, array.Length - 1);
	}

	public static void Alternate(Vertex[] array, int length, int seed = 57113)
	{
		VertexSorter qs = new VertexSorter(array, seed);
		int divider = length >> 1;
		if (length - divider >= 2)
		{
			if (divider >= 2)
			{
				qs.AlternateAxes(0, divider - 1, 1);
			}
			qs.AlternateAxes(divider, length - 1, 1);
		}
	}

	private void QuickSort(int left, int right)
	{
		int oleft = left;
		int oright = right;
		int num = right - left + 1;
		Vertex[] array = points;
		if (num < 32)
		{
			for (int i = left + 1; i <= right; i++)
			{
				Vertex a = array[i];
				int j = i - 1;
				while (j >= left && (array[j].x > a.x || (array[j].x == a.x && array[j].y > a.y)))
				{
					array[j + 1] = array[j];
					j--;
				}
				array[j + 1] = a;
			}
			return;
		}
		int pivot = rand.Next(left, right);
		double pivotx = array[pivot].x;
		double pivoty = array[pivot].y;
		left--;
		right++;
		while (left < right)
		{
			do
			{
				left++;
			}
			while (left <= right && (array[left].x < pivotx || (array[left].x == pivotx && array[left].y < pivoty)));
			do
			{
				right--;
			}
			while (left <= right && (array[right].x > pivotx || (array[right].x == pivotx && array[right].y > pivoty)));
			if (left < right)
			{
				Vertex temp = array[left];
				array[left] = array[right];
				array[right] = temp;
			}
		}
		if (left > oleft)
		{
			QuickSort(oleft, left);
		}
		if (oright > right + 1)
		{
			QuickSort(right + 1, oright);
		}
	}

	private void AlternateAxes(int left, int right, int axis)
	{
		int num = right - left + 1;
		int divider = num >> 1;
		if (num <= 3)
		{
			axis = 0;
		}
		if (axis == 0)
		{
			VertexMedianX(left, right, left + divider);
		}
		else
		{
			VertexMedianY(left, right, left + divider);
		}
		if (num - divider >= 2)
		{
			if (divider >= 2)
			{
				AlternateAxes(left, left + divider - 1, 1 - axis);
			}
			AlternateAxes(left + divider, right, 1 - axis);
		}
	}

	private void VertexMedianX(int left, int right, int median)
	{
		int num = right - left + 1;
		int oleft = left;
		int oright = right;
		Vertex[] array = points;
		if (num == 2)
		{
			if (array[left].x > array[right].x || (array[left].x == array[right].x && array[left].y > array[right].y))
			{
				Vertex temp = array[right];
				array[right] = array[left];
				array[left] = temp;
			}
			return;
		}
		int pivot = rand.Next(left, right);
		double pivot2 = array[pivot].x;
		double pivot3 = array[pivot].y;
		left--;
		right++;
		while (left < right)
		{
			do
			{
				left++;
			}
			while (left <= right && (array[left].x < pivot2 || (array[left].x == pivot2 && array[left].y < pivot3)));
			do
			{
				right--;
			}
			while (left <= right && (array[right].x > pivot2 || (array[right].x == pivot2 && array[right].y > pivot3)));
			if (left < right)
			{
				Vertex temp = array[left];
				array[left] = array[right];
				array[right] = temp;
			}
		}
		if (left > median)
		{
			VertexMedianX(oleft, left - 1, median);
		}
		if (right < median - 1)
		{
			VertexMedianX(right + 1, oright, median);
		}
	}

	private void VertexMedianY(int left, int right, int median)
	{
		int num = right - left + 1;
		int oleft = left;
		int oright = right;
		Vertex[] array = points;
		if (num == 2)
		{
			if (array[left].y > array[right].y || (array[left].y == array[right].y && array[left].x > array[right].x))
			{
				Vertex temp = array[right];
				array[right] = array[left];
				array[left] = temp;
			}
			return;
		}
		int pivot = rand.Next(left, right);
		double pivot2 = array[pivot].y;
		double pivot3 = array[pivot].x;
		left--;
		right++;
		while (left < right)
		{
			do
			{
				left++;
			}
			while (left <= right && (array[left].y < pivot2 || (array[left].y == pivot2 && array[left].x < pivot3)));
			do
			{
				right--;
			}
			while (left <= right && (array[right].y > pivot2 || (array[right].y == pivot2 && array[right].x > pivot3)));
			if (left < right)
			{
				Vertex temp = array[left];
				array[left] = array[right];
				array[right] = temp;
			}
		}
		if (left > median)
		{
			VertexMedianY(oleft, left - 1, median);
		}
		if (right < median - 1)
		{
			VertexMedianY(right + 1, oright, median);
		}
	}
}
