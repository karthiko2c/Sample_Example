using SubQuip.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.CommonData;
using SubQuip.ViewModel.Partner;
using SubQuip.Common.Enums;
using SubQuip.Entity.Models;
using SubQuip.Common.Extensions;
using System.Security.Principal;
using System.Security.Claims;
using SubQuip.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace SubQuip.Business.Logic
{
    public class PartnerManagerService : IPartnerManagerService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly IPartnerRepository _partnerRepository;

        /// <summary>
        ///  Initializes a new instance of the PartnerManagerService
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="partnerRepository"></param>
        public PartnerManagerService(IPrincipal principal, IPartnerRepository partnerRepository)
        {
            _principal = principal as ClaimsPrincipal;
            _partnerRepository = partnerRepository;
        }

        /// <summary>
        /// Get All Partners.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetAllPartners(SearchSortModel search)
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
                var partnerViewModels = new List<PartnerViewModel>();
                var allPartner = _partnerRepository.GetAllPartners(search);
                if(allPartner != null && allPartner.Any())
                {                   
                    search.SearchResult = partnerViewModels.MapFromModel<Partner, PartnerViewModel>(allPartner);                    
                }
                else
                {
                    search.SearchResult = partnerViewModels;
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
        /// Get Specific Partner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IResult GetPartnerById(string id)
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
                    var partner = _partnerRepository.GetOne(t => t.PartnerId == ObjectId.Parse(id));
                    if (partner != null)
                    {
                        var partnerViewModel = new PartnerViewModel();
                        result.Body = partnerViewModel.MapFromModel(partner);
                    }
                    else
                    {
                        result.Message = PartnerNotification.PartnerNotFound;
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
        /// Insert Partner
        /// </summary>
        /// <param name="partnerViewModel"></param>
        /// <returns></returns>
        public IResult Create(PartnerViewModel partnerViewModel)
        {
            partnerViewModel.PartnerId = null;
            var result = new Result
            {
                Operation = Operation.Create,
                Status = Status.Success
            };
            try
            {
                var partner = new Partner();
                partner.MapFromViewModel(partnerViewModel, (ClaimsIdentity)_principal.Identity);
                _partnerRepository.InsertOne(partner);
                result.Body = partnerViewModel.MapFromModel(partner);
                result.Message = PartnerNotification.Created;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Update Partner
        /// </summary>
        /// <param name="partnerViewModel"></param>
        /// <returns></returns>
        public IResult Update(PartnerViewModel partnerViewModel)
        {
            var result = new Result
            {
                Operation = Operation.Update,
                Status = Status.Success
            };
            try
            {
                var updateDefinition = Builders<Partner>.Update
                    .Set(x => x.Name, partnerViewModel.Name)
                    .Set(x => x.Email, partnerViewModel.Email)
                    .Set(x => x.ModifiedDate, GenericHelper.CurrentDate);

                _partnerRepository.UpdateOne(t => t.PartnerId == ObjectId.Parse(partnerViewModel.PartnerId), updateDefinition);
                result.Message = PartnerNotification.Updated;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Delete a Partner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IResult Delete(string id)
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _partnerRepository.DeleteOne(t => t.PartnerId == ObjectId.Parse(id));
                result.Message = PartnerNotification.Deleted;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = Status.Fail;
            }
            return result;
        }

        /// <summary>
        /// Delete All Partners.
        /// </summary>
        /// <returns></returns>
        public IResult DeleteAllPartners()
        {
            var result = new Result
            {
                Operation = Operation.Delete,
                Status = Status.Success
            };
            try
            {
                _partnerRepository.DeleteMany();
                result.Message = PartnerNotification.Deleted;
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
