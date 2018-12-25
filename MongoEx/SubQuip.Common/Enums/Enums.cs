namespace SubQuip.Common.Enums
{
    public enum Status { Fail, Success }

    public enum Operation
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4
    }

    public enum SortDirection
    {
        Asc, Desc
    }

    public enum RequestFormType
    {
        Request,
        Share,
        BillOfMaterial,
        Material,
        Equipment
    }

    public enum MailTemplate
    {
        Request,
        ContactUs,
        BillOfMaterial,
        Material,
        Equipment
    }

    public enum BomItemType
    {
        Material, Equipment
    }

    public enum BomStatus
    {
        InProgress, Completed
    }

    public enum BomType
    {
        Template, Bom
    }
}
