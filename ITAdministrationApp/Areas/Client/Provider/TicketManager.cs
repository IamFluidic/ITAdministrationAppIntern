using Common.Lib;
using SQLHelper;
using TicketManagement.Models;

public class TicketManager
{
    public async Task<OperationResponse<string>> CreateTicket(ServiceTicketModel model, string username)
    {
        OperationResponse<string> response = new OperationResponse<string>();

        SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
        IList<KeyValue> param = new List<KeyValue>();
        {
            param.Add(new KeyValue("@TicketID", model.TicketID));
            param.Add(new KeyValue("@Title", model.Title));
            param.Add(new KeyValue("@Description", model.Description));
            param.Add(new KeyValue("@CurrentUsername", username));
            param.Add(new KeyValue("@RequestedBy", model.RequestedBy));
            param.Add(new KeyValue("@RequestedOn", model.RequestedOn == default ? DateTime.Now : model.RequestedOn));
            param.Add(new KeyValue("@TicketStatusID", model.TicketStatusID));
            param.Add(new KeyValue("@AssignTo", model.AssignTo ?? (object)DBNull.Value));
            param.Add(new KeyValue("@AssignedBy", model.AssignedBy ?? (object)DBNull.Value));
        };

        int opStatus = await sqlHelper.ExecuteNonQueryAsync("[dbo].[usp_AddServiceTicket]", param, "@OpStatus");

        response.Result = model.TicketID == 0 ? "Service Created Successfully !" : "Service Updated Successfully !";
        return response;
    }
    //public async Task<string> GetUsernameByLoginIDAsync(string loginID)
    //{
    //    SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
    //    IList<KeyValue> param = new List<KeyValue>
    //    {
    //        new KeyValue("@LoginID", loginID)
    //    };
    //    var username = await sqlHelper.ExecuteAsScalarAsync<string>("[dbo].[GetUsernameByLoginID]", param);
    //    return username;
    //}


    public async Task<List<ServiceTicketModel>> GetAllTickets(int offset, int limit, string searchKeyword = "", string sortColumn = null, string sortOrder = null)
    {
        SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
        IList<KeyValue> param = new List<KeyValue>
        {
            new KeyValue("@Offset", offset),
            new KeyValue("@Limit", limit),
            new KeyValue("@searchKeyword", searchKeyword)
        };
        var thelist = await sqlHelper.ExecuteAsListAsync<ServiceTicketModel>("[dbo].[usp_GetAllServiceTickets]", param);
        return thelist;
    }
}
