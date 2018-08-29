using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Chapter04.Core
{
    public partial class FormQ38 : Form
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FormQ38()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 解析を開始します。
        /// </summary>
        /// <param name="analyzer"></param>
        public FormQ38(MorphologicalAnalyzer analyzer)
        {
            InitializeComponent();
            IDictionary<string, List<Word>> result = analyzer.GetGroupByWord();
            var resultByGroup = result.GroupBy(pair => pair.Value.Count);
            ChartArea chartArea = new ChartArea("base")
            {
                AxisX = { Title = "出現頻度" },
                AxisY = { Title = "種類数" }
            };

            Series series = new Series
            {
                ChartType = SeriesChartType.Bar,
                Name = "単語の頻出回数",
                ChartArea = chartArea.Name
            };
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);
            foreach (var item in resultByGroup)
            {
                series.Points.AddXY(item.Key, item.Count());
            }
            chart.Series.Add(series);

        }


    }
}
