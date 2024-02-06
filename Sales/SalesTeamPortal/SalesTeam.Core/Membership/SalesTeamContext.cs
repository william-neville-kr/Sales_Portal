using SalesTeam.Core.Repository;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using SalesTeam.Core.Data;

namespace SalesTeam.Core.Membership
{
    class SalesTeamContext
    {
        private readonly HttpContext context = HttpContext.Current;
        public vwSalesTeam CurrentLoggedInUser { get; set; }

        #region Ctor
        private SalesTeamContext()
        {
            try
            {
                if (CurrentLoggedInUser == null)
                {
                    UnitOfWork unitOfWork = new UnitOfWork();

                    vwSalesTeam user = unitOfWork.vwSalesTeamsRepository.GetAsQuerable(x => x.SalesRepresentativeFullName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
                    if (user != null && user.SalesTeamId > 0)
                    {
                        CurrentLoggedInUser = user;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.LogException(ex.Message + ex.InnerException, ex);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 	Sets cookie
        /// </summary>
        /// <param name="application"> Application </param>
        /// <param name="key"> Key </param>
        /// <param name="val"> Value </param>
        private static void SetCookie(HttpApplication application, string key, string val)
        {
            HttpCookie cookie = new HttpCookie(key) { Value = val, Expires = string.IsNullOrEmpty(val) ? DateTime.UtcNow.AddMonths(-1) : DateTime.UtcNow.AddHours(128) };
            application.Response.Cookies.Remove(key);
            application.Response.Cookies.Add(cookie);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 	Gets an instance of the Click2MeContext, which can be used to retrieve information about current context.
        /// </summary>
        public static SalesTeamContext Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                if (HttpContext.Current.Items["SalesTeamContext"] == null)
                {
                    SalesTeamContext context2 = new SalesTeamContext();
                    HttpContext.Current.Items.Add("SalesTeamContext", context2);
                    return context2;
                }
                return (SalesTeamContext)HttpContext.Current.Items["SalesTeamContext"];
            }
        }

        /// <summary>
        /// 	Gets or sets an object item in the context by the specified key.
        /// </summary>
        /// <param name="key"> The key of the value to get. </param>
        /// <returns> The value associated with the specified key. </returns>
        public object this[string key]
        {
            get
            {
                if (context == null)
                {
                    return null;
                }

                if (context.Items[key] != null)
                {
                    return context.Items[key];
                }
                return null;
            }
            set
            {
                if (context != null)
                {
                    context.Items.Remove(key);
                    context.Items.Add(key, value);
                }
            }
        }



        /// <summary>
        /// 	Sets the CultureInfo
        /// </summary>
        /// <param name="culture"> Culture </param>
        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        #endregion
    }
}
