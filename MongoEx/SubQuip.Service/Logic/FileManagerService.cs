using SubQuip.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;
using SubQuip.Common;
using System.Security.Claims;
using System.Security.Principal;
using SubQuip.Data.Interfaces;
using MongoDB.Bson;
using SubQuip.Common.Extensions;
using System.Linq;
using SubQuip.Common.Importer;
using SubQuip.Entity.Models;
using SubQuip.ViewModel.Material;

namespace SubQuip.Business.Logic
{
    public class FileManagerService : IFileManagerService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IFileRepository _fileRepository;
     

        /// <summary>
        /// Initializes a new instance of the FileManagerService
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="fileRepository"></param>
        public FileManagerService(IPrincipal principal, IFileRepository fileRepository)
        {
            _principal = principal as ClaimsPrincipal;
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// Get Specific File Details.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IResult GetFileById(string id)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    FileDetails fileViewModel = null;
                    var file = _fileRepository.GetOne(t => t.FileId == ObjectId.Parse(id));
                    if (file != null)
                    {
                        fileViewModel = new FileDetails();
                        result.Body = fileViewModel.MapFromModel(file);
                    }
                    else
                    {
                        result.Message = FileNotification.FileNotFound;
                    }
                }
                else
                {
                    result.Status = Status.Fail;
                    result.Message = CommonErrorMessages.NoIdentifierProvided;
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Get All Files.
        /// </summary>
        /// <param name="searchSortModel"></param>
        /// <returns></returns>
        public IResult GetFiles(SearchSortModel searchSortModel)
        {
            throw new NotImplementedException();
        }

        public List<FileDetails> GetFiles(List<ObjectId> fileIds)
        {
            List<FileDetails> fileDetails = null;
            var files = _fileRepository.GetFiles(fileIds);
            if (files != null && files.Any())
            {
                fileDetails = new List<FileDetails>();
                fileDetails = files.Select(t =>
                {
                    var fileViewModel = new FileDetails
                    {
                        FileId = t.FileId.ToString(),
                        ContentType = t.ContentType,
                        CreatedDate = t.CreatedDate,
                        Description = t.Description,
                        IsActive = t.IsActive,
                        Name = t.Name,
                        Title = t.Title,
                        ModifiedDate = t.ModifiedDate
                    };
                    return fileViewModel;
                }).ToList();
            }
            return fileDetails;
        }

        /// <summary>
        /// Delete All Files.
        /// </summary>
        /// <returns></returns>
        public IResult DeleteAllFiles()
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _fileRepository.DeleteMany();
                result.Message = "Files Deleted";
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }
    }
}
