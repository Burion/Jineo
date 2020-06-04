using System;
using System.Collections.Generic;

namespace Jineo.DTOs
{
    public class ProductDTO
    {
        public int Id {get;set;}
        public int ProductTypeId {get;set;}
        public string Name {get;set;}
        public string Description {get;set;}
        public string Image {get;set;}
        public float AvgPrice {get;set;}
        public int AvgMark {get;set;}
        public List<ProductLinkDTO> Links {get;set;}
        public List<ReviewDTO> Reviews {get;set;}
        public int Running {get;set;}
    }
}