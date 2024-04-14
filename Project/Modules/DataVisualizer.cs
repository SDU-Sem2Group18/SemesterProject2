using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Modules   
{
    public class Visualization
    {
        private string Name;
        
        private string ImagePath;

        public string GetName(){
            
            
            
            return Name;
        }

        public string GetImagePath(){
            
            
            return ImagePath;
        }

        public void Display(){



        }

        public Visualization(string name, string imagePath){
            Name = name;
            ImagePath = imagePath; 

        }


    }

    public enum DataType{
        //Simple place holders
        Type1,
        Type2,
        Type3
    }

    public class DataVisualizationManager
    {
        private Dictionary<DataType, Visualization> visualizations;

        public DataVisualizationManager()
        {
            visualizations = new Dictionary<DataType, Visualization>();
        }

        public void CreateVisualization(DataType type){

            // Logic to create a visualization based on the DataType
            // Example instantiation - we would have our own logic to define Name and ImagePath
            var visualization = new Visualization($"Visualization for {type}", $"path/to/image/for/{type}");
            visualizations[type] = visualization;
        }

        public Visualization GetVisualization(DataType type){
            
            visualizations.TryGetValue(type, out var visualization);
            return visualization;
        }

        public void UpdateVisualization(DataType type, Visualization newData){

            if(visualizations.ContainsKey(type)) visualizations[type] = newData;

        }
    }
}