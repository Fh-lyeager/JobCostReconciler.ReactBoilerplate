using JobCostReconciliation.Interfaces.Services;
using JobCostReconciliation.Interfaces.Repositories;
using JobCostReconciliation.Interfaces.Clients;
using JobCostReconciliation.Model;
using Pervasive.Data.SqlClient;
using System;
using System.Configuration;

namespace JobCostReconciliation.Services
{
    public class PervasiveService : IPervasiveService
    {
        private readonly IPervasiveClient _pervasiveClient;

        public PervasiveService(IPervasiveClient pervasiveClient)
        {
            _pervasiveClient = pervasiveClient;
        }

        public void SavePO(POHeader poHeader)
        {
            var connString = ConfigurationManager.ConnectionStrings["ADONET35"].ToString();
            var queryString = CreateSelectStatement(poHeader);
            
            var fromDb = _pervasiveClient.QueryPervasiveADO(queryString, connString);
            
            if (fromDb == null || fromDb.Rows.Count == 0)
            {
                InsertPo(poHeader);
            }
            else
            {
                UpdatePo(poHeader);
            }
        }

        private string CreateSelectStatement(POHeader poHeader)
        {
            var selectStatement =
                "SELECT * FROM PO_Header" +
                $" WHERE SapphireObjID = {'"'}{poHeader.SapphireObjID}{'"'} " +
                $"AND SapphireObjRID = {poHeader.SapphireObjRID}";

            return selectStatement;
        }

        private void InsertPo(POHeader poHeader)
        {
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["ADONET35"].ToString();
                using (var oConnection = new PsqlConnection(connString))
                {
                    oConnection.Open();
                    var updateCommand = CreateUpdateCommand(oConnection, poHeader);
                    updateCommand.ExecuteNonQuery();
                    oConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdatePo(POHeader poHeader)
        {
            try
            {
                var connString = ConfigurationManager.ConnectionStrings["ADONET35"].ToString();
                using (var oConnection = new PsqlConnection(connString))
                {
                    oConnection.Open();
                    var insertCommand = CreateInsertStatement(oConnection, poHeader);
                    insertCommand.ExecuteNonQuery();
                    oConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CreateCommandParameters(PsqlCommand command, POHeader poHeader)
        {
            command.Parameters.AddWithValue("@po_no", poHeader.po_no);
            command.Parameters.AddWithValue("@NewJobNumber", poHeader.NewJobNumber);
            command.Parameters.AddWithValue("@activity", poHeader.activity);
            command.Parameters.AddWithValue("@vendor_id", poHeader.vendor_id);
            command.Parameters.AddWithValue("@po_type", poHeader.po_type);
            command.Parameters.AddWithValue("@release_date", poHeader.release_date);
            command.Parameters.AddWithValue("@cancelled_date", poHeader.cancelled_date);
            command.Parameters.AddWithValue("@payment_amount", poHeader.payment_amount);
            command.Parameters.AddWithValue("@subtotal", poHeader.subtotal);
            command.Parameters.AddWithValue("@tax", poHeader.tax);
            command.Parameters.AddWithValue("@total", poHeader.total);
            command.Parameters.AddWithValue("@egm_amount", poHeader.egm_amount);
            command.Parameters.AddWithValue("@vpo_yes_no", poHeader.vpo_yes_no);
            command.Parameters.AddWithValue("@UserID", poHeader.UserID);
            command.Parameters.AddWithValue("@LastModifiedDate", poHeader.LastModifiedDate);
            command.Parameters.AddWithValue("@Community", poHeader.Community);
            command.Parameters.AddWithValue("@Product", poHeader.Product);
            command.Parameters.AddWithValue("@Building", poHeader.Building);
            command.Parameters.AddWithValue("@Unit", poHeader.Unit);
            command.Parameters.AddWithValue("@taxable_amount", poHeader.taxable_amount);
            command.Parameters.AddWithValue("@job_no", poHeader.job_no);
            command.Parameters.AddWithValue("@eSubmittalDate", poHeader.eSubmittalDate);
            command.Parameters.AddWithValue("@ApprovePaymentDate", poHeader.ApprovePaymentDate);
            command.Parameters.AddWithValue("@Invoice", poHeader.Invoice);
            command.Parameters.AddWithValue("@TaxRate", poHeader.TaxRate);
            command.Parameters.AddWithValue("@eMeasurementPO", poHeader.eMeasurementPO);
            command.Parameters.AddWithValue("@SapphirePONumber", poHeader.SapphirePONumber);
            command.Parameters.AddWithValue("@SapphireObjID", poHeader.SapphireObjID);
            command.Parameters.AddWithValue("@SapphireObjRID", poHeader.SapphireObjRID);
        }

        private PsqlCommand CreateInsertStatement(PsqlConnection oConnection, POHeader poHeader)
        {
            var commandText = "INSERT INTO PO_Header (po_no, NewJobNumber, activity, vendor_id, po_type, release_date, cancelled_date, payment_amount," +
                              "subtotal, tax, total, egm_amount, vpo_yes_no, UserID, LastModifiedDate, Community, Product, Building, Unit," +
                              "taxable_amount, job_no, eSubmittalDate, ApprovePaymentDate, Invoice, TaxRate, eMeasurementPO, SapphirePONumber, SapphireObjID," +
                              "SapphireObjRID)" +
                              "VALUES (@po_no,@NewJobNumber, @activity, @vendor_id, @po_type, @release_date, @cancelled_date, @payment_amount,@subtotal, @tax," +
                              "@total, @egm_amount,@vpo_yes_no, @UserID, @LastModifiedDate, @Community,@Product, @Building,@Unit, @taxable_amount," +
                              "@job_no, @eSubmittalDate, @ApprovePaymentDate, @Invoice,@TaxRate, @eMeasurementPO, @SapphirePONumber, @SapphireObjID, @SapphireObjRID)";

            var command = new PsqlCommand
            {
                Connection = oConnection,
                CommandText = commandText
            };
            CreateCommandParameters(command, poHeader);
            return command;
        }

        private PsqlCommand CreateUpdateCommand(PsqlConnection oConnection, POHeader poHeader)
        {
            var commandText = "UPDATE PO_Header" +
                " SET po_no = @po_no, NewJobNumber = @NewJobNumber," +
                " activity = @activity, vendor_id = @vendor_id," +
                " po_type = @po_type, release_date = @release_date," +
                " cancelled_date = @cancelled_date, payment_amount = @payment_amount," +
                " subtotal = @subtotal, tax = @tax," +
                " total = @total, egm_amount = @egm_amount," +
                " vpo_yes_no = @vpo_yes_no, UserID = @UserID," +
                " LastModifiedDate = @LastModifiedDate, Community = @Community," +
                " Product = @Product, Building = @Building," +
                " Unit = @Unit, taxable_amount = @taxable_amount," +
                " job_no = @job_no, eSubmittalDate = @eSubmittalDate," +
                " ApprovePaymentDate = @ApprovePaymentDate, Invoice = @Invoice," +
                " TaxRate = @TaxRate, eMeasurementPO = @eMeasurementPO," +
                " SapphirePONumber = @SapphirePONumber, SapphireObjID = @SapphireObjID," +
                " SapphireObjRID = @SapphireObjRID" +
                " WHERE SapphireObjID = @SapphireObjID AND SapphireObjRID = @SapphireObjRID ";

            var command = new PsqlCommand
            {
                Connection = oConnection,
                CommandText = commandText
            };
            CreateCommandParameters(command, poHeader);
            return command;
        }
    }
}
