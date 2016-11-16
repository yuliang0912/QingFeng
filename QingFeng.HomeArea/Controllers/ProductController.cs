using System.Collections.Generic;
using System.Web.Mvc;
using QingFeng.Business;
using QingFeng.Common;
using QingFeng.Common.ApiCore;
using QingFeng.Common.ApiCore.Result;
using QingFeng.Models;
using QingFeng.WebArea.Fillter;

namespace QingFeng.WebArea.Controllers
{
    [AdminAuthorize(AgentEnums.UserRole.Administrator)]
    public class ProductController : CustomerController
    {
        private readonly ProductService _productService = new ProductService();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddBaseProduct(ProductBase model)
        {
            if (model == null)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "数据接收失败"});
            }

            var result = _productService.CreateBaseProduct(model);

            return Json(new ApiResult<bool>(result));
        }

        public JsonResult CreateProduct(int baseId, List<KeyValuePair<int, string>> colorSku)
        {
            var baseProduct = _productService.GetProductBase(baseId);

            if (null == baseProduct)
            {
                return Json(new ApiResult<int>(2) {Ret = RetEum.ApplicationError, Message = "未找到商品"});
            }
            if (colorSku == null || colorSku.Count < 1)
            {
                return Json(new ApiResult<int>(3) {Ret = RetEum.ApplicationError, Message = "颜色SKU必须指定一个"});
            }
            var result = _productService.CreateProduct(baseId, colorSku);

            return Json(new ApiResult<int>(result));
        }
    }
}