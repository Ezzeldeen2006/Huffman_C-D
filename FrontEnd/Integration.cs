using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CND
{
   public  class Integration  
    {
        private string executablePath;
        private int bufferSize;

        
        public event EventHandler<string> ProgressUpdate;
        public event EventHandler<bool> OperationCompleted;

        public Integration(string exePath, int bufferSize = 1024)
        {
            this.executablePath = exePath;
            this.bufferSize = bufferSize;

           
            if (!File.Exists(executablePath))
            {
                throw new FileNotFoundException("Huffman compressor executable not found " + exePath);
            }
        }

        
        public async Task<bool> CompressFileAsync(string inputFilePath, string outputFilePath = null)
        {
           
            if (string.IsNullOrEmpty(outputFilePath))
            {
                outputFilePath = inputFilePath + ".ece2103";
            }

            return await ExecuteOperationAsync("compress", inputFilePath, outputFilePath);
        }

        public async Task<bool> DecompressFileAsync(string inputFilePath, string outputFilePath = null)
        {
            
            if (string.IsNullOrEmpty(outputFilePath))
            {
                if (inputFilePath.EndsWith(".ece2103"))
                {
                    outputFilePath = inputFilePath.Substring(0, inputFilePath.Length - 8);
                }
                else
                {
                    outputFilePath = Path.GetDirectoryName(inputFilePath) + "\\" + Path.GetFileNameWithoutExtension(inputFilePath) + "_decompressed" + Path.GetExtension(inputFilePath);
                }
            }

            return await ExecuteOperationAsync("decompress", inputFilePath, outputFilePath);
        }

        private async Task<bool> ExecuteOperationAsync(string operation, string inputFile, string outputFile) 
        {
            return await Task.Run(() => ExecuteOperation(operation, inputFile, outputFile));
        }

        private bool ExecuteOperation(string operation, string inputFile, string outputFile)  
        {
            try
            {
               
                if (!File.Exists(inputFile))
                {
                    OnProgressUpdate($"Error: Input file not found: {inputFile}");
                    return false;
                }

                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = executablePath,
                    Arguments = operation == "compress"
                        ? $"compress \"{inputFile}\" \"{outputFile}\" {bufferSize}"
                        : $"decompress \"{inputFile}\" \"{outputFile}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                OnProgressUpdate("Starting " + operation + "...");

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;

                    // Handle real-time output
                    process.OutputDataReceived += (sender, e) => 
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            OnProgressUpdate(e.Data);
                        }
                    };

                    process.ErrorDataReceived += (sender, e) => 
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            OnProgressUpdate($"Error: {e.Data}");
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    
                    process.WaitForExit();

                    bool success = process.ExitCode == 0;

                    if (success)
                    {
                        OnProgressUpdate($"{char.ToUpper(operation[0]) + operation.Substring(1)} completed successfully!");

                       
                        if (File.Exists(outputFile))
                        {
                            FileInfo inputInfo = new FileInfo(inputFile);
                            FileInfo outputInfo = new FileInfo(outputFile);

                            if (operation == "compress")
                            {
                                double ratio = (1.0 - (double)outputInfo.Length / inputInfo.Length) * 100;
                                OnProgressUpdate($"Original: {FormatFileSize(inputInfo.Length)}, " +
                                               $"Compressed: {FormatFileSize(outputInfo.Length)}, " +
                                               $"Ratio: {ratio:F1}%");
                            }
                            else
                            {
                                OnProgressUpdate($"Decompressed file size: {FormatFileSize(outputInfo.Length)}");
                            }
                        }
                    }
                    else
                    {
                        OnProgressUpdate($"{char.ToUpper(operation[0]) + operation.Substring(1)} failed!");
                    }

                    OnOperationCompleted(success);
                    return success;
                }
            }
            catch (Exception ex)
            {
                OnProgressUpdate($"Error: {ex.Message}");
                OnOperationCompleted(false);
                return false;
            }
        }

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        private void OnProgressUpdate(string message)
        {
            ProgressUpdate?.Invoke(this, message);
        }

        private void OnOperationCompleted(bool success)
        {
            OperationCompleted?.Invoke(this, success);
        }

       
        public bool CompressFile(string inputFilePath, string outputFilePath = null)
        {
            return CompressFileAsync(inputFilePath, outputFilePath).Result;
        }

        public bool DecompressFile(string inputFilePath, string outputFilePath = null)
        {
            return DecompressFileAsync(inputFilePath, outputFilePath).Result;
        }
    }
}
