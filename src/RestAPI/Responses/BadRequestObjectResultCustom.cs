using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RestAPI.Responses
{
    /// <summary>
    /// Extends <see cref="BadRequestObjectResult"/> by providin custom error message
    /// </summary>
    public class BadRequestObjectResultCustom : BadRequestObjectResult
    {
        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="errors">Model state dictionary with errors</param>
        /// <param name="errorMessage">Custom error message</param>
        public BadRequestObjectResultCustom(ModelStateDictionary errors, string errorMessage) : base(errors)
        {
            Value = new { Errors = Value, Message = errorMessage };
        }
    }
}
