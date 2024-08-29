using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManagementSystemCore
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string password = (string)value!;
            if (string.IsNullOrWhiteSpace(password) || 
                password.Length < 9 ||
                !Regex.IsMatch(password,@"[A-Z]") ||
                !Regex.IsMatch(password,@"[a-z]") ||
                !Regex.IsMatch(password,@"[0-9]") ||
                !Regex.IsMatch(password,@"[\W_]") 
                ) {
                return new ValidationResult("Şifre büyük ve küçük harf, rakam, özel karakter içermeli ve en az 9 karakter uzunluğunda olmalı");       
            
            }
            return null;
           
        }
    }
}
