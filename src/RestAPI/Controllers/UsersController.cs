using System;
using System.Threading.Tasks;
using BL.DTO.User;
using BL.Facades;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserFacade userFacade;

        public UsersController(UserFacade userFacade)
        {
            this.userFacade = userFacade;
        }

        /// <summary>
        /// Get user from the system by guid
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Informationa about user</returns>
        /// <example>api/users/00000000-0000-0000-0000-000000000000</example>
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            return "value";
        }

        /// <summary>
        /// Creates user in the system
        /// </summary>
        /// <param name="user">User to be created</param>
        /// <returns>Informationa about signed user</returns>
        /// <example>api/users/signUp</example>
        [HttpPost("signUp")]
        public async Task<UserSignedDTO> Post(UserCreateDTO user)
        {
            return await userFacade.AddUserAsync(user);
        }
    }
}
