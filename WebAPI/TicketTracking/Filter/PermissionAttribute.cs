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

        //private Security.AuthenticationManager _authManager;
        #region constructor
        public PermissionTypeAttribute(string actionName, string resourceName, bool hadPrefix)
        {
            ResourceName = resourceName;
            ActionName = actionName;
            HadPrefix = hadPrefix;

        }
        //public PermissionTypeAttribute(Security.AuthenticationManager authManager, UserService srv, string actionName, string resourceName, bool hadPrefix, ILogger<PermissionTypeAttribute> Logger)
        //{
        //    _authManager = authManager;
        //    _srv = srv;
        //    ResourceName = resourceName;
        //    ActionName = actionName;
        //    HadPrefix = hadPrefix;
        //    _Logger = Logger;
        //}
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

            this.VaildToken(context);

            //if (!string.IsNullOrEmpty(ResourceName) && !string.IsNullOrEmpty(ActionName))
            //{
            //    try
            //    {
            //        if (SecurityContextHolder.SecurityContext != null)
            //        {
            //            var actions = _srv.GetResourcePermission(SecurityContextHolder.SecurityContext.CurrentUser.UserAccount, ResourceName);
            //            if (actions.Success)
            //            {
            //                var actionList = actions.Result as List<ResourcePermission>;
            //                var actionData = actionList.Where(x => x.ResourceName == ResourceName && x.ActionName == ActionName && x.Enabled).FirstOrDefault();
            //                if (actionData == null)
            //                    throw new Exception("Access denied (1)");
            //            }
            //            else
            //            {
            //                throw new Exception("Access denied (2)");
            //            }
            //        }                    
            //    }
            //    catch (Exception ex)
            //    {
            //        _Logger.LogWarning("403, "+ex.Message +" : "+ ex.StackTrace);
            //        var jsonResult = new
            //        {
            //            StatusCode = 403,
            //            Status = ActionStatus.Fail.ToString(),
            //            Message = ex.Message
            //        };
            //        BadRequestObjectResult forbid = new BadRequestObjectResult(jsonResult);
            //        forbid.StatusCode = 403;
            //        //forbid.
            //        context.Result = forbid;
            //    }
            //}

        }

        private void VaildToken(ActionExecutingContext context)
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
        }

        private void Throw401(ActionExecutingContext context, string message)
        {
            var jsonResult = new
            {
                StatusCode = 401,
                Status = ActionStatus.Fail.ToString(),
                Message = message
            };
            //UnauthorizedObjectResult unauthorizedObjectResult = new UnauthorizedObjectResult(jsonResult);
            //unauthorizedObjectResult.StatusCode = 401;

            ////filterContext.Result = unauthorizedObjectResult;


            BadRequestObjectResult forbid = new BadRequestObjectResult(jsonResult);
            forbid.StatusCode = 401;
            //forbid.
            context.Result = forbid;
        }
    }
}
