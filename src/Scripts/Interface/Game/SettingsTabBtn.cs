using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingsTabBtn : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] GameObject tabsRoot;
    [SerializeField] GameObject tabsContentRoot;
    [SerializeField] GameObject targetObject;
    [SerializeField] public bool isActive;

    private void Awake()
    {
        Button x = this.gameObject.GetComponent<Button>();
        x.onClick.AddListener(() => OnClick());

        //TODO: Fix the bug where the last active is still true

        if (isActive)
        {
            gameObject.transform.Find("Text").GetComponent<Text>().color = Color.white;
            gameObject.transform.Find("Active").gameObject.SetActive(true);
            isActive = true;
            targetObject.SetActive(true);
        }
    }

    private void OnClick()
    {
        //Sets the tab as disable
        foreach(Transform child in tabsRoot.transform)
        {
            if (child.gameObject.name != gameObject.name && child.gameObject.GetComponent<SettingsTabBtn>())
            {
                if (child.gameObject.GetComponent<SettingsTabBtn>().isActive)
                {
                    child.gameObject.transform.Find("Text").GetComponent<Text>().color = Color.grey;
                    child.gameObject.GetComponent<SettingsTabBtn>().isActive = false;
                    gameObject.transform.Find("Active").gameObject.SetActive(false);
                }
            }
        }
        
        //Disabled the tabContent of previous object
        foreach(Transform childz in tabsContentRoot.transform)
        {
            if (childz.gameObject.activeSelf && childz.gameObject.name != targetObject.gameObject.name)
                childz.gameObject.SetActive(false);
        }

        //Disable the active child if he's on
        foreach(Transform child in tabsRoot.transform)
        {
            if (child.gameObject.GetComponent<SettingsTabBtn>() && child.gameObject.name != gameObject.name)
            {
                if (child.transform.Find("Active").gameObject.activeSelf)
                    child.transform.Find("Active").gameObject.SetActive(false);
            }
        }

        //is not active? then lets set it as active
        if (!isActive)
        {
            gameObject.transform.Find("Text").GetComponent<Text>().color = Color.white;
            gameObject.transform.Find("Active").gameObject.SetActive(true);
            isActive = true;
            targetObject.SetActive(true);

        }

    }
}
