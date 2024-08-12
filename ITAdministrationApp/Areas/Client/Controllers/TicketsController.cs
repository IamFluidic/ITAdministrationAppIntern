using ITAdministrationApp.Base;
using ITAdministrationApp.Models;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Models;
using WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using ITAdministrationApp.Services;

namespace ITAdministrationApp.Areas.Client.Controllers
{
    [Area("Client")]
    public class TicketsController : AdminBaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        //private readonly TicketService  _ticketService;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(IWebHostEnvironment hostingEnvironment,/* TicketService ticketService*/ ILogger<TicketsController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            //_ticketService = ticketService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewData["IsSuccess"] = "False";
            return View(new ServiceTicketModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ServiceTicketModel Model)
        {
            Model.RequestedOn = DateTime.Now;
            Model.RequestedBy = GetUserName;
            Model.AssignedBy = "Admin";
            //if (ModelState.IsValid)
            //{
                _logger.LogInformation("Model state is valid. Proceeding to create/update service ticket.");
                var ticketManager = new TicketManager();

                var result = await ticketManager.CreateTicket(Model, GetUserName); ; // Use class name to call static method


                // Correct the condition to check if result.Result is a success message string
                if (result != null && !string.IsNullOrEmpty(result.Result))
                {


                    return RedirectToAction("Index", "Tickets", new { search = "", sortColumn = "TicketId", sortOrder = "ASC", page = 1, pageSize = 5 });
                }
                else
                {
                    var errorMessages = result?.ErrorMessage ?? new List<string> { "An error occurred while creating the ticket." };
                    ShowActionMessage(string.Join(",", errorMessages), eMessageType.error);
                    ModelState.AddModelError(string.Empty, errorMessages.First());
                    _logger.LogError("Error creating ticket: {Errors}", string.Join(", ", errorMessages));
                }
            //}
            //else
            //{
            //    foreach (var state in ModelState)
            //    {
            //        foreach (var error in state.Value.Errors)
            //        {
            //            _logger.LogError("Model validation error on {Property}: {Error}", state.Key, error.ErrorMessage);
            //        }
            //    }
            //}


            return View(Model);
        }

        [HttpGet]
        public IActionResult ActivityLog()
        {
            return View();
        }

        public async Task<IActionResult> Index(string search = "", string sortColumn = "TicketId", string sortOrder = "ASC", int page = 1, int pageSize = 5)
        {
            var ticketManager = new TicketManager();
            var allTickets = await ticketManager.GetAllTickets(0, int.MaxValue, search);
            //might be cause issue
            if (!string.IsNullOrWhiteSpace(search))
            {
                allTickets = allTickets
                    .Where(c => c.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var totalItems = allTickets.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var paginatedServices = allTickets.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.SearchKeyword = search;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;

            return View(paginatedServices);
        }
    }
}
