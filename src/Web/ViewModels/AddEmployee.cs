using System;
using System.ComponentModel.DataAnnotations;

namespace Jineo.ViewModels
{
    public class AddEmployee
    {   
         [Required(ErrorMessage = "Не указано имя")]
        public string Email { get; set; }
        public string CompanyId { get; set; }
    }
}