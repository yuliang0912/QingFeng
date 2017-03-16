/*
Navicat MySQL Data Transfer

Source Server         : 192.168.1.64
Source Server Version : 50517
Source Host           : 192.168.1.64:3306
Source Database       : cw_test_agent

Target Server Type    : MYSQL
Target Server Version : 50517
File Encoding         : 65001

Date: 2017-03-16 16:57:28
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `logistics`
-- ----------------------------
DROP TABLE IF EXISTS `logistics`;
CREATE TABLE `logistics` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `orderId` bigint(20) NOT NULL,
  `flowIds` varchar(255) NOT NULL,
  `companyId` int(11) NOT NULL,
  `companyName` varchar(20) NOT NULL,
  `oddNumber` varchar(30) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `status` int(11) NOT NULL,
  `createDate` datetime NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of logistics
-- ----------------------------
INSERT INTO `logistics` VALUES ('1', '1612090018410960807', '13,14', '1', '顺丰快递', '456112631548', '9.90', '0', '2016-11-26 11:13:29', '2016-12-10 10:23:27');
INSERT INTO `logistics` VALUES ('2', '1612100008204696367', '22', '1', '顺丰速运', '123156465', '1.00', '0', '2016-12-10 15:29:23', '2016-12-10 15:39:48');
INSERT INTO `logistics` VALUES ('3', '1612100008204696367', '23', '4', '申通快递', '15616151', '54.00', '0', '2016-12-10 15:29:54', '2016-12-10 15:39:51');
INSERT INTO `logistics` VALUES ('4', '1612100026541504420', '24,25', '2', '邮政EMS', '1231564', '123.00', '0', '2016-12-10 15:38:58', '2016-12-10 15:38:58');
INSERT INTO `logistics` VALUES ('5', '1612100004236724301', '21', '7', '宅急送', '18165156', '1.00', '0', '2016-12-10 16:26:49', '2016-12-10 16:26:49');
INSERT INTO `logistics` VALUES ('6', '1612100004236724301', '20', '4', '申通快递', '432432432', '3.00', '0', '2016-12-10 16:27:01', '2016-12-10 16:27:01');

-- ----------------------------
-- Table structure for `orderdetail`
-- ----------------------------
DROP TABLE IF EXISTS `orderdetail`;
CREATE TABLE `orderdetail` (
  `flowId` int(11) NOT NULL AUTO_INCREMENT,
  `orderId` bigint(20) NOT NULL,
  `orderNo` varchar(30) NOT NULL,
  `baseId` int(11) NOT NULL,
  `baseNo` varchar(32) NOT NULL DEFAULT '',
  `baseName` varchar(120) NOT NULL DEFAULT '',
  `productId` int(11) NOT NULL,
  `productNo` varchar(40) NOT NULL DEFAULT '',
  `productName` varchar(120) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `quantity` int(11) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `remark` varchar(120) NOT NULL,
  `orderStatus` int(11) NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `skuId` int(11) NOT NULL,
  `skuName` varchar(30) NOT NULL,
  PRIMARY KEY (`flowId`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of orderdetail
-- ----------------------------
INSERT INTO `orderdetail` VALUES ('26', '1703111540254447433', '323245346756756765756', '11', '7-29SPW1023', '7-29SPW1023', '10041', '7-29SPW102311C', '7-29SPW1023', '240.00', '14', '3360.00', '', '1', '2017-03-11 15:40:12', '16', '41.5');

-- ----------------------------
-- Table structure for `orderlogs`
-- ----------------------------
DROP TABLE IF EXISTS `orderlogs`;
CREATE TABLE `orderlogs` (
  `logId` int(11) NOT NULL AUTO_INCREMENT,
  `orderId` bigint(20) NOT NULL,
  `userId` int(11) NOT NULL,
  `userName` varchar(32) NOT NULL,
  `title` varchar(64) NOT NULL,
  `content` varchar(2000) NOT NULL,
  `createDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`logId`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of orderlogs
-- ----------------------------
INSERT INTO `orderlogs` VALUES ('2', '1611302246169946118', '10007', 'admin', '添加订单', '女士休闲鞋-蓝色-38*2', '2016-11-30 22:46:17');
INSERT INTO `orderlogs` VALUES ('3', '1612090018410960807', '10017', 'admin', '添加订单', '女士休闲鞋-红色-37*1,女士休闲鞋-蓝色-38*1', '2016-12-09 00:18:41');
INSERT INTO `orderlogs` VALUES ('4', '1612090026017362481', '10017', 'admin', '添加订单', '女士休闲鞋-蓝色-41.5*1,女士休闲鞋-红色-38*1', '2016-12-09 00:26:02');
INSERT INTO `orderlogs` VALUES ('5', '1612090026535817624', '10017', 'admin', '添加订单', '女士休闲鞋-蓝色-38*1,女士休闲鞋-红色-37*1', '2016-12-09 00:26:54');
INSERT INTO `orderlogs` VALUES ('6', '1612092337362780668', '10017', 'admin', '添加订单', '女士休闲鞋-蓝色-37*1', '2016-12-09 23:37:36');
INSERT INTO `orderlogs` VALUES ('7', '1612100004236724301', '10017', 'admin', '添加订单', '女士休闲鞋-蓝色-37*1,女士休闲鞋-红色-38*1', '2016-12-10 00:04:24');
INSERT INTO `orderlogs` VALUES ('8', '1612100008204696367', '10017', 'admin', '添加订单', '女士休闲鞋-蓝色-37*1,女士休闲鞋-蓝色-38*1', '2016-12-10 00:08:21');
INSERT INTO `orderlogs` VALUES ('9', '1612100026541504420', '10017', 'admin', '添加订单', '蓝色38  蓝色39  ', '2016-12-10 00:26:54');
INSERT INTO `orderlogs` VALUES ('10', '1612090018410960807', '10017', 'admin', '确认收款', 'admin已确认收到订单款项', '2016-12-10 11:43:22');
INSERT INTO `orderlogs` VALUES ('11', '1612090018410960807', '10017', 'admin', '取消订单', '客户要求取消,已退款', '2016-12-10 12:04:37');
INSERT INTO `orderlogs` VALUES ('12', '1612100008204696367', '10017', 'admin', '发货', '顺丰速运,运单号:123156465', '2016-12-10 15:29:23');
INSERT INTO `orderlogs` VALUES ('13', '1612100008204696367', '10017', 'admin', '发货', '申通快递,运单号:15616151', '2016-12-10 15:29:54');
INSERT INTO `orderlogs` VALUES ('14', '1612100026541504420', '10017', 'admin', '确认收款', '已确认收到订单款项', '2016-12-10 15:38:47');
INSERT INTO `orderlogs` VALUES ('15', '1612100026541504420', '10017', 'admin', '发货', '邮政EMS,运单号:1231564', '2016-12-10 15:38:58');
INSERT INTO `orderlogs` VALUES ('16', '1612100004236724301', '10017', 'admin', '发货', '宅急送,运单号:18165156', '2016-12-10 16:26:49');
INSERT INTO `orderlogs` VALUES ('17', '1612100004236724301', '10017', 'admin', '发货', '申通快递,运单号:432432432', '2016-12-10 16:27:01');
INSERT INTO `orderlogs` VALUES ('18', '1612092337362780668', '10017', 'admin', '确认收款', '已确认收到订单款项', '2016-12-10 16:44:09');
INSERT INTO `orderlogs` VALUES ('19', '1612090026535817624', '10017', 'admin', '支付订单', 'admin已支付了订单,等待后台管理员审核', '2016-12-10 17:48:12');
INSERT INTO `orderlogs` VALUES ('20', '1612090026535817624', '10017', 'admin', '确认收款', '已确认收到订单款项', '2016-12-10 17:50:58');
INSERT INTO `orderlogs` VALUES ('21', '1703111540254447433', '10017', 'admin', '添加订单', '41.5  ', '2017-03-11 15:40:25');

-- ----------------------------
-- Table structure for `ordermaster`
-- ----------------------------
DROP TABLE IF EXISTS `ordermaster`;
CREATE TABLE `ordermaster` (
  `orderId` bigint(20) NOT NULL,
  `orderNo` varchar(30) NOT NULL,
  `orderAmount` decimal(10,2) NOT NULL,
  `orderFavorable` decimal(10,2) NOT NULL,
  `orderDetailCount` int(11) NOT NULL,
  `storeId` int(11) NOT NULL,
  `storeName` varchar(32) NOT NULL DEFAULT '',
  `IsSelfSupport` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `contactName` varchar(30) NOT NULL,
  `contactPhone` varchar(30) NOT NULL,
  `postCode` int(6) NOT NULL,
  `provinceId` int(11) NOT NULL,
  `cityId` int(11) NOT NULL,
  `areaCode` varchar(20) NOT NULL,
  `areaName` varchar(200) NOT NULL,
  `address` varchar(100) NOT NULL,
  `orderStatus` int(11) NOT NULL,
  `payType` int(11) NOT NULL,
  `payFee` decimal(10,2) NOT NULL,
  `payStatus` int(11) NOT NULL,
  `payDate` datetime NOT NULL,
  `remark` varchar(500) NOT NULL,
  `createDate` datetime NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`orderId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of ordermaster
-- ----------------------------
INSERT INTO `ordermaster` VALUES ('1703111540254447433', '323245346756756765756', '3360.00', '0.00', '1', '21', 'F01', '0', '10017', '呵呵', '15814040136', '200020', '0', '0', '891', '上海 上海市 卢湾区 ', '福田街道1号', '3', '0', '0.00', '1', '1970-01-01 00:00:00', '', '2017-03-11 15:40:25', '2017-03-13 11:29:52');

-- ----------------------------
-- Table structure for `payorder`
-- ----------------------------
DROP TABLE IF EXISTS `payorder`;
CREATE TABLE `payorder` (
  `payNo` varchar(50) NOT NULL,
  `orderId` bigint(20) NOT NULL,
  `orderNo` varchar(128) NOT NULL,
  `outsideId` varchar(128) NOT NULL,
  `actualPrice` decimal(10,2) NOT NULL,
  `counterFee` decimal(10,2) NOT NULL,
  `payType` int(11) NOT NULL,
  `payStatus` int(11) NOT NULL,
  `verifyStatus` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `createDate` datetime NOT NULL,
  `payDate` datetime NOT NULL,
  `verifyDate` datetime NOT NULL,
  PRIMARY KEY (`payNo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of payorder
-- ----------------------------

-- ----------------------------
-- Table structure for `product`
-- ----------------------------
DROP TABLE IF EXISTS `product`;
CREATE TABLE `product` (
  `productId` int(11) NOT NULL AUTO_INCREMENT,
  `productName` varchar(150) NOT NULL,
  `baseId` int(11) NOT NULL,
  `baseNo` varchar(30) NOT NULL DEFAULT '',
  `baseName` varchar(120) NOT NULL,
  `productNo` varchar(40) NOT NULL,
  `originalPrice` decimal(10,2) NOT NULL,
  `actualPrice` decimal(10,2) NOT NULL,
  `status` int(11) NOT NULL,
  `createDate` datetime NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`productId`),
  UNIQUE KEY `unique_product_no` (`baseNo`,`productNo`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=10042 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of product
-- ----------------------------
INSERT INTO `product` VALUES ('10039', '7-31SPW0036', '10', '7-31SPW0036', '7-31SPW0036', '7-31SPW0036A75', '599.00', '588.00', '0', '2017-03-04 17:15:27', '2017-03-04 17:59:12');
INSERT INTO `product` VALUES ('10040', '7-29SPW1023', '11', '7-29SPW1023', '7-29SPW1023', '7-29SPW1023GG2', '999.00', '250.00', '0', '2017-03-04 17:50:50', '2017-03-04 17:50:49');
INSERT INTO `product` VALUES ('10041', '7-29SPW1023', '11', '7-29SPW1023', '7-29SPW1023', '7-29SPW102311C', '999.00', '240.00', '0', '2017-03-04 17:50:50', '2017-03-04 17:50:49');

-- ----------------------------
-- Table structure for `productbase`
-- ----------------------------
DROP TABLE IF EXISTS `productbase`;
CREATE TABLE `productbase` (
  `baseId` int(11) NOT NULL AUTO_INCREMENT,
  `baseName` varchar(120) NOT NULL,
  `baseNo` varchar(32) NOT NULL,
  `categoryId` int(11) NOT NULL,
  `brandId` int(11) NOT NULL DEFAULT '1',
  `sexId` int(11) NOT NULL DEFAULT '1',
  `status` int(11) NOT NULL,
  `createUserId` int(11) NOT NULL,
  `createDate` datetime NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`baseId`),
  UNIQUE KEY `unique_base_no` (`baseNo`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of productbase
-- ----------------------------
INSERT INTO `productbase` VALUES ('10', '7-31SPW0036', '7-31SPW0036', '2', '1', '1', '0', '155014', '2017-03-04 17:15:27', '2017-03-04 17:59:25');
INSERT INTO `productbase` VALUES ('11', '7-29SPW1023', '7-29SPW1023', '8', '1', '1', '0', '155014', '2017-03-04 17:50:50', '2017-03-04 17:50:49');

-- ----------------------------
-- Table structure for `productskus`
-- ----------------------------
DROP TABLE IF EXISTS `productskus`;
CREATE TABLE `productskus` (
  `psId` int(11) NOT NULL AUTO_INCREMENT,
  `skuId` int(11) NOT NULL,
  `skuName` varchar(255) NOT NULL DEFAULT '',
  `baseId` int(11) NOT NULL,
  `productId` int(11) NOT NULL,
  `price` decimal(6,2) NOT NULL,
  `status` int(11) NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`psId`),
  UNIQUE KEY `unique_product_sku` (`productId`,`skuId`) USING BTREE
) ENGINE=MyISAM AUTO_INCREMENT=27 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of productskus
-- ----------------------------
INSERT INTO `productskus` VALUES ('11', '8', '37.5', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('12', '9', '38', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('13', '10', '38.5', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('14', '14', '40.5', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('15', '15', '41', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('16', '16', '41.5', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('8', '8', '37.5', '10', '10039', '599.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('9', '14', '40.5', '10', '10039', '599.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('10', '27', '47', '10', '10039', '599.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('17', '20', '43.5', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('18', '21', '44', '11', '10040', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('19', '8', '37.5', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('20', '9', '38', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('21', '10', '38.5', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('22', '14', '40.5', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('23', '15', '41', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('24', '16', '41.5', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('25', '20', '43.5', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');
INSERT INTO `productskus` VALUES ('26', '21', '44', '11', '10041', '999.00', '0', '2017-03-04 20:23:45');

-- ----------------------------
-- Table structure for `productstock`
-- ----------------------------
DROP TABLE IF EXISTS `productstock`;
CREATE TABLE `productstock` (
  `stockId` int(11) NOT NULL AUTO_INCREMENT,
  `baseId` int(11) NOT NULL,
  `productId` int(11) NOT NULL,
  `skuId` int(11) NOT NULL,
  `skuName` varchar(32) NOT NULL,
  `stockNum` int(11) NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`stockId`),
  UNIQUE KEY `Unique_productId_skuId` (`productId`,`skuId`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of productstock
-- ----------------------------
INSERT INTO `productstock` VALUES ('34', '11', '10040', '8', '37.5', '83', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('35', '11', '10040', '9', '38', '8', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('36', '11', '10040', '10', '38.5', '5', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('37', '11', '10040', '14', '40.5', '9', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('38', '11', '10040', '15', '41', '11', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('39', '11', '10040', '16', '41.5', '56', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('40', '11', '10040', '20', '43.5', '15', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('41', '11', '10040', '21', '44', '17', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('42', '11', '10041', '8', '37.5', '52', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('43', '11', '10041', '9', '38', '15', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('44', '11', '10041', '10', '38.5', '17', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('45', '11', '10041', '14', '40.5', '95', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('46', '11', '10041', '15', '41', '168', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('47', '11', '10041', '16', '41.5', '872', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('48', '11', '10041', '20', '43.5', '5921', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('49', '11', '10041', '21', '44', '157', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('50', '10', '10039', '8', '37.5', '100', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('51', '10', '10039', '14', '40.5', '52', '2017-03-11 12:06:31');
INSERT INTO `productstock` VALUES ('52', '10', '10039', '27', '47', '5', '2017-03-11 12:06:31');

-- ----------------------------
-- Table structure for `skuitems`
-- ----------------------------
DROP TABLE IF EXISTS `skuitems`;
CREATE TABLE `skuitems` (
  `skuId` int(11) NOT NULL AUTO_INCREMENT,
  `skuName` varchar(32) NOT NULL,
  `skuImgUrl` varchar(128) NOT NULL DEFAULT '',
  `skuType` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  PRIMARY KEY (`skuId`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of skuitems
-- ----------------------------
INSERT INTO `skuitems` VALUES ('1', '34', '', '2', '0');
INSERT INTO `skuitems` VALUES ('2', '34.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('3', '35', '', '2', '0');
INSERT INTO `skuitems` VALUES ('4', '35.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('5', '36', '', '2', '0');
INSERT INTO `skuitems` VALUES ('6', '36.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('7', '37', '', '2', '0');
INSERT INTO `skuitems` VALUES ('8', '37.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('9', '38', '', '2', '0');
INSERT INTO `skuitems` VALUES ('10', '38.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('11', '39', '', '2', '0');
INSERT INTO `skuitems` VALUES ('12', '39.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('13', '40', '', '2', '0');
INSERT INTO `skuitems` VALUES ('14', '40.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('15', '41', '', '2', '0');
INSERT INTO `skuitems` VALUES ('16', '41.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('17', '42', '', '2', '0');
INSERT INTO `skuitems` VALUES ('18', '42.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('19', '43', '', '2', '0');
INSERT INTO `skuitems` VALUES ('20', '43.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('21', '44', '', '2', '0');
INSERT INTO `skuitems` VALUES ('22', '44.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('23', '45', '', '2', '0');
INSERT INTO `skuitems` VALUES ('24', '45.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('25', '46', '', '2', '0');
INSERT INTO `skuitems` VALUES ('26', '46.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('27', '47', '', '2', '0');
INSERT INTO `skuitems` VALUES ('28', '47.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('29', '48', '', '2', '0');
INSERT INTO `skuitems` VALUES ('30', '48.5', '', '2', '0');
INSERT INTO `skuitems` VALUES ('31', '49', '', '2', '0');

-- ----------------------------
-- Table structure for `storeinfo`
-- ----------------------------
DROP TABLE IF EXISTS `storeinfo`;
CREATE TABLE `storeinfo` (
  `storeId` int(11) NOT NULL AUTO_INCREMENT,
  `storeType` int(11) NOT NULL,
  `storeName` varchar(64) NOT NULL,
  `masterUserId` int(11) NOT NULL,
  `homeUrl` varchar(128) NOT NULL DEFAULT '',
  `IsSelfSupport` int(11) NOT NULL,
  `status` int(11) NOT NULL,
  `createDate` datetime NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`storeId`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of storeinfo
-- ----------------------------
INSERT INTO `storeinfo` VALUES ('15', '0', 'F01', '10018', '', '0', '0', '2016-12-07 21:14:22', '2016-12-07 21:27:34');
INSERT INTO `storeinfo` VALUES ('16', '0', 'F02', '10018', '', '0', '0', '2016-12-07 21:14:25', '2016-12-07 22:54:34');
INSERT INTO `storeinfo` VALUES ('17', '0', 'F01', '10017', '', '0', '0', '2016-12-07 21:25:24', '2016-12-07 21:25:24');
INSERT INTO `storeinfo` VALUES ('18', '0', 'F02', '10017', '', '0', '0', '2016-12-07 21:25:57', '2016-12-07 21:25:57');
INSERT INTO `storeinfo` VALUES ('19', '0', 'F03', '10018', '', '0', '0', '2016-12-07 21:27:48', '2016-12-07 21:27:48');
INSERT INTO `storeinfo` VALUES ('20', '0', 'F04', '10018', '', '0', '0', '2016-12-07 21:28:28', '2016-12-07 21:28:28');
INSERT INTO `storeinfo` VALUES ('21', '0', 'F01', '0', '', '1', '0', '2016-12-07 21:28:43', '2017-03-11 15:41:49');
INSERT INTO `storeinfo` VALUES ('22', '0', 'F02', '0', '', '1', '0', '2016-12-07 21:28:45', '2017-03-11 15:41:50');
INSERT INTO `storeinfo` VALUES ('23', '0', 'F03', '10019', '', '0', '0', '2016-12-07 21:28:47', '2016-12-07 21:28:46');
INSERT INTO `storeinfo` VALUES ('24', '0', 'F05', '10018', '', '0', '0', '2016-12-07 22:46:20', '2016-12-07 22:46:19');
INSERT INTO `storeinfo` VALUES ('25', '0', 'F06', '10018', '', '0', '0', '2016-12-07 22:54:20', '2016-12-07 22:54:19');

-- ----------------------------
-- Table structure for `userinfo`
-- ----------------------------
DROP TABLE IF EXISTS `userinfo`;
CREATE TABLE `userinfo` (
  `userId` int(11) NOT NULL AUTO_INCREMENT,
  `userName` varchar(32) NOT NULL,
  `nickName` varchar(32) NOT NULL,
  `userRole` int(11) NOT NULL,
  `avatar` varchar(128) NOT NULL,
  `passWord` varchar(128) NOT NULL,
  `salt` varchar(64) NOT NULL,
  `status` int(11) NOT NULL,
  `createDate` datetime NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=10020 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userinfo
-- ----------------------------
INSERT INTO `userinfo` VALUES ('10017', 'admin', 'admin', '1', '', 'opsVM4GPP8ZOdMI9huXnfnyE7Dc=', '9d5a9c0c32bf4e12b7d1cc5044e1d24a3ae1c4dfd11645e19d82d21d7e59b591', '0', '2016-12-07 20:41:15', '2016-12-11 23:23:54');
INSERT INTO `userinfo` VALUES ('10018', 'L-呵呵', 'skyvsmm', '3', ' ', 'opsVM4GPP8ZOdMI9huXnfnyE7Dc=', '9d5a9c0c32bf4e12b7d1cc5044e1d24a3ae1c4dfd11645e19d82d21d7e59b591', '1', '2017-03-09 15:21:07', '2017-03-09 15:21:07');

-- ----------------------------
-- Table structure for `userproductprice`
-- ----------------------------
DROP TABLE IF EXISTS `userproductprice`;
CREATE TABLE `userproductprice` (
  `priceId` int(11) NOT NULL AUTO_INCREMENT,
  `baseId` int(11) NOT NULL,
  `brandId` int(11) NOT NULL,
  `productId` int(11) NOT NULL,
  `userId` int(11) NOT NULL,
  `originalPrice` decimal(6,2) NOT NULL,
  `actualPrice` decimal(6,2) NOT NULL,
  `status` int(11) NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`priceId`),
  UNIQUE KEY `unique_user_product_id` (`productId`,`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userproductprice
-- ----------------------------
INSERT INTO `userproductprice` VALUES ('13', '10', '1', '10039', '10018', '599.00', '250.00', '0', '2017-03-09 17:01:50');
INSERT INTO `userproductprice` VALUES ('14', '11', '1', '10040', '10018', '999.00', '250.00', '0', '2017-03-09 17:02:02');
INSERT INTO `userproductprice` VALUES ('15', '11', '1', '10041', '10018', '999.00', '250.00', '0', '2017-03-09 17:02:03');
