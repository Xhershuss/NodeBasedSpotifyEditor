using Microsoft.Win32;
using Newtonsoft.Json;
using SpotifyEditor.Api.Models;
using SpotifyEditor.Model.Nodes;
using SpotifyEditor.View.Nodes.Ports;
using SpotifyEditor.ViewModel.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace SpotifyEditor.Helpers.FileController
{

    public static class FileService
    {
  

        public static void Save(string filePath)
        {
            
            List<SerializableNode> serializableNodes = NodeService.Instance.AllNodes.Select(o=>
                new SerializableNode
                {
                    Id = o.Id,
                    NodeType = o.NodeType,
                    Name = o.Name,
                    UserInputText = o.UserInputText,
                    PosX = o.Position.X,
                    PosY = o.Position.Y,
                    InputCount = o.InputCount,
                    OutputCount = o.OutputCount,
                    InputPorts = o.InputPorts.Select(p => new SerializablePort
                    {
                        PortType = "InputPort",
                        PortId = p.Id,
                        ConnectedPortIds = p.ConnectedNodes.Select(c => c.Id).ToList(),
                        PosX = p.Position.X,
                        PosY = p.Position.Y
                    }).ToList(),
                    OutputPorts = o.OutputPorts.Select(p => new SerializablePort
                    {
                        PortType = "OutputPort",
                        PortId = p.Id,
                        ConnectedPortIds = p.ConnectedNodes.Select(c => c.Id).ToList(),
                        PosX = p.Position.X,
                        PosY = p.Position.Y
                    }).ToList()
                }
            ).ToList();



            var successedConnections = InputPortView.SuccessedConnections;
            List<PortConnection> connections = successedConnections.Select(o=>
                new PortConnection
                {
                    StartPort = new SerializablePort
                    {
                        PortType = "InputPort",
                        PortId = o.Start.Id,
                        PosX = o.Start.portModel.Position.X,
                        PosY = o.Start.portModel.Position.Y,

                    },
                    EndPort = new SerializablePort
                    {
                        PortType = "OutputPort",
                        PortId = o.End.Id,
                        PosX = o.End.portModel.Position.X,
                        PosY = o.End.portModel.Position.Y,
                    }
                }
            ).ToList();


            var saveData = new ProjectDto
            {
                SavedAt = DateTime.UtcNow,
                Nodes = serializableNodes,
                Connections = connections
            };


            var settings = new JsonSerializerSettings
            {
               
                Formatting = Formatting.Indented,
            };

            string json = JsonConvert.SerializeObject(saveData,settings);
            File.WriteAllText(filePath, json);
        }

        public static string SelectFolder()
        {
            var folderDialog = new OpenFolderDialog();
            folderDialog.Title = "Select Folder";

            if (folderDialog.ShowDialog() == true)
            {
                var folderName = folderDialog.FolderName;

                string uniqueName = DateTime.Now.ToString("yyMMdd_HHmmss");
                string filePath = Path.Combine(folderName, $"ProjectData_{uniqueName}.json");
                
                return filePath;
            }
            return string.Empty;
        }

       
        public static ProjectDto? Load(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var projectDto = JsonConvert.DeserializeObject<ProjectDto>(File.ReadAllText(filePath));
            if (projectDto == null)
                throw new InvalidDataException("The file content is not valid.");
            return projectDto;
           
        }
         
        public static string SelectFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Open Project File",
                Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {

                return openFileDialog.FileName;
            }
            return string.Empty;
        }


    }
}
