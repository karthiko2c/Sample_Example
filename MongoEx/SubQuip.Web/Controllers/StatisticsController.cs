using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubQuip.Business.Interfaces;
using SubQuip.Common.CommonData;
using SubQuip.ViewModel.Statistics;
using SubQuip.Common.Enums;
using SubQuip.Data.Interfaces;
using System.Security.Claims;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Statistics controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/statistics")]
    [ValidateModel]
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly IBillOfMaterialsManagerService _billOfMaterialsManagerService;
        private readonly IEquipmentManagerService _equipmentManagerService;
        private readonly IMaterialManagerService _materialManagerService;

        /// <summary>
        /// Stat controller
        /// </summary>
        /// <param name="billOfMaterialsManagerService"></param>
        /// <param name="equipmentManagerService"></param>
        public StatisticsController(IBillOfMaterialsManagerService billOfMaterialsManagerService, IEquipmentManagerService equipmentManagerService, IMaterialManagerService materialManagerService)
        {
            _billOfMaterialsManagerService = billOfMaterialsManagerService;
            _equipmentManagerService = equipmentManagerService;
            _materialManagerService = materialManagerService;

        }

        /// <summary>
        /// Get Statistics 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResult GetStats()
        {
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var latestSearchModel = new SearchSortModel
                {
                    SortColumn = Constants.CreatedDate,
                    SortDirection = SortDirection.Desc,
                    Page = 1,
                    PageSize = 5
                };

                result.Body = new Statistics
                {
                    NumBoms = _billOfMaterialsManagerService.CountNumBillOfMaterial(),
                    NumBomsCompleted = _billOfMaterialsManagerService.CountBOMNumCompleted(),
                    NumBomsInProgress = _billOfMaterialsManagerService.CountBOMNumInProgress(),

                    NumEquipments = _equipmentManagerService.CountNumEquipments(),
                    NumEquipmentForPartner = _equipmentManagerService.CountNumEquipmentsPerPartner(),
                    NumEquipmentsWithDocumentation = _equipmentManagerService.CountNumEquipmentsWithDocument(),
                    NumRecentEquipments = _equipmentManagerService.CountNumEquipmentsAddedLastWeek(),

                    NumMaterials = _materialManagerService.CountNumMaterials(),
                    NumMaterialsWithDocumentation = _materialManagerService.CountNumMaterialsWithDocument(),
                    NumRecentMaterials = _materialManagerService.CountNumMaterialsAddedLastWeek(),

                    LastFiveRequests = _billOfMaterialsManagerService.GetSearchedBom(latestSearchModel, string.Empty)

                };

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
