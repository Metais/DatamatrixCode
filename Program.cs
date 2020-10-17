using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DataMatrix.net;

using System.IO;
using System.Drawing.Imaging;
using System.Data;

namespace DatamatrixCode
{
	class Program
	{

        static readonly DmtxImageEncoderOptions EncoderOptions = new DmtxImageEncoderOptions()
        {
            ForeColor = Color.Black,
            BackColor = Color.White,
            ModuleSize = 8,
            MarginSize = 100,
            Scheme = DmtxScheme.DmtxSchemeAsciiGS1
        };

        static void Main(string[] args)
        {
            string[] filesInDirectory = Directory.GetFiles("InputFiles");

            if(!filesInDirectory.Any())
			{
                ErrorPrompt.errorPrompt("Please enter some files into the 'InputFiles' directory!");
			}

            foreach (var file in filesInDirectory)
			{

                if (new FileInfo(file).Extension != ".xlsx")
                {
                    ErrorPrompt.errorPrompt("Please only place .xlsx files in the InputFiles directory.");
                }

                // ~$ is an opened file, this can happen if you have the excel file open currently on your pc
                if (file.Contains("~$"))
				{
                    ErrorPrompt.errorPrompt("Please close the Excel files that you are trying to process."); ;
				}
                    
                var dataTable = new ExcelReader(file).GetTableFromFilePath();

                var bitmaps = new ExcelToBitmapConverter(dataTable, EncoderOptions).CreateBitmapsFromExcel();

                var formattedBitmaps = new BitmapsFormatter(bitmaps).FormatBitmaps();

                SaveBitmaps(file, formattedBitmaps);
            }

            Console.WriteLine("Finished. Press any key to exit.");
            Console.Read();
            Environment.Exit(-1);
        }
        
        private static void SaveBitmaps(string excelFile, List<BitmapModel> bitmaps)
		{
            foreach(var bitmapModel in bitmaps)
			{
                try
                {
                    var directoryName = excelFile.Remove(excelFile.LastIndexOf('.'));
                    directoryName = directoryName.Substring(excelFile.LastIndexOf('\\'));
                    Directory.CreateDirectory($"OutputFiles/{directoryName}");

                    var fileName = $"OutputFiles/{directoryName}/{bitmapModel.ProductCode} - {bitmapModel.Batch} - {bitmapModel.RunningNumber}.png";
                    bitmapModel.bitmap.Save(fileName, ImageFormat.Png);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"When saving the datamatrix image an exception occurred when handling number {bitmapModel.RunningNumber}:\n{0}", ex.Message);
                }
            }

        }

    }
}
