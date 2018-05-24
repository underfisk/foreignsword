using HelperPackage;
using HttpPackage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CharacterSceneController : MonoBehaviour
{

    /// <summary>
    /// Temporary List of characters
    /// </summary>
    public static List<Character> playerCharacters;

    public List<Character> _Characters
    {
        get
        {
            return playerCharacters;
        }
        set
        {
            playerCharacters = value;
        }
    }
    /// <summary>
    /// Max allowed characters creation and default character index
    /// </summary>
    private const int MAX_ALLOWED_CHARS = 5, DEFAULT_CHAR_INDEX = 0;

    [Header("Prefabs Section")]
    [SerializeField,Tooltip("Represents the character rendering position where the model will be placed on")] public GameObject RenderPosition;
    [SerializeField,Tooltip("Character grid card which shows the character information in a card format")] public GameObject CardPrefab;
    [SerializeField,Tooltip("Warrior class model")] public GameObject WarriorPrefab;
    [SerializeField,Tooltip("Barbarian class model")] public GameObject BarbarianPrefab;

    /// <summary>
    /// Characters layout CharactersGrid
    /// </summary>
    [SerializeField] public GridLayoutGroup CharactersGrid;

    /// <summary>
    /// Singleton instance to access character thread
    /// </summary>
    public static CharacterSceneController Instance;

    /// <summary>
    /// Handles the 3 main buttons
    /// </summary>
    [Header("ActionButtons")]
    [SerializeField] Button CreateButton, PlayButton, LogoutButton;

    /// <summary>
    /// Variable used to control which character is selected
    /// </summary>
    [SerializeField] public int SelectedCharacterIndex = -1;

    [SerializeField] public GameObject DeletePopup;


    protected void Start()
    {
        //Initializes the button handlers
        CreateButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("create_character"));
        PlayButton.onClick.AddListener(() => OnPlayClick());

        //Singleton instance is null? then we set this instance
        if (Instance == null) Instance = this;

        ILog.toUnity($"Initializing user id [{GameData.PlayerID}] characters..");
        InitCharacters();


    }

    protected void Logout()
    {
        //soon when we refactor => delete session
        //delete local data 
        //return to login scene
    }

    /// <summary>
    /// Protected function used to instantiate the characters
    /// </summary>
    protected void InitCharacters()
    {

        //Initialize playerCharacters with database information
        RetrieveDatabaseCharacters();

        //Instantiate the characters prefab and set his details
        if (playerCharacters != null && playerCharacters.Count >= 1)
        {
            int i = 0;
            foreach (Character c in playerCharacters)
            {
                ILog.toUnity($"Instantiating the character: {c.Name} on the grid..");
                AppendToGrid(CardPrefab, c, i);
                i++;
            }

            //Before we show everything, we also bind the button create character button
           // AddCharacterButton();

            //Set default index if he has some char
            UpdateCharacter(playerCharacters[DEFAULT_CHAR_INDEX].ID, playerCharacters[DEFAULT_CHAR_INDEX].ClassID);
            ILog.toUnity("The scene data is already set, the char 0 is set", LType.Success);
        }
    }

    /// <summary>
    /// Handles the delete button event and proceed deleting the character
    /// </summary>
    public void OnDeleteClick()
    {
        //TODO: Used the modal to confirm name, not just delete
        if (SelectedCharacterIndex != -1)
        {
            ILog.toUnity("Deleting.. char id = " + SelectedCharacterIndex);
            HttpForm formData = new HttpForm();
            formData.AddField("char_id", SelectedCharacterIndex);

            HttpRequest request = new HttpRequest();
            request.Post(HttpLinks.character_delete, formData);

            if (request.isDone)
            {
                if (!request.isError || request.statusCode != System.Net.HttpStatusCode.OK)
                {
                    ILog.toUnity("Deleted.", LType.Success);
                    //After we remove the gameobject with this id from the CharactersGrid
                    Destroy(GameObject.Find($"Character_ID_{SelectedCharacterIndex}"));
                    if (playerCharacters.Count >= 1)
                    {
                        UpdateCharacter(playerCharacters[DEFAULT_CHAR_INDEX].ID, playerCharacters[DEFAULT_CHAR_INDEX].ClassID);
                    }
                }
                else
                {
                    ILog.toUnity("Failed deleting the character", LType.Error);
                    //TODO: Show an error message
                }
            }
        }
    }

    /// <summary>
    /// Instantiates the character model at characterPosition gameobject receiving the class_id
    /// </summary>
    /// <param name="class_id"></param>
    public void ChangeModel(int class_id)
    {
        switch (class_id)
        {
            case 1:
                InstantiateModel(WarriorPrefab);
                break;
            case 2:
                InstantiateModel(BarbarianPrefab);
                break;
        }
    }

    /// <summary>
    /// On Play we retrieve the current character data and initialize gamedata char
    /// </summary>
    public void OnPlayClick()
    {
        //Check if any char is selected before
        if (SelectedCharacterIndex != -1)
        {
            //Set chardata
            ILog.toUnity("Seleted char id => " + SelectedCharacterIndex);

            foreach (Character a in playerCharacters.Where(x => x.ID == SelectedCharacterIndex))
            {
                GameData.CharacterData = a;
                break;
            }

            //just proceed after
            ILog.toUnity("Playing..");
            SceneManager.LoadSceneAsync("loading");
        }
        else
        {
            //Show the GUI Message Box to handle this error
        }
    }


    /// <summary>
    /// When the character is clicked we update the character plus we change the model
    /// </summary>
    /// <param name="c"></param>
    public void UpdateCharacter(int char_id, int class_id)
    {
        SelectedCharacterIndex = char_id;
        ChangeModel(class_id);

    }

    /// <summary>
    /// Append character card to the CharactersGrid
    /// </summary>
    /// <param name="prefabName"></param>
    /// <param name="charData"></param>
    /// <param name="index"></param>
    public void AppendToGrid(GameObject prefabName, Character charData, int index)
    {

        GameObject obj = Instantiate(prefabName) as GameObject;
        obj.name = $"Character_ID_{charData.ID}";
        obj.GetComponent<CharacterPickController>().myData = charData;
        obj.GetComponent<CharacterPickController>().order_index = index;
        obj.GetComponent<CharacterPickController>().internalID = charData.ID;
        //Name object
        obj.transform.Find("Name").GetComponent<Text>().text = charData.Name;
        
        //Level object
        obj.transform.Find("SubInfo").transform.Find("Level").GetComponent<Text>().text = charData.Level.ToString();

        //Class obj
        obj.transform.Find("SubInfo").transform.Find("Class").GetComponent<Text>().text = ClassName(charData.ClassID);

        obj.transform.SetParent(CharactersGrid.transform);
        obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        if (index == 0)
        {
            //Sets the active border and active as true
            obj.transform.Find("Active Overlay").gameObject.SetActive(true);
            obj.GetComponent<CharacterPickController>().Active = true;

      
            //Sets text color
            obj.transform.Find("Name").GetComponent<Text>().color = Color.white;

        }

        //Finally we check if is enable or not and force to be if it isn't
        if (!obj.gameObject.activeSelf)
            obj.gameObject.SetActive(true);
    }


    /// <summary>
    /// Initializes the character model with the information given by database
    /// </summary>
    /// <param name="prefabName"></param>
    public void InstantiateModel(GameObject prefabName)
    {
        bool charPosExists = GameObject.Find("CharacterSpawnPoint").gameObject ? true : false;
        if (charPosExists)
        {
            GameObject charPos = GameObject.Find("CharacterSpawnPoint").gameObject;
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
            GameObject obj = Instantiate(prefabName, RenderPosition.transform.position, Quaternion.Euler(new Vector3(0, 90, 0))) as GameObject;

            obj.transform.SetParent(charPos.transform);
            obj.AddComponent<CharacterRotate>();
            Destroy(obj.GetComponent<Combat>());
            Destroy(obj.GetComponent<Movement>());
            Destroy(obj.GetComponent<Player>());
            Destroy(obj.GetComponent<NavMeshAgent>());
            ILog.toUnity($"New object in scene named {obj.name}");

        }
        else
        {
            ILog.toUnity($"Instantiating characterPosition in scene ", LType.Success);
            GameObject charPos = Instantiate(RenderPosition) as GameObject;
            charPos.transform.position = new Vector3(163.621f, 0.529f, -389.455f);

            GameObject obj = Instantiate(prefabName, RenderPosition.transform.position, Quaternion.Euler(new Vector3(0, 90, 0))) as GameObject;

            obj.transform.SetParent(charPos.transform);
            obj.AddComponent<CharacterRotate>();
            Destroy(obj.GetComponent<Combat>());
            Destroy(obj.GetComponent<Movement>());
            Destroy(obj.GetComponent<Player>());
            Destroy(obj.GetComponent<NavMeshAgent>());


        }
    }

    /// <summary>
    /// Retrieves the characters information from the database
    /// </summary>
    /// <returns></returns>
    protected bool? RetrieveDatabaseCharacters()
    {
        //Retrieve the characters from database
        HttpForm formData = new HttpForm();
        formData.AddField("user_id", GameData.PlayerID);

        HttpRequest request = new HttpRequest();
        request.Post(HttpLinks.character, formData);

        if (request.isDone)
        {
            if (!request.isError || request.statusCode != System.Net.HttpStatusCode.OK)
            {
                if (request.isJson)
                {
                    playerCharacters = JsonHelper.toCharacters(request.ContentResponse);
                    return true;
                }
                else
                    return false;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the class name according to the class id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string ClassName(int id)
    {
        switch(id)
        {
            case 0:
                return "Warrior";
            case 1:
                return "Barbarian";
        }
        return "";
    }


}
