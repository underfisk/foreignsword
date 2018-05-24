using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatAbilities : MonoBehaviour
{
    public enum BuffType
    {
        HealthBoost,
        ManaBoost,
        DefenseBoost,
        StrengthBoost,
        DexteryBoost,
        AgilityBoost,
        IntelligenceBoost
    }

    public bool CastBuff(int spell_id, float duration, float amount)
    {
        switch (spell_id)
        {
            case 1001:
                //Gives X health
                GameObject.FindWithTag("Player").gameObject.GetComponent<Player>().Health += amount;
                return true;
            case 1002:
                //Gives X dmg boost + for X second
                StartCoroutine(
                    BuffPlayer(BuffType.HealthBoost, duration, amount)
                );
                return true;
            case 1003:
                //Give X def boost + for X second
                break;

        }

        return false;
    }

    public bool CastMelee(int spell_id, int min_damage, int max_damage)
    {
        Debug.Log("Casting spell id : " + spell_id);
        switch(spell_id)
        {
            case 5001:
                StateManager.isCasting = 5001;
                return true;
            case 5002:
                StateManager.isCasting = 5002;
                Debug.Log("State: " + StateManager.isCasting);
                return true;
        }

        return false;
    }



    IEnumerator BuffPlayer(BuffType type, float duration, float amount)
    {
        GameObject player = GameObject.FindWithTag("Player").gameObject;
        //Set the player buff
        switch(type)
        {
            case BuffType.HealthBoost:
                player.GetComponent<Player>().Health += amount;
                break;
            case BuffType.DefenseBoost:
                
                break;
        }

        //Wait time because the buff will be active during this time
        yield return new WaitForSeconds(duration);

        //Remove player buff after the time
        switch (type)
        {
            case BuffType.HealthBoost:
                player.GetComponent<Player>().Health -= amount;
                break;
            case BuffType.DefenseBoost:

                break;
        }
    }

}
