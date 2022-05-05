
using MyNoSqlServer.Abstractions;

namespace Service.GroupManager.Domain.Models.NoSql
{
    public class GroupNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-groups";

        public static string GeneratePartitionKey() => "Groups";

        public static string GenerateRowKey(string id) => id;
        
        public Group Group { get; set; }

        public static GroupNoSqlEntity Create(Group group) =>
            new()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(group.GroupId),
                Group = group
            };
    }
}