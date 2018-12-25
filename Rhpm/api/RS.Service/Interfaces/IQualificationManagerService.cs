using RS.Common.CommonData;
using RS.ViewModel.Qualification;
using RS.ViewModel.SearchAndSortModel;

namespace RS.Service.Interfaces
{
    public interface IQualificationManagerService
    {
        /// <summary>
        /// Create a Qualification
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        IResult CreateQualification(QualificationViewModel qualification);

        /// <summary>
        /// Update a Qualification
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        IResult UpdateQualification(QualificationViewModel qualification);

        /// <summary>
        /// Delete a Qualification
        /// </summary>
        /// <param name="qualification"></param>
        /// <returns></returns>
        IResult DeleteQualification(QualificationViewModel qualification);

        /// <summary>
        /// Get All Qualifications
        /// </summary>
        /// <returns></returns>
        IResult GetAllQualification();

        /// <summary>
        /// Get Qualification By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResult GetQualificationById(int id);

        /// <summary>
        /// Get Qualification Results
        /// </summary>
        /// <param name="qualificationId"></param>
        /// <returns></returns>
        IResult GetQualificationsResults(SearchAndSortModel searchAndSortModel);
    }
}
