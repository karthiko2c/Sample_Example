using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SubQuip.Common.CommonData;

namespace SubQuip.Common.Extensions
{
    public static class FileHelper
    {
        public static FileDetails GetFileDetails(IFormFile file)
        {
            var fileModel = new FileDetails
            {
                Name = file.FileName,
                ContentType = file.ContentType,
                Content = GetFileContent(file)
            };
            return fileModel;
        }

        public static string GetFileContent(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }
            string content = null;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                content = Convert.ToBase64String(fileBytes);
            }
            return content;
        }
    }
}

