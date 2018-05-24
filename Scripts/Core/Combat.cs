using System;
using UnityEngine;
using UnityEngine.UI;
using HelperPackage;

public class Combat : MonoBehaviour, IDamageable
{
    #region CombatData
    /// <summary>
    /// This var will be set when we find an enemy
    /// </summary>
    private GameObject opponent;

    public GameObject ActiveTarget
    {
        get { return this.opponent; }
        set { this.opponent = value; }
    }
    /// <summary>
    /// Test var for now but this var will be set automaticly from player class
    /// </summary>
    private float max_hp = 100;

    /// <summary>
    /// Player current health the same will happen as max_hp vaaar
    /// </summary>
    public float hp = 25;

    /// <summary>
    /// Player mana for magic spells
    /// </summary>
    private int mana = 100;

    /// <summary>
    /// Player energy for agility spells
    /// </summary>
    private int energy = 100;

    /// <summary>
    /// Player weapon damage, ofc temporary this will be set by his current weapon item + other calculations
    /// </summary>
    private int damage = 50;

    /// <summary>
    /// Movement obj
    /// </summary>
    private Movement movObj;
    #endregion

    #region CoreEvents
    private void Start()
    {
        movObj = GetComponent<Movement>();
    }

    void Update()
    {
        if (StateManager.isDead)
        {
            //TODO: dead anim comes up and hide the frame
            //TODO: also show the try again or respawn
            return; //to prevent actions

        }

        //This is temporary we will update the playerHealth with a delegate function just on change of the hp
        GameObject.Find("UI").GetComponent<GUI_Manager>().UpdatePlayerHealth();

        //only makes sense if we have an enemy by mouseovering him first to get him
        if (opponent != null)
        {

            //Get attack click on the target
            if (Input.GetMouseButtonDown(1) && movObj.InAttackRange(opponent.transform.position))
            {
                if (opponent != null)
                    transform.LookAt(opponent.transform.position); //look to the opponent

                //Call function attack
                Attack();
            }
        }
        else
        {
            //StateManager.isCasting = -1;
        }
    }

    #endregion

    #region Mechanisms
    /// <summary>
    /// Set attack functions and etc as Coroutines not as function
    /// </summary>
    void Attack()
    {

        // Debug.Log("attacking");
        //Do the animation
        //Play the sound
        //Check if we trigger on the body or somewhere of the enemy
        //yes -> apply damage, no -> nothing
        //when the animation finish we set false the animation
       
        StateManager.isRunning = false;
        StateManager.isIdle = false;
        //StateManager.isCasting = -1;
        // SOUND HERE
       
    }


    public void TakeDamage(float dmg)
    {
        if (hp - dmg <= 0)
        {
            hp = 0;
            StateManager.isDead = true;

        }
        else
            hp -= dmg;
    }
    #endregion

    #region Public Methods
    public GameObject ActiveEnemy
    {
        get { return this.opponent; }
        set { this.opponent = value; }
    }

    #endregion

}