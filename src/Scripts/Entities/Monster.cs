using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AI_Combat))]
public class Monster : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    private float max_hp, hp;
    private int exp;
    private float dmg;

    //temporary
    public Monster(string name, int _exp, float _hp , float _max_hp, float _dmg)
    {
        this.exp = _exp;
        this.hp = _hp;
        this.max_hp = _max_hp;
        this.dmg = _dmg;
    }

    public float Health
    {
        get { return this.hp; }
        set { this.hp = value; }
    }
    
    public float MaxHealth
    {
        get { return this.max_hp; }
        set { this.max_hp = value; }
    }

    public int Experience
    {
        get { return this.exp;}
        set { this.exp = value; }
    }

    public float Damage
    {
        get { return this.dmg; }
        set { this.dmg = value; }
    }


    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

}