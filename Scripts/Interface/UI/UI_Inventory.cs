using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{

    /*
        Class used to instantiate the slots, manage what's inside of them, hold the items in memory
        hold the gold,silver and bronze and also for loot when the items are picked up
    */

    /// <summary>
    /// Currencies
    /// </summary>
    [SerializeField] public Text Gold, Silver, Copper;

    /// <summary>
    /// Slot prefab to be instantiated according to the slots count
    /// </summary>
    [SerializeField] public GameObject SlotPrefab;

    /// <summary>
    /// Maximum slots defined for slots content
    /// </summary>
    [SerializeField] public int Slots = 42; //stands for the maximum slots

    /// <summary>
    /// Button designed to control the close of this window
    /// </summary>
    [SerializeField] public Button CloseButton;

    /// <summary>
    /// If this state is active, it will instantiate the slots
    /// </summary>
    [SerializeField] public bool GenerateSlots = false;

    /// <summary>
    /// If this state is active, it will generate the items into the slot according to their position
    /// </summary>
    [SerializeField] public bool GenerateDatabaseItems = false;

    /// <summary>
    /// Instance of player inventory, also soon make this instance to be created in game data and not here
    /// </summary>
    private Inventory m_inventory;

    protected virtual void Start()
    {
        //TODO: Retrieve from GameData.CharacterData.Inv the inventory but if the player hasn't have an inventory yet we create an instance here!
        m_inventory = new Inventory();

        if (CloseButton != null)
            CloseButton.onClick.AddListener(() => GUI_Manager.instance.inventoryWindow.SetActive(false));

        if (GenerateSlots)
        {
            for (int i = 1; i < Slots; i++)
            {
                GameObject slot = Instantiate(SlotPrefab, GameObject.Find("Slots").transform);
                slot.gameObject.name = $"Slot ({i})";
                slot.GetComponent<UI_Slot>().Slot_Index_Position = i;
                slot.transform.SetSiblingIndex(i);
                slot.GetComponent<UI_Slot>().u_inventory = this;

                if (slot.GetComponent<UI_Slot>().ItemData != null && slot.GetComponent<UI_Slot>().ItemData.Icon != null)
                {
                    GameObject icon = Instantiate(slot.GetComponent<UI_Slot>().IconPrefab, slot.gameObject.transform);
                    icon.name = "Icon";
                    icon.transform.SetAsFirstSibling();
                    icon.GetComponent<Image>().sprite = slot.GetComponent<UI_Slot>().ItemData.Icon;
                    if (!icon.gameObject.activeSelf) icon.gameObject.SetActive(true);
                    slot.GetComponent<UI_Slot>().IconGraphic = icon.gameObject;
                }

            }
        }

        if (GenerateDatabaseItems)
        {
            //Loop through slots
            foreach(Transform slot in transform.Find("Slots").transform)
            {
                if (slot.GetComponent<UI_Slot>())
                {
                    if (m_inventory.DatabaseItems.ContainsKey(slot.GetComponent<UI_Slot>().Slot_Index_Position))
                    {
                        //means we have the slot
                        //and now lets do the work there etc
                        int slot_index = slot.GetComponent<UI_Slot>().Slot_Index_Position;
                        AssignItem(m_inventory.GetSingleItem(slot_index), slot_index);
                        slot.GetComponent<UI_Slot>().UpdateItemUI();
                    }
                }
            }
        }

        //Update currency text
        UpdateCurrencys();
    }

    /// <summary>
    /// Updates the GUI Currencys
    /// </summary>
    public void UpdateCurrencys()
    {
        Gold.text = m_inventory.Gold.ToString();
        Silver.text = m_inventory.Silver.ToString();
        Copper.text = m_inventory.Copper.ToString();
    }

    /// <summary>
    /// Updates the Slot GUI
    /// </summary>
    /// <param name="newItem"></param>
    /// <param name="slot"></param>
    public void OnItemChange(Item newItem, GameObject slot)
    {
        slot.GetComponent<UI_Slot>().ItemData = newItem;
        slot.GetComponent<Image>().sprite = newItem.Icon;
        slot.GetComponent<Image>().gameObject.SetActive(true);
        slot.transform.SetAsFirstSibling();
        Debug.Log($"The slot {slot.gameObject.name} data has been changed with the new item : {newItem.Name}");
    }

    public void OnItemThrow()
    {

    }

    public void OnItemSwap()
    {

    }

    /// <summary>
    /// Receives an item and optional the slot_id (default = -1 means we'll find a free slot)
    /// </summary>
    /// <param name="i"></param>
    /// <param name="slot_id"></param>
    public void AssignItem(Item i, int slot_id = -1)
    {
        //Make sure we have slots
        if (Slots > 0)
        {
            foreach (Transform slot in transform.Find("Slots").gameObject.transform)
            {
                if (slot.gameObject.GetComponent<UI_Slot>())
                {
                    if (slot_id != -1)
                    {
                        if (slot.gameObject.GetComponent<UI_Slot>().Slot_Index_Position == slot_id)
                        {
                            //slot.gameObject.GetComponent<UI_Slot>().ChangeItem(i);
                            m_inventory.AddOrUpdateItem(slot_id, i);
                            break; //we just want to switch 1 not more so lets break here
                        }
                    }
                    else
                    {
                        //slot.gameObject.GetComponent<UI_Slot>().ChangeItem(i);
                        m_inventory.AddOrUpdateItem(slot.gameObject.GetComponent<UI_Slot>().Slot_Index_Position, i);
                        break; //we just want to switch 1 not more so lets break here
                    }
                }
            }
        }
        else
            Debug.LogError("Please be sure of having slots before trying to assign an item");
    }
}
