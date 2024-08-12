
using ITAdministrationApp.Areas.Admin.Models;
using ITAdministrationApp.Areas.Admin.Provider;
using ITAdministrationApp.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace ITAdministrationApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicePaymentController : AdminBaseController
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly ServicePaymentManager _servicePaymentManager;

        public ServicePaymentController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _servicePaymentManager = new ServicePaymentManager();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int serviceID, string search = "", string sortColumn = "LogID", string sortOrder = "ASC", int page = 1, int pageSize = 5)
        {
            //var offset = (page - 1) * pageSize;
            var allPayments = await _servicePaymentManager.GetAllServicePayments(serviceID, 0, int.MaxValue, /*offset,*/ "", /*pageSize, search,*/ sortColumn, sortOrder);

            if (!string.IsNullOrWhiteSpace(search))
            {
                allPayments = allPayments
                    .Where(p => p.PaidBy.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var totalItems = allPayments.Count();
            var paginatedPayments = allPayments.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            


            //calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.ServiceID = serviceID;

            ViewBag.SearchKeyword = search;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;

            return View(paginatedPayments);
        }

        [HttpGet]
        public IActionResult Create(int serviceID)
        {
            ViewData["IsSuccess"] = "False";
            ViewBag.ServiceID = serviceID;
            return View(new ServicePaymentAddModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicePaymentAddModel model, int serviceID)
        {
            //if (ModelState.IsValid)
            //{
                var result = await _servicePaymentManager.AddPayment(model, GetUserName, serviceID);

                ////Update After Payment today
                //var serviceUpdateResult = await _servicePaymentManager.UpdateServiceAfterPayment(serviceID, model.PaidAmount, DateTime.Now);

                if (result.IsSucess)
                {
                    return RedirectToAction(nameof(Index), new { serviceID });
                }
                else
                {
                    ShowActionMessage(string.Join(",", result.ErrorMessage), eMessageType.danger);
                    ModelState.AddModelError(string.Empty, result.ErrorMessage.First());
                }
            //}
            ViewBag.ServiceID = serviceID;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int serviceID)
        {
            ServicePaymentManager manager = new();
            var result = await manager.Delete(id);

            if (result.IsSucess)
            {
                //ShowActionMessage(result.Result, eMessageType.success);
                return RedirectToAction(nameof(Index), new {serviceID});
            }

            else
            {
                // Deletion failed
                ModelState.AddModelError(string.Empty, "Failed to delete payment.");
                // You might want to handle this error differently, such as displaying a message to the user.
                return RedirectToAction(nameof(Index), new { serviceID });
            }
            //var response = await manager.GetAllServicePayments( serviceID, 0, 10, "");
            ////return View(nameof(Index), response);
            //return RedirectToAction(nameof(Index), new { serviceID });
        }
    }
}


