using System;

namespace Jineo.Models
{
    public class ProductLink
    {
        public int Id {get;set;}
        public int ProductId {get;set;}
        public Product Product {get;set;}
        public string Link {get;set;}
        public float Price {get;set;}
        public string Store {get;set;}
    }
}