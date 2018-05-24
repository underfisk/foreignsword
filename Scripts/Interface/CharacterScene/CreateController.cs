using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HelperPackage;
using System.Text.RegularExpressions;
using HttpPackage;

public class CreateController : MonoBehaviour
{
    [SerializeField] Toggle barbarianToogle, warriorToogle;
    [SerializeField] Button submitCreateBtn, returnBtn;
    [SerializeField] Text errorText;
    [SerializeField] InputField charName;
    [SerializeField] public int choosedClass = 2;

    [Header("Prefabs")]
    [SerializeField] GameObject warriorPrefab, barbarianPrefab, charPosPrefab;


    protected void Start()
    {
        warriorToogle.onValueChanged.AddListener(delegate{
            ClassChange(1);
        });

        barbarianToogle.onValueChanged.AddListener(delegate {
            ClassChange(2);
        });
    }

    protected void ClassChange(int class_id)
    {
        if (choosedClass != class_id)
        {
            choosedClass = class_id;
            ILog.toUnity($"Changing choosed class to {class_id}..");
            switch (class_id)
            {
                case 1:
                    ChangeModel(warriorPrefab);
                    break;
                case 2:
                    ChangeModel(barbarianPrefab);
                    break;
            }
        }
    }

    private void ChangeModel(GameObject prefabName)
    {

        bool charPosExists = GameObject.Find("CharacterPosition").gameObject ? true : false;
        if (charPosExists)
        {
            GameObject charPos = GameObject.Find("CharacterPosition").gameObject;
            //Check if he has already a model so we destroy it
            if (charPos.transform.childCount >= 1)
            {
                foreach (Transform child in charPos.transform)
                {
                    ILog.toUnity($"Succesfully destroyed the old model named: {child.name}", LType.Destroyed);
                    Destroy(child.gameObject);
                }
            }


            ILog.toUnity("Instantiating the character..");
            GameObject obj = Instantiate(prefabName,charPos.transform) as GameObject;

            //Bind the model to the character position
            obj.transform.localPosition = Vector3.zero;
            Destroy(obj.GetComponent<Combat>());
            Destroy(obj.GetComponent<Movement>());
            Destroy(obj.GetComponent<Player>());
            ILog.toUnity($"New object in scene named {obj.name}");
        }
        else
        {
            ILog.toUnity($"Instantiating characterPosition in scene ", LType.Success);
            GameObject charPos = Instantiate(charPosPrefab) as GameObject;
            charPos.transform.localPosition = new Vector3(166.41f, 0.4f, 391.05f);

            GameObject obj = Instantiate(prefabName, charPos.transform) as GameObject;
            //Bind the model to the character position
            obj.transform.localPosition = Vector3.zero;
            Destroy(obj.GetComponent<Combat>());
            Destroy(obj.GetComponent<Movement>());
            Destroy(obj.GetComponent<Player>());
        }
        
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            ProcessCreation();
    }

    protected void Awake()
    {
        Debug.Log("The player id is => " + GameData.PlayerID);
        errorText.text = "Undefined";
        errorText.gameObject.SetActive(false);

        //test json data to array in sv
        string js = "[{\"name\": \"Joana\", \"city\": \"CB\"}]";

        HttpForm formData = new HttpForm();
        formData.AddField("jsdata", js);

        HttpRequest request = new HttpRequest();
        request.Post(HttpLinks.base_url + "test", formData);
        if (request.isDone)
        {
            if (!request.isError || request.statusCode != System.Net.HttpStatusCode.OK)
            {
                Debug.Log("request = " + request.ContentResponse);
                ILog.toUnity("Test error", LType.Success);
            }
        }
        submitCreateBtn.onClick.AddListener(() => ProcessCreation());
        returnBtn.onClick.AddListener(() => SceneManager.LoadSceneAsync("CharactersScene"));
    }

    private void ProcessCreation()
    {
        errorText.gameObject.SetActive(false);
        if (charName.text == "")
        {
            errorText.text = "Please define a name";
            errorText.gameObject.SetActive(true);
            return;
        }
        else
        {
            errorText.text = "";
            errorText.gameObject.SetActive(false);
        }


        ILog.toUnity("Processing the creation");
        HttpForm formData = new HttpForm();
        formData.AddField("char_name", charName.text);

        HttpRequest request = new HttpRequest();
        request.Post(HttpLinks.character_exists, formData);

        if (request.isDone)
        {
            if (!request.isError || request.statusCode != System.Net.HttpStatusCode.OK)
            {
                if (request.ContentResponse == "no")
                {
                    //does not exists
                    ILog.toUnity("Character name is able to use!", LType.Success);

                    HttpForm charData = new HttpForm();
                    charData.AddField("u_id", GameData.PlayerID);
                    charData.AddField("c_name", charName.text);
                    charData.AddField("c_class", choosedClass);

                    HttpRequest request2 = new HttpRequest();
                    request2.Post(HttpLinks.character_create, charData);


                    ILog.toUnity("Data being sent : " + charData.ToString());

                    if (request2.isDone)
                    {
                        if (!request2.isError || request2.statusCode != System.Net.HttpStatusCode.OK)
                        {
                            ILog.toUnity("Request of creating : " + request2.ContentResponse);
                            if (request2.ContentResponse == "success")
                            {
                                //we say we added showing the modal
                                ILog.toUnity($"CreateRequest : {request2.ContentResponse} ", LType.Success);
                                SceneManager.LoadScene("CharactersScene");
                            }
                            else
                            {
                                //lets filter the error
                                ILog.toUnity($"On CreateRequest Error : {request2.ContentResponse} ", LType.Error);
                                errorText.text = "Internal Server Error, restart the game!";
                                errorText.gameObject.SetActive(true);
                            }
                        }
                    }
                }
                else
                {
                    //filter the message and check if is error or not
                    ILog.toUnity("Exists or request :" + request.ContentResponse);
                }
            }
            else
            {
                //throw internal error
                errorText.text = "Internal server error";
                errorText.gameObject.SetActive(true);
            }
        }


        //if yes we say to choose another in errorText
        //else we process

        //in process we try to httprequest the char data
    }
}
