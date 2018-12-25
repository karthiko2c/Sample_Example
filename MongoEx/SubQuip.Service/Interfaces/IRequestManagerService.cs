using SubQuip.Common;
using SubQuip.Common.CommonData;
using SubQuip.ViewModel.Request;
using System.Collections.Generic;

namespace SubQuip.Business.Interfaces
{
    public interface IRequestManagerService
    {

        /// <summary>
        /// Get All Request Info
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        IResult GetRequests(SearchSortModel search);

        /// <summary>
        /// Get a single Request
        /// </summary>
        /// <param name="id">Request id</param>
        /// <returns></returns>
        IResult GetRequestById(string id);

        /// <summary>
        /// Insert Request
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        IResult InsertRequest(List<FileDetails> fileList, RequestViewModel requestViewModel);

        /// <summary>
        /// Update Request
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        IResult UpdateRequest(List<FileDetails> fileList, RequestViewModel requestViewModel);

        /// <summary>
        /// Delete a single Request
        /// </summary>
        /// <param name="id">Request id</param>
        /// <returns></returns>
        IResult DeleteRequest(string id);

        /// <summary>
        /// Delete All Requests.
        /// </summary>
        /// <returns></returns>
        IResult DeleteAllRequests();


    }
}
