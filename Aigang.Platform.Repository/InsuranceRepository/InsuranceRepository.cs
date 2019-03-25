using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Utils;
using Dapper;
using MySql.Data.MySqlClient;

namespace Aigang.Platform.Repository.InsuranceRepository
{
    public class InsuranceRepository : IInsuranceRepository
    {
        private readonly string _connectionString = ConfigurationManager.GetConnectionString("MySql");
        private string _appName = ConfigurationManager.GetString("AppName");

        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<Policy> InsertPolicyAsync(Policy policy)
        {
            policy.Id = GuidGenerator.Generate();
            
            policy.ModifiedUtc = DateTime.UtcNow;
            policy.CreateUtc = DateTime.UtcNow;
            
            using (IDbConnection dbConnection = Connection)
            {
                var query = "INSERT INTO `aigang.insurance`.policy (Id, DeviceId, Status, Premium, Payout, Fee, Properties, ModifiedUtc, CreateUtc, ProductTypeId, ProductAddress) " +
                            "VALUES (@Id, @DeviceId, @Status, @Premium, @Payout, @Fee, @Properties, @ModifiedUtc, @CreateUtc, @ProductTypeId, @ProductAddress)";
                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, 
                    new
                    {
                        Id = policy.Id,
                        DeviceId = policy.DeviceId,
                        Status = policy.Status,
                        Premium = policy.Premium,
                        Payout = policy.Payout,
                        Fee = policy.Fee,
                        Properties = policy.Properties,
                        ModifiedUtc = policy.ModifiedUtc,
                        CreateUtc = policy.CreateUtc,
                        ProductTypeId = policy.ProductTypeId,
                        ProductAddress = policy.ProductAddress
                    });
            }

            return policy;
        }

        
        public async Task<Policy> UpdatePolicyAsync(Policy policy)
        {    
            var query = "UPDATE `aigang.insurance`.policy " + 
                        "SET Status = @Status, ModifiedUtc = @ModifiedUtc, ClaimProperties = @ClaimProperties, PayoutUtc = @PayoutUtc " +
                        "WHERE Id = @Id";
            
            var now = DateTime.UtcNow;
      
            policy.ModifiedUtc = now;
           
            using (IDbConnection connection = Connection)
            {
                connection.Open();
                await connection.ExecuteAsync(query, 
                     new
                     {
                         Status = policy.Status,
                         ModifiedUtc = policy.ModifiedUtc,
                         ClaimProperties = policy.ClaimProperties,
                         PayoutUtc = policy.PayoutUtc,
                         Id = policy.Id
                     });

            }
            
            return policy;
        }


        public async Task<string> GetPolicyDeviceId(string policyId)
        {
            var query = "SELECT DeviceId FROM `aigang.insurance`.policy WHERE Id = @Id";

            string result;

            using (IDbConnection connection = Connection)
            {
                connection.Open();
                result = await connection.QuerySingleAsync<string>(query,
                    new
                    {
                        Id = policyId
                    });
            }

            return result;
        }
        
        public async Task<PolicyTransactions> GetPolicyTransactions(string policyId)
        {
            var query = "SELECT AddPolicyTx, ClaimTx FROM `aigang.insurance`.policy WHERE Id = @Id";

            PolicyTransactions result;

            using (IDbConnection connection = Connection)
            {
                connection.Open();
                result = await connection.QuerySingleAsync<PolicyTransactions>(query,
                    new
                    {
                        Id = policyId
                    });
            }

            return result;
        }
    }

    public interface IInsuranceRepository
    {
        Task<Policy> InsertPolicyAsync(Policy policy);
        
        Task<Policy> UpdatePolicyAsync(Policy policy);
        
        Task<string> GetPolicyDeviceId(string policyId);
        
        Task<PolicyTransactions> GetPolicyTransactions(string policyId);
    }
}