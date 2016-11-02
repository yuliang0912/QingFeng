using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>
    {
        public OrderDetailRepository() : base("orderDetail") { }
    }
}
