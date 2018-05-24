using System;
using UnityEngine;

/*
Doesn´t need monobehaviour its just storage object clsas
*/
[CreateAssetMenu(menuName = ("Items/Weapons"))]
public class Weapon : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public Transform GripTransform;
    [SerializeField] public double Price;
    [SerializeField] public Texture Image;
    [SerializeField, Range(1,300)] public int MinDamage;
    [SerializeField, Range(1,700)] public int MaxDamage;
}