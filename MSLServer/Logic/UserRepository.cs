using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IList<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }
        public async Task<User> GetById(string id)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        //
        public async Task<User> GetByEmail(string email)
        {
            return await context.Users.FirstOrDefaultAsync(x => x.Email == Secure.Encrypt(email));
        }
        public async Task Insert(User user)
        {
            user.Email = Secure.Encrypt(user.Email);
            user.Password = Secure.Encrypt(user.Password);
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task RegisterUser(UserRegisterRequest request)
        {
            if (context.Users.Any(x => x.Email == request.Email))
            {
                throw new Exception("User is already exists");
            }

            var currentuser = new User()
            {
                Email = Secure.Encrypt(request.Email),
                Password = Secure.Encrypt(request.Password),
                VerificationToken = Secure.CreateRandomToken(),
            };
            context.Users.Add(currentuser);
            await context.SaveChangesAsync();
        }
        //
        public async Task<bool> LoginUser(UserLoginRequest request)
        {
            var currentuser = await GetByEmail(request.Email);
            if (currentuser == null)
            {
                throw new Exception("You cannot login with that email-password combination");
            }
            if (currentuser.VerifiedAt == null)
            {
                throw new Exception("User not verified");
            }
            if (Secure.Decrypt(currentuser.Email) != request.Email || Secure.Decrypt(currentuser.Password) != request.Password)
            {
                throw new Exception("You cannot login with that email-password combination");
            }
            return true;
        }
        public async Task VerifyUser(string token)
        {
            var currentuser = await context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (currentuser == null)
            {
                throw new Exception("Invalid token");
            }
            currentuser.VerifiedAt = DateTime.Now;
            await context.SaveChangesAsync();
        }



        public async Task Update(User obj)
        {
            var olduser = await GetById(obj.Id);
            //Securing method
            obj.Email = Secure.Encrypt(obj.Email);
            obj.Password = Secure.Encrypt(obj.Password);
            olduser = obj;
            await context.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            User existing = await GetById(id);
            if (existing == null)
            {
                throw new Exception("User doesnt exists with that id");
            }
            context.Users.Remove(existing);
            context.SaveChangesAsync();
        }
    }
}
