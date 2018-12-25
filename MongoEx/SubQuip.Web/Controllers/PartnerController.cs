using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubQuip.Common.CommonData;
using SubQuip.Common.Enums;
using SubQuip.ViewModel.Partner;
using Microsoft.AspNetCore.Authorization;
using SubQuip.Business.Interfaces;
using SubQuip.ViewModel.User;

namespace SubQuip.WebApi.Controllers
{
    /// <summary>
    /// Partner controller, to handle the different partners in SubQuip
    /// </summary>
    [Produces("application/json")]
    [Route("api/Partner/[Action]")]
    [ValidateModel]
    [Authorize]
    public class PartnerController : Controller
    {
        private readonly IPartnerManagerService _partnerManager;
        
        /// <summary>
        /// Initializes a new instance of the PartnerController
        /// </summary>
        /// <param name="partnerManager"></param>
        public PartnerController(IPartnerManagerService partnerManager)
        {
            _partnerManager = partnerManager;
        }

        /// <summary>
        /// Get a list of all partners registered in SubQuip.
        /// </summary>
        /// <returns>List of partners</returns>
        [HttpGet]
        public IResult Partners(SearchSortModel search)
        {
            var partners = _partnerManager.GetAllPartners(search);
            return partners;
        }

        /// <summary>
        /// Get a specific partner by its identifier.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Partner</returns>
        [HttpGet]
        public IResult Details(string id)
        {
            var partner = _partnerManager.GetPartnerById(id);
            return partner;
        }

        /// <summary>
        /// Create a Partner.
        /// </summary>
        /// <param name="partnerView"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult Create([FromBody]PartnerViewModel partnerView)
        {
            var partner = _partnerManager.Create(partnerView);
            return partner;
        }

        /// <summary>
        /// Update a Partner.
        /// </summary>
        /// <param name="partnerView"></param>
        /// <returns></returns>
        [HttpPost]
        public IResult Update([FromBody]PartnerViewModel partnerView)
        {
            var partner = _partnerManager.Update(partnerView);
            return partner;
        }

        /// <summary>
        /// Delete a Partner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IResult Delete(string id)
        {
            var partner = _partnerManager.Delete(id);
            return partner;
        }

        /// <summary>
        /// Delete All Partners.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpDelete]
        public IResult DeleteAll([FromBody]UserLoginViewModel loginModel)
        {
            if (loginModel.UserName.Equals("test") && loginModel.UserPassword.Equals("test"))
            {
                return _partnerManager.DeleteAllPartners();
            }
            return null;
        }
    }
}