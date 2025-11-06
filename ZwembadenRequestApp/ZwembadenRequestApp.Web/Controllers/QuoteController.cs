using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZwembadenRequestApp.Core.Entities;
using ZwembadenRequestApp.Core.Interfaces;
using ZwembadenRequestApp.Web.Models;
using ZwembadenRequestApp.Web.Services;

namespace ZwembadenRequestApp.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    public class QuoteController : Controller
    {
        private readonly IQuoteRequestService _quoteService;
        private ApplicationUserManager _userManager;

        public QuoteController(IQuoteRequestService quoteService)
        {
            _quoteService = quoteService;
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // GET: Quote/Create
        public async Task<ActionResult> Create()
        {
            string userId = User.Identity.GetUserId();
            if (await _quoteService.CustomerHasOpenRequestAsync(userId))
            {
                TempData["Error"] = "U heeft al een openstaande offerte aanvraag. U kunt er maar één hebben.";
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Quote/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateQuoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity.GetUserId();
                    var user = await UserManager.FindByIdAsync(userId);

                    var quoteRequest = await _quoteService.SubmitQuoteRequestAsync(
                        userId, user.UserName, model.PoolType, model.Length, model.Width,
                        model.Depth, model.NumberOfLights, model.HasStairs, model.AdditionalNotes);

                    TempData["Success"] = "Uw offerte aanvraag is succesvol ingediend!";
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    TempData["Error"] = ex.Message;
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        // GET: Quote/Index
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var quotes = await _quoteService.GetByCustomerIdAsync(userId);
            return View(quotes);
        }

        // GET: Quote/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var quote = await _quoteService.GetByIdAsync(id);

                if (quote == null || quote.CustomerId != userId)
                    return HttpNotFound();

                if (quote.Status != QuoteStatus.New)
                {
                    TempData["Error"] = "U kunt alleen aanvragen wijzigen met status 'New'";
                    return RedirectToAction("Index");
                }

                var model = new CreateQuoteViewModel
                {
                    PoolType = quote.PoolType,
                    Length = quote.Length,
                    Width = quote.Width,
                    Depth = quote.Depth,
                    NumberOfLights = quote.NumberOfLights,
                    HasStairs = quote.HasStairs,
                    AdditionalNotes = quote.AdditionalNotes
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Er is een fout opgetreden bij het ophalen van de offerte.";
                return RedirectToAction("Index");
            }
        }

        // POST: Quote/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, CreateQuoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.Identity.GetUserId();
                    var quote = await _quoteService.GetByIdAsync(id);

                    if (quote == null || quote.CustomerId != userId)
                        return HttpNotFound();

                    if (quote.Status != QuoteStatus.New)
                    {
                        TempData["Error"] = "U kunt alleen aanvragen wijzigen met status 'New'";
                        return RedirectToAction("Index");
                    }

                    // Update the quote properties
                    quote.PoolType = model.PoolType;
                    quote.Length = model.Length;
                    quote.Width = model.Width;
                    quote.Depth = model.Depth;
                    quote.NumberOfLights = model.NumberOfLights;
                    quote.HasStairs = model.HasStairs;
                    quote.AdditionalNotes = model.AdditionalNotes;

                    await _quoteService.UpdateAsync(quote);
                    TempData["Success"] = "Offerte aanvraag succesvol bijgewerkt!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Er is een fout opgetreden bij het bijwerken van de offerte.";
                    return View(model);
                }
            }
            return View(model);
        }

        // POST: Quote/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var quote = await _quoteService.GetByIdAsync(id);

                if (quote == null || quote.CustomerId != userId)
                    return HttpNotFound();

                if (quote.Status != QuoteStatus.New)
                {
                    TempData["Error"] = "U kunt alleen aanvragen verwijderen met status 'New'";
                    return RedirectToAction("Index");
                }

                var result = await _quoteService.DeleteAsync(id);
                if (result)
                {
                    TempData["Success"] = "Offerte aanvraag succesvol verwijderd!";
                }
                else
                {
                    TempData["Error"] = "Offerte aanvraag kon niet worden verwijderd.";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Er is een fout opgetreden bij het verwijderen van de offerte.";
                return RedirectToAction("Index");
            }
        }

        // GET: Quote/Details/5
        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var quote = await _quoteService.GetByIdAsync(id);

                if (quote == null || quote.CustomerId != userId)
                {
                    return HttpNotFound();
                }

                return View(quote);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Er is een fout opgetreden bij het ophalen van de offerte details.";
                return RedirectToAction("Index");
            }
        }
    }
}