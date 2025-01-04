using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Mpmt.Core.Common.Attribites
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize = (5 * 1048576),string ErrorMessage = null) : base(ErrorMessage)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult(GetErrorMessage());
                }
            }
            var filelist = value as List<IFormFile>;
            if (filelist != null)
            {
                foreach(var fileitem in filelist)
                {
                    if (fileitem.Length > _maxFileSize)
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }
               
            }


            return ValidationResult.Success;
        }
        public string GetErrorMessage()
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {
                return $"Maximum allowed file size is {_maxFileSize / 1048576} mb.";
            }
            return ErrorMessage;
        }
    }
}
