using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource bgm;
    public AudioSource sFx;
    public AudioClip crashSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    
    public void Crash()
    {
        sFx.PlayOneShot(crashSound);
    }

    public void Pause()
    {
        AudioListener.pause = true;
    }

    public void Resume()
    {
        AudioListener.pause = false;
    }

    public void Restart()
    {
        AudioListener.pause = false;
        Invoke("Replay", LevelChanger.instance.transitionTime);
    }

    void Replay()
    {
        bgm.Play();
    } 

    public void Mute(bool isMuted)
    {
        Debug.Log(isMuted);
        AudioListener.volume = isMuted ? 0 : 1;
    }


}
