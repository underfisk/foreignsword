using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
* This class will just set the data needed on scripts, the data must be set on gamedata on choose char not here
*/
public class LoadingController : MonoBehaviour
{
    [Header("Array of Sprites/Images")]
    [SerializeField] Texture[] loadingImages;
    [SerializeField] Sprite[] loadingSprites;

    [Header("ObjReferences")]
    [SerializeField] GameObject Spinner;

    protected void Awake()
    {
        if (loadingImages.Length >= 1)
            GetComponent<RawImage>().texture = loadingImages[Random.Range(0, loadingImages.Length)]; //random images from sprite arrays

        StartCoroutine(SpinLoader()); //starts spinner before the screen renders
        Initialize(); //Starts loading data
    }

    private void Initialize()
    {
        UpdateProgressText("Loading Resources..");
        //initialize the game settings
        GameData.InitializeSettings();

        //init achivements

        //init items not working yet
        //GameData.InitializeItems();

        //init level exp base
        GameData.InitializeLevelEXP();

        //init more soon

        //Load the scene finally
        UpdateProgressText("Loading map..");
        SceneManager.LoadSceneAsync("test");
        GameData.MapName = "Foreign Lands"; //as the first map / our open world map


    }

    /// <summary>
    /// Updates the progress label with given string
    /// </summary>
    /// <param name="t"></param>
    private void UpdateProgressText(string t)
    {
        GameObject.Find("ProgressText").GetComponent<Text>().text = t;
    }


    /// <summary>
    /// Corouting Enumerator for Change spinner images
    /// </summary>
    /// <returns></returns>
    IEnumerator SpinLoader()
    {

        int count = loadingSprites.Length;
        int index = 0;

        for (; index <= count; index++)
        {
            if (index >= count)
            {
                index = 0;
                Spinner.GetComponent<Image>().sprite = loadingSprites[index];
            }

            Spinner.GetComponent<Image>().sprite = loadingSprites[index];

            yield return new WaitForSeconds(1);
        }
    }
}
