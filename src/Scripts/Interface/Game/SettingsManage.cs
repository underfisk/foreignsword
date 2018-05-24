using System.Collections;
using System.Collections.Generic;
using HelperPackage;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManage : MonoBehaviour
{
    #region Data

    [Header("Hardware info-group")]
    [SerializeField] Text cpu;
    [SerializeField] Text gpu;
    [SerializeField] Text ram;
    [SerializeField] Text os;
    [SerializeField] Text hwid;

    [Header("Group Root GameObjects")]
    [SerializeField] GameObject healthGroup;
    [SerializeField] GameObject charRenderGroup;
    [SerializeField] GameObject timeGroup;
    [SerializeField] GameObject fpsGroup;
    [SerializeField] GameObject tabsMenu;
    [SerializeField] GameObject tabsContent;

    [Header("Buttons Binded")]
    [SerializeField] Button CloseButton;

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static SettingsManage Instance;
    

    #endregion

    #region UnityEvents

    protected void Start()
    {
        CloseButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    protected void Awake()
    {
        if (Instance == null) Instance = this;
    }

    #endregion


    /// <summary>
    /// Loads the default tab/content information
    /// </summary>
    public void LoadDefaultTab()
    {
        //Set every tab to disabled by default
        foreach (Transform child in tabsMenu.transform)
        {
            if (child.gameObject.GetComponent<SettingsTabBtn>())
            {
                if (child.gameObject.GetComponent<SettingsTabBtn>().isActive)
                {
                    child.gameObject.transform.Find("Text").GetComponent<Text>().color = Color.grey;
                    child.gameObject.GetComponent<SettingsTabBtn>().isActive = false;
                }
            }
        }

        //Set default tab active
        GameObject defaultTab = tabsMenu.transform.Find("Tab (1)").gameObject;
        defaultTab.transform.Find("Text").GetComponent<Text>().color = Color.white;
        defaultTab.GetComponent<SettingsTabBtn>().isActive = true;

        //Set everything inside content invisible by default
        foreach(Transform child in tabsContent.transform)
        {
            if (child.gameObject.activeSelf)
                child.gameObject.SetActive(false);
        }

        //Set default tab content active
        GameObject defaultTabContent = tabsContent.transform.Find("Ingame Tab").gameObject;
        defaultTabContent.SetActive(true);

    }

    /// <summary>
    /// Initializes the PlayerPrefs on the GUI/Classes with dependecy
    /// </summary>
    public void InitializePrefs()
    {
        ILog.toUnity("Inializating player prefs..",LType.Processing);
        int defaultHP = LocalPrefs.Health;
        int defaultChar = LocalPrefs.CharacterRender;
        int defaultTime = LocalPrefs.Time;
        int defaultFPS = LocalPrefs.FPS;

        //Means if it's different than 0 we have the PlayerPrefs already defined
        if (defaultHP != 0)
        {
            switch (defaultHP)
            {
                case 1:
                    InitHeathObject(SettingsOptionsBtn.ValueTypes.HP_ONLY);
                    break;
                case 2:
                    InitHeathObject(SettingsOptionsBtn.ValueTypes.HP_PERC);
                    break;
                case 3:
                    InitHeathObject(SettingsOptionsBtn.ValueTypes.PERC_ONLY);
                    break;
                default:
                    Debug.Log("Setting the default option which is Option1");
                    //Force init on playerPrefs
                    LocalPrefs.Health = 1;
                    healthGroup.transform.Find("Option1").gameObject.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active,true);
                    break;
            }
        }
        else
        {
            //The playerPrefs were not initialized before so lets set default data on GUI
            LocalPrefs.Health = 1;
            GameObject child = healthGroup.transform.Find("Option1").gameObject;
            child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active,true);
        }

        //Means if it's different than 0 we have CharacterRender already defined
        if (defaultChar != 0)
        {
            switch(defaultChar)
            {
                case 4:
                    InitCharacterObject(SettingsOptionsBtn.ValueTypes.SHOW);
                    break;
                case 5:
                    InitCharacterObject(SettingsOptionsBtn.ValueTypes.HIDE);
                    break;
                default:
                    Debug.Log("Setting up the default option which is Radio (1)");
                    //force init
                    LocalPrefs.CharacterRender = 4;
                    charRenderGroup.transform.Find("Radio (1)").gameObject.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
                    break;
            }
        }
        else
        {
            //The playerPrefs were not initialized before so lets set default data on GUI
            LocalPrefs.CharacterRender = 4;
            GameObject child = charRenderGroup.transform.Find("Radio (1)").gameObject;
            child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
        }

        if (defaultTime != 0)
        {
            switch(defaultTime)
            {
                case 1:
                    InitTimeObject(SettingsOptionsBtn.ValueTypes.YES);
                    break;
                case 2:
                    InitTimeObject(SettingsOptionsBtn.ValueTypes.NO);
                    break;
                default:
                    //The playerPrefs were not initialized before so lets set default data on GUI
                    LocalPrefs.Time = 1;
                    GameObject child = timeGroup.transform.Find("Option_t1)").gameObject;
                    child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
                    break;
            }
        }
        else
        {
            //The playerPrefs were not initialized before so lets set default data on GUI
            LocalPrefs.Time = 1;
            GameObject child = timeGroup.transform.Find("Option_t1").gameObject;
            child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
        }

        if (defaultFPS != 0)
        {
            switch (defaultFPS)
            {
                case 1:
                    InitFPSObject(SettingsOptionsBtn.ValueTypes.YES);
                    break;
                case 2:
                    InitFPSObject(SettingsOptionsBtn.ValueTypes.NO);
                    break;
                default:
                    //The playerPrefs were not initialized before so lets set default data on GUI
                    LocalPrefs.FPS = 1;
                    GameObject child = fpsGroup.transform.Find("Option_f1)").gameObject;
                    child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
                    break;
            }
        }
        else
        {
            //The playerPrefs were not initialized before so lets set default data on GUI
            LocalPrefs.FPS = 1;
            GameObject child = fpsGroup.transform.Find("Option_f1").gameObject;
            child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
        }
    }

    /// <summary>
    /// Receives SettingsOptionsBtn value type and filter in gameObjects Components
    /// </summary>
    /// <param name="val"></param>
    private void InitCharacterObject(SettingsOptionsBtn.ValueTypes val)
    {
        //Loops trough the child objects of root parent and set the one we're looking for
        foreach (Transform child in charRenderGroup.transform)
        {
            if (child.GetComponent<SettingsOptionsBtn>() && child.GetComponent<SettingsOptionsBtn>().Value == val)
                child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
        }
    }

    /// <summary>
    /// Receives the value to compare when we are finding the child
    /// </summary>
    /// <param name="val"></param>
    private void InitHeathObject(SettingsOptionsBtn.ValueTypes val)
    {
        //Loops trough the child objects of root parent and set the one we're looking for
        foreach (Transform child in healthGroup.transform)
        {
            if (child.GetComponent<SettingsOptionsBtn>() && child.GetComponent<SettingsOptionsBtn>().Value == val)
                child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
        }
    }

    private void InitTimeObject(SettingsOptionsBtn.ValueTypes val)
    {
        //Loops trough the child objects of root parent and set the one we're looking for
        foreach (Transform child in timeGroup.transform)
        {
            if (child.GetComponent<SettingsOptionsBtn>() && child.GetComponent<SettingsOptionsBtn>().Value == val)
                child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
        }
    }

    private void InitFPSObject(SettingsOptionsBtn.ValueTypes val)
    {
        //Loops trough the child objects of root parent and set the one we're looking for
        foreach (Transform child in fpsGroup.transform)
        {
            if (child.GetComponent<SettingsOptionsBtn>() && child.GetComponent<SettingsOptionsBtn>().Value == val)
                child.GetComponent<SettingsOptionsBtn>().ChangeState(SettingsOptionsBtn.ObjectState.Active, true);
        }
    }

    /// <summary>
    /// Set GUI Hardware information
    /// </summary>
    public void SetHardwareInfo()
    {
        Settings h = GameData.GameSettings;
        if (h == null) h = new Settings();
        cpu.text = h.CPU;
        gpu.text = h.GPU;
        ram.text = h.MemoryRAM.ToString() + " MB(s)";
        os.text = h.OperativeSystem;
        hwid.text = h.HardwareID;

        ILog.toUnity($"Settings were succesfully initialized from settingsmanage", LType.Success);

    }

  
}
