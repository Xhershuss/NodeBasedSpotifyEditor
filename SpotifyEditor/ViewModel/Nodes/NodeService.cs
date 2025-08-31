using SpotifyEditor.Model;
using SpotifyEditor.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.ViewModel.Nodes
{
    public class NodeService
    {
        private static readonly Lazy<NodeService> _lazy
            = new Lazy<NodeService>(() => new NodeService());

        public static NodeService Instance => _lazy.Value;

        private NodeService() { }
        public ObservableCollection<BaseNodeModel> AllNodes { get; }
                                    = new ObservableCollection<BaseNodeModel>();

        public int IsAllSystemsExecuted = 0;
 

        public ObservableCollection<BaseNodeModel> _startNodes { get; set; }

        public BaseNodeModel _endNode { get; set; }

        public void DetermineStartAndEndNodes()
        {
            var startList = new List<BaseNodeModel>();
            var endList = new List<BaseNodeModel>();

            foreach (var node in AllNodes)
            {

                if (node.InputPorts.Any())
                {
                    bool allInputsEmpty = true;
                    foreach (var inPort in node.InputPorts)
                    {
                        if (inPort.ConnectedNodes.Count > 0)
                        {
                            allInputsEmpty = false;
                            break;
                        }
                    }
                    if (allInputsEmpty)
                        startList.Add(node);
                }
                else
                {
                    // If there are no input ports, consider it a start node
                    startList.Add(node);
                }


                if (node.OutputPorts.Any())
                {

                    bool allOutputsEmpty = true;
                    foreach (var outPort in node.OutputPorts)
                    {
                        if (outPort.ConnectedNodes.Count > 0)
                        {
                            allOutputsEmpty = false;
                            break;
                        }
                    }
                    if (allOutputsEmpty)
                        endList.Add(node);
                }
                else
                {
                    // If there are no output ports, consider it an end node
                    endList.Add(node);
                }
            }

            

            if (endList.Count != 1)
            {
                throw new InvalidOperationException("End Node must be one and only");
            }
            _endNode = endList.Single();
            _startNodes = new ObservableCollection<BaseNodeModel>(startList);

        }


        public virtual async Task Execute(ObservableCollection<BaseNodeModel> nodes)
        {

            var tasks = new List<Task>();

            bool allExecuted = false;
            var scheduled = new HashSet<BaseNodeModel>();
            var executedNodes = new List<BaseNodeModel>();

            foreach (var node in nodes)
            {

                if (scheduled.Contains(node)) continue;
                if (node.IsExecuted) continue;

                allExecuted = node.InputPorts
                            .All(inputPort => inputPort.ConnectedNodes.All(n => n.IsExecuted));

                var collectedData = new List<object>();
                foreach (var inputPort in node.InputPorts)
                {
                    
                    foreach (var port in inputPort.ConnectedOutputPorts)
                    {
                        collectedData.Add(port.OutputPortData);
                    }
                    
                }
                var context = new DataExecutionContext {
                    Data = collectedData,
                    Metadata = new Dictionary<string, object>(),
                    MultiplicityState = collectedData.Count() == 1 ? DataExecutionContext.DataMultiplicity.Single: DataExecutionContext.DataMultiplicity.Multiple
                };

                if (allExecuted)
                {
                    tasks.Add(node.Process(context));
                    executedNodes.Add(node);
                    scheduled.Add(node);

                }
            }

            await Task.WhenAll(tasks);


            if (nodes.FirstOrDefault() == _endNode)
                return;

            var nextNodes = GetNextNodes(nodes);
            await Execute(nextNodes);

        }

        public ObservableCollection<BaseNodeModel> GetNextNodes(ObservableCollection<BaseNodeModel> nodes)
        {
            var nextNodes = new ObservableCollection<BaseNodeModel>();
            foreach (var node in nodes)
            {
                if (node.OutputPorts != null && node.OutputPorts.Count > 0)
                {
                    foreach (var outputPort in node.OutputPorts)
                    {
                        if (outputPort.ConnectedNodes != null && outputPort.ConnectedNodes.Count > 0)
                        {
                            foreach (var n in outputPort.ConnectedNodes)
                                nextNodes.Add(n);

                        }
                    }
                }
            }
            return nextNodes;
        }


    }
}
