using DotNetNuke.Web.Api;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public abstract class <%= props.projectName %>ApiController : DnnApiController
    {
        private ModuleSettings _settings;
        public ModuleSettings Settings
        {
            get { return _settings ?? (_settings = ModuleSettings.GetSettings(ActiveModule)); }
            set { _settings = value; }
        }

        private ContextSecurity _security;
        public ContextSecurity Security
        {
            get { return _security ?? (_security = new ContextSecurity(ActiveModule)); }
            set { _security = value; }
        }

    }
}