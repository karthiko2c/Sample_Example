using SubQuip.Common.CommonData;
using System.Collections.Generic;
using SubQuip.Entity.Models;
using MongoDB.Bson;
using SubQuip.Common.Importer;
using SubQuip.ViewModel.Material;

namespace SubQuip.Business.Interfaces
{
    public interface IFileManagerService
    {
        /// <summary>
        /// Get All Files.
        /// </summary>
        /// <param name="searchSortModel"></param>
        /// <returns></returns>
        IResult GetFiles(SearchSortModel searchSortModel);

        /// <summary>
        /// Get a Specific File.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult GetFileById(string id);

        /// <summary>
        /// Get files by its Identifiers.
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        List<FileDetails> GetFiles(List<ObjectId> fileIds);

        /// <summary>
        /// Delete All Files.
        /// </summary>
        /// <returns></returns>
        IResult DeleteAllFiles();
        
    }
}