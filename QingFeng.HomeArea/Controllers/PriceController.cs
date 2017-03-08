using QingFeng.Business;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QingFeng.WebArea.Controllers
{
    public class PriceController : CustomerController
    {
        private readonly UserService _userService = new UserService();
        private readonly ProductService _productService = new ProductService();


        public ActionResult Index(int userId = 0, string keyWord = "", int brandId = 1, int categoryId = 0, int page = 1,
            int pageSize = 30)
        {
            var totalItem = 0;
            var list = new List<ProductBase>();
            var userPrice = new Dictionary<int, UserProductPrice>();

            if (userId > 0)
            {
                list = _productService.SearchBaseProduct(keyWord, categoryId, 0, page, pageSize, out totalItem).ToList();

                userPrice = _userService.GetUserPrice(userId, brandId, list.Select(t => t.BaseId).ToArray())
                    .ToDictionary(c => c.ProductId, c => c);
            }

            var allUsers = _userService.GetList(new { UserRole = 3 }).ToList();

            ViewBag.brandId = brandId;
            ViewBag.categoryId = categoryId;
            ViewBag.keyWord = keyWord;
            ViewBag.userId = userId;
            ViewBag.userPrice = userPrice;
            ViewBag.userList = allUsers;

            return View(new ApiPageList<ProductBase>
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalItem,
                PageList = list
            });
        }

        //设置价格
        public ActionResult DistributorSet(int userId, int baseId)
        {
            return View();
        }

        //批量EXCEL导入
        public ActionResult DistributorImport()
        {
            var allUsers = _userService.GetList(new { UserRole = 3 }).ToList();

            return View(allUsers);
        }
    }
}