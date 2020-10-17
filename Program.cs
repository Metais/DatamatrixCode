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

            bool isHorizontal = DirectionQuestion();
            Console.WriteLine("\nWill print in " + (isHorizontal ? "horizontal" : "vertical") + " direction...");

            bool textOnSameDirection = TextPositionQuestion();
            Console.WriteLine("\nWill write text in " + (textOnSameDirection ? "same path as code" : "wider area"));

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

                var formattedBitmaps = new BitmapsFormatter(bitmaps).FormatBitmaps(isHorizontal, textOnSameDirection);

                SaveBitmap(file, formattedBitmaps);
            }
            
        }

        private static void SaveBitmap(string file, Bitmap bitmap)
		{
            try
            {
                var fileName = file.Remove(file.LastIndexOf('.'));
                fileName = fileName.Substring(file.LastIndexOf('\\'));
                bitmap.Save("OutputFiles/" + fileName + "Processed.png", ImageFormat.Png);
            }
            catch (Exception ex)
            {
                Console.WriteLine("When saving the datamatrix image an exception occurred:\n{0}", ex.Message);
            }
        }

        private static bool DirectionQuestion()
		{
            bool isHorizontal = true;

            Console.WriteLine("Would you like to print (h)orizontal or (v)ertical?");
            var directionAnswer = Console.ReadKey().Key;
            if (directionAnswer == ConsoleKey.H)
            {
                isHorizontal = true;
            }
            else if (directionAnswer == ConsoleKey.V)
            {
                isHorizontal = false;
            }
            else
            {
                ErrorPrompt.errorPrompt("Please enter either [h] or [v]!");
            }

            return isHorizontal;
        }

        private static bool TextPositionQuestion()
        {
            bool textOnSameDirection = true;
            Console.WriteLine("Do you want the text in the [s]ame line/path as the code, or [n]ext to it? (same line = longer line, next to it = wider paper)");
            var textPositionAnswer = Console.ReadKey().Key;
            if (textPositionAnswer == ConsoleKey.S)
            {
                textOnSameDirection = true;
            }
            else if (textPositionAnswer == ConsoleKey.N)
            {
                textOnSameDirection = false;
            }
            else
            {
                ErrorPrompt.errorPrompt("Please enter either [s] or [n]!");
            }

            return textOnSameDirection;
        }

    }
}
