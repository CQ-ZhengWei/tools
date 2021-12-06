using EasyBusinessOnline.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public partial class AndroidButton : BaseForm
    {
        public AndroidButton()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChooseColor(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ChooseColor(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ChooseColor(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ChooseColor(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ChooseColor(5);
        }
        List<TextBox> textBoxes
        {
            get
            {
                return new List<TextBox>() { textBox1, textBox2, textBox3, textBox4, textBox5 };
            }
        }
        List<Label> labels
        {
            get
            {
                return new List<Label>() { label1, label2, label3, label4, label5 };
            }
        }
        private void ChooseColor(int index)
        {
            if (DialogResult.OK == colorDialog.ShowDialog())
            {
                Color color = colorDialog.Color;
                string strColor = ToAndroidColor(color);
                TextBox textBox = textBoxes[index - 1];
                Label label = labels[index - 1];
                textBox.Text = strColor;
                label.BackColor = color;
            }
        }
        private string ToAndroidColor(Color color)
        {
            string str = "#";
            if (color.A < 255)
            {
                str += color.A.ToString("X");
            }
            str += RGBToString(color.R);
            str += RGBToString(color.G);
            str += RGBToString(color.B);
            return str;
        }
        private string RGBToString(int value)
        {
            string str = value.ToString("X");
            if (str.Length == 1)
            {
                str = "0" + str;
            }
            return str;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string name = tb_Name.Text;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            string color1 = textBox1.Text;//正常背景
            string color2 = textBox2.Text;//禁用背景
            string color3 = textBox3.Text;//按下背景
            string color4 = textBox4.Text;//正常字体颜色
            string color5 = textBox5.Text;//禁用字体颜色
            string path = Utils.MapPath("/res/androidButton");
            string templatePath = Utils.MapPath("/res/androidButton/template");
            DirectoryInfo templateDirectory = new DirectoryInfo(templatePath);
            FileInfo[] templates = templateDirectory.GetFiles();
            string pathFile = Utils.MapPath("\\res\\androidButton\\" + name);
            DirectoryInfo fileDirectory = new DirectoryInfo(pathFile);
            if (!fileDirectory.Exists)
            {
                fileDirectory.Create();
            }

            foreach (var item in templates)
            {
                string fileName = pathFile + "\\" + item.Name.Replace("template", name);
                var fileStream = File.OpenWrite(fileName);
                string str = File.ReadAllText(item.FullName);
                str = str.Replace("disabled_font_color", color5);
                str = str.Replace("normal_font_color", color4);


                str = str.Replace("shape_bg_disabled_template", "shape_bg_disabled_" + name);
                str = str.Replace("shape_bg_normal_template", "shape_bg_normal_" + name);
                str = str.Replace("shape_btn_bg_pressed_template", "shape_btn_bg_pressed_" + name);


                str = str.Replace("disabled_bg_color", color2);


                str = str.Replace("normal_bg_color", color1);

                str = str.Replace("pressed_bg_color", color3);


                var bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(str);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            System.Diagnostics.Process.Start("explorer.exe", pathFile);
        }
    }
}
