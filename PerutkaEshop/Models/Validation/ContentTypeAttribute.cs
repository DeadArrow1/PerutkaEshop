using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Perutka.Eshop.Web.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
    public class ContentTypeAttribute : ValidationAttribute, IClientModelValidator
    {
        public string ContentType { get; set; }
        const string ErrorMessage = "The file doesnt have the appropriate content!";
        public ContentTypeAttribute(string contentType)
        {
            ContentType = contentType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {

                return ValidationResult.Success;
            }
            else if (value is IFormFile formfile)
            {
                if (formfile.ContentType.ToLower().Contains(ContentType.ToLower()))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage, new List<string>() {validationContext.MemberName });
            }
            throw new NotImplementedException($"The validation is not supported for this type {value.GetType()}");
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-filecontent", ErrorMessage);
            context.Attributes.Add("data-val-filecontent-type", ContentType);
        }
    }
}
