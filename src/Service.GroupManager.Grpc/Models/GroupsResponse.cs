using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.GroupManager.Domain.Models;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class GroupsResponse
{
    [DataMember(Order = 1)]
    public List<Group> Groups { get; set; }
}