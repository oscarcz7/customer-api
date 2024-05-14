using customer_api.Models;

namespace customer_api.Services
{
    public interface IUserService
    {
        Task<User> Register(UserInsertDto user);
        Task<string> Login(UserInsertDto user);
    }
}