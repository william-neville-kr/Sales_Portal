using System;
using System.Linq;
using System.Web.Security;
using SalesTeam.Core.Data;

namespace SalesTeam.Core.Membership
{
    class SalesTeamMembership : MembershipProvider
    {
        private Repository.UnitOfWork _unitOfWork = new Repository.UnitOfWork();
        public override string ApplicationName
        {
            get
            {
                return "Krogger";
            }

            set
            {
                
            }
        }

        public override bool EnablePasswordReset
        {
            get
            {
                return true;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return true;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 5;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return 3;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return 1;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return false;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return true;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            vwSalesTeam user = _unitOfWork.vwSalesTeamsRepository.GetAsQuerable(x => x.SalesRepresentativeFullName == username).FirstOrDefault();

            if (user != null && user.SalesTeamId > 0)
            {
                return new MembershipUser("", user.SalesRepresentativeFullName, user.SalesRepresentativeFullName, user.SalesRepresentativeFullName, null, null, true, false,
                                          DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow,
                                          DateTime.MaxValue);
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return GetUser(providerUserKey.ToString(), userIsOnline);
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            vwSalesTeam user = _unitOfWork.vwSalesTeamsRepository.GetAsQuerable(x => x.SalesRepresentativeFullName.Equals(username)/*&& Implement password matching here*/).FirstOrDefault();

            if (user != null && user.SalesTeamId > 0)
            {
                SalesTeamContext.Current.CurrentLoggedInUser = user;

                return true;
            }
            return false;
        }
    }
}
