using Common.Lib;
using ITAdministrationApp.Areas.Admin.Controllers;
using ITAdministrationApp.Areas.Admin.Models;
using ITAdministrationApp.Areas.Admin.Provider;
using ITAdministrationApp.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WebApp.Models;

namespace ITAdministrationApp.Areas.Admin
{
    [Area("Admin")]
    public class ItServiceController : AdminBaseController
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly ITServiceManager _serviceManager;

        public ItServiceController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _serviceManager = new ITServiceManager();
        }

        //public async Task<IActionResult> Index(string search = "")
        //{
        //    var allServices = await _serviceManager.GetAllServices(0, int.MaxValue, search);

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        allServices = allServices
        //            .Where(c => c.ServiceName.Contains(search, StringComparison.OrdinalIgnoreCase))
        //            .ToList();
        //    }

        //    ViewBag.SearchKeyword = search;

        //    return View(allServices);
        //}


        //before adding pagination code

        //public async Task<IActionResult> Index(string search = "", string sortColumn = "ServiceID", string sortOrder = "ASC")
        //{
        //    var allServices = await _serviceManager.GetAllServices(0, int.MaxValue, search, sortColumn, sortOrder);

        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        allServices = allServices
        //            .Where(c => c.ServiceName.Contains(search, StringComparison.OrdinalIgnoreCase))
        //            .ToList();
        //    }

        //    ViewBag.SearchKeyword = search;
        //    ViewBag.SortColumn = sortColumn;
        //    ViewBag.SortOrder = sortOrder;

        //    return View(allServices);
        //}

        //after pagination code
        //public async Task<IActionResult> Index(string search = "", string sortColumn = "ServiceID", string sortOrder = "ASC", int page = 1, int pageSize = 10)
        //{
        //    var allServices = await _serviceManager.GetAllServices(0, int.MaxValue, search, sortColumn, sortOrder);

        //    // Apply search filter if search keyword is provided
        //    if (!string.IsNullOrEmpty(search))
        //    {
        //        allServices = allServices
        //            .Where(c => c.ServiceName.Contains(search, StringComparison.OrdinalIgnoreCase))
        //            .ToList();
        //    }

        //    // Count total number of items
        //    var totalItems = allServices.Count();

        //    // Calculate total pages
        //    var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        //    // Apply pagination
        //    var paginatedServices = allServices.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //    ViewBag.SearchKeyword = search;
        //    ViewBag.SortColumn = sortColumn;
        //    ViewBag.SortOrder = sortOrder;
        //    ViewBag.Page = page;
        //    ViewBag.PageSize = pageSize;
        //    ViewBag.TotalItems = totalItems;
        //    ViewBag.TotalPages = totalPages;

        //    return View(paginatedServices);
        //}

        public async Task<IActionResult> Index(string search = "", string sortColumn = "ServiceID", string sortOrder = "ASC", int page = 1, int pageSize = 5)
        {
            // Fetch all services without applying search filter
            var allServices = await _serviceManager.GetAllServices(0, int.MaxValue, "", sortColumn, sortOrder);

            // Apply search filter if search keyword is provided
            if (!string.IsNullOrWhiteSpace(search))
            {
                allServices = allServices
                    .Where(c => c.ServiceName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Count total number of items
            var totalItems = allServices.Count();

            // Apply pagination
            var paginatedServices = allServices.Skip((page - 1) * pageSize).Take(pageSize).Select((i, index) => new ItServiceModel
            {
                //added to on Wednesday to check row count
                RowNumber = (page - 1) * pageSize + index + 1,
                ServiceID = i.ServiceID,
                ServiceName = i.ServiceName,
                ServiceDescription = i.ServiceDescription,
                ServiceType = i.ServiceType,
                BuyFrom = i.BuyFrom,
                BuyDate = i.BuyDate,
                ExpiredOn = i.ExpiredOn,
                LastPaidDate = i.LastPaidDate,
                PaidAmount = i.PaidAmount,
                UsedInDomains = i.UsedInDomains,
                AddedBy = i.AddedBy,
                AddedOn = i.AddedOn,
                UpdatedBy = i.UpdatedBy,
                UpdatedOn = i.UpdatedOn,
                IsActive = i.IsActive,
                IsDeleted = i.IsDeleted,
                DeletedOn = i.DeletedOn,
                DeletedBy = i.DeletedBy
            })
                .ToList();


            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewBag.SearchKeyword = search;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortOrder = sortOrder;
            ViewBag.Page = page;
             ViewBag.PageSize = pageSize;
            ViewBag.TotalItems = totalItems;
            ViewBag.TotalPages = totalPages;

            return View(paginatedServices);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IsSuccess"] = "False";
            return View(new ItServiceEditModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItServiceEditModel model)
        {
            if (ModelState.IsValid)
            {
                //added in today only, can be removed later
                //model.PaidAmount = null;
                //model.LastPaidDate = null;
                //model.DeletedOn = model.DeletedOn ?? DateTime.UtcNow;
                var result = await _serviceManager.AddUpdate(model, GetUserName);

                if (result.IsSucess)
                {

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ShowActionMessage(string.Join(",", result.ErrorMessage), eMessageType.danger);
                    ModelState.AddModelError(string.Empty, result.ErrorMessage.First());
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ItServiceEditModel model = await _serviceManager.GetServicebyID(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItServiceEditModel model)
        {
            if (ModelState.IsValid)
            {
                ITServiceManager manager = new();
                var result = await manager.AddUpdate(model, GetUserName);

                if (result.IsSucess)
                {

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ShowActionMessage(string.Join(",", result.ErrorMessage), eMessageType.danger);
                }

                ModelState.AddModelError(string.Empty, result.ErrorMessage.First());
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            ITServiceManager manager = new();
            var result = await manager.Delete(id);

            if (result.IsSucess)
            {
                ShowActionMessage(result.Result, eMessageType.success);
                return RedirectToAction(nameof(Index));
            }


            var response = await manager.GetAllServices(0, 10, "");
            return View(nameof(Index), response);
        }
        //public IActionResult ViewPayment(/*int serviceID*/)
        //{
        //    return RedirectToAction("Index", "ServicePayment"/*, new { serviceID = serviceID }*/);
        //}
        public IActionResult ViewPayment(int serviceID)
        {
            return RedirectToAction("Index", "ServicePayment", new { serviceID });
        }

    }
}
