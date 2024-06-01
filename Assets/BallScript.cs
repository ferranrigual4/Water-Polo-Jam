using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private float t;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float baseH;

    public float maxH;

    private Action passMissedAction;

    // Start is called before the first frame update
    void Start()
    {
        t = -1;
    }

    public void SetupThrowing(Vector3 start, Vector3 end, Action missCallback)
    {
        startPoint = start;
        endPoint = end;

        baseH = startPoint.y;
        t = 0;

        passMissedAction = missCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (t >= 0 && t <= 1)
        {
            t += Time.deltaTime;
            UpdatePassPosition();
        }
        if (t >= 1.0f)
        {
            t = -1;
            passMissedAction();
        }


    }

    private void UpdatePassPosition() {
        float xt = Mathf.Lerp(startPoint.x, endPoint.x, t);
        //        float yt = baseH + (0.5f - Mathf.Abs(0.5f - t)) * maxH * 2;
        float yt = baseH + (0.25f - Mathf.Pow(0.5f - t, 2)) * 4.0f;
        float zt = Mathf.Lerp(startPoint.z, endPoint.z, t);
        this.transform.position = new Vector3(xt, yt, zt);
    }

    public void StopPass()
    {
        t = -1;
    }

}
