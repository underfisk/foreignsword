using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour, IDamageable
{
    #region Game Instance CharacterData
    /// <summary>
    /// Character id reference used for update httprequest and identification
    /// </summary>
    private int c_id;

    /// <summary>
    /// Character name to be displayed
    /// </summary>
    private string c_name;

    /// <summary>
    /// Character class id
    /// </summary>
    private int class_id;

    /// <summary>
    /// Character currency
    /// </summary>
    private double gold, silver, bronze;

    /// <summary>
    /// Character crafting materials
    /// </summary>
    private List<Material> materials;

    /// <summary>
    /// This var is set when the player logs in the world onApplicationQuit or update method 
    /// the calculation is startedPlayingAt - Datetime.now => hours he played
    /// </summary>
    private Time startedPlayingAt;

    /// <summary>
    /// Invetory items in the slots stores the list of item in GUI inventory
    /// </summary>
    private List<Item> inventory;

    /// <summary>
    /// Inventory  items in equipment slots
    /// </summary>
    private List<Item> equipedItems;

    /// <summary>
    /// Character level
    /// </summary>
    private int lvl;

    /// <summary>
    /// Player experience 
    /// </summary>
    private float exp;

    /// <summary>
    /// Player health, max health and mana
    /// </summary>
    private float hp, max_hp, mana;

    //Quests
    //Can be done this way or just saving quests_id but its better full quest object
    private List<Quest> active_quests; //Quest will have all the info
    private List<Quest> complete_quests; //all the complete quests goes here
    private List<Quest> failed_quests; //Witcher quest system, you have a date on the quest to be done

    /// <summary>
    /// We have a object so we can manage this current char stats from there by class id
    /// </summary>
    private Stats c_stats;

    /// <summary>
    /// Stats left to be used like dekaron
    /// </summary>
    private int stats_left;
    #endregion

    private GUI_Manager UI;

    public Vector3 CharacterPosition { get { return transform.position; } }

    public List<Material> Materials { get { return this.materials; } }

    public int CharacterID { get { return this.c_id; } }

    public float Health { get { return this.hp > 0 ? this.hp : 0; } set { this.hp = value; } }
    public float MaxHealth { get { return this.max_hp > 0 ? this.max_hp : 0; } }

    public delegate void OnLevelUp(int exp);
    public event OnLevelUp LevelUpObservers;

    public Weapon mainWeaponItem;
    public Transform mainWeaponSocket;

    public Weapon secondaryWeaponItem; //shields, etc
    public Transform secondaryWeaponSocket; 

    public void TakeDamage(float dmg)
    {
        hp = Mathf.Clamp(hp - dmg, 0, max_hp);
    }


    protected void Start()
    {
        //Register a new handler
        EquipWeapon();
        max_hp = 200;
        hp= 180;
        Level = 1;
        Experience = 1f;
        GameData.InitializeLevelEXP();
        UI = GameObject.Find("UI").gameObject.GetComponent<GUI_Manager>();
        //UI.SetPlayerName( String.IsNullOrWhiteSpace(GameData.CharacterData.Name) ? GameData.CharacterData.Name : "Undefined");
        UI.SetPlayerLevel(Level);
        UI.SetPlayerEXP(Experience);
        HelperPackage.ILog.toUnity("Player data was been iniatilized!",HelperPackage.LType.Success);
        UI.UpdatePlayerHealth();
        UI.UpdatePlayerEXP();
    }

    protected void EquipWeapon()
    {
        if (mainWeaponItem.Prefab != null )
        {
            var wepPrefab = mainWeaponItem.Prefab;
            var wep = Instantiate(wepPrefab, mainWeaponSocket);
            wep.transform.localPosition = mainWeaponItem.GripTransform.transform.localPosition;
            wep.transform.localRotation = mainWeaponItem.GripTransform.transform.localRotation;
        }
    }

    protected void Update()
    {
        //GUI_Manager.instance.UpdatePlayerHealth();
    }

    public float Experience
    {
        get { return this.exp; }
        set { this.exp = value; }
    }

    public int Level
    {
        get { return this.lvl; }
        set { this.lvl = value; }
    }
}