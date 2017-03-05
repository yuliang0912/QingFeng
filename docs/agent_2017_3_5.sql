/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50703
Source Host           : localhost:3306
Source Database       : agent

Target Server Type    : MYSQL
Target Server Version : 50703
File Encoding         : 65001

Date: 2017-03-05 22:49:49
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for logistics
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
-- Table structure for orderdetail
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
  `imgUrl` varchar(128) NOT NULL,
  `price` decimal(10,2) NOT NULL,
  `quantity` int(11) NOT NULL,
  `amount` decimal(10,2) NOT NULL,
  `remark` varchar(120) NOT NULL,
  `orderStatus` int(11) NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `skuId` int(11) NOT NULL,
  `skuName` varchar(30) NOT NULL,
  PRIMARY KEY (`flowId`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of orderdetail
-- ----------------------------
INSERT INTO `orderdetail` VALUES ('13', '1612090018410960807', '2545763666429514', '1', '7-29SPW1030', '女士休闲鞋', '10000', '7-29SPW1030-001', '女士休闲鞋-红色', '', '450.00', '1', '450.00', '', '1', '2016-12-09 00:18:41', '4', '37');
INSERT INTO `orderdetail` VALUES ('14', '1612090018410960807', '2545763666429514', '1', '7-29SPW1030', '女士休闲鞋', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '1', '2016-12-09 00:18:41', '5', '38');
INSERT INTO `orderdetail` VALUES ('15', '1612090026017362481', '2464604448941232', '1', '7-29SPW1030', '女士休闲鞋', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '1', '2016-12-09 00:26:01', '7', '41.5');
INSERT INTO `orderdetail` VALUES ('16', '1612090026017362481', '2464604448941232', '1', '7-29SPW1030', '女士休闲鞋', '10000', '7-29SPW1030-001', '女士休闲鞋-红色', '', '450.00', '1', '450.00', '', '1', '2016-12-09 00:26:01', '5', '38');
INSERT INTO `orderdetail` VALUES ('17', '1612090026535817624', '2409062472387623', '1', '7-29SPW1030', '女士休闲鞋', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '1', '2016-12-09 00:26:53', '5', '38');
INSERT INTO `orderdetail` VALUES ('18', '1612090026535817624', '2409062472387623', '1', '7-29SPW1030', '女士休闲鞋', '10000', '7-29SPW1030-001', '女士休闲鞋-红色', '', '450.00', '1', '450.00', '', '1', '2016-12-09 00:26:53', '4', '37');
INSERT INTO `orderdetail` VALUES ('19', '1612092337362780668', '25457636663', '1', '7-29SPW1030', '女士休闲鞋1', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '1', '2016-12-09 23:37:36', '4', '37');
INSERT INTO `orderdetail` VALUES ('20', '1612100004236724301', '44444444', '1', '7-29SPW1030', '女士休闲鞋1', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '2', '2016-12-10 16:27:01', '4', '37');
INSERT INTO `orderdetail` VALUES ('21', '1612100004236724301', '44444444', '1', '7-29SPW1030', '女士休闲鞋1', '10000', '7-29SPW1030-001', '女士休闲鞋-红色', '', '450.00', '1', '450.00', '', '2', '2016-12-10 16:26:48', '5', '38');
INSERT INTO `orderdetail` VALUES ('22', '1612100008204696367', '33', '1', '7-29SPW1030', '女士休闲鞋1', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '2', '2016-12-10 15:29:22', '4', '37');
INSERT INTO `orderdetail` VALUES ('23', '1612100008204696367', '33', '1', '7-29SPW1030', '女士休闲鞋1', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '2', '2016-12-10 15:29:54', '5', '38');
INSERT INTO `orderdetail` VALUES ('24', '1612100026541504420', '423432', '1', '7-29SPW1030', '女士休闲鞋1', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '2', '2016-12-10 15:38:57', '5', '38');
INSERT INTO `orderdetail` VALUES ('25', '1612100026541504420', '423432', '1', '7-29SPW1030', '女士休闲鞋1', '10001', '7-29SPW1030-003', '女士休闲鞋-蓝色', '', '450.00', '1', '450.00', '', '2', '2016-12-10 15:38:57', '6', '39');

-- ----------------------------
-- Table structure for orderlogs
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
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8;

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

-- ----------------------------
-- Table structure for ordermaster
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
  `userId` int(11) NOT NULL,
  `contactName` varchar(30) NOT NULL,
  `contactPhone` varchar(30) NOT NULL,
  `postCode` int(6) NOT NULL,
  `areaCode` varchar(20) NOT NULL,
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
INSERT INTO `ordermaster` VALUES ('1612090018410960807', '2545763666429514', '900.00', '0.00', '2', '17', 'F01', '10017', '闫武兵', '18633369676', '510145', '2199', '广东省 广州市 荔湾区 山市路北区高第花园3楼3门202', '1', '0', '0.00', '1', '1970-01-01 00:00:00', '呵呵', '2016-12-09 00:18:41', '2016-12-10 17:03:54');
INSERT INTO `ordermaster` VALUES ('1612090026017362481', '2464604448941232', '900.00', '0.00', '2', '17', 'F01', '10017', '卓宋值', '15759321905', '48400', '245', '山西省 晋城市 高平市 松城街道百丽花园6号楼106', '3', '0', '0.00', '1', '1970-01-01 00:00:00', '', '2016-12-09 00:26:02', '2016-12-10 18:03:41');
INSERT INTO `ordermaster` VALUES ('1612090026535817624', '2409062472387623', '900.00', '0.00', '2', '17', 'F01', '10017', '周卓', '15150001432', '300041', '24', '天津市 天津市 和平区 杨山路88号，江苏协鑫软控设备科技发展有限公司，物资部', '5', '0', '0.00', '1', '1970-01-01 00:00:00', '', '2016-12-09 00:26:54', '2016-12-10 17:50:57');
INSERT INTO `ordermaster` VALUES ('1612092337362780668', '25457636663', '450.00', '0.00', '1', '17', 'F01', '10017', '111', '11', '10001', '2', '北京 北京市 东城区 111', '5', '0', '0.00', '1', '1970-01-01 00:00:00', '', '2016-12-09 23:37:36', '2016-12-10 17:03:56');
INSERT INTO `ordermaster` VALUES ('1612100004236724301', '44444444', '900.00', '0.00', '2', '17', 'F01', '10017', '55555', '6666666', '45000', '245', '山西省 阳泉市 矿区 432432432', '7', '0', '0.00', '1', '1970-01-01 00:00:00', '', '2016-12-10 00:04:24', '2016-12-10 17:03:56');
INSERT INTO `ordermaster` VALUES ('1612100008204696367', '33', '900.00', '0.00', '2', '17', 'F01', '10017', '33', '33', '412000', '2048', '湖南省 株洲市 芦淞区 333', '7', '0', '0.00', '1', '1970-01-01 00:00:00', '', '2016-12-10 00:08:20', '2016-12-10 17:03:57');
INSERT INTO `ordermaster` VALUES ('1612100026541504420', '423432', '900.00', '0.00', '2', '17', 'F01', '10017', '432', '4324', '410002', '2048', '湖南省 长沙市 天心区 432423', '7', '0', '0.00', '1', '1970-01-01 00:00:00', '', '2016-12-10 00:26:54', '2016-12-10 17:03:58');

-- ----------------------------
-- Table structure for product
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
-- Table structure for productbase
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
-- Table structure for productskus
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
-- Table structure for productstock
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
  PRIMARY KEY (`stockId`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of productstock
-- ----------------------------
INSERT INTO `productstock` VALUES ('1', '1', '10000', '4', '37码', '88', '2016-12-10 20:15:25');
INSERT INTO `productstock` VALUES ('2', '1', '10000', '5', '38码', '88', '2016-11-25 23:23:17');
INSERT INTO `productstock` VALUES ('3', '1', '10000', '6', '39码', '99', '2016-12-10 20:10:58');
INSERT INTO `productstock` VALUES ('4', '1', '10001', '4', '37码', '66', '2016-11-25 23:23:19');
INSERT INTO `productstock` VALUES ('5', '1', '10001', '5', '38码', '55', '2016-11-25 23:23:20');
INSERT INTO `productstock` VALUES ('6', '1', '10001', '6', '39码', '44', '2016-11-25 23:23:21');
INSERT INTO `productstock` VALUES ('7', '1', '10001', '7', '41.5码', '33', '2016-11-25 23:23:23');
INSERT INTO `productstock` VALUES ('8', '1', '10001', '8', '42', '0', '2016-12-10 19:54:38');
INSERT INTO `productstock` VALUES ('9', '1', '10001', '9', '43', '0', '2016-12-10 19:54:46');
INSERT INTO `productstock` VALUES ('10', '1', '10001', '11', '43.5', '0', '2016-12-10 19:54:46');
INSERT INTO `productstock` VALUES ('11', '4', '10017', '6', '39', '2', '2016-12-10 20:11:33');
INSERT INTO `productstock` VALUES ('12', '4', '10017', '7', '41.5', '3', '2016-12-10 20:11:37');
INSERT INTO `productstock` VALUES ('13', '4', '10017', '8', '42', '4', '2016-12-10 20:11:40');
INSERT INTO `productstock` VALUES ('14', '4', '10017', '9', '43', '5', '2016-12-10 20:11:44');
INSERT INTO `productstock` VALUES ('15', '1', '10000', '7', '41.5', '55', '2016-12-10 20:11:13');
INSERT INTO `productstock` VALUES ('16', '1', '10000', '8', '42', '32', '2016-12-10 20:11:17');
INSERT INTO `productstock` VALUES ('17', '1', '10000', '11', '43.5', '8', '2016-12-10 20:11:21');
INSERT INTO `productstock` VALUES ('18', '4', '10017', '5', '38', '6', '2016-12-10 20:11:47');
INSERT INTO `productstock` VALUES ('19', '4', '10017', '4', '37', '50', '2016-12-10 20:12:02');
INSERT INTO `productstock` VALUES ('20', '4', '10017', '11', '43.5', '57', '2016-12-10 20:12:07');
INSERT INTO `productstock` VALUES ('21', '1', '10000', '9', '43', '7', '2016-12-10 20:15:52');
INSERT INTO `productstock` VALUES ('22', '2', '10020', '7', '41.5', '0', '2016-12-10 20:32:25');
INSERT INTO `productstock` VALUES ('23', '2', '10020', '8', '42', '0', '2016-12-10 20:32:25');
INSERT INTO `productstock` VALUES ('24', '2', '10020', '9', '43', '0', '2016-12-10 20:32:25');
INSERT INTO `productstock` VALUES ('25', '6', '10035', '15', '41', '0', '2017-03-02 23:04:05');
INSERT INTO `productstock` VALUES ('26', '6', '10035', '21', '44', '0', '2017-03-02 23:04:05');
INSERT INTO `productstock` VALUES ('27', '6', '10035', '27', '47', '0', '2017-03-02 23:04:05');

-- ----------------------------
-- Table structure for skuitems
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
-- Table structure for storeinfo
-- ----------------------------
DROP TABLE IF EXISTS `storeinfo`;
CREATE TABLE `storeinfo` (
  `storeId` int(11) NOT NULL AUTO_INCREMENT,
  `storeName` varchar(64) NOT NULL,
  `masterUserId` int(11) NOT NULL,
  `homeUrl` varchar(128) NOT NULL DEFAULT '',
  `status` int(11) NOT NULL,
  `createDate` datetime NOT NULL,
  `updateDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`storeId`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of storeinfo
-- ----------------------------
INSERT INTO `storeinfo` VALUES ('15', 'F01', '10018', '', '0', '2016-12-07 21:14:22', '2016-12-07 21:27:34');
INSERT INTO `storeinfo` VALUES ('16', 'F02', '10018', '', '0', '2016-12-07 21:14:25', '2016-12-07 22:54:34');
INSERT INTO `storeinfo` VALUES ('17', 'F01', '10017', '', '0', '2016-12-07 21:25:24', '2016-12-07 21:25:24');
INSERT INTO `storeinfo` VALUES ('18', 'F02', '10017', '', '0', '2016-12-07 21:25:57', '2016-12-07 21:25:57');
INSERT INTO `storeinfo` VALUES ('19', 'F03', '10018', '', '0', '2016-12-07 21:27:48', '2016-12-07 21:27:48');
INSERT INTO `storeinfo` VALUES ('20', 'F04', '10018', '', '0', '2016-12-07 21:28:28', '2016-12-07 21:28:28');
INSERT INTO `storeinfo` VALUES ('21', 'F01', '10019', '', '0', '2016-12-07 21:28:43', '2016-12-07 21:28:42');
INSERT INTO `storeinfo` VALUES ('22', 'F02', '10019', '', '0', '2016-12-07 21:28:45', '2016-12-07 21:28:44');
INSERT INTO `storeinfo` VALUES ('23', 'F03', '10019', '', '0', '2016-12-07 21:28:47', '2016-12-07 21:28:46');
INSERT INTO `storeinfo` VALUES ('24', 'F05', '10018', '', '0', '2016-12-07 22:46:20', '2016-12-07 22:46:19');
INSERT INTO `storeinfo` VALUES ('25', 'F06', '10018', '', '0', '2016-12-07 22:54:20', '2016-12-07 22:54:19');

-- ----------------------------
-- Table structure for userinfo
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
INSERT INTO `userinfo` VALUES ('10018', 'yuliang0912', 'yuliang1', '2', '/content/agent/img/user.jpg', 'Daktw/+XLZvU3RnnpHUexULBi/M=', '63c32de9d8f647c69bb350112599a7e584bf3ac7889444b994f5c03a170d48c1', '0', '2016-12-07 20:48:19', '2016-12-07 20:53:50');
INSERT INTO `userinfo` VALUES ('10019', 'yuliang09121', '我是牛魔王', '2', '/content/agent/img/user.jpg', 'F5UGIqc3DTnR3AeXmALCzq2Jbww=', '8402df963db241c298cec414e06ab8d3abfc2ee16afc428982b170efe0032512', '0', '2016-12-07 20:49:55', '2016-12-07 20:53:19');
