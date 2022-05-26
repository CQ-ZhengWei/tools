using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBusinessOnline.Tools
{
    public class StringCompressionUtils
	{
		/// <summary>
		/// 从原始字节数组生成已压缩的字节数组。
		/// </summary>
		/// <param name="bytesToCompress">原始字节数组。</param>
		/// <returns>返回已压缩的字节数组</returns>
		public static byte[] Compress(byte[] bytesToCompress)
		{
			MemoryStream ms = new MemoryStream();
			Stream s = new GZipOutputStream(ms);
			s.Write(bytesToCompress, 0, bytesToCompress.Length);
			s.Close();
			return ms.ToArray();
		}

		/// <summary>
		/// 从压缩字节数组生成解压的字节数组。
		/// </summary>
		/// <param name="bytesToDeCompress"></param>
		/// <returns></returns>
		public static byte[] DeCompress(byte[] bytesToDeCompress)
		{
			byte[] writeData = new byte[4096];
			Stream s2 = new GZipInputStream(new MemoryStream(bytesToDeCompress));
			MemoryStream outStream = new MemoryStream();
			while (true)
			{
				int size = s2.Read(writeData, 0, writeData.Length);
				if (size > 0)
				{
					outStream.Write(writeData, 0, size);
				}
				else
				{
					break;
				}
			}
			s2.Close();
			byte[] outArr = outStream.ToArray();
			outStream.Close();
			return outArr;
		}

		/// <summary>
		/// 压缩字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string Compression(string str)
		{
			return Compression(Encoding.UTF8.GetBytes(str));
		}

		/// <summary>
		/// 压缩字符串
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string Compression(byte[] bytes)
		{
			return Convert.ToBase64String(Compress(bytes));
		}

		/// <summary>
		/// 解压字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string DeCompression(string str)
		{
			var bytes = Convert.FromBase64String(str);
			return DeCompression(bytes);
		}

		/// <summary>
		/// 解压字符串
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string DeCompression(byte[] bytes)
		{
			return Encoding.UTF8.GetString(DeCompress(bytes));
		}
	}
}
