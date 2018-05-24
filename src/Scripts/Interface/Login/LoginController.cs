using HelperPackage;
using HttpPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;

public class LoginController : MonoBehaviour {

    [SerializeField] GameObject userError, pwdError;
    [SerializeField] InputField userField;
    [SerializeField] InputField pwdField;
    [SerializeField] Button submitBtn;
    [SerializeField] GameObject logWindow;
    [SerializeField] Button btnLogClose;
    [SerializeField] Text windowTitle, windowBody;
    [SerializeField] GameObject formElements;
    [SerializeField] GameObject loadingWindow;
    [SerializeField] Sprite[] loadingSprites;
    [SerializeField] GameObject fillTarget;

    // Use this for initialization
    void Start () {
        
    
        //Bind submit button an event
        Button btn = submitBtn.GetComponent<Button>();
        btn.onClick.AddListener(SubmitOnClick);

        btnLogClose.onClick.AddListener(HideErrorWindow);

        //Hide them
        userError.SetActive(false);
        pwdError.SetActive(false);
        logWindow.SetActive(false);
        loadingWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        //set the tab event for switch on fields
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (userField.isFocused == true)
            {
                pwdField.Select();
                pwdField.transform.Find("Focus").gameObject.SetActive(true);
                userField.transform.Find("Focus").gameObject.SetActive(false);

            }
            else
            {
                userField.Select();
                pwdField.transform.Find("Focus").gameObject.SetActive(false);
                userField.transform.Find("Focus").gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitOnClick();
        }


    }

    /// <summary>
    /// Handles the click + Keypad enter event
    /// </summary>
    public void SubmitOnClick()
    {
        if (string.IsNullOrEmpty(userField.text))
        {
            userError.GetComponent<Text>().text = "Please insert your username do not leave it blank";
            userError.SetActive(true);
        }
        else if (string.IsNullOrEmpty(pwdField.text))
        {
            userError.SetActive(false);
            pwdError.GetComponent<Text>().text = "Please insert your password do not leave it blank";
            pwdError.SetActive(true);
        }
        else
        {
            //hide the errors label
            userError.SetActive(false);
            pwdError.SetActive(false);

            ILog.toUnity("Authing...");
            submitBtn.enabled = false;


            HttpForm formData = new HttpForm();
            formData.AddField("username", userField.text);
            formData.AddField("password", pwdField.text);

            HttpRequest request = new HttpRequest();
            request.Post(HttpLinks.auth, formData);

            //Re-enable the button again
            submitBtn.enabled = true;

            //Make sure is done
            if (request.isDone)
            {
                //Test debug of json data
                Debug.Log(request.ContentResponse);

                //is anError or there is any http error?
                if (!request.isError || request.statusCode != System.Net.HttpStatusCode.OK)
                {
                    //is a json request?
                    if (request.isJson)
                    {
                        //then we have the user data meaning that the user has been logged succesfully
                        ShowLoadingWindow();

                        //handle json request and set the data we need from the user account
                        bool success = JsonHelper.toAccount(request.ContentResponse);
                        if (success)
                        {
                            //load the scene now async
                            SceneManager.LoadSceneAsync("CharactersScene");
                        }
                        else
                        {
                            DisplayError("Account Server Error", "Our gateway is not buffering your acount, please restart the game");
                        }
                    }
                    else
                    {
                        //filter it and see if is any internal error if we're just expecting to be a json result
                        DisplayError("Bad reply", "Server has returned a bad response, please try to login again");
                    }
                }
                else
                {
                    //Show the error message
                    ILog.toUnity("Cant connect to the server");
                    DisplayError($"Server Status: {request.statusCode}", "Error connecting with the Remote Host");
                }
            }
        }

       
    }


    protected void UpdateText(string t)
    {
        windowBody.text = t;
    }

    protected void DisplayError(string title,string txt)
    {
        HideFormElements();
        windowTitle.text = title;
        windowBody.text = txt;
        logWindow.SetActive(true);
    }

    protected void HideErrorWindow()
    {
        if (logWindow.activeSelf)
            logWindow.SetActive(false);

        ShowFormElements();
    }

    protected void ShowFormElements()
    {
        if (!formElements.activeSelf)
            formElements.SetActive(true);
    }

    protected void HideFormElements()
    {
        if (formElements.activeSelf)
            formElements.SetActive(false);
    }

    protected void ShowLoadingWindow()
    {
        HideFormElements();
        loadingWindow.SetActive(true);
        StartCoroutine(SpinLoader()); //start spinner
                                      //update the text under the gif


    }

    protected void StopLoadingWindow()
    {
        loadingWindow.SetActive(false);
    }

    IEnumerator SpinLoader()
    {

        int count = loadingSprites.Length;
        int index = 0;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        for (; index <= count;index++)
        {
            /*if (index >= count) This is the correct one
                index = 0;
                fillTarget.GetComponent<Image>().sprite = loadingSprites[index];
             */

            //Now when reaches the max we load the game
            if (index >= count)
            {
                SceneManager.UnloadSceneAsync(0);
 
            }
            else
                fillTarget.GetComponent<Image>().sprite = loadingSprites[index];

            yield return new WaitForSeconds(1);
        }


    }
    
}
