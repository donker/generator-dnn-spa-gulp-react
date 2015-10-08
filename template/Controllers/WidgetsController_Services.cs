using System.Net;
using System.Net.Http;
using System.Web.Http;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Models.<%= props.widgetName %>s;
using DotNetNuke.Web.Api;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Controllers
{

    public partial class <%= props.widgetName %>sController : <%= props.projectName %>ApiController
    {

        #region Service Methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        [<%= props.projectName %>Authorize(SecurityLevel = SecurityAccessLevel.Pointer)]
        public HttpResponseMessage <%= props.widgetName %>(<%= props.widgetName %>Base postData)
        {
            <%= props.widgetName %> returnData;
            postData.ModuleId = ActiveModule.ModuleID;
            if (postData.<%= props.widgetName %>Id == -1)
            {
                Add<%= props.widgetName %>(ref postData, UserInfo.UserID);
                returnData = Get<%= props.widgetName %>(postData.<%= props.widgetName %>Id, ActiveModule.ModuleID);
            }
            else
            {
                var oldData = Get<%= props.widgetName %>(postData.<%= props.widgetName %>Id, ActiveModule.ModuleID).Get<%= props.widgetName %>Base();
                if (oldData.CreatedByUserID == UserInfo.UserID | Settings.AllowOtherEdit | Security.CanEdit | Security.IsAdmin)
                {
                    oldData.Latitude = postData.Latitude;
                    oldData.Longitude = postData.Longitude;
                    oldData.Message = postData.Message;
                    Update<%= props.widgetName %>(oldData, UserInfo.UserID);
                    returnData = Get<%= props.widgetName %>(postData.<%= props.widgetName %>Id, ActiveModule.ModuleID);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "");                   
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, returnData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [<%= props.projectName %>Authorize(SecurityLevel = SecurityAccessLevel.Pointer)]
        public HttpResponseMessage Delete(int id)
        {
            var <%= props.widgetName %> = Get<%= props.widgetName %>(id, ActiveModule.ModuleID);
            if (<%= props.widgetName %>.CreatedByUserID == UserInfo.UserID | Settings.AllowOtherEdit | Security.CanEdit |
                Security.IsAdmin)
            {
                Delete<%= props.widgetName %>(<%= props.widgetName %>.Get<%= props.widgetName %>Base());
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, <%= props.widgetName %>);
        }

        #endregion

    }
}

