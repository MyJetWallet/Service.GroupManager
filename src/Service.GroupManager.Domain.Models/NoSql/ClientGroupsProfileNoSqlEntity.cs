
using MyNoSqlServer.Abstractions;

namespace Service.GroupManager.Domain.Models.NoSql
{
    public class ClientGroupsProfileNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-groupsprofile";

        public static string GeneratePartitionKey() => "GroupsProfiles";

        public static string GenerateRowKey(string clientId) => clientId;
        
        public ClientGroupsProfile ClientProfile { get; set; }

        public static ClientGroupsProfileNoSqlEntity Create(ClientGroupsProfile clientProfile) =>
            new()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(clientProfile.ClientId),
                ClientProfile = clientProfile
            };
    }
}