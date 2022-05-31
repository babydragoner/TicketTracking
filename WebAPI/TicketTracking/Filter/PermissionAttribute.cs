using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketTracking.Models;

namespace Portal.Filter
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Check current user allow access resource action or not
        /// </summary>
        /// <param name="ResourceName"></param>
        /// <param name="ActionName"></param>
        public PermissionAttribute(string ActionName, string ResourceName = "", bool HadPrefix = false) : base(typeof(PermissionTypeAttribute))
        {
            Arguments = new object[] { ActionName, ResourceName, HadPrefix };
        }

        public PermissionAttribute(ActionType ActionName, string ResourceName = "", bool HadPrefix = false) : base(typeof(PermissionTypeAttribute))
        {
            Arguments = new object[] { ActionName.ToString(), ResourceName, HadPrefix };
        }
    }

    public class PermissionTypeAttribute : Attribute, IActionFilter
    {
        //private UserService _srv;
        public string ResourceName { get; set; }
        public string ActionName { get; set; }
        public bool HadPrefix { get; set; }

        ILogger _Logger;
        private readonly DBContext _context;

        //private Security.AuthenticationManager _authManager;
        #region constructor
        public PermissionTypeAttribute(DBContext context, string actionName, string resourceName, bool hadPrefix)
        {
            _context = context;
            ResourceName = resourceName;
            ActionName = actionName;
            HadPrefix = hadPrefix;

        }
        #endregion

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(ResourceName))
                ResourceName = context.RouteData.Values["controller"].ToString();

            if (this.HadPrefix)
            {
                string prefix = context.HttpContext.Items["ResourcePrefix"].ToString();
                ResourceName = prefix + "_"+ ResourceName;
            }
            context.HttpContext.Items["ResourceName"] = ResourceName;

            var role = this.VaildToken(context);

            var list = _context.RoleActionItems.Where(a => a.Role == role && a.Action == ActionName).ToList();
            if(list.Count < 1)
            {
                this.Throw401(context, "Authencate failed");
            }
        }

        private TicketRole? VaildToken(ActionExecutingContext context)
        {
            string token = null;

            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader != null)
            {
                AuthenticationHeaderValue authorization = AuthenticationHeaderValue.Parse(authHeader);
                if (authorization.Scheme.Equals("Basic"))
                {
                    token = authorization.Parameter;
                }
            }

            try
            {
                if (token != null)
                {
                    var role = (TicketRole)Enum.Parse(typeof(TicketTracking.Models.TicketRole), token);
                    context.HttpContext.Items["role"] = role;
                    return role;
                }
                else
                {
                    this.Throw401(context, "Authencate failed");
                }
            }
            catch (Exception ex)
            {
                this.Throw401(context, "validation failed");
                _Logger.LogError(ex.Message + ", " + ex.StackTrace);
                //throw ex;
            }
            return null;
        }

        private void Throw401(ActionExecutingContext context, string message)
        {
            var jsonResult = new
            {
                StatusCode = 401,
                Status = ActionStatus.Fail.ToString(),
                Message = message
            };

            BadRequestObjectResult forbid = new BadRequestObjectResult(jsonResult);
            forbid.StatusCode = 401;
            //forbid.
            context.Result = forbid;
        }
    }
}
