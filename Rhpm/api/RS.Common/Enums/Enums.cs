namespace RS.Common.Enums
{
    public enum Status { Fail = 1, Success = 2, Error = 3 }

    public enum OpeningSkillType
    {
        Primary = 1,
        Secondary = 2
    }

    public enum Operation
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4
    }

    public enum Approval
    {
        Opening = 1,
        Candidate = 2
    }

    public enum ShowType
    {
        All = 1,
        Gender = 2
    }

    public enum TemplateType
    {
        UserRegistration = 1,
        ScheduleUserForInterview = 2,
        InterviewCancelled = 3
    }
}
