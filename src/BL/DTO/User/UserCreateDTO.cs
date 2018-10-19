
namespace BL.DTO.User
{
    public class UserCreateDTO : UserBaseDTO
    {
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
