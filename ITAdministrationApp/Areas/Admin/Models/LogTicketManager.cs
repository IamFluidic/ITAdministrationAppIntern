
using Common.Lib;
using ITAdministrationApp.Areas.Admin.Models;
using Microsoft.Data.SqlClient;
using SQLHelper;


namespace ITAdministrationApp.Areas.Admin.Models
{
    public class LogTicketManager
    {
        public async Task<List<LogTicketModel>> GetAllActivities(int offset, int limit, string searchKeyword = "", string sortColumn = "TicketID", string sortOrder = "ASC")
        {
            SQLHandlerAsync sqlHelper = new SQLHandlerAsync();
            IList<KeyValue> param = new List<KeyValue>
    {
        //new KeyValue("@LogID",),
        new KeyValue("@Offset", offset),
        new KeyValue("@Limit", limit),
        new KeyValue("@searchKeyword", searchKeyword),
        new KeyValue("@SortColumn", sortColumn),
        new KeyValue("@SortOrder", sortOrder)
    };
            var TheList = await sqlHelper.ExecuteAsListAsync<LogTicketModel>("[dbo].[GetAllActivities]", param);
            return TheList;



        }

    }

}

