using System;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoApi.Models
{
    [DynamoDBTable("Planets")]
    public class Planet
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        
        [DynamoDBVersion]
        public int? Version { get; set; }
        
        [DynamoDBProperty]
        public int AlienScore { get; set; }
        
        [DynamoDBProperty]
        public string Moons { get; set; }
    }
}