namespace IngameDebugConsole;

public class CircularBuffer<T>
{
	private readonly T[] array;

	private int startIndex;

	public int Count { get; private set; }

	public T this[int index] => array[(startIndex + index) % array.Length];

	public CircularBuffer(int capacity)
	{
		array = new T[capacity];
	}

	public void Add(T value)
	{
		if (Count < array.Length)
		{
			array[Count++] = value;
			return;
		}
		array[startIndex] = value;
		if (++startIndex >= array.Length)
		{
			startIndex = 0;
		}
	}
}
