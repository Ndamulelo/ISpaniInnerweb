using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Models.SharedViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IUserService
    {
        User Get(string id);
        User GetByUserId(string id);
        User AuthUser(User user);
        bool IsUserRegistered(User user);
        bool IsEmailRegistered(string email);
        User GetUserByEmail(string email);
        bool PasswordMatches(ChangePasswordViewModel changePasswordViewModel, string userId);
        void ChangePassword(ChangePasswordViewModel changePasswordViewModel, string userId);
    }
}
