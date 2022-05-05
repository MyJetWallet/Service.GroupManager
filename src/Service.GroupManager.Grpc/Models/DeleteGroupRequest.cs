using System.Runtime.Serialization;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class DeleteGroupRequest
{
    [DataMember(Order = 1)] public string GroupId { get; set; }
}