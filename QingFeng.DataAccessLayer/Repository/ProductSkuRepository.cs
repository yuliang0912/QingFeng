using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductSkuRepository : RepositoryBase<ProductSkus>
    {
        const string TableName = "productSkus";

        public ProductSkuRepository() : base(TableName)
        {

            //add on duc.... update status ....
        }
    }
}
