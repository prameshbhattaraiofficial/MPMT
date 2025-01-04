using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mpmt.Core.Common.Attribites
{
   
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        
        public AllowedExtensionsAttribute(string ErrorMessage = null) : base(ErrorMessage)
        {
           
        }

        protected  override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            
            var file = value as IFormFile;
            if (file != null)
            {
                var (isvalidimage, _) =  FileValidatorUtils.IsValidImageAsync(file, FileTypes.ImageFiles).Result;
                
                if (!isvalidimage)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            var filelist = value as List<IFormFile>;
            if (filelist != null)
            {
                foreach(var f in filelist)
                {
                    var (isvalidimage, _) = FileValidatorUtils.IsValidImageAsync(f, FileTypes.ImageFiles).Result;

                    if (!isvalidimage)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
               
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage()
        {
            if( string.IsNullOrEmpty(ErrorMessage))
            {
                return $"This Image extension is not allowed!";
            }
            return ErrorMessage;
        }
    }
}
