using System.Runtime.Serialization;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Models.<%= props.widgetName %>s
{

    [TableName("vw_<%= props.organization %>_<%= props.projectName %>_<%= props.widgetName %>s")]
    [PrimaryKey("<%= props.widgetName %>Id", AutoIncrement = true)]
    [Scope("ModuleId")]
    [DataContract]
    public class <%= props.widgetName %> : <%= props.widgetName %>Base
    {

        #region Public Properties
        [DataMember]
        public string CreatedByUser { get; set; }
        [DataMember]
        public string LastModifiedByUser { get; set; }
        #endregion

        #region Public Methods
        public <%= props.widgetName %>Base Get<%= props.widgetName %>Base()
        {
            <%= props.widgetName %>Base res = new <%= props.widgetName %>Base();
            res.CreatedByUserID = CreatedByUserID;
            res.CreatedOnDate = CreatedOnDate;
            res.LastModifiedByUserID = LastModifiedByUserID;
            res.LastModifiedOnDate = LastModifiedOnDate;
            res.<%= props.widgetName %>Id = <%= props.widgetName %>Id;
            res.ModuleId = ModuleId;
            res.Message = Message;
            return res;
        }
        #endregion

    }
}
