using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using System;
using System.Data;
using System.Data.Odbc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AESEncryption {

	
	/// <summary>
	/// 读文件
	/// </summary>
	/// <param name="file_path"></param>
	/// <returns></returns>
	public string FileReader(string file_path)
	{
		string jsonReadText = "";
		jsonReadText += File.ReadAllText(file_path);
		return jsonReadText;
	}

	/// <summary>
	/// 将信息加密后写入指定文件
	/// </summary>
	/// <param name="file_path"></param>
	/// <param name="wirte_data"></param>
	public void FileWriter(string file_path, string wirte_data)
	{
		FileStream fileStreamData = new FileStream(file_path, FileMode.Create, FileAccess.Write);
		StreamWriter strWriter = new StreamWriter(fileStreamData);

		string encryptedWrite_data = EncryptRSA(wirte_data, Convert.FromBase64String("efGufsjRwSuPFaE2T9ZP6+D3yucxCJ6npWp4Y1RT+t0="), Convert.FromBase64String("rOaXVv0zSojJqft3nLuX1Q=="));

		strWriter.Write(encryptedWrite_data);
		strWriter.Close();
	}

	/// <summary>
	/// 加密文件还原
	/// </summary>
	/// <param name="file_path"></param>
	public void FileRestore(string file_path)
	{
		string fileContent = FileReader(file_path);

		File.Delete(file_path);

		string originFileContent = DecryptRSA(fileContent, Convert.FromBase64String("efGufsjRwSuPFaE2T9ZP6+D3yucxCJ6npWp4Y1RT+t0="), Convert.FromBase64String("rOaXVv0zSojJqft3nLuX1Q=="));

		FileStream fileStreamData = new FileStream(file_path, FileMode.Create, FileAccess.Write);
		StreamWriter strWriter = new StreamWriter(fileStreamData);
		strWriter.Write(originFileContent);
		strWriter.Close();
	}

	/// <summary>
	/// 读取加密文件的信息
	/// </summary>
	/// <param name="file_path"></param>
	/// <param name="sheet_name"></param>
	/// <returns></returns>
	public DataTable FileContentToDataTable(string file_path, string sheet_name)
	{
		string jsonText = @"";
		string tmpContent = File.ReadAllText(file_path);
		string tmpContent2 = DecryptRSA(tmpContent, Convert.FromBase64String("efGufsjRwSuPFaE2T9ZP6+D3yucxCJ6npWp4Y1RT+t0="), Convert.FromBase64String("rOaXVv0zSojJqft3nLuX1Q=="));
		jsonText += tmpContent2;

		DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(jsonText);
		DataTable jsonData = dataSet.Tables[sheet_name];

		return jsonData;
	}

	/// <summary>
	/// 加密
	/// </summary>
	/// <param name="original_data"></param>
	/// <param name="key"></param>
	/// <param name="iv"></param>
	/// <returns></returns>
	public string EncryptRSA(string original_data, byte[] key, byte[] iv)
	{
		// Check arguments.
		if (original_data == null || original_data.Length <= 0)
			throw new ArgumentNullException(original_data + "，字符串为空！");
		if (key == null || key.Length <= 0)
			throw new ArgumentNullException("Key为空");
		if (iv == null || iv.Length <= 0)
			throw new ArgumentNullException("IV为空");
		byte[] encrypted;

		using (AesManaged aesAlg = new AesManaged())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;

			// Create a decrytor to perform the stream transform.
			ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			// Create the streams used for encryption.
			using (MemoryStream msEncrypt = new MemoryStream())
			{
				using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
					{
						//Write all data to the stream.
						swEncrypt.Write(original_data);
						swEncrypt.Close();
					}
					encrypted = msEncrypt.ToArray();
				}
			}
		}

		return Convert.ToBase64String(encrypted);
	}

	/// <summary>
	/// 解密
	/// </summary>
	/// <param name="encrypt_data"></param>
	/// <param name="key"></param>
	/// <param name="iv"></param>
	public string DecryptRSA(string encrypt_data, byte[] key, byte[] iv)
	{
		byte[] cipherText = Convert.FromBase64String(encrypt_data);
		if (cipherText == null || cipherText.Length <= 0)
			throw new ArgumentNullException(encrypt_data + "，字符串为空！");
		if (key == null || key.Length <= 0)
			throw new ArgumentNullException("Key");
		if (iv == null || iv.Length <= 0)
			throw new ArgumentNullException("IV");
		string originalStr = "";

		using (AesManaged aesAlg = new AesManaged())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;

			// Create a decrytor to perform the stream transform.
			ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

			// Create the streams used for decryption.
			using (MemoryStream msDecrypt = new MemoryStream(cipherText))
			{
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
					{
						// Read the decrypted bytes from the decrypting stream
						// and place them in a string.
						originalStr = srDecrypt.ReadToEnd();
						srDecrypt.Close();
					}
				}
			}
		}

		return originalStr;
	}
}
