using System.Collections.Generic;
using System.Threading.Tasks;
using newplgapi.model;
using newplgapi.model.Dto;

namespace newplgapi.Repository.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register(User user,string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<bool> ChangePassword(string username, string passwordold, string passwordnew);
        Task<AuthReg> RegisterGA(string user);
        Task<IEnumerable<User>> GetUsers();
    }
}