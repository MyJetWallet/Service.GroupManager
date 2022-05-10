using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.GroupManager.Domain.Models;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class ProfileWithGroupResponse
{
    [DataMember(Order = 1)] public ClientGroupsProfile Profile { get; set; }
    [DataMember(Order = 2)] public Group Group { get; set; }
}