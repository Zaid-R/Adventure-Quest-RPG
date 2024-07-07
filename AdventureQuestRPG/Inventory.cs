using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureQuestRPG
{
    public class Inventory
    {
        public readonly List<Item> items;
        
        public Inventory()
        {
            items = new List<Item>();
        }

        public Inventory(List<Item> items) {
            this.items = new(items);
        }
        public void add(Item item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
            }
        }

        

        public void remove(Item item)
        {
            items.Remove(item);
        }
        public void display()
        {
            int counter = 1;
            Console.WriteLine("\t\tinventory:");
            foreach (Item item in items)
            {
                Console.Write($"\t\t[{counter++}] {item.Name} ({item.Description})");
            }
            Console.WriteLine();
        }

        public bool isEmpty()
        {
            return items.Count == 0;
        }

    }
}
