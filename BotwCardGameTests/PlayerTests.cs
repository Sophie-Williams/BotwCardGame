using System;
using BotwCardGame.Class;
using BotwCardGame.Ressources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BotwCardGameTests
{
    [TestClass]
    public class PlayerTests
    {
        private Player _player;
        private const int ExistingItemNumber = 2;

        [TestInitialize]
        public void TestInit()
        {
            _player = new Player();
        }

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

        [TestMethod]
        public void PlayerHappyPathTest()
        {
            Assert.IsTrue(_player.Life == 3);
            Assert.IsTrue(_player.Inventory.Count == 0);
            Assert.IsTrue(_player.ExistingItemsList.Count == ExistingItemNumber);

            AddItemToInventory(1, false, 1, "name", "description", 1);
            AddItemToInventory(2, false, 2, "name2", "description2", 1);
            AddItemToInventory(1, false, 2, "name", "description", 2);
            AddItemToInventory(-1, true, 2);

            RemoveItemFromInventory(-1, true, 2);
            RemoveItemFromInventory(1, false, 2, 1);
            RemoveItemFromInventory(2, false, 1);
            RemoveItemFromInventory(1, false);
            RemoveItemFromInventory(1, true);
        }
    }
}
