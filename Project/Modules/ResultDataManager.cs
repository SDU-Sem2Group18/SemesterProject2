using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.VisualBasic;

namespace Project.Modules
{
    public class ResultDataManager: IDisposable {

        [Delimiter(",")]
        [CultureInfo("dk-DK")]
        [InjectionOptions(CsvHelper.Configuration.InjectionOptions.Exception)]
        public class ResultData {
            [Index(0)]
            [Name("Unit Name")]
            public string UnitName { get; set; }

            [Index(1)]
            [Name("Produced Heat (MWh)")]
            public float ProducedHeat { get; set; }

            [Index(2)]
            [Name("Produced Electricity (MWh)")]
            public float ProducedElectricity { get; set; }

            [Index(3)]
            [Name("Consumed Electricity (MWh)")]
            public float ConsumedElectricity { get; set; }

            [Index(4)]
            [Name("Energy Consumed (MWh)")]
            public float EnergyConsumed { get; set; }
            
            
            [Index(5)]
            [Name("CO2 Emissions (tonnes)")]
            public float CO2Emissions { get; set; }
            
            
            
            [Index(6)]
            [Name("Production Cost (Kr)")]
            public float ProductionCost { get; set; }
            
            
            [Index(7)]
            [Name("Profit (Kr)")]
            public float Profit { get; set; }

            public ResultData(
            string unitName, 
            float producedHeat, 
            float producedELectricity, 
            float consumedElectricity, 
            float energyConsumed, 
            float co2Emissions, 
            float productionCost, 
            float profit) {
                UnitName = unitName;
                ProducedHeat = producedHeat;
                ProducedElectricity = producedELectricity;
                ConsumedElectricity = consumedElectricity;
                EnergyConsumed = energyConsumed;
                CO2Emissions = co2Emissions;
                ProductionCost = productionCost;
                Profit = profit;
            }
        }

        public List<ResultData> ResultDataList = new List<ResultData>();

        public bool SaveResultData(string path, bool overwrite){
            // Check if file path is valid
            try
    {
        // Check if file path is valid
        FileInfo fi = new FileInfo(path);

        long length;
        if (File.Exists(path))
        {
            using (var file = File.Open(path, FileMode.Open))
            {
                length = file.Length;
            }
        }
        else
        {
            using (var file = File.Create(path)) { }
            length = 0;
        }

        using (var writer = new StreamWriter(path, overwrite))
        using (var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("dk-DK")))
        {
            if (length!= 0 &&!overwrite)
            {
                throw new IOException("File already exists and overwrite is false.");
            }
            else
            {
                csv.WriteRecords(ResultDataList);
            }
        }
        return true;
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error: Invalid file path. {ex.Message}");
        return false;
    }
    catch (PathTooLongException ex)
    {
        Console.WriteLine($"Error: File path is too long. {ex.Message}");
        return false;
    }
    catch (NotSupportedException ex)
    {
        Console.WriteLine($"Error: File path is not supported. {ex.Message}");
        return false;
    }
    catch (IOException ex)
    {
        Console.WriteLine($"Error: Unable to write to file. {ex.Message}");
        return false;
    }
    catch (CsvHelper.WriterException ex)
    {
        Console.WriteLine($"Error: Unable to write CSV data. {ex.Message}");
        return false;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: An unexpected error occurred. {ex.Message}");
        return false;
    }
        }

        public List<ResultData>? GetResultData(){
            return ResultDataList;
        }

        //Constructor for ResultDataManager
        public ResultDataManager(){}

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool Disposed {get; private set; }
        protected virtual void Dispose(bool disposing) {
            Disposed = true;
        }
    }
}