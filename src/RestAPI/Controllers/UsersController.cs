using System;
using System.Threading.Tasks;
using BL.DTO.User;
using BL.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Exceptions;
using RestAPI.Security;

namespace RestAPI.Controllers
{
    /// <summary>
    /// Controller for user related requests
    /// </summary>
    public class UsersController : BaseController
    {
        private readonly UserFacade userFacade;

        /// <summary>
        /// Creates new instance of User Controller
        /// </summary>
        /// <param name="userFacade">User facade to access user logic</param>
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
        [Authorize, HttpGet("{id}")]
        public async Task<UserDTO> Get(Guid id)
        {
            var user = await userFacade.GetUserAsync(id);

            return user;
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
            user.Token = JwtTokenHelper.GenerateToken(user.Email);

            return await ExecuteAsync(
                () => userFacade.AddUserAsync(user),
                (ex) => throw new BadRequestException(ex));
        }

        /// <summary>
        /// Signs up user
        /// </summary>
        /// <param name="user">User credentials</param>
        /// <returns>Informationa about signed user</returns>
        /// <example>api/users/signIn</example>
        [HttpPost("signIn")]
        public async Task<UserSignedDTO> Post(UserCredentialsDTO credentials)
        {
            credentials.Token = JwtTokenHelper.GenerateToken(credentials.Email);

            return await ExecuteAsync(
                () => userFacade.SignInUser(credentials),
                (ex) => throw new UnauthorizedException(ex));
        }
    }
}
