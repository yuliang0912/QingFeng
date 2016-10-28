using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductStockRepository : RepositoryBase<ProductStock>
    {
        public ProductStockRepository() : base("productStock") { }
    }
}
