using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class CameraLook : MonoBehaviour
{
    private InputAction lookAction;
    private Camera view;

    public float xAngle = 30;
    public float yAngle = 15;
    public float Sensitivity = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Mouse Movement Action
        lookAction = InputSystem.actions.FindAction("Look");

        // Mouse locked in window and hidden
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Move mouse to middle
        Mouse.current.WarpCursorPosition(new Vector3(Screen.width/2,Screen.height/2,0)); 

        // Get camera
        view = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get distance mouse moved
        Vector2 lookValue = lookAction.ReadValue<Vector2>();

        // If camera not out of range, rotate proportionately to mouse movement 
        if (transform.localEulerAngles.y >= (360 - (xAngle / 2) - 0.1) || transform.localEulerAngles.y <= ((xAngle / 2) + 0.1))
        {
            transform.Rotate(0, lookValue.x * Sensitivity / Screen.width * xAngle, 0, Space.World);
            if (transform.localEulerAngles.y > 180f && transform.localEulerAngles.y < (360 - (xAngle / 2)))
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (360 - (xAngle / 2)), 0);
            }
            if (transform.localEulerAngles.y > (xAngle / 2) && transform.localEulerAngles.y < 180f)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (xAngle / 2), 0);
            }
        }

        if (transform.localEulerAngles.x >= (360-(yAngle/2)-0.1) || transform.localEulerAngles.x <= ((yAngle/2)+0.1))
        {
            transform.Rotate(lookValue.y*-1*Sensitivity/Screen.height*yAngle, 0, 0, Space.Self);
            if (transform.localEulerAngles.x > 180f && transform.localEulerAngles.x < (360-(yAngle/2)))
            {
                transform.localEulerAngles = new Vector3((360-(yAngle/2)), transform.localEulerAngles.y, 0);
            }
            if (transform.localEulerAngles.x > (yAngle/2) && transform.localEulerAngles.x < 180f)
            {
                transform.localEulerAngles = new Vector3((yAngle/2), transform.localEulerAngles.y, 0);
            }
        }
    }
}
