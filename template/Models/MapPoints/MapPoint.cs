using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.DNN.Modules.Map.Models.MapPoints
{

    [TableName("vw_Connect_Map_MapPoints")]
    [PrimaryKey("MapPointId", AutoIncrement = true)]
    [Scope("ModuleId")]
    [DataContract]
    public class MapPoint : MapPointBase
    {

        #region Public Properties
        [DataMember]
        public string CreatedByUser { get; set; }
        [DataMember]
        public string LastModifiedByUser { get; set; }
        #endregion

        #region Public Methods
        public MapPointBase GetMapPointBase()
        {
            MapPointBase res = new MapPointBase();
            res.CreatedByUserID = CreatedByUserID;
            res.CreatedOnDate = CreatedOnDate;
            res.LastModifiedByUserID = LastModifiedByUserID;
            res.LastModifiedOnDate = LastModifiedOnDate;
            res.MapPointId = MapPointId;
            res.ModuleId = ModuleId;
            res.Latitude = Latitude;
            res.Longitude = Longitude;
            res.Message = Message;
            return res;
        }
        #endregion

    }
}
