using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{


    [Header("Menu")]
    [SerializeField] private GameObject menu;
    private void Awake()
    {
        menu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    private void TogglePause()
    {
        menu.SetActive(!menu.activeSelf);
        Cursor.visible = menu.activeSelf;
        if(menu.activeSelf)
            Time.timeScale=0.0f;
        else
            Time.timeScale=1.0f;
        Cursor.lockState = menu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void backToMainMenu()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        SceneManager.LoadScene("MainMenue");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
