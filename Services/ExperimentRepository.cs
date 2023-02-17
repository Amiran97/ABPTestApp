using ABPTestApp.Models;
using ABPTestApp.Models.DTOs;
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

        public async Task CreateAsync(string deviceToken, string key, string value)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                DbTransaction transaction = await connection.BeginTransactionAsync();
                using (DbCommand command = connection.CreateCommand())
                {
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText = @"INSERT INTO [dbo].[Experiments] 
                                            ([DeviceToken], [Key], [Value])
                                            VALUES (@deviceToken, @key, @value)";
                        command.Parameters.Add(new SqlParameter("@deviceToken", deviceToken));
                        command.Parameters.Add(new SqlParameter("@key", key));
                        command.Parameters.Add(new SqlParameter("@value", value));
                        await command.ExecuteNonQueryAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }
            }
        }

        public async Task CreateAsync(Experiment experiment)
        {
            await CreateAsync(experiment.DeviceToken, experiment.Key, experiment.Value);
        }

        public async Task<string> GetExperimentValueByDeviceTokenAndKeyAsync(string deviceToken, string key)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[dbo].[GetExperimentValueByDeviceTokenAndKey]", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@deviceToken", deviceToken));
                    command.Parameters.Add(new SqlParameter("@key", key));
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                return reader.GetString("Value");
                            }
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<StatisticsResponse> GetStatisticsAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var experiments = await GetExperimentsAsync(connection);
                var countDevices = (await GetAllDeviceTokensAsync(connection)).Count;
                var countDevicesOfValuesByKeys = new List<CountDevicesOfValuesByKeyResponse>();
                var countDevicesOfValuesByButtonColors = await GetCountDevicesOfValuesByKeyAsync(connection, "button-color");
                var countDevicesOfValuesByPrices = await GetCountDevicesOfValuesByKeyAsync(connection, "price");
                countDevicesOfValuesByKeys.Add(new CountDevicesOfValuesByKeyResponse
                {
                    Key = "button_color",
                    CountDevicesOfValues = countDevicesOfValuesByButtonColors
                });
                countDevicesOfValuesByKeys.Add(new CountDevicesOfValuesByKeyResponse
                {
                    Key = "price",
                    CountDevicesOfValues = countDevicesOfValuesByPrices
                });
                return new StatisticsResponse
                {
                    Experiments = experiments,
                    CountDevices = countDevices,
                    CountDevicesOfValuesByKeys = countDevicesOfValuesByKeys
                };
            }
        }

        private async Task<ICollection<ExperimentResponse>> GetExperimentsAsync(SqlConnection connection)
        {
            ICollection<ExperimentResponse> experiments = new List<ExperimentResponse>();
            var query = @"SELECT *
                            FROM [dbo].[Experiments]";
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new ExperimentResponse();
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

        private async Task<ICollection<DeviceTokensResponse>> GetAllDeviceTokensAsync(SqlConnection connection)
        {
            ICollection<DeviceTokensResponse> result = new List<DeviceTokensResponse>();

            using (SqlCommand command = new SqlCommand("[dbo].[GetAllDeviceTokens]", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new DeviceTokensResponse();
                            item.DeviceToken = reader.GetString("DeviceToken");
                            result.Add(item);
                        }
                    }
                    return result;
                }
            }
        }

        private async Task<ICollection<CountDevicesOfValueResponse>> GetCountDevicesOfValuesByKeyAsync (SqlConnection connection, string key)
        {
            ICollection<CountDevicesOfValueResponse> result = new List<CountDevicesOfValueResponse>();

            using (SqlCommand command = new SqlCommand("[dbo].[GetCountDevicesOfValuesByKey]", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@key", key));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new CountDevicesOfValueResponse();
                            item.Value = reader.GetString("Value");
                            item.CountDevices = reader.GetInt32("DeviceCount");
                            result.Add(item);
                        }
                    }
                    return result;
                }
            }
        }
    }
}