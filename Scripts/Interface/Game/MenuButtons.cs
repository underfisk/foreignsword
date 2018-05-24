using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{

    [SerializeField] public Button resumeBtn;
    [SerializeField] public Button settingsBtn;
    [SerializeField] public Button saveBtn;
    [SerializeField] public Button helpBtn;
    [SerializeField] public Button exitBtn;

    protected void Start()
    {
        //Bind buttons events
        resumeBtn.onClick.AddListener(() => GUI_Manager.instance.menuWindow.SetActive(false)); //just hide the menu cuz it's what we want
        settingsBtn.onClick.AddListener(() => ShowSettings());
        exitBtn.onClick.AddListener(() => ExitApp());
    }

    protected void ShowSettings()
    {
        //load the settings of player
        GUI_Manager.instance.settingsWindow.GetComponent<SettingsManage>().SetHardwareInfo();

        //load player prefs
        GUI_Manager.instance.settingsWindow.GetComponent<SettingsManage>().InitializePrefs();

        //go to default tab
        GUI_Manager.instance.settingsWindow.GetComponent<SettingsManage>().LoadDefaultTab();

        //display the window
        GUI_Manager.instance.menuWindow.SetActive(false); //turn off cuz we dont need it more
        GUI_Manager.instance.settingsWindow.SetActive(true);
    }

    void ExitApp()
    {
        Debug.Log("Exiting the app");
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        Application.Quit();
    }

    protected void OnApplicationQuit()
    {
        //When we quit we save some data
        Debug.Log("saving some data");
    }
}
