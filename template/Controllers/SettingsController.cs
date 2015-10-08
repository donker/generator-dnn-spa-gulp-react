using System.Net;
using System.Net.Http;
using System.Web.Http;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common;
using DotNetNuke.Web.Api;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Controllers
{

    public partial class SettingsController : <%= props.projectName %>ApiController
    {
        public class SettingsDTO
        {
            public int MySetting { get; set; }
        }


        #region Service Methods
        [HttpPost]
        [MapAuthorize(SecurityLevel = SecurityAccessLevel.Edit)]
        [ValidateAntiForgeryToken]
        public HttpResponseMessage Update(SettingsDTO newSettings)
        {
            var oldSettings = ModuleSettings.GetSettings(ActiveModule);
            oldSettings.MySetting = newSettings.MySetting;
            oldSettings.SaveSettings();
            return Request.CreateResponse(HttpStatusCode.OK, oldSettings);
        }
        #endregion

    }
}

