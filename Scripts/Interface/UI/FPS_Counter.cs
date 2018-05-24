using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS_Counter : MonoBehaviour
{

    // Attach this to a GUIText to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // correct overall FPS even if the interval renders something like
    // 5.5 frames.

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval

    [SerializeField] public Text FPS_Text;

    void Start()
    {
        if (HelperPackage.LocalPrefs.FPS == 1)
        {
            FPS_Text.gameObject.SetActive(true);

            if (FPS_Text == null)
                Debug.Log("Please add a text object to be able to show FPS counter");
            else
                timeleft = updateInterval;
        }
    }

    void Update()
    {
        if (HelperPackage.LocalPrefs.FPS == 1)
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                float fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);
                FPS_Text.text = format;
                FPS_Text.gameObject.SetActive(true);

                if (fps < 30)
                    FPS_Text.color = Color.yellow;
                else
                    if (fps < 10)
                    FPS_Text.color = Color.red;
                else
                    FPS_Text.color = Color.green;

                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
            }
        }
        else
            FPS_Text.gameObject.SetActive(false);
    }
}