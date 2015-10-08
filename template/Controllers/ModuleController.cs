using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Controllers
{

    public partial class ModuleController : <%= props.projectName %>ApiController
    {

        public class InitData
        {
            public ModuleSettings Settings { get; set; }
            public IEnumerable<<%= props.widgetName %>> <%= props.widgetName %>s { get; set; }
            public ContextSecurity Security { get; set; }
            public Dictionary<string, string> ClientResources { get; set; }
        }

        #region Service Methods
        [HttpGet]
        [<%= props.projectName %>Authorize(SecurityLevel = SecurityAccessLevel.View)]
        public HttpResponseMessage InitialData()
        {
            InitData init = new InitData();
            init.Settings = ModuleSettings.GetSettings(ActiveModule);
            init.Security = new ContextSecurity(ActiveModule);
            init.ClientResources = Localization.GetResourceFile(PortalSettings, ClientResourceFileName,
                System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            return Request.CreateResponse(HttpStatusCode.OK, init);
        }
        #endregion

    }
}

