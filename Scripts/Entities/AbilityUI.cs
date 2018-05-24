using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class AbilityUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField, Tooltip("The canvas group holding this")] public GameObject Canvas;
    [SerializeField, Tooltip("Contains the scriptable object of this skill with every data")] public Ability SkillData;

    private Vector3 startPosition;
    private Transform rootParent;

    protected virtual void Awake()
    {
        if (Canvas == null) Canvas = GameObject.Find("UI").gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.localPosition;
        rootParent = transform.parent;
        gameObject.transform.SetParent(Canvas.transform);
        GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = true;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    /// <summary>
    /// OnDrag we bind the ability to the mouse
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    /// <summary>
    /// OnEnd we check if is a skill slot, if yes we bind else we return
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        var hitObject = eventData.pointerCurrentRaycast.gameObject;
        //Debug.Log("hit object: " + hitObject);
        
        //Verify if the object first hits something different of the target
        if (hitObject != null)
        {
            //After verify if is in cooldown
            if (hitObject.gameObject.name == "Cooldown" || hitObject.gameObject.name == "DarkOverlay")
            {
                StartCoroutine(GUI_Manager.instance.DisplayWarningBox("You can't change skills in cooldown", 1f));
                ResetPosition();
                GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = false;
                return;
            }

            //Has slot actions script? so it's eligible to have a skill
            if (hitObject.gameObject.GetComponent<SlotActions>())
            {
                bool alreadyExists = false;
                //Loop trought the childs of ability slots
                foreach (Transform child in GameObject.Find("AbilitiesSlots").transform)
                {
                    //Make sure the skill does not exist already
                    if (child.GetComponent<SlotActions>().SkillData == SkillData)
                    {
                        alreadyExists = true;
                        Debug.Log("The skill : " + child.gameObject.name + " is already in action bar");
                        break;
                    }
                }

                if (!alreadyExists)
                {
                    Debug.Log("The skill does not exist yet so lets add");
                    hitObject.gameObject.GetComponent<SlotActions>().SkillData = SkillData;
                    hitObject.gameObject.GetComponent<SlotActions>().OnAbilitySwap();
                }

            }

            ResetPosition();
            GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = false;
        }
        else
        {
            ResetPosition();
            GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = false;
        }
    }

    public void ResetPosition()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameObject.transform.SetParent(rootParent);
        if (transform.localPosition != startPosition)
            transform.localPosition = startPosition;
    }


}
