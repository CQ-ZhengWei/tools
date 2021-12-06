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
    public partial class DrawableRename : BaseForm
    {
        public DrawableRename()
        {
            InitializeComponent();
            this.Resize += DrawableRename_Resize;
        }
        string imgType = "drawable";
        string[] types = new string[] { "hdpi", "mdpi", "xhdpi", "xxhdpi", "xxxhdpi" };
        string path = null;
        string[] files;
        List<TextBox> textBoxes = new List<TextBox>();
        private void Button1_Click(object sender, EventArgs e)
        {
            
            path =  textBox1.Text;
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("请输入路径");
                return;
            }
            foreach (var type in types)
            {
                var drawable = path + "\\"+ imgType + "-" + type;
                if (!Directory.Exists(drawable))
                {
                    MessageBox.Show("找不到" + drawable);
                    return;
                }
            }
            var lastDrawable = path + "\\"+ imgType + "-" + types[types.Length - 1];
            files = Directory.GetFiles(lastDrawable);
            var gboxWidth = panel1.Width - 15;
            var toolWidth = 200;
            var toolHeight = 280;
            var imgMaxWidth = toolWidth;
            var imgMaxHeight = toolWidth;
            var col = gboxWidth / (toolWidth + 20);
            if (col <= 0)
            {
                return;
            }
            panel1.Controls.Clear();
            textBoxes = new List<TextBox>();
            var row = files.Length / col + (files.Length % col == 0 ? 0 : 1);
            var leftMarginContainer = 10;
            var topMarginContainer = 20;
            var leftMargin = (int)(((gboxWidth - leftMarginContainer * 2) * 1.0 / col - toolWidth) / 2);
            //var topMargin = 20; 108-40=68/5
            files = files.OrderByDescending(a=>a).ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                var name = files[i];
                FileInfo file =new FileInfo(name);
                 System.Drawing.Image img = System.Drawing.Image.FromFile(name);
                 System.Drawing.Image bitmap = new System.Drawing.Bitmap(img);
                 img.Dispose();
                //Image image = Image.FromFile(name);
                //Image bitmap = (Image)image.Clone();
                //image.Clone();
                var left = i % col * (toolWidth + leftMargin * 2) + leftMarginContainer + leftMargin;
                var top = i / col * toolHeight + topMarginContainer;
                PictureBox pictureBox = new PictureBox();
                //label.TextAlign = ContentAlignment.MiddleCenter;
                pictureBox.Top = top;
                pictureBox.Left = left;
                pictureBox.BackgroundImage = bitmap;
                pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
                pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                //label.Text = tool.text;
                var height = bitmap.Height;
                var width = bitmap.Width;

                if (i == 1)
                {

                }
                if (height > imgMaxHeight|| width > imgMaxWidth)
                {
                    //高比较大 400 300
                    if(height > width)
                    {
                        height = imgMaxHeight;
                        width = (int)(bitmap.Width * 1.0 / bitmap.Height * imgMaxHeight);
                    }
                    else
                    {

                        width = imgMaxWidth;
                        height = (int)(bitmap.Height * 1.0 / bitmap.Width * imgMaxWidth);
                    }
                }
                pictureBox.Width = width;
                pictureBox.Height = height;
                pictureBox.BackColor = Color.White;
                //pictureBox.Image = bitmap;
                //pictureBox.BackgroundImageLayout = ImageLayout.Stretch;
                //pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                //label.ForeColor = Color.Blue;
                //label.BackColor = Color.White;
                //pictureBox.TabIndex = i;
                //pictureBox.Click += new EventHandler(Label_Click);

                TextBox textBox = new TextBox();
                textBox.Text = file.Name;
                textBox.Width = toolWidth;
                textBox.Top = top+imgMaxHeight;
                textBox.Left = left;
                textBox.TabIndex = i;
                textBox.TextChanged += TextBox_TextChanged;
                //textBox.LostFocus += TextBox_LostFocus;


                Button save = new Button();
                save.Text = "保存";
                save.Width = toolWidth/2;
                save.Top = top + imgMaxHeight+ textBox.Height+15;
                save.Left = left;
                save.TabIndex = i;
                save.Click += Save_Click;

                Button delete = new Button();
                delete.Text = "删除";
                delete.Width = toolWidth / 2;
                delete.Top = top + imgMaxHeight + textBox.Height + 15;
                delete.Left = left+ toolWidth / 2;
                delete.TabIndex = i;
                delete.Click += Delete_Click;

                panel1.Controls.Add(textBox);
                panel1.Controls.Add(save);
                panel1.Controls.Add(delete);
                panel1.Controls.Add(pictureBox);
                textBoxes.Add(textBox);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            var i = (sender as Button).TabIndex;
            var name = files[i];
            FileInfo file = new FileInfo(name);
            TextBox textBox = textBoxes[i];
            SetTextBox(textBox, false);
            if (File.Exists(file.DirectoryName + "\\" + textBox.Text))
            {
                textBox.Text = file.Name;
                MessageBox.Show("文件名已存在，修改失败");
                return;
            }
            //修改文件名，并重命名另外的
            foreach (var item in types)
            {
                var drawable = path + "\\"+ imgType + "-" + item + "\\" + file.Name;
                FileInfo fi = new FileInfo(drawable);
                if (textBox.Text != file.Name)
                {
                    try
                    {
                        fi.MoveTo(path + "\\"+ imgType + "-" + item + "\\" + textBox.Text);
                        //fi.Delete();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            Button1_Click(null, null);
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var i = (sender as Button).TabIndex;
            var name = files[i];
            FileInfo file = new FileInfo(name);
            //修改文件名，并重命名另外的
            foreach (var item in types)
            {
                var drawable = path + "\\"+ imgType + "-" + item + "\\" + file.Name;
                FileInfo fi = new FileInfo(drawable);
                if (!fi.Exists)
                {
                    MessageBox.Show("无法删除"+drawable);
                }
                else
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                
            }
            Button1_Click(null, null);
        }

        //private void TextBox_LostFocus(object sender, EventArgs e)
        //{
        //    SetTextBox((sender as TextBox), false);
        //    var i = (sender as TextBox).TabIndex;
        //    var name = files[i];
        //    FileInfo file =new FileInfo(name);
        //    if(File.Exists(file.DirectoryName + "\\" + (sender as TextBox).Text))
        //    {
        //        (sender as TextBox).Text = file.Name;
        //        //MessageBox.Show("文件名已存在，修改失败");
        //        return;
        //    }
        //    //修改文件名，并重命名另外的
        //    foreach (var item in types)
        //    {
        //        var drawable = path + "\\drawable-" + item+"\\"+ file.Name;
        //        FileInfo fi = new FileInfo(drawable);
        //        if((sender as TextBox).Text!=file.Name)
        //        {
        //            try
        //            {
        //                fi.MoveTo(path + "\\drawable-" + item + "\\" + (sender as TextBox).Text);
        //                //fi.Delete();
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }
        //    Button1_Click(null,null);
        //}

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var i = (sender as TextBox).TabIndex;
            var name = files[i];
            FileInfo file = new FileInfo(name);
            SetTextBox((sender as TextBox),true);
        }
        private void SetTextBox(TextBox textBox, bool isChanged)
        {
            if (isChanged)
            {
                textBox.BackColor = Color.DarkRed;
                textBox.ForeColor = Color.White;
            }
            else
            {
                textBox.BackColor = Color.White;
                textBox.ForeColor = Color.Black;
            }
        }
        private void DrawableRename_Load(object sender, EventArgs e)
        {
            panel1.Height = Height - 100;
            label2.Text = "复制文件目录到路径，点击检索文件";
        }

        private void DrawableRename_Resize(object sender, EventArgs e)
        {
            panel1.Height = Height-100;
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if((sender as CheckBox).Checked)
            {
                imgType = "mipmap";
            }
            else
            {
                imgType = "drawable";
            }
        }
        //浏览赋值
        private void Button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.textBox1.Text = path.SelectedPath;
        }
        //打开目录
        private void Button3_Click(object sender, EventArgs e)
        {
            string path = textBox1.Text;
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("目录为空，无法打开");
                return;
            }
            if (!System.IO.Directory.Exists(path))
            {
                MessageBox.Show("目录找不到，无法打开");
                return;
            }
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }
}
