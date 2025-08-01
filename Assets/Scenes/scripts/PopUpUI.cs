using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelPopupUI : MonoBehaviour
{
 
    public Button continueButton;
    public Button restartButton;

    public string nextLevelName; // Set this in Inspector if using continue

    private void Start()
    {
        gameObject.SetActive(false);
        continueButton.onClick.AddListener(ContinueLevel);
        restartButton.onClick.AddListener(RestartLevel);
    }

    public void ShowPopup()
    {
        gameObject.SetActive(true);
    }

    private void ContinueLevel()
    {
        Debug.Log("Level was supposed to change");
        // Load next level or do anything else
        if (!string.IsNullOrEmpty(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
