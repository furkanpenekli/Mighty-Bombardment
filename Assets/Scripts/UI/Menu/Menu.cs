using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuUI;

    private void Awake()
    {
        CloseMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (menuUI != null && menuUI.activeSelf)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void OpenMenu()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(true);
            PauseGame();
        }
    }

    public void CloseMenu()
    {
        if (menuUI != null)
        {
            menuUI.SetActive(false);
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
