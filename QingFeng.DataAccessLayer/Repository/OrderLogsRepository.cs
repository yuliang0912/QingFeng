using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class OrderLogsRepository : RepositoryBase<OrderLogs>
    {
        private const string TableName = "orderLogs";

        public OrderLogsRepository() : base(TableName)
        {

        }
    }
}
