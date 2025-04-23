using UnityEngine;

public class MouseControl : MonoBehaviour
{
    private void Start()
    {
        // Make the cursor visible and unlock it
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}