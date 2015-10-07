using System.Net;
using System.Net.Http;
using System.Web.Http;
using Connect.DNN.Modules.Map.Common;
using Connect.DNN.Modules.Map.Models.MapPoints;
using DotNetNuke.Web.Api;

namespace Connect.DNN.Modules.Map.Controllers
{

    public partial class MapPointsController : MapApiController
    {

        #region Service Methods
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MapAuthorize(SecurityLevel = SecurityAccessLevel.Pointer)]
        public HttpResponseMessage MapPoint(MapPointBase postData)
        {
            MapPoint returnData;
            postData.ModuleId = ActiveModule.ModuleID;
            if (postData.MapPointId == -1)
            {
                AddMapPoint(ref postData, UserInfo.UserID);
                returnData = GetMapPoint(postData.MapPointId, ActiveModule.ModuleID);
            }
            else
            {
                var oldData = GetMapPoint(postData.MapPointId, ActiveModule.ModuleID).GetMapPointBase();
                if (oldData.CreatedByUserID == UserInfo.UserID | Settings.AllowOtherEdit | Security.CanEdit | Security.IsAdmin)
                {
                    oldData.Latitude = postData.Latitude;
                    oldData.Longitude = postData.Longitude;
                    oldData.Message = postData.Message;
                    UpdateMapPoint(oldData, UserInfo.UserID);
                    returnData = GetMapPoint(postData.MapPointId, ActiveModule.ModuleID);
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
        [MapAuthorize(SecurityLevel = SecurityAccessLevel.Pointer)]
        public HttpResponseMessage Delete(int id)
        {
            var mapPoint = GetMapPoint(id, ActiveModule.ModuleID);
            if (mapPoint.CreatedByUserID == UserInfo.UserID | Settings.AllowOtherEdit | Security.CanEdit |
                Security.IsAdmin)
            {
                DeleteMapPoint(mapPoint.GetMapPointBase());
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized, mapPoint);
        }

        #endregion

    }
}

