﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingFeng.Models
{
    public class OrderDetail
    {
        /// <summary>
        /// 流水号
        /// </summary>		
        public string FlowId { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>		
        public string OrderId { get; set; }


        public int BaseId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>		
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>		
        public string ProductName { get; set; }

        /// <summary>
        /// 商品图片
        /// </summary>		
        public string ImgUrl { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>		
        public decimal Price { get; set; }

        /// <summary>
        /// 购买数量
        /// </summary>		
        public int Quantity { get; set; }

        /// <summary>
        /// 商品金额
        /// </summary>		
        public decimal Amount { get; set; }

        /// <summary>
        /// 商品备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        /// 商品状态
        /// </summary>		
        public int OrderSatus { get; set; }
    }
}