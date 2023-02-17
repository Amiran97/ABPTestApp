using ABPTestApp.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace ABPTestApp.Services
{
    public class ExperimentRepository : IExperimentRepository
    {
        private readonly string connectionString;

        public ExperimentRepository(IConfiguration configuration)
        {
            connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = configuration.GetConnectionString("LocaleSqlServer");
            }
        }

        public async Task CreateAsync(string DeviceToken, string Key, string Value)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DbTransaction transaction = await connection.BeginTransactionAsync();
                DbCommand command = connection.CreateCommand();
                command.Transaction = transaction;

                try
                {
                    command.CommandText = @"INSERT INTO [dbo].[Experiments] 
                                            ([DeviceToken], [Key], [Value])
                                            VALUES (@deviceToken, @key, @value)";
                    command.Parameters.Add(new SqlParameter("@deviceToken", DeviceToken));
                    command.Parameters.Add(new SqlParameter("@key", Key));
                    command.Parameters.Add(new SqlParameter("@value", Value));
                    await command.ExecuteNonQueryAsync();

                    await transaction.CommitAsync();
                } catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task CreateAsync(Experiment experiment)
        {
            await CreateAsync(experiment.DeviceToken, experiment.Key, experiment.Value);
        }

        public async Task<ICollection<Experiment>> GetDataAsync()
        {
            ICollection<Experiment> experiments = new List<Experiment>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = @"SELECT *
                              FROM [dbo].[Experiments]";
                var command = new SqlCommand(query, connection);
                var reader = await command.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Experiment item = new Experiment();
                        item.Id = reader.GetGuid("Id");
                        item.DeviceToken = reader.GetString("DeviceToken");
                        item.Key = reader.GetString("Key");
                        item.Value = reader.GetString("Value");
                        experiments.Add(item);
                    }
                }
                return experiments;
            }
        }
    }
}
