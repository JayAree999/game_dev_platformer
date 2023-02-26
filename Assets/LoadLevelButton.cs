using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelButton : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneManager.LoadScene("L1");
    }
    public void LoadSelect()
    {
        SceneManager.LoadScene("Select");
    }
    
    public void L2()
    {
        SceneManager.LoadScene("L2");
    }
    
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

  

    
}