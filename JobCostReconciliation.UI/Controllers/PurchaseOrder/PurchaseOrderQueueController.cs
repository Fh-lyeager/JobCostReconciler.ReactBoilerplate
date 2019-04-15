using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobCostReconciliation.Model;
using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Services;

namespace JobCostReconciliation.UI.Controllers.PurchaseOrder
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class PurchaseOrderQueueController : ControllerBase
    {
        //// GET: api/PurchaseOrderQueue
        //[HttpGet]
        //public IEnumerable<PurchaseOrderQueue> Get()
        //{
        //    return new List<PurchaseOrderQueue> { };
        //}

        //// GET: api/PurchaseOrderQueue/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // GET: api/PurchaseOrderQueue/LastRun
        [HttpGet("[action]")]
        public string[] LastRun()
        {
            PurchaseOrderProcessorNextRun purchaseOrderProcessorNextRun = new PurchaseOrderProcessorNextRun();
            string[] lastRunArray = new string[]
            {
                purchaseOrderProcessorNextRun.LastRun.ToString("MM/dd/yyyy h:mm:ss")
            };

            return lastRunArray;
        }

        // GET: api/PurchaseOrderQueue/NextRun
        [HttpGet("[action]")]
        public string[] NextRun()
        {
            PurchaseOrderProcessorNextRun purchaseOrderProcessorNextRun = new PurchaseOrderProcessorNextRun();
            string[] nextRunArray = new string[]
            {
                purchaseOrderProcessorNextRun.NextRun.ToString("M/d/yyyy hh:mm:ss")
            };

            return nextRunArray;
        }

        // GET: api/PurchaseOrderQueue/ItemsInQueue
        [HttpGet("[action]")]
        public int[] ItemsInQueue()
        {
            PurchaseOrderQueueService purchaseOrderQueueService = new PurchaseOrderQueueService();

            int[] itemsInQueue = new int[]
            {
                purchaseOrderQueueService.ItemsInQueue()
            };

            return itemsInQueue;
        }

        // GET: api/PurchaseOrderQueue/FailedRecords
        [HttpGet("[action]")]
        public int[] FailedRecords()
        {
            PurchaseOrderQueueService purchaseOrderQueueService = new PurchaseOrderQueueService();

            int[] failedRecords = new int[]
            {
                purchaseOrderQueueService.CountErroredItems()
            };

            return failedRecords;
        }

        public class PurchaseOrderProcessorNextRun
        {
            public DateTime LastRun
            {
                get
                {
                    PurchaseOrderLastRunService purchaseOrderLastRunService = new PurchaseOrderLastRunService();
                    return purchaseOrderLastRunService.GetLastRun();
                }
            }
            public DateTime NextRun
            {
                get
                {
                    return LastRun.AddMinutes(30);
                }
            }
        }
    }
}
