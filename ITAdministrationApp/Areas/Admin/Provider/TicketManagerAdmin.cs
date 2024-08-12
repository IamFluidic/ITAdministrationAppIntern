using Common.Lib;
using SQLHelper;
using TicketManagement.Models;

namespace ITAdministrationApp.Areas.Admin.Provider
{
    public class TicketManagerAdmin
    {
        public async Task<List<ServiceTicketModel>> GetAllTickets(int offset, int limit, string searchKeyword = "", string SortColumn = "TicketID", string SortOrder = "ASC")
        {
            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>
            {
                new KeyValue("@Offset", offset),
                new KeyValue("@Limit", limit),
                new KeyValue("@searchKeyword", searchKeyword),
                 new KeyValue("@SortColumn", SortColumn),
                  new KeyValue("@SortOrder", SortOrder)
            };
            var thelist = await sqlHelper.ExecuteAsListAsync<ServiceTicketModel>("[dbo].[usp_GetAllServiceTickets]", param);
            return thelist;
        }
        public async Task<bool> UpdateTicketStatus(string Title, int TicketID, string statusID, string Remarks)
        {
            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>
            {
                 new KeyValue("@Title", Title),
                new KeyValue("@TicketID", TicketID),
                 new KeyValue ("@StatusID", statusID),
                 new KeyValue ("@Remarks", Remarks)


            };
            var result = await sqlHelper.ExecuteNonQueryAsync("[dbo].[UpdateServiceTicket]", param);
            return result > 0;
        }
        public async Task<TicketUpdateViewModel> GetTicketById(int ticketId)
        {
            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>
        {
            new KeyValue("@TicketID", ticketId)
        };
            var ticket = await sqlHelper.ExecuteAsObjectAsync<TicketUpdateViewModel>("[dbo].[GetTicketById]", param);
            return ticket;
        }

        public async Task<OperationResponse<string>> AddUpdate(TicketUpdateViewModel model, string UserName)
        {
            OperationResponse<string> response = new OperationResponse<string>();


            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> fetchParam = new List<KeyValue>
              {
            new KeyValue("@TicketID", model.TicketID)
               };

            var previousTicket = await sqlHelper.ExecuteAsObjectAsync<TicketUpdateViewModel>("[dbo].[GetTicketById]", fetchParam);
            int PrevTicketStatusID = previousTicket?.TicketStatusID ?? 0;

            //    //update service ticket paila ko code hoo haii
            //    IList<KeyValue> param = new List<KeyValue>();
            //    { 
            //    param.Add(new KeyValue("@Title", model.Title));
            //    param.Add(new KeyValue("@TicketID", model.TicketID));
            //    param.Add(new KeyValue("@TicketStatusID", model.TicketStatusID));

            //    // param.Add(new KeyValue("@CurrentUserName", model.AdddedBy));
            //};



            //naya code for ale
            // Update the service ticket and log ticket in one stored procedure call
            IList<KeyValue> param = new List<KeyValue>
            {
                new KeyValue("@TicketID", model.TicketID),
                new KeyValue("@Title", model.Title),
                new KeyValue("@StatusID", model.TicketStatusID),
                new KeyValue("@Remarks", model.Remarks)
            };

            int opStatus = await sqlHelper.ExecuteNonQueryAsync("[dbo].[UpdateServiceTicket]", param);
            //response.Result = model.TicketID == 0 ? "Ticket added successfully !" : "Category Updated Successfully !";
            //return response;
            if (opStatus > 0)
            {//log the update activity
                IList<KeyValue> logParam = new List<KeyValue>
                {
                    new KeyValue("@Title", model .Title),
                    new KeyValue("@TicketID", model .TicketID),
                    new KeyValue("@Remarks", model .Remarks),
                    new KeyValue("@CurrentTicketStatusID", model.TicketStatusID),
                    new KeyValue("@PrevTicketStatusID", PrevTicketStatusID),


                };
                await sqlHelper.ExecuteNonQueryAsync("[dbo].[InsertServiceTicketActivityLog]", logParam);
                response.Result = model.TicketID == 0 ? "Ticket added successfully!" : "Ticket updated successfully!";
            }
            else
            {
                response.Result = "Failed to update the ticket.";
            }
            return response;
        }

        //internal async Task<IEnumerable<object>> GetAllTickets(int v, int maxValue, string search, object sortColumn)
        //{
        //    throw new NotImplementedException();
        //}
        //yati china sakxa haii




        //public static async Task<OperationResponse<string>> AddUpdate(ServiceTicketModel model)
        //{
        //    OperationResponse<string> response = new OperationResponse<string>();

        //    SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
        //    IList<KeyValue> param = new List<KeyValue>();

        //    param.Add(new KeyValue("@Title", model.Title));
        //    param.Add(new KeyValue("@Description", model.Description));
        //    param.Add(new KeyValue("@RequestedBy", model.RequestedBy));
        //    param.Add(new KeyValue("@RequestedOn", model.RequestedOn == default ? DateTime.Now : model.RequestedOn)); // Default to current time if not provided
        //    param.Add(new KeyValue("@TicketStatusID", model.TicketStatusID));
        //    param.Add(new KeyValue("@AssignTo", model.AssignTo ?? (object)DBNull.Value)); // Handle null values
        //    param.Add(new KeyValue("@AssignedBy", model.AssignedBy ?? (object)DBNull.Value)); // Handle null values

        //    int opStatus = await sqlHelper.ExecuteNonQueryAsync("[dbo].[UpdateServiceTicket]", param, "@OpStatus");

        //    response.Result = "Service ticket added successfully!";
        //    return response;
        //}
    }
}