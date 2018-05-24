using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {


    public bool isDraggingUI = false;

    /// <summary>
    /// Retrieve the player Animator
    /// </summary>
    /// <returns></returns>
    public static Animator PlayerAnimator
    {
        get
        {
            return GameObject.FindWithTag("Player").gameObject.GetComponent<Animator>();
        }
    }

    /// <summary>
    /// Static getter for isDead 
    /// </summary>
    /// <returns>isDead boolean</returns>
    public static bool isDead
    {
        get { return PlayerAnimator.GetBool("isDead");  }
        set { PlayerAnimator.SetBool("isDead",value); }
    }

    public static bool isRunning
    {
        get { return PlayerAnimator.GetBool("isRunning"); }
        set { PlayerAnimator.SetBool("isRunning", value); }
    }

    public static bool isIdle
    {
        get { return PlayerAnimator.GetBool("isIdle"); }
        set { PlayerAnimator.SetBool("isIdle", value); }
    }

    public static void ImpactPlayer()
    {
        PlayerAnimator.SetTrigger("isImpacted");
    }

    public static int isCasting
    {
        get { return PlayerAnimator.GetInteger("isCasting"); }
        set { PlayerAnimator.SetInteger("isCasting", value); }
    }

    /// <summary>
    /// This function is only for animation event to stop the isAttacking at a trigger time
    /// </summary>
    public void StopAnimFromAttack(float i)
    {
        Debug.Log("Stopping the attack animation");
        isCasting = -1;
        isIdle = true;

        //Re-enable the collider
        foreach(Transform obj in gameObject.GetComponent<Player>().mainWeaponSocket.gameObject.transform)
        {
            if (obj.gameObject.GetComponent<SwordCollider>())
                obj.gameObject.GetComponent<SwordCollider>().collidedTimes = 0;
        }
    }


}
