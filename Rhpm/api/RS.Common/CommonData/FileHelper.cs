using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RS.Common.CommonData
{
    public static class FileHelper
    {
        public static void SaveFile(IFormFile file,string documentName,string[] allowedExtensions, IHostingEnvironment hostingEnvironment, string folder)
        {
            string path = Path.Combine(hostingEnvironment.ContentRootPath, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var extension = Path.GetExtension(file.FileName);
            var results = Array.Find(allowedExtensions, s => s.Equals(extension));
            if (!string.IsNullOrEmpty(results))
            {
                var filePath = Path.Combine(path, documentName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
        }

        public static string GetExtension(IFormFile file, string[] allowedExtensions)
        {
            string result = null;
            if (file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                var results = Array.Find(allowedExtensions, s => s.Equals(extension));
                if (!string.IsNullOrEmpty(results))
                {
                    result = extension;
                }
            }
            return result;
        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".png", "image/png" }
            };
        }
    }
}

