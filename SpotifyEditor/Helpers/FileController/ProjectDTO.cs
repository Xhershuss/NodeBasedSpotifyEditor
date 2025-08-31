using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyEditor.Helpers.FileController
{
    public class ProjectDto
    {
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;

        public List<SerializableNode> Nodes { get; set; } 
        public List<PortConnection> Connections { get; set; }
    }
    public class SerializableNode
    {
        public string Id { get; set; }
        public string NodeType { get; set; }
        public string Name { get; set; }
        public string UserInputText { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public int InputCount { get; set; }
        public int OutputCount { get; set; }

        public List<SerializablePort> InputPorts { get; set; }
        public List<SerializablePort> OutputPorts { get; set; }
     


    }

    public class SerializablePort
    {
        public string PortType { get; set; }
        public string PortId { get; set; }
        public List<string> ConnectedPortIds { get; set; }

        public double PosX { get; set; }
        public double PosY { get; set; }
    }

    public class PortConnection
    {
        public SerializablePort StartPort { get; set; }
        public SerializablePort EndPort { get; set; }
    }
}
