using Stripe;

namespace SimpleECommerceAPI.Services
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentAsync(decimal amount, string currency);
    }
}
