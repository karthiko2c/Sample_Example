using SubQuip.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace SubQuip.Data.Interfaces
{
    public interface IFileRepository: IRepository<File>
    {

        /// <summary>
        /// Get files for material or equipment
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        List<File> GetFiles(List<ObjectId> fileIds);
    }
}
