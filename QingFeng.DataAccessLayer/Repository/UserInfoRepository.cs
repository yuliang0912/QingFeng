using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QingFeng.Common;
using QingFeng.Common.Dapper;
using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class UserInfoRepository : RepositoryBase<UserInfo>
    {
        const string TableName = "userInfo";

        public UserInfoRepository() : base("userInfo")
        {
        }

        public IEnumerable<UserInfo> Search(string keyWords, AgentEnums.UserRole userRole)
        {
            var additional = string.IsNullOrWhiteSpace(keyWords)
                ? string.Empty
                : "AND (userName LIKE @keyWords OR nickName LIKE @keyWords) ";

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords");

            using (var connection = GetReadConnection)
            {
                return connection.QueryList<UserInfo>(new {userRole = userRole.GetHashCode()}, TableName, buildWhereSql);
            }
        }
    }
}
