using MongoDB.Bson;
using SubQuip.Common;
using SubQuip.Common.CommonData;
using SubQuip.ViewModel.Equipment;
using SubQuip.ViewModel.PartProperties;
using SubQuip.ViewModel.Request;
using SubQuip.ViewModel.Statistics;
using SubQuip.ViewModel.TechSpecs;
using System.Collections.Generic;

namespace SubQuip.Business.Interfaces
{
    public interface IEquipmentManagerService
    {
        /// <summary>
        /// Get All Equipment Info
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetAllEquipments(SearchSortModel search);

        /// <summary>
        /// Get a single Equipment
        /// </summary>
        /// <param name="id">equipment id</param>
        /// <returns></returns>
        IResult GetEquipmentById(string id);

        /// <summary>
        /// Insert Equipment
        /// </summary>
        /// <param name="equipmentViewModel"></param>
        /// <returns></returns>
        IResult InsertEquipment(EquipmentViewModel equipmentViewModel);

        /// <summary>
        /// Save Equipment Document
        /// </summary>
        /// <param name="documents"></param>
        /// <returns></returns>
        IResult SaveEquipmentDocument(List<FileDetails> fileList, DocumentViewModel document);

        /// <summary>
        /// Update Equipment
        /// </summary>
        /// <param name="equipmentViewModel"></param>
        /// <returns></returns>
        IResult UpdateEquipment(EquipmentViewModel equipmentViewModel);

        /// <summary>
        /// Insert Update the equipment overview
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IResult ManageEquipmentPartProperties(PartPropertyViewModel viewModel);

        /// <summary>
        /// Insert update the technical specifications
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        IResult ManageEquipmentTechnicalSpecs(TechSpecsViewModel viewModel);

        /// <summary>
        /// Delete a single Equipment
        /// </summary>
        /// <param name="id">equipment id</param>
        /// <returns></returns>
        IResult DeleteEquipment(string id);

        /// <summary>
        /// Delete All Equipments.
        /// </summary>
        /// <returns></returns>
        IResult DeleteAllEquipment();

        /// <summary>
        /// Total (the total number of equipments in the database)
        /// </summary>
        /// <returns></returns>
        int CountNumEquipments();

        /// <summary>
        /// the number of equipments that has the Owner fiel as the logged in user or her company
        /// </summary>
        /// <returns></returns>
        List<PartnerStatistics> CountNumEquipmentsPerPartner();

        /// <summary>
        /// the number of equipments with at least one element in the property Documents
        /// </summary>
        /// <returns></returns>
        int CountNumEquipmentsWithDocument();

        /// <summary>
        /// the number of equipments that have been created the previous 7 days by cretaedDate
        /// </summary>
        /// <returns></returns>
        int CountNumEquipmentsAddedLastWeek();

        /// <summary>
        /// Insert Equipment Request
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="equipmentRequestViewModel"></param>
        /// <returns></returns>
        IResult InsertEquipmentRequest(List<FileDetails> fileList, EquipmentRequestViewModel equipmentRequestViewModel);

        /// <summary>
        /// Get List of Equipments for corresponding identifiers.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        List<EquipmentViewModel> GetListofEquipments(List<ObjectId> ids);
    }

}

