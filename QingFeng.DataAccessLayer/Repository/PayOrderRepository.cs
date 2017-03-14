using QingFeng.Common.Dapper;
using QingFeng.Common.Extensions;
using QingFeng.Models;
using System;
using System.Collections.Generic;

namespace QingFeng.DataAccessLayer.Repository
{
    public class PayOrderRepository : RepositoryBase<PayOrder>
    {
        const string TableName = "payOrder";

        public PayOrderRepository() : base("payOrder")
        {

        }

        public IEnumerable<PayOrder> SearchPayOrder(int status, int verifyStatus, DateTime beginDate, DateTime endDate,
           string keyWords,
           int page,
           int pageSize, out int totalItem)
        {
            var additional = string.IsNullOrWhiteSpace(keyWords)
                ? string.Empty
                : "AND (orderId LIKE @keyWords OR orderNo LIKE @keyWords OR payNo LIKE @keyWords) ";

            additional += "AND CreateDate Between @beginDate AND @endDate ";

            if (status > 0)
            {
                additional += $"AND status = {status} ";
            }
            if (verifyStatus > 0)
            {
                additional += $"AND verifyStatus = {verifyStatus} ";
            }

            var condition = new
            {
                beginDate,
                endDate,
                keyWords = string.IsNullOrWhiteSpace(keyWords) ? string.Empty : keyWords.FormatSqlLikeString(),
            };

            Func<object, string> buildWhereSql =
                (cond) => SqlMapperExtensions.BuildWhereSql(cond, false, additional, "keyWords", "beginDate", "endDate");

            using (var connection = GetReadConnection)
            {
                return connection.QueryPaged<PayOrder>(condition, TableName, "CreateDate DESC", page, pageSize,
                    out totalItem, buildWhereSql);
            }
        }
    }
}
