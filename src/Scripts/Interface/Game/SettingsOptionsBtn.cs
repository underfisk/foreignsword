using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingsOptionsBtn : MonoBehaviour
{
    /// <summary>
    /// Group member type will know which value need to set according to the value group this member is in
    /// </summary>
    public enum GroupType
    {
        HEALTH,
        CHAR_RENDER,
        TIME,
        SOUND,
        FPS
    }

    /// <summary>
    /// We'll soon handle this but it's good for control
    /// </summary>
    public enum ObjectType
    {
        Option,
        Button,
        Trigger,
        Image
    }

    /// <summary>
    /// Predefined types for value in playerprefs
    /// </summary>
    public enum ValueTypes
    {
        SHOW,
        HIDE,
        TRUE,
        FALSE,
        YES,
        NO,
        HP_ONLY,
        HP_PERC,
        PERC_ONLY
    }

    /// <summary>
    /// Initial object state
    /// </summary>
    public enum ObjectState
    {
        Hidden,
        Active,
        Visible
    }

    [Header("Properties")]
    [SerializeField] public GameObject Graphic;
    [SerializeField] public ObjectState State;
    [SerializeField] public ObjectType Type;
    [SerializeField] public GameObject Group;
    [SerializeField] public ValueTypes Value;
    [SerializeField] public GroupType GroupMemberType;
    
    /// <summary>
    /// Default unity event and we set some data there
    /// </summary>
    protected void Start()
    {
        switch(State)
        {
            case ObjectState.Active:
                gameObject.SetActive(true);
                Graphic.SetActive(true); //active mark
                break;
            case ObjectState.Visible:
                gameObject.SetActive(true);
                break;
            case ObjectState.Hidden:
                gameObject.SetActive(false);
                break;
        }

        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(() => OnClick());
        //btn on mouse enter later
        //btn on leave later 
    }

    /// <summary>
    /// Event binded to onClick listener to change state and value
    /// </summary>
    protected void OnClick()
    {
        foreach(Transform child in Group.transform)
        {
            if (child.gameObject.name != gameObject.name && child.gameObject.GetComponent<SettingsOptionsBtn>())
                child.gameObject.GetComponent<SettingsOptionsBtn>().ChangeState(ObjectState.Visible);
        }

        switch(GroupMemberType)
        {
            case GroupType.HEALTH:
                if (Value == ValueTypes.HP_ONLY)
                    HelperPackage.LocalPrefs.Health = 1;
                else if (Value == ValueTypes.HP_PERC)
                    HelperPackage.LocalPrefs.Health = 2;
                else if (Value == ValueTypes.PERC_ONLY)
                    HelperPackage.LocalPrefs.Health = 3;
                break;
            case GroupType.CHAR_RENDER:
                if (Value == ValueTypes.SHOW)
                    HelperPackage.LocalPrefs.CharacterRender = 4;
                else if (Value == ValueTypes.HIDE)
                    HelperPackage.LocalPrefs.CharacterRender = 5;
                break;
            case GroupType.TIME:
                if (Value == ValueTypes.YES)
                    HelperPackage.LocalPrefs.Time = 1;
                else if (Value == ValueTypes.NO)
                    HelperPackage.LocalPrefs.Time = 2;
                break;
            case GroupType.FPS:
                if (Value == ValueTypes.YES)
                    HelperPackage.LocalPrefs.FPS = 1;
                else if (Value == ValueTypes.NO)
                    HelperPackage.LocalPrefs.FPS = 2;
                break;
        }

        //set the state as active
        ChangeState(ObjectState.Active, true);
        Debug.Log($"Value was been set: {Value}");
    }
    
    /// <summary>
    /// Receives the new state and the condition if we want to set the obj in gui as active (the image)
    /// </summary>
    /// <param name="state"></param>
    /// <param name="changeGUI"></param>
    public void ChangeState(ObjectState _state, bool changeGUI = false)
    {
        //Loops and make sure there are no objects active when we change state
        foreach (Transform child in gameObject.transform.parent.transform)
        {
            if (child.GetComponent<SettingsOptionsBtn>() && child.GetComponent<SettingsOptionsBtn>().State == ObjectState.Active)
            {
                child.GetComponent<SettingsOptionsBtn>().State = ObjectState.Visible;
                child.GetComponent<SettingsOptionsBtn>().Graphic.SetActive(false);
            }
        }

        State = _state;
        if (changeGUI)
            Graphic.SetActive(true);
    }
}
