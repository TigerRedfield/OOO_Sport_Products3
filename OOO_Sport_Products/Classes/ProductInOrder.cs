using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOO_Sport_Products.Classes
{
    public class ProductInOrder
    {
        Classes.ProductExtended ProductExtendedInOrder { get; set; }
        int countProductInOrder { get; set; }
        public ProductInOrder(ProductExtended productExtended)
        {
            ProductExtendedInOrder = productExtended;
            this.countProductInOrder = 1;
        }
        public ProductInOrder()
        {

        }
    }
}