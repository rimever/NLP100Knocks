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
    public partial class FormQ39 : Form
    {
        private readonly MorphologicalAnalyzer _analyzer = new MorphologicalAnalyzer();

        public FormQ39()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyzer"></param>
        public FormQ39(MorphologicalAnalyzer analyzer)
        {
            InitializeComponent();
            IDictionary<string, List<Word>> result = analyzer.GetGroupByWord();

            ChartArea chartArea = new ChartArea("base")
            {
                AxisX = { Title = "単語頻出順位" },
                AxisY = { Title = "頻出回数" }
            };
            chartArea.AxisX.IsLogarithmic = true;
            chartArea.AxisY.IsLogarithmic = true;

            Series series = new Series
            {
                ChartType = SeriesChartType.Line,
                Name = "単語の頻出回数",
                ChartArea = chartArea.Name
            };
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);
            foreach (var item in result.OrderByDescending(pair => pair.Value.Count).Select((value, index) => new { value, index }))
            {
                series.Points.AddXY(item.index + 1, item.value.Value.Count);
            }

            chart.Series.Add(series);
        }
    }
}
