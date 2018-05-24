using HelperPackage;
using UnityEngine;

public class SwordCollider : MonoBehaviour
{
    /// <summary>
    /// Prevent multiple clicks var 
    /// </summary>
    public int collidedTimes;

    protected void Awake()
    {
        collidedTimes = 0;
        Debug.Log("Initialized collided times, CollidedTimes : " + collidedTimes);
    }

    /// <summary>
    /// When this collider detects other collider we filter it
    /// </summary>
    /// <param name="_c"></param>
    private void OnTriggerEnter(Collider _c)
    {
        if (StateManager.isDead) return;
        if (StateManager.isIdle) return;
        if (StateManager.isRunning) return;

        //Is the other collider an enemy and we are attacking?
        if (_c.CompareTag("Enemy") && StateManager.isCasting != -1)
        {
            //We haven't collide/attack the enemy?
            if (collidedTimes == 0)
            {
                var enemy_mob = GameObject.Find(_c.name).gameObject;
                var enemy_animator = enemy_mob.GetComponent<Animator>();

                var damageable = _c.gameObject.GetComponent(typeof(IDamageable)); //does the other enemy has IDamageable interface?
                if (damageable && !enemy_mob.GetComponent<AI_Combat>().isDead)
                {
                    collidedTimes = 1;

                    float dmg = 50;

                    (damageable as IDamageable).TakeDamage(dmg); //notice the take damage function of this monster
                    
                    StartCoroutine(
                        GameObject.Find("UI").gameObject.GetComponent<PopupDamage>().Display(dmg,enemy_mob.gameObject)
                    );

                    enemy_animator.SetTrigger("isImpacted"); //to be removed

                    GameObject.Find("UI").GetComponent<GUI_Manager>().UpdatePlayerHealth();
                }


            }
        }
    }

    /// <summary>
    /// When our collider gets out, we set hasCollide to false so he can attack after
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        //Lets test making sure if the player is not attacking before reset var
        if (collidedTimes == 1)
        {
            Debug.Log("Collided times : " + collidedTimes);
           // collidedTimes = 0; //reset the var
        }

    }


}
