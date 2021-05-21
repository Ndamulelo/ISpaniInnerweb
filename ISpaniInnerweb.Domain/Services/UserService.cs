using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using ISpaniInnerweb.Domain.Models.SharedViewModel;
using Microsoft.Extensions.Logging;

namespace ISpaniInnerweb.Domain.Services
{
    public class UserService : IUserService
    {
        IRepository<User> userRepository;
        IRepository<Recruiter> recruiterRepository;
        IRepository<JobSeeker> jobSeekerRepository;

        ILogger<UserService> logger;

        public UserService(ILogger<UserService> logger, IRepository<User> userRepository, IRepository<Recruiter> recruiterRepository,
            IRepository<JobSeeker> jobSeekerRepository)
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }

        public User AuthUser(User userModel)
        {
            var authUser = userRepository.FindByCondition(x => x.Email.Equals(userModel.Email) && x.Password.Equals(userModel.Password));
            var results = authUser.FirstOrDefault();
            return results; 
        }

        public User Get(string id)
        {
            var user = new User();

            try
            {

                 user = this.userRepository.Get(id);
                logger.LogInformation("User " + user.UserId + "retrieved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return user;
        }   
        
        public User GetByUserId(string id)
        {
            var user = new User();

            try
            {

                 user = userRepository.FindByCondition(x => x.UserId.Equals(id)).FirstOrDefault();
                logger.LogInformation("User " + user.UserId + "retrieved");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return user;
        }

        public bool IsUserRegistered(User user)
        {
            var isRegistered = false;

            try
            {
                var tempUser = userRepository.FindByCondition(x => x.Email.Equals(user.Email)).FirstOrDefault();
                
                if(tempUser != null)
                { return isRegistered; }

                user.Id = Guid.NewGuid().ToString();
                user.IsActive = true;
                //user.UserId = Guid.NewGuid().ToString();
                userRepository.Insert(user);
                logger.LogInformation("User " + user.Email + "registered");
                isRegistered = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return isRegistered;
        }

        public bool PasswordMatches(ChangePasswordViewModel changePasswordViewModel,string userId)
        {
            if (userId.Equals("b2ef107e-d828-43b0-8945-7b883ba9d3f7"))
            {
                var user = userRepository.FindByConditionAsNoTracking(
                p => p.Password.Equals(changePasswordViewModel.OldPassword)
                && p.Id.Equals(userId)).FirstOrDefault();

                return user != null ? true : false;
            }
            else
            {
                var user = userRepository.FindByConditionAsNoTracking(
                                p => p.Password.Equals(changePasswordViewModel.OldPassword)
                                && p.UserId.Equals(userId)).FirstOrDefault();

                return user != null ? true : false;
            }
        }

        public void ChangePassword(ChangePasswordViewModel changePasswordViewModel,string userId)
        {
            if (userId.Equals("b2ef107e-d828-43b0-8945-7b883ba9d3f7"))
            {
                var userToUpdate = userRepository.FindByConditionAsNoTracking(
                        p => p.Password.Equals(changePasswordViewModel.OldPassword)
                        && p.Id.Equals(userId)).FirstOrDefault();

                userToUpdate.Password = changePasswordViewModel.NewPassword;

                userRepository.Update(userToUpdate);
            }
            else
            {
                var userToUpdate = userRepository.FindByConditionAsNoTracking(
                            p => p.Password.Equals(changePasswordViewModel.OldPassword)
                            && p.UserId.Equals(userId)).FirstOrDefault();

                userToUpdate.Password = changePasswordViewModel.NewPassword;

                userRepository.Update(userToUpdate);
            }
        }

        public User GetUserByEmail(string email)
        {
            return userRepository.FindByCondition(u => u.Email.Equals(email)).FirstOrDefault();
        }

        public bool IsEmailRegistered(string email)
        {
           var user =  userRepository.FindByCondition(u => u.Email.Equals(email)).FirstOrDefault();

            return (user != null ? true : false);
        }
    }
}
