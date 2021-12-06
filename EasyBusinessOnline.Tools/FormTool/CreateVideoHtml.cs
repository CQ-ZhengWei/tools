using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public partial class CreateVideoHtml : BaseForm
    {
        public CreateVideoHtml()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(Create);
            thread.IsBackground = true;
            thread.Start();
        }
        public void Create()
        {
            string path = textBox1.Text;
            if (!File.Exists(path + "/video001.html"))
            {
                MessageBox.Show("文件不存在");
                return;
            }
            for (int i = 10001; i <= 10100; i++)
            {
                var fileName = path + "/video" + i + ".html";
                //File.Create(fileName);
                Thread.Sleep(100);
                var strs = File.ReadAllLines(path + "/video001.html", System.Text.Encoding.GetEncoding("GB2312"));
                var fileStream = File.OpenWrite(fileName);
                var str = "";
                foreach (var item in strs)
                {
                    str += item + "\r\n";
                }
                str = str.Replace("video()", "video('http://microbox.download.yiwenyingxiao.cn/video/tool/v" + i + ".mp4?v="+DateTime.Now.ToString("yyyyMMddhhmmssfff")+"')");
                var bytes = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                Log($"正在生成{i}");
            }
        }
        delegate void LogDelegate(string msg);
        public void Log(string msg)
        {
            if (this.label1.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
            {
                while (!this.label1.IsHandleCreated)
                {
                    //解决窗体关闭时出现“访问已释放句柄“的异常
                    if (this.label1.Disposing || this.label1.IsDisposed)
                        return;
                }
                LogDelegate d = new LogDelegate(Log);
                this.label1.Invoke(d, new object[] { msg });
            }
            else
            {
                this.label1.Text = msg;
            }
        }

        private void CreateVideoHtml_Load(object sender, EventArgs e)
        {

            string path = @"F:\wyy\源码\代理商户系统\一文营销2.0\YiWenYX\CQ.YW.WX.MicroBoxDownload\course";
            textBox1.Text = path;
        }
    }
}
