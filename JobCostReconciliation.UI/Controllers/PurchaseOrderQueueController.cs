using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JobCostReconciliation.Model;
using JobCostReconciliation.Services;

namespace JobCostReconciliation.UI.Controllers
{
    [Route("admin/api/[controller]")]
    [ApiController]
    public class PurchaseOrderQueueController : ControllerBase
    {
        // GET: api/PurchaseOrderQueue
        [HttpGet]
        public IEnumerable<PurchaseOrderLastRun> Get()
        {
            PurchaseOrderLastRunService purchaseOrderLastRunService = new PurchaseOrderLastRunService();
            List<Model.PurchaseOrderLastRun> list = purchaseOrderLastRunService.List();

            return Enumerable.Range(1, list.Count-1).Select(i => new Model.PurchaseOrderLastRun
            {
                NextRunId = list[i].NextRunId,
                RunComplete = list[i].RunComplete,
                Status = list[i].Status
            });
        }

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
                purchaseOrderQueueService.GetNewItems().Count
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
                purchaseOrderQueueService.GetErroredItems().Count
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
