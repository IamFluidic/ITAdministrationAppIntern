
using Common.Lib;
using ITAdministrationApp.Areas.Admin;
using ITAdministrationApp.Areas.Admin.Controllers;
using ITAdministrationApp.Areas.Admin.Models;
using ITAdministrationApp.Areas.Client;
using ITAdministrationApp.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Owin.Security.Provider;
using System.Drawing.Printing;

namespace ITAdministrationApp.Areas.Admin.Controllers

{

    [Area("Admin")]
    public class LogTicketController : AdminBaseController

    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly LogTicketManager _LogTicketManager;

        public LogTicketController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            //string connectionString = "Data Source=110.44.116.231,5397;Initial Catalog=ITAdministration;Integrated Security=False;User ID=LTVIW016;Password=?Znbnc8#c0u(6W;MultipleActiveResultSets=True";
            _LogTicketManager = new LogTicketManager();

        }
        public async Task<IActionResult> Index(string searchKeyword = "",string sortColumn= "TicketID", string sortOrder = "ASC", int page =1, int pageSize = 5)
        {

            var allTickets = await _LogTicketManager.GetAllActivities(0, int.MaxValue, searchKeyword,sortColumn, sortOrder);


            if (!string.IsNullOrEmpty(searchKeyword))
            {
                allTickets = allTickets
                .Where(c => c.Title.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }
            // Count total number of items
            var totalItems = allTickets.Count();

            // Apply pagination
            var paginatedTickets = allTickets.Skip((page - 1) * pageSize).Take(pageSize).Select((i, index) => new LogTicketModel
            {

                RowNumber = (page - 1) * pageSize + index + 1,
                //LogID = i.LogID,
                TicketID = i.TicketID,
                Title = i.Title,
                PrevTicketStatusID = i.PrevTicketStatusID,
                CurrentTicketStatusID = i.CurrentTicketStatusID,
                AddedBy = i.AddedBy,
                AddedOn = i.AddedOn,
                Remarks = i.Remarks



            })
                .ToList();

            var totalPages = (int)Math.Ceiling((double)allTickets.Count / pageSize);
            ViewBag.SearchKeyword = searchKeyword;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            //ViewBag.Status = status;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortOrder = sortOrder;
            return View(paginatedTickets);
            //return View(allTickets);

        }
        
    }
   
}







