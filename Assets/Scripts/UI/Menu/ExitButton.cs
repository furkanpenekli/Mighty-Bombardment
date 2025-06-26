using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void OnButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
