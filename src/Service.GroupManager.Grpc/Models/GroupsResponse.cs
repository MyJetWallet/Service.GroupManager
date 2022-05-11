using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.GroupManager.Domain.Models;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class GroupsResponse
{
    [DataMember(Order = 1)]
    public List<GroupResponse> Groups { get; set; }

    
}

[DataContract]
public class GroupResponse
{
    [DataMember(Order = 1)]
    public Group Group { get; set; }
    [DataMember(Order = 2)]
    public int UserCount { get; set; }
}

