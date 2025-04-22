using System.Text;

namespace SimpleJSON;

internal class JSONLazyCreator : JSONNode
{
	private JSONNode m_Node;

	private string m_Key;

	public override JSONNodeType Tag => JSONNodeType.None;

	public override JSONNode this[int aIndex]
	{
		get
		{
			return new JSONLazyCreator(this);
		}
		set
		{
			JSONArray tmp = new JSONArray();
			tmp.Add(value);
			Set(tmp);
		}
	}

	public override JSONNode this[string aKey]
	{
		get
		{
			return new JSONLazyCreator(this, aKey);
		}
		set
		{
			JSONObject tmp = new JSONObject();
			tmp.Add(aKey, value);
			Set(tmp);
		}
	}

	public override int AsInt
	{
		get
		{
			JSONNumber tmp = new JSONNumber(0.0);
			Set(tmp);
			return 0;
		}
		set
		{
			JSONNumber tmp = new JSONNumber(value);
			Set(tmp);
		}
	}

	public override float AsFloat
	{
		get
		{
			JSONNumber tmp = new JSONNumber(0.0);
			Set(tmp);
			return 0f;
		}
		set
		{
			JSONNumber tmp = new JSONNumber(value);
			Set(tmp);
		}
	}

	public override double AsDouble
	{
		get
		{
			JSONNumber tmp = new JSONNumber(0.0);
			Set(tmp);
			return 0.0;
		}
		set
		{
			JSONNumber tmp = new JSONNumber(value);
			Set(tmp);
		}
	}

	public override bool AsBool
	{
		get
		{
			JSONBool tmp = new JSONBool(aData: false);
			Set(tmp);
			return false;
		}
		set
		{
			JSONBool tmp = new JSONBool(value);
			Set(tmp);
		}
	}

	public override JSONArray AsArray
	{
		get
		{
			JSONArray tmp = new JSONArray();
			Set(tmp);
			return tmp;
		}
	}

	public override JSONObject AsObject
	{
		get
		{
			JSONObject tmp = new JSONObject();
			Set(tmp);
			return tmp;
		}
	}

	public JSONLazyCreator(JSONNode aNode)
	{
		m_Node = aNode;
		m_Key = null;
	}

	public JSONLazyCreator(JSONNode aNode, string aKey)
	{
		m_Node = aNode;
		m_Key = aKey;
	}

	private void Set(JSONNode aVal)
	{
		if (m_Key == null)
		{
			m_Node.Add(aVal);
		}
		else
		{
			m_Node.Add(m_Key, aVal);
		}
		m_Node = null;
	}

	public override void Add(JSONNode aItem)
	{
		JSONArray tmp = new JSONArray();
		tmp.Add(aItem);
		Set(tmp);
	}

	public override void Add(string aKey, JSONNode aItem)
	{
		JSONObject tmp = new JSONObject();
		tmp.Add(aKey, aItem);
		Set(tmp);
	}

	public static bool operator ==(JSONLazyCreator a, object b)
	{
		if (b == null)
		{
			return true;
		}
		return (object)a == b;
	}

	public static bool operator !=(JSONLazyCreator a, object b)
	{
		return !(a == b);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return true;
		}
		return (object)this == obj;
	}

	public override int GetHashCode()
	{
		return 0;
	}

	internal override void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode)
	{
		aSB.Append("null");
	}
}
