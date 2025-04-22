using System;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Topology;

namespace TriangleNet;

public class TrianglePool : ICollection<Triangle>, IEnumerable<Triangle>, IEnumerable
{
	private class Enumerator : IEnumerator<Triangle>, IEnumerator, IDisposable
	{
		private int count;

		private Triangle[][] pool;

		private Triangle current;

		private int index;

		private int offset;

		public Triangle Current => current;

		object IEnumerator.Current => current;

		public Enumerator(TrianglePool pool)
		{
			count = pool.Count;
			this.pool = pool.pool;
			index = 0;
			offset = 0;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			while (index < count)
			{
				current = pool[offset / 1024][offset % 1024];
				offset++;
				if (current.hash >= 0)
				{
					index++;
					return true;
				}
			}
			return false;
		}

		public void Reset()
		{
			index = (offset = 0);
		}
	}

	private const int BLOCKSIZE = 1024;

	private int size;

	private int count;

	private Triangle[][] pool;

	private Stack<Triangle> stack;

	public int Count => count - stack.Count;

	public bool IsReadOnly => true;

	public TrianglePool()
	{
		size = 0;
		int n = Math.Max(1, 64);
		pool = new Triangle[n][];
		pool[0] = new Triangle[1024];
		stack = new Stack<Triangle>(1024);
	}

	public Triangle Get()
	{
		Triangle triangle;
		if (stack.Count > 0)
		{
			triangle = stack.Pop();
			triangle.hash = -triangle.hash - 1;
			Cleanup(triangle);
		}
		else if (count < size)
		{
			triangle = pool[count / 1024][count % 1024];
			triangle.id = triangle.hash;
			Cleanup(triangle);
			count++;
		}
		else
		{
			triangle = new Triangle();
			triangle.hash = size;
			triangle.id = triangle.hash;
			int block = size / 1024;
			if (pool[block] == null)
			{
				pool[block] = new Triangle[1024];
				if (block + 1 == pool.Length)
				{
					Array.Resize(ref pool, 2 * pool.Length);
				}
			}
			pool[block][size % 1024] = triangle;
			count = ++size;
		}
		return triangle;
	}

	public void Release(Triangle triangle)
	{
		stack.Push(triangle);
		triangle.hash = -triangle.hash - 1;
	}

	public TrianglePool Restart()
	{
		foreach (Triangle item in stack)
		{
			item.hash = -item.hash - 1;
		}
		stack.Clear();
		count = 0;
		return this;
	}

	internal IEnumerable<Triangle> Sample(int k, Random random)
	{
		int count = Count;
		if (k > count)
		{
			k = count;
		}
		while (k > 0)
		{
			int i = random.Next(0, count);
			Triangle t = pool[i / 1024][i % 1024];
			if (t.hash >= 0)
			{
				k--;
				yield return t;
			}
		}
	}

	private void Cleanup(Triangle triangle)
	{
		triangle.label = 0;
		triangle.area = 0.0;
		triangle.infected = false;
		for (int i = 0; i < 3; i++)
		{
			triangle.vertices[i] = null;
			triangle.subsegs[i] = default(Osub);
			triangle.neighbors[i] = default(Otri);
		}
	}

	public void Add(Triangle item)
	{
		throw new NotImplementedException();
	}

	public void Clear()
	{
		stack.Clear();
		int blocks = size / 1024 + 1;
		for (int i = 0; i < blocks; i++)
		{
			Triangle[] block = pool[i];
			int length = (size - i * 1024) % 1024;
			for (int j = 0; j < length; j++)
			{
				block[j] = null;
			}
		}
		size = (count = 0);
	}

	public bool Contains(Triangle item)
	{
		int i = item.hash;
		if (i < 0 || i > size)
		{
			return false;
		}
		return pool[i / 1024][i % 1024].hash >= 0;
	}

	public void CopyTo(Triangle[] array, int index)
	{
		IEnumerator<Triangle> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			array[index] = enumerator.Current;
			index++;
		}
	}

	public bool Remove(Triangle item)
	{
		throw new NotImplementedException();
	}

	public IEnumerator<Triangle> GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
