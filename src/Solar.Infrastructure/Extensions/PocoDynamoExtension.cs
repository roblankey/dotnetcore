using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Amazon.DynamoDBv2;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Aws.DynamoDb;
using Solar.Core.Entities;

namespace Solar.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class PocoDynamoExtension
    {
        public static void AddPocoDynamo(this IServiceCollection services, AmazonDynamoDBConfig config)
        {
            var dynamoClient = new AmazonDynamoDBClient(config);
            var pocoDynamo = new PocoDynamo(dynamoClient);

            // register entities
            var entities = Assembly.GetAssembly(typeof(BaseEntity))?.GetTypes()
                .Where(t => t.BaseType == typeof(BaseEntity)).ToList();
            foreach (var entity in entities ?? new List<Type>())
            {
                pocoDynamo.RegisterTable(entity);
            }
            
            pocoDynamo.InitSchema();
            services.AddSingleton<IPocoDynamo>(pocoDynamo);
        }
    }
}
