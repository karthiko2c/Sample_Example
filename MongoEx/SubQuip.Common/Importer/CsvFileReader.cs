using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace SubQuip.Common.Importer
{
    public class CsvFileReader
    {
        private readonly char _seperator = ';';
        private readonly Encoding _encoding = Encoding.UTF8;

        /// <summary>
        /// Default constructor for CsvFileReader. Default seperator is ;
        /// </summary>
        public CsvFileReader()
        {

        }

        /// <summary>
        /// Create a new CsvFileReader with a custom seperator
        /// </summary>
        /// <param name="seperator">The field seperator to use</param>
        /// <param name="encoding"></param>
        public CsvFileReader(char seperator, Encoding encoding)
        {
            this._seperator = seperator;
            this._encoding = encoding;
        }

        public ImportedData ProcessFile(Stream fileStream)
        {
            using (var streamReader = new StreamReader(fileStream, _encoding))
            {
                var headerLine = streamReader.ReadLine();
                var importedData = new ImportedData();
                if (headerLine == null) return importedData;
                var headers = headerLine.Split(_seperator).Select(s => s.Trim()).ToList();
                importedData.Headers.AddRange(headers);
                while (!streamReader.EndOfStream)
                {
                    var rowLine = streamReader.ReadLine();
                    if (rowLine == null) continue;
                    var rowValues = rowLine.Split(_seperator).Select(s => s.Trim()).ToList();
                    var row = new Dictionary<string, string>();
                    for (var i = 0; i < headers.Count; i++)
                    {
                        if (i < rowValues.Count)
                            row.Add(headers[i], rowValues[i]);
                    }
                    importedData.RowData.Add(row);
                }
                return importedData;
            }
        }
    }
}