using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TenantInspectAdmin.Domain;

namespace TenantInspectAdmin.DAL.Repository
{
    public interface ITenantInspectAdminRepository
    {
        Task<List<TDestination>> SelectAllAsync<TSource, TDestination>(string? storedProcedure = null) where TSource : EntityBase where TDestination : DTOBase;
        Task<TDestination> SelectAsync<TSource, TDestination>(string? storedProcedure, string _params, string paramsName) where TSource : EntityBase where TDestination : DTOBase;
        Task<TDestination> SelectGuidAsync<TSource, TDestination>(string? storedProcedure, Guid ID, string paramsName) where TSource : EntityBase where TDestination : DTOBase;
        Task<List<TDestination>> SelectAllByIDAsync<TSource, TDestination>(string? storedProcedure, Guid ID, string paramsName) where TSource : EntityBase where TDestination : DTOBase;
        Task<bool> TransactionAsync(string? storedProcedure, object _params);
        Task<bool> TransactionDelete(string? storedProcedure, Guid _params, string paramsName);
    }
}
