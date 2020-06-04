using System;

namespace Jineo.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ProjectId {get;set;}
        public Project Project {get;set;}
        public int TypeId {get;set;}

        public int ProductId {get;set;}
        public Product Product {get;set;}
        public float X { get; set; }
        public float Y { get; set; }

        public string Data {get;set;}
        public float UpperValue {get;set;}
        public float LowerValue {get;set;}
    }
}