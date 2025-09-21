using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
        PlayerStats.instance.OnDeath.AddListener(() =>
        {
            gameObject.SetActive(true);
            Time.timeScale = 0f;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HyHyScene");
    }
    
    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
