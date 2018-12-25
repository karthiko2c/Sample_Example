using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using MongoDB.Bson;
using MongoDB.Driver;
using SubQuip.Business.Interfaces;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;
using SubQuip.Common.Extensions;
using SubQuip.Data.Interfaces;
using SubQuip.Entity.Models;
using SubQuip.Entity.Models.BillOfMaterials;
using SubQuip.ViewModel.BillOfMaterial;
using SubQuip.ViewModel.Equipment;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using SubQuip.Common;
using SubQuip.ViewModel.Material;
using SubQuip.Entity.Models.Graph;
using SubQuip.ViewModel.Request;

namespace SubQuip.Business.Logic
{
    public class BillOfMaterialsManagerService : IBillOfMaterialsManagerService
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ClaimsPrincipal _principal;
        private readonly IBillOfMaterialRepository _bomRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IGraphRepository _graphRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IMaterialManagerService _materialManager;
        private readonly IEquipmentManagerService _equipmentManager;

        /// <summary>
        /// Initializes a new instance of the BillOfMaterialsManagerService
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="bomRepository"></param>
        /// <param name="configuration"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="fileRepository"></param>

        public BillOfMaterialsManagerService(IPrincipal principal, IBillOfMaterialRepository bomRepository, IConfiguration configuration, IHostingEnvironment hostingEnvironment, IFileRepository fileRepository, IGraphRepository graphRepository, IRequestRepository requestRepository, IMaterialManagerService materialManager, IEquipmentManagerService equipmentManager)
        {
            _principal = principal as ClaimsPrincipal;
            _bomRepository = bomRepository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _fileRepository = fileRepository;
            _graphRepository = graphRepository;
            _requestRepository = requestRepository;
            _materialManager = materialManager;
            _equipmentManager = equipmentManager;
        }

        /// <summary>
        /// Get Bill Of Material For Particular User.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IResult GetBillOfMaterialsForUser(SearchSortModel search, string userid)
        {
            if (string.IsNullOrEmpty(search.SortColumn))
            {
                search.SortColumn = Constants.CreatedDate;
            }
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                if (!string.IsNullOrEmpty(userid))
                {
                    var bomViewModels = GetSearchedBom(search, userid);
                    if (!bomViewModels.Any())
                    {
                        result.Message = CommonErrorMessages.NoResultFound;
                    }
                    search.SearchResult = bomViewModels;
                    result.Body = search;
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
        /// Get list of bom's viewmodel for logged user
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<BillOfMaterialViewModel> GetSearchedBom(SearchSortModel search, string userid)
        {
            var bomViewModels = new List<BillOfMaterialViewModel>();
            var allBoms = _bomRepository.GetAllBillOfMaterial(search, userid);
            if (allBoms.Any())
            {
                bomViewModels = allBoms.Select(t =>
                {
                    var bomViewModel = new BillOfMaterialViewModel();
                    bomViewModel.MapFromModel(t);
                    bomViewModel.TemplateId = t.TemplateId.ToString();
                    if (t.BomUser == null) return bomViewModel;
                    var bomUserViewModel = new BomUserViewModel();
                    bomViewModel.BomUser = (BomUserViewModel)bomUserViewModel.MapFromModel(t.BomUser);
                    return bomViewModel;
                }).ToList();
            }
            return bomViewModels;
        }

        /// <summary>
        /// Get Specific Bill Of Material.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IResult GetBillOfMaterialById(string id)
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
                    var bom = _bomRepository.GetOne(t => t.BomId == ObjectId.Parse(id));
                    if (bom != null)
                    {
                        var bomViewModel = MapBomToViewModel(bom);
                        if (bom.Image != null)
                        {
                            var file = _fileRepository.GetOne(t => t.FileId == bom.Image);
                            bomViewModel.ImageContent = file.Content;
                        }

                        result.Body = bomViewModel;
                    }
                    else
                    {
                        result.Message = BomNotification.BOMNotFound;
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
        /// Get Added Items of BOM.
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        public IResult GetAddedItemsOfBom(string bomId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                if (!string.IsNullOrEmpty(bomId))
                {
                    var bom = _bomRepository.GetChildDocument(x => x.BomId == ObjectId.Parse(bomId), "AddedItems", "BomId");
                    if (bom.AddedItems != null && bom.AddedItems.Any())
                    {
                        var item = new Item();
                        var materialIds = bom.AddedItems.Where(x => x.ItemType == BomItemType.Material).Select(x => x.RegardingId).ToList();
                        var equipmentIds = bom.AddedItems.Where(x => x.ItemType == BomItemType.Equipment).Select(x => x.RegardingId).ToList();

                        var materialList = _materialManager.GetListofMaterials(materialIds);
                        if (materialList != null && materialList.Any())
                        {
                            item.Materials = materialList;
                        }
                        var equipmentList = _equipmentManager.GetListofEquipments(equipmentIds);
                        if (equipmentList != null && equipmentList.Any())
                        {
                            item.Equipments = equipmentList;
                        }
                        result.Body = item;
                    }
                    else
                    {
                        result.Message = BomNotification.NoAddedItem;
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
        /// Get Boms of Specific Template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public IResult GetBomsForTemplate(string templateId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var query = (from bom in _bomRepository.Query.Where(x => x.Type == BomType.Bom)
                             join template in _bomRepository.Query on bom.TemplateId.Value equals template.BomId
                             join request in _requestRepository.Query on bom.BomId equals request.RegardingId.Value into multipleRequests                            
                             where bom.TemplateId == ObjectId.Parse(templateId)
                             select new
                             {
                                 bom.BomId,
                                 bom.Name,
                                 bom.BomUser,
                                 bomTemplateName = template.Name,
                                 multipleRequests
                             }).ToList();

                if (query != null && query.Any())
                {
                    result.Body = query.Select(t =>
                    {
                        var bom = new BomsForTemplateViewModel();
                        bom.BomId = t.BomId.ToString();
                        bom.BomName = t.Name;
                        bom.BomUser = MapBomUserToViewModel(t.BomUser);
                        bom.BomTemplateName = t.bomTemplateName;
                        if (t.multipleRequests != null && t.multipleRequests.Any())
                        {
                            var orderByCreatedDate = t.multipleRequests.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                            bom.FromDate = orderByCreatedDate.FromDate;
                            bom.ToDate = orderByCreatedDate.ToDate;
                        }
                        return bom;
                    }).ToList();
                }
                else
                {
                    result.Body = CommonErrorMessages.NoResultFound;
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
        /// Get All Bom Templates.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetBomTemplates(SearchSortModel search)
        {
            if (string.IsNullOrEmpty(search.SortColumn))
            {
                search.SortColumn = Constants.CreatedDate;
            }
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var query = (from bomTemplate in _bomRepository.Query.Where(x => x.Type == BomType.Template)
                             join bom in _bomRepository.Query on bomTemplate.BomId equals bom.TemplateId.Value into boms
                             select new
                             {
                                 bomTemplate,
                                 boms
                             }).OrderByDescending(t => t.bomTemplate.ModifiedDate).ToList();
                if (query != null && query.Any())
                {
                    result.Body = query.Select(x => new BomTemplateViewModel
                    {
                        BomTemplateId = x.bomTemplate.BomId.ToString(),
                        BomTemplateName = x.bomTemplate.Name,
                        Count = x.boms.Count()
                    }).ToList();
                }
                else
                {
                    result.Message = CommonErrorMessages.NoResultFound;
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
        /// Insert Bill Of Material.
        /// </summary>
        /// <param name="bomViewModel"></param>
        /// <param name="fileViewModel"></param>
        /// <returns></returns>
        public IResult CreateBillOfMaterial(BillOfMaterialViewModel bomViewModel, FileDetails fileViewModel)
        {
            bomViewModel.BomId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                if (bomViewModel.Type == BomType.Template)
                {
                    bomViewModel.TemplateId = null;
                }
                else
                {
                    if (string.IsNullOrEmpty(bomViewModel.TemplateId))
                    {
                        result.Message = BomNotification.TemplateNotFound;
                        return result;
                    }
                }

                var bom = MapBomToModel(bomViewModel);
                var emailClaim = ((ClaimsIdentity)_principal.Identity).GetActiveUserId();
                if (!string.IsNullOrEmpty(emailClaim))
                {
                    if (_configuration["AuthMethod"] == "ad")
                    {
                        var user = _graphRepository.RequestUserDetails(emailClaim).Result;
                        if (user != null)
                        {
                            bom.BomUser = new BomUser
                            {
                                FirstName = user.DisplayName,
                                LastName = user.Surname,
                                Mail = user.Mail,
                                Company = user.Company
                            };
                        }
                    }
                    else
                    {
                        bom.BomUser = new BomUser
                        {
                            FirstName = "Admin",
                            LastName = "User",
                            Mail = "admin@subquip.com",
                            Company = "SubquipAdmin"
                        };
                    }

                }
                bom.Status = BomStatus.InProgress;
                _bomRepository.InsertOne(bom);
                if (fileViewModel != null)
                {
                    var file = new File();
                    file.MapFromViewModel(fileViewModel, (ClaimsIdentity)_principal.Identity);
                    _fileRepository.InsertOne(file);
                    bom.Image = file.FileId;
                    var updateDefinition = Builders<BillOfMaterial>.Update.Set(x => x.Image, bom.Image);
                    _bomRepository.UpdateOne(t => t.BomId == bom.BomId, updateDefinition);
                }
                result.Body = MapBomToViewModel(bom);
                result.Message = BomNotification.Created;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Update Bill Of Material.
        /// </summary>
        /// <param name="bomViewModel"></param>
        /// <param name="fileViewModel"></param>
        /// <returns></returns>
        public IResult UpdateBillOfMaterial(BillOfMaterialViewModel bomViewModel, FileDetails fileViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                #region BOM Update
                var bom = MapBomToModel(bomViewModel);
                var updateDefinition = Builders<BillOfMaterial>.Update
                    .Set(x => x.Name, bom.Name)
                    .Set(x => x.Description, bom.Description)
                    .Set(x => x.ModifiedDate, bom.ModifiedDate)
                    .Set(x => x.Groups, bom.Groups)
                    .Set(x => x.Status, bom.Status);
                _bomRepository.UpdateOne(t => t.BomId == ObjectId.Parse(bomViewModel.BomId), updateDefinition);
                #endregion

                #region Update BOM Image
                if (fileViewModel != null)
                {
                    var file = new File();
                    file.MapFromViewModel(fileViewModel, (ClaimsIdentity)_principal.Identity);
                    if (bom.Image == null)
                    {
                        _fileRepository.InsertOne(file);
                        bom.Image = file.FileId;
                        var updateBOMDefinition = Builders<BillOfMaterial>.Update.Set(x => x.Image, bom.Image);
                        _bomRepository.UpdateOne(t => t.BomId == bom.BomId, updateBOMDefinition);
                    }
                    else
                    {
                        var updateImageDefinition = Builders<File>.Update
                            .Set(x => x.Name, file.Name)
                            .Set(x => x.Content, file.Content)
                            .Set(x => x.ContentType, file.ContentType)
                            .Set(x => x.ModifiedDate, file.ModifiedDate);
                        _fileRepository.UpdateOne(t => t.FileId == bom.Image, updateImageDefinition);
                    }

                }
                #endregion

                result.Body = MapBomToViewModel(bom);
                result.Message = BomNotification.Updated;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Delete Bill Of Material.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IResult DeleteBillOfMaterial(string id)
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _bomRepository.DeleteOne(t => t.BomId == ObjectId.Parse(id));
                result.Message = BomNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        public IResult DeleteAllBillOfMaterial()
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _bomRepository.DeleteMany();
                result.Message = BomNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Add Comment to BOM.
        /// </summary>
        /// <param name="bomCommentViewModel"></param>
        /// <returns></returns>
        public IResult SaveCommentForBOM(BomCommentViewModel bomCommentViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var bomComment = new BomComment();
                bomComment.MapFromViewModel(bomCommentViewModel, (ClaimsIdentity)_principal.Identity);
                var lstBomComments = new List<BomComment>();
                lstBomComments.Add(bomComment);
                var updateDefinition = Builders<BillOfMaterial>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).AddToSet(t => t.Comments, bomComment);
                _bomRepository.UpdateOne(t => t.BomId.Equals(ObjectId.Parse(bomCommentViewModel.RegardingId)), updateDefinition);
                result.Message = BomNotification.CommentCreated;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Add Option to BOM.
        /// </summary>
        /// <param name="bomOptionViewModel"></param>
        /// <returns></returns>
        public IResult SaveOptionForBOM(BomOptionViewModel bomOptionViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                BomOption bomOption = null;
                if (bomOptionViewModel != null)
                {
                    var bom = _bomRepository.GetOne(t => t.BomId == ObjectId.Parse(bomOptionViewModel.RegardingId));
                    if (bom != null)
                    {
                        bomOption = new BomOption();
                        if (!string.IsNullOrEmpty(bomOptionViewModel.License)) { bomOption.License = ObjectId.Parse(bomOptionViewModel.License); }
                        if (!string.IsNullOrEmpty(bomOptionViewModel.Owner)) { bomOption.Owner = ObjectId.Parse(bomOptionViewModel.Owner); }
                        if (!string.IsNullOrEmpty(bomOptionViewModel.Location)) { bomOption.Location = ObjectId.Parse(bomOptionViewModel.Location); }
                        var updateDefinition = Builders<BillOfMaterial>.Update.Set(t => t.Option, bomOption);
                        _bomRepository.UpdateOne(t => t.BomId.Equals(ObjectId.Parse(bomOptionViewModel.RegardingId)), updateDefinition);
                        result.Message = BomNotification.OptionSaved;
                    }
                    else
                    {
                        result.Message = BomNotification.BOMNotFound;
                    }
                }
                else
                {
                    result.Message = BomNotification.NoOptionProvided;
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
        /// Get Comments of Bill Of Material For Particular User.
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public IResult GetCommentsForBom(string bomId)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var bomComments = _bomRepository.GetCommentsForBom(bomId);

                if (bomComments != null && bomComments.Any())
                {
                    var bomCommentViewModels = bomComments.Select(t =>
                    {
                        var bomCommentViewModel = new BomCommentViewModel();
                        bomCommentViewModel.MapFromModel(t);
                        bomCommentViewModel.RegardingId = bomId;
                        return bomCommentViewModel;
                    }).ToList();

                    result.Body = bomCommentViewModels;
                }
                else
                {
                    result.Message = BomNotification.CommentsNotFound;
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
        /// Insert BOM Request
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        public IResult InsertBOMRequest(List<FileDetails> fileList, BomRequestViewModel bomRequestViewModel)
        {
            bomRequestViewModel.RequestId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var bomRequest = new Request();
                bomRequest.MapFromViewModel(bomRequestViewModel, (ClaimsIdentity)_principal.Identity);

                bomRequest.MailUsers = new List<MailUserDetails>();
                if (bomRequestViewModel.MailUsers != null && bomRequestViewModel.MailUsers.Any())
                {
                    bomRequest.MailUsers = bomRequestViewModel.MailUsers.Select(user =>
                    {
                        var mailUser = new MailUserDetails();
                        mailUser.MapFromViewModel(user);
                        return mailUser;
                    }).ToList();
                }

                if (bomRequestViewModel.BomRequestNodes != null && bomRequestViewModel.BomRequestNodes.Any())
                {
                    bomRequest.BomRequestNodes = bomRequestViewModel.BomRequestNodes.Select(t =>
                    {
                        var bomNode = new BomRequestNode();
                        bomNode.MapFromViewModel(t);
                        return bomNode;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(bomRequestViewModel.RegardingId))
                {
                    bomRequest.RegardingId = ObjectId.Parse(bomRequestViewModel.RegardingId);
                }
                _requestRepository.InsertOne(bomRequest);

                if (fileList != null && fileList.Any())
                {
                    bomRequest.Files = fileList.Select(fileViewModel =>
                    {
                        var file = new File();
                        file.MapFromViewModel(fileViewModel, (ClaimsIdentity)_principal.Identity);
                        _fileRepository.InsertOne(file);
                        return file.FileId;
                    }).ToList();
                }
                var updateDefinition = Builders<Request>.Update.Set(t => t.Files, bomRequest.Files);
                _requestRepository.UpdateOne(t => t.RequestId.Equals(bomRequest.RequestId), updateDefinition);
                if (bomRequest.Files != null && bomRequest.Files.Any())
                {
                    bomRequestViewModel.Files = new List<string>();
                    bomRequest.Files.ForEach(x => bomRequestViewModel.Files.Add(x.ToString()));
                }
                result.Body = bomRequestViewModel.MapFromModel(bomRequest);
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
        /// Add Equipment/Material To BOM.
        /// </summary>
        /// <param name="addItem"></param>
        /// <returns></returns>
        public IResult AddItemToBom(AddItemToBom addItem)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                AddToBom item = null;
                if (addItem != null)
                {
                    item = new AddToBom();
                    item.MapFromViewModel(addItem);
                    var bom = _bomRepository.GetOne(t => t.BomId == ObjectId.Parse(addItem.BomId));
                    if (bom != null)
                    {
                        var updateDefinition = Builders<BillOfMaterial>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).AddToSet(t => t.AddedItems, item);
                        _bomRepository.UpdateOne(t => t.BomId.Equals(ObjectId.Parse(addItem.BomId)), updateDefinition);
                        result.Message = BomNotification.ItemAdded;
                    }
                    else
                    {
                        result.Message = BomNotification.BOMNotFound;
                    }
                }
                else
                {
                    result.Message = BomNotification.NoItemProvided;
                }

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        #region Private methods for BOM model to view model

        /// <summary>
        /// Map bom model to view model
        /// </summary>
        /// <param name="bom"></param>
        /// <returns></returns>
        private BillOfMaterialViewModel MapBomToViewModel(BillOfMaterial bom)
        {
            var bomViewModel = new BillOfMaterialViewModel();
            bomViewModel.MapFromModel(bom);

            if (bom.TemplateId != null)
            {
                bomViewModel.TemplateId = bom.TemplateId.ToString();
            }

            if (bom.Image != null)
            {
                bomViewModel.Image = bom.Image.ToString();
            }

            bomViewModel.BomUser = MapBomUserToViewModel(bom.BomUser);

            if (bom.Groups != null && bom.Groups.Any())
            {
                bomViewModel.Groups = new List<BomGroupViewModel>();
                bom.Groups.ForEach(t =>
                {
                    var bomGroupViewModel = MapBomGroupToViewModel(t);
                    bomViewModel.Groups.Add(bomGroupViewModel);
                });
            }

            if (bom.Comments != null && bom.Comments.Any())
            {
                bomViewModel.Comments = bom.Comments.Select(t =>
                {
                    var bomCommentViewModel = new BomCommentViewModel();
                    bomCommentViewModel.MapFromModel(t);
                    return bomCommentViewModel;
                }).ToList();
            }

            if(bom.Option != null)
            {
                bomViewModel.Option = new BomOptionViewModel();
                if (bom.Option.License != null) { bomViewModel.Option.License = bom.Option.License.ToString(); }
                if (bom.Option.Owner != null) { bomViewModel.Option.Owner = bom.Option.Owner.ToString(); }
                if (bom.Option.Location != null) { bomViewModel.Option.Location = bom.Option.Location.ToString(); }
            }

            return bomViewModel;
        }

        private BomUserViewModel MapBomUserToViewModel(BomUser bomUser)
        {
            var bomUserViewModel = new BomUserViewModel();
            if (bomUser != null)
            {
                bomUserViewModel = (BomUserViewModel)bomUserViewModel.MapFromModel(bomUser);
            }
            return bomUserViewModel;
        }


        /// <summary>
        /// Map group model to view model
        /// </summary>
        /// <param name="bomGroup"></param>
        /// <returns></returns>
        private BomGroupViewModel MapBomGroupToViewModel(BomGroup bomGroup)
        {
            var bomGroupViewModel = new BomGroupViewModel { Name = bomGroup.Name };
            if (bomGroup.BillOfMaterials != null && bomGroup.BillOfMaterials.Any())
            {
                var bomViewModel = new List<BillOfMaterialViewModel>();
                foreach (var groupBillOfMaterial in bomGroup.BillOfMaterials)
                {
                    bomViewModel.Add(MapBomToViewModel(groupBillOfMaterial));
                }
                bomGroupViewModel.BillOfMaterials = bomViewModel;
            }

            if (bomGroup.BomItems != null && bomGroup.BomItems.Any())
            {
                var bomItemsViewModel = new List<BomItemViewModel>();
                foreach (var bomGroupBomItem in bomGroup.BomItems)
                {
                    bomItemsViewModel.Add(MapBomItemToViewModel(bomGroupBomItem));
                }
                bomGroupViewModel.BomItems = bomItemsViewModel;
            }
            return bomGroupViewModel;
        }

        /// <summary>
        /// Map item model to view model
        /// </summary>
        /// <param name="bomItem"></param>
        /// <returns></returns>
        private BomItemViewModel MapBomItemToViewModel(BomItem bomItem)
        {
            var bomItemViewModel = new BomItemViewModel { ItemType = bomItem.ItemType };
            if (bomItem.ItemType == BomItemType.Equipment)
            {
                var equipmentViewModel = new EquipmentViewModel();
                equipmentViewModel.MapFromModel(bomItem.Item as Equipment);
                bomItemViewModel.Item = equipmentViewModel;
            }
            else
            {
                var materialViewModel = new MaterialViewModel();
                materialViewModel.MapFromModel(bomItem.Item as Material);
                bomItemViewModel.Item = materialViewModel;
            }
            return bomItemViewModel;
        }

        #endregion

        #region Private methods for BOM ViewModel to Model


        /// <summary>
        /// Map View Model to Model
        /// </summary>
        /// <param name="bomViewModel"></param>
        /// <returns></returns>
        private BillOfMaterial MapBomToModel(BillOfMaterialViewModel bomViewModel)
        {
            var bom = new BillOfMaterial();
            bom.MapFromViewModel(bomViewModel, (ClaimsIdentity)_principal.Identity);

            if (bomViewModel.TemplateId != null)
            {
                bom.TemplateId = ObjectId.Parse(bomViewModel.TemplateId);
            }

            if (!string.IsNullOrEmpty(bomViewModel.Image))
            {
                bom.Image = ObjectId.Parse(bomViewModel.Image);
            }
            bom.AddedItems = new List<AddToBom>();
            bom.Comments = new List<BomComment>();
            if (bomViewModel.Groups != null && bomViewModel.Groups.Any())
            {
                bom.Groups = new List<BomGroup>();
                bomViewModel.Groups.ForEach(t =>
                {
                    var bomGroup = MapBomGroupToModel(t);
                    bom.Groups.Add(bomGroup);
                });
            }
            return bom;
        }

        /// <summary>
        /// Map Bom Group View Model to Model.
        /// </summary>
        /// <param name="bomGroupViewModel"></param>
        /// <returns></returns>
        private BomGroup MapBomGroupToModel(BomGroupViewModel bomGroupViewModel)
        {
            var bomGroup = new BomGroup { Name = bomGroupViewModel.Name };
            if (bomGroupViewModel.BillOfMaterials != null && bomGroupViewModel.BillOfMaterials.Any())
            {
                var bom = new List<BillOfMaterial>();
                foreach (var groupBillOfMaterial in bomGroupViewModel.BillOfMaterials)
                {
                    bom.Add(MapBomToModel(groupBillOfMaterial));
                }
                bomGroup.BillOfMaterials = bom;
            }

            if (bomGroupViewModel.BomItems != null && bomGroupViewModel.BomItems.Any())
            {
                var bomItems = new List<BomItem>();
                foreach (var bomGroupBomItem in bomGroupViewModel.BomItems)
                {
                    bomItems.Add(MapBomItemToModel(bomGroupBomItem));
                }
                bomGroup.BomItems = bomItems;
            }
            return bomGroup;
        }


        /// <summary>
        /// Map Bom Item View Model To Model
        /// </summary>
        /// <param name="bomItemViewModel"></param>
        /// <returns></returns>
        private BomItem MapBomItemToModel(BomItemViewModel bomItemViewModel)
        {
            var bomItem = new BomItem { ItemType = bomItemViewModel.ItemType };
            if (bomItemViewModel.ItemType == BomItemType.Equipment)
            {
                var equipmentViewModel = JsonConvert.DeserializeObject<EquipmentViewModel>(bomItemViewModel.Item.ToString());
                var equipment = new Equipment();
                bomItem.Item = equipment.MapFromViewModel(equipmentViewModel as EquipmentViewModel) as BillOfMaterialsItem;
            }
            else
            {
                var materialViewModel = JsonConvert.DeserializeObject<MaterialViewModel>(bomItemViewModel.Item.ToString());
                var material = new Material();
                bomItem.Item = material.MapFromViewModel(materialViewModel as MaterialViewModel) as BillOfMaterialsItem;
            }
            return bomItem;
        }

        public int CountNumBillOfMaterial()
        {
            return _bomRepository.Count(bom => true);
        }

        public int CountBOMNumInProgress()
        {
            return _bomRepository.Count(bom => bom.Status == BomStatus.InProgress);
        }

        public int CountBOMNumCompleted()
        {
            return _bomRepository.Count(bom => bom.Status == BomStatus.Completed);
        }
        #endregion
    }
}
