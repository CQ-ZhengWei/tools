using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EasyBusinessOnline.Tools
{
    public class RSAHelper
    {
        public static bool CreateKey(ref string path,bool isXml)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string public_Key;
            string private_Key;
            if (isXml)
            {
                public_Key = RSA.ToXmlString(false);
                private_Key = RSA.ToXmlString(true);
            }
            else {
                public_Key = Convert.ToBase64String(RSA.ExportCspBlob(false));
                private_Key = Convert.ToBase64String(RSA.ExportCspBlob(true));
            }
            path = $"{path}/RSA生成器/{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string []paths = new string[] { $"{path}/public.key" , $"{path}/private.key" };
            if(WriteFile(public_Key, paths[0])
                &&WriteFile(private_Key, paths[1]))
            {
                return true;
            }
            return false;
        }
        public static bool WriteFile(string data,string path)
        {
            try
            {
                var sw = File.CreateText(path);
                sw.Write(data);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
