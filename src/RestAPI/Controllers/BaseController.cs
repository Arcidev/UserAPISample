using System;
using System.Threading.Tasks;
using BL.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    /// <summary>
    /// Controller for shared logic across controllers
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Executes action with <see cref="BLException"/> failure callback
        /// </summary>
        /// <param name="action">Action to be done</param>
        /// <param name="failureCallback">Callback if <see cref="BLException"/> occurs</param>
        /// <returns>Result of action</returns>
        protected async Task<T> ExecuteAsync<T>(Func<Task<T>> action, Action<BLException> failureCallback = null) where T : class
        {
            try
            {
                return await action();
            }
            catch (BLException ex)
            {
                failureCallback?.Invoke(ex);
                return null;
            }
        }
    }
}
