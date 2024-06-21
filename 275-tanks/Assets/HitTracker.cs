using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTracker : MonoBehaviour
{
    [SerializeField] MoveAgent ma;

    Transform startingTransform;
    // Start is called before the first frame update
    void Start()
    {
        //startingTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BlueBullet"))
        {
            // got hit by agent
            ma.HitEnemy();
            //transform.position = startingTransform.position;
        }
    }
}
