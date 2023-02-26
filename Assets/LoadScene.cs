using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneName;  // The name of the scene to load

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Check for left mouse button click
        {
            LoadSceneByName(sceneName);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        LoadSceneByName(sceneName);
    }

    void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}