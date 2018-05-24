using HelperPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour {

    public VideoClip videoToPlay;
    public RawImage imgTarget;

    private VideoPlayer videoPlayer;
    private VideoSource videoSource;

    private AudioSource audioSource;
	void Start () {
        Application.runInBackground = true;

        StartCoroutine(PlayVideo());
	}
	
    IEnumerator PlayVideo()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            ILog.toUnity("Preparing the video..");
            yield return waitTime;
            break;
        }

        imgTarget.texture = videoPlayer.texture;
        videoPlayer.Play();
        ILog.toUnity("Playing the video.");
    }
}
