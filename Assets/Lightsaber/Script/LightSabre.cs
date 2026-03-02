using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine.XR;

public class LightSabre : MonoBehaviour
{
    public XRNode controllerNode = XRNode.LeftHand; // Or LeftHand
    private bool isActive = false; // Flag to check if the light saber is active
    private GameObject laser;
    private Vector3 fullSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool lastGripState = false;

    private AudioSource source;
    public AudioClip sabreMoving; // Assign this in the inspector with your sound effect
    public AudioClip sabreOn; // Assign this in the inspector with your sound effect
    public AudioClip sabreHum; // Assign this in the inspector with your sound effect

    InputDevice device;
    private List<InputDevice> inputDevices = new List<InputDevice>();
    bool gripPressed = false;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 1.0f; // Set spatial blend to 3D sound
        source.volume = 0.5f; // Set volume to a reasonable level
        laser = transform.Find("SingleLine-LightSaber").gameObject;
        fullSize = laser.transform.localScale;
        laser.transform.localScale = new Vector3(fullSize.x, 0, fullSize.z);
        //Keep laser pulled in until the player presses the button
        // device = InputDevices.GetDeviceAtXRNode(controllerNode);
        // print(device.name);
    }

    // Update is called once per frame
    void Update()
    {
        inputDevices.Clear();

        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.Left , inputDevices);
        
        foreach (var device in inputDevices)
        {
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
            
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out gripPressed) && gripPressed)
            {
                Debug.Log("Trigger button is pressed.");
            }
            else
            {
                gripPressed = false;
            }
        }
        GetInput(gripPressed);
        LaserControl(gripPressed);

        // device.TryGetFeatureValue(CommonUsages.gripButton, out gripPressed);

        // Detect "button down" event (implement your button state logic)
        // Laser control logic
        // ...

        // Get controller velocity
        Vector3 velocity;
        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out velocity))
        {
            if (isActive = true && velocity.magnitude > 6f && sabreMoving != null)
            {
                source.PlayOneShot(sabreMoving);
            }
            else if(source.isPlaying == false)
            {
                source.PlayOneShot(sabreHum);
            }
        }

    }

    private void GetInput(bool gripPressed)
    {
        if (gripPressed && !lastGripState)
        {
            isActive = !isActive;
            laser.SetActive(isActive);
        }
    }

    private void LaserControl(bool gripPressed)
    {
        lastGripState = gripPressed;

        // Animate laser scaling
        if (isActive && laser.transform.localScale.y < fullSize.y)
        {
            Debug.Log("laser growing");
            laser.SetActive(true);
            laser.transform.localScale += new Vector3(0, 0.0001f, 0);
        }
        else if (isActive == false && laser.transform.localScale.y > .1)
        {
            Debug.Log("laser shrinking");
            laser.transform.localScale += new Vector3(0, -0.0001f, 0);
        }
        else if (isActive == false)
        {
            Debug.Log("laser disabled");
            laser.SetActive(false);
        }
    }


}
