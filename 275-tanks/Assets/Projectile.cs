using UnityEngine;

public class Projectile : MonoBehaviour
{

    private ShootAgent tankAgent;

    public void SetAgent(ShootAgent agent) {
        tankAgent = agent;
    }

    private void OnTriggerEnter(Collider other) {
        tankAgent.projectileDestroyed(); // Notify the agent that the projectile was destroyed
        if (other.CompareTag("Target")) {
            tankAgent.RegisterHit(); // Notify the agent of a hit
            Destroy(gameObject); // Destroy the projectile
        } else if (other.CompareTag("OutOfBounds")) {
            tankAgent.RegisterMiss(); // Notify the agent of a miss
            Destroy(gameObject); // Destroy the projectile
        }
    }
}
