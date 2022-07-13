using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.GroupManager.Grpc.Models;

[DataContract]
public class ProfilesListResponse
{
    [DataMember(Order = 1)] public List<string> ConverterProfiles { get; set; }
    [DataMember(Order = 2)] public List<string> WithdrawalProfiles { get; set; }
    [DataMember(Order = 3)] public List<string> InterestProfiles { get; set; }
    [DataMember(Order = 4)] public List<string> DepositProfiles { get; set; }
    [DataMember(Order = 5)] public List<string> AssetSettingsProfiles { get; set; }
}