using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MSLServer.Data;
using MSLServer.Models;
using MSLServer.SecureServices;

namespace MSLServer.Logic
{
    public class UserRepository : IUserRepository
    {
        ServerListDBContext context;

        public UserRepository(ServerListDBContext _context)
        {
            context = _context;
        }
        public IList<User> GetAll()
        {
            return context.Users.ToList();
        }
        public User GetById(string id)
        {
            return context.Users.FirstOrDefault(x => x.Id == id);
        }
        //
        public User GetByEmail(string email)
        {
            return context.Users.FirstOrDefault(x => x.Email == Secure.Encrypt(email));
        }
        public void Insert(User user)
        {
            user.UserName = Secure.Encrypt(user.UserName);
            user.Email = Secure.Encrypt(user.Email);
            user.Password = Secure.Encrypt(user.Password);
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Register(string username, string email, string password)
        {
            var currentuser = new User(Secure.Encrypt(username), Secure.Encrypt(email), Secure.Encrypt(password));
            context.Users.Add(currentuser);
            context.SaveChanges();
        }
        //
        public bool LoginUser(string email, string password)
        {
            var currentuser = GetByEmail(email);
            if (currentuser != null)
            {
                if (currentuser.Email == Secure.Encrypt(email) && currentuser.Password == Secure.Encrypt(password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


        public void Update(User obj)
        {
            var olduser = GetById(obj.Id);
            //Securing method
            obj.UserName = Secure.Encrypt(obj.UserName);
            obj.Email = Secure.Encrypt(obj.Email);
            obj.Password = Secure.Encrypt(obj.Password);
            olduser = obj;
            context.SaveChanges();
        }
        public void Delete(string id)
        {
            User existing = context.Users.Find(id);
            context.Users.Remove(existing);
            context.SaveChanges();
        }

        public User GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
