using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    [SerializeField] Vector3 mousePos;
    [SerializeField] Camera cam;
    [SerializeField] Transform crosshair;

    [SerializeField] Transform predictedPositionIndicator;

    public static Transform trfm;
    public static Player self;
    public PositionTracker tracker;

    // Start is called before the first frame update
    void Awake()
    {
        self = GetComponent<Player>();        
        trfm = transform;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private new void FixedUpdate()
    {
        base.FixedUpdate();
        

        Aim(crosshair.position);

        if (Input.GetKey(KeyCode.W))
        {
            Drive(driveSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Drive(-driveSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.D))
            {
                Steer(-steerSpeed);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Steer(steerSpeed);
        }
    }

    public static Vector3 PredictedPosition(float time)
    {
        return self.tracker.PredictedPosition(time);
    }
}
