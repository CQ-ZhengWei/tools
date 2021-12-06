using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public partial class RSAKey : Form
    {
        public RSAKey()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            RSATool myRSA = new RSATool();
            Dictionary<string, string> dictK = new Dictionary<string, string>();
            dictK = myRSA.GetKey();
            string strText = "123456";
            Console.WriteLine("要加密的字符串是：{0}", strText);
            string str1 = myRSA.Encrypt("123456", dictK["PublicKey"]);
            Console.WriteLine("加密后的字符串：{0}", str1);
            string str2 = myRSA.Decrypt(str1, dictK["PrivateKey"]);
            Console.WriteLine("解密后的字符串：{0}", str2);

            var isXml= checkBox1.Checked;
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择生成文件存放位置";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                string path = dialog.SelectedPath;
                var result=RSAHelper.CreateKey(ref path, isXml);
                if (result)
                {
                    //MessageBox.Show(this, "生成成功", "提示");
                    //Process p = new Process();
                    //p.StartInfo.FileName = "explorer.exe";
                    //p.StartInfo.Arguments = $" /select,{path}";
                    //p.Start();
                    Process.Start(path);
                }
                else
                {
                    MessageBox.Show(this, "生成失败", "提示");
                }
            }
            
        }
    }


    public class RSATool
    {
        public string Encrypt(string strText, string strPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(strPublicKey);
            byte[] byteText = Encoding.UTF8.GetBytes(strText);
            byte[] byteEntry = rsa.Encrypt(byteText, false);
            return Convert.ToBase64String(byteEntry);
        }
        public string Decrypt(string strEntryText, string strPrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(strPrivateKey);
            byte[] byteEntry = Convert.FromBase64String(strEntryText);
            byte[] byteText = rsa.Decrypt(byteEntry, false);
            return Encoding.UTF8.GetString(byteText);
        }
        public Dictionary<string, string> GetKey()
        {
            Dictionary<string, string> dictKey = new Dictionary<string, string>();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            dictKey.Add("PublicKey", rsa.ToXmlString(false));
            dictKey.Add("PrivateKey", rsa.ToXmlString(true));
            return dictKey;
        }
    }
}
