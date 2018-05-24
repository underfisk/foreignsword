using System;
using System.Collections;
using System.Collections.Generic;
using HelperPackage;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Manager : MonoBehaviour
{
    //TODO : Make the chat functions to add message and also a button to clean the console :)

    #region GUIData

    /// <summary>
    /// Singleton static instance, not going to open more so it's a safe thread
    /// </summary>
    public static GUI_Manager instance;

    public delegate void OnPlayerHealthChange();
    public event OnPlayerHealthChange HealthChangeObservers;



    [Header("Player Core Data")]
    [SerializeField] public GameObject PlayerNameText;
    [SerializeField] public Text player_name;
    [SerializeField] public Text enemy_name;

    [Header("Windows")]
    [SerializeField] public GameObject inventoryWindow;
    [SerializeField] public GameObject menuWindow;
    [SerializeField] public GameObject settingsWindow;
    [SerializeField] public GameObject characterWindow;
    [SerializeField] public GameObject spellBookWindow;

    [Header("Minimap")]
    [SerializeField] public Text MapName;

    [Header("Frames")]
    [SerializeField] GameObject player_frame;
    [SerializeField] GameObject enemy_frame;

    [Header("Player Bars")]
    public RectTransform HP_Bar;
    public Text HP_Text;
    public RectTransform MP_Bar;
    public Text MP_Text;

    [Header("Enemy_Bar")]
    public RectTransform Enemy_HP_Bar;
    public Text Enemy_HP_Text;
    public Text player_level;

    [Header("NotificationFrame")]
    [SerializeField] GameObject notificationObj;
    [SerializeField] Text notificationTitle;
    [SerializeField] Text notificationContext;

    [Header("LevelUpNotification")]
    [SerializeField] GameObject lvlUpNotificationObj;
    [SerializeField] Text level_val;

    [Header("Experience_Bar")]
    [SerializeField] Text brute_expText;
    [SerializeField] Text perc_expText;
    [SerializeField] RectTransform ExpBar;

    [Header("TimeFrame")]
    [SerializeField] public Text _TimeHours;
    [SerializeField] public GameObject TimeFrame;

    [Header("WarningFrame")]
    [SerializeField] public GameObject FrameBox;
    [SerializeField] public Text WarningText;

    #endregion

    #region Frames

    public void Start()
    {
        if (instance == null) instance = this;
        if (inventoryWindow == null) inventoryWindow = GameObject.Find("Inventory").gameObject;
        if (menuWindow == null) menuWindow = GameObject.Find("MenuWindow").gameObject;
        if (settingsWindow == null) settingsWindow = GameObject.Find("Settings").gameObject;
        if (characterWindow == null) characterWindow = GameObject.Find("CharacterWindow").gameObject;
        if (spellBookWindow == null) spellBookWindow = GameObject.Find("SpellBook").gameObject;

        //TODO : Check if gameobject does not exist to search for it
        HealthChangeObservers += UpdatePlayerHealth;


        //Init map name
        MapName.text = !String.IsNullOrEmpty(GameData.MapName) ? GameData.MapName : "World Map";

        //Start Time clock
        if (LocalPrefs.Time == 1)
        {
            TimeFrame.gameObject.SetActive(true);
            //init the first time so it dont be blank
            StartCoroutine(TimeClock());
            _TimeHours.text = DateTime.Now.ToString("HH:mm") + "h";
        }
    }

    public void Update()
    {
        if (LocalPrefs.Time == 1)
        {
            TimeFrame.gameObject.SetActive(true);
            //init the first time so it dont be blank
            StartCoroutine(TimeClock());
            _TimeHours.text = DateTime.Now.ToString("HH:mm") + "h";
        }
        else
        {
            TimeFrame.gameObject.SetActive(false);
            StopCoroutine(TimeClock());
        }
    }


    /// <summary>
    /// Displays a message in the warning box designed for the ability system
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator DisplayWarningBox(string msg, float duration)
    {
        WarningText.text = msg;
        if (!FrameBox.activeSelf)
            FrameBox.SetActive(true);

        yield return new WaitForSeconds(duration);

        WarningText.text = "Undefined";
        FrameBox.SetActive(false);
    }

    /// <summary>
    /// Updates the time every 60 real time seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeClock()
    {
        _TimeHours.text = DateTime.Now.ToString("HH:mm");
        yield return new WaitForSecondsRealtime(60);
    }

    public IEnumerator DisplayNotification(string t,string b)
    {
        notificationTitle.text = t;
        notificationContext.text = b;
        notificationObj.SetActive(true);
        yield return new WaitForSeconds(5);
        //after when we return we will fade
        //for now just hide
        notificationObj.SetActive(false);
    }

    /// <summary>
    /// Shows a levelup message
    /// </summary>
    /// <param name="lvl"></param>
    /// <returns></returns>
    public IEnumerator DisplayLevelUp(int lvl)
    {
        level_val.text = $"Level {lvl.ToString()}";
        if (!lvlUpNotificationObj.activeSelf)
            lvlUpNotificationObj.SetActive(true);

        yield return new WaitForSeconds(2);

        if (lvlUpNotificationObj.activeSelf)
            lvlUpNotificationObj.SetActive(false);
    }

    /// <summary>
    /// will be used for hotkey porpuse like turn on hud or turnoff
    /// </summary>
    public void HideShowPlayerFrame()
    {
        if (player_frame.gameObject.activeSelf)
            player_frame.gameObject.SetActive(false);
        else
            player_frame.gameObject.SetActive(true);
    }

    /// <summary>
    /// Shows the enemy frame and init the values
    /// </summary>
    /// <param name="enemy_name"></param>
    public void ShowEnemyFrame(string enemy_name)
    {
        var hp = GameObject.Find(enemy_name).GetComponent<AI_Combat>().data.Health;
        var max_hp = GameObject.Find(enemy_name).GetComponent<AI_Combat>().data.MaxHealth;

        if (LocalPrefs.Health == 1)
            Enemy_HP_Text.text = $"{hp} / {max_hp}"; //brute hp
        else if (LocalPrefs.Health == 2)
            Enemy_HP_Text.text = $"{hp} / {max_hp} ({GetHealthPercentage(hp, max_hp)})"; //brute + perc
        else if (LocalPrefs.Health == 3)
            Enemy_HP_Text.text = $"{GetHealthPercentage(hp, max_hp)}"; //perc only
        else
            Enemy_HP_Text.text = $"{hp} / {max_hp}"; //default => brute hp

        Enemy_HP_Bar.GetComponent<Image>().fillAmount = (hp / max_hp * 100f) / 100f;
        //Enemy_HP_Bar
        enemy_frame.SetActive(true);
    }

    /// <summary>
    /// Hides the frame and sets the text to default
    /// </summary>
    /// <param name="reset"></param>
    public void HideEnemyFrame(bool reset = false)
    {
        enemy_frame.SetActive(false);

        if (reset)
        {
            Enemy_HP_Bar.GetComponent<Image>().fillAmount = 0f;
            Enemy_HP_Text.text = "0 %";
        }
    }

    /// <summary>
    /// Updates the health according to the player prefs
    /// max_bar_size* health_amount / 100% on the percentage
    /// </summary>
    public void UpdatePlayerHealth()
    {
        var hp = GameObject.FindWithTag("Player").GetComponent<Player>().Health;
        var max_hp = GameObject.FindWithTag("Player").GetComponent<Player>().MaxHealth;

        if (LocalPrefs.Health == 1)
            HP_Text.text = $"{hp} / {max_hp}"; //brute hp
        else if (LocalPrefs.Health == 2)
            HP_Text.text = $"{hp} / {max_hp} ({GetHealthPercentage(hp, max_hp)})"; //brute + perc
        else if (LocalPrefs.Health == 3)
            HP_Text.text = $"{GetHealthPercentage(hp, max_hp)}"; //perc only
        else
            HP_Text.text = $"{hp} / {max_hp}"; //default => brute hp

        HP_Bar.GetComponent<Image>().fillAmount = (hp / max_hp * 100f) / 100f;
    }

    /// <summary>
    /// Receives the enemy obj name and update his component
    /// </summary>
    /// <param name="enemy_name"></param>
    public void UpdateEnemyHealth(string enemy_name)
    {
        var hp = GameObject.Find(enemy_name).GetComponent<AI_Combat>().data.Health;
        var max_hp = GameObject.Find(enemy_name).GetComponent<AI_Combat>().data.MaxHealth;

        Enemy_HP_Text.text = GetHealthPercentage(hp,max_hp);
        if (LocalPrefs.Health == 1)
            Enemy_HP_Text.text = $"{hp} / {max_hp}";
        else if (LocalPrefs.Health == 2)
            Enemy_HP_Text.text = $"{hp} / {max_hp} ({GetHealthPercentage(hp, max_hp)})";
        else if (LocalPrefs.Health == 3)
            Enemy_HP_Text.text = $"{GetHealthPercentage(hp, max_hp)}"; //perc only
        else
            Enemy_HP_Text.text = $"{hp} / {max_hp}";
    }

    public void UpdatePlayerEXP()
    {
        float exp = GameObject.FindWithTag("Player").GetComponent<Player>().Experience;
        int playerLevel = GameObject.FindWithTag("Player").GetComponent<Player>().Level;
        float nextLevelExp = GameData.LevelsExperience[playerLevel + 1];
        ExpBar.gameObject.GetComponent<Image>().fillAmount = exp / nextLevelExp;

        brute_expText.text = $"{exp} / {nextLevelExp} XP"; //needed exp soon
        perc_expText.text = $"{ (exp / nextLevelExp) * 100f} %";
    }

    /// <summary>
    /// Formula = Experience / Needed Experience * 100% = Experience %
    /// </summary>
    /// <param name="exp"></param>
    public void SetPlayerEXP(float exp)
    {
        int pLevel = GameObject.FindWithTag("Player").gameObject.GetComponent<Player>().Level;
        float expPercentage =  (exp/GameData.LevelsExperience[pLevel + 1]) * 100f;

        brute_expText.text = $"{exp} / {GameData.LevelsExperience[pLevel+1]} XP"; //needed exp soon
        perc_expText.text = $"{expPercentage} %";
        Debug.Log("The percentage exp is => " + expPercentage);

    }

    #endregion

    #region Calculations
    /// <summary>
    /// The formula is Health / Max_Health * 100 (%) 
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="max_hp"></param>
    /// <returns></returns>
    public string GetHealthPercentage(float hp, float max_hp)
    {
        return $"{ (hp / max_hp) * 100f} %";
    }

    #endregion

    #region PublicSetters
    public void SetPlayerName(string name)
    {
        player_name.text = name;
    }

    public void SetPlayerLevel(int lvl)
    {
        player_level.text = lvl.ToString();
    }

    public void SetPlayerAvatar(string avatarName)
    {

    }

    public void SetEnemyName(string name)
    {
        if (!String.IsNullOrEmpty(name))
            enemy_name.text = name;
        else
            enemy_name.text = "CharName";

    }

    public void SetEnemyLevel(int lvl)
    {

    }

    public void SetEnemyAvatar(string avatarName)
    {

    }
    #endregion
}
