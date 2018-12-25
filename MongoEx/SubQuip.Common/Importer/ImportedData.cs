using System.Collections.Generic;

namespace SubQuip.Common.Importer
{
    public class ImportedData
    {
        public List<string> Headers { get; } = new List<string>();
        public List<Dictionary<string, string>> RowData { get; } = new List<Dictionary<string, string>>();

    }
}