using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoorTrigger : MonoBehaviour
{
    public string nextScene;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (gameObject.name == "LevelDoorL")
                GameManager.instance.SavePosition(sceneIndex, new Vector2(-6f, -3.0f));
            else if (gameObject.name == "LevelDoorR")
                GameManager.instance.SavePosition(sceneIndex, new Vector2(7f, -3.0f));
            LevelChanger.instance.ChangeScene(nextScene);
        }
    }
}
