#region

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

#endregion

namespace Chapter04.Core
{
    public partial class FormQ37 : Form
    {
        public FormQ37()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyzer"></param>
        public FormQ37(MorphologicalAnalyzer analyzer)
        {
            InitializeComponent();

            IDictionary<string, List<Word>> result = analyzer.GetGroupByWord();

            ChartArea chartArea = new ChartArea("base")
            {
                AxisX = {Title = "単語"},
                AxisY = {Title = "頻出回数"}
            };

            Series series = new Series
            {
                ChartType = SeriesChartType.Bar,
                Name = "単語の頻出回数",
                XValueType = ChartValueType.String,
                ChartArea = chartArea.Name
            };
            chartArea.AxisX.MajorGrid.Interval = 1;
            chartArea.AxisX.LabelStyle = new LabelStyle
            {
                Interval = 1
            };
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);
            foreach (var item in result.OrderByDescending(pair => pair.Value.Count).Take(10)
                .Select((value, index) => new {value, index}))
            {
                var first = item.value.Value.FirstOrDefault();
                series.Points.AddXY(first.Base, item.value.Value.Count);
            }

            chart.Series.Add(series);
        }
    }
}