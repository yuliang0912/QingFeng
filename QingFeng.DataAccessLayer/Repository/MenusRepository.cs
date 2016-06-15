using QingFeng.Models;

namespace QingFeng.DataAccessLayer.Repository
{
    public class MenusRepository : RepositoryBase<MenuModel>
    {
        public MenusRepository() : base("qingfeng", "menus")
        {
        }
    }
}
