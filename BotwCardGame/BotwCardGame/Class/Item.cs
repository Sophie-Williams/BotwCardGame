using BotwCardGame.Ressources;

namespace BotwCardGame.Class
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Damage { get; set; }
        public ItemType Type { get; set; }
        public BoostSkill BoostSkill { get; set; }
        public Equipment Equipment { get; set; }
    }
}
