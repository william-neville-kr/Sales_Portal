using Microsoft.AspNet.Identity;
using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Helpers;
using PhysicianPortal.Core.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Data.Entity.Core.Objects;
//InnerException = {"Store does not implement IUserSecurityStampStore<TUser>."}
namespace PhysicianPortal.Core.UserStore
{
    public class UserStoreService : IUserStore<User>, IUserPasswordStore<User>, IUserEmailStore<User>,
        IUserSecurityStampStore<User>, IUserLockoutStore<User, string>, IUserTwoFactorStore<User, string>, IUserPhoneNumberStore<User>, IUserRoleStore<User>, IUserClaimStore<User>
    //, IUserLoginStore<User>
    {
        private readonly MH_DWEntities globalContext = new MH_DWEntities();


        public Task CreateAsync(User user)
        {
            //MH_DWEntities context = new MH_DWEntities();
            globalContext.Users.Add(user);
            globalContext.Configuration.ValidateOnSaveEnabled = false;
            globalContext.SaveChanges();
            //context.Dispose();
            return Task.FromResult(1);
        }

        public Task DeleteAsync(User user)
        {
            //MH_DWEntities context = new MH_DWEntities();
            globalContext.Users.Remove(user);
            globalContext.Configuration.ValidateOnSaveEnabled = false;
            globalContext.SaveChanges();
            //context.Dispose();
            return Task.FromResult(1);
        }
        public Task<User> FindByIdAsync(string userId)
        {
            //MH_DWEntities context = new MH_DWEntities();

            var id = userId.ToInt0();

            User user = globalContext.Users.Where(u => u.UserId == id).FirstOrDefault();
            //context.Dispose();
            return Task.FromResult(user);
        }
        public Task<User> FindByNameAsync(string userName)
        {
            //MH_DWEntities context = new MH_DWEntities();
            User user = globalContext.Users.Where(u => u.Email.ToLower() == userName.ToLower()).FirstOrDefault();
            //context.Dispose();
            return Task.FromResult(user);
        }
        public Task UpdateAsync(User user)
        {
            //MH_DWEntities context = new MH_DWEntities();
            globalContext.Users.Attach(user);
            globalContext.Entry(user).State = EntityState.Modified;
            globalContext.Configuration.ValidateOnSaveEnabled = false;
            globalContext.SaveChanges();
            //context.Dispose();
            return Task.FromResult(1);


            //UnitOfWork unitOfWork = new UnitOfWork();
            //unitOfWork.UserRepository.context.Configuration.ValidateOnSaveEnabled = false;
            //unitOfWork.UserRepository.Update(user);
            //unitOfWork.Save();
            //return Task.FromResult(1);
        }



        public void Dispose()
        {
            MH_DWEntities context = new MH_DWEntities();
            context.Dispose();
        }


        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.PasswordHash);
        }


        public Task<User> FindByEmailAsync(string email)
        {
            //MH_DWEntities context = new MH_DWEntities();
            User user = globalContext.Users.FirstOrDefault(x => x.Email == email);
            //context.Dispose();
            return Task.FromResult(user);
        }


        public Task<string> GetEmailAsync(User user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user)
        {
            return Task.FromResult(user.EmailConfirmed); ;
        }


        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetEmailAsync(User user, string email)
        {
            return Task.FromResult(user.Email = email);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return Task.FromResult(user.PasswordHash = passwordHash);
        }


        public Task<string> GetSecurityStampAsync(User user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
        public Task SetSecurityStampAsync(User user, string stamp)
        {
            return Task.FromResult(user.SecurityStamp = stamp);
        }


        public Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            if (user.LockoutEndDateUtc != null)
            {
                var x = new DateTimeOffset(user.LockoutEndDateUtc.Value);
                return Task.FromResult(x);
            }

            return Task.FromResult(new DateTimeOffset(DateTime.UtcNow.AddHours(-1)));


        }

        public Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult(user.LockoutEndDateUtc = lockoutEnd.UtcDateTime);
        }

        public Task<int> IncrementAccessFailedCountAsync(User user)
        {
            int no = user.AccessFailedCount ?? 0;
            no = no + 1;
            user.AccessFailedCount = no;
            return Task.FromResult(no);
        }

        public Task ResetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount = 0);
        }

        public Task<int> GetAccessFailedCountAsync(User user)
        {
            return Task.FromResult(user.AccessFailedCount ?? 0);
        }

        public Task<bool> GetLockoutEnabledAsync(User user)
        {
            return Task.FromResult(user.LockoutEnabled ?? false);
        }

        public Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult(user.LockoutEnabled = enabled);
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            return Task.FromResult(user.TwoFactorEnabled = enabled);
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return Task.FromResult(user.TwoFactorEnabled ?? false);
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            return Task.FromResult(user.PhoneNumber = phoneNumber);
        }

        public Task<string> GetPhoneNumberAsync(User user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed ?? false);
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            return Task.FromResult(user.PhoneNumberConfirmed = confirmed);
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            //MH_DWEntities context = new MH_DWEntities();
            Role role = globalContext.Roles.Where(x => x.Name == roleName).FirstOrDefault();
            //context.Dispose();
            return Task.FromResult(user.RoleId = role.RoleId);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            //MH_DWEntities context = new MH_DWEntities();
            Role role = globalContext.Roles.Where(x => x.Name == roleName).FirstOrDefault();
            //context.Dispose();
            if (user.RoleId == role.RoleId)
            {
                return Task.FromResult(user.RoleId = 0);
            }
            return null;
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            IList<string> roles = new List<string>();
            roles.Add(user.Role.Name);
            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            //MH_DWEntities context = new MH_DWEntities();
            Role role = globalContext.Roles.Where(x => x.Name == roleName).FirstOrDefault();
            //context.Dispose();
            return Task.FromResult(user.RoleId == role.RoleId ? true : false);
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            int userId = user.UserId;
            //MH_DWEntities context = new MH_DWEntities();
            IList<Claim> claims = globalContext.UserClaims.Where(x => x.UserId == userId).ToList().Select(t => new Claim(t.ClaimName, t.ClaimValue)).ToList();
            //context.Dispose();

            return Task.FromResult(claims);
        }

        public Task AddClaimAsync(User user, Claim claim)
        {
            int userId = user.UserId;
            //MH_DWEntities context = new MH_DWEntities();
            globalContext.UserClaims.Add(new UserClaim { UserId = userId, ClaimName = claim.Type, ClaimValue = claim.Value });
            globalContext.SaveChanges();
            //context.Dispose();
            return Task.FromResult(1);
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            int userId = user.UserId;
            //MH_DWEntities context = new MH_DWEntities();
            UserClaim claimToRemove = globalContext.UserClaims.Where(x => x.UserId == userId && x.ClaimName == claim.Type).First();

            if (claimToRemove != null)
            {
                globalContext.UserClaims.Remove(claimToRemove);
            }
            globalContext.SaveChanges();
            //context.Dispose();
            return Task.FromResult(1);
        }

        //public Task AddLoginAsync(User user, UserLoginInfo login)
        //{
        //	throw new NotImplementedException();
        //}

        //public Task RemoveLoginAsync(User user, UserLoginInfo login)
        //{
        //	throw new NotImplementedException();
        //}

        //public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        //{
        //	throw new NotImplementedException();
        //}

        //public Task<User> FindAsync(UserLoginInfo login)
        //{
        //	throw new NotImplementedException();
        //}
    }
}
