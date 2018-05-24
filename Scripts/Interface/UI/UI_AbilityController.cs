using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]
public class UI_AbilityController : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Vector3 startPosition;
    private Transform rootParent;
    private GameObject Canvas;

    public void Awake()
    {
        Canvas = GameObject.Find("UI").gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.localPosition;
        rootParent = transform.parent;
        gameObject.transform.SetParent(Canvas.transform);
        GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = true;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var hitObject = eventData.pointerCurrentRaycast.gameObject;

        //Verify if the object first hits something different of the target
        if (hitObject == null)
        {
            Debug.Log("hitObjected : is null");
            rootParent.gameObject.GetComponent<SlotActions>().CleanAbility();
        }

        if (hitObject != null)
        {
            Debug.Log("hitObjected : " + hitObject.name);
            Debug.Log("Parent name : " + hitObject.gameObject.name);
            if (hitObject.gameObject.GetComponent<SlotActions>())
            {
                Debug.Log("He has component");
                if (hitObject.gameObject.GetComponent<SlotActions>().SkillData == null)
                {
                    Debug.Log("And its null");
                    hitObject.gameObject.GetComponent<SlotActions>().SkillData = rootParent.gameObject.GetComponent<SlotActions>().SkillData;
                    hitObject.gameObject.GetComponent<SlotActions>().OnAbilitySwap();
                    rootParent.gameObject.GetComponent<SlotActions>().CleanAbility();
                    gameObject.transform.SetParent(rootParent);
                    if (transform.localPosition != startPosition)
                        transform.localPosition = startPosition;
                }
            }
            else if (hitObject.gameObject.name == "Icon")
            {
                if (hitObject.transform.parent.gameObject.GetComponent<SlotActions>())
                {
                    if (hitObject.transform.parent.gameObject.GetComponent<SlotActions>().SkillData != null)
                    {
                        Debug.Log("Hitobject" + hitObject.transform.parent.gameObject.name + " has a skill on it");
                        Ability previous_skillData = rootParent.gameObject.GetComponent<SlotActions>().SkillData;
                        Ability new_skillData = hitObject.transform.parent.gameObject.GetComponent<SlotActions>().SkillData;

                        hitObject.transform.parent.gameObject.GetComponent<SlotActions>().SkillData = previous_skillData;
                        rootParent.gameObject.GetComponent<SlotActions>().SkillData = new_skillData;

                        gameObject.transform.SetParent(rootParent);
                        if (transform.localPosition != startPosition)
                            transform.localPosition = startPosition;

                        hitObject.transform.parent.gameObject.GetComponent<SlotActions>().OnAbilitySwap();
                        rootParent.gameObject.GetComponent<SlotActions>().OnAbilitySwap();
                    }
                }
            }
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GameObject.FindWithTag("Player").gameObject.GetComponent<StateManager>().isDraggingUI = false;

    }
}
