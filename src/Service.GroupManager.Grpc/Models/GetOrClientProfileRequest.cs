using System.Runtime.Serialization;

namespace Service.GroupManager.Grpc.Models
{
    [DataContract]
    public class GetOrClientProfileRequest
    {
        [DataMember(Order = 1)]
        public string ClientId { get; set; }
    }
}