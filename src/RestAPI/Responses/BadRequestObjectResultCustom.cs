using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace RestAPI.Responses
{
    public class BadRequestObjectResultCustom : BadRequestObjectResult
    {
        public BadRequestObjectResultCustom(ModelStateDictionary errors, string errorMessage) : base(new { Errors = errors?.Values.Select(x => x.Errors), Message = errorMessage })
        {
        }
    }
}
