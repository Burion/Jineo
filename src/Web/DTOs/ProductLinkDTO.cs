using System;

namespace Jineo.DTOs
{
    public class ProductLinkDTO
    {
        public int Id {get;set;}
        public int ProductId {get;set;}
        public ProductDTO Product {get;set;}
        public string Link {get;set;}
        public float Price {get;set;}
        public string Store {get;set;}
    }
}