using System;
namespace SubQuip.Common.CommonData
{
    public class Attachment
    {
		public string Name { get; set; }
		public string ContentType { get; set; }
		public byte[] Content { get; set; }
    }
}
