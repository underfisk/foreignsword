using System;
using UnityEngine;
using UnityEngine.UI;

/**
* TODO: Remove slot key from ability and add from player prefs 
*/
[RequireComponent(typeof(Button))]
public class SlotActions : MonoBehaviour
{
    [SerializeField,Tooltip("Add a scriptable object here and the slot gathers automaticly the data from it")] public Ability SkillData;
    [SerializeField] public Text CooldownText;
    [SerializeField] public Image Mask; //mask for disabled cast
    private GameObject CooldownObject;
    private float coolDownDuration;
    private float nextReadyTime;
    private float coolDownTimeLeft;
    private Image Icon;
    private Text Hotkey;
    private bool isCasting = false;

    
    protected void Start()
    {
        //Initialize icon 
        Icon = gameObject.transform.Find("Icon").gameObject.GetComponent<Image>();

        //Initialize hotkey
        Hotkey = gameObject.transform.Find("Hotkey").gameObject.GetComponent<Text>();

        //Initialize cdObj
        CooldownObject = transform.Find("Cooldown").gameObject;

        //Verify if has something also control the gui, cooldowns etc
        gameObject.GetComponent<Button>().onClick.AddListener(() => HandleClick());

        //Initialize gui if he has a skill
        if (SkillData != null)
        {
            if(SkillData.Icon != null)
            {
                Icon.gameObject.SetActive(true);
            }

            LoadSkillData();
            CooldownObject.gameObject.SetActive(false);
            CooldownText.gameObject.SetActive(false);
            Mask.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Disables the cooldown text and remove the dark mask so we know its ready but also here we will blink an effect soon
    /// </summary>
    private void AbilityReady()
    {
        CooldownObject.gameObject.SetActive(false);
        CooldownText.gameObject.SetActive(false);
        Mask.gameObject.SetActive(false);
    }

    /// <summary>
    /// Button listener for skill handling
    /// </summary>
    protected void HandleClick()
    {
        //1st verifys if he has a skill on the slot
        if (SkillData != null)
        {
            //The cooldown has passed?
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (coolDownComplete)
            {
                CombatAbilities ca = new CombatAbilities();
                GameObject player = GameObject.FindWithTag("Player").gameObject;

                // abilitySource.clip = ability.aSound;
                // abilitySource.Play();
                // ability.TriggerAbility();
                //TODO ..

                //TODO : Also in melee check if we have a weapon otherwise we dont do anything or just simply pop up an message
                if (StateManager.isCasting == -1)
                {
                    switch (SkillData.SkillType)
                    {
                        case Ability.Type.Melee:
                            //Usually melee will check if is in enemy range and after we trigger the spell
                            if (SkillData.NeedTarget)
                            {
                                //do we have enemy?
                                if (player.GetComponent<Combat>().ActiveEnemy != null)
                                {
                                    //are we in range of attack?
                                    if (player.GetComponent<Movement>().InAttackRange(player.GetComponent<Combat>().ActiveEnemy.transform.position))
                                    {
                                        //Set our destination locally and stop whatever we're doing
                                        player.GetComponent<Movement>().NavAgent.SetDestination(transform.position);
                                        StateManager.isIdle = false;
                                        StateManager.isRunning = false;

                                        //Force player looking at him
                                        player.GetComponent<Transform>().LookAt(player.GetComponent<Combat>().ActiveEnemy.transform.position);
                                        ca.CastMelee(SkillData.ID, SkillData.MinDamage, SkillData.MaxDamage);
                                        SkillUsed();
                                    }
                                    else
                                    {
                                        //POP UP a message Too far away
                                        StartCoroutine(GUI_Manager.instance.DisplayWarningBox("The target is too far away", 1f));
                                    }
                                }
                                else
                                {
                                    //POP UP No unit selected
                                    StartCoroutine(GUI_Manager.instance.DisplayWarningBox("Ability requires a target", 1f));
                                }
                            }
                            else
                            {
                                Debug.Log("We dont need target so lets cast");
                                player.GetComponent<Movement>().NavAgent.SetDestination(transform.position);
                                StateManager.isIdle = false;
                                StateManager.isRunning = false;
                                ca.CastMelee(SkillData.ID, SkillData.MinDamage, SkillData.MaxDamage);
                                SkillUsed();

                            }

                            break;
                        case Ability.Type.Buff:
                            //In buffs we'll just have personal buffs so this one we just start the coroutine of the buff giving the buff data

                            break;
                        case Ability.Type.Spell:
                            //In this case will be more hard so will be the last one
                            break;

                    }
                }
                else
                {
                    StartCoroutine(GUI_Manager.instance.DisplayWarningBox("You have a cast ability active!", 1f));
                }
            }
            else
            {
                Cooldown();
                StartCoroutine(GUI_Manager.instance.DisplayWarningBox("Spell is still in cooldown", 1f));
            }
                
        }
    }

    /// <summary>
    /// Reactivates the cooldown
    /// </summary>
    protected void SkillUsed()
    {
        nextReadyTime = coolDownDuration + Time.time; //set the cooldown
        coolDownTimeLeft = coolDownDuration;
        CooldownObject.gameObject.SetActive(true);
        Mask.gameObject.SetActive(true);
        CooldownText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Calculates the cooldown left and formats it to output
    /// </summary>
    protected void Cooldown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCd = Mathf.Round(coolDownTimeLeft);
        CooldownText.text = roundedCd.ToString();
        Mask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }


    /// <summary>
    /// Changes the important things
    /// </summary>
    public void OnAbilitySwap()
    {
        Debug.Log($"Swapping the {gameObject.name} ability");
        gameObject.transform.Find("Icon").transform.SetAsFirstSibling();
        LoadSkillData();
        if (!Icon.gameObject.activeSelf) Icon.gameObject.SetActive(true);
    }

    /// <summary>
    /// Cleans the ability from the slot
    /// </summary>
    public void CleanAbility()
    {
        Icon.gameObject.SetActive(false);
        CooldownObject.SetActive(false);
        Hotkey.text = "TODO";
        SkillData = null;
    }


    protected void Update()
    {

        if (SkillData != null)
        {
            bool coolDownComplete = (Time.time > nextReadyTime);
            if (coolDownComplete)
            {
                AbilityReady();

                if (Input.GetKeyDown(SkillData.KeyOnPress))
                {
                    gameObject.GetComponent<Button>().onClick.Invoke(); //Calls the func of the button

                }
            }
            else
            {
                Cooldown();
            }
        }

    }

    /// <summary>
    /// Reloads the GUI data with the new skill data
    /// </summary>
    protected void LoadSkillData()
    {
        Icon.sprite = SkillData.Icon;
        coolDownDuration = SkillData.Cooldown;
        Hotkey.text = SlotKey(SkillData.KeyOnPress);
    }

    /// <summary>
    /// Returns the translated key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    protected string SlotKey(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.Alpha0:
                return "0";
            case KeyCode.Alpha1:
                return "1";
            case KeyCode.Alpha2:
                return "2";
            case KeyCode.Alpha3:
                return "3";
            default:
                return "Invalid Key";
        }
    }
}