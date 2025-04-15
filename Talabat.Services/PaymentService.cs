using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Product = Talabat.Core.Entites.Product;

namespace Talabat.Services
{
    public class PaymentService : IPaymentServiec
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOFWork _unitOFWork;

        public PaymentService(IConfiguration configuration
            , IBasketRepository basketRepository
            , IUnitOFWork unitOFWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOFWork = unitOFWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];

            // Basket
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;

            // ShippingPrice
            var ShippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOFWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod?.Cost ?? 0M;
            }

            // SubTotal
            if (Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var Product = await _unitOFWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != Product.Price)
                    {
                        item.Price = Product.Price;
                    }
                }
            }
            var SubTotal = Basket.Items.Sum(I => I.Price * I.Quantity);

            // Create Payment Intent 
            var Services = new PaymentIntentService();
            PaymentIntent paymentIntent; 
            if (string.IsNullOrEmpty(Basket.PaymentIntentId)) // Create 
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() {"card"}
                };
                paymentIntent = await Services.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update 
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                };
                paymentIntent =  await Services.UpdateAsync(Basket.PaymentIntentId , Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }

            // Update Basket
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;

        }
    }
}
