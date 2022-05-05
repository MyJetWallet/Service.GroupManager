
using System.Collections.Generic;
using MyNoSqlServer.Abstractions;

namespace Service.GroupManager.Domain.Models.NoSql
{
    public class ServiceGroupsListNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "myjetwallet-service-groups";

        public static string GeneratePartitionKey() => "ServiceGroups";

        public static string GenerateRowKey() => "ServiceGroups";
        
        public List<string> InterestGroups { get; set; }

        public List<string> ConverterGroups { get; set; }

        public List<string> WithdrawalGroups { get; set; }

        public static ServiceGroupsListNoSqlEntity Create(List<string> interestGroups, List<string> converterGroups, List<string> withdrawalGroups ) =>
            new()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(),
                InterestGroups = interestGroups,
                WithdrawalGroups = withdrawalGroups,
                ConverterGroups = converterGroups
            };
    }
}