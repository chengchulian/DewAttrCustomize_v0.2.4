using System.IO;
using System.Text;

namespace SimpleJSON;

public class JSONString : JSONNode
{
	private string m_Data;

	public override JSONNodeType Tag => JSONNodeType.String;

	public override bool IsString => true;

	public override string Value
	{
		get
		{
			return m_Data;
		}
		set
		{
			m_Data = value;
		}
	}

	public JSONString(string aData)
	{
		m_Data = aData;
	}

	public override void Serialize(BinaryWriter aWriter)
	{
		aWriter.Write((byte)3);
		aWriter.Write(m_Data);
	}

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append('"').Append(JSONNode.Escape(m_Data)).Append('"');
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			return true;
		}
		if (obj is string s)
		{
			return m_Data == s;
		}
		JSONString s2 = obj as JSONString;
		if (s2 != null)
		{
			return m_Data == s2.m_Data;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return m_Data.GetHashCode();
	}
}
