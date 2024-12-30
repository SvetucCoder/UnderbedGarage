using UnityEngine;

public class WindAudio : MonoBehaviour
{
    public AudioSource audio;
    public float fullradius;
    public float radius;
    public float decreaseradius;
    public float fullvolume;
    public float volume;
    public float decreasevolume;
    public bool window1;
    public bool window2;
    void Start()
    {
        
    }

    public void SetWindow1(bool window)
    {
        window1 = window;
    }
    public void SetWindow2(bool window)
    {
        window2 = window;
    }
    void Update()
    {
        radius = fullradius;
        volume = fullvolume;
        if (window1)
        {
            radius -= decreaseradius;
            volume -= decreasevolume;
        }
        if (window2)
        {
            radius -= decreaseradius;
            volume -= decreasevolume;
        }
        audio.maxDistance = radius;
        audio.volume = volume;
    }
}
