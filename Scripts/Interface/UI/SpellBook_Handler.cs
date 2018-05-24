using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook_Handler : MonoBehaviour
{
    [SerializeField] GameObject SpellCardPrefab;
    [SerializeField] Button SpellBookClose;
    [SerializeField] List<Ability> Spells;

    /// <summary>
    /// TODO: Soon make an optional spell listening by level so we just instantiate the skills that the player can play with
    /// </summary>
    
	protected virtual void Start ()
    {
        SpellBookClose.onClick.AddListener(() => GameObject.Find("Spell Book").gameObject.SetActive(false));
		if (Spells.Count >= 1)
        {
            if (SpellCardPrefab != null)
            {
                foreach(Ability spell in Spells)
                {
                    GameObject card = Instantiate(SpellCardPrefab, transform);
                    //Find the data to be sub
                    Text spell_name = card.transform.Find("Info").transform.Find("Line").transform.Find("name").gameObject.GetComponent<Text>();
                    Text spell_level = card.transform.Find("Info").transform.Find("Line").transform.Find("level").gameObject.GetComponent<Text>();
                    Text spell_desc = card.transform.Find("Info").transform.Find("desc").gameObject.GetComponent<Text>();
                    GameObject spell_icon = card.transform.Find("Spell Slot").transform.Find("Icon").gameObject;

                    //init visual data
                    spell_name.text = spell.name;
                    spell_level.text = $"Level {spell.Level}";
                    spell_desc.text = spell.Description;
                    spell_icon.GetComponent<Image>().sprite = spell.Icon;
                    spell_icon.GetComponent<AbilityUI>().SkillData = spell;

                }
            }
        }
	}
	
}
