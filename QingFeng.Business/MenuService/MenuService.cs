using QingFeng.DataAccessLayer.Repository;
using QingFeng.Models;

namespace QingFeng.Business.MenuService
{
    public class MenuService
    {
        private readonly MenusRepository _menusRepository = new MenusRepository();

        public MenuModel Get()
        {
            int total = 0;
            var list = _menusRepository.GetPageList(new {menuId = 1}, "menuId", 1, 10, out total);
            return _menusRepository.Get(new {menuId = 1});
        }
    }
}
