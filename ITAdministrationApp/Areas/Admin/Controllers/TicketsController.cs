using ITAdministrationApp.Areas.Admin.Models;
using ITAdministrationApp.Areas.Admin.Provider;
using ITAdministrationApp.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.Models;
using WebApp.Models;

namespace ITAdministrationApp.Areas.Admin.Controllers
{
    [Area("admin")]
    public class TicketsController : AdminBaseController
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly TicketManagerAdmin _TicketManager;
        private readonly ILogger<TicketsController> _logger;

        public TicketsController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, ILogger<TicketsController> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _TicketManager = new TicketManagerAdmin();
            _logger = logger;
        }
       
        public async Task<IActionResult> Index(string search = "", string status = "", string SortColumn = "TicketID", string SortOrder = "ASC", int page = 1, int pageSize = 5)
        {
            _logger.LogInformation("Loading Index view with search: {Search}", search);

            var allTickets = await _TicketManager.GetAllTickets(0, int.MaxValue, search, SortColumn, SortOrder);


            if (!string.IsNullOrEmpty(search))
            {
                allTickets = allTickets
                .Where(c => c.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                allTickets = allTickets
                    .Where(c => c.TicketStatusName.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

           
            var totalItems = allTickets.Count();

            // Apply pagination
            var paginatedTickets = allTickets.Skip((page - 1) * pageSize).Take(pageSize).Select((i, index) => new ServiceTicketModel
            {
                //added to on Wednesday to check row count
                RowNumber = (page - 1) * pageSize + index + 1,
                TicketID = i.TicketID,
                Title = i.Title,
                Description = i.Description,
                RequestedBy = i.RequestedBy,
                RequestedOn = i.RequestedOn,
                AssignedBy = i.AssignedBy,
                TicketStatusID = i.TicketStatusID,

                AssignTo = i.AssignTo,
                LastUpdated = i.LastUpdated,
            })
                .ToList();


            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.SearchKeyword = search;
            ViewBag.SortColumn = SortColumn;
            ViewBag.SortOrder = SortOrder;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;

            return View(paginatedTickets);
        }

    //      
    //        }
    [HttpGet]
        public async Task<IActionResult> Update(int TicketID)
        {
          TicketUpdateViewModel model = await _TicketManager.GetTicketById(TicketID);

            
            if (model == null)
            {
               return NotFound();
            }

           

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Update(TicketUpdateViewModel model)
        {
            _logger.LogInformation("Updating ticket with ID: {TicketID}", model.TicketID);

            if (ModelState.IsValid)
            {

                var Result = await _TicketManager.UpdateTicketStatus(model.Title, model.TicketID, model.TicketStatusID.ToString(), model.Remarks);

                if (Result)
                {
                    _logger.LogInformation("Successfully updated ticket with ID: {TicketID}", model.TicketID);
                    return RedirectToAction(nameof(Index));
                    //return View(model);
                }
                _logger.LogError("Failed to update ticket with ID: {TicketID}", model.TicketID);
                ModelState.AddModelError(string.Empty, "An error occurred while updating the ticket status.");
            }
            else
            {
                _logger.LogWarning("ModelState is invalid for ticket with ID: {TicketID}", model.TicketID);
            }

            
            return RedirectToAction("Index");
            //return View(model);
        }

        
    }
}


