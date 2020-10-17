using ExcelDataReader;
using System;
using System.Data;
using System.IO;

namespace DatamatrixCode
{
	public class ExcelReader
	{
		private readonly string filePath;

		public ExcelReader(string filePath)
		{
			this.filePath = filePath;
		}

        /**
         * Gets the DataTable of the {tableNumber}'th table of the Excel file at object's filePath
         */
		public DataTable GetTableFromFilePath(int tableNumber = 0)
		{
            DataTable dataTable = null;

            try
			{
                using (var stream = File.Open(this.filePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {

                            UseHeaderRow = true
                        }
                    };

                    var dataSet = reader.AsDataSet(conf);
                    dataTable = dataSet.Tables[tableNumber];
                }
            }
            catch (Exception ex)
			{
                ErrorPrompt.errorPrompt("When reading file '{0}' an exception occurred:/n{1}", this.filePath, ex.Message);
            }

            return dataTable;
        }
	}
}
