using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        //// ----------------------------------------------------------------------------------------------------------
		 
        private readonly List<CartLine> lineCollection;

        //// ----------------------------------------------------------------------------------------------------------

        public IEnumerable<CartLine> Lines
        {
            get
            {
                return this.lineCollection;
            }
        }

        //// ----------------------------------------------------------------------------------------------------------

        public Cart()
        {
            this.lineCollection = new List<CartLine>();
        }

        //// ----------------------------------------------------------------------------------------------------------

        public void AddItem(Product product, int quantity)
        {
            var cartLine = this.lineCollection.FirstOrDefault(p => p.Product.ProductId == product.ProductId);

            if (cartLine == null)
            {
                this.lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        public void RemoveLine(Product product)
        {
            this.lineCollection.RemoveAll(lc => lc.Product.ProductId == product.ProductId);
        }

        //// ----------------------------------------------------------------------------------------------------------
		 
        public decimal ComputeTotalValue()
        {
            return this.lineCollection.Sum(lc => lc.Product.Price * lc.Quantity);
        }

        //// ----------------------------------------------------------------------------------------------------------

        public void Clear()
        {
            this.lineCollection.Clear();
        }

        //// ----------------------------------------------------------------------------------------------------------
    }
}
