using ClosedXML.Excel;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;
using System;
using System.Linq;
using System.Web.Mvc;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Common.Extensions;
using QingFeng.WebArea.Codes;

namespace QingFeng.WebArea.Controllers
{
    public class PayController : Controller
    {
        [AdminAuthorize]
        public ActionResult Index(string keyWords = "", int payStatus = 0, int page = 1, int pageSize = 20)
        {
            int totalItem;

            var list = PayOrderService.Instance.SearchPayOrder(payStatus, 0, new DateTime(2017, 1, 1), DateTime.MaxValue,
                keyWords, page, pageSize, out totalItem);

            ViewBag.payStatus = payStatus;
            ViewBag.keyWords = keyWords;

            return View(new ApiPageList<PayOrder>
            {
                Page = page,
                PageSize = pageSize,
                PageList = list
            });
        }

        [AdminAuthorize]
        public ActionResult Export()
        {
            return View();
        }

        [AdminAuthorize]
        public ActionResult PayRedirect(UserInfo user, long orderId)
        {
            var orderInfo = OrderService.Instance.Get(new {orderId});

            if (orderInfo == null)
            {
                return Content("未找到订单");
            }

            if (orderInfo.OrderStatus != AgentEnums.MasterOrderStatus.待支付)
            {
                return Content("只有待支付的订单才能支付");
            }

            if (orderInfo.UserId != user.UserId)
            {
                return Content("只能支付自己的订单");
            }

            var model = PayOrderService.Instance.CreatePayOrder(orderInfo);

            var dateNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var productName = string.Join(",", orderInfo.OrderDetails.Select(t => t.ProductName));

            ViewBag.dateTime = dateNow;
            ViewBag.sign = PayHelper.GetPaySign(model, productName, dateNow);
            ViewBag.productName = productName;

            return View(model);
        }

        [HttpPost]
        public ActionResult Notify()
        {
            var orderAmount = Request.Form["order_amount"].Trim();
            var extraReturnParam = Request.Form["extra_return_param"];
            var tradeNo = Request.Form["trade_no"].Trim();
            var tradeTime = Request.Form["trade_time"].Trim();
            var tradeStatus = Request.Form["trade_status"].Trim();

            if (!PayHelper.CheckSign(Request))
            {
                return Content("签名验证失败");
            }

            var model = PayOrderService.Instance.Get(new {payNo = extraReturnParam});
            model.ActualPrice = Convert.ToDecimal(orderAmount);

            var result = PayOrderService.Instance.SetPayed(model, tradeTime, tradeNo, tradeStatus);

            return Content(result.ToString());
        }

        [AdminAuthorize(AgentEnums.SubMenuEnum.导出支付流水)]
        public ActionResult ExportFile(int payStatus = 0, string beginDateStr = "", string endDateStr = "")
        {
            DateTime beginDate, endDate;

            if (!DateTime.TryParse(beginDateStr, out beginDate))
            {
                beginDate = DateTime.MinValue;
            }
            if (!DateTime.TryParse(endDateStr, out endDate))
            {
                endDate = DateTime.Now;
            }
            endDate = endDate.AddDays(1).AddSeconds(-1);


            int totalItem;

            var list =
                PayOrderService.Instance.SearchPayOrder(payStatus, 0, beginDate, endDate, string.Empty, 1, int.MaxValue,
                    out totalItem).ToList();

            var orderDetailDict = OrderService.Instance.GetOrderDetailList(list.Select(t => t.OrderId).ToArray());

            var workbook = new XLWorkbook();
            workbook.Worksheets.Add("Sheet1");
            var workSheet = workbook.Worksheet(1);
            workSheet.Cell(1, 1).Value = "支付流水号";
            workSheet.Cell(1, 2).Value = "支付方式";
            workSheet.Cell(1, 3).Value = "支付金额";
            workSheet.Cell(1, 4).Value = "交易手续费";
            workSheet.Cell(1, 5).Value = "平台业务单号";
            workSheet.Cell(1, 6).Value = "状态";
            workSheet.Cell(1, 7).Value = "支付时间";
            workSheet.Cell(1, 8).Value = "审核时间";
            workSheet.Cell(1, 9).Value = "审核状态";
            workSheet.Cell(1, 10).Value = "订单流水号";
            workSheet.Cell(1, 11).Value = "品牌";
            workSheet.Cell(1, 12).Value = "货号";
            workSheet.Cell(1, 13).Value = "颜色";
            workSheet.Cell(1, 14).Value = "单价";
            workSheet.Cell(1, 15).Value = "数量";

            var rows = 2;
            for (var i = 0; i < list.Count; i++)
            {
                var payOrder = list[i];
                if (!orderDetailDict.ContainsKey(payOrder.OrderId))
                {
                    continue;
                }
                var orderDetail = orderDetailDict[payOrder.OrderId];

                workSheet.Cell(rows, 1).Value = payOrder.PayNo;
                workSheet.Cell(rows, 2).Value = payOrder.PayType.ToString();
                workSheet.Cell(rows, 3).Value = payOrder.ActualPrice;
                workSheet.Cell(rows, 4).Value = payOrder.CounterFee;
                workSheet.Cell(rows, 5).Value = payOrder.OutsideId;
                workSheet.Cell(rows, 6).Value = payOrder.PayStatus;
                workSheet.Cell(rows, 7).Value = payOrder.PayDate.ToInitialValue();
                workSheet.Cell(rows, 8).Value = payOrder.VerifyDate.ToInitialValue();
                workSheet.Cell(rows, 9).Value = payOrder.VerifyStatus.ToString();
                workSheet.Cell(rows, 10).Value = payOrder.OrderId.ToString();

                workSheet.Range(rows, 1, rows - 1 + orderDetail.Count(), 1).Merge();
                workSheet.Range(rows, 2, rows - 1 + orderDetail.Count(), 2).Merge();
                workSheet.Range(rows, 3, rows - 1 + orderDetail.Count(), 3).Merge();
                workSheet.Range(rows, 4, rows - 1 + orderDetail.Count(), 4).Merge();
                workSheet.Range(rows, 5, rows - 1 + orderDetail.Count(), 5).Merge();
                workSheet.Range(rows, 6, rows - 1 + orderDetail.Count(), 6).Merge();
                workSheet.Range(rows, 7, rows - 1 + orderDetail.Count(), 7).Merge();
                workSheet.Range(rows, 8, rows - 1 + orderDetail.Count(), 8).Merge();
                workSheet.Range(rows, 9, rows - 1 + orderDetail.Count(), 9).Merge();
                workSheet.Range(rows, 10, rows - 1 + orderDetail.Count(), 10).Merge();

                for (var j = 0; j < orderDetail.Count; j++)
                {
                    var detail = orderDetail[j];

                    workSheet.Cell(j + rows, 11).Value = detail.BrandId;
                    workSheet.Cell(j + rows, 12).Value = detail.BaseNo;
                    workSheet.Cell(j + rows, 13).Value = detail.ProductNo;
                    workSheet.Cell(j + rows, 14).Value = detail.Price;
                    workSheet.Cell(j + rows, 15).Value = detail.Quantity;
                }

                rows += orderDetail.Count;
            }

            workSheet.Rows(1, 1000).Height = 20;
            workSheet.Columns(1, 100).Width = 25;
            workSheet.Range("A1:O1").Style.Fill.BackgroundColor = XLColor.Green;
            workSheet.Range("A1:O1").Style.Font.SetFontColor(XLColor.Yellow);
            workSheet.Range("A1:O1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            workSheet.Range("A2:J100").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            workSheet.Range("A2:O100").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            return new Common.ActionResultExtensions.ExportExcelResult
            {
                workBook = workbook,
                FileName =
                    string.Concat(beginDate.ToString("yyyy-MM-dd"), "-", endDate.ToString("yyyy-MM-dd"), "-pay-order",
                        ".xlsx")
            };
        }
    }
}