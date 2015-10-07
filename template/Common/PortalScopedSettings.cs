using DotNetNuke.Entities.Portals;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public class PortalScopedSettings : StringBasedSettings
    {
        public PortalScopedSettings(int portalId)
            : base(
                name => PortalController.GetPortalSetting(name, portalId, ""),
                (name, value) => PortalController.UpdatePortalSetting(portalId, name, value, true)
                )
        { }
    }
}