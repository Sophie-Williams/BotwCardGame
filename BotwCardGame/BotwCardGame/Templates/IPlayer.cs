using System.Collections.Generic;
using BotwCardGame.Class;
using BotwCardGame.Ressources;

namespace BotwCardGame.Templates
{
    public interface IPlayer
    {
        Constants AddItemToInventory(int id);
        Constants RemoveItemFromInventory(int id);

        int Life { get; set; }
        List<Item> Inventory { get; set; }
        List<Item> ExistingItemsList { get; }
    }
}