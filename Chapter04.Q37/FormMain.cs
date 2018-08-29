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
using Chapter04.Core;

namespace Chapter04.Q37
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Load += FormMain_Load;
        }
        private readonly MorphologicalAnalyzer _analyzer = new MorphologicalAnalyzer();


        private void FormMain_Load(object sender, EventArgs e)
        {

            _analyzer.Execute();
            IDictionary<string, List<Word>> result = new Dictionary<string, List<Word>>();
            foreach (var word in _analyzer.EnumerableWords())
            {
                if (!result.ContainsKey(word.Base))
                {
                    result.Add(word.Base, new List<Word>());
                }
                result[word.Base].Add(word);
            }

            ChartArea chartArea = new ChartArea("base")
            {
                AxisX = {Title = "単語"},
                AxisY = {Title = "頻出回数"}
            };

            Series series = new Series
            {
                ChartType = SeriesChartType.Line,
                Name = "単語の頻出回数",                
                XValueType = ChartValueType.String,
                ChartArea = chartArea.Name
            };
            chartArea.AxisX.MajorGrid.Interval = 1;
            chartArea.AxisX.LabelStyle = new LabelStyle()
            {
                Interval = 1
            };
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(chartArea);
            foreach (var item in result.OrderByDescending(pair => pair.Value.Count).Take(10).Select((value, index) => new {value, index}))
            {
                var first = item.value.Value.FirstOrDefault();
                series.Points.AddXY(first.Base, item.value.Value.Count);
            }

            chart.Series.Add(series);
        }
    }
}
