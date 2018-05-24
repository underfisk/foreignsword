using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
*
*  Class responsible for the item, save the scriptable object and also to move item, delete from inv etc
*  TODO Important: Delete the icon instnatiate object and also instantiate when we have
*/

//Rename class to UI_Slot
[RequireComponent(typeof(CanvasGroup))]
public class UI_Slot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Item Data")]
    /// <summary>
    /// Saves the item data here, if is null we check on the UI Manager
    /// </summary>
    [SerializeField] public Item ItemData;
    [SerializeField] public UI_Inventory u_inventory;

    [SerializeField] public int Slot_Index_Position = 0;

    [Header("Icon Properties")]
    /// <summary>
    /// Used for graphic content such as the Icon
    /// </summary>
    [SerializeField] public GameObject IconPrefab;

    /// <summary>
    /// If there is an image, we'll figure out the icon on the transform
    /// </summary>
    [SerializeField] public GameObject IconGraphic;


    [Header("Hover/Press Target Properties")]
    /// <summary>
    /// Hover overlay image
    /// </summary>
    [SerializeField] public Image HoverGraphic;

    /// <summary>
    /// Pressed overlay image
    /// </summary>
    [SerializeField] public Image PressedGraphic;

    /// <summary>
    /// Turns on or off wheter the item is avaiable
    /// </summary>
    [SerializeField] public Image AvailableOverlay;


    /// <summary>
    /// Internal data but will be search dinamicly the canvas
    /// </summary>
    private GameObject canvas;

    /// <summary>
    /// Start position is saved to know the main position where the item was
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// Root parent = slot
    /// </summary>
    private Transform rootParent;


    protected virtual void Start()
    {
        if (canvas == null) canvas = GameObject.Find("UI").gameObject;
        if (u_inventory == null) Debug.LogError("Please add UI Inventory to UI_Item slot");
    }

    /// <summary>
    /// Defines some important data also allows dragg
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Means we have an item to move
        if (ItemData != null)
        {
            startPosition = IconGraphic.transform.localPosition;
            rootParent = gameObject.transform;
            IconGraphic.transform.SetParent(canvas.transform);
            GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = true;
            GetComponent<CanvasGroup>().blocksRaycasts = false;

            //Highlight every available slot we can set data in
            HighLightSlots(true);
        }
    }

    public void HighLightSlots(bool visible = true)
    {
        foreach (Transform slot in rootParent.transform.parent.transform)
        {
            if (slot.gameObject.GetComponent<UI_Slot>())
            {
                if (slot.gameObject.GetComponent<UI_Slot>().ItemData == null)
                {
                    if (visible)
                        slot.gameObject.GetComponent<UI_Slot>().AvailableOverlay.gameObject.SetActive(true);
                    else
                        slot.gameObject.GetComponent<UI_Slot>().AvailableOverlay.gameObject.SetActive(false);
                }
            }
        }

    }

    /// <summary>
    /// Binds the cloneTarget(Icon) to the mouse
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (ItemData != null)
            IconGraphic.transform.position = Input.mousePosition;
    }

    /// <summary>
    /// Verify if the target is a slot, if has any item etc
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (ItemData != null)
        {
            var hitObject = eventData.pointerCurrentRaycast.gameObject;

            //Are we throwing out the item? 
            if (hitObject == null)
            {
                //Before throw check if the item has stacks and if yes we show a box so the user may choose the amount to throw out
                //Throw the item out 
                ResetDefault(); //temporary
                return;

            }

            Debug.Log("Hit object: " + hitObject.gameObject.name);

            //Are we hitting an overlay? ok no problem lets check if he has a parent with slot
            if (hitObject.name == "Available Overlay" || hitObject.name == "Hover Overlay" || hitObject.name == "Press Overlay")
            {
                var slot = hitObject.transform.parent.GetComponent<UI_Slot>();
                if (slot != null)
                {
                    Debug.Log("We hit an hover overlay and his parent is : " + slot.gameObject.name);
                    if (slot.ItemData != null)
                    {
                        VerifySwapOrChange(slot);
                    }
                }
            }

            if (hitObject != null)
            {
                if (hitObject.GetComponent<UI_Slot>()) //is it a slot?
                {
                    Debug.Log("We hit a slot" + hitObject.gameObject.name);
                    var slot = hitObject.GetComponent<UI_Slot>();
                    VerifySwapOrChange(slot);
                }

                /*
                else if (hitObject.GetComponent<UI_GearSlot>())
                {
                 TODO: Create GearSlot script which will hold the gear in part
                 and when switched for the gear slot we need to also update the stats
                }*/
            }

            //Reset to default values
            ResetDefault();
        }

    }


    public void VerifySwapOrChange(UI_Slot slot)
    {
        if (slot.ItemData != null)
        {
            //he has an item on it so lets perfom swap
            Debug.Log($"Perfom swap with the item : {slot.ItemData.Name} and this item {ItemData.name} ");
        }
        else
        {
            Debug.Log("Theres no item on it");
            //he doesn't have lets just add on it
            //u_inventory.OnItemChange(ItemData,hitObject.transform.Find("Icon").gameObject);
        }
    }

    /// <summary>
    /// Resets the values to default and enable/disable the necessary things
    /// </summary>
    public void ResetDefault()
    {
        //Disable highlight
        HighLightSlots(false);

        GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = false;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        //Slot parent will return to Slots
        IconGraphic.transform.SetParent(rootParent);


        if (IconGraphic.transform.localPosition != startPosition)
            IconGraphic.transform.localPosition = startPosition;

    }

    /// <summary>
    /// Handles the mouse pointer enter event to show ToolTip
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!HoverGraphic.gameObject.activeSelf)
            HoverGraphic.gameObject.SetActive(true);

        var hitObject = eventData.pointerCurrentRaycast.gameObject;
        if (hitObject.gameObject.GetComponent<UI_Slot>())
        {
            //Debug.Log($"The {hitObject.gameObject.name} is of type UISlot");
            //So lets show the tooltip of this item
            var slot = hitObject.gameObject.GetComponent<UI_Slot>();
            if (slot.ItemData != null)
            {
                //We have an item here so lets prepare our tooltip and send the necessary info
            }
        }
    }

    /// <summary>
    /// Handles the mouse pointer exit event and destroys the tooltip
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (HoverGraphic.gameObject.activeSelf)
            HoverGraphic.gameObject.SetActive(false);

        if (PressedGraphic.gameObject.activeSelf)
            PressedGraphic.gameObject.SetActive(false);


        //Call tooltip class and destroy any tooltip we created
    }

    //TODO : On Swap gameObject.transform.SetAsFirstSibling(); set as first the icon

    /// <summary>
    /// Handles the mouse click event
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (HoverGraphic.gameObject.activeSelf)
            HoverGraphic.gameObject.SetActive(false);

        if (!PressedGraphic.gameObject.activeSelf)
            PressedGraphic.gameObject.SetActive(true);

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Right click");
        else if (Input.GetMouseButtonDown(0))
            Debug.Log("Left click");
    }

    public void UpdateItemUI()
    {
        //Update the UI of it and also we instantiate the icon prefab
        
    }
}
