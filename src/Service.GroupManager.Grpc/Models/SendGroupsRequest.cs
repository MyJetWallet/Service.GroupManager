using System.Collections.Generic;

namespace Service.GroupManager.Grpc.Models;

public class SendGroupsRequest
{
    public List<string> Groups { get; set; }
}