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
            var allLines = text.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries);
            var temp = new List<string>();
            foreach (var item in allLines)
            {
                if (string.IsNullOrEmpty(item)) continue;
                var line = item.Trim();
                if (new Regex("^(///[\\s\\S]+(summary|default|length|nullable)[\\s\\S]+)|(public[\\s\\S]+class[\\s\\S]+)", RegexOptions.IgnoreCase).IsMatch(line))
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
            string name = null;
            foreach (var item in temp)
            {
                Regex regexDesc = new Regex("/// *(desc)*([\\s\\S]+)", RegexOptions.IgnoreCase);
                Regex regexField = new Regex("public[\\s\\S]+ ([^{]+)[\\s\\S]+get[\\s\\S]+set", RegexOptions.IgnoreCase);
                if (regexDesc.IsMatch(item))
                {
                    desc = regexDesc.Matches(item)[0].Groups[2].Value.Trim();
                }
                else if (regexField.IsMatch(item))
                {
                    name = regexField.Matches(item)[0].Groups[1].Value.Trim();
                    fields[name] = desc;
                }
            }
            string allFields = string.Join(",", fields.Select(it=>$"\"{it.Key}\""));
            string json = string.Join(",\r\n", fields.Select(it => $"\t{it.Key.ToFirstLetterLower()}:\"{it.Value}\""));
            string jsTable = string.Join("\r\n", fields.Select(it => $"<el-table-column prop=\"{it.Key.ToFirstLetterLower()}\" label=\"{it.Value}\" align=\"center\"></el-table-column>"));

            label1.Text = "{\r\n"+json + "\r\n}\r\n\r\n" + jsTable;
            textBox2.Text = allFields;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(label1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Clipboard.SetDataObject(textBox2.Text);
        }
    }
}
