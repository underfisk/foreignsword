using System;
using UnityEngine;

[CreateAssetMenu(menuName = ("Items/Item")), System.Serializable]
public class Item : ScriptableObject
{
    /// <summary>
    /// Weapon type enumator (Main, Off or nothing)
    /// </summary>
    public enum WeaponHand
    {
        None = 0,
        MainHand = 1,
        OffHand = 2
    }

    /// <summary>
    /// Equipment Type enumator (helmet, chest, boots)
    /// </summary>
    public enum EquipmentType
    {
        None = 0,
        Helmet = 1,
        Necklace = 2,
        Shoulder = 3,
        Chest = 4,
        Shirt = 5,
        Bracers = 6,
        Gloves = 7,
        Belt = 8,
        Pants = 9,
        Boots = 10,
        Finger = 11,
        Trinket = 12
    }

    /// <summary>
    /// Item Type (Weapon, gear, potion)
    /// </summary>
    public enum ItemType
    {
        None,
        Weapon,
        Equipment,
        HP_Potion,
        MP_Potion
    }

    //Main fields 
    [SerializeField] public int ID;
    [SerializeField] public string Name;
    [SerializeField] public Sprite Icon;
    [SerializeField] public string Description;
    [SerializeField] public ItemQuality.Quality Quality;
    [SerializeField] public ItemType Type;

    [SerializeField] public double Price;

    [SerializeField, Tooltip("Determinates if this item can be sold to an NPC")] public bool isSellable;

    [SerializeField, Tooltip("Determinates if this item can have multiple stack or not")] public bool isStackable;

    [SerializeField, Tooltip("Determinates the stack size of an item")] public int MaxStack;

    //Conditional fields
    [SerializeField] public EquipmentType EquipType;

    [SerializeField] public WeaponHand Hand;

    [SerializeField, Range(1f, 9999f)] public float MinDamage;

    [SerializeField, Range(1f, 9999f)] public float MaxDamage;

    [Range(1f,100f), SerializeField, Tooltip("This attribute is used as the attack speed (as % percentage) for Skill Cooldown reduction")] public float AttackSpeed;

    [SerializeField,Tooltip("When the player dies loses 1 durability")] public int Durability;

    [SerializeField, Range(1f,9999f)] public float MinDefense;

    [SerializeField, Range(1f,9999f)] public float MaxDefense;

    [HideInInspector, Range(1f,100f), Tooltip("This attribute is used to calculate the block rate/ miss of the enemy")] public float BlockRate;

    [SerializeField, Range(1f, 9999f)] public float Health;

    [SerializeField, Range(1f, 9999f)] public float Mana;

    [SerializeField, Range(1f, 9999f)] public float PotionRecoverHealth;

    [SerializeField, Range(1f, 9999f)] public float PotionRecoverMana;

    /// <summary>
    /// Holds the weapon prefab if this item turns to be a weapon
    /// </summary>
    [SerializeField] public GameObject WeaponPrefab;

    /// <summary>
    /// Holds the weapon grip if this item is a weapon
    /// </summary>
    [SerializeField] public Transform WeaponGrip;

    /// <summary>
    /// Holds the equipment part prefab
    /// </summary>
    [SerializeField] public GameObject EquipmentPrefab;

    /// <summary>
    /// Holds the equipment part grip (respective one)
    /// </summary>
    [SerializeField] public Transform EquipmentGrip;
}
