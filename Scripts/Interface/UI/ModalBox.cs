using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalBox : MonoBehaviour
{
    [SerializeField] public Canvas canvas;
    [SerializeField] public float OverlayDensity = 1f;
    [SerializeField] public bool fadeIn, fadeOut;
    [SerializeField] public float fadeInTime, fadeOutTime;
    [SerializeField] public Text title, content;

    private Vector2 DefaultSize = new Vector2(64, 64);
    private Transform DefaultAnchor;
	
    public enum Type
    {
        None,
        Information,
        Question_Int,
        Question_Float,
        Question_Enum,
        Question_String,
        Question_Double,
        Warning,
        Error,
        SaveLoad //used for save/loading game when we press to save to we show this box and block the game until the game is saved
    }

    /// <summary>
    /// Create an advanced box 
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <param name="title"></param>
    /// <param name="question"></param>
    public void Create(float sizeX, float sizeY, string title, string question, Type _type)
    {
        
    }

    /// <summary>
    /// Creates a stand box using the default size values
    /// </summary>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="_type"></param>
    public void Create(string title, string content, Type _type)
    {

    }

    public void OnClose()
    {
        //When the button close is pressed
    }

    IEnumerator FadeOut()
    {
        if (fadeOut && fadeOutTime > 0)
        {
            yield return new WaitForSeconds(fadeOutTime);
            //Disable the modal
            
        }
    }

    IEnumerator FadeIn()
    {
        if (fadeIn && fadeInTime > 0)
        {
            yield return new WaitForSeconds(fadeInTime);
            //Enable the modal
        }
    }
    


}
