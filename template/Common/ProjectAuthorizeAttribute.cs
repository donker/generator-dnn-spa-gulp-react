using DotNetNuke.Common;
using DotNetNuke.Entities.Users;
using DotNetNuke.Web.Api;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public enum SecurityAccessLevel
    {
        Anonymous = 0,
        View = 1,
        Edit = 2,
        Admin = 3,
        Host = 4
    }

    public class <%= props.projectName %>AuthorizeAttribute : AuthorizeAttributeBase, IOverrideDefaultAuthLevel
    {
        public SecurityAccessLevel SecurityLevel { get; set; }
        public UserInfo User { get; set; }

        public <%= props.projectName %>AuthorizeAttribute()
        {
            SecurityLevel = SecurityAccessLevel.Admin;
        }

        public <%= props.projectName %>AuthorizeAttribute(SecurityAccessLevel accessLevel)
        {
            SecurityLevel = accessLevel;
        }

        public override bool IsAuthorized(AuthFilterContext context)
        {
            if (SecurityLevel == SecurityAccessLevel.Anonymous)
            {
                return true;
            }
            User = HttpContextSource.Current.Request.IsAuthenticated ? UserController.Instance.GetCurrentUserInfo() : new UserInfo();
            ContextSecurity security = new ContextSecurity(context.ActionContext.Request.FindModuleInfo());
            switch (SecurityLevel)
            {
                case SecurityAccessLevel.Host:
                    return User.IsSuperUser;
                case SecurityAccessLevel.Admin:
                    return security.IsAdmin | User.IsSuperUser;
                case SecurityAccessLevel.Edit:
                    return security.CanEdit | security.IsAdmin | User.IsSuperUser;
                case SecurityAccessLevel.View:
                    return security.CanView;
            }

            return false;
        }
    }
}