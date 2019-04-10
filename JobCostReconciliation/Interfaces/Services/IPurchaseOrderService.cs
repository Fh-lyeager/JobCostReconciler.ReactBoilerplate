﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobCostReconciliation.Interfaces.Services
{
    public interface IPurchaseOrderService
    {
        DataTable GetSapphireJobPORecords();
        void ReconcilePurchaseOrderDates(string dataObject = "JobPOs");
    }
}
