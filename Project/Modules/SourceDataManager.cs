using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.VisualBasic;

namespace Project.Modules
{
    public class SourceDataManager : IDisposable
    {
        [Delimiter(",")]
        [CultureInfo("dk-DK")]
        [InjectionOptions(CsvHelper.Configuration.InjectionOptions.Exception)]
        public class HeatData {
            [Name("Time from")]
            [Format("dd/MM/yyyy HH:mm")]
            public DateTime TimeFrom { get; set; }

            [Name("Time to")]
            [Format("dd/MM/yyyy HH:mm")]
            public DateTime TimeTo { get; set; }

            [Name("Heat Demand")]
            public float HeatDemand { get; set; }

            [Name("Electricity Price")]
            public float ElectricityPrice { get; set; }
        }
        string SourcePath;

        public SourceDataManager(string sourcePath) {
            SourcePath = sourcePath;
        }

        public List<HeatData> GetHeatData() {
            List<HeatData> returnList= new List<HeatData>();
            try {
                using (var reader = new StreamReader(SourcePath))
                using (var csv = new CsvReader(reader, new CultureInfo("dk-DK", false))) {
                    var records = csv.GetRecords<HeatData>();
                    foreach (var record in records) {
                        returnList.Add(record);
                    }
                }
                return returnList;
            } catch (System.IO.DirectoryNotFoundException) {
                throw new Exception("The source file could not be found. Please check the path and try again.");
            } catch (System.IO.FileNotFoundException) {
                throw new Exception("The source file could not be found. Please check the path and try again.");
            } catch (Exception ex) {
                throw new Exception($"Something went wrong while parsing the source file. Please check the file and try again. Details:\n{ex.Message}");
            }
        }

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