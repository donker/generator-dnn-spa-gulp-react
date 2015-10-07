using DotNetNuke.Web.Api;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public class RouteMapper : IServiceRouteMapper
    {

        #region IServiceRouteMapper
        public void RegisterRoutes(IMapRoute routeManager)
        {
            routeManager.MapHttpRoute("<%= props.organization %>/<%= props.projectName %>", "<%= props.organization %><%= props.projectName %>1", "{controller}/{action}", null, null, new[] { "<%= props.organization %>.DNN.Modules.<%= props.projectName %>.Controllers" });
            routeManager.MapHttpRoute("<%= props.organization %>/<%= props.projectName %>", "<%= props.organization %><%= props.projectName %>2", "{controller}/{action}/{id}", null, new { id = "\\d*" }, new[] { "<%= props.organization %>.DNN.Modules.<%= props.projectName %>.Controllers" });
        }
        #endregion

    }
}
