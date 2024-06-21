using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentHit : MonoBehaviour
{

    [SerializeField] MoveAgent ma;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HERE");
        if (other.CompareTag("RedBullet"))
        {
            // got hit
            ma.GotHit();
        }
    }
}
