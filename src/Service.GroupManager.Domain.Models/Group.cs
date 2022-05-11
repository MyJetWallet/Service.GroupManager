using System.Runtime.Serialization;

namespace Service.GroupManager.Domain.Models
{
    [DataContract]
    public class Group
    {
        [DataMember(Order = 1)] public string GroupId { get; set; }
        [DataMember(Order = 2)] public string ConverterProfileId { get; set; }
        [DataMember(Order = 3)] public string WithdrawalProfileId { get; set; }
        [DataMember(Order = 4)] public string InterestRateProfileId { get; set; }
        [DataMember(Order = 5)] public int Weight { get; set; }
    }
}