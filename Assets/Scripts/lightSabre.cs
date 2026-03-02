using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class lightSabre : MonoBehaviour
{
    private InputAction shootAction;
    private InputAction swingAction;
    private Ray ray;
    private RaycastHit hit;
    private AudioSource source;
    private Animator animate;
    private BoxCollider sabreCollider;
    
    private bool swinging = false;
    private bool swingLeft = true;
    private bool shooting = false;
    private int ammo = 5;
    private bool gearing = false;
    private bool geared = true;

    private delegate void ParamsAction(params object[] arguments);
    
    public GameObject mainCamera;
    public GameObject reticle;
    public GameObject player;
    public AudioClip shootSFX;
    public AudioClip swingSFX;
    public AudioClip reloadSFX;
    public GameObject colliderObject;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(Delay(1f, Foo, new object[] {"a", 10, 20, "b"}));
        
        source = GetComponent<AudioSource>();
        animate = GetComponent<Animator>();

        // Left Click Action
        shootAction = InputSystem.actions.FindAction("Attack");

        // Right Click Action
        swingAction = InputSystem.actions.FindAction("Melee");

        sabreCollider = colliderObject.GetComponent<BoxCollider>();
        sabreCollider.enabled = false;

        // if (!geared)
        // {
        //     animate.applyRootMotion = false;
        //     animate.SetTrigger("gearUp");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast from mouse
        ray = Camera.main.ScreenPointToRay(reticle.transform.position);
        // ray = Camera.main.ScreenPointToRay(new Vector2(0,0));

        // // If hit,
        // if (Physics.Raycast(ray, out hit)) {

        //     // Get hit object's transform
        //     Transform objectHit = hit.transform;

        //     // Point Sabre at object
        //     transform.LookAt(hit.point);
        // }
        // else
        // {
        if (geared)
        {
            transform.LookAt(ray.GetPoint(10), player.transform.up);
        }
        // }
        
        // When left clicked
        if (shootAction.WasPerformedThisFrame() && !swinging && !shooting && ammo > 0 && geared && !gearing)
        {
            shooting = true;
            source.pitch = Random.Range(.95f,1.05f);
            source.volume = Random.Range(.9f,1f);
            source.PlayOneShot(shootSFX);

            // Raycast from mouse
            ray = Camera.main.ScreenPointToRay(reticle.transform.position);
            
            // If hit,
            if (Physics.Raycast(ray, out hit)) {

                // Get hit object's transform
                Transform objectHit = hit.transform;
                
                print("hit " + objectHit.name + " at " + hit.point);

                // Hit indicator sphere
                // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                // SphereCollider sphereCollider = sphere.GetComponent<SphereCollider>();
                // Destroy(sphereCollider);

                // sphere.transform.position = hit.point;
                // Object.Destroy(sphere,1f);

                if (objectHit.tag == "Enemy")
                {
                    objectHit.GetComponent<Enemy_Health>().TakeDamage(1);
                }

                if (objectHit.tag == "Melee")
                {
                    objectHit.GetComponent<EnemyMeleeAI>().TakeDamage(1);
                }

                if (objectHit.tag == "Boss")
                {
                    objectHit.GetComponent<EnemyBossAI>().TakeDamage(1);
                }

                //if (objectHit.tag == "Boss")
                //{
                //    objectHit.GetComponent<EnemyBossAI>().TakeDamage(1);
                //}
            }
            
            ammo -= 1;

            if (ammo <= 0)
            {
                print("reloading...");
                source.PlayOneShot(reloadSFX);
                StartCoroutine(Delay(1f, Reload));
            }

            StartCoroutine(Delay(.25f, () =>
            {
                shooting = false;
            }));
        }
        
        // When right clicked
        if (swingAction.WasPerformedThisFrame() && !swinging && geared && !gearing)
        {
            animate.applyRootMotion = false;
            swinging = true;
            if (swingLeft == true)
            {
                animate.SetTrigger("swingLeft");
            }
            else
            {
                animate.SetTrigger("swingRight");
            }
            source.pitch = Random.Range(.95f,1.05f);
            source.volume = Random.Range(.9f,1f);
            source.clip = swingSFX;
            // StartCoroutine(Delay(.1f, () =>
            // {
            //     source.PlayOneShot(swingSFX);
            // }));
            // StartCoroutine(Delay(.25f, ToggleCollider));
            swingLeft = !swingLeft;
        }
        
        // if (Input.GetKeyDown("space") && !swinging && !gearing)
        // {
        //     animate.applyRootMotion = false;
        //     gearing = true;
        //     if (geared)
        //     {
        //         animate.SetTrigger("gearDown");
        //     }
        //     else
        //     {
        //         animate.SetTrigger("gearUp");
        //     }
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (swinging && other.tag == "Enemy")
        {
            other.GetComponent<Enemy_Health>().TakeDamage(5);
        }
        else if (swinging && other.tag == "Melee")
        {
            other.GetComponent<EnemyMeleeAI>().TakeDamage(5);
        }
        else if (swinging && other.tag == "Boss")
        {
            other.GetComponent<EnemyBossAI>().TakeDamage(5);
        }
    }
    
    IEnumerator Delay(float delay, System.Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
    
    IEnumerator Delay(float delay, ParamsAction callback, object[] args)
    {
        yield return new WaitForSeconds(delay);
        callback(args);
    }

    private void doneSwing()
    {
        animate.applyRootMotion = true;
        swinging = false;
        sabreCollider.enabled = false;
    }

    private void gearToggle()
    {
        if (!geared)
        {
            animate.applyRootMotion = true;
        }
        geared = !geared;
        gearing = false;
    }

    private void ToggleCollider()
    {
        sabreCollider.enabled = true;
    }

    private void Reload()
    {
        print("reloaded");
        ammo = 5;
    }

    private void ColliderOn()
    {
        source.PlayOneShot(swingSFX);
        sabreCollider.enabled = true;
    }

    public void Foo(object[] args)
    {
        string a = (string) args[0];
        object b = args[1];
        object c = args[2];
        object d = args[3];

        print(c);
    }
}
