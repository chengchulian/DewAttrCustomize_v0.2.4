using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CI.QuickSave.Core.Settings;

public static class Cryptography
{
	public static string Encrypt(string value, SecurityMode securityMode, string password)
	{
		return securityMode switch
		{
			SecurityMode.Aes => AesEncrypt(password, value), 
			SecurityMode.Base64 => Base64Encode(value), 
			_ => value, 
		};
	}

	public static string Decrypt(string value, SecurityMode securityMode, string password)
	{
		return securityMode switch
		{
			SecurityMode.Aes => AesDecrypt(password, value), 
			SecurityMode.Base64 => Base64Decode(value), 
			_ => value, 
		};
	}

	private static string AesEncrypt(string encryptionKey, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		using Aes encryptor = Aes.Create();
		Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 118
		});
		encryptor.Key = pdb.GetBytes(32);
		encryptor.IV = pdb.GetBytes(16);
		using MemoryStream streamIn = new MemoryStream(Encoding.UTF8.GetBytes(value));
		using MemoryStream streamOut = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(streamOut, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
		{
			streamIn.CopyTo(cryptoStream);
		}
		return Convert.ToBase64String(streamOut.ToArray());
	}

	private static string AesDecrypt(string encryptionKey, string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return value;
		}
		using Aes decryptor = Aes.Create();
		Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[13]
		{
			73, 118, 97, 110, 32, 77, 101, 100, 118, 101,
			100, 101, 118
		});
		decryptor.Key = pdb.GetBytes(32);
		decryptor.IV = pdb.GetBytes(16);
		using MemoryStream streamIn = new MemoryStream(Convert.FromBase64String(value));
		using MemoryStream streamOut = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(streamIn, decryptor.CreateDecryptor(), CryptoStreamMode.Read))
		{
			cryptoStream.CopyTo(streamOut);
		}
		return Encoding.UTF8.GetString(streamOut.ToArray());
	}

	private static string Base64Encode(string value)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
	}

	private static string Base64Decode(string value)
	{
		return Encoding.UTF8.GetString(Convert.FromBase64String(value));
	}
}
