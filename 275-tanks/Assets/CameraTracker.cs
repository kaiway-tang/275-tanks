using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    [SerializeField] Transform trackedTrfm;
    [SerializeField] float rotationLerp, translateLerp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, trackedTrfm.rotation, rotationLerp * Time.deltaTime);
        transform.position += (trackedTrfm.position - transform.position) * translateLerp;
    }
}
