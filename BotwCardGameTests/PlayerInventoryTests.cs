using BotwCardGame.Class;
using BotwCardGame.Ressources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BotwCardGameTests
{
    [TestClass]
    public class PlayerInventoryTests
    {
        private Player _player;
        private const int ExistingItemNumber = 3;

        [TestInitialize]
        public void TestInit()
        {
            _player = new Player();
        }

        [TestMethod]
        public void PlayerHappyPathTest()
        {
            Assert.IsTrue(_player.Life == 3);
            Assert.IsTrue(_player.Inventory.Count == 0);
            Assert.IsTrue(_player.ExistingItemsList.Count == ExistingItemNumber);

            AddItemToInventory(101, false, 1, "Coeur de Vie", "Rajoute 1 PV max au joueur", 1);
            AddItemToInventory(201, false, 2, "Master Sword", "Epee de Legende", 1);
            AddItemToInventory(101, false, 2, "Coeur de Vie", "Rajoute 1 PV max au joueur", 2);
            AddItemToInventory(-1, true, 2);

            RemoveItemFromInventory(-1, true, 2);
            RemoveItemFromInventory(101, false, 2, 1);
            RemoveItemFromInventory(201, false, 1);
            RemoveItemFromInventory(101, false);
            RemoveItemFromInventory(101, true);
        }

        #region MaxHealth

        [TestMethod]
        public void PlayerAddMaxHealthTest()
        {
            AddItemToInventory(101, false, 1, "Coeur de Vie", "Rajoute 1 PV max au joueur", 1);
            Assert.AreEqual(_player.MaxLife, 3);
            Assert.AreEqual(_player.Life, 3);
            Assert.AreEqual(_player.Inventory.Count, 1);

            var ret = _player.UseItemFromInventory(101);
            Assert.AreEqual(ret, Constants.Boosted);
            Assert.AreEqual(_player.Inventory.Count, 0);
            Assert.AreEqual(_player.Life, 4);
            Assert.AreEqual(_player.MaxLife, 4);
        }

        #endregion

        #region Health

        [TestMethod]
        public void PlayerWorkingRestoreHealthTest()
        {
            AddItemToInventory(102, false, 1, "Venaison Divine", "Venaison Divine", 1);
            Assert.AreEqual(_player.MaxLife, 3);
            Assert.AreEqual(_player.Life, 3);
            Assert.AreEqual(_player.Inventory.Count, 1);

            _player.Life = 1;
            Assert.AreEqual(_player.MaxLife, 3);
            Assert.AreEqual(_player.Life, 1);
            Assert.AreEqual(_player.Inventory.Count, 1);

            var ret = _player.UseItemFromInventory(102);
            Assert.AreEqual(ret, Constants.Boosted);
            Assert.AreEqual(_player.Inventory.Count, 0);
            Assert.AreEqual(_player.Life, 3);
            Assert.AreEqual(_player.MaxLife, 3);
        }

        [TestMethod]
        public void PlayerNotWorkingRestoreHealthTest()
        {
            AddItemToInventory(102, false, 1, "Venaison Divine", "Venaison Divine", 1);
            Assert.AreEqual(_player.MaxLife, 3);
            Assert.AreEqual(_player.Life, 3);
            Assert.AreEqual(_player.Inventory.Count, 1);

            var ret = _player.UseItemFromInventory(102);
            Assert.AreEqual(ret, Constants.Failed);
            Assert.AreEqual(_player.Inventory.Count, 0);
            Assert.AreEqual(_player.Life, 3);
            Assert.AreEqual(_player.MaxLife, 3);
        }

        #endregion

        #region Weapon

        [TestMethod]
        public void PlayerEquipWeaponTest()
        {
            AddItemToInventory(201, false, 1, "Master Sword", "Epee de Legende", 1);

            var ret = _player.UseItemFromInventory(201);
            Assert.AreEqual(ret, Constants.Equiped);
            Assert.IsNotNull(_player.EquipmentBody.Weapon);
            Assert.AreEqual(_player.EquipmentBody.Weapon.Id, 201);
            Assert.AreEqual(_player.EquipmentBody.Weapon.Name, "Master Sword");
            Assert.AreEqual(_player.EquipmentBody.Weapon.Description, "Epee de Legende");
            Assert.AreEqual(_player.EquipmentBody.Weapon.Quantity, 1);
            Assert.AreEqual(_player.EquipmentBody.Weapon.Type, ItemType.Equipment);
            Assert.AreEqual(_player.EquipmentBody.Weapon.Equipment.Type, EquipmentType.Sword);
            Assert.AreEqual(_player.EquipmentBody.Weapon.Equipment.Value, 60);
            Assert.AreEqual(_player.Inventory.Find(i => i.Id == 201).Equipment.IsEquiped, true);
        }

        [TestMethod]
        public void PlayerRemoveWeaponTest()
        {
            AddItemToInventory(201, false, 1, "Master Sword", "Epee de Legende", 1);
            var ret = _player.UseItemFromInventory(201);
            Assert.AreEqual(ret, Constants.Equiped);
            Assert.IsNotNull(_player.EquipmentBody.Weapon);

            ret = _player.RemoveEquipment(EquipmentBodyType.Weapon);
            Assert.AreEqual(ret, Constants.Removed);
            Assert.IsNull(_player.EquipmentBody.Weapon);
        }

        #endregion

        #region Private Inventory Management Methods

        private void AddItemToInventory(int id, bool mustFail, int inventoryCount, string name = null, string description = null, int quantity = 0)
        {
            var ret = _player.AddItemToInventory(id);
            if (mustFail == false)
            {
                Assert.AreEqual(ret, Constants.Added);

                var item = _player.Inventory.Find(i => i.Id == id);
                Assert.IsNotNull(item);
                Assert.AreEqual(item.Name, name);
                Assert.AreEqual(item.Description, description);
                Assert.AreEqual(item.Quantity, quantity);
            }
            else
                Assert.AreEqual(ret, Constants.NotFound);
            Assert.AreEqual(_player.Inventory.Count, inventoryCount);
        }

        private void RemoveItemFromInventory(int id, bool mustFail, int inventoryCount = 0, int remainingItemNumber = 0)
        {
            var ret = _player.RemoveItemFromInventory(id);
            if (mustFail == false)
            {
                Assert.AreEqual(ret, Constants.Removed);

                if (remainingItemNumber != 0)
                    Assert.AreEqual(_player.Inventory.Find(i => i.Id == id).Quantity, remainingItemNumber);
                else
                    Assert.IsNull(_player.Inventory.Find(i => i.Id == id));
            }
            else
                Assert.AreEqual(ret, Constants.NotFound);
            Assert.AreEqual(_player.Inventory.Count, inventoryCount);
        }

        #endregion
    }
}
