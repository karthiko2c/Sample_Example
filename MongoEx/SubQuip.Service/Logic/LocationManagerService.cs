using SubQuip.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SubQuip.Common.CommonData;
using SubQuip.Data.Interfaces;
using SubQuip.Common.Enums;
using SubQuip.ViewModel.Location;
using System.Linq;
using SubQuip.Common.Extensions;
using SubQuip.Entity.Models;

namespace SubQuip.Business.Logic
{
    public class LocationManagerService : ILocationManagerService
    {
        private readonly ILocationRepository _locationRepository;

        /// <summary>
        /// Initializes a new instance of the LocationManagerService.
        /// </summary>
        /// <param name="locationRepository"></param>
        public LocationManagerService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        /// <summary>
        /// Get all Locations.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IResult GetAllLocations(SearchSortModel search)
        {
            if (string.IsNullOrEmpty(search.SortColumn))
            {
                search.SortColumn = "LocationName";
            }
            var result = new Result
            {
                Operation = Operation.Read,
                Status = Status.Success
            };
            try
            {
                var locationViewModels = new List<LocationViewModel>();
                var locations = _locationRepository.GetAllLocations(search);
                if (locations != null && locations.Any())
                {
                    search.SearchResult = locationViewModels.MapFromModel<Location, LocationViewModel>(locations);
                }
                else
                {
                    search.SearchResult = locationViewModels;
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
    }
}
