using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Chapter05.Core;
using Chapter05.Q44.Models;
using GraphX.Controls;
using GraphX.PCL.Common.Enums;
using GraphX.PCL.Logic.Algorithms.LayoutAlgorithms;

namespace Chapter05.Q44
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window,IDisposable
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
        private Graph SetupGraph(int selectIndex)
        {
            //Lets make new data graph instance
            var dataGraph = new Graph();
            IDictionary<string, DataVertex> vertexDictionary = new Dictionary<string, DataVertex>();


            foreach (var sentence in _analyzer.Sentences.Skip(selectIndex).Take(1))
            {
                foreach (var chunk in sentence.Chunks)
                {
                    string text = string.Join(string.Empty, chunk.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface));
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
                    string from = string.Join(string.Empty, chunk.Morphs.Where(m => m.Pos != Morph.SignPosName).Select(m => m.Surface));
                    string to = string.Join(string.Empty,
                        sentence.Chunks[chunk.Dst].Morphs.Where(m => m.Pos != Morph.SignPosName)
                            .Select(m => m.Surface));
                    if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                    {
                        continue;
                    }
                    var dataEdge = new DataEdge(vertexDictionary[from], vertexDictionary[to]);
                    dataGraph.AddEdge(dataEdge);
                }

            }

            return dataGraph;
        }


        private void SetupGraphArea(int selectIndex)
        {
            //Lets create logic core and filled data graph with edges and vertices
            var logicCore = new GXLogicCore() { Graph = SetupGraph(selectIndex) };
            //This property sets layout algorithm that will be used to calculate vertices positions
            //Different algorithms uses different values and some of them uses edge Weight property.
            logicCore.DefaultLayoutAlgorithm = LayoutAlgorithmTypeEnum.KK;
            //Now we can set parameters for selected algorithm using AlgorithmFactory property. This property provides methods for
            //creating all available algorithms and algo parameters.
            logicCore.DefaultLayoutAlgorithmParams = logicCore.AlgorithmFactory.CreateLayoutParameters(LayoutAlgorithmTypeEnum.KK);
            //Unfortunately to change algo parameters you need to specify params type which is different for every algorithm.
            ((KKLayoutParameters)logicCore.DefaultLayoutAlgorithmParams).MaxIterations = 100;

            //This property sets vertex overlap removal algorithm.
            //Such algorithms help to arrange vertices in the layout so no one overlaps each other.
            logicCore.DefaultOverlapRemovalAlgorithm = OverlapRemovalAlgorithmTypeEnum.FSA;
            //Default parameters are created automaticaly when new default algorithm is set and previous params were NULL
            logicCore.DefaultOverlapRemovalAlgorithmParams.HorizontalGap = 50;
            logicCore.DefaultOverlapRemovalAlgorithmParams.VerticalGap = 50;

            //This property sets edge routing algorithm that is used to build route paths according to algorithm logic.
            //For ex., SimpleER algorithm will try to set edge paths around vertices so no edge will intersect any vertex.
            //Bundling algorithm will try to tie different edges that follows same direction to a single channel making complex graphs more appealing.
            logicCore.DefaultEdgeRoutingAlgorithm = EdgeRoutingAlgorithmTypeEnum.SimpleER;

            //This property sets async algorithms computation so methods like: Area.RelayoutGraph() and Area.GenerateGraph()
            //will run async with the UI thread. Completion of the specified methods can be catched by corresponding events:
            //Area.RelayoutFinished and Area.GenerateGraphFinished.
            logicCore.AsyncAlgorithmCompute = false;

            //Finally assign logic core to GraphArea object
            Area.LogicCore = logicCore;
        }

        public void Dispose()
        {
            //If you plan dynamicaly create and destroy GraphArea it is wise to use Dispose() method
            //that ensures that all potential memory-holding objects will be released.
            Area.Dispose();
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
            //Lets generate configured graph using pre-created data graph assigned to LogicCore object.
            //Optionaly we set first method param to True (True by default) so this method will automatically generate edges
            //  If you want to increase performance in cases where edges don't need to be drawn at first you can set it to False.
            //  You can also handle edge generation by calling manually Area.GenerateAllEdges() method.
            //Optionaly we set second param to True (True by default) so this method will automaticaly checks and assigns missing unique data ids
            //for edges and vertices in _dataGraph.
            //Note! Area.Graph property will be replaced by supplied _dataGraph object (if any).
            Area.GenerateGraph(true, true);

            /* 
             * After graph generation is finished you can apply some additional settings for newly created visual vertex and edge controls
             * (VertexControl and EdgeControl classes).
             * 
             */

            //This method sets the dash style for edges. It is applied to all edges in Area.EdgesList. You can also set dash property for
            //each edge individually using EdgeControl.DashStyle property.
            //For ex.: Area.EdgesList[0].DashStyle = GraphX.EdgeDashStyle.Dash;
            Area.SetEdgesDashStyle(EdgeDashStyle.Dash);

            //This method sets edges arrows visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowArrows = true;
            Area.ShowAllEdgesArrows(false);

            //This method sets edges labels visibility. It is also applied to all edges in Area.EdgesList. You can also set property for
            //each edge individually using property, for ex: Area.EdgesList[0].ShowLabel = true;
            Area.ShowAllEdgesLabels(true);

        }
    }

    public class SentenceViewModel
    {
        public string Text { get; set; }

        public Sentence Sentence { get; set; }

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
    }
}
