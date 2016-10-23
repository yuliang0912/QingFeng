using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class ProductBaseRepository : RepositoryBase<ProductBase>
    {
        public ProductBaseRepository() : base("productBase")
        {
        }
    }
}
