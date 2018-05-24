using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Enumeration type of the npc
/// </summary>
public enum NPCType
{
   QUEST_NPC = 1,
   SHOP_NPC = 2,
   CRAFT_NPC = 3,
   ENEMY_NPC = 4
}

[RequireComponent(typeof(AIStateMachine))]
public class NPCActions : MonoBehaviour
{
    [Header("Core Data")]
    [SerializeField] public NPCType npcType;
    [SerializeField] public AIStateMachine state;

    [Header("Dialogue Data")]
    [SerializeField] public List<string> dialogText;
    [SerializeField] public int startIndex = 0;

    [Header("Rewards Data")]
    [SerializeField] public double gold;
    [SerializeField] public double silver;
    [SerializeField] public double bronze;
    [SerializeField] public List<Item> items;

    private NPC npcData;

    protected void Start()
    {
        state.SetIdle(true);
    }


    public void ShowDialogue()
    {
        HelperPackage.ILog.toUnity("Talking to npc .. " + gameObject.name);
        //set gui data
        //shhow after the window
    }

    //target => the string to be loaded or the target object
    public void LoadDialog(int npc_id, ref string _target)
    {

    }

    public void ResetDialog(ref string _target)
    {
        //delete the string inside
    }

    public void FormatGold()
    {
        //GUI format from text to gold icon
        //GUIManager.playerGold = mainData.GetPlayerData().getGold(); not tested yet
    }

    public void FormatItems()
    {
        // same to the items
    }

    public void FormatDiamond()
    {
        //same to the diamonds
    }

    public void BeginQuest(int quest_id, int char_id)
    {
        //start the quest id on the char
    }

    public void FinishQuest(int quest_id, int char_id)
    {

    }

    public void CheckQuestStatus(int quest_id, int char_id)
    {
        //check the quest_id status on the char_id and see if is done,in progress or failed
    }

    /*
      Load weapon needed resources according to weapon_id
      Take the char_id to know who's crafting it
      take char resources to remove the used ones
    */
    public void CraftWeapon(int char_id, int weapon_id, List<Material> materials)
    {

    }

    public void LoadWeaponMaterial(int weapon_id)
    {

    }
}