﻿using MSLServer.Models;
using MSLServer.Models.User;

namespace MSLServer.Logic
{
    public interface IUserRepository
    {
        Task Delete(string id);
        Task ForgotPassword(string email);
        IList<User> GetAll();
        User GetByEmail(string email);
        User GetById(string id);
        User GetByResetToken(string token);
        Task Insert(User user);
        Task<User> LoginUser(UserLoginRequest request);
        Task RegisterUser(UserRegisterRequest request);
        Task ResetPassword(ResetPasswordRequest request);
        Task Update(User obj);
        Task VerifyUser(string token);
        bool IsUserAthorized(string token, string id);
    }
}