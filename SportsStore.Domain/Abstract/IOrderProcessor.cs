using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Abstract
{
    public interface IOrderProcessor
    {
        //// ----------------------------------------------------------------------------------------------------------
		 
        void ProcessCart(Cart cart, ShippingDetails shippingDetails);

        //// ----------------------------------------------------------------------------------------------------------
    }
}
