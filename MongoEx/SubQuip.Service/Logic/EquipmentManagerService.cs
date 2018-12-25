using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;
using SubQuip.Common.Extensions;
using SubQuip.Data.Interfaces;
using SubQuip.Data.Logic;
using SubQuip.Entity.Models;
using SubQuip.Entity.Models.BillOfMaterials;
using SubQuip.ViewModel.Equipment;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.PartProperties;
using SubQuip.ViewModel.TechSpecs;
using SubQuip.Common;
using Microsoft.Extensions.Configuration;
using SubQuip.ViewModel.Statistics;
using SubQuip.ViewModel.Request;

namespace SubQuip.Business.Logic
{
    public class EquipmentManagerService : IEquipmentManagerService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IGraphRepository _graphRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IFileManagerService _fileManagerService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the EquipmentManagerService
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="equipmentRepository"></param>
        /// <param name="materialRepository"></param>
        /// <param name="fileRepository"></param>
        /// <param name="fileManagerService"></param>
        /// <param name="graphRepository"></param>
        /// <param name="configuration"></param>
        public EquipmentManagerService(IPrincipal principal, IEquipmentRepository equipmentRepository, IMaterialRepository materialRepository, IFileRepository fileRepository, IFileManagerService fileManagerService, IGraphRepository graphRepository, IConfiguration configuration, IPartnerRepository partnerRepository, IRequestRepository requestRepository)
        {
            _principal = principal as ClaimsPrincipal;
            _equipmentRepository = equipmentRepository;
            _materialRepository = materialRepository;
            _fileRepository = fileRepository;
            _fileManagerService = fileManagerService;
            _graphRepository = graphRepository;
            _configuration = configuration;
            _partnerRepository = partnerRepository;
            _requestRepository = requestRepository;
        }

        /// <summary>
        /// Get All Equipment Info
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetAllEquipments(SearchSortModel search)
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
                var equipmentViewModels = new List<EquipmentViewModel>();
                var equipments = _equipmentRepository.GetAllEquipments(search);
                if (equipments != null && equipments.Any())
                {
                    equipmentViewModels = equipments.Select(t =>
                    {
                        var equipmentViewModel = new EquipmentViewModel();
                        equipmentViewModel.MapFromModel(t);
                        if (t.TechnicalSpecifications == null || !t.TechnicalSpecifications.Any())
                            return equipmentViewModel;
                        var techSpecs = new List<TechSpecsViewModel>();
                        equipmentViewModel.TechnicalSpecifications = techSpecs.MapFromModel<TechnicalSpecification, TechSpecsViewModel>(t.TechnicalSpecifications);
                        return equipmentViewModel;
                    }).ToList();
                    search.SearchResult = equipmentViewModels;
                }
                else
                {
                    search.SearchResult = equipmentViewModels;
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
        /// Get a single Equipment
        /// </summary>
        /// <param name="id">equipment id</param>
        /// <returns></returns>
        public IResult GetEquipmentById(string id)
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
                    EquipmentViewModel equipmentViewModel = null;
                    var equipment = _equipmentRepository.GetOne(t => t.EquipmentId == ObjectId.Parse(id));
                    if (equipment != null)
                    {
                        equipmentViewModel = new EquipmentViewModel();
                        equipmentViewModel.MapFromModel(equipment);
                        if (equipment.PartProperties != null && equipment.PartProperties.Any())
                        {
                            var overviews = new List<PartPropertyViewModel>();
                            equipmentViewModel.PartProperties = overviews.MapFromModel<PartProperty, PartPropertyViewModel>(equipment.PartProperties);
                        }
                        if (equipment.Documents != null && equipment.Documents.Any())
                        {
                            equipmentViewModel.Documents = _fileManagerService.GetFiles(equipment.Documents);
                        }
                        if (equipment.TechnicalSpecifications != null && equipment.TechnicalSpecifications.Any())
                        {
                            var techSpecs = new List<TechSpecsViewModel>();
                            equipmentViewModel.TechnicalSpecifications = techSpecs.MapFromModel<TechnicalSpecification, TechSpecsViewModel>(equipment.TechnicalSpecifications);
                        }
                        result.Body = equipmentViewModel;
                    }
                    else
                    {
                        result.Message = EquipmentNotification.EquipmentNotFound;
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
        /// Insert Equipment
        /// </summary>
        /// <param name="equipmentViewModel"></param>
        /// <returns></returns>
        public IResult InsertEquipment(EquipmentViewModel equipmentViewModel)
        {
            equipmentViewModel.EquipmentId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                if (!string.IsNullOrEmpty(equipmentViewModel.Material))
                {
                    var equipment = new Equipment();
                    equipment.MapFromViewModel(equipmentViewModel, (ClaimsIdentity)_principal.Identity);
                    // here logged AD user email/company will be added
                    equipment.Owner = equipment.CreatedBy;
                    equipment.PartProperties = new List<PartProperty>();
                    equipment.Documents = new List<ObjectId>();
                    equipment.TechnicalSpecifications = new List<TechnicalSpecification>();
                    _equipmentRepository.InsertOne(equipment);

                    var resultView = new EquipmentViewModel();
                    resultView.MapFromModel(equipment);

                    result.Body = resultView;
                    result.Message = EquipmentNotification.Created;
                }
                else
                {
                    result.Status = Status.Fail;
                    result.Message = EquipmentNotification.MaterialNotProvided;
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
        /// Save Equipment Document
        /// </summary>
        /// <param name="documentList"></param>
        /// <returns></returns>
        public IResult SaveEquipmentDocument(List<FileDetails> fileList, DocumentViewModel document)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var documentIds = new List<ObjectId>();
                var fileDetails = new List<FileDetails>();
                if (fileList != null && fileList.Any())
                {
                    fileList.ForEach(fileViewModel =>
                    {
                        var file = new File();
                        file.MapFromViewModel(fileViewModel, (ClaimsIdentity)_principal.Identity);
                        _fileRepository.InsertOne(file);
                        documentIds.Add(file.FileId);

                        var fileDetail = new FileDetails();
                        fileDetail.MapFromModel(file);
                        fileDetail.Content = string.Empty;
                        fileDetails.Add(fileDetail);

                    });
                }

                var updateDefinition = Builders<Equipment>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).AddToSetEach(t => t.Documents, documentIds);
                _equipmentRepository.UpdateOne(t => t.EquipmentId.Equals(ObjectId.Parse(document.RegardingId)), updateDefinition);
                result.Operation = Operation.Create;
                result.Message = EquipmentNotification.DocumentCreated;
                result.Body = fileDetails;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Update Equipment
        /// </summary>
        /// <param name="equipmentViewModel"></param>
        /// <returns></returns>
        public IResult UpdateEquipment(EquipmentViewModel equipmentViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var updateDefinition = Builders<Equipment>.Update
                    .Set(x => x.EquipmentNumber, equipmentViewModel.EquipmentNumber)
                    .Set(x => x.SerialNumber, equipmentViewModel.SerialNumber)
                    .Set(x => x.License, equipmentViewModel.License)
                    .Set(x => x.Location, equipmentViewModel.Location)
                    .Set(x => x.ModifiedDate, GenericHelper.CurrentDate)
                    .Set(x => x.Material, ObjectId.Parse(equipmentViewModel.Material));

                _equipmentRepository.UpdateOne(t => t.EquipmentId == ObjectId.Parse(equipmentViewModel.EquipmentId), updateDefinition);
                result.Message = EquipmentNotification.Updated;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Insert Update the equipment overview
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IResult ManageEquipmentPartProperties(PartPropertyViewModel viewModel)
        {
            var result = new Result
            {
                Status = Status.Success
            };
            try
            {
                if (string.IsNullOrEmpty(viewModel.PartPropertyId) || viewModel.PartPropertyId == null)
                {
                    viewModel.PartPropertyId = null;
                    var partProperty = new PartProperty();
                    partProperty.MapFromViewModel(viewModel, (ClaimsIdentity)_principal.Identity);
                    partProperty.PartPropertyId = ObjectId.GenerateNewId();
                    var updateDefinition = Builders<Equipment>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).AddToSet(t => t.PartProperties, partProperty);
                    _equipmentRepository.UpdateOne(t => t.EquipmentId.Equals(ObjectId.Parse(viewModel.RegardingId)), updateDefinition);
                    result.Body = partProperty;
                    result.Message = EquipmentNotification.OverviewCreated;
                    result.Operation = Operation.Create;
                }
                else
                {
                    var updateDefinition = Builders<Equipment>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).Set(t => t.PartProperties[-1].PropertyName, viewModel.PropertyName).
                        Set(t => t.PartProperties[-1].PropertyValue, viewModel.PropertyValue);
                    _equipmentRepository.UpdateOne(
                        t => t.EquipmentId.Equals(ObjectId.Parse(viewModel.RegardingId)) &&
                             t.PartProperties.Any(o => o.PartPropertyId.Equals(ObjectId.Parse(viewModel.PartPropertyId))),
                        updateDefinition);
                    result.Body = viewModel;
                    result.Message = EquipmentNotification.OverviewUpdated;
                    result.Operation = Operation.Update;
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
        /// Insert update the technical specifications
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IResult ManageEquipmentTechnicalSpecs(TechSpecsViewModel viewModel)
        {
            var result = new Result
            {
                Status = Status.Success
            };
            try
            {
                if (string.IsNullOrEmpty(viewModel.TechSpecId) || viewModel.TechSpecId == null)
                {
                    viewModel.TechSpecId = null;
                    var technicalSpecification = new TechnicalSpecification();
                    technicalSpecification.MapFromViewModel(viewModel, (ClaimsIdentity)_principal.Identity);
                    technicalSpecification.TechSpecId = ObjectId.GenerateNewId();
                    var updateDefinition = Builders<Equipment>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).AddToSet(t => t.TechnicalSpecifications, technicalSpecification);
                    _equipmentRepository.UpdateOne(t => t.EquipmentId.Equals(ObjectId.Parse(viewModel.RegardingId)), updateDefinition);
                    result.Body = technicalSpecification;
                    result.Message = EquipmentNotification.TechSpecCreated;
                    result.Operation = Operation.Create;
                }
                else
                {
                    var updateDefinition = Builders<Equipment>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).Set(t => t.TechnicalSpecifications[-1].TechSpecName, viewModel.TechSpecName).
                                 Set(t => t.TechnicalSpecifications[-1].Value, viewModel.Value).Set(t => t.TechnicalSpecifications[-1].IncludeInOverview, viewModel.IncludeInOverview);

                    _equipmentRepository.UpdateOne(
                        t => t.EquipmentId.Equals(ObjectId.Parse(viewModel.RegardingId)) &&
                             t.TechnicalSpecifications.Any(o => o.TechSpecId.Equals(ObjectId.Parse(viewModel.TechSpecId))),
                        updateDefinition);
                    result.Body = viewModel;
                    result.Message = EquipmentNotification.TechSpecUpdated;
                    result.Operation = Operation.Update;
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
        /// Delete a single Equipment
        /// </summary>
        /// <param name="id">equipment id</param>
        /// <returns></returns>
        public IResult DeleteEquipment(string id)
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _equipmentRepository.DeleteOne(t => t.EquipmentId == ObjectId.Parse(id));
                result.Message = EquipmentNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Delete All Equipments.
        /// </summary>
        /// <returns></returns>
        public IResult DeleteAllEquipment()
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _equipmentRepository.DeleteMany();
                result.Message = EquipmentNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Insert Equipment Request
        /// </summary>
        /// <param name="equipmentRequestViewModel"></param>
        /// <returns></returns>
        public IResult InsertEquipmentRequest(List<FileDetails> fileList, EquipmentRequestViewModel equipmentRequestViewModel)
        {
            equipmentRequestViewModel.RequestId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var equipmentRequest = new Request();
                equipmentRequest.MapFromViewModel(equipmentRequestViewModel, (ClaimsIdentity)_principal.Identity);

                equipmentRequest.MailUsers = new List<MailUserDetails>();
                if (equipmentRequestViewModel.MailUsers != null && equipmentRequestViewModel.MailUsers.Any())
                {
                    equipmentRequest.MailUsers = equipmentRequestViewModel.MailUsers.Select(user =>
                    {
                        var mailUser = new MailUserDetails();
                        mailUser.MapFromViewModel(user);
                        return mailUser;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(equipmentRequestViewModel.RegardingId))
                {
                    equipmentRequest.RegardingId = ObjectId.Parse(equipmentRequestViewModel.RegardingId);
                }
                _requestRepository.InsertOne(equipmentRequest);

                if (fileList != null && fileList.Any())
                {
                    equipmentRequest.Files = fileList.Select(fileViewModel =>
                    {
                        var file = new File();
                        file.MapFromViewModel(fileViewModel, (ClaimsIdentity)_principal.Identity);
                        _fileRepository.InsertOne(file);
                        return file.FileId;
                    }).ToList();
                }

                var updateDefinition = Builders<Request>.Update.Set(t => t.Files, equipmentRequest.Files);
                _requestRepository.UpdateOne(t => t.RequestId.Equals(equipmentRequest.RequestId), updateDefinition);

                if (equipmentRequest.Files != null && equipmentRequest.Files.Any())
                {
                    equipmentRequestViewModel.Files = new List<string>();
                    equipmentRequest.Files.ForEach(x => equipmentRequestViewModel.Files.Add(x.ToString()));
                }
                result.Body = equipmentRequestViewModel.MapFromModel(equipmentRequest);
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
        /// Total (the total number of equipments in the database)
        /// </summary>
        /// <returns></returns>
        public int CountNumEquipments()
        {
            var count = _equipmentRepository.Query.Count(t => true);
            return count;
        }

        /// <summary>
        /// the number of equipments of each partner that has the Owner field as the logged in user or her company
        /// </summary>
        /// <returns></returns>
        public List<PartnerStatistics> CountNumEquipmentsPerPartner()
        {
            var partnerStatList = new List<PartnerStatistics>();
            var partners = _partnerRepository.Query.ToList();
            if (partners.Any())
            {
                partners.ForEach(t =>
                {
                    var partnerStat = new PartnerStatistics();
                    partnerStat.Name = t.Name;
                    partnerStat.Email = t.Email;
                    partnerStat.EquipmentCount = _equipmentRepository.Query.Count(x => (x.Owner.Contains(t.Name)));
                    partnerStatList.Add(partnerStat);
                });
            }
            return partnerStatList;
        }

        /// <summary>
        /// the number of equipments with at least one element in the property Documents
        /// </summary>
        /// <returns></returns>
        public int CountNumEquipmentsWithDocument()
        {
            var count = _equipmentRepository.Query.Count(t => t.Documents.Any());
            return count;
        }

        /// <summary>
        /// the number of equipments that have been created the previous 7 days by cretaedDate
        /// </summary>
        /// <returns></returns>
        public int CountNumEquipmentsAddedLastWeek()
        {
            return (int)_equipmentRepository.GetRecentAddedEquipment();
        }

        /// <summary>
        /// Get List of Equipments for corresponding identifiers.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<EquipmentViewModel> GetListofEquipments(List<ObjectId> ids)
        {
            if (ids != null && ids.Any())
            {
                var requiredFields = new List<FieldDefinition<Equipment>>
                {
                    "EquipmentId", "EquipmentNumber", "SerialNumber", "Owner", "License", "Location", "IsActive", "CreatedDate", "ModifiedDate", "Material"
                };

                return ids.Select(x =>
                {
                    var equipmentView = new EquipmentViewModel();
                    var equipment = _equipmentRepository.GetMultipleChildDocument(t => t.EquipmentId == x, requiredFields, null);
                    equipmentView.MapFromModel(equipment);
                    return equipmentView;
                }).ToList();
            }
            return null;
        }
    }
}
