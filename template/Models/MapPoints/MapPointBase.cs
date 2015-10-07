
using System;
using System.Runtime.Serialization;
using Connect.DNN.Modules.Map.Common;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace Connect.DNN.Modules.Map.Models.MapPoints
{
    [TableName("Connect_Map_MapPoints")]
    [PrimaryKey("MapPointId", AutoIncrement = true)]
    [Scope("ModuleId")]
    [DataContract]
    public class MapPointBase : AuditableEntity
    {

        #region Public Properties
        [DataMember]
        public int MapPointId { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
        [DataMember]
        public string Message { get; set; }
        #endregion

        #region Methods
        public void ReadMapPointBase(MapPointBase mapPoint)
        {
            if (mapPoint.MapPointId > -1)
                MapPointId = mapPoint.MapPointId;

            if (mapPoint.ModuleId > -1)
                ModuleId = mapPoint.ModuleId;

            Latitude = mapPoint.Latitude;

            Longitude = mapPoint.Longitude;

            if (!String.IsNullOrEmpty(mapPoint.Message))
                Message = mapPoint.Message;

        }
        #endregion

    }
}



