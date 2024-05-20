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

        private string FilePath;

        private List<Optimiser.OptimisedData> ResultDataList = new List<Optimiser.OptimisedData>();

        public bool SaveResultData(bool overwrite){
            // Check if file path is valid
            try
            {
                // Check if file path is valid
                FileInfo fi = new FileInfo(FilePath);

                long length;
                if (File.Exists(FilePath))
                {
                    using (var file = File.Open(FilePath, FileMode.Open))
                    {
                        length = file.Length;
                    }
                }
                else
                {
                    using (var file = File.Create(FilePath)) { }
                    length = 0;
                }

                using (var writer = new StreamWriter(FilePath, overwrite))
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

        //Constructor for ResultDataManager
        public ResultDataManager(List<Optimiser.OptimisedData> data, string filePath) {
            ResultDataList = data;
            FilePath = filePath;
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