using SubQuip.Common.CommonData;
using SubQuip.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubQuip.Business.Interfaces
{
    public interface ISearchManagerService
    {

        /// <summary>
        /// Global search
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        IResult Search(int skip = 0, int take = 50, SearchOptions options = null);

    }
}
