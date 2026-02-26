using System.Text.Json.Serialization;

namespace Domain_Service.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoleType
    {
        Admin = 1,
        Clerk = 2,
        Faculty = 3,
        Students = 4,
    }

}
