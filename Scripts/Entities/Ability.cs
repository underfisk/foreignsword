using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Spells/Ability"))]
public class Ability : ScriptableObject
{
    public enum Type
    {
        Spell,
        Buff,
        Melee
    }

    [SerializeField, Tooltip("When clicked the given key triggers the button event")] public KeyCode KeyOnPress;
    [SerializeField, Tooltip("Skill id for isCasting animator param and for slot action handling")] public int ID;
    [SerializeField] public Type SkillType;
    [SerializeField] public string Name;
    [SerializeField] public string Description;
    [SerializeField] public int Level;
    [SerializeField] public Sprite Icon;
    [SerializeField] public AudioClip Sound;
    [SerializeField] public float Cooldown = 1f;
    [SerializeField] public int MinDamage, MaxDamage;
    [SerializeField] public bool NeedTarget = true;
}
