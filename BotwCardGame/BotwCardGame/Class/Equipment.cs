using BotwCardGame.Ressources;

namespace BotwCardGame.Class
{
    public abstract class Equipment
    {
        public EquipmentType Type { get; set; }
        public int Value { get; set; }
        public bool IsEquiped { get; set; }
    }
}
