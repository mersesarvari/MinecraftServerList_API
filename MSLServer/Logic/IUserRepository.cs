using MSLServer.Models;

namespace MSLServer.Logic
{
    public interface IUserRepository
    {
        Task Delete(string id);
        Task<IList<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> GetById(string id);
        Task Insert(User user);
        Task<bool> LoginUser(UserLoginRequest request);
        Task RegisterUser(UserRegisterRequest request);
        Task Update(User obj);
    }
}