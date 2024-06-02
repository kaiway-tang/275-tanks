using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    [SerializeField] Vector3 mousePos;
    [SerializeField] Camera cam;
    [SerializeField] Transform crosshair;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
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
}
