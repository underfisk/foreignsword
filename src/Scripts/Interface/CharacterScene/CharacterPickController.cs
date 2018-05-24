using HelperPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterPickController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// This data type is set on the intantiate of this object
    /// </summary>
    [SerializeField] public Character myData;
    [SerializeField] public int order_index = -1;
    [SerializeField] public int internalID = -1;
    [SerializeField] public bool Active = false;

    private Button deleteBtn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        var x = gameObject.transform.Find("Hover Overlay").gameObject;
        if (!x.activeSelf)
            x.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var x = gameObject.transform.Find("Hover Overlay").gameObject;
        if (x.activeSelf)
            x.SetActive(false);
    }

    protected void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => DefineActiveCharacter());
        deleteBtn = gameObject.transform.Find("DeleteBtn").gameObject.GetComponent<Button>();
        deleteBtn.onClick.AddListener(() => ShowDeletePopup());
    }

    protected void ShowDeletePopup()
    {
        //bind data
        GameObject.Find("GUI").gameObject.GetComponent<CharacterSceneController>().DeletePopup.GetComponent<DeleteCharacterController>().c_delete_id = myData.ID;
        //show popup
        GameObject.Find("GUI").gameObject.GetComponent<CharacterSceneController>().DeletePopup.SetActive(true);
    }

    /// <summary>
    /// Button delegated function to handle the click on the characters card
    /// </summary>
    protected void DefineActiveCharacter()
    {
        if (GameObject.Find("GUI").GetComponent<CharacterSceneController>().SelectedCharacterIndex != internalID)
        {
            CharacterSceneController.Instance.UpdateCharacter(myData.ID,myData.ClassID);

            //Loops trought the character grid and checks who is active and disable them
            for (int i = 0; i < CharacterSceneController.Instance.CharactersGrid.transform.childCount; i++)
            {
                //Is this child active?
                if (CharacterSceneController.Instance.CharactersGrid.transform.GetChild(i).GetComponent<CharacterPickController>().Active)
                {

                    //Set active false
                    CharacterSceneController.Instance.CharactersGrid.transform.GetChild(i).GetComponent<CharacterPickController>().Active = false;
                    //Set back the border to the normal one to remove the active effect
                    CharacterSceneController.Instance.CharactersGrid.transform.GetChild(i).transform.Find("Active Overlay").gameObject.SetActive(false);
                }
            }

            ILog.toUnity($"The current object : {gameObject.name} and the active is : {Active}");
            if (!Active)
            {
                //Finds the GUI and set selected index as this one
                GameObject.Find("GUI").GetComponent<CharacterSceneController>().SelectedCharacterIndex = internalID;
                //Sets active border
                gameObject.transform.Find("Active Overlay").gameObject.SetActive(true);
                Active = true;
            }
        }
    }



}
