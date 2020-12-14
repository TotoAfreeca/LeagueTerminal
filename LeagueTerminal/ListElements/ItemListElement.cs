using RiotSharp.Endpoints.StaticDataEndpoint.Item;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueTerminal.LeagueUtils
{
    public class ItemListElement
    {
        public ItemStatic Item{ get; set; }
        public string ItemName { get; set; }

        public ItemListElement(ItemStatic item)
        {
            Item = item;
            ItemName = item.Name;
        }

        public override string ToString()
        {
            return Item.Name;
        }
    }
}
