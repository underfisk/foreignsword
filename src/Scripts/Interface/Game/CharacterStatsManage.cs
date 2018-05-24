using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsManage : MonoBehaviour
{
    /// <summary>
    /// Character stats text
    /// </summary>
    [Header("Stats")]
    [SerializeField] Text hp_val;
    [SerializeField] Text energy_val;
    [SerializeField] Text str_val;
    [SerializeField] Text agi_val;
    [SerializeField] Text int_val;
    [SerializeField] Text spr_val;

    /// <summary>
    /// Core info
    /// </summary>
    [Header("Core info")]
    [SerializeField] Text _name;
    [SerializeField] Text active_title;

    /// <summary>
    /// Inventory open button
    /// </summary>
    [Header("Buttons")]
    [SerializeField] Button inv;
    private void Start()
    {
        LoadCharacterInformation(); //by default
    }

    public void LoadCharacterInformation()
    {
        HelperPackage.ILog.toUnity("Loading the character information..", HelperPackage.LType.Processing);
        //Init button inv open
        inv.onClick.AddListener(() => InventoryOpen());

        //Init core info first
        _name.text = !String.IsNullOrEmpty(GameData.CharacterData.Name.ToString()) ? GameData.CharacterData.Name.ToString() : "Name";
        active_title.text = "Todo..";


        //Access character stats and display it
    }

    protected void InventoryOpen()
    {
        //call open inv func cuz it needs to init the items

        //after close this window
    }
}
