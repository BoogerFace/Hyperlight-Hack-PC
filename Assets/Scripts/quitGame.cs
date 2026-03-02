using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class quitGame : MonoBehaviour
{
    private InputAction quitAction;

    // Start is called before the first frame update
    void Start()
    {
        quitAction = InputSystem.actions.FindAction("Quit");
    }

    // Update is called once per frame
    void Update()
    {
        if (quitAction.WasPerformedThisFrame())
        {
            Application.Quit();
        }
    }
}
