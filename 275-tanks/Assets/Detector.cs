using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] Enemy enemyScript;
    [SerializeField] Transform bodyTrfm;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per framez
    void Update()
    {
        
    }

    Transform temp;
    private void OnTriggerStay(Collider other)
    {
        temp = other.transform;
        if (ValidateBullet(temp) && Vector3.SqrMagnitude(enemyScript.transform.position - temp.position) < enemyScript.MinBulletDistance())
        {
            enemyScript.closestBullet = temp;
        }
    }

    bool ValidateBullet(Transform bullet)
    {
        return Vector3.Dot(bullet.up, (bodyTrfm.position - bullet.position)) > 0;
    }
}
