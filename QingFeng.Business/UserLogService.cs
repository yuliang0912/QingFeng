using QingFeng.Common;
using QingFeng.DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.Business
{
    public class UserLogService : Singleton<UserLogService>
    {
        private readonly UserLogsRepository _orderLogs = new UserLogsRepository();
    }
}
