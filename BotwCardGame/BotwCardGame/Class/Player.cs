using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BotwCardGame.Ressources;
using BotwCardGame.Templates;
using Nancy.Json;

namespace BotwCardGame.Class
{
    public class Player : IPlayer
    {
        public Player()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Constants)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("BotwCardGame.Ressources.ItemList.json");
            var streamReader = new StreamReader(stream);
            var json = streamReader.ReadToEnd();
            ExistingItemsList = new JavaScriptSerializer().Deserialize<List<Item>>(json);
            Life = 3;
            Inventory = new List<Item>(); 
        }

        public Constants AddItemToInventory(int id)
        {
            //Verification que l'item existe dans la liste complete
            if (ExistingItemsList.Exists(i => i.Id == id))
            {
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

            return Constants.NotFound;
        }

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

        public int Life { get; set; }
        public List<Item> Inventory { get; set; }
        public List<Item> ExistingItemsList { get; }
    }
}
