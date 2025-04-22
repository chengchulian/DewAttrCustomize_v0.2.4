using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SimpleJSON;

public abstract class JSONNode
{
	internal static StringBuilder m_EscapeBuilder = new StringBuilder();

	public virtual JSONNode this[int aIndex]
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual JSONNode this[string aKey]
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual string Value
	{
		get
		{
			return "";
		}
		set
		{
		}
	}

	public virtual int Count => 0;

	public virtual bool IsNumber => false;

	public virtual bool IsString => false;

	public virtual bool IsBoolean => false;

	public virtual bool IsNull => false;

	public virtual bool IsArray => false;

	public virtual bool IsObject => false;

	public virtual IEnumerable<JSONNode> Children
	{
		get
		{
			yield break;
		}
	}

	public IEnumerable<JSONNode> DeepChildren
	{
		get
		{
			foreach (JSONNode C in Children)
			{
				foreach (JSONNode deepChild in C.DeepChildren)
				{
					yield return deepChild;
				}
			}
		}
	}

	public abstract JSONNodeType Tag { get; }

	public virtual double AsDouble
	{
		get
		{
			double v = 0.0;
			if (double.TryParse(Value, out v))
			{
				return v;
			}
			return 0.0;
		}
		set
		{
			Value = value.ToString();
		}
	}

	public virtual int AsInt
	{
		get
		{
			return (int)AsDouble;
		}
		set
		{
			AsDouble = value;
		}
	}

	public virtual float AsFloat
	{
		get
		{
			return (float)AsDouble;
		}
		set
		{
			AsDouble = value;
		}
	}

	public virtual bool AsBool
	{
		get
		{
			bool v = false;
			if (bool.TryParse(Value, out v))
			{
				return v;
			}
			return !string.IsNullOrEmpty(Value);
		}
		set
		{
			Value = (value ? "true" : "false");
		}
	}

	public virtual JSONArray AsArray => this as JSONArray;

	public virtual JSONObject AsObject => this as JSONObject;

	public virtual void Add(string aKey, JSONNode aItem)
	{
	}

	public virtual void Add(JSONNode aItem)
	{
		Add("", aItem);
	}

	public virtual JSONNode Remove(string aKey)
	{
		return null;
	}

	public virtual JSONNode Remove(int aIndex)
	{
		return null;
	}

	public virtual JSONNode Remove(JSONNode aNode)
	{
		return aNode;
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		WriteToStringBuilder(sb, 0, 0, JSONTextMode.Compact);
		return sb.ToString();
	}

	public virtual string ToString(int aIndent)
	{
		StringBuilder sb = new StringBuilder();
		WriteToStringBuilder(sb, 0, aIndent, JSONTextMode.Indent);
		return sb.ToString();
	}

	internal abstract void WriteToStringBuilder(StringBuilder aSB, int aIndent, int aIndentInc, JSONTextMode aMode);

	public static implicit operator JSONNode(string s)
	{
		return new JSONString(s);
	}

	public static implicit operator string(JSONNode d)
	{
		if (!(d == null))
		{
			return d.Value;
		}
		return null;
	}

	public static implicit operator JSONNode(double n)
	{
		return new JSONNumber(n);
	}

	public static implicit operator double(JSONNode d)
	{
		if (!(d == null))
		{
			return d.AsDouble;
		}
		return 0.0;
	}

	public static implicit operator JSONNode(float n)
	{
		return new JSONNumber(n);
	}

	public static implicit operator float(JSONNode d)
	{
		if (!(d == null))
		{
			return d.AsFloat;
		}
		return 0f;
	}

	public static implicit operator JSONNode(int n)
	{
		return new JSONNumber(n);
	}

	public static implicit operator int(JSONNode d)
	{
		if (!(d == null))
		{
			return d.AsInt;
		}
		return 0;
	}

	public static implicit operator JSONNode(bool b)
	{
		return new JSONBool(b);
	}

	public static implicit operator bool(JSONNode d)
	{
		if (!(d == null))
		{
			return d.AsBool;
		}
		return false;
	}

	public static bool operator ==(JSONNode a, object b)
	{
		if ((object)a == b)
		{
			return true;
		}
		bool num = a is JSONNull || (object)a == null || a is JSONLazyCreator;
		bool bIsNull = b is JSONNull || b == null || b is JSONLazyCreator;
		if (num && bIsNull)
		{
			return true;
		}
		return a.Equals(b);
	}

	public static bool operator !=(JSONNode a, object b)
	{
		return !(a == b);
	}

	public override bool Equals(object obj)
	{
		return (object)this == obj;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	internal static string Escape(string aText)
	{
		m_EscapeBuilder.Length = 0;
		if (m_EscapeBuilder.Capacity < aText.Length + aText.Length / 10)
		{
			m_EscapeBuilder.Capacity = aText.Length + aText.Length / 10;
		}
		foreach (char c in aText)
		{
			switch (c)
			{
			case '\\':
				m_EscapeBuilder.Append("\\\\");
				break;
			case '"':
				m_EscapeBuilder.Append("\\\"");
				break;
			case '\n':
				m_EscapeBuilder.Append("\\n");
				break;
			case '\r':
				m_EscapeBuilder.Append("\\r");
				break;
			case '\t':
				m_EscapeBuilder.Append("\\t");
				break;
			case '\b':
				m_EscapeBuilder.Append("\\b");
				break;
			case '\f':
				m_EscapeBuilder.Append("\\f");
				break;
			default:
				m_EscapeBuilder.Append(c);
				break;
			}
		}
		string result = m_EscapeBuilder.ToString();
		m_EscapeBuilder.Length = 0;
		return result;
	}

	private static void ParseElement(JSONNode ctx, string token, string tokenName, bool quoted)
	{
		if (quoted)
		{
			ctx.Add(tokenName, token);
			return;
		}
		string tmp = token.ToLower();
		switch (tmp)
		{
		case "false":
		case "true":
			ctx.Add(tokenName, tmp == "true");
			return;
		case "null":
			ctx.Add(tokenName, null);
			return;
		case "undefined":
			ctx.Add(tokenName, null);
			return;
		}
		if (double.TryParse(token, out var val))
		{
			ctx.Add(tokenName, val);
		}
		else
		{
			ctx.Add(tokenName, token);
		}
	}

	public static JSONNode Parse(string aJSON)
	{
		Stack<JSONNode> stack = new Stack<JSONNode>();
		JSONNode ctx = null;
		int i = 0;
		StringBuilder Token = new StringBuilder();
		string TokenName = "";
		bool QuoteMode = false;
		bool TokenIsQuoted = false;
		for (; i < aJSON.Length; i++)
		{
			switch (aJSON[i])
			{
			case '{':
				if (QuoteMode)
				{
					Token.Append(aJSON[i]);
					break;
				}
				stack.Push(new JSONObject());
				if (ctx != null)
				{
					ctx.Add(TokenName, stack.Peek());
				}
				TokenName = "";
				Token.Length = 0;
				ctx = stack.Peek();
				break;
			case '[':
				if (QuoteMode)
				{
					Token.Append(aJSON[i]);
					break;
				}
				stack.Push(new JSONArray());
				if (ctx != null)
				{
					ctx.Add(TokenName, stack.Peek());
				}
				TokenName = "";
				Token.Length = 0;
				ctx = stack.Peek();
				break;
			case ']':
			case '}':
				if (QuoteMode)
				{
					Token.Append(aJSON[i]);
					break;
				}
				if (stack.Count == 0)
				{
					throw new Exception("JSON Parse: Too many closing brackets");
				}
				stack.Pop();
				if (Token.Length > 0 || TokenIsQuoted)
				{
					ParseElement(ctx, Token.ToString(), TokenName, TokenIsQuoted);
					TokenIsQuoted = false;
				}
				TokenName = "";
				Token.Length = 0;
				if (stack.Count > 0)
				{
					ctx = stack.Peek();
				}
				break;
			case ':':
				if (QuoteMode)
				{
					Token.Append(aJSON[i]);
					break;
				}
				TokenName = Token.ToString();
				Token.Length = 0;
				TokenIsQuoted = false;
				break;
			case '"':
				QuoteMode = !QuoteMode;
				TokenIsQuoted = TokenIsQuoted || QuoteMode;
				break;
			case ',':
				if (QuoteMode)
				{
					Token.Append(aJSON[i]);
					break;
				}
				if (Token.Length > 0 || TokenIsQuoted)
				{
					ParseElement(ctx, Token.ToString(), TokenName, TokenIsQuoted);
					TokenIsQuoted = false;
				}
				TokenName = "";
				Token.Length = 0;
				TokenIsQuoted = false;
				break;
			case '\t':
			case ' ':
				if (QuoteMode)
				{
					Token.Append(aJSON[i]);
				}
				break;
			case '\\':
				i++;
				if (QuoteMode)
				{
					char C = aJSON[i];
					switch (C)
					{
					case 't':
						Token.Append('\t');
						break;
					case 'r':
						Token.Append('\r');
						break;
					case 'n':
						Token.Append('\n');
						break;
					case 'b':
						Token.Append('\b');
						break;
					case 'f':
						Token.Append('\f');
						break;
					case 'u':
					{
						string s = aJSON.Substring(i + 1, 4);
						Token.Append((char)int.Parse(s, NumberStyles.AllowHexSpecifier));
						i += 4;
						break;
					}
					default:
						Token.Append(C);
						break;
					}
				}
				break;
			default:
				Token.Append(aJSON[i]);
				break;
			case '\n':
			case '\r':
				break;
			}
		}
		if (QuoteMode)
		{
			throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
		}
		return ctx;
	}

	public virtual void Serialize(BinaryWriter aWriter)
	{
	}

	public void SaveToStream(Stream aData)
	{
		BinaryWriter W = new BinaryWriter(aData);
		Serialize(W);
	}

	public void SaveToCompressedStream(Stream aData)
	{
		throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
	}

	public void SaveToCompressedFile(string aFileName)
	{
		throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
	}

	public string SaveToCompressedBase64()
	{
		throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
	}

	public void SaveToFile(string aFileName)
	{
		Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
		using FileStream F = File.OpenWrite(aFileName);
		SaveToStream(F);
	}

	public string SaveToBase64()
	{
		using MemoryStream stream = new MemoryStream();
		SaveToStream(stream);
		stream.Position = 0L;
		return Convert.ToBase64String(stream.ToArray());
	}

	public static JSONNode Deserialize(BinaryReader aReader)
	{
		JSONNodeType type = (JSONNodeType)aReader.ReadByte();
		switch (type)
		{
		case JSONNodeType.Array:
		{
			int count2 = aReader.ReadInt32();
			JSONArray tmp2 = new JSONArray();
			for (int j = 0; j < count2; j++)
			{
				tmp2.Add(Deserialize(aReader));
			}
			return tmp2;
		}
		case JSONNodeType.Object:
		{
			int count = aReader.ReadInt32();
			JSONObject tmp = new JSONObject();
			for (int i = 0; i < count; i++)
			{
				string key = aReader.ReadString();
				JSONNode val = Deserialize(aReader);
				tmp.Add(key, val);
			}
			return tmp;
		}
		case JSONNodeType.String:
			return new JSONString(aReader.ReadString());
		case JSONNodeType.Number:
			return new JSONNumber(aReader.ReadDouble());
		case JSONNodeType.Boolean:
			return new JSONBool(aReader.ReadBoolean());
		case JSONNodeType.NullValue:
			return new JSONNull();
		default:
			throw new Exception("Error deserializing JSON. Unknown tag: " + type);
		}
	}

	public static JSONNode LoadFromCompressedFile(string aFileName)
	{
		throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
	}

	public static JSONNode LoadFromCompressedStream(Stream aData)
	{
		throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
	}

	public static JSONNode LoadFromCompressedBase64(string aBase64)
	{
		throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");
	}

	public static JSONNode LoadFromStream(Stream aData)
	{
		using BinaryReader R = new BinaryReader(aData);
		return Deserialize(R);
	}

	public static JSONNode LoadFromFile(string aFileName)
	{
		using FileStream F = File.OpenRead(aFileName);
		return LoadFromStream(F);
	}

	public static JSONNode LoadFromBase64(string aBase64)
	{
		return LoadFromStream(new MemoryStream(Convert.FromBase64String(aBase64))
		{
			Position = 0L
		});
	}
}
