using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantInspectAdmin.Domain;
using TenantInspectAdmin.DAL.Repository;

namespace TenantInspectAdmin.Service.Services
{
    public class TenantInspectAdminService : ServiceBase, ITenantInspectAdminRepository
    {
        private ILoggerFactory _loggerFactory { get; set; }
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string _sendgridkey;
        public IConfiguration _configuration { get; }
        private readonly string _connectionString;
        public TenantInspectAdminService(ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            this.httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _sendgridkey = _configuration.GetConnectionString("SendGridKey");
            _connectionString = _configuration.GetConnectionString("MoveInspectorConnection");
        }

        public async Task<List<TDestination>> SelectAllAsync<TSource, TDestination>(string? storedProcedure = null)
            where TSource : EntityBase
            where TDestination : DTOBase
        {
            var list = new List<TDestination>();


            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<TDestination>(storedProcedure, commandType: CommandType.StoredProcedure);

                var mapper = new Mapper(_mapperConfiguration);

                result.ToList().ForEach(r =>
                {
                    list.Add(mapper.Map<TDestination>(r));
                });
                return result.ToList();
            }
        }

        public async Task<TDestination> SelectGuidAsync<TSource, TDestination>(string? storedProcedure, Guid ID, string paramsName)
            where TSource : EntityBase
            where TDestination : DTOBase
        {
            var list = new List<TDestination>();

            var dictionary = new Dictionary<string, object>
            {
                {$"@{paramsName}", ID}
            };

            var parameters = new DynamicParameters(dictionary);

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<TDestination>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                var mapper = new Mapper(_mapperConfiguration);

                result.ToList().ForEach(r =>
                {
                    list.Add(mapper.Map<TDestination>(r));
                });
                if (result.Count() > 0)
                {
                    return result.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<TDestination>> SelectAllByIDAsync<TSource, TDestination>(string? storedProcedure, Guid ID, string paramsName)
            where TSource : EntityBase
            where TDestination : DTOBase
        {
            var list = new List<TDestination>();

            var dictionary = new Dictionary<string, object>
            {
                {$"@{paramsName}", ID}
            };

            var parameters = new DynamicParameters(dictionary);

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<TDestination>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                var mapper = new Mapper(_mapperConfiguration);

                result.ToList().ForEach(r =>
                {
                    list.Add(mapper.Map<TDestination>(r));
                });
                return result.ToList();
            }
        }

        public async Task<bool> TransactionAsync(string? storedProcedure, object _params)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var dbArgs = new DynamicParameters();
                var xx = JsonConvert.SerializeObject(_params);
                var request = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(_params));
                if (request != null)
                {
                    foreach (var obj in request)
                    {
                        dbArgs.Add($"@{obj.Key}", obj.Value);
                    }
                    try
                    {
                        var affectedRows = await connection.ExecuteAsync(storedProcedure, dbArgs, commandType: CommandType.StoredProcedure);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<TDestination> SelectAsync<TSource, TDestination>(string? storedProcedure, string _params, string paramsName)
            where TSource : EntityBase
            where TDestination : DTOBase
        {
            var list = new List<TDestination>();

            var dictionary = new Dictionary<string, object>
            {
                {$"@{paramsName}", _params}
            };

            var parameters = new DynamicParameters(dictionary);

            using (var connection = new SqlConnection(_connectionString))
            {
                var result = await connection.QueryAsync<TDestination>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                var mapper = new Mapper(_mapperConfiguration);

                result.ToList().ForEach(r =>
                {
                    list.Add(mapper.Map<TDestination>(r));
                });
                if (result.Count() > 0)
                {
                    return result.First();
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<bool> TransactionDelete(string? storedProcedure, Guid _params, string paramsName)
        {
            var dictionary = new Dictionary<string, object>
            {
                {$"@{paramsName}", _params}
            };

            var parameters = new DynamicParameters(dictionary);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.QueryAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);


                return true;

            }
        }
    }
}
