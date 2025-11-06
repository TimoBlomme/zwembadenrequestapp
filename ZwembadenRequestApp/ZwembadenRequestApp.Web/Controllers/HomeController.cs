using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZwembadenRequestApp.Core.Entities;
using ZwembadenRequestApp.Core.Interfaces;

namespace ZwembadenRequestApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteRequestService _quoteService;

        public HomeController(IQuoteRequestService quoteService)
        {
            _quoteService = quoteService;
        }

        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Customer"))
                {
                    string userId = User.Identity.GetUserId();
                    List<QuoteRequest> pendingQuotes = (await _quoteService.GetByCustomerIdAsync(userId))
                        .Where(q => q.Status != QuoteStatus.Done)
                        .OrderByDescending(q => q.RequestDate)
                        .ToList();
                    ViewBag.PendingQuotes = pendingQuotes;
                }
                else if (User.IsInRole("Admin"))
                {
                    List<QuoteRequest> allPendingQuotes = (await _quoteService.GetAllAsync())
                        .Where(q => q.Status != QuoteStatus.Done)
                        .OrderBy(q => q.RequestDate)
                        .ToList();

                    List<QuoteRequest> quotesWithOffer = allPendingQuotes.Where(q => q.ProposedPrice.HasValue && q.ProposedPrice > 0).ToList();
                    List<QuoteRequest> quotesWithoutOffer = allPendingQuotes.Where(q => !q.ProposedPrice.HasValue || q.ProposedPrice == 0).ToList();

                    ViewBag.QuotesWithOffer = quotesWithOffer;
                    ViewBag.QuotesWithoutOffer = quotesWithoutOffer;
                }
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Your privacy page.";

            return View();
        }
    }
}