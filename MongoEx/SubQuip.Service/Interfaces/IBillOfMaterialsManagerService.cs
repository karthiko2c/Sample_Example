using System.Collections.Generic;
using SubQuip.Common;
using SubQuip.Common.CommonData;
using SubQuip.ViewModel.BillOfMaterial;
using SubQuip.ViewModel.Request;

namespace SubQuip.Business.Interfaces
{
    public interface IBillOfMaterialsManagerService
    {
        /// <summary>
        /// Get Specific Bill Of Material.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult GetBillOfMaterialById(string id);

        /// <summary>
        /// Get Bill Of Matrials For Particular User.
        /// </summary>
        /// <param name="searchSortModel"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        IResult GetBillOfMaterialsForUser(SearchSortModel searchSortModel, string userid);

        /// <summary>
        /// Get list of bom's viewmodel for logged user
        /// </summary>
        /// <param name="search"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<BillOfMaterialViewModel> GetSearchedBom(SearchSortModel search, string userid);

        /// <summary>
        /// Insert Bill Of Material.
        /// </summary>
        /// <param name="billOfMaterial"></param>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        IResult CreateBillOfMaterial(BillOfMaterialViewModel billOfMaterial, FileDetails fileModel);

        /// <summary>
        /// Update Bill Of Material.
        /// </summary>
        /// <param name="billOfMaterial"></param>
        /// <param name="fileModel"></param>
        /// <returns></returns>
        IResult UpdateBillOfMaterial(BillOfMaterialViewModel billOfMaterial, FileDetails fileModel);

        /// <summary>
        /// Delete Bill Of Material.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult DeleteBillOfMaterial(string id);

        /// <summary>

        /// Total Number Of BOM.
        /// </summary>
        /// <returns></returns>
		int CountNumBillOfMaterial();

        /// <summary>
        /// Total Number Of BOM InProgress.
        /// </summary>
        /// <returns></returns>
		int CountBOMNumInProgress();

        /// <summary>
        /// Total Number Of BOM Completed.
        /// </summary>
        /// <returns></returns>
		int CountBOMNumCompleted();

        /// <summary>
        /// Delete All Bill Of Material.
        /// </summary>
        /// <returns></returns>
        IResult DeleteAllBillOfMaterial();

        /// <summary>
        /// Add comment to BOM 
        /// </summary>
        IResult SaveCommentForBOM(BomCommentViewModel bomCommentViewModel);

        /// <summary>
        /// Returns comments for specified BOM
        /// </summary>
        /// <param name="bomId">Bom id for bom</param>
        /// <returns></returns>
        IResult GetCommentsForBom(string bomId);

        /// <summary>
        ///  Insert BOM Request
        /// </summary>
        /// <returns></returns>
        IResult InsertBOMRequest(List<FileDetails> fileList, BomRequestViewModel bomRequestViewModel);

        /// <summary>
        /// Add Equipment/Material To BOM.
        /// </summary>
        /// <param name="addItemToBom"></param>
        /// <returns></returns>
        IResult AddItemToBom(AddItemToBom addItemToBom);

        /// <summary>
        /// Add Option To BOM.
        /// </summary>
        /// <param name="bomOptionViewModel"></param>
        /// <returns></returns>
        IResult SaveOptionForBOM(BomOptionViewModel bomOptionViewModel);

        /// <summary>
        /// Get Added Items of BOM.
        /// </summary>
        /// <param name="bomId"></param>
        /// <returns></returns>
        IResult GetAddedItemsOfBom(string bomId);

        /// <summary>
        /// Get Boms of Specific Template
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        IResult GetBomsForTemplate(string templateId);

        /// <summary>
        /// Get all Bom Templates.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetBomTemplates(SearchSortModel search);

    }
}
