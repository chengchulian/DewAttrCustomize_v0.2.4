using System;
using System.Collections.Generic;

namespace EpicTransport;

public struct Packet
{
	public const int headerSize = 9;

	public int id;

	public int fragment;

	public bool moreFragments;

	public byte[] data;

	public int size => 9 + data.Length;

	public byte[] ToBytes()
	{
		byte[] array = new byte[size];
		array[0] = (byte)id;
		array[1] = (byte)(id >> 8);
		array[2] = (byte)(id >> 16);
		array[3] = (byte)(id >> 24);
		array[4] = (byte)fragment;
		array[5] = (byte)(fragment >> 8);
		array[6] = (byte)(fragment >> 16);
		array[7] = (byte)(fragment >> 24);
		array[8] = (byte)(moreFragments ? 1 : 0);
		Array.Copy(data, 0, array, 9, data.Length);
		return array;
	}

	public void FromBytes(ArraySegment<byte> array)
	{
		id = BitConverter.ToInt32(array.AsSpan());
		fragment = BitConverter.ToInt32(array.AsSpan(4));
		moreFragments = ((IList<byte>)array)[8] == 1;
		data = new byte[array.Count - 9];
		ArraySegment<byte> arraySegment = array;
		arraySegment[9..].CopyTo(data, 0);
	}

	public void FromBytes(byte[] array)
	{
		FromBytes(new ArraySegment<byte>(array));
	}
}
