using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void OnButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
