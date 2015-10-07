using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security;
using DotNetNuke.Security.Permissions;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public class ContextSecurity
    {
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool IsAdmin { get; set; }
        public int UserId { get; set; }

        #region ctor
        public ContextSecurity(ModuleInfo objModule)
        {
            UserId = UserController.Instance.GetCurrentUserInfo().UserID;
            CanView = ModulePermissionController.CanViewModule(objModule);
            CanEdit = ModulePermissionController.HasModulePermission(objModule.ModulePermissions, "EDIT");
            IsAdmin = PortalSecurity.IsInRole(PortalSettings.Current.AdministratorRoleName);
        }
        #endregion

    }
}