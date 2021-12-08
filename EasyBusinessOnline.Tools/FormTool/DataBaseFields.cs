using EasyBusinessOnline.Tools.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyBusinessOnline.Tools.FormTool
{
    public partial class DataBaseFields : BaseForm
    {
        public DataBaseFields()
        {
            InitializeComponent();
        }
        DateTime? lastTime = null;
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            
            var form = textBox2.Text;
            var pre = textBox3.Text;
            var dataFieldInfos = GetDataFields();
            var label1Result = "";
            var label3Result = "";
            var label4Result = "";
            foreach (var dataFieldInfo in dataFieldInfos)
            {
                if (!string.IsNullOrEmpty(dataFieldInfo.desc))
                {
                    string desc = $"\r\nEXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{dataFieldInfo.desc}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{dataFieldInfo.table}'";
                    label1Result += desc;
                }
                foreach (var dataField in dataFieldInfo.dataFields)
                {
                    string desc=$"\r\nEXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{dataField.desc}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{dataFieldInfo.table}', @level2type=N'COLUMN',@level2name=N'{dataField.field}'";
                    string index= $"\r\nCREATE NONCLUSTERED INDEX {dataField.table}_{dataField.field} on [{dataField.table}]([{dataField.field}]);";
                    label3Result += index;

                    label1Result += desc;
                    label4Result += $@"IF NOT EXISTS(SELECT * FROM [SYSCOLUMNS] WHERE [ID]=OBJECT_ID('{dataField.table}') AND [NAME]='{dataField.field}')
    BEGIN
ALTER TABLE {dataField.table} ADD {dataField.field} {dataField.type}
	{desc}
{index}
print '成功增加字段{dataField.field}'
    END
"; 
                }
            }
            label1.Text = label1Result;
            label3.Text = label3Result;
            label4.Text = label4Result;


            var label2Result = "";
            var index2 = 0;
           
            label2Result += $"<div class=\"form-group ng-hide\" y-control=\"{{form:'{form}',name:'{pre}_id',title:'id',type:'number'}}\"></div>";
            foreach (var dataFieldInfo in dataFieldInfos)
            {
                foreach (var item in dataFieldInfo.dataFields)
                {
                    label2Result += "\r\n";
                    if (item.type.IndexOf("varchar") != -1)
                    {
                        //字符串
                        label2Result += $"<div class=\"form-group\" y-control=\"{{form:'{form}',name:'{pre}_{item.field.ToFirstLetterLower()}',title:'{item.desc}',type:'text',required:true";
                        if (item.len != null)
                        {
                            label2Result += $",maxlength: { item.len / 2}";
                        }
                        label2Result += $"}}\"></div>";
                    }
                    else if (item.type == "bit")
                    {
                        label2Result += $"<div class=\"form-group\" y-control=\"{{form:'{form}',name:'{pre}_{item.field.ToFirstLetterLower()}',title:'{item.desc}',type:'radio',dataSource:{{ tips: '所有', data: 'yesOrNos' }}}}\"></div>";
                    }
                    else if (item.type == "datetime")
                    {
                        label2Result += $"<div class=\"form-group\" y-control=\"{{form:'{form}',name:'{pre}_{item.field.ToFirstLetterLower()}',title:'{item.desc}',type:'datetime',format:'yyyy/mm/dd hh:ii',required:true}}\"></div>";
                    }
                    else
                    {
                        label2Result += $"<div class=\"form-group\" y-control=\"{{form:'{form}',name:'{pre}_{item.field.ToFirstLetterLower()}',title:'{item.desc}',type:'number',required:true,min:0,max:0,desc:''}}\" ></div>";
                    }
                    index2++;
                    //label2Result += $"\r\nEXECUTE sp_addextendedproperty N'MS_Description', '{item.desc}', N'user', N'dbo', N'table', N'{item.table}', N'column', N'{item.field}';";
                }

                label2Result += "\r\n\r\n\r\n";
            }
            label2.Text = label2Result;
        }
        private List<DataFieldInfo> GetDataFields()
        {
            string content = textBox1.Text;
            var arr = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            string table = "";
            string tableDesc = "";
            List<DataFieldInfo> dataFieldInfos = new List<DataFieldInfo>();
            DataFieldInfo dataFieldInfo = null;
            foreach (var item in arr)
            {
                if (item.StartsWith("--"))
                {
                    tableDesc = item.Replace("--","");
                }
                else if (item.ToLower().IndexOf("create table") != -1)
                {
                    //if(!string.IsNullOrEmpty(table))
                    //{
                    //    result += $"-----------------{table}表 end-----------------";
                    //}
                    //if (dataFields != null)
                    //{
                    //    dataFieldInfos.Add(new DataFieldInfo() { dataFields=dataFields, desc=tableDesc, table=table });
                    //}
                    dataFieldInfo = new DataFieldInfo() { dataFields=new List<DataField>() };
                    dataFieldInfos.Add(dataFieldInfo);
                    table = item.Replace("create table", "").Replace("create table".ToUpper(), "").Trim();
                    table = table.Replace("[", "").Replace("]", "").Replace("(", "");
                    if (table.ToLower().IndexOf("dbo.") != -1)
                    {
                        table = table.Substring(4);
                    }
                    dataFieldInfo.desc = tableDesc;
                    dataFieldInfo.table = table;
                    //result += $"-----------------{table}表 start-----------------";
                }
                //else if (item.IndexOf("CREATE TABLE") != -1)
                //{
                //    //if(!string.IsNullOrEmpty(table))
                //    //{
                //    //    result += $"-----------------{table}表 end-----------------";
                //    //}
                //    table = item.Replace("CREATE TABLE", "").Trim();
                //    //result += $"-----------------{table}表 start-----------------";
                //}
                else if (item.IndexOf("--") != -1)
                {
                    //EXECUTE sp_addextendedproperty N'MS_Description', '状态 0显示 1禁用', N'user', N'dbo', N'table', N'SystemMessages', N'column', N'Status';
                    string str = item.Replace("\t", "");
                    var strArr = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    string desc = Regex.Replace(str, "^.*--", "");
                    string field = strArr[0];
                    if (string.IsNullOrEmpty(desc) || string.IsNullOrEmpty(field) || string.IsNullOrEmpty(table) || field.IndexOf("--") != -1)
                    {
                        continue;
                    }
                    var type = strArr[1];
                    var lenStr = type.Replace("nvarchar","").Replace("varchar", "").Replace("(","").Replace(")", "");
                    int? len = null;
                    try
                    {
                        len = Convert.ToInt32(lenStr);
                    }
                    catch (Exception)
                    {
                    }
                    if(field.StartsWith("[")&& field.EndsWith("]"))
                    {
                        field = field.Substring(1, field.Length-2);
                    }
                    DataField dataField = new DataField() {
                         desc=desc.Trim(), field= field, table=table,len=len,type=type, old= type
                    };
                    dataFieldInfo.dataFields.Add(dataField);
                }
            }
            return dataFieldInfos;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label1.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label2.Text);
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox1_TextChanged(null, null);
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
            TextBox1_TextChanged(null,null);
        }

        private void DataBaseFields_Load(object sender, EventArgs e)
        {
            AllSelect(textBox1);
            AllSelect(textBox2);
            AllSelect(textBox3);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label3.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label3.Text+"\r\n\r\n"+ label1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label4.Text);
        }
    }


}
