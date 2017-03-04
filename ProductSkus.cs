using QingFeng.Common.Dapper;
using System;

namespace QingFeng.Models
{
    public class ProductSkus
    {
        [IgnoreField]
        public int PsId { get; set; }

        public int SkuId { get; set; }

        public int BaseId { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Status { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}

//CREATE TABLE `productskus` (
//  `psId` int(11) NOT NULL AUTO_INCREMENT,
//  `skuId` int(11) NOT NULL,
//  `baseId` int(11) NOT NULL,
//  `productId` int(11) NOT NULL,
//  `price` decimal(6,2) NOT NULL,
//  `status` int(11) NOT NULL,
//  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
//  PRIMARY KEY(`psId`),
//  UNIQUE KEY `unique_product_sku` (`productId`,`skuId`) USING BTREE
//) ENGINE=MyISAM AUTO_INCREMENT = 2 DEFAULT CHARSET = utf8;