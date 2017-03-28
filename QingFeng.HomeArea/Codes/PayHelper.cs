using System.Web;
using QingFeng.Models;

namespace QingFeng.WebArea.Codes
{
    public static class PayHelper
    {
        private const int MerchantCode = 1118015066;
        private const string NotifyUrl = "http://www.tonyzu.com/pay/notify";
        private const string ReturnUrl = "http://www.tonyzu.com/pay/return";

        private const string PrivateKey =
            "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAKgc0hIhv5BZSB1lkol+X9cFxhM/qaYisqSq0e7P224eMeaNPGj1BpqZYM1GRC9vDKeQvJm/pvV/ZpWZRGL8a4kvth40m1aFSxG8mNXDzGC/3P+VHxAOkTO0/PT62kIjz5miLM2ysPNNaUVsWzLYe7GLfKsRsPMBTTbcJlGd0C8ZAgMBAAECgYEApSj7cXjafPRaFxdtKcuQWO2BNfP7tg6st35jFV38VGkx2TG0weDIBibdpY58+qT9J7rYr6xMWTA7FoonV6Bp/BvSy+Mhz8b26XBpIhIGPRiKVCl4XjsAuZvN6Us0Gc6a/mLj6adUpSRVXO/IvybbpDE7omxLl2zQ0xoX4R1+nMkCQQDQ8xB7qoSlXl6bUJ8eebOdcjggRZNGY5SFOJUs91iD8mo8Y5oB46aKKdKEMJJfRCSI14rYpwEhwbBdi2jYflKzAkEAzfe0iFrdVqVUHSD78HHT9ATtNqmZ9+sf2z8sV5ggqmnF9DrJZAoru+XtfbChgkNlKsmgiYV5EHDtFo6F6+VtAwJAEixRzq+yAcAHcnK8pCXpnVQF1ai3enPGwx98ugB5TmCTJNV02501KucgTCb9VBPVKaG1jcpYqtrxv/EUGWBDSQJAegcD4rVS5X7WMtvT6ETIOo6gq/4XxpZ7LT5kWxE6aTx5l5UstCSCfLRg3FLFnZOoYI1Mm62EaIU/MloGZhhrWQJAFjDKRKFISJCunEsLd55rIVl2dwPhDCJ7jHkpSS4+nsfwQtma4rVBknsw8h4n7fz9dd43ekrRMo9Ug1jScddB4A==";

        private const string PublicKey =
            "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCAWOjvxeUnjmGqtYkkNXfmy12gUv2xXS8ZRIRGjbAWNt5GKanylJvUlXqzOmWnPAzj1tzDR3Ot3grekFO0mcLA/Eh0eCv8HD4seGrMOleSNQbcEO9VlMKRwfTkBlbJ1TREKvq5h+HJtieY99WZ6+YlP38m2T1bM9FhNdCKzQdzTwIDAQAB";

        public static string GetPaySign(PayOrder order, string productName, string timeLine)
        {
            var signParams =
                $"bank_code=&client_ip=&extend_param={order.PayNo}&extra_return_param={order.PayNo}&input_charset=UTF-8&";
            signParams += $"interface_version=V3.0&merchant_code={MerchantCode}&notify_url={NotifyUrl}&";
            signParams +=
                $"order_amount={order.ActualPrice}&order_no={order.OrderNo}&order_time={timeLine}&";
            signParams += $"pay_type=&product_code=&product_desc=&product_name={productName}&product_num=&";
            signParams += $"redo_flag=&return_url={ReturnUrl}&service_type=direct_pay&show_url=";

            //私钥转换成C#专用私钥
            var convertPrivateKey = HttpHelp.RSAPrivateKeyJava2DotNet(PrivateKey);
            //签名
            return HttpHelp.RSASign(signParams, convertPrivateKey);
        }

        public static bool CheckSign(HttpRequestBase request)
        {
            var merchantCode = request.Form["merchant_code"].Trim();
            var notifyType = request.Form["notify_type"].Trim();
            var notifyId = request.Form["notify_id"].Trim();
            var interfaceVersion = request.Form["interface_version"].Trim();
            var signType = request.Form["sign_type"].Trim();
            var dinpaySign = request.Form["sign"].Trim();
            var orderNo = request.Form["order_no"].Trim();
            var orderTime = request.Form["order_time"].Trim();
            var orderAmount = request.Form["order_amount"].Trim();
            var extraReturnParam = request.Form["extra_return_param"];
            var tradeNo = request.Form["trade_no"].Trim();
            var tradeTime = request.Form["trade_time"].Trim();
            var tradeStatus = request.Form["trade_status"].Trim();
            var bankSeqNo = request.Form["bank_seq_no"];

            if (merchantCode != MerchantCode.ToString())
            {
                return false;
            }

            var signParams = string.Empty;
            if (!string.IsNullOrEmpty(bankSeqNo))
            {
                signParams += "bank_seq_no=" + bankSeqNo.Trim() + "&";
            }
            if (!string.IsNullOrEmpty(extraReturnParam))
            {
                signParams += "extra_return_param=" + extraReturnParam + "&";
            }
            signParams = signParams + "interface_version=V3.0&";
            signParams = signParams + "merchant_code=" + merchantCode + "&";
            if (!string.IsNullOrEmpty(notifyId))
            {
                signParams += "notify_id=" + notifyId + "&notify_type=" + notifyType + "&";
            }
            signParams = signParams + "order_amount=" + orderAmount + "&";
            signParams = signParams + "order_no=" + orderNo + "&";
            signParams = signParams + "order_time=" + orderTime + "&";
            signParams = signParams + "trade_no=" + tradeNo + "&";
            signParams = signParams + "trade_status=" + tradeStatus + "&";
            if (!string.IsNullOrEmpty(tradeTime))
            {
                signParams += "trade_time=" + tradeTime;
            }
            if (signType == "RSA-S")
            {
                var convertPublickKey = HttpHelp.RSAPrivateKeyJava2DotNet(PublicKey);
                return HttpHelp.ValidateRsaSign(signParams, convertPublickKey, dinpaySign);
            }

            return false;
        }
    }
}