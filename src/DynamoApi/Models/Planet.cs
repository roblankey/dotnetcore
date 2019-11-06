using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoApi.Models
{
    [DynamoDBTable("Planets")]
    public class Planet
    {
        [DynamoDBHashKey]
        public string Universe { get; set; }
        
        [DynamoDBRangeKey]
        public string Name { get; set; }
        
        [DynamoDBVersion]
        public int? Version { get; set; }
        
        public List<Moon> Moons { get; set; }
    }
}