using DataMatrix.net;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace DatamatrixCode
{
	public class ExcelToBitmapConverter
	{
		private readonly DataTable table;
        private readonly DmtxImageEncoderOptions encoderOptions;

		public ExcelToBitmapConverter(DataTable table, DmtxImageEncoderOptions encoderOptions)
		{
			this.table = table;
            this.encoderOptions = encoderOptions;
		}

		public List<BitmapModel> CreateBitmapsFromExcel()
		{
            if (this.table.Rows.Count < 1)
            {
                ErrorPrompt.errorPrompt("Can't read file well! Did you start inputting data on row 2?");
            }

            string productCode;
            string batch;
            string exp;
            string serialNumber;
            List<BitmapModel> bitmaps = new List<BitmapModel>();
            DmtxImageEncoder encoder = new DmtxImageEncoder();

            // Keep track of all serialNumbers to ensure there's no duplicate
            List<string> serialNumbers = new List<string>();

            // Assuming following format:
            // Product Code | LOT (Batch) | EXP | Serial Number
            for (var i = 0; i < this.table.Rows.Count; i++)
            {
                productCode = this.table.Rows[i][0].ToString();
                if (productCode.Length < 14)
                {
                    while (productCode.Length != 14)
                    {
                        productCode = "0" + productCode;
                    }
                }
                batch = this.table.Rows[i][1].ToString();
                exp = this.table.Rows[i][2].ToString();
                serialNumber = this.table.Rows[i][3].ToString();
                serialNumbers.Add(serialNumber);

                var bitmapCodeString = EncodeProduct(productCode, batch, exp, serialNumber);
                var bitmap = encoder.EncodeImage(bitmapCodeString, this.encoderOptions);

                var model = new BitmapModel()
                {
                    bitmap = bitmap,
                    ProductCode = productCode,
                    Batch = batch,
                    SerialNumber = serialNumber,
                    Exp = exp,
                    RunningNumber = (i + 1).ToString("00000")
                };
                bitmaps.Add(model);
            }

            if (serialNumbers.Distinct().Count() != serialNumbers.Count)
			{
                var dupes = serialNumbers.GroupBy(x => x)
                              .Where(g => g.Count() > 1)
                              .Select(y => y.Key)
                              .ToList();

                var first = serialNumbers.IndexOf(dupes[0]);

                ErrorPrompt.errorPrompt($"Found a SerialNumber that exists twice on line {first + 2}!");
            }

            return bitmaps;
        }

        private string EncodeProduct(string productCode, string batch, string exp, string serialNumber)
        {
            return $"01{productCode}21{serialNumber}\u001d10{batch}\u001d17{exp}";
        }
    }
}
