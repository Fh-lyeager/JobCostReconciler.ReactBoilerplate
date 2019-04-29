using System;
using Newtonsoft.Json.Linq;

namespace JobCostReconciliation.Model
{
    public class SapphireWorkflowRequestObject
    {
        private int _refObjRID;
        private string _refObjType;
        private UInt64 _timeStamp;
        private int _wFlowRID;

        public int RefObjRID { get => _refObjRID; set => _refObjRID = (value == null || value == 0) ? 0 : value; }
        public string RefObjType { get => _refObjType; set => _refObjType = String.IsNullOrEmpty(value) ? "" : value; }
        public UInt64 TimeStamp { get => _timeStamp; set => _timeStamp = value; }
        public int WFlowRID { get => _wFlowRID; set => _wFlowRID = value; }

        public SapphireWorkflowRequestObject()
        {

        }

        public SapphireWorkflowRequestObject(string requestBody)
        {
            //if (requestBody is null)
            //{
            //    throw new InvalidSapphireWorkflowRequestObjectException();
            //}

            JObject jObject = JObject.Parse(requestBody);

            var refObjRID = int.TryParse(jObject["RefObjRID"].Value<string>(), out int resultRefObjRID) ? resultRefObjRID : 0;
            var refObjType = jObject["RefObjType"].Value<string>();
            var timeStamp = UInt64.TryParse(jObject["TimeStamp"].Value<UInt64>().ToString(), out ulong resultTimeStamp) ? resultTimeStamp : 0;
            var wFlowRID = int.TryParse(jObject["WFlowRID"].Value<string>(), out int resultWFlowRID) ? resultWFlowRID : 0;

            this.RefObjRID = refObjRID;
            this.RefObjType = refObjType;
            this.TimeStamp = timeStamp;
            this.WFlowRID = wFlowRID;
        }

        public SapphireWorkflowRequestObject(int refObjRID, string refObjType, int wFlowRID)
        {
            this.RefObjRID = refObjRID;
            this.RefObjType = refObjType;
            this.WFlowRID = wFlowRID;
        }
    }
}
