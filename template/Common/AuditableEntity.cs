using System;
using System.Data;
using System.Runtime.Serialization;
using DotNetNuke.Common.Utilities;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{

    [DataContract]
    public abstract class AuditableEntity
	{

		public void FillAuditFields(IDataReader dr)
		{
			CreatedByUserID = Convert.ToInt32(Null.SetNull(dr["CreatedByUserID"], CreatedByUserID));
			CreatedOnDate = Convert.ToDateTime(Null.SetNull(dr["CreatedOnDate"], CreatedOnDate));
			LastModifiedByUserID = Convert.ToInt32(Null.SetNull(dr["LastModifiedByUserID"], LastModifiedByUserID));
			LastModifiedOnDate = Convert.ToDateTime(Null.SetNull(dr["LastModifiedOnDate"], LastModifiedOnDate));
		}

		#region Public Properties
        [DataMember]
        public int CreatedByUserID { get; set; }
		[DataMember]
		public DateTime CreatedOnDate { get; set; }
        [DataMember]
        public int LastModifiedByUserID { get; set; }
		[DataMember]
		public DateTime LastModifiedOnDate { get; set; }
		#endregion


	}
}

