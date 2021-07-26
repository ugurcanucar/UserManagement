using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class MongoClas
    {
        [BsonId]
        public ObjectId _Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public ICollection<Address> AddressList { get; set; }
    } 

public class Address
{
    public string Title { get; set; }
    public string Description { get; set; }
}
}
