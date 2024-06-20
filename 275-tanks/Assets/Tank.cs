using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] Rigidbody bodyRB;
    [SerializeField] Transform bodyTrfm, turretTrfm;
    [SerializeField] protected float driveSpeed, steerSpeed, aimSpeed;

    [SerializeField] int fireCooldown;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform firePoint;
    protected int fireTimer;

    [SerializeField] GameObject deathFX;
    [SerializeField] ParticleSystem smokePtcls;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void FixedUpdate()
    {
        if (fireTimer > 0) { fireTimer--; }
    }

    static Vector3 tempVect;
    protected void Aim(Vector3 position)
    {
        position.y = 0;

        tempVect = turretTrfm.position;
        tempVect.y = 0;

        turretTrfm.forward = position - tempVect;
    }

    protected GameObject Shoot()
    {
        if (fireTimer > 150) { return null; }
        fireTimer += fireCooldown;
                
        return Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

    protected void Drive(float speed)
    {
        bodyRB.AddForce(transform.forward * speed);
    }

    protected void Steer(float speed)
    {
        bodyTrfm.Rotate(Vector3.up * speed);
    }

    Vector3 temp;
    protected void SteerTowards(Vector3 directionVector)
    {
        temp.x = -directionVector.z;
        temp.z = directionVector.x;
        temp.y = bodyTrfm.position.y;

        directionVector.y = 0;

        if (Vector3.Dot(temp, bodyTrfm.forward) > 0)
        {
            Steer(steerSpeed);

            if (Vector3.Dot(temp, bodyTrfm.forward) < 0)
            {
                bodyTrfm.forward = directionVector;
            }
        }
        else
        {
            Steer(-steerSpeed);

            if (Vector3.Dot(temp, bodyTrfm.forward) > 0)
            {
                bodyTrfm.forward = directionVector;
            }
        }
    }

    public void Hit()
    {
        Instantiate(deathFX, transform.position, Quaternion.identity);
        smokePtcls.Play();
        Destroy(gameObject);
    }
}
