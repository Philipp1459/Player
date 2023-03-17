using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BetterPlayer))]
public class HeadBober : MonoBehaviour
{
    public AnimationCurve HeadBobCurve = new AnimationCurve(new Keyframe[] {
        new Keyframe(0,0), new Keyframe(0.2082245f, 0.1f), new Keyframe(0.75f,-.1f), new Keyframe(1,0)});
    public AnimationCurve JumpBobCurve = new AnimationCurve(new Keyframe[] {
    new Keyframe(0,0), new Keyframe(0.3107313f,-.6f), new Keyframe(1,0)});

    public Transform cam;
    BetterPlayer bp;

    public float interval = 2, J_interval = 6;

    Vector3 originalPosition;

    float time;
    bool jbuff;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = cam.transform.localPosition;
        bp = GetComponent<BetterPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bp.ismoving && !bp.isjumping && !jbuff)
        {
            time += Time.deltaTime * (!bp.isrunning ? interval : interval * 2);

            var pos = cam.transform.localPosition;
            pos.y = .8f + HeadBobCurve.Evaluate(time);

            cam.transform.localPosition = pos;

            if (time > 1f) time = 0;
        }
        else
        {
            if (!jbuff)
            {
                time = 0;
                cam.transform.localPosition = originalPosition;
            }
        }

        if (bp.isjumping) jbuff = true;

        if (jbuff)
        {
            if (bp.isground)
            {
                time += Time.deltaTime * J_interval;
                var pos = cam.transform.localPosition;

                pos.y = .8f + JumpBobCurve.Evaluate(time);

                cam.transform.localPosition = pos;

                if (time > 1f)
                {
                    time = 0; 
                    cam.transform.localPosition = originalPosition;
                    jbuff = false;
                }
            }
        }
    }

}
