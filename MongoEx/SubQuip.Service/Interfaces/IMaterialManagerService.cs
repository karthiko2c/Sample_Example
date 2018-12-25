using SubQuip.Common.CommonData;
using SubQuip.ViewModel.PartProperties;
using System.Collections.Generic;
using SubQuip.ViewModel.Material;
using SubQuip.ViewModel.TechSpecs;
using SubQuip.Common;
using SubQuip.ViewModel.Request;
using MongoDB.Bson;
using SubQuip.Common.Importer;
using SubQuip.Entity.Models;

namespace SubQuip.Business.Interfaces
{
    public interface IMaterialManagerService
    {
        /// <summary>
        /// Get All Materials
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetAllMaterials(SearchSortModel search);

        /// <summary>
        /// Get a Material
        /// </summary>
        /// <param name="id">material id</param>
        /// <returns></returns>
        IResult GetMaterialById(string id);

        /// <summary>
        /// Get Equipments For Material
        /// </summary>
        /// <param name="id">material id</param>
        /// <returns></returns>
        IResult GetEquipmentsForMaterial(string id);

        /// <summary>
        /// Insert Material
        /// </summary>
        /// <param name="materialViewModel"></param>
        /// <returns></returns>
        IResult InsertMaterial(MaterialViewModel materialViewModel);

        /// <summary>
        /// Save Material Document
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        IResult SaveMaterialDocument(List<FileDetails> fileList, DocumentViewModel document);

        /// <summary>
        ///  Update Material
        /// </summary>
        /// <param name="materialViewModel"></param>
        /// <returns></returns>
        IResult UpdateMaterial(MaterialViewModel materialViewModel);

        /// <summary>
        /// Delete a Material
        /// </summary>
        /// <param name="id">material id</param>
        /// <returns></returns>
        IResult DeleteMaterial(string id);

        /// <summary>
        /// Delete All Materials.
        /// </summary>
        /// <returns></returns>
        IResult DeleteAllMaterial();

        /// <summary>
        /// Insert Update the material overview
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IResult ManageMaterialPartProperties(PartPropertyViewModel viewModel);

        /// <summary>
        /// Insert update the technical specifications
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IResult ManageMaterialTechnicalSpecs(TechSpecsViewModel viewModel);

        /// <summary>
        /// Total (the total number of materials in the database)
        /// </summary>
        /// <returns></returns>
        int CountNumMaterials();

        /// <summary>
        /// the number of materials with at least one element in the property Documents
        /// </summary>
        /// <returns></returns>
        int CountNumMaterialsWithDocument();

        /// <summary>
        /// the number of equipments that have been created during past 7 days by cretaedDate
        /// </summary>
        /// <returns></returns>
        int CountNumMaterialsAddedLastWeek();

        /// <summary>
        /// Insert Material Request
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="materialRequestViewModel"></param>
        /// <returns></returns>
        IResult InsertMaterialRequest(List<FileDetails> fileList, MaterialRequestViewModel materialRequestViewModel);

        /// <summary>
        /// Get List of Materials for corresponding identifiers.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<MaterialViewModel> GetListofMaterials(List<ObjectId> ids);

        /// <summary>
        /// Get list of material number that are already present in db
        /// </summary>
        /// <param name="materialNums"></param>
        /// <returns></returns>
        List<string> GetAlreadyAddedMaterialNumber(List<string> materialNums);

        /// <summary>
        /// Import material data
        /// </summary>
        /// <param name="dataToImport"></param>
        /// <returns></returns>
        IResult ImportMaterials(ImportedData dataToImport);


    }
}
