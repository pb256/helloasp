using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadScene(GameManager.instance.sceneNameMoveTo));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            Debug.Log(async.progress);
            yield return null;
        }
    }
}
