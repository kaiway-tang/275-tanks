using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public static Bullet[] bullets;
    static int nextBulletsIndex;
    [SerializeField] float velocity;
    static bool once;

    [SerializeReference] Vector3 myForward;

    // Start is called before the first frame update
    void Start()
    {
        if (!once)
        {
            bullets = new Bullet[20];
            once = true;
        }

        bullets[nextBulletsIndex] = this;
        nextBulletsIndex++;
        nextBulletsIndex = nextBulletsIndex % bullets.Length;

        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myForward = transform.forward;
        transform.position += transform.up * velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {

        }
        else
        {
            if (other.gameObject.layer == 6)
            {
                other.GetComponent<Tank>().Hit();
            }

            Destroy(gameObject);
        }        
    }
}
