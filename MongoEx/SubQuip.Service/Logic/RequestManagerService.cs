using System;
using System.Security.Claims;
using System.Security.Principal;
using MongoDB.Bson;
using SubQuip.Business.Interfaces;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models;
using SubQuip.ViewModel.Request;
using SubQuip.Common.Extensions;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using SubQuip.Common;

namespace SubQuip.Business.Logic
{
    public class RequestManagerService : IRequestManagerService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IRequestRepository _requestRepository;
        private readonly IFileRepository _fileRepository;

        /// <summary>
        /// Initializes a new instance of the RequestManagerService
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="requestRepository"></param>
        /// <param name="fileRepository"></param>
        public RequestManagerService(IPrincipal principal, IRequestRepository requestRepository, IFileRepository fileRepository)
        {
            _principal = principal as ClaimsPrincipal;
            _requestRepository = requestRepository;
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// Get All Request Info
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetRequests(SearchSortModel search)
        {
            if (string.IsNullOrEmpty(search.SortColumn))
            {
                search.SortColumn = "RequestedBy";
            }

            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var requestViewModels = new List<RequestViewModel>();
                var allRequest = _requestRepository.GetAllRequests(search);
                if (allRequest != null && allRequest.Any())
                {
                    requestViewModels = allRequest.Select(t =>
                    {
                        var requestViewModel = new RequestViewModel();
                        requestViewModel.MapFromModel(t);
                        if (t.Files != null && t.Files.Any())
                        {
                            requestViewModel.Files = new List<string>();
                            t.Files.ForEach(x => requestViewModel.Files.Add(x.ToString()));
                        }
                        if (t.MailUsers != null && t.MailUsers.Any())
                        {
                            var mailUsers = new List<MailUserDetails>();
                            requestViewModel.MailUsers = mailUsers.MapFromModel<MailUserDetails, MailUser>(t.MailUsers);
                        }
                        return requestViewModel;
                    }).ToList();
                    search.SearchResult = requestViewModels;                    
                }
                else
                {
                    search.SearchResult = requestViewModels;
                    result.Message = CommonErrorMessages.NoResultFound;
                }
                result.Body = search;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;

        }

        /// <summary>
        /// Get a single Request
        /// </summary>
        /// <param name="id">Request id</param>
        /// <returns></returns>
        public IResult GetRequestById(string id)
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
                    RequestViewModel requestViewModel = null;
                    var request = _requestRepository.GetOne(t => t.RequestId == ObjectId.Parse(id));
                    if (request != null)
                    {
                        requestViewModel = new RequestViewModel();
                        requestViewModel.MapFromModel(request);
                        if (request.Files != null && request.Files.Any())
                        {
                            requestViewModel.Files = new List<string>();
                            request.Files.ForEach(x => requestViewModel.Files.Add(x.ToString()));
                        }
                        if (request.MailUsers != null && request.MailUsers.Any())
                        {
                            var mailUsers = new List<MailUserDetails>();
                            requestViewModel.MailUsers = mailUsers.MapFromModel<MailUserDetails, MailUser>(request.MailUsers);
                        }
                        result.Body = requestViewModel;
                    }
                    else
                    {
                        result.Message = RequestNotification.RequestNotFound;
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
        /// Insert Request
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        public IResult InsertRequest(List<FileDetails> fileList, RequestViewModel requestViewModel)
        {
            requestViewModel.RequestId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var request = new Request();
                request.MailUsers = new List<MailUserDetails>();
                if (requestViewModel.MailUsers.Any())
                {
                    requestViewModel.MailUsers.ForEach(user =>
                    {
                        var mailUser = new MailUserDetails();
                        mailUser.MapFromViewModel(user);
                        request.MailUsers.Add(mailUser);
                    });
                }
                request.MapFromViewModel(requestViewModel);
                request.CreatedDate = GenericHelper.CurrentDate;
                request.ModifiedDate = GenericHelper.CurrentDate;
                request.IsActive = true;
                _requestRepository.InsertOne(request);
               
                if (fileList != null && fileList.Any())
                {
                    request.Files = new List<ObjectId>();
                    fileList.ForEach(fileViewModel =>
                        {
                            var file = new File();
                            file.MapFromViewModel(fileViewModel);
                            file.CreatedDate = GenericHelper.CurrentDate;
                            file.ModifiedDate = GenericHelper.CurrentDate;
                            file.IsActive = true;
                            _fileRepository.InsertOne(file);
                            request.Files.Add(file.FileId);
                        });
                }
                var updateDefinition = Builders<Request>.Update.Set(t => t.Files, request.Files);
                _requestRepository.UpdateOne(t => t.RequestId.Equals(request.RequestId), updateDefinition);
                if (request.Files != null && request.Files.Any())
                {
                    requestViewModel.Files = new List<string>();
                    request.Files.ForEach(x => requestViewModel.Files.Add(x.ToString()));
                }
                result.Body = requestViewModel.MapFromModel(request);
                result.Message = RequestNotification.Created;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Update Request
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        public IResult UpdateRequest(List<FileDetails> fileList, RequestViewModel requestViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var listFiles = new List<ObjectId>();
                if (fileList != null && fileList.Any())
                {
                    fileList.ForEach(fileViewModel =>
                    {
                        var file = new File();
                        file.MapFromViewModel(fileViewModel);
                        file.CreatedDate = GenericHelper.CurrentDate;
                        file.ModifiedDate = GenericHelper.CurrentDate;
                        file.IsActive = true;
                        _fileRepository.InsertOne(file);
                        listFiles.Add(file.FileId);
                    });
                }

                var mailUsers = new List<MailUserDetails>();
                if (requestViewModel.MailUsers.Any())
                {
                    requestViewModel.MailUsers.ForEach(user =>
                    {
                        var mailUser = new MailUserDetails();
                        mailUser.MapFromViewModel(user);
                        mailUsers.Add(mailUser);
                    });
                }

                var updateDefinition = Builders<Request>.Update
                    .Set(x => x.Type, requestViewModel.Type)
                    .Set(x => x.Description, requestViewModel.Description)
                    .Set(x => x.FromDate, requestViewModel.FromDate)
                    .Set(x => x.ToDate, requestViewModel.ToDate)
                    .Set(x => x.RequestedBy, requestViewModel.RequestedBy)
                    .Set(x => x.Company, requestViewModel.Company)
                    .Set(x => x.PhoneNumber, requestViewModel.PhoneNumber)
                    .AddToSetEach(x => x.MailUsers, mailUsers)
                    .AddToSetEach(x => x.Files, listFiles);


                _requestRepository.UpdateOne(t => t.RequestId.Equals(ObjectId.Parse(requestViewModel.RequestId)), updateDefinition);
                result.Message = RequestNotification.Updated;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Delete a single Request
        /// </summary>
        /// <param name="id">Request id</param>
        /// <returns></returns>
        public IResult DeleteRequest(string id)
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _requestRepository.DeleteOne(t => t.RequestId == ObjectId.Parse(id));
                result.Message = RequestNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Delete All Requets.
        /// </summary>
        /// <returns></returns>
        public IResult DeleteAllRequests()
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _requestRepository.DeleteMany();
                result.Message = RequestNotification.Deleted;
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
