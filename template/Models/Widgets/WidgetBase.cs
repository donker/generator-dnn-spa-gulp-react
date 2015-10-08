
using System;
using System.Runtime.Serialization;
using <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common;
using DotNetNuke.ComponentModel.DataAnnotations;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Models.<%= props.widgetName %>s
{
    [TableName("<%= props.organization %>_<%= props.projectName %>_<%= props.widgetName %>s")]
    [PrimaryKey("<%= props.widgetName %>Id", AutoIncrement = true)]
    [Scope("ModuleId")]
    [DataContract]
    public class <%= props.widgetName %>Base : AuditableEntity
    {

        #region Public Properties
        [DataMember]
        public int <%= props.widgetName %>Id { get; set; }
        [DataMember]
        public int ModuleId { get; set; }
        [DataMember]
        public string Message { get; set; }
        #endregion

        #region Methods
        public void Read<%= props.widgetName %>Base(<%= props.widgetName %>Base <%= props.widgetName %>)
        {
            if (<%= props.widgetName %>.<%= props.widgetName %>Id > -1)
                <%= props.widgetName %>Id = <%= props.widgetName %>.<%= props.widgetName %>Id;

            if (<%= props.widgetName %>.ModuleId > -1)
                ModuleId = <%= props.widgetName %>.ModuleId;

            if (!String.IsNullOrEmpty(<%= props.widgetName %>.Message))
                Message = <%= props.widgetName %>.Message;

        }
        #endregion

    }
}



