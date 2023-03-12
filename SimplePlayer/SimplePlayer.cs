using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{

    public float go = 5, run = 10, jump = 5, gravity = 4, rotYspeed = 2, rotXspeed = 0.7f;
    public bool ground;
    public float min = -.5f, max = .5f;
    [Space(5)]
    public bool isCrouch;
    public float crouch = .5f;
    public float crouchMoveSpeed = 3;
    public float crouchLerpSpeed = 5;
    float dt, normalhight;
    public Transform cam;
    Rigidbody ri;

    private void Start()
    {
        ri = GetComponent<Rigidbody>();
        setCursorLock(true);
        normalhight = transform.localScale.y;
    }

    private void Update()
    {
       
    }


    private void FixedUpdate()
    {
        ri.velocity = Vector3.zero;
        Vector3 dir = Vector3.zero;
        bool running = false;

        if (Input.GetKey(KeyCode.LeftShift) && ! isCrouch) running = true;

        if (Input.GetKey(KeyCode.W)) dir.z = (!running ? 1 * (!isCrouch ? go : crouchMoveSpeed) * 100 : 1 * run * 100);
        else if (Input.GetKey(KeyCode.S)) dir.z = -1 * (!isCrouch ? go : crouchMoveSpeed) * 100;

        if (Input.GetKey(KeyCode.D)) dir.x = (!running ? 1 * (!isCrouch ? go : crouchMoveSpeed) * 100 : 1 * run * 100);
        else if (Input.GetKey(KeyCode.A)) dir.x = (!running ? -1 * (!isCrouch ? go : crouchMoveSpeed) * 100 : -1 * run * 100);


        float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;

        ground = Physics.Raycast(transform.position, Vector3.down, DisstanceToTheGround + 0.1f);

        if (ground)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isCrouch) ri.velocity += Vector3.up * -Physics.gravity.y * (jump * 100) * Time.deltaTime;
        }
        
        ri.AddRelativeForce(dir * Time.fixedDeltaTime, ForceMode.Impulse);

        if (!ground)
        {
            ri.velocity += Vector3.up * Physics.gravity.y * (gravity * 10) * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(duck(false));
        else if (Input.GetKeyUp(KeyCode.LeftControl)) StartCoroutine(duck(true));

    }

    IEnumerator duck(bool i)
    {
        float t = 0;

        if (!i)
        {
            var l = transform.localScale;
            var p = transform.position;

            dt = l.y - crouch;

            l.y = crouch;

            while (transform.localScale.y != crouch)
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime * crouchLerpSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, l, t);
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, p.y - dt, transform.position.z), t);
            }
            isCrouch = true;
        }
        else
        {
            var l = transform.localScale;
            var p = transform.position;

            l.y = normalhight;

            while (transform.localScale.y != normalhight)
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime * crouchLerpSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, l, t);
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, p.y + dt, transform.position.z), t);
            }
            isCrouch = false;
        }
    }

    private void LateUpdate()
    {
        transform.rotation *= Quaternion.Euler(0, (rotYspeed * 100) * Input.GetAxis("Mouse X") * Time.deltaTime, 0);


        var r = cam.transform.localRotation;

        r.x += rotXspeed * -Input.GetAxis("Mouse Y") * Time.deltaTime;
        r.x = Mathf.Clamp(r.x, min, max);
        r.y = 0;
        r.z = 0;

        cam.transform.localRotation = r;

    }

    public static void setCursorLock(bool i)
    {
        if (i)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

}
