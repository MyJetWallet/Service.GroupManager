using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class AssignUsersToGroupRequest
{
    [DataMember(Order = 1)]
    public string GroupId { get; set; }
    
    [DataMember(Order = 2)]
    public List<string> ClientIds { get; set; }
}