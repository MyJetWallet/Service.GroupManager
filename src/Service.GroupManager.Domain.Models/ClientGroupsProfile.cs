using System.Runtime.Serialization;

namespace Service.GroupManager.Domain.Models
{
    [DataContract]
    public class ClientGroupsProfile
    {
        [DataMember(Order = 1)] public string ClientId { get; set; }
        [DataMember(Order = 2)] public string ClientEmail { get; set; }
        [DataMember(Order = 3)] public string GroupId { get; set; }
    }
}