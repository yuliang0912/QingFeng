using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;
using QingFeng.Common;

namespace QingFeng.Business
{
    public class OrderLogsService: Singleton<OrderLogsService>
    {
        private OrderLogsService() { }
        private readonly OrderLogsRepository _orderLogsRepository = new OrderLogsRepository();

        public IEnumerable<OrderLogs> GetList(long orderId)
        {
            return _orderLogsRepository.GetList(new {orderId}).OrderByDescending(t => t.CreateDate);
        }

        public bool CreateLog(OrderLogs model)
        {
            model.CreateDate = DateTime.Now;
            return _orderLogsRepository.Insert(model) > 0;
        }
    }
}
