using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    public StarterAssets.ThirdPersonController thirdPersonController;
    public StarterAssets.StarterAssetsInputs starterAssetsInputs;

    private bool isPaused = false;

    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            pausePanel.transform.SetAsLastSibling();
        }

        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = false;
        }

        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.enabled = false;
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (thirdPersonController != null)
        {
            thirdPersonController.enabled = true;
        }

        if (starterAssetsInputs != null)
        {
            starterAssetsInputs.enabled = true;
        }

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        Debug.Log("Main Menu pressed.");

        // We'll connect this to a real Main Menu scene later.
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}