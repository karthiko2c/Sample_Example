using Microsoft.Extensions.Configuration;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MongoDB.Bson;

namespace SubQuip.Data.Logic
{
    public class FileRepository: Repository<File>, IFileRepository
    {
        public IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the FileRepository
        /// </summary>
        /// <param name="configuration"></param>
        public FileRepository(IConfiguration configuration) : base(configuration, "file")
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get files for material or equipment
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public List<File> GetFiles(List<ObjectId> fileIds)
        {
            return Query.Where(t => fileIds.Contains(t.FileId)).ToList();
        }
    }
}
