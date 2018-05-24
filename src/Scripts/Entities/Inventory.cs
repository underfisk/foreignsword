using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Internal class standed for UI_Inventory as the instance of inv and UI_Inventory just uses it 
*/
public class Inventory
{
    /// <summary>
    /// int -> Slot index
    /// Item -> Item object
    /// </summary>
    private Dictionary<int, Item> items;
   
    public double Gold, Silver, Copper;
    
    
    public Dictionary<int,Item> DatabaseItems
    {
        get { return this.items; }
        private set { }
    }

    

    
    /// <summary>
    /// Verifys if has the database item inside, if yes update if no we add it for when we update this inv to database we will have it
    /// </summary>
    /// <param name="slot_index"></param>
    /// <param name="i"></param>
    public void AddOrUpdateItem(int slot_index, Item i)
    {
        Item val;
        if (items.TryGetValue(slot_index,out val))
            items[slot_index] = i;
        else
            items.Add(slot_index, i);
    }

    public Item GetSingleItem(int slot_index)
    {
        Item i;
        if (items.TryGetValue(slot_index, out i))
            return i;
        else
            return null;
    }

    //public Inventory(uint slot, Item i, uint quant, Sprite ico)
    //{
    //    this.slot_id = slot;
    //    this._item = i;
    //    this.quantity = quant;
    //    this.icon = ico;
    //}

    //private Inventory(Inventory obj)
    //{
    //    this.slot_id = obj.slot_id;
    //    this._item = obj._item;
    //    this.quantity = obj.quantity;
    //    this.icon = obj.icon;
    //}
}
