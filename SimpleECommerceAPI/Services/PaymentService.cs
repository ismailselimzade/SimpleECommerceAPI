using Stripe;

namespace SimpleECommerceAPI.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentIntent> CreatePaymentAsync(decimal amount, string currency)
        {
            PaymentIntentCreateOptions options = new() { Amount = (long)amount * 100, Currency = currency };

            PaymentIntentService service = new();
            PaymentIntent stripe = await service.CreateAsync(options);
            return stripe;
        }
    }
}
