using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Model
{
    public class Product
    {
        [BsonId]
        public ObjectId ID { get; set; }
        // external Id, easier to reference: 1,2,3 or A, B, C etc.
        public string PId { get; set; }         
        public string Name { get; set; }
        public string Detail { get; set; }
        public bool Watch { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public int? Amountlost { get; set; }
        public string EventNo { get; set; }
    }
    public class ProductDto
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public bool Watch { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public int? Amountlost { get; set; }
        public string EventNo { get; set; }
    }    
}