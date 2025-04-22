using System;
using UnityEngine;

namespace IngameDebugConsole;

public class DynamicCircularBuffer<T>
{
	private T[] array;

	private int startIndex;

	public int Count { get; private set; }

	public int Capacity => array.Length;

	public T this[int index]
	{
		get
		{
			return array[(startIndex + index) % array.Length];
		}
		set
		{
			array[(startIndex + index) % array.Length] = value;
		}
	}

	public DynamicCircularBuffer(int initialCapacity = 2)
	{
		array = new T[initialCapacity];
	}

	private void SetCapacity(int capacity)
	{
		T[] newArray = new T[capacity];
		if (Count > 0)
		{
			int elementsBeforeWrap = Mathf.Min(Count, array.Length - startIndex);
			Array.Copy(array, startIndex, newArray, 0, elementsBeforeWrap);
			if (elementsBeforeWrap < Count)
			{
				Array.Copy(array, 0, newArray, elementsBeforeWrap, Count - elementsBeforeWrap);
			}
		}
		array = newArray;
		startIndex = 0;
	}

	public void AddFirst(T value)
	{
		if (array.Length == Count)
		{
			SetCapacity(Mathf.Max(array.Length * 2, 4));
		}
		startIndex = ((startIndex > 0) ? (startIndex - 1) : (array.Length - 1));
		array[startIndex] = value;
		Count++;
	}

	public void Add(T value)
	{
		if (array.Length == Count)
		{
			SetCapacity(Mathf.Max(array.Length * 2, 4));
		}
		this[Count++] = value;
	}

	public void AddRange(DynamicCircularBuffer<T> other)
	{
		if (other.Count != 0)
		{
			if (array.Length < Count + other.Count)
			{
				SetCapacity(Mathf.Max(array.Length * 2, Count + other.Count));
			}
			int insertStartIndex = (startIndex + Count) % array.Length;
			int elementsBeforeWrap = Mathf.Min(other.Count, array.Length - insertStartIndex);
			int otherElementsBeforeWrap = Mathf.Min(other.Count, other.array.Length - other.startIndex);
			Array.Copy(other.array, other.startIndex, array, insertStartIndex, Mathf.Min(elementsBeforeWrap, otherElementsBeforeWrap));
			if (elementsBeforeWrap < otherElementsBeforeWrap)
			{
				Array.Copy(other.array, other.startIndex + elementsBeforeWrap, array, 0, otherElementsBeforeWrap - elementsBeforeWrap);
			}
			else if (elementsBeforeWrap > otherElementsBeforeWrap)
			{
				Array.Copy(other.array, 0, array, insertStartIndex + otherElementsBeforeWrap, elementsBeforeWrap - otherElementsBeforeWrap);
			}
			int copiedElements = Mathf.Max(elementsBeforeWrap, otherElementsBeforeWrap);
			if (copiedElements < other.Count)
			{
				Array.Copy(other.array, copiedElements - otherElementsBeforeWrap, array, copiedElements - elementsBeforeWrap, other.Count - copiedElements);
			}
			Count += other.Count;
		}
	}

	public T RemoveFirst()
	{
		T result = array[startIndex];
		array[startIndex] = default(T);
		if (++startIndex == array.Length)
		{
			startIndex = 0;
		}
		Count--;
		return result;
	}

	public T RemoveLast()
	{
		int index = (startIndex + Count - 1) % array.Length;
		T result = array[index];
		array[index] = default(T);
		Count--;
		return result;
	}

	public int RemoveAll(Predicate<T> shouldRemoveElement)
	{
		return RemoveAll<T>(shouldRemoveElement, null, null);
	}

	public int RemoveAll<Y>(Predicate<T> shouldRemoveElement, Action<T, int> onElementIndexChanged, DynamicCircularBuffer<Y> synchronizedBuffer)
	{
		Y[] synchronizedArray = synchronizedBuffer?.array;
		int elementsBeforeWrap = Mathf.Min(Count, array.Length - startIndex);
		int removedElements = 0;
		int i = startIndex;
		int newIndex = startIndex;
		int endIndex;
		for (endIndex = startIndex + elementsBeforeWrap; i < endIndex; i++)
		{
			if (shouldRemoveElement(array[i]))
			{
				removedElements++;
				continue;
			}
			if (removedElements > 0)
			{
				T element = array[i];
				array[newIndex] = element;
				if (synchronizedArray != null)
				{
					synchronizedArray[newIndex] = synchronizedArray[i];
				}
				onElementIndexChanged?.Invoke(element, newIndex - startIndex);
			}
			newIndex++;
		}
		i = 0;
		endIndex = Count - elementsBeforeWrap;
		if (newIndex < array.Length)
		{
			for (; i < endIndex; i++)
			{
				if (shouldRemoveElement(array[i]))
				{
					removedElements++;
					continue;
				}
				T element2 = array[i];
				array[newIndex] = element2;
				if (synchronizedArray != null)
				{
					synchronizedArray[newIndex] = synchronizedArray[i];
				}
				onElementIndexChanged?.Invoke(element2, newIndex - startIndex);
				if (++newIndex == array.Length)
				{
					i++;
					break;
				}
			}
		}
		if (newIndex == array.Length)
		{
			newIndex = 0;
			for (; i < endIndex; i++)
			{
				if (shouldRemoveElement(array[i]))
				{
					removedElements++;
					continue;
				}
				if (removedElements > 0)
				{
					T element3 = array[i];
					array[newIndex] = element3;
					if (synchronizedArray != null)
					{
						synchronizedArray[newIndex] = synchronizedArray[i];
					}
					onElementIndexChanged?.Invoke(element3, newIndex + elementsBeforeWrap);
				}
				newIndex++;
			}
		}
		TrimEnd(removedElements);
		synchronizedBuffer?.TrimEnd(removedElements);
		return removedElements;
	}

	public void TrimStart(int trimCount, Action<T> perElementCallback = null)
	{
		TrimInternal(trimCount, startIndex, perElementCallback);
		startIndex = (startIndex + trimCount) % array.Length;
	}

	public void TrimEnd(int trimCount, Action<T> perElementCallback = null)
	{
		TrimInternal(trimCount, (startIndex + Count - trimCount) % array.Length, perElementCallback);
	}

	private void TrimInternal(int trimCount, int startIndex, Action<T> perElementCallback)
	{
		int elementsBeforeWrap = Mathf.Min(trimCount, array.Length - startIndex);
		if (perElementCallback == null)
		{
			Array.Clear(array, startIndex, elementsBeforeWrap);
			if (elementsBeforeWrap < trimCount)
			{
				Array.Clear(array, 0, trimCount - elementsBeforeWrap);
			}
		}
		else
		{
			int i = startIndex;
			for (int endIndex = startIndex + elementsBeforeWrap; i < endIndex; i++)
			{
				perElementCallback(array[i]);
				array[i] = default(T);
			}
			int j = 0;
			for (int endIndex2 = trimCount - elementsBeforeWrap; j < endIndex2; j++)
			{
				perElementCallback(array[j]);
				array[j] = default(T);
			}
		}
		Count -= trimCount;
	}

	public void Clear()
	{
		int elementsBeforeWrap = Mathf.Min(Count, array.Length - startIndex);
		Array.Clear(array, startIndex, elementsBeforeWrap);
		if (elementsBeforeWrap < Count)
		{
			Array.Clear(array, 0, Count - elementsBeforeWrap);
		}
		startIndex = 0;
		Count = 0;
	}

	public int IndexOf(T value)
	{
		int elementsBeforeWrap = Mathf.Min(Count, array.Length - startIndex);
		int index = Array.IndexOf(array, value, startIndex, elementsBeforeWrap);
		if (index >= 0)
		{
			return index - startIndex;
		}
		if (elementsBeforeWrap < Count)
		{
			index = Array.IndexOf(array, value, 0, Count - elementsBeforeWrap);
			if (index >= 0)
			{
				return index + elementsBeforeWrap;
			}
		}
		return -1;
	}

	public void ForEach(Action<T> action)
	{
		int elementsBeforeWrap = Mathf.Min(Count, array.Length - startIndex);
		int i = startIndex;
		for (int endIndex = startIndex + elementsBeforeWrap; i < endIndex; i++)
		{
			action(array[i]);
		}
		int j = 0;
		for (int endIndex2 = Count - elementsBeforeWrap; j < endIndex2; j++)
		{
			action(array[j]);
		}
	}
}
