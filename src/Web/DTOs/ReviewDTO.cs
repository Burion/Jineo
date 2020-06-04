using System;

namespace Jineo.DTOs
{
    public class ReviewDTO
    {
        public string UserId {get;set;}
        public UserDTO User {get;set;}
        public int ProductId {get;set;}
        public ProductDTO Product {get;set;}

        public string Text {get;set;}
        public int Mark {get;set;}
    }
}