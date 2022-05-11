using System.Runtime.Serialization;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class GetUsersRequest
{
    [DataMember(Order = 1)]
    public string GroupId { get; set; }
    [DataMember(Order = 2)] 
    public int Skip { get; set; }
    [DataMember(Order = 3)] 
    public int Take { get; set; }
    [DataMember(Order = 4)]
    public string SearchText { get; set; }
}