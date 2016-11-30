using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;

namespace QingFeng.Business
{
    public class OrderLogsService
    {
        private readonly OrderLogsRepository _orderLogsRepository = new OrderLogsRepository();

        public IEnumerable<OrderLogs> GetList(long orderId)
        {
            return _orderLogsRepository.GetList(new {orderId}).OrderByDescending(t => t.CreateDate);
        }
    }
}
