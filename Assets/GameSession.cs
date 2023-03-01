using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    public int manaCount = 0;

    private PlayerMovement playerMovement;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI manaText;
	[SerializeField] GameObject fastBulletPrefab;

    [SerializeField] int manaThreshold = 6;

    private const int MAX_MANA = 6;

    void Awake() 
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numGameSessions >= 2)
        {
            Destroy(gameObject);
        } 
        else
        {
            DontDestroyOnLoad(gameObject);
			DontDestroyOnLoad(fastBulletPrefab);
		
        }
    }

    void Start()
    {
        
        if(playerMovement == null)
    	{
			Debug.Log("PlayerISNULL");
       		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        	playerMovement = playerObject.GetComponent<PlayerMovement>();

    	}
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
        manaText.text = manaCount.ToString();
    }

    public void AddToScore(int points)
{
    if (points == 6)
    {
        manaCount++;
        if (manaCount >= 6)
        {
            manaCount = 0;
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerMovement = playerObject.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.StartCoroutine(FastMovement(5f, 2f));
                }
                else
                {
                    Debug.Log("PlayerMovement is null");
                }
            }
            else
            {
                Debug.Log("Player object is null");
            }
        }
        manaText.text = manaCount.ToString();
    }
    else
    {
        score += points;
        scoreText.text = score.ToString();
    }
}


private IEnumerator FastMovement(float duration, float multiplier)
{
    playerMovement.moveSpeed *= multiplier;
    playerMovement.fastShootDelay *= multiplier;
    playerMovement.isFast = true;
    


    yield return new WaitForSeconds(duration);
    
    playerMovement.moveSpeed /= multiplier;
    playerMovement.fastShootDelay /= multiplier;
    playerMovement.isFast = false;
    
}





    public void ProcessPlayerDeath()
    {
        if(playerLives > 1) 
        {
            playerLives--;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
            livesText.text = playerLives.ToString();
        }
        else
        {
            SceneManager.LoadScene("Menu");
            Destroy(gameObject);
        }
    }


}