using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BotwCardGame.Models;
using BotwCardGame.Ressources.Enums;
using Newtonsoft.Json;

namespace BotwCardGame.Class
{
    public class Player
    {
        /// <summary>
        /// Init player
        /// </summary>
        public Player()
        {
            var stream = typeof(Constants).GetTypeInfo().Assembly.GetManifestResourceStream("BotwCardGame.Ressources.ItemList.json");
            var streamReader = new StreamReader(stream ?? throw new InvalidOperationException());
            var json = streamReader.ReadToEnd();
            ExistingItemsList = JsonConvert.DeserializeObject<List<Item>>(json);
            Life = 3;
            MaxLife = 3;
            Inventory = new List<Item>(); 
            EquipmentBody = new EquipmentBody();
        }

        /// <summary>
        /// Add an item to the player inventory
        /// </summary>
        /// <param name="id">Id of the item to add</param>
        public Constants AddItemToInventory(int id)
        {
            //Verification que l'item existe dans la liste complete
            if (!ExistingItemsList.Exists(i => i.Id == id))
                return Constants.NotFound;

            //Si on en as pas encore, on l'ajoute
            if (!Inventory.Exists(i => i.Id == id))
            {
                var item = ExistingItemsList.Find(i => i.Id == id);
                Inventory.Add(item);
            }

            //s'il existe on incremente la quantite
            else
                Inventory.Find(i => i.Id == id).Quantity += 1;

            return Constants.Added;
        }

        /// <summary>
        /// Remove an item from the player inventory
        /// </summary>
        /// <param name="id">Id of the item to remove</param>
        public Constants RemoveItemFromInventory(int id)
        {
            //Verification que l'item existe dans la liste complete & dans l'inventaire
            if (!ExistingItemsList.Exists(i => i.Id == id) || !Inventory.Exists(i => i.Id == id))
                return Constants.NotFound;

            //S'il n'y en as qu'un on le supprime
            if (Inventory.Find(i => i.Id == id).Quantity == 1)
                Inventory.RemoveAll(i => i.Id == id);

            //Sinon on le diminue de un
            else
                Inventory.Find(i => i.Id == id).Quantity -= 1;

            return Constants.Removed;
        }

        /// <summary>
        /// Use an item on the player inventory
        /// </summary>
        /// <param name="id">Id of the item to use</param>
        public Constants UseItemFromInventory(int id)
        {
            var item = Inventory.Find(i => i.Id == id);
            if (item == null)
                return Constants.NotFound;
            switch (item.Type)
            {
                case ItemType.Boost:
                    return UseBoostItem(item);
                case ItemType.Equipment:
                    return UseEquipmentItem(item);
                default:
                    return Constants.Ok;
            }
        }

        /// <summary>
        /// Remove an equipment from player body
        /// </summary>
        /// <param name="type">EquipmentBodyType of the equipment to remove</param>
        public Constants RemoveEquipment(EquipmentBodyType type)
        {
            switch (type)
            {
                case EquipmentBodyType.Head:
                    EquipmentBody.Head = null;
                    break;
                case EquipmentBodyType.Body:
                    EquipmentBody.Body = null;
                    break;
                case EquipmentBodyType.Foot:
                    EquipmentBody.Foot = null;
                    break;
                case EquipmentBodyType.Weapon:
                    EquipmentBody.Weapon = null;
                    break;
                case EquipmentBodyType.Bow:
                    EquipmentBody.Bow = null;
                    break;
                case EquipmentBodyType.Shield:
                    EquipmentBody.Shield = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return Constants.Removed;
        }

        /// <summary>
        /// Equip an equipment
        /// </summary>
        /// <param name="equipment">Item to equip</param>
        private Constants UseEquipmentItem(Item equipment)
        {
            switch (equipment.Equipment.Type)
            {
                case EquipmentType.Head:
                    if (EquipmentBody.Head != null)
                        Inventory.Find(i => i.Id == EquipmentBody.Head.Id).Equipment.IsEquiped = false;
                    EquipmentBody.Head = equipment;
                    Inventory.Find(i => i.Id == equipment.Id).Equipment.IsEquiped = true;
                    break;
                case EquipmentType.Body:
                    if (EquipmentBody.Body != null)
                        Inventory.Find(i => i.Id == EquipmentBody.Body.Id).Equipment.IsEquiped = false;
                    EquipmentBody.Body = equipment;
                    Inventory.Find(i => i.Id == equipment.Id).Equipment.IsEquiped = true;
                    break;
                case EquipmentType.Foot:
                    if (EquipmentBody.Foot != null)
                        Inventory.Find(i => i.Id == EquipmentBody.Foot.Id).Equipment.IsEquiped = false;
                    EquipmentBody.Foot = equipment;
                    Inventory.Find(i => i.Id == equipment.Id).Equipment.IsEquiped = true;
                    break;
                case EquipmentType.Sword:
                    if (EquipmentBody.Weapon != null)
                        Inventory.Find(i => i.Id == EquipmentBody.Weapon.Id).Equipment.IsEquiped = false;
                    EquipmentBody.Weapon = equipment;
                    Inventory.Find(i => i.Id == equipment.Id).Equipment.IsEquiped = true;
                    break;
                case EquipmentType.LongSword:
                    if (EquipmentBody.Weapon != null)
                        Inventory.Find(i => i.Id == EquipmentBody.Weapon.Id).Equipment.IsEquiped = false;
                    if (EquipmentBody.Shield != null)
                    {
                        Inventory.Find(i => i.Id == EquipmentBody.Shield.Id).Equipment.IsEquiped = false;
                        EquipmentBody.Shield = null;
                    }
                    EquipmentBody.Weapon = equipment;
                    Inventory.Find(i => i.Id == equipment.Id).Equipment.IsEquiped = true;
                    break;
                case EquipmentType.Bow:
                    if (EquipmentBody.Bow != null)
                        Inventory.Find(i => i.Id == EquipmentBody.Bow.Id).Equipment.IsEquiped = false;
                    EquipmentBody.Bow = equipment;
                    Inventory.Find(i => i.Id == equipment.Id).Equipment.IsEquiped = true;
                    break;
                case EquipmentType.Shield:
                    if (EquipmentBody.Shield != null)
                        Inventory.Find(i => i.Id == EquipmentBody.Shield.Id).Equipment.IsEquiped = false;
                    if (EquipmentBody.Weapon != null && EquipmentBody.Weapon.Equipment.Type == EquipmentType.LongSword)
                    {
                        Inventory.Find(i => i.Id == EquipmentBody.Weapon.Id).Equipment.IsEquiped = false;
                        EquipmentBody.Weapon = null;
                    }
                    EquipmentBody.Shield = equipment;
                    Inventory.Find(i => i.Id == equipment.Id).Equipment.IsEquiped = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Constants.Equiped;
        }

        /// <summary>
        /// Use a boost item
        /// </summary>
        /// <param name="item">Item to use</param>
        private Constants UseBoostItem(Item item)
        {
            switch (item.BoostSkill.BoostSkillType)
            {
                case BoostSkillType.MaxHealth:
                    MaxLife += item.BoostSkill.Value;
                    Life = MaxLife;
                    RemoveItemFromInventory(item.Id);
                    break;
                case BoostSkillType.Health:
                    RemoveItemFromInventory(item.Id);
                    if (Life == MaxLife)
                        return Constants.Failed;
                    Life += item.BoostSkill.Value;
                    if (Life > MaxLife)
                        Life = MaxLife;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Constants.Boosted;
        }

        public int Life { get; set; }
        public int MaxLife { get; private set; }
        public List<Item> Inventory { get; }
        public EquipmentBody EquipmentBody { get; }
        public List<Item> ExistingItemsList { get; }
    }
}
