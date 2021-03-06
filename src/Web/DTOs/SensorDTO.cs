using System;

namespace Jineo.DTOs
{
    public class SensorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId {get;set;}
        public float X { get; set; }
        public float Y { get; set; }

        public string Data {get;set;}
        public int ProductId {get;set;}
        public ProductDTO Product {get;set;}
        public float UpperValue {get;set;}
        public float LowerValue {get;set;}
    }
}