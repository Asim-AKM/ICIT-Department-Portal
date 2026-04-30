using System.Text.Json.Serialization;

namespace Domain_Service.Enum
{

    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Blocked = 3,
        Suspended = 4
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleType
    {
        Admin = 1,
        Clerk = 2,
        Faculty = 3,
        Students = 4,
    }
    public enum StudentStatus
    {
        Unverified = 1,
        Verified = 2,
        Rejected = 3,
    }
    public enum SessionStatus
    {
        Active = 1, // When session Currently Running
        Inactive = 2, //Incase We Inactive current Session 
        Completed = 3, // when Session Completed
    }
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        Failed = 3
    }
    public enum ChallanStatus
    {
        Pending = 1,
        Paid = 2,
        Expired = 3
    }
    public enum ProposalStatus
    {
        Draft=1,
        Submitted=2,
        UnderReview=3,
        Approved=4,
        Rejected=5,
        RevisionRequested=6
    }
    public enum MilestoneStatus
    {
        Pending = 1,
        Submitted = 2,
        Approved = 3,
        Rejected = 4
    }
    public enum BatchStatus
    {
        Processing = 1,
        Completed = 2,
        Failed = 3
    }
    public enum EnrollmentStatus
    {
        Enrolled = 1,
        Dropped = 2,
        Completed = 3
    }


    public enum DepartmentStatus
    {
        Active = 1,
        Inactive = 2
    }

    public enum TeamMemberRole
    {
        Leader = 1,
        Member = 2
    }
}
