using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    /// <summary>
    /// Important windows goes here in case we need to close
    /// </summary>
    List<GameObject> iWindows;
    private bool changed = false;


    protected void Start ()
    {
        //grab soon the keys associated for the actions
        iWindows = new List<GameObject>();
        iWindows.Add(this.GetComponent<GUI_Manager>().inventoryWindow.gameObject);
        iWindows.Add(this.GetComponent<GUI_Manager>().settingsWindow.gameObject);
        iWindows.Add(this.GetComponent<GUI_Manager>().characterWindow.gameObject);
        iWindows.Add(this.GetComponent<GUI_Manager>().spellBookWindow.gameObject);
        //iWindows.ForEach(x => HelperPackage.ILog.toUnity($"Name:{x.name} "));
	}
	
	private bool CloseImportantWindows()
    {
        bool closedSome = false;
        foreach(GameObject x in iWindows)
        {
            if (x.activeSelf && x.gameObject.name != "Settings")
            {
                HelperPackage.ILog.toUnity($"Closing a window {x.name}");
                x.SetActive(false);
                closedSome = true;
            }
        }
        return closedSome;
        
    }

    //TODO : This is temporary, the inventory needs to get info about it every time we open, the same the others because we cannot just init it
    //Soon : Maybe instead of set active lets see if instantiate and destroy is better in terms of memory saving

	protected void Update ()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!GUI_Manager.instance.menuWindow.activeSelf)
            {
                //Opens the inventory if it's not closed and close if is open
                if (!GUI_Manager.instance.inventoryWindow.activeSelf)
                    GUI_Manager.instance.inventoryWindow.SetActive(true);
                else
                    GUI_Manager.instance.inventoryWindow.SetActive(false);
            }

        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Opens the game menu if it's not closed and close if is open
            if (!CloseImportantWindows())
            {
                if (!GUI_Manager.instance.menuWindow.activeSelf)
                    GUI_Manager.instance.menuWindow.SetActive(true);
                else
                    GUI_Manager.instance.menuWindow.SetActive(false);
            }

            if (GUI_Manager.instance.settingsWindow.activeSelf)
                GUI_Manager.instance.settingsWindow.SetActive(false);

        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (!GUI_Manager.instance.menuWindow.activeSelf)
            {

                //Opens the character window
                if (!GUI_Manager.instance.characterWindow.activeSelf)
                    GUI_Manager.instance.characterWindow.SetActive(true);
                else
                    GUI_Manager.instance.characterWindow.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(GUI_Manager.instance.DisplayLevelUp(5));
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (!GUI_Manager.instance.spellBookWindow.activeSelf)
                GUI_Manager.instance.spellBookWindow.SetActive(true);
            else
                GUI_Manager.instance.spellBookWindow.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            //hides player frame
            GUI_Manager.instance.HideShowPlayerFrame();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(GUI_Manager.instance.DisplayNotification("Test", "Hello hero you're welcome to our game"));
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            CameraController cc = FindObjectOfType<CameraController>();
            if (cc != null)
            {
                if (!changed)
                {
                    cc.height = 2;
                    cc.distance = 5;
                    changed = true;
                }
                else
                {
                    cc.height = 7;
                    cc.distance = 10;
                    changed = false;
                }

            }

        }
	}
}
