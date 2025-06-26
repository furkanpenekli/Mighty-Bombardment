using UnityEngine;

public class PlayButton : MonoBehaviour
{
    [SerializeField]
    private Menu _menu;

    public void OnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void CloseMenu()
    {
        _menu.CloseMenu();
    }
}
