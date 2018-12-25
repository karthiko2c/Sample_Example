using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SubQuip.WebApi
{
    public class FileUploadOperation : IOperationFilter
    {
        //public void Apply(Operation operation, OperationFilterContext context)
        //{
        //    if (operation.OperationId.ToLower() == "apivaluesuploadpost")
        //    {
        //        operation.Parameters.Clear();
        //        operation.Parameters.Add(new NonBodyParameter
        //        {
        //            Name = "uploadedFile",
        //            In = "formData",
        //            Description = "Upload File",
        //            Required = true,
        //            Type = "file"
        //        });
        //        operation.Consumes.Add("multipart/form-data");
        //    }
        //}

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor.Parameters.Any(x => x.ParameterType == typeof(IFormFile)))
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadFile", // must match parameter name from controller method
                    In = "formData",
                    Description = "Upload file.",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("application/form-data");
            }
        }
    }
}
