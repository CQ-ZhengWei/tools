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
    public partial class CsharpToJsTable : Form
    {
        public CsharpToJsTable()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.Concrete.Model.Group.Models.OutModel.Report
{
    /// <summary>
    /// 生产发货追踪报表
    /// </summary>
    public class ProdPreformanceReport
    {
        /// <summary>
        /// 签收方量
        /// </summary>
        public float VerifyStere { get; set; }

        /// <summary>
        /// 车数
        /// </summary>
        public float CarNumber { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 计划方量
        /// </summary>
        public float MarketIndentStere { get; set; }

        /// <summary>
        /// 追加方量
        /// </summary>
        public float MarketIndentAddStere { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string GeneralStationName { get; set; }

        /// <summary>
        /// 任务单号
        /// </summary>
        public string MarketIndentCode { get; set; }

        /// <summary>
        /// 计划开盘时间
        /// </summary>
        public string MarketIndentPlannedTime { get; set; }

        /// <summary>
        /// 工程名称
        /// </summary>
        public string ContractProjectName { get; set; }

        /// <summary>
        /// 施工单位
        /// </summary>
        public string Builder { get; set; }

        /// <summary>
        /// 施工部位
        /// </summary>
        public string MarketIndentConstructionSite { get; set; }

        /// <summary>
        /// 砼标号
        /// </summary>
        public string MarketIndentStrengthLevel { get; set; }

        /// <summary>
        /// 坍落度
        /// </summary>
        public string Slumps { get; set; }

        /// <summary>
        /// 生产线
        /// </summary>
        public string ProductionLineCode { get; set; }


        /// <summary>
        /// 浇筑方式
        /// </summary>
        public string CastingMode { get; set; }
    }
}
";
            text = textBox1.Text;
            if (string.IsNullOrEmpty(text)) return;
            var allLines = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> fields;
            if (checkBox1.Checked)
            {
                fields = Line(allLines);
            }
            else
            {
                fields = Default(allLines);
            }
            GetResult(fields);
        }
        private Dictionary<string, string> Default(string[] allLines)
        {
            var temp = new List<string>();
            foreach (var item in allLines)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var line = item.Trim();
                if (new Regex("^(///[\\s\\S]+(summary|default|length|nullable|<para>)[\\s\\S]+)|(public[\\s\\S]+class[\\s\\S]+)", RegexOptions.IgnoreCase).IsMatch(line))
                {
                    continue;
                }
                if (new Regex("^(///[\\s\\S]+)|(public[\\s\\S]+)", RegexOptions.IgnoreCase).IsMatch(line))
                {
                    temp.Add(line);
                }
            }
            Dictionary<string, string> fields = new Dictionary<string, string>();
            string desc = null;
            string name;
            foreach (var item in temp)
            {
                Regex regexDesc = new Regex("/// *(desc)*([\\s\\S]+)", RegexOptions.IgnoreCase);
                Regex regexField = new Regex("public ([\\s\\S]+) ([^{]+)[\\s\\S]+get[\\s\\S]+set", RegexOptions.IgnoreCase);
                if (regexDesc.IsMatch(item))
                {
                    desc = regexDesc.Matches(item)[0].Groups[2].Value.Trim();
                }
                else if (regexField.IsMatch(item))
                {
                    foreach (Group a in regexField.Matches(item)[0].Groups)
                    {
                        Console.WriteLine(a.Value);
                    }
                    string type = regexField.Matches(item)[0].Groups[1].Value.Trim();
                    name = regexField.Matches(item)[0].Groups[2].Value.Trim();
                    fields[name + "__TYPE:" + type] = desc;
                }
            }
            return fields;
        }
        private Dictionary<string, string> Line(string[] allLines)
        {
            var temp = new List<string>();
            foreach (var item in allLines)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var line = item.Trim();
                if (!new Regex("^[^/]+//[^/]+$", RegexOptions.IgnoreCase).IsMatch(line))
                {
                    continue;
                }
                temp.Add(line);
            }
            Dictionary<string, string> fields = new Dictionary<string, string>();
            string desc = null;
            string name;
            foreach (var str in temp)
            {
                var item = str;
                Regex regexDesc = new Regex("//([\\s\\S]+)", RegexOptions.IgnoreCase);
                if (item.IndexOf("=") != -1)
                {
                    item = "t." + str.Substring(0, str.IndexOf("="));//拼接注释
                    item += str.Substring(str.IndexOf("//"));//拼接注释
                }
                Regex regexField = new Regex("[\\s\\S]+\\.([^,]+)[,]{0,1}[\\s\\S]*//[\\s\\S]+", RegexOptions.IgnoreCase);
                desc = regexDesc.Matches(item)[0].Groups[1].Value.Trim();
                var a = regexField.Matches(item)[0];
                name = regexField.Matches(item)[0].Groups[1].Value.Trim();
                fields[name] = desc;
            }
            return fields;
        }
        public void GetResult(Dictionary<string, string> fields)
        {
            string allFields = string.Join(",", fields.Select(it => $"\"{GetFieldName(it.Key, it.Value, out string _, out string fieldName, out string desc, out string sortable, true)}\""));
            string json = string.Join("\r\n", fields.Select(it => $"\t{GetFieldName(it.Key, it.Value, out string type, out string fieldName, out string desc, out string sortable, true).ToFirstLetterLower()}:\"{it.Value}\",{(string.IsNullOrEmpty(type) ? "" : "//" + type)}"));
            //string jsTable = string.Join("\r\n", fields.Select(it => $"<el-table-column prop=\"{GetFieldName(it.Key, out string _).ToFirstLetterLower()}\" label=\"{it.Value}\" align=\"center\"></el-table-column>"));
            //<el-table-column show-overflow-tooltip align="left" prop="key" label="字典名"/>
            string jsTable = string.Join("\r\n", fields.Select(it => $"<el-table-column prop=\"{GetFieldName(it.Key, it.Value, out string type, out string fieldName, out string desc, out string sortable).ToFirstLetterLower()}\" label=\"{desc}\" align=\"{(IsNumber(fieldName, type) ? "right" : "left")}\" {(IsShowOverflowTooltip(fieldName, type) ? "show-overflow-tooltip" : "")}{sortable}{(GetMinWidth(fieldName, type, out int minWidth) ? " min-width=\"" + minWidth + "\"" : "")}></el-table-column>"));
            label1.Text = "{\r\n" + json + "\r\n}\r\n\r\n" + jsTable;
            textBox2.Text = allFields;
            label2.Text = string.Join("\r\n", fields.Select(it => $@"   
        /// <summary>
        /// {it.Value}
        /// </summary>
        public string {GetFieldName(it.Key, it.Value, out string type, out string fieldName, out string desc, out string sortable)} {{ get; set; }}"));
        }
        private string GetFieldName(string str, string desc, out string type, out string fieldName, out string fieldDesc, out string sortable, bool notChangeFieldName = false)
        {
            string typeSplit = "__TYPE:";
            var index = str.IndexOf(typeSplit);
            if (index == -1)
            {
                type = null;
                fieldName = str;
                fieldDesc = desc;
                AutoSortable(fieldName, out sortable);
                return str;
            }
            fieldName = str.Substring(0, index);
            string oldFieldName = fieldName;
            type = str.Substring(index + typeSplit.Length);
            fieldDesc = null;
            if ("CreateID" == fieldName)
            {
                fieldName = "Creator";
                fieldDesc = "创建人";
            }
            else if ("ModifyID" == fieldName)
            {
                fieldName = "Modifier";
                fieldDesc = "修改人";
            }
            else if ("Reviewer" == fieldName)
            {
                fieldName = "ReviewerName";
                fieldDesc = "审核人员";
            }
            if (fieldDesc == null)
                fieldDesc = desc;
            if (notChangeFieldName)
            {
                fieldName = oldFieldName;
            }
            AutoSortable(fieldName, out sortable);
            return fieldName;
        }
        private void AutoSortable(string name,out string sortable)
        {
            if ("RowIndex" == name || "ModifyID" == name || "Reviewer" == name || "CreateID" == name)
            {
                sortable = null;
            }
            else
            {
                sortable = " sortable=\"custom\"";
            }
        }
        private bool IsNumber(string name, string type)
        {
            if (name == "RowIndex")
            {
                return false;
            }
            else if (name == "Creator")
            {
                return false;
            }
            else if (name == "Status")
            {
                return false;
            }
            if ("int" == type)
            {
                return true;
            }
            else if ("decimal" == type)
            {
                return true;
            }
            else if ("float" == type)
            {
                return true;
            }
            else if ("long" == type)
            {
                return true;
            }
            return false;
        }
        public bool IsShowOverflowTooltip(string name, string type)
        {
            if (type == "string")
            {
                return true;
            }
            return false;
        }
        public bool GetMinWidth(string name, string type, out int minWidth)
        {
            if (type == "datetime?")
            {
                minWidth = 140;
                return true;
            }
            else if (type == "datetime")
            {
                minWidth = 140;
                return true;
            }
            minWidth = 0;
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Clipboard.SetDataObject(textBox2.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1_TextChanged(null, null);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label2.Text);
        }
    }
}
