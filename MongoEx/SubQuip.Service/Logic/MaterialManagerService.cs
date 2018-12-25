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
using SubQuip.ViewModel.Roles;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.Equipment;
using SubQuip.ViewModel.Material;
using SubQuip.ViewModel.PartProperties;
using SubQuip.ViewModel.TechSpecs;
using SubQuip.Common;
using SubQuip.Common.Importer;
using SubQuip.ViewModel.Request;

namespace SubQuip.Business.Logic
{
    public class MaterialManagerService : IMaterialManagerService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IMaterialRepository _materialRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IFileManagerService _fileManagerService;

        /// <summary>
        /// Initializes a new instance of the MaterialManagerService
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="materialRepository"></param>
        /// <param name="equipmentRepository"></param>
        /// <param name="fileRepository"></param>
        public MaterialManagerService(IPrincipal principal, IMaterialRepository materialRepository, IEquipmentRepository equipmentRepository, IFileRepository fileRepository, IFileManagerService fileManagerService, IRequestRepository requestRepository)
        {
            _principal = principal as ClaimsPrincipal;
            _materialRepository = materialRepository;
            _equipmentRepository = equipmentRepository;
            _fileRepository = fileRepository;
            _fileManagerService = fileManagerService;
            _requestRepository = requestRepository;
        }

        /// <summary>
        /// Get All Materials
        /// </summary>
        /// <returns></returns>
        public IResult GetAllMaterials(SearchSortModel search)
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
                List<MaterialViewModel> materialViewModels = new List<MaterialViewModel>();
                var allMaterial = _materialRepository.GetAllMaterials(search);
                if (allMaterial != null && allMaterial.Any())
                {
                    materialViewModels = allMaterial.Select(t =>
                    {
                        var materialViewModel = new MaterialViewModel();
                        materialViewModel.MapFromModel(t);
                        if (t.TechnicalSpecifications == null || !t.TechnicalSpecifications.Any())
                            return materialViewModel;
                        var techSpecs = new List<TechSpecsViewModel>();
                        materialViewModel.TechnicalSpecifications = techSpecs.MapFromModel<TechnicalSpecification, TechSpecsViewModel>(t.TechnicalSpecifications);
                        return materialViewModel;

                    }).ToList();
                    search.SearchResult = materialViewModels;
                }
                else
                {
                    search.SearchResult = materialViewModels;
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
        /// Get a Material
        /// </summary>
        /// <param name = "id" > material id</param>
        /// <returns></returns>
        public IResult GetMaterialById(string id)
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
                    MaterialViewModel materialViewModel = null;
                    var material = _materialRepository.GetOne(t => t.MaterialId == ObjectId.Parse(id));
                    if (material != null)
                    {
                        materialViewModel = new MaterialViewModel();
                        materialViewModel.MapFromModel(material);
                        if (material.PartProperties != null && material.PartProperties.Any())
                        {
                            var overviews = new List<PartPropertyViewModel>();
                            materialViewModel.PartProperties = overviews.MapFromModel<PartProperty, PartPropertyViewModel>(material.PartProperties);
                        }
                        if (material.Documents != null && material.Documents.Any())
                        {
                            materialViewModel.Documents = _fileManagerService.GetFiles(material.Documents);
                        }
                        if (material.TechnicalSpecifications != null && material.TechnicalSpecifications.Any())
                        {
                            var techSpecs = new List<TechSpecsViewModel>();
                            materialViewModel.TechnicalSpecifications = techSpecs.MapFromModel<TechnicalSpecification, TechSpecsViewModel>(material.TechnicalSpecifications);
                        }
                        result.Body = materialViewModel;
                    }
                    else
                    {
                        result.Message = MaterialNotification.MaterialNotFound;
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
        /// Insert Material
        /// </summary>
        /// <param name="materialViewModel"></param>
        /// <returns></returns>
        public IResult InsertMaterial(MaterialViewModel materialViewModel)
        {
            materialViewModel.MaterialId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var material = new Material();
                material.MapFromViewModel(materialViewModel, (ClaimsIdentity)_principal.Identity);
                material.PartProperties = new List<PartProperty>();
                material.Documents = new List<ObjectId>();
                material.TechnicalSpecifications = new List<TechnicalSpecification>();
                _materialRepository.InsertOne(material);
                result.Body = material;
                result.Message = MaterialNotification.Created;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Save Material Document
        /// </summary>
        /// <param name = "document" ></ param >
        /// <param name="fileList"></param>
        /// < returns ></ returns >
        public IResult SaveMaterialDocument(List<FileDetails> fileList, DocumentViewModel document)
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
                    fileList.ForEach(t =>
                    {
                        var file = new File();
                        file.MapFromViewModel(t, (ClaimsIdentity)_principal.Identity);
                        _fileRepository.InsertOne(file);
                        documentIds.Add(file.FileId);

                        var fileDetail = new FileDetails();
                        fileDetail.MapFromModel(file);
                        fileDetail.Content = string.Empty;
                        fileDetails.Add(fileDetail);
                    });
                }

                var updateDefinition = Builders<Material>.Update.Set(t => t.ModifiedDate, GenericHelper.CurrentDate).AddToSetEach(t => t.Documents, documentIds);
                _materialRepository.UpdateOne(t => t.MaterialId.Equals(ObjectId.Parse(document.RegardingId)), updateDefinition);
                result.Operation = Operation.Create;
                result.Message = MaterialNotification.DocumentCreated;
                result.Body = fileDetails;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        // <summary>
        /// Update Material
        /// </summary>
        /// <param name="materialViewModel"></param>
        /// <returns></returns>
        public IResult UpdateMaterial(MaterialViewModel materialViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var updateDefinition = Builders<Material>.Update
                    .Set(x => x.Materialnumber, materialViewModel.Materialnumber)
                    .Set(x => x.Description, materialViewModel.Description)
                    .Set(x => x.ManufactorPartNumber, materialViewModel.ManufactorPartNumber)
                    .Set(x => x.ManufactorName, materialViewModel.ManufactorName)
                    .Set(x => x.Type, materialViewModel.Type)
                    .Set(x => x.ModifiedDate, GenericHelper.CurrentDate);

                _materialRepository.UpdateOne(t => t.MaterialId.Equals(ObjectId.Parse(materialViewModel.MaterialId)), updateDefinition);
                result.Message = MaterialNotification.Updated;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Delete a Material
        /// </summary>
        /// <param name="id">material id</param>
        /// <returns></returns>
        public IResult DeleteMaterial(string id)
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _materialRepository.DeleteOne(t => t.MaterialId == ObjectId.Parse(id));
                result.Message = MaterialNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Insert Update the material overview
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public IResult ManageMaterialPartProperties(PartPropertyViewModel viewModel)
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
                    var updateDefinition = Builders<Material>.Update.Set(x => x.ModifiedDate, GenericHelper.CurrentDate).AddToSet(t => t.PartProperties, partProperty);
                    _materialRepository.UpdateOne(t => t.MaterialId.Equals(ObjectId.Parse(viewModel.RegardingId)), updateDefinition);
                    result.Body = partProperty;
                    result.Message = MaterialNotification.OverviewCreated;
                    result.Operation = Operation.Create;
                }
                else
                {
                    var updateDefinition = Builders<Material>.Update.Set(x => x.ModifiedDate, GenericHelper.CurrentDate).Set(t => t.PartProperties[-1].PropertyName, viewModel.PropertyName).
                        Set(t => t.PartProperties[-1].PropertyValue, viewModel.PropertyValue);
                    _materialRepository.UpdateOne(
                        t => t.MaterialId.Equals(ObjectId.Parse(viewModel.RegardingId)) &&
                             t.PartProperties.Any(o => o.PartPropertyId.Equals(ObjectId.Parse(viewModel.PartPropertyId))),
                        updateDefinition);
                    result.Body = viewModel;
                    result.Message = MaterialNotification.OverviewUpdated;
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
        ///  Get Equipments For Material
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IResult GetEquipmentsForMaterial(string id)
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var equipmentViewModels = new List<EquipmentViewModel>();
                var equipments = _equipmentRepository.Filter(t => t.Material == ObjectId.Parse(id)).ToList();
                if (equipments.Any())
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
                }
                result.Body = equipmentViewModels;
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
        public IResult ManageMaterialTechnicalSpecs(TechSpecsViewModel viewModel)
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
                    var updateDefinition = Builders<Material>.Update.Set(x => x.ModifiedDate, GenericHelper.CurrentDate).AddToSet(t => t.TechnicalSpecifications, technicalSpecification);
                    _materialRepository.UpdateOne(t => t.MaterialId.Equals(ObjectId.Parse(viewModel.RegardingId)), updateDefinition);
                    result.Body = technicalSpecification;
                    result.Message = MaterialNotification.TechSpecCreated;
                    result.Operation = Operation.Create;
                }
                else
                {
                    var updateDefinition = Builders<Material>.Update.Set(x => x.ModifiedDate, GenericHelper.CurrentDate).Set(t => t.TechnicalSpecifications[-1].TechSpecName, viewModel.TechSpecName).
                                        Set(t => t.TechnicalSpecifications[-1].Value, viewModel.Value).Set(t => t.TechnicalSpecifications[-1].IncludeInOverview, viewModel.IncludeInOverview);

                    _materialRepository.UpdateOne(
                        t => t.MaterialId.Equals(ObjectId.Parse(viewModel.RegardingId)) &&
                             t.TechnicalSpecifications.Any(o => o.TechSpecId.Equals(ObjectId.Parse(viewModel.TechSpecId))),
                        updateDefinition);
                    result.Body = viewModel;
                    result.Message = MaterialNotification.TechSpecUpdated;
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
        /// Insert Material Request
        /// </summary>
        /// <param name="materialRequestViewModel"></param>
        /// <returns></returns>
        public IResult InsertMaterialRequest(List<FileDetails> fileList, MaterialRequestViewModel materialRequestViewModel)
        {
            materialRequestViewModel.RequestId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var materialRequest = new Request();
                materialRequest.MapFromViewModel(materialRequestViewModel, (ClaimsIdentity)_principal.Identity);

                materialRequest.MailUsers = new List<MailUserDetails>();
                if (materialRequestViewModel.MailUsers != null && materialRequestViewModel.MailUsers.Any())
                {
                    materialRequest.MailUsers = materialRequestViewModel.MailUsers.Select(user =>
                    {
                        var mailUser = new MailUserDetails();
                        mailUser.MapFromViewModel(user);
                        return mailUser;
                    }).ToList();
                }

                if (!string.IsNullOrEmpty(materialRequestViewModel.RegardingId))
                {
                    materialRequest.RegardingId = ObjectId.Parse(materialRequestViewModel.RegardingId);
                }
                _requestRepository.InsertOne(materialRequest);

                if (fileList != null && fileList.Any())
                {
                    materialRequest.Files = fileList.Select(fileViewModel =>
                    {
                        var file = new File();
                        file.MapFromViewModel(fileViewModel, (ClaimsIdentity)_principal.Identity);
                        _fileRepository.InsertOne(file);
                        return file.FileId;
                    }).ToList();
                }

                var updateDefinition = Builders<Request>.Update.Set(t => t.Files, materialRequest.Files);
                _requestRepository.UpdateOne(t => t.RequestId.Equals(materialRequest.RequestId), updateDefinition);

                if (materialRequest.Files != null && materialRequest.Files.Any())
                {
                    materialRequestViewModel.Files = new List<string>();
                    materialRequest.Files.ForEach(x => materialRequestViewModel.Files.Add(x.ToString()));
                }
                result.Body = materialRequestViewModel.MapFromModel(materialRequest);
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
        /// Delete All Material.
        /// </summary>
        /// <returns></returns>
        public IResult DeleteAllMaterial()
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _materialRepository.DeleteMany();
                result.Message = MaterialNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Total (the total number of materials in the database)
        /// </summary>
        /// <returns></returns>
        public int CountNumMaterials()
        {
            var count = _materialRepository.Query.Count(t => true);
            return count;
        }


        /// <summary>
        /// the number of materials with at least one element in the property Documents
        /// </summary>
        /// <returns></returns>
        public int CountNumMaterialsWithDocument()
        {
            var count = _materialRepository.Query.Count(t => t.Documents.Any());
            return count;
        }

        /// <summary>
        /// the number of materials that have been created during past 7 days by createdDate
        /// </summary>
        /// <returns></returns>
        public int CountNumMaterialsAddedLastWeek()
        {
            return (int)_materialRepository.GetRecentAddedMaterials();
        }

        /// <summary>
        /// Get List of Materials for corresponding identifiers.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<MaterialViewModel> GetListofMaterials(List<ObjectId> ids)
        {
            if (ids != null && ids.Any())
            {
                var requiredFields = new List<FieldDefinition<Material>>
                {
                    "MaterialId", "Materialnumber", "Description", "ManufactorPartNumber", "ManufactorName", "Type", "IsSparePart", "IsActive", "CreatedDate", "ModifiedDate"
                };

                return ids.Select(x =>
                {
                    var materialView = new MaterialViewModel();
                    var material = _materialRepository.GetMultipleChildDocument(t => t.MaterialId == x, requiredFields, null);
                    materialView.MapFromModel(material);
                    return materialView;
                }).ToList();
            }
            return null;
        }

        /// <summary>
        /// Get list of material number that are already present in db
        /// </summary>
        /// <param name="materialNums"></param>
        /// <returns></returns>
        public List<string> GetAlreadyAddedMaterialNumber(List<string> materialNums)
        {
            var materialNumDb = new List<string>();
            var data = _materialRepository.Query.Where(t => materialNums.Contains(t.Materialnumber)).ToList();
            if (data.Any())
            {
                materialNumDb = data.Select(t => t.Materialnumber).ToList();
            }
            return materialNumDb;
        }

        /// <summary>
        /// Import material data
        /// </summary>
        /// <param name="dataToImport"></param>
        /// <returns></returns>
        public IResult ImportMaterials(ImportedData dataToImport)
        {
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                // 1. Map the incoming data to models
                var mappedMaterialModels = MapImportedMaterialToModel(dataToImport);

                // 2. Get the list of new material number(material) as whole.
                var newMaterialNums = new List<string>();
                if (mappedMaterialModels.Any())
                    newMaterialNums = mappedMaterialModels.Select(t => t.Materialnumber).ToList();

                // 3. Search the above new material numbers in db and find out the already exist in db.
                var materialNumsDb = new List<string>();
                if (newMaterialNums.Any())
                    materialNumsDb = GetAlreadyAddedMaterialNumber(newMaterialNums);

                // 4. Prepare seperate list for items to insert and update collection
                List<Material> materialsForUpdate;
                List<Material> materialsForInsert;
                if (materialNumsDb != null && materialNumsDb.Any())
                {
                    materialsForInsert = mappedMaterialModels.Where(t => !materialNumsDb.Contains(t.Materialnumber))
                        .ToList();
                    materialsForUpdate = mappedMaterialModels.Where(t => materialNumsDb.Contains(t.Materialnumber))
                        .ToList();
                }
                else
                {
                    materialsForInsert = mappedMaterialModels;
                    materialsForUpdate = new List<Material>();
                }

                // 5. Insert the material in bulk to db except the already exist
                if (materialsForInsert.Any())
                    _materialRepository.InsertMany(materialsForInsert);

                // 6. Update the Already exist materials
                if (materialsForUpdate.Any())
                {
                    materialsForUpdate.ForEach(m =>
                    {
                        var updateDefinition = Builders<Material>.Update.
                        Set(x => x.Description, m.Description).
                        Set(x => x.ManufactorPartNumber, m.ManufactorPartNumber).
                        Set(x => x.ManufactorName, m.ManufactorName).
                        Set(x => x.Owner, m.Owner).
                        Set(x => x.License, m.License).
                        Set(x => x.Type, m.Type).
                        Set(x => x.GrossWeight, m.GrossWeight).
                        Set(x => x.SizeDimension, m.SizeDimension).
                        Set(x => x.Quantity, m.Quantity).
                        Set(x => x.ModifiedDate, GenericHelper.CurrentDate);
                        _materialRepository.UpdateOne(t => t.Materialnumber.Equals(m.Materialnumber), updateDefinition);
                    });
                }

                // 7. Map the result with viewmodel
                var importResult = new MaterialImportResult
                {
                    InsertedCount = materialsForInsert.Count,
                    UpdatedCount = materialsForUpdate.Count
                };

                result.Body = importResult;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        #region Private Method

        /// <summary>
        /// Map imported row key value pair to material model
        /// </summary>
        /// <param name="dataToImport"></param>
        /// <returns></returns>
        private List<Material> MapImportedMaterialToModel(ImportedData dataToImport)
        {
            var emailClaim = ((ClaimsIdentity)_principal.Identity).GetActiveUserId();
            var materials = new List<Material>();
            if (dataToImport.RowData != null && dataToImport.RowData.Any())
            {
                materials = dataToImport.RowData.Select(t =>
                {
                    var materialModel = new Material
                    {
                        Materialnumber = t["Material"],
                        Description = t["Description"],
                        ManufactorPartNumber = t["Manufacturer Part no"],
                        ManufactorName = t["Manufacturer Name"],
                        Owner = t["Eier"],
                        License = t["Lisens"],
                        Type = t["Type"],
                        GrossWeight = t["Gross Weight"],
                        SizeDimension = t["Size/dimens."],
                        Quantity = t["Antall på lager pr dato"],
                        IsSparePart = false,
                        PartProperties = new List<PartProperty>(),
                        Documents = new List<ObjectId>(),
                        TechnicalSpecifications = new List<TechnicalSpecification>(),
                        IsActive = true,
                        CreatedBy = emailClaim,
                        CreatedDate = GenericHelper.CurrentDate,
                        ModifiedBy = emailClaim,
                        ModifiedDate = GenericHelper.CurrentDate
                    };
                    return materialModel;
                }).ToList();
            }
            return materials;
        }

        #endregion
    }
}
