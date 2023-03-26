using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MSLServer.Data;
using MSLServer.Models;
using MSLServer.SecureServices;
using System.Security.Cryptography;

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
            var user = context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new Exception("User with that id not found");
            }
            return user;
        }
        //
        public User GetByEmail(string email)
        {
            var user = context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                throw new Exception("User with that email address not found");
            }
            return user;
        }

        public User GetByResetToken(string token)
        {
            var user = context.Users.FirstOrDefault(x => x.ResetTokenExpires < DateTime.Now && x.PasswordResetToken == token);
            if (user == null)
            {
                throw new Exception("This token is invalid");
            }
            return user;
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
                Email = request.Email,
                Password = Secure.Encrypt(request.Password),
                VerificationToken = Secure.CreateRandomToken(),
            };
            context.Users.Add(currentuser);
            await context.SaveChangesAsync();
        }

        public async Task<string> LoginUser(UserLoginRequest request)
        {
            var currentuser = GetByEmail(request.Email);
            if (currentuser == null)
            {
                throw new Exception("You cannot login with that email-password combination");
            }
            if (currentuser.VerifiedAt == null)
            {
                throw new Exception("User not verified");
            }
            if (currentuser.Email != request.Email || Secure.Decrypt(currentuser.Password) != request.Password)
            {
                throw new Exception("You cannot login with that email-password combination");
            }
            return currentuser.Id;
        }
        public async Task VerifyUser(string token)
        {
            var currentuser = await context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (currentuser == null)
            {
                throw new Exception("Invalid token");
            }
            if (currentuser.VerifiedAt != null)
            {
                throw new Exception("This user is already verified");
            }
            //Nem biztos hogy itt ki kéne törölni a tokent miután aktiváltuk az accountot. Bár picit felesleges is megtartani
            //currentuser.VerificationToken = null;
            currentuser.VerifiedAt = DateTime.Now;
            await context.SaveChangesAsync();
        }

        public async Task ForgotPassword(string email)
        {
            var currentuser = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (currentuser == null)
            {
                //Dont have to send an error message. we dont want the user to know that the email is registered or not.
                return;
            }

            currentuser.PasswordResetToken = Secure.CreateRandomToken();
            currentuser.ResetTokenExpires = DateTime.Now.AddDays(1);
            await context.SaveChangesAsync();
        }
        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var currentuser = await context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (currentuser == null || currentuser.ResetTokenExpires < DateTime.Now)
            {
                throw new Exception("Invalid or expired token");
            }
            currentuser.Password = Secure.Encrypt(request.Password);
            currentuser.PasswordResetToken = null;
            currentuser.ResetTokenExpires = null;
            await context.SaveChangesAsync();
        }


        public async Task Update(User obj)
        {
            var olduser = GetById(obj.Id);
            //Securing method
            obj.Email = Secure.Encrypt(obj.Email);
            obj.Password = Secure.Encrypt(obj.Password);
            olduser = obj;
            await context.SaveChangesAsync();
        }
        public async Task Delete(string id)
        {
            User existing = GetById(id);
            if (existing == null)
            {
                throw new Exception("User doesnt exists with that id");
            }
            context.Users.Remove(existing);
            context.SaveChangesAsync();
        }
    }
}
