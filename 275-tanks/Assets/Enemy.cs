using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Tank
{
    [SerializeField] bool FFA;

    [SerializeField] PositionTracker target;
    [SerializeField] float predictionRatio; //0.2 for 0.34 spd bullet; 0.03 for 0.68 spd

    int movementMode;
    const int DODGE = 0, APPROACH = 1, STRAFE = 2;

    public Transform closestBullet;

    [SerializeField] int[] steerFreq;
    [SerializeField] int steerTimer;
    int steerDirection; const int STRAIGHT = 0, LEFT = 1, RIGHT = 2;

    [SerializeReference] int minDist, maxDist;

    [SerializeField] MoveAgent moveAgent;

    [SerializeField] GameObject detectorObj;
    [SerializeField] PositionTracker tracker;

    [SerializeField] int shootDelay;
    // Start is called before the first frame update
    void Start()
    {
        if (!target && !FFA)
        {
            target = Player.self.tracker;
        }
    }

    private void OnDestroy()
    {
        Destroy(tracker);
        Destroy(detectorObj);
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        base.FixedUpdate();

        if (FFA) { HandleFFATargetSelection(); }

        HandleMovement();

        if (target) { sqrDist = Vector3.SqrMagnitude(target.trfm.position - transform.position); } else { return; }        

        HandleShooting();        
    }

    int searchCooldown = 25;
    void HandleFFATargetSelection()
    {
        if (searchCooldown > 0)
        {
            searchCooldown--;
        }
        else
        {
            target = GameManager.NearestTarget(tracker.trfm.position);
            searchCooldown = 25;
        }
    }

    void HandleMovement()
    {
        if (!HandleAvoidance())
        {
            if (!HandleSpacing())
            {
                HandleStrafing();
            }            
        }
    }


    Vector3 bulletBodyVector, orthogonalDirection;
    bool HandleAvoidance()
    {
        if (MinBulletDistance() < 400)
        {
            bulletBodyVector = transform.position - closestBullet.position;
            orthogonalDirection.x = -bulletBodyVector.z;
            orthogonalDirection.z = bulletBodyVector.x;
            orthogonalDirection.y = closestBullet.position.y;

            if (Vector3.Dot(closestBullet.up, orthogonalDirection) > 0) //left of player
            {
                if (Vector3.Dot(orthogonalDirection, transform.forward) > 0)
                {
                    Drive(-driveSpeed);
                }
                else
                {
                    Drive(driveSpeed);
                }
            }
            else
            {
                if (Vector3.Dot(orthogonalDirection, transform.forward) > 0)
                {
                    Drive(driveSpeed);
                }
                else
                {
                    Drive(-driveSpeed);
                }
            }

            if (Vector3.Dot(transform.right, closestBullet.position - transform.position) > 0) //coming from right
            {
                SteerTowards(closestBullet.right);
            }
            else
            {
                SteerTowards(-closestBullet.right);
            }

            return true;
        }
        return false;
    }

    public float MinBulletDistance()
    {
        if (closestBullet && ValidateBullet())
        {
            return Vector3.SqrMagnitude(closestBullet.position - transform.position);
        }
        else
        {
            return 999999;
        }
    }

    bool ValidateBullet()
    {
        return Vector3.Dot(closestBullet.up, (transform.position - closestBullet.position)) > 0;
    }

    float sqrDist;
    bool HandleSpacing()
    {        
        if (!target) { return false;}
        if (sqrDist < minDist * minDist)
        {
            SteerTowards(transform.position - target.trfm.position);
            Drive(driveSpeed);
            return true;
        }
        else if (sqrDist > maxDist * maxDist)
        {
            SteerTowards(target.trfm.position - transform.position);
            Drive(driveSpeed);
            return true;
        }
        return false;
    }

    void HandleStrafing()
    {
        Drive(driveSpeed);

        if (steerTimer > 0)
        {
            steerTimer--;
        }
        else
        {
            steerTimer = Random.Range(steerFreq[0], steerFreq[1]);
            steerDirection = Random.Range(0, 3);
        }

        if (steerDirection == LEFT)
        {
            Steer(-steerSpeed);
        }
        else if (steerDirection == RIGHT)
        {
            Steer(steerSpeed);
        }
    }

    bool directShot;
    Vector3 predictedPosition;
    void HandleShooting()
    {
        float predictTime = Vector3.Distance(target.trfm.position, transform.position) * predictionRatio;
        
        if (predictTime > 2) { predictTime = 2; }

        predictedPosition = target.PredictedPosition(predictTime);        
        if (directShot || Vector3.Dot(predictedPosition - transform.position, target.trfm.position - transform.position) < 0)
        {
            Aim(target.trfm.position);
        }
        else
        {
            Aim(predictedPosition);
        }
        

        if (shootDelay < 1)
        {
            GameObject bullet = Shoot();
            if (moveAgent && bullet)
            {
                moveAgent.SetTrackedBullet(bullet);
            }

            shootDelay += Random.Range(10,25);

            if (fireTimer > 150 && sqrDist > minDist * minDist && Random.Range(0,3) == 0) { shootDelay += 150; }

            directShot = Random.Range(0,3) == 0;
        }        
        if (shootDelay > 0) { shootDelay--; }
    }
}
