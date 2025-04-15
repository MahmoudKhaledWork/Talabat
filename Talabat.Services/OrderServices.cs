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
using Talabat.Core.Specifications.Order_spec;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Services
{
    public class OrderServices : IOrderServices
    {
        #region OLD
        //private readonly IBasketRepository _basketRepository;
        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<DelivreyMethod> _delivreyMethodRepo;
        //private readonly IGenericRepository<Order> _orderRepo;

        //public OrderServices(IBasketRepository basketRepository
        //    , IGenericRepository<Product> ProductRepo 
        //    , IGenericRepository<DelivreyMethod> DelivreyMethodRepo
        //    ,IGenericRepository<Order> OrderRepo)
        //{
        //    _basketRepository = basketRepository;
        //    _productRepo = ProductRepo;
        //    _delivreyMethodRepo = DelivreyMethodRepo;
        //    _orderRepo = OrderRepo;
        //} 
        #endregion
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOFWork _unitOFWork;
        private readonly IPaymentServiec _paymentServiec;

        public OrderServices(IBasketRepository basketRepository 
            , IUnitOFWork unitOFWork 
            ,IPaymentServiec paymentServiec)
        {
            _basketRepository = basketRepository;
            _unitOFWork = unitOFWork;
            this._paymentServiec = paymentServiec;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //1.Get Basket From Basket Repo            
            var Basket = await _basketRepository.GetBasketAsync(BasketId);

            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if (Basket?.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    //var Product = await _productRepo.GetByIdAsync(item.Id);
                    var Product = await _unitOFWork.Repository<Product>().GetByIdAsync(item.Id);
                    // ProductId, ProductName , PictureUrl
                    var ProductItemOrderd = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    // ProductItemOrderd , Price => Product  , Quantity => Item in  Basket 
                    var orderItem = new OrderItem(ProductItemOrderd, item.Quantity, Product.Price);
                    OrderItems.Add(orderItem);
                }
            }
            //3.Calculate SubTotal
            // Price * Quantity
            var SubTotal = OrderItems.Sum(I => I.Quantity * I.Price);
            //4.Get Delivery Method From DeliveryMethod Repo
            //var deliveryMethod = await _delivreyMethodRepo.GetByIdAsync(DeliveryMethodId);
            var DeliveryMethod = await _unitOFWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            //5.Create Order
            var Spec = new OrderWithPaymentSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOFWork.Repository<Order>().GetByIdAsyncWithSpec(Spec);
            if (ExOrder is not null)
            {
                await _unitOFWork.Repository<Order>().Delete(ExOrder);
                await _paymentServiec.CreateOrUpdatePaymentIntent(BasketId);
            }
            var Order = new Order(BuyerEmail,ShippingAddress, DeliveryMethod, OrderItems, SubTotal,Basket.PaymentIntentId);
            //6.Add Order Locally

            //await _orderRepo.Add(Order);
            await _unitOFWork.Repository<Order>().Add(Order);
            //7.Save Order To Database[ToDo]
            try
            {
                 await _unitOFWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            //if(Result <= 0 ) return null;
            }
            return Order;
        }

        public Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
        {
            var Spec = new OrderSpecification(BuyerEmail , OrderId);
            var order = _unitOFWork.Repository<Order>().GetByIdAsyncWithSpec(Spec);
            return order;
        }

        public Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail)
        {
            var Spec = new OrderSpecification(BuyerEmail);
            var Order = _unitOFWork.Repository<Order>().GetAllAsyncWithSpec(Spec);
            return Order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
        {
            var Delivery =  _unitOFWork.Repository<DeliveryMethod>().GetAllAsync();
            return Delivery;
        }
    }
}
