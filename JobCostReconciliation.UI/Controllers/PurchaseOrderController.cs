using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using JobCostReconciliation.Services;

namespace JobCostReconciliation.UI.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        // GET: api/PurchaseOrder
        [HttpGet]
        public List<PurchaseOrder> Get()
        {
            try
            {
                PurchaseOrderService _purchaseOrderService = new PurchaseOrderService();
                var dataTable = _purchaseOrderService.GetSapphireJobPORecords();

                List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

                foreach(DataRow dataRow in dataTable.DefaultView)
                {
                    var purchaseOrder = new PurchaseOrder
                    {

                    };
                    purchaseOrders.Add(purchaseOrder);
                }

                return purchaseOrders;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/PurchaseOrder/5
        //[HttpGet("{purchaseOrderNumber}", Name = "Get")]
        //public Workflow Get(string purchaseOrderNumber)
        //{
        //    try
        //    {
        //        //WorkflowService _workflowService = new WorkflowService();
        //        //return _workflowService.GetWorkflow(jobNumber);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public class PurchaseOrder
        {
            public int JobPORID { get; set; }
            public string JobPOID { get; set; }
            public DateTime DateComplByVnd { get; set; }
            public DateTime DateApproved { get; set; }

        }

    }
}
