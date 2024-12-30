using UnityEngine;
using UnityEngine.Video;

public class Win7Start : MonoBehaviour
{
    public VideoPlayer video;
    public VideoClip clip;
    public VideoClip clip2;
    public GameObject obh;
    public bool play;
    float timer = 0;
    public void PlayVideo()
    {
 
        video.clip = clip;
    }
    public void StopVideo()
    {
        video.clip = clip2;
    }

}
