using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class CustomItemAttributes : Editor
{
    SerializedObject item;

    SerializedProperty id, _name, icon, desc, quality, type, price, sellable, stackable, stacks, 
        equiptype, hand, minDmg, maxDmg, atkSpeed, durability, minDef, maxDef, blockRate, hp, mp, pot_rec_hp, pot_rec_mp,
        weaponPrefab, weaponGrip, equipmentPrefab, equipmentGrip;

    public void OnEnable()
    {
        //Target => Dependecy script
        item = new SerializedObject(target);
        id = item.FindProperty("ID");
        _name = item.FindProperty("Name");
        icon = item.FindProperty("Icon");
        desc = item.FindProperty("Description");
        quality = item.FindProperty("Quality");
        type = item.FindProperty("Type");
        price = item.FindProperty("Price");
        sellable = item.FindProperty("isSellable");
        stackable = item.FindProperty("isStackable");
        stacks = item.FindProperty("MaxStack");
        equiptype = item.FindProperty("EquipType");
        hand = item.FindProperty("Hand");
        minDmg = item.FindProperty("MinDamage");
        maxDmg = item.FindProperty("MaxDamage");
        atkSpeed = item.FindProperty("AttackSpeed");
        durability = item.FindProperty("Durability");
        minDef = item.FindProperty("MinDefense");
        maxDef = item.FindProperty("MaxDefense");
        blockRate = item.FindProperty("BlockRate");
        hp = item.FindProperty("Health");
        mp = item.FindProperty("Mana");
        pot_rec_hp = item.FindProperty("PotionRecoverHealth");
        pot_rec_mp = item.FindProperty("PotionRecoverMana");
        weaponPrefab = item.FindProperty("WeaponPrefab");
        weaponGrip = item.FindProperty("WeaponGrip");
        equipmentPrefab = item.FindProperty("EquipmentPrefab");
        equipmentGrip = item.FindProperty("EquipmentGrip");
    }

    public override void OnInspectorGUI()
    {
        item.Update();

        //Render basic fields
        EditorGUILayout.PropertyField(item.FindProperty("m_Script"));
        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(_name);
        EditorGUILayout.PropertyField(desc);
        EditorGUILayout.PropertyField(icon);
        EditorGUILayout.PropertyField(quality);
        EditorGUILayout.PropertyField(sellable);
        EditorGUILayout.PropertyField(stackable);
        EditorGUILayout.PropertyField(type);


        if (GUI.changed)
        {
            Debug.Log("Values changed");
        }

        //the item can stack?
        if (stackable.boolValue)
            EditorGUILayout.PropertyField(stacks);
        else
            stacks.intValue = 0;

        //the item can be sold?
        if (sellable.boolValue)
            EditorGUILayout.PropertyField(price);
        else
            price.doubleValue = 0;

        //is it a health potion?
        if (type.enumValueIndex == 3)
            EditorGUILayout.PropertyField(pot_rec_hp);
        else
            pot_rec_hp.floatValue = 0;

        if (type.enumValueIndex == 4)
            EditorGUILayout.PropertyField(pot_rec_mp);
        else
            pot_rec_mp.floatValue = 0;


        if (type.enumValueIndex == 1)
        {
            EditorGUILayout.HelpBox("Please insert correct data otherwise the game will throw alot of errors due to important calculations data being wrong",MessageType.Info);
            EditorGUILayout.PropertyField(hand);
            EditorGUILayout.PropertyField(weaponPrefab);
            EditorGUILayout.PropertyField(weaponGrip);
            EditorGUILayout.PropertyField(minDmg);
            EditorGUILayout.PropertyField(maxDmg);
            EditorGUILayout.PropertyField(atkSpeed);
            EditorGUILayout.PropertyField(durability);
            EditorGUILayout.Separator();
        }
        else
        {
            hand.enumValueIndex = 0;
            weaponPrefab = null;
            weaponGrip = null;
            minDmg.floatValue = 0;
            maxDmg.floatValue = 0;
            atkSpeed.floatValue = 0;
            durability.intValue = 0;
        }

        if (type.enumValueIndex == 2)
        {
            EditorGUILayout.HelpBox("Please insert correct data otherwise the game will throw alot of errors due to important calculations data being wrong", MessageType.Info);
            EditorGUILayout.PropertyField(equiptype);
            EditorGUILayout.PropertyField(equipmentPrefab);
            EditorGUILayout.PropertyField(equipmentGrip);
            EditorGUILayout.PropertyField(minDef);
            EditorGUILayout.PropertyField(maxDef);
            EditorGUILayout.PropertyField(blockRate);
            EditorGUILayout.PropertyField(hp);
            EditorGUILayout.PropertyField(mp);
            EditorGUILayout.PropertyField(durability);
            EditorGUILayout.Separator();
        }
        else
        {
            equiptype.enumValueIndex = 0;
            equipmentPrefab = null;
            equipmentGrip = null;
            minDef.floatValue = 0;
            maxDef.floatValue = 0;
            blockRate.floatValue = 0;
            hp.floatValue = 0;
            mp.floatValue = 0;
            durability.intValue = 0;
        }

        //Save changes on object
        item.ApplyModifiedProperties();
    }
}
