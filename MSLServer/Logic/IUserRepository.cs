using MSLServer.Models;

namespace MSLServer.Logic
{
    public interface IUserRepository
    {
        IList<User> GetAll();
        User GetById(Guid id);
        void Insert(User user);
        void Update(User obj);
        void Delete(Guid id);
        void Register(string username, string email, string password);
        bool LoginUser(string email, string password);
    }
}