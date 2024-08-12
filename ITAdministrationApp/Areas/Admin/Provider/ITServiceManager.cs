using Common.Lib;
using ITAdministrationApp.Areas.Admin.Controllers;
using ITAdministrationApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using SQLHelper;
using System.Drawing;
using WebApp.Models;
namespace ITAdministrationApp.Areas.Admin.Provider
{
    public class ITServiceManager
    {
        //public async Task<List<ItServiceModel>> GetAllServices(int offset, int limit, string searchKeyword = "")
        //{
        //    SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
        //    IList<KeyValue> param = new List<KeyValue>();
        //    param.Add(new KeyValue("@Offset", offset));
        //    param.Add(new KeyValue("@Limit", limit));
        //    param.Add(new KeyValue("@searchKeyword", searchKeyword));
        //    var thelist = await sqlHelper.ExecuteAsListAsync<ItServiceModel>("[dbo].[GetAllITServices]", param);
        //    return thelist;
        //}

        public async Task<List<ItServiceModel>> GetAllServices(int offset, int limit, string searchKeyword = "", string sortColumn = "ServiceID", string sortOrder = "ASC")
        {
            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>
            {
                new KeyValue("@Offset", offset),
                new KeyValue("@Limit", limit),
                new KeyValue("@searchKeyword", searchKeyword),
                new KeyValue("@SortColumn", sortColumn),
                new KeyValue("@SortOrder", sortOrder)
            };
            var thelist = await sqlHelper.ExecuteAsListAsync<ItServiceModel>("[dbo].[GetAllITServices]", param);
            return thelist;


        }

        public async Task<OperationResponse<string>> AddUpdate(ItServiceEditModel model, string username)
        {
            OperationResponse<string> response = new OperationResponse<string>();
            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>();

            param.Add(new KeyValue("@ServiceID", model.ServiceID));
            param.Add(new KeyValue("@ServiceName", model.ServiceName));
            param.Add(new KeyValue("@ServiceDescription", model.ServiceDescription));
            param.Add(new KeyValue("@BuyFrom", model.BuyFrom));
            //param.Add(new KeyValue("@BuyDate", model.BuyDate));
            param.Add(new KeyValue("@ExpiredOn", model.ExpiredOn));
            //param.Add(new KeyValue("@LastPaidDate", model.LastPaidDate));
            //param.Add(new KeyValue("@PaidAmount", model.PaidAmount));
            param.Add(new KeyValue("@UsedInDomains", model.UsedInDomains));
            param.Add(new KeyValue("@ServiceType", model.ServiceType));
            param.Add(new KeyValue("@CurrentUsername", username));
            param.Add(new KeyValue("@IsActive", model.IsActive));
            param.Add(new KeyValue("@IsDeleted", model.IsDeleted));
            //param.Add(new KeyValue("@PayStatus", model.ItStatus));
            // Removed DeletedOn and DeletedBy from the parameters since they're not used for updates

            int opStatus = await sqlHelper.ExecuteNonQueryAsync("[dbo].[CreateUpdateITService]", param, "@OpStatus");
            response.Result = model.ServiceID == 0 ? "Service Created Successfully !" : "Service Updated Successfully !";
            return response;
        }

        public async Task<ItServiceEditModel> GetServicebyID(int id)
        {
            SQLHandlerAsync handlerAsync = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>();
            param.Add(new KeyValue("@ServiceID", id));
            var datamodel = await handlerAsync.ExecuteAsObjectAsync<ItServiceEditModel>("[dbo].[GetITServiceByID]", param);
            return datamodel;
        }

        public async Task<OperationResponse<string>> Delete(int id)
        {
            OperationResponse<string> response = new OperationResponse<string>();
            SQLHandlerAsync handlerAsync = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>();
            param.Add(new KeyValue("@ServiceID", id));
            await handlerAsync.ExecuteNonQueryAsync("[dbo].[DeleteITService]", param);
            response.Result = "Deleted suucessfully!";
            return response;
        }

        //can be deleted
        public async Task<OperationResponse<string>> UpdateServicePaymentInfo(int serviceID, decimal paidAmount, bool isactive /*, string vendor*/ /*, DateTime? lastPaidDate*/)
        {
            OperationResponse<string> response = new OperationResponse<string>();
            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>();


            param.Add(new KeyValue("@ServiceID", serviceID));
            param.Add(new KeyValue("@PaidAmount", paidAmount));
            //for status
            param.Add(new KeyValue("@IsActive", isactive));
            //param.Add(new KeyValue("@Vendor", vendor));
            //new KeyValue("@LastPaidDate", lastPaidDate)


            int opStatus = await sqlHelper.ExecuteNonQueryAsync("[dbo].[UpdateServicePaymentInfo]", param, "@OpStatus");
            response.Result = opStatus == 0 ? "Service Updated Successfully !" : "Service Created Successfully !"; // Corrected success message
            return response;
        }
    }
}
