using BotwCardGame.Ressources.Enums;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace BotwCardGame.Models
{
    public class Equipment
    {
        public EquipmentType Type { get; set; }
        public int Value { get; set; }
        public bool IsEquiped { get; set; }
    }
}
