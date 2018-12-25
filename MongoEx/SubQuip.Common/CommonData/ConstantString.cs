namespace SubQuip.Common.CommonData
{
    public static class Constants
    {
        /// <summary>
        /// Added By Column
        /// </summary>
        public const string CreatedBy = "CreatedBy";

        /// <summary>
        /// Added Date Column
        /// </summary>
        public const string CreatedDate = "CreatedDate";

        /// <summary>
        /// The modified by column
        /// </summary>
        public static string ModifiedBy = "ModifiedBy";

        /// <summary>
        /// The modified date column
        /// </summary>
        public static string ModifiedDate = "ModifiedDate";

        /// <summary>
        /// Is Deleted Column
        /// </summary>
        public const string IsDeletedColumn = "IsDeleted";

        /// <summary>
        /// Is Active Column
        /// </summary>
        public const string IsActiveColumn = "IsActive";

    }

    public static class RoleNotification
    {
        public const string Created = "Role Created";
        public const string Updated = "Role Updated";
        public const string Deleted = "Role Deleted";
        public const string Duplicate = "Role with the same name Already Exists!.";
    }

    public static class FileNotification
    {
        public const string FileNotFound = "No Files Found";
    }

    public static class EquipmentNotification
    {
        public const string Created = "Equipment Created";
        public const string Updated = "Equipment Updated";
        public const string Deleted = "Equipment Deleted";
        public const string OverviewCreated = "Equipment overview Created";
        public const string OverviewUpdated = "Equipment overview Updated";
        public const string OverviewDeleted = "Equipment overview Deleted";
        public const string DocumentCreated = "Equipment document Created";
        public const string DocumentUpdated = "Equipment document Updated";
        public const string DocumentDeleted = "Equipment document Deleted";
        public const string TechSpecCreated = "Equipment technical specification Created";
        public const string TechSpecUpdated = "Equipment technical specification Updated";
        public const string TechSpecDeleted = "Equipment technical specification Deleted";
        public const string EquipmentNotFound = "No Equipment found";
        public const string MaterialNotProvided = "Material Identifier Not Provided";
    }

    public static class MaterialNotification
    {
        public const string Created = "Material Created";
        public const string Updated = "Material Updated";
        public const string Deleted = "Material Deleted";
        public const string OverviewCreated = "Material overview Created";
        public const string OverviewUpdated = "Material overview Updated";
        public const string OverviewDeleted = "Material overview Deleted";
        public const string DocumentCreated = "Material document Created";
        public const string DocumentUpdated = "Material document Updated";
        public const string DocumentDeleted = "Material document Deleted";
        public const string TechSpecCreated = "Material technical specification Created";
        public const string TechSpecUpdated = "Material technical specification Updated";
        public const string TechSpecDeleted = "Material technical specification Deleted";
        public const string MaterialNotFound = "No Material found";
    }

    public static class RequestNotification
    {
        public const string Created = "Request Created";
        public const string Updated = "Request Updated";
        public const string Deleted = "Request Deleted";
        public const string RequestNotFound = "No Request found";
    }

    public static class BomNotification
    {
        public const string Created = "Bom Created";
        public const string Updated = "Bom Updated";
        public const string Deleted = "Bom Deleted";
        public const string BOMNotFound = "No Bill Of Material found";
        public const string CommentCreated = "Bom Comment Created";
        public const string OptionSaved = "Bom Option Saved";
        public const string CommentsNotFound = "Bom comments not found";
        public const string ItemAdded = "Bom Item Added";
        public const string NoItemProvided = "Item Not Provide To Add";
        public const string NoAddedItem = "Item Not Found";
        public const string TemplateNotFound = "Template Not Found";
        public const string NoOptionProvided = "Option Not Provided";
    }

    public static class PartnerNotification
    {
        public const string Created = "Partner Created";
        public const string Updated = "Partner Updated";
        public const string Deleted = "Partner Deleted";
        public const string PartnerNotFound = "No Partner Found";
    }

    public static class UserNotification
    {
        public const string TabDetailsSaved = "Tab Details Saved";
        public const string MailNotFound = "Mail Not Found in Token";
        public const string NoTabDetails = "Tab Details Not Provided";
    }

    public static class CommonErrorMessages
    {
        public const string UnknownError = "Sorry, we have encountered an error.";
        public const string BadRequest = "Invalid Request";
        public const string NoIdentifierProvided = "Provided Identifier is Null";
        public const string NoResultFound = "No Result Found";
    }
}
