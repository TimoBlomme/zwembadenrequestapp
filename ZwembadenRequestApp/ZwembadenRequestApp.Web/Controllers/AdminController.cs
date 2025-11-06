using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZwembadenRequestApp.Core.Entities;
using ZwembadenRequestApp.Core.Interfaces;
using ZwembadenRequestApp.Web.Models;
using ZwembadenRequestApp.Web.Services;
using System.Collections.Generic;

namespace ZwembadenRequestApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IQuoteRequestService _quoteRequestService;

        public AdminController(IQuoteRequestService quoteRequestService)
        {
            _quoteRequestService = quoteRequestService;
        }

        public async Task<ActionResult> Index()
        {
            IEnumerable<QuoteRequest> quoteRequests = await _quoteRequestService.GetAllAsync();
            return View(quoteRequests);
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                QuoteRequest quoteRequest = await _quoteRequestService.GetByIdAsync(id);
                if (quoteRequest == null)
                {
                    return HttpNotFound();
                }

                AdminQuoteViewModel model = new AdminQuoteViewModel
                {
                    Id = quoteRequest.Id,
                    CustomerName = quoteRequest.CustomerName,
                    Configuration = quoteRequest.Configuration,
                    Status = quoteRequest.Status,
                    ProposedPrice = quoteRequest.ProposedPrice,
                    AdminNotes = quoteRequest.AdminNotes,
                    RequestDate = quoteRequest.RequestDate,
                    ResponseDate = quoteRequest.ResponseDate,
                    // Include all the original quote details for reference
                    PoolType = quoteRequest.PoolType,
                    Length = quoteRequest.Length,
                    Width = quoteRequest.Width,
                    Depth = quoteRequest.Depth,
                    NumberOfLights = quoteRequest.NumberOfLights,
                    HasStairs = quoteRequest.HasStairs,
                    AdditionalNotes = quoteRequest.AdditionalNotes
                };

                ViewBag.StatusList = new SelectList(new[]
                {
                    new { Value = QuoteStatus.New, Text = "New" },
                    new { Value = QuoteStatus.InProgress, Text = "In Progress" },
                    new { Value = QuoteStatus.Done, Text = "Done" }
                }, "Value", "Text", model.Status);

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Er is een fout opgetreden bij het ophalen van de offerte.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AdminQuoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _quoteRequestService.ProcessAdminResponseAsync(
                        model.Id, model.Status, model.ProposedPrice, model.AdminNotes);

                    TempData["Success"] = "Offerte aanvraag succesvol bijgewerkt!";
                    return RedirectToAction("Index");
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Er is een fout opgetreden bij het bijwerken van de offerte.";
                    return RedirectToAction("Index");
                }
            }

            // If we got this far, something failed; redisplay form with status list
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = QuoteStatus.New, Text = "New" },
                new { Value = QuoteStatus.InProgress, Text = "In Progress" },
                new { Value = QuoteStatus.Done, Text = "Done" }
            }, "Value", "Text", model.Status);

            return View(model);
        }

        // Additional functionality for filtering and sorting
        public async Task<ActionResult> Filter(string status, string sortBy = "RequestDate", bool ascending = false)
        {
            try
            {
                IEnumerable<QuoteRequest> allRequests = await _quoteRequestService.GetAllAsync();

                // Filter by status if provided
                if (!string.IsNullOrEmpty(status) && status != "All")
                {
                    allRequests = allRequests.Where(q => q.Status == status);
                }

                // Sort the results
                switch (sortBy.ToLower())
                {
                    case "customername":
                        allRequests = ascending ?
                            allRequests.OrderBy(q => q.CustomerName) :
                            allRequests.OrderByDescending(q => q.CustomerName);
                        break;
                    case "requestdate":
                        allRequests = ascending ?
                            allRequests.OrderBy(q => q.RequestDate) :
                            allRequests.OrderByDescending(q => q.RequestDate);
                        break;
                    case "status":
                        allRequests = ascending ?
                            allRequests.OrderBy(q => q.Status) :
                            allRequests.OrderByDescending(q => q.Status);
                        break;
                    default:
                        allRequests = allRequests.OrderByDescending(q => q.RequestDate);
                        break;
                }

                ViewBag.CurrentStatus = status;
                ViewBag.SortBy = sortBy;
                ViewBag.Ascending = ascending;

                return View("Index", allRequests);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Er is een fout opgetreden bij het filteren van de offertes.";
                return RedirectToAction("Index");
            }
        }

        // Quick status update actions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateStatus(int id, string status)
        {
            try
            {
                await _quoteRequestService.ProcessAdminResponseAsync(id, status, null, null);
                TempData["Success"] = $"Status succesvol gewijzigd naar {status}";
            }
            catch (ArgumentException ex)
            {
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Er is een fout opgetreden bij het wijzigen van de status.";
            }

            return RedirectToAction("Index");
        }
    }
}