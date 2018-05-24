using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFade : MonoBehaviour
{

    [SerializeField] public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.08f;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    public virtual void Awake()
    {
        StartCoroutine(BeginFade(-1));
    }

    public virtual void OnGUI()
    {
        //fade out/in alpha value using direction, speed and time to convert the operation to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        //force (clamp) between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        //set color of the GUI

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);

    }


    /// <summary>
    /// 1 to fadeIn , -1 to fadeOut
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public IEnumerator BeginFade(int direction)
    {
        fadeDir = direction;
        float fadeTime = fadeSpeed;
        yield return new WaitForSeconds(fadeTime);
    }
}
