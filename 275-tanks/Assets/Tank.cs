using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] Rigidbody bodyRB;
    [SerializeField] Transform bodyTrfm, turretTrfm;
    [SerializeField] protected float driveSpeed, steerSpeed, aimSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static Vector3 tempVect;
    protected void Aim(Vector3 position)
    {
        position.y = 0;

        tempVect = turretTrfm.position;
        tempVect.y = 0;

        turretTrfm.forward = position - tempVect;
    }

    protected void Drive(float speed)
    {
        bodyRB.AddForce(transform.forward * speed);
    }

    protected void Steer(float speed)
    {
        bodyTrfm.Rotate(Vector3.up * speed);
    }
}
