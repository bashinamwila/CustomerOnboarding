﻿using System.Text;
using Csla;
using Csla.DataPortalClient;
using Csla.Server;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;


namespace CustomerOnboarding.BusinessLibrary.Multitenancy
{
    public class MultitenancyDataPortalCache(IMemoryCache cache,ILogger<MultitenancyDataPortalCache>logger):IDataPortalCache
    {
        private readonly IMemoryCache _cache=cache;
        private readonly ILogger<MultitenancyDataPortalCache> _logger=logger;

        public async Task<DataPortalResult> GetDataPortalResultAsync(Type objectType, object criteria,
            DataPortalOperations operation, Func<Task<DataPortalResult>> portal)
        {
            if (operation == DataPortalOperations.Fetch && objectType == typeof(TenantInfo))
            {
                _logger.LogInformation($"At {DateTime.Now.ToString()} fetch data from cache");
                // this operation + type is cached
                return await GetResultAsync(objectType, criteria, operation, portal);
            }
            else
            {
                _logger.LogInformation($"At {DateTime.Now.ToString()} fetch data from db");
                // the result isn't cached
                return await portal();
            }
        }

        private async Task<DataPortalResult> GetResultAsync(Type objectType, object criteria, 
            DataPortalOperations operation, Func<Task<DataPortalResult>> portal)
        {
            DataPortalResult? result;
            var key = GetKey(objectType, criteria, operation);
            result = await _cache.GetOrCreateAsync(key, async (v) =>
            {
                var obj = await portal();
                v.AbsoluteExpiration = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(60);
                return obj;
            });
            if (result != null)
                return result;
            else
                return await portal();
        }

        private static string GetKey(Type objectType, object criteria, DataPortalOperations operation)
        {
            var builder = new StringBuilder();
            // requested type
            builder.Append(objectType.FullName);
            builder.Append('|');

            // criteria values (each criteria has 'valid' ToString)
            var criteriaList = Csla.Server.DataPortal.GetCriteriaArray(criteria);
            foreach (var item in criteriaList)
            {
                builder.Append(item.ToString());
                builder.Append('|');
            }

            // operation
            builder.Append(operation.ToString());
            return builder.ToString();
        }
    }
}
