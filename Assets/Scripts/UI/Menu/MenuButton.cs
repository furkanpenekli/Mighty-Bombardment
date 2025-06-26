using UnityEngine;
using UnityEngine.UI;
public class MenuButton : MonoBehaviour
{
    public void OnButtonClick(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
