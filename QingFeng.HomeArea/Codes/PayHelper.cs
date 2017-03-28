using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QingFeng.Common.Extensions;
using QingFeng.Models;

namespace QingFeng.WebArea.Codes
{
    public static class PayHelper
    {
        private const int MerchantCode = 1118015066;
        private const string NotifyUrl = "http://www.tonyzu.com/a";
        private const string ReturnUrl = "http://www.tonyzu.com/a";

        public static string GetPayUrl(PayOrder order, string productName)
        {
            var baseUlr = "https://pay.dinpay.com/gateway?";

            var signParams = $@"client_ip=127.0.0.1&extend_param={order.OrderId}&input_charset=UTF-8&
                                interface_version=V3.0&merchant_code={MerchantCode}&notify_url={NotifyUrl}&
                                order_amount={order.ActualPrice}&order_no={order.OrderNo}&order_time={DateTime.Now:yyyy-MM-dd HH:mm:ss}&
                                product_name={productName}&return_url={ReturnUrl}&service_type=direct_pay";

            var privateKey =
                "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAKgc0hIhv5BZSB1lkol+X9cFxhM/qaYisqSq0e7P224eMeaNPGj1BpqZYM1GRC9vDKeQvJm/pvV/ZpWZRGL8a4kvth40m1aFSxG8mNXDzGC/3P+VHxAOkTO0/PT62kIjz5miLM2ysPNNaUVsWzLYe7GLfKsRsPMBTTbcJlGd0C8ZAgMBAAECgYEApSj7cXjafPRaFxdtKcuQWO2BNfP7tg6st35jFV38VGkx2TG0weDIBibdpY58+qT9J7rYr6xMWTA7FoonV6Bp/BvSy+Mhz8b26XBpIhIGPRiKVCl4XjsAuZvN6Us0Gc6a/mLj6adUpSRVXO/IvybbpDE7omxLl2zQ0xoX4R1+nMkCQQDQ8xB7qoSlXl6bUJ8eebOdcjggRZNGY5SFOJUs91iD8mo8Y5oB46aKKdKEMJJfRCSI14rYpwEhwbBdi2jYflKzAkEAzfe0iFrdVqVUHSD78HHT9ATtNqmZ9+sf2z8sV5ggqmnF9DrJZAoru+XtfbChgkNlKsmgiYV5EHDtFo6F6+VtAwJAEixRzq+yAcAHcnK8pCXpnVQF1ai3enPGwx98ugB5TmCTJNV02501KucgTCb9VBPVKaG1jcpYqtrxv/EUGWBDSQJAegcD4rVS5X7WMtvT6ETIOo6gq/4XxpZ7LT5kWxE6aTx5l5UstCSCfLRg3FLFnZOoYI1Mm62EaIU/MloGZhhrWQJAFjDKRKFISJCunEsLd55rIVl2dwPhDCJ7jHkpSS4+nsfwQtma4rVBknsw8h4n7fz9dd43ekrRMo9Ug1jScddB4A==";

            //私钥转换成C#专用私钥
            privateKey = HttpHelp.RSAPrivateKeyJava2DotNet(privateKey);
            //签名
            var signData = HttpHelp.RSASign(signParams, privateKey);

            return baseUlr + signParams + "&sign=" + signData + "&sign_type=RSA-S";
        }
    }
}