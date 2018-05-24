using HelperPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent))]
[RequireComponent (typeof(AI_Movement))]
[RequireComponent (typeof(Animator))]

public class AI_Combat : MonoBehaviour,IDamageable
{
    /// <summary>
    /// Gets player objects, currently there is only 1 opponent because it's not multiplayer
    /// </summary>
    private GameObject player = null;

    [Header("Mechanism Attributes")]
    /// <summary>
    /// Aggro radius to be able to follow, means if the players comes in our radius we chase him
    /// </summary>
    [SerializeField, Range(1f,40f)] float aggroRadius = 4f;

    /// <summary>
    /// Attack Radius is recommended to be half of aggro radius so we can attack him in this range
    /// </summary>
    [SerializeField, Range(1f, 20f)] public float attackRadius = 2f;

    /// <summary>
    /// For now they are private members
    /// </summary>
    private Animator animator;
    private AI_Movement aiMovement;

    [SerializeField] public float AttackCooldown = 1f;
    private float nextReadyTime;
    private float coolDownTimeLeft;

    public Monster data; //internal data like exp,hp,etc

    //Soon just refactor this, i just need to remake a monster state class but this time using instance 
    #region Monster State Machine

    private bool deadState = false;

    public bool isIdle
    {
        get { return animator.GetBool("isIdle"); }
        set { animator.SetBool("isIdle", value); }
    }

    public bool isWalking
    {
        get { return animator.GetBool("isWalking"); }
        set { animator.SetBool("isWalking", value); }
    }

    public bool isDead
    {
        get { return deadState; }
        set { deadState = value; }
    }

    public bool isAttacking
    {
        get { return animator.GetBool("isAttacking"); }
        set { animator.SetBool("isAttacking", value); }
    }


    #endregion

    void Awake()
    {
       if (data == null)
       {
            data = new Monster("Test", 50,100,100,10f);
       }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiMovement = gameObject.GetComponent<AI_Movement>();
        animator = gameObject.GetComponent<Animator>();

        //Monster state initialization
        isIdle = true;
        isWalking = false;
        isAttacking = false;
        isDead = false;

        //Check if monster has cooldown
        AttackCooldown = AttackCooldown >= 0 ? AttackCooldown : 0;
        Debug.Log("Cooldown : " + AttackCooldown);
    }

    void Update()
    {
        if (isDead)
            return;

        if (player != null && !isDead)
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= aggroRadius)
            {
                aiMovement.SetTarget(player.transform);
               
                //Check if the player is our attack Radius to attack him
                if (distanceToPlayer <= attackRadius)
                {
                    bool coolDownComplete = (Time.time > nextReadyTime);
                    if (coolDownComplete)
                    {
                        Attack(); //TODO: Replace func to Corouting with attack cooldown
                    }
                    else
                    {
                        coolDownTimeLeft -= Time.deltaTime;
                    }
                }
            }
            else
            {
                aiMovement.SetTarget(transform);
                isIdle = true;
                isWalking = false;
                isAttacking = false;
            }
        }
    }

    public void Attack()
    {
        isIdle = false;
        isWalking = false;
        isAttacking = true;

    }

    /// <summary>
    /// Sets the damage to player when the animation is over
    /// </summary>
    /// <param name="i"></param>
    public void AnimSetDamage(float i)
    {
        nextReadyTime = AttackCooldown + Time.time;
        GameObject.FindWithTag("Player").gameObject.GetComponent<Player>().TakeDamage(data.Damage);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);

        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    protected void OnMouseEnter()
    {
        GameObject.FindWithTag("Player").GetComponent<Combat>().ActiveEnemy = gameObject;
        GUI_Manager.instance.SetEnemyName(gameObject.name);
        GUI_Manager.instance.ShowEnemyFrame(gameObject.name);
    }

    protected void OnMouseExit()
    {
        GameObject.FindWithTag("Player").GetComponent<Combat>().ActiveEnemy = null;
        GUI_Manager.instance.HideEnemyFrame(true);
    }

    /// <summary>
    /// Interface implementation of monster self damage
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(float dmg)
    {
        if (data.Health - dmg <= 0)
        {
            data.Health = 0;
            Die();
        }
        else
            data.Health -= dmg;
    }

    public void Die()
    {
        Debug.Log("Killing the monster : " + gameObject.name);
        //Also destroy the opponent 
        GameObject.FindWithTag("Player").GetComponent<Combat>().ActiveTarget = null;
        GameObject.Find("UI").GetComponent<GUI_Manager>().HideEnemyFrame(true);

        //dead anim comes up and hide the frame
        isIdle = false;
        isWalking = false;
        isAttacking = false;

        //Start a coroutine
        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("isDead");
            gameObject.GetComponent<AI_Movement>().SetDead(true);
        }

 
        //After the monster die, transform to a dead body and enable the option to open the loot of this monster


        //Notify the player observer with the exp
        float exp = data.Experience * GameData.ServerEXP; //to be calculated yet the exp of monster multiplying with the server state

        // not working yet player.GetComponent<Player>().LevelUpObservers += SendPlayerEXP;
        SendPlayerEXP(exp);

    }

    /// <summary>
    /// Soon replace with a level observer in player class and just notify
    /// </summary>
    /// <param name="exp"></param>
    public void SendPlayerEXP(float exp)
    {
        ILog.toUnity($"{gameObject.name} is giving {exp} to player..",LType.Processing);
        //first verify if he had lvledup
        int pLevel = player.GetComponent<Player>().Level;
        float pExp = player.GetComponent<Player>().Experience;
        for (int i = 1; i < GameData.LevelsExperience.Length; i++)
        {
            if (pExp > 0 && (pExp + exp) >= GameData.LevelsExperience[pLevel +1])
            {
                pExp -= GameData.LevelsExperience[pLevel + 1];
                pLevel++;
                StartCoroutine(GUI_Manager.instance.DisplayLevelUp(pLevel++));
            }
            else
            {
                pExp += exp;
                break;
            }
        }

        //second notify the internal exp var
        player.GetComponent<Player>().Level = pLevel;
        player.GetComponent<Player>().Experience = pExp;

        //third notify the gui exp bar 
        GUI_Manager.instance.SetPlayerLevel(pLevel);
        GUI_Manager.instance.SetPlayerEXP(pExp);

        //fourth update database level/exp
        
    }
}
