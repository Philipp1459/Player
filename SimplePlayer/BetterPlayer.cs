using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// made by AzeS
/// 2023
/// </summary>
public class BetterPlayer : MonoBehaviour
{
    //go = walkspeed
    //run = runspeed
    //jump = jump height
    //gravity = the gravity multiplier
    //rotYspeed= rotation speed on Y axe
    //rotXspeed = rotation speed on X axe
    public float go = 5, run = 10, jump = 12, gravity = 4, rotYspeed = 2, rotXspeed = 0.7f;
    [Tooltip("Min and max rotation of camera vertical axe")]
    public float min = -.5f, max = .5f;

    [Serializable]
    public class Crouch
    {
        [Tooltip("Can player crouch")]
        public bool canCrouch;
        [Tooltip("The crouch height of player")]
        public float crouch = .5f;
        //how fast you can move when crouch
        public float crouchMoveSpeed = 3;
        //how quick you crouch
        public float crouchLerpSpeed = 5;
    }

    [Space(5)]
    public Crouch crouch;
    [Tooltip("Can double jump")]
    public bool doubleJump;

    //Camera
    public Transform cam;

    //dj = double jump buffer 
    //isCrouch = is player crouching
    bool dj, isCrouch;
    //dt = distance between normal standing and crouching
    //normalheight = the normal player height / Y scale
    //jf = jump force
    float dt, normalheight, jf;
    
    CharacterController _ch;

    private void Start()
    {
        _ch = GetComponent<CharacterController>();
        setCursorLock(true);
        normalheight = transform.localScale.y;
    }

    private void Update()
    {
        bool running = false;

        //running
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouch) running = true;

        //Input movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //moveing direction
        Vector3 dir = transform.right * x + transform.forward * z;
        dir *= (!running ? (!isCrouch ? go : crouch.crouchMoveSpeed) : run);

        if (_ch.isGrounded)
        {
            dj = false;
            //Get the jump button default is it space, you can change it in the unity editor
            if (Input.GetButtonDown("Jump") && !isCrouch)
            {
                dj = true;
                jf = jump;
            }
        }
        else
        {
            if (doubleJump && dj)
            {
                if (Input.GetButtonDown("Jump") && !isCrouch)
                {
                    dj = false;
                    jf = jump;
                }
            }

            //add gravity force
            jf += Physics.gravity.y * gravity * Time.deltaTime;
        }

        //add jump
        dir.y = jf;

        //move player
        _ch.Move(dir * Time.deltaTime);

        //crouch
        if (crouch.canCrouch)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(duck(false));
            else
            {
                //check if you can get up
                if (Physics.CheckSphere(transform.position + (Vector3.up * 1.5f), 1)) return;
                else
                {
                    //automatic or manual standing up
                    if (isCrouch && !Input.GetKey(KeyCode.LeftControl)) StartCoroutine(duck(true));
                    else if (Input.GetKeyUp(KeyCode.LeftControl)) StartCoroutine(duck(true));
                }
            }
        }
    }


    IEnumerator duck(bool i)
    {
        //t for Time
        float t = 0;
        var l = transform.localScale;
        var p = transform.position;

        //lerp player (Y scale / player height) to crouch height 
        if (!i)
        {
            dt = l.y - crouch.crouch;
            l.y = crouch.crouch;

            while (transform.localScale.y != crouch.crouch)
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime * crouch.crouchLerpSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, l, t);
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, p.y - dt, transform.position.z), t);
            }
            isCrouch = true;
        }
        else
        {//set player scale back to normal

            l.y = normalheight;

            while (transform.localScale.y != normalheight)
            {
                yield return new WaitForEndOfFrame();
                t += Time.deltaTime * crouch.crouchLerpSpeed;
                transform.localScale = Vector3.Lerp(transform.localScale, l, t);
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, p.y + dt, transform.position.z), t);
            }
            isCrouch = false;
        }
    }

    private void LateUpdate()
    {
        //rotate the player around the Y axe
        transform.rotation *= Quaternion.Euler(0, (rotYspeed * 100) * Input.GetAxis("Mouse X") * Time.deltaTime, 0);

        var r = cam.transform.localRotation;
        //rotate camera up and down
        r.x += rotXspeed * -Input.GetAxis("Mouse Y") * Time.deltaTime;
        //clamp the view field
        r.x = Mathf.Clamp(r.x, min, max);
        r.y = 0;
        r.z = 0;

        cam.transform.localRotation = r;

    }
    /// <summary>
    /// lock or delockt the cursor
    /// </summary>
    /// <param name="i"></param>
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
    /// <summary>
    /// Teleport Player to position
    /// </summary>
    /// <param name="position"></param>
    public void TP_Player(Vector3 position)
    {
        _ch.enabled = false;
        transform.position = position;
        _ch.enabled = true;
    }
}
