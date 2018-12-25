using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubQuip.Common.CommonData;
using SubQuip.Common.Importer;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.Material;
using SubQuip.ViewModel.User;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// File controller for handeling file upload
    /// </summary>
    [Produces("application/json")]
    [Route("api/File/[Action]")]
    [ValidateModel]
    [Authorize]
    public class FileController : Controller
    {
        private readonly IFileManagerService _fileManager;
        private readonly IMaterialManagerService _materialManagerService;

        /// <summary>
        ///  Initializes a new instance of File controller
        /// </summary>
        /// <param name="fileManager"></param>
        /// <param name="materialManagerService"></param>
        public FileController(IFileManagerService fileManager, IMaterialManagerService materialManagerService)
        {
            _fileManager = fileManager;
            _materialManagerService = materialManagerService;
        }

        /// <summary>
        /// CSV file upload.
        /// </summary>
        /// <returns>The imported CSV data.</returns>
        /// <param name="uploadedFiles"></param>
        [HttpPost]
        public IResult UploadData(List<IFormFile> uploadedFiles)
        {
            List<ImportedData> importedFileResult = new List<ImportedData>();
            foreach (IFormFile uploadedFile in uploadedFiles)
            {
                switch (Path.GetExtension(uploadedFile.FileName))
                {
                    case "csv":
                        importedFileResult.Add(new CsvFileReader(',', Encoding.UTF8).ProcessFile(uploadedFile.OpenReadStream()));
                        break;
                    case "xlsx":
                        importedFileResult.Add(new ExcelFileReader().ProcessFile(uploadedFile.OpenReadStream()));
                        break;
                    case "xls":
                        importedFileResult.Add(new ExcelFileReader().ProcessFile(uploadedFile.OpenReadStream()));
                        break;
                }
            }
            return new Result { Body = importedFileResult };
        }

        /// <summary>
        /// Get File Details By Its Identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IResult Detail(string id)
        {
            var file = _fileManager.GetFileById(id);
            return file;
        }

        /// <summary>
        /// Delete All Files.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpDelete]
        public IResult DeleteAll([FromBody]UserLoginViewModel loginModel)
        {
            if (loginModel.UserName.Equals("test") && loginModel.UserPassword.Equals("test"))
            {
                return _fileManager.DeleteAllFiles();
            }
            return null;
        }

        /// <summary>
        /// Import material data
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult ImportMaterialData(IFormFile uploadFile)
        {
            var importedFileResult = new ImportedData();
            if (Path.GetExtension(uploadFile.FileName) == ".csv")
                importedFileResult = new CsvFileReader().ProcessFile(uploadFile.OpenReadStream());
            var result = _materialManagerService.ImportMaterials(importedFileResult);
            return result;
        }
    }
}