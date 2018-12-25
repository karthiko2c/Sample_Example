using SubQuip.Common.CommonData;
using SubQuip.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Business.Interfaces
{
    public interface IUserManagerService
    {
        /// <summary>
        /// Save User and Tab Details.
        /// </summary>
        /// <param name="savedTabViewModel"></param>
        /// <returns></returns>
        IResult SaveTabDetail(SavedTabViewModel savedTabViewModel);

        /// <summary>
        /// Get all Tabs for User.
        /// </summary>
        /// <returns></returns>
        IResult GetTabsForUser();
    }
}
