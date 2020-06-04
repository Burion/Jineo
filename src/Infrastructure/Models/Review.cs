using System;

namespace Jineo.Models
{
    public class Review
    {
        public string UserId {get;set;}
        public JineoUser User {get;set;}
        public int ProductId {get;set;}
        public Product Product {get;set;}

        public string Text {get;set;}
        public int Mark {get;set;}
    }
}