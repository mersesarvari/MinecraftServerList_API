﻿using MSLServer.Models;

namespace MSLServer.Logic
{
    public interface IUserRepository
    {
        Task Delete(string id);
        Task ForgotPassword(string email);
        Task<IList<User>> GetAll();
        Task<User> GetByEmail(string email);
        Task<User> GetById(string id);
        Task Insert(User user);
        Task<bool> LoginUser(UserLoginRequest request);
        Task RegisterUser(UserRegisterRequest request);
        Task ResetPassword(ResetPasswordRequest request);
        Task Update(User obj);
        Task VerifyUser(string token);
    }
}