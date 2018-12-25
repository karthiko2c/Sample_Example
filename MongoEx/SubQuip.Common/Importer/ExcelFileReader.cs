using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;

namespace SubQuip.Common.Importer
{
    public class ExcelFileReader
    {
        public ImportedData ProcessFile(Stream fileStream)
        {
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Load(fileStream);

                var importedData = new ImportedData();
                var currentSheet = excelPackage.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                var obj = (object[,])workSheet.Cells.Value;

                for (var firstRow = 0; firstRow < noOfCol; firstRow++)   // header row for columns name
                {
                    try
                    {
                        var columnName = Convert.ToString(obj[0, firstRow]);
                        if (string.IsNullOrEmpty(columnName) == false)
                            importedData.Headers.Add(Convert.ToString(obj[0, firstRow]).Trim().ToLower());
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        if (!string.IsNullOrEmpty(ex.Message))
                            continue;
                    }
                }

                if (importedData.Headers.Count != noOfCol)
                    noOfCol = importedData.Headers.Count;

                for (var row = 1; row < noOfRow; row++)  // start from second means data row 
                {
                    var data = new Dictionary<string, string>();
                    for (var col = 0; col < noOfCol; col++)
                    {
                        try
                        {
                            data.Add(importedData.Headers[col], Convert.ToString(obj[row, col]));
                        }
                        catch (Exception ex)
                        {
                            if (!string.IsNullOrEmpty(ex.Message))
                                throw;
                        }
                    }
                    importedData.RowData.Add(data);
                }
                return importedData;

            }
        }
    }
}
