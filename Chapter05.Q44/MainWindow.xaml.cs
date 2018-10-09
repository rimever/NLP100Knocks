#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Chapter05.Core;
using Chapter05.Q44.Models;
using GraphX.Controls;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;

#endregion

namespace Chapter05.Q44
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        readonly CabochaAnalyzer _analyzer = new CabochaAnalyzer();

        public MainWindow()
        {
            InitializeComponent();
            //Customize Zoombox a bit
            //Set minimap (overview) window to be visible by default
            ZoomControl.SetViewFinderVisibility(ZoomControl, Visibility.Visible);
            //Set Fill zooming strategy so whole graph will be always visible
            ZoomControl.ZoomToFill();
            _analyzer.Execute();
            foreach (var sentence in _analyzer.Sentences)
            {
                ListBoxSentence.Items.Add(new SentenceViewModel(sentence).Text);
            }
        }

        public void Dispose()
        {
            //If you plan dynamicaly create and destroy GraphArea it is wise to use Dispose() method
            //that ensures that all potential memory-holding objects will be released.
            Area.Dispose();
        }

        private Graph SetupGraph(int selectIndex)
        {
            //Lets make new data graph instance
            var dataGraph = new Graph();
            IDictionary<string, DataVertex> vertexDictionary = new Dictionary<string, DataVertex>();


            foreach (var sentence in _analyzer.Sentences.Skip(selectIndex).Take(1))
            {
                foreach (var chunk in sentence.Chunks)
                {
                    string text = string.Join(string.Empty,
                        chunk.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface));
                    if (vertexDictionary.ContainsKey(text))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    var vertex = new DataVertex(text);
                    vertexDictionary.Add(text, vertex);
                    dataGraph.AddVertex(vertex);
                }

                foreach (var chunk in sentence.Chunks)
                {
                    if (chunk.Dst == -1)
                    {
                        continue;
                    }

                    string from = string.Join(string.Empty,
                        chunk.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface));
                    string to = string.Join(string.Empty,
                        sentence.Chunks[chunk.Dst].Morphs.Where(m => m.Pos != Morph.SignPosName)
                            .Select(m => m.Surface));
                    if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    {
                        continue;
                    }

                    var dataEdge = new DataEdge(vertexDictionary[from], vertexDictionary[to])
                    {
                        Text = $"{from} -> {to}"
                    };
                    dataGraph.AddEdge(dataEdge);
                }
            }

            return dataGraph;
        }


        private void SetupGraphArea(int selectIndex)
        {
            //Lets create logic core and filled data graph with edges and vertices
            var logicCore = new GXLogicCore {Graph = SetupGraph(selectIndex)};
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set parameters for selected algorithm using AlgorithmFactory property. This property provides methods for
            //creating all available algorithms and algo parameters.
            logicCore.DefaultLayoutAlgorithmParams =
                logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters) logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Default parameters are created automaticaly when new default algorithm is set and previous params were NULL
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;


            //このプロパティは、アルゴリズムロジックに従ってルートパスを構築するために使用されるエッジルーティングアルゴリズムを設定します。
            //例えば、SimpleERアルゴリズムは頂点の周りに辺のパスを設定しようとするので、辺が頂点と交差することはありません。
            // Bundlingアルゴリズムは、複雑なグラフをより魅力的にする単一のチャンネルと同じ方向に続く異なるエッジを結びつけるよう試みます。
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;


            //このプロパティは、非同期アルゴリズムの計算を、Area.RelayoutGraph（）やArea.GenerateGraph（）のように設定します。
            // UIスレッドで非同期を実行します。指定されたメソッドの完了は、対応するイベントによってキャッチできます。
            //Area.RelayoutFinishedおよびArea.GenerateGraphFinished。
            logicCore.AsyncAlgorithmCompute = false;


            //最後に、GraphAreaオブジェクトにロジックコアを割り当てます
            Area.LogicCore = logicCore;
        }

        private void ListBoxSentence_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            if (listBox == null
                || listBox.SelectedIndex == -1)
            {
                return;
            }

            TextBoxSentence.Text = listBox.SelectedItem.ToString();
            SetupGraphArea(listBox.SelectedIndex);
            // LogicCoreオブジェクトにあらかじめ作成されたデータグラフを使用して、設定されたグラフを生成します。
            //このメソッドが自動的にエッジを生成するように、最初のメソッドparamをTrue（デフォルトはTrue）に設定することもできます
            //最初にエッジを描画する必要がない場合にパフォーマンスを向上させたい場合は、Falseに設定します。
            //手動でArea.GenerateAllEdges（）メソッドを呼び出すことによって、エッジ生成を処理することもできます。
            //また、2番目のparamをTrue（デフォルトはTrue）に設定すると、このメソッドは自動的に欠落している一意のデータIDをチェックして割り当てます
            //_dataGraphの辺と頂点のです。
            //注意！ Area.Graphプロパティは、指定された_dataGraphオブジェクト（存在する場合）に置き換えられます。
            Area.GenerateGraph(true, true);


            /*
            *グラフの生成が完了したら、新しく作成されたビジュアル頂点とエッジコントロールにいくつかの追加設定を適用できます
            *（VertexControlおよびEdgeControlクラス）。
        *
            */

            //このメソッドは、エッジのダッシュスタイルを設定します。 Area.EdgesListのすべてのエッジに適用されます。あなたはダッシュプロパティを
            // EdgeControl.DashStyleプロパティを使用して各エッジを個別に処理します。
            //例：Area.EdgesList [0] .DashStyle = GraphX.EdgeDashStyle.Dash;

            Area.SetEdgesDashStyle(EdgeDashStyle.Dash);
            //このメソッドは、エッジ矢印の可視性を設定します。 Area.EdgesListのすべてのエッジにも適用されます。プロパティを設定することもできます
            //それぞれの辺を個別にプロパティを使用して、例：Area.EdgesList [0] .ShowArrows = true;
            Area.ShowAllEdgesArrows(false);


            //このメソッドは、エッジラベルの可視性を設定します。 Area.EdgesListのすべてのエッジにも適用されます。プロパティを設定することもできます
            //それぞれの辺が個別にプロパティを使用します。例：Area.EdgesList [0] .ShowLabel = true;
            // 今回は表示しない
            Area.ShowAllEdgesLabels(false);
        }
    }

    public class SentenceViewModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sentence"></param>
        public SentenceViewModel(Sentence sentence)
        {
            Sentence = sentence;
            Text = string.Join(string.Empty,
                sentence.Chunks.Select(c => string.Join(string.Empty, c.Morphs.Select(m => m.Surface))));
        }

        public string Text { get; set; }

        public Sentence Sentence { get; set; }
    }
}