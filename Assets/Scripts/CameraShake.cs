using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1;
    public float shakeDuration = 0;
    private Vector3 defaultPos;

    void Start()
    {
        defaultPos = transform.localPosition;
        
    }

    void Update()
    {
        // We access the camera shake just by increading shake duration
        //Once we do, the camera's local position will be set to its defualt position plus a little shift in a random direction
        if (shakeDuration > 0)
        {
            transform.localPosition  = defaultPos + (Vector3)Random.insideUnitCircle * shakeAmount;
            //Shake durstion gets decreased until it reaches zero
            shakeDuration -= Time.deltaTime * decreaseFactor;


        }
        else
        {
            //Once the shake duration is zero, we return the camera to its original position
            transform.localPosition = defaultPos;
        }

        
    }
}
