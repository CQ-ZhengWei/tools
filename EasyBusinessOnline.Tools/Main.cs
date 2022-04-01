using EasyBusinessOnline.Tools.FormTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools
{
    public partial class Main : Form
    {
        public Main()
        {
            //this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Icon = Properties.Resources.favicon;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Init();
        }

        List<ToolModel> tools = ToolModel.GetToolModels();
        private void Init()
        {

            if (DateTime.Now > Convert.ToDateTime("2022-12-01"))
            {
                Application.Exit();
            }
            var gboxWidth = panel_all.Width - 15;
            var toolWidth = 150;
            var toolHeight = 60;
            var col = gboxWidth / (toolWidth + 20);
            if (col <= 0)
            {
                return;
            }
            var row = tools.Count / col + (tools.Count % col == 0 ? 0 : 1);
            var leftMarginContainer = 10;
            var topMarginContainer = 20;
            var leftMargin = (int)(((gboxWidth - leftMarginContainer * 2) * 1.0 / col - toolWidth) / 2);
            //var topMargin = 20; 108-40=68/5
            for (int i = 0; i < tools.Count; i++)
            {
                var left = i % col * (toolWidth + leftMargin * 2) + leftMarginContainer + leftMargin;
                var top = i / col * toolHeight + topMarginContainer;
                var tool = tools[i];
                Label label = new Label();
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Top = top;
                label.Left = left;
                label.Text = tool.text;
                label.Width = toolWidth;
                label.Height = toolHeight - 20;
                label.ForeColor = Color.Blue;
                label.BackColor = Color.White;
                label.TabIndex = i;
                label.Click += new EventHandler(Label_Click);
                panel_all.Controls.Add(label);
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            var i = (sender as Label).TabIndex;
            var tool = tools[i];
            var a = tool.form.WindowState;
            //tool.form.MdiParent = this;
            tool.form.Text = tool.text;
            //tool.form.Width = this.Width - 50;
            //tool.form.Height = this.Height - 50;
            tool.form.StartPosition = FormStartPosition.CenterScreen;
            tool.form.Show();
            tool.form.TopMost = true;
            tool.form.TopMost = false;
        }
    }
    public class ToolEventArgs : EventArgs
    {

    }
    class ToolModel
    {
        public string text { get; set; }
        public Form form { get; set; }
        public static List<ToolModel> GetToolModels()
        {
            var tools = new List<ToolModel>();
            tools.Add(new ToolModel()
            {
                text = "drawable重命名",
                form = new DrawableRename()
            });
            tools.Add(new ToolModel()
            {
                text = "数据库字段说明",
                form = new DataBaseFields()
            });

            tools.Add(new ToolModel()
            {
                text = "数据库增加字段",
                form = new AddDatabaseField()
            });
            tools.Add(new ToolModel()
            {
                text = "驼峰命名转换工具",
                form = new Hump()
            });
            tools.Add(new ToolModel()
            {
                text = "RSA加密",
                form = new RSAKey()
            });
            tools.Add(new ToolModel()
            {
                text = "CreateVideoHtml",
                form = new CreateVideoHtml()
            });

            tools.Add(new ToolModel()
            {
                text = "HTML转JavaScript",
                form = new HtmlToJs()
            });
            //for (int i = 2; i < 100; i++)
            //{

            //    tools.Add(new ToolModel()
            //    {
            //        text = i+"drawable重命名",
            //    });
            //}
            tools.Add(new ToolModel()
            {
                text = "AndroidButton",
                form = new AndroidButton()
            });
            tools.Add(new ToolModel()
            {
                text = "IconfontPath",
                form = new IconfontPath()
            });
            tools.Add(new ToolModel()
            {
                text = "FileSearch",
                form = new FileSearch()
            });
            tools.Add(new ToolModel()
            {
                text = "Lines",
                form = new Lines()
            });
            tools.Add(new ToolModel()
            {
                text = "Json转换C#类",
                form = new JsonToCsharp()
            });

            tools.Add(new ToolModel()
            {
                text = "C#类转Js",
                form = new CsharpToJsTable()
            });

            tools.Add(new ToolModel()
            {
                text = "SetupFiles",
                form = new SetupFiles()
            });
            return tools;
        }
    }
}
