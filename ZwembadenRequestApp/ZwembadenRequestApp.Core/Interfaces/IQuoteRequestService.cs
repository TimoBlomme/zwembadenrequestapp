using System.Collections.Generic;
using System.Threading.Tasks;
using ZwembadenRequestApp.Core.Entities;

namespace ZwembadenRequestApp.Core.Interfaces
{
    public interface IQuoteRequestService
    {
        // Data access methods
        Task<QuoteRequest> GetByIdAsync(int id);
        Task<IEnumerable<QuoteRequest>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<QuoteRequest>> GetAllAsync();
        Task<QuoteRequest> CreateAsync(QuoteRequest quoteRequest);
        Task<QuoteRequest> UpdateAsync(QuoteRequest quoteRequest);
        Task<bool> DeleteAsync(int id);

        // Business logic methods
        Task<bool> CustomerHasOpenRequestAsync(string customerId);
        Task<QuoteRequest> SubmitQuoteRequestAsync(string customerId, string customerName,
            string poolType, decimal length, decimal width, decimal depth,
            int numberOfLights, bool hasStairs, string additionalNotes);
        Task<QuoteRequest> ProcessAdminResponseAsync(int quoteId, string status,
            decimal? proposedPrice, string adminNotes);
    }
}