using System;

namespace Epic.OnlineServices;

public abstract class Handle : IEquatable<Handle>, IFormattable
{
	public IntPtr InnerHandle { get; internal set; }

	public Handle()
	{
	}

	public Handle(IntPtr innerHandle)
	{
		InnerHandle = innerHandle;
	}

	public static bool operator ==(Handle left, Handle right)
	{
		if ((object)left == null)
		{
			if ((object)right == null)
			{
				return true;
			}
			return false;
		}
		return left.Equals(right);
	}

	public static bool operator !=(Handle left, Handle right)
	{
		return !(left == right);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as Handle);
	}

	public override int GetHashCode()
	{
		return (int)(65536 + InnerHandle.ToInt64());
	}

	public bool Equals(Handle other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		if (GetType() != other.GetType())
		{
			return false;
		}
		return InnerHandle == other.InnerHandle;
	}

	public override string ToString()
	{
		return InnerHandle.ToString();
	}

	public virtual string ToString(string format, IFormatProvider formatProvider)
	{
		if (format != null)
		{
			return InnerHandle.ToString(format);
		}
		return InnerHandle.ToString();
	}
}
