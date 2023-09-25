using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public static LevelChanger instance;

    public float transitionTime = 2f;
    [SerializeField] Animator transitionAnim;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else Destroy(gameObject);
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DelayLoadScene(string sceneName)
    {
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        ChangeScene(sceneName);
        transitionAnim.SetTrigger("End");
    }

}
