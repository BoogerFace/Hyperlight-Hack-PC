using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class followMouse : MonoBehaviour
{
    private InputAction lookAction;
    private InputAction shootAction;
    private Ray ray;
    private RaycastHit hit;
    private float Sensitivity = 1;
    
    public GameObject mainCamera;
    public GameObject sabre;
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sensitivity = mainCamera.GetComponent<CameraLook>().Sensitivity;
        
        // Mouse Movement Action
        lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        // Get distance mouse moved
        Vector2 lookValue = lookAction.ReadValue<Vector2>();

        // Move reticle proportionately to mouse movement
        if (transform.position.y > 0 && transform.position.y < Screen.height)
        {
            transform.position += new Vector3(0, lookValue.y * Sensitivity, 0);
            if (transform.position.y <= 0)
            {
                transform.position = new Vector3(transform.position.x, .1f, 0);
            }
            if (transform.position.y >= Screen.height)
            {
                transform.position = new Vector3(transform.position.x, Screen.height-.1f, 0);
            }
        }
        if (transform.position.x > 0 && transform.position.x < Screen.width)
        {
            transform.position += new Vector3(lookValue.x * Sensitivity, 0, 0);
            if (transform.position.x <= 0)
            {
                transform.position = new Vector3(.1f, transform.position.y, 0);
            }
            if (transform.position.x >= Screen.width)
            {
                transform.position = new Vector3(Screen.width-.1f, transform.position.y, 0);
            }
        }
    }
}
