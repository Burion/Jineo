using System;
using System.Collections.Generic;

namespace Jineo.Models
{
    public class Product
    {
        public int Id {get;set;}
        public int ProductTypeId {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public string Image {get;set;}
        public List<ProductLink> Links {get;set;}
        public List<Review> Reviews {get;set;}
    }
}