using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ZwembadenRequestApp.Core.Entities;
using ZwembadenRequestApp.Core.Interfaces;
using ZwembadenRequestApp.Web.App_Data;

namespace ZwembadenRequestApp.Web.Services
{
    public class QuoteRequestService : IQuoteRequestService
    {
        private readonly ApplicationDbContext _context;

        public QuoteRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Data access methods
        public async Task<QuoteRequest> GetByIdAsync(int id)
        {
            return await _context.QuoteRequests.FindAsync(id);
        }

        public async Task<IEnumerable<QuoteRequest>> GetByCustomerIdAsync(string customerId)
        {
            return await _context.QuoteRequests
                .Where(q => q.CustomerId == customerId)
                .OrderByDescending(q => q.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<QuoteRequest>> GetAllAsync()
        {
            return await _context.QuoteRequests
                .OrderByDescending(q => q.RequestDate)
                .ToListAsync();
        }

        public async Task<QuoteRequest> CreateAsync(QuoteRequest quoteRequest)
        {
            _context.QuoteRequests.Add(quoteRequest);
            await _context.SaveChangesAsync();
            return quoteRequest;
        }

        public async Task<QuoteRequest> UpdateAsync(QuoteRequest quoteRequest)
        {
            _context.Entry(quoteRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return quoteRequest;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var quoteRequest = await _context.QuoteRequests.FindAsync(id);
            if (quoteRequest == null)
                return false;

            _context.QuoteRequests.Remove(quoteRequest);
            await _context.SaveChangesAsync();
            return true;
        }

        // Business logic methods
        public async Task<bool> CustomerHasOpenRequestAsync(string customerId)
        {
            return await _context.QuoteRequests
                .AnyAsync(q => q.CustomerId == customerId &&
                              (q.Status == QuoteStatus.New || q.Status == QuoteStatus.InProgress));
        }

        public async Task<QuoteRequest> SubmitQuoteRequestAsync(string customerId, string customerName,
            string poolType, decimal length, decimal width, decimal depth,
            int numberOfLights, bool hasStairs, string additionalNotes)
        {
            // Business rule: Customer can only have one open request
            if (await CustomerHasOpenRequestAsync(customerId))
            {
                throw new InvalidOperationException("U heeft al een openstaande offerte aanvraag.");
            }

            var quoteRequest = new QuoteRequest
            {
                CustomerId = customerId,
                CustomerName = customerName,
                PoolType = poolType,
                Length = length,
                Width = width,
                Depth = depth,
                NumberOfLights = numberOfLights,
                HasStairs = hasStairs,
                AdditionalNotes = additionalNotes,
                Status = QuoteStatus.New,
                RequestDate = DateTime.Now
            };

            return await CreateAsync(quoteRequest);
        }

        public async Task<QuoteRequest> ProcessAdminResponseAsync(int quoteId, string status,
            decimal? proposedPrice, string adminNotes)
        {
            var quoteRequest = await GetByIdAsync(quoteId);
            if (quoteRequest == null)
            {
                throw new ArgumentException("Offerte aanvraag niet gevonden.");
            }

            // Business logic for admin response
            quoteRequest.Status = status;
            quoteRequest.AdminNotes = adminNotes;

            // Set response date when price is first proposed
            if (proposedPrice.HasValue && !quoteRequest.ProposedPrice.HasValue)
            {
                quoteRequest.ProposedPrice = proposedPrice;
                quoteRequest.ResponseDate = DateTime.Now;
            }
            else if (proposedPrice != quoteRequest.ProposedPrice)
            {
                quoteRequest.ProposedPrice = proposedPrice;
                if (!quoteRequest.ResponseDate.HasValue && proposedPrice.HasValue)
                {
                    quoteRequest.ResponseDate = DateTime.Now;
                }
            }

            return await UpdateAsync(quoteRequest);
        }
    }
}