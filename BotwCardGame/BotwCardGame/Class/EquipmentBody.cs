namespace BotwCardGame.Class
{
    public class EquipmentBody
    {
        public EquipmentBody()
        {
            Head = null;
            Body = null;
            Foot = null;
            Weapon = null;
            Bow = null;
            Shield = null;
        }

        public Item Head { get; set; }
        public Item Body { get; set; }
        public Item Foot { get; set; }
        public Item Weapon { get; set; }
        public Item Bow { get; set; }
        public Item Shield { get; set; }
    }
}
