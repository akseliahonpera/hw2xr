
using UnityEngine;
using UnityEngine.InputSystem;

public class Quit : MonoBehaviour
{
    public InputActionReference quitAction;
    // Start is called before the first frame update
    void Start()
    {
        quitAction.action.Enable();
        quitAction.action.performed += (ctx) =>
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif 
        };
        
    }

 
}
