namespace QingFeng.Models
{
    public class MenuModel
    {
        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public int MenuType { get; set; }

        public int Level { get; set; }

        public int OrderId { get; set; }

        public int ParentId { get; set; }
    }
}
