using UnityEngine;

public class Projectile : MonoBehaviour
{

    private TankAgent tankAgent;

    public void SetAgent(TankAgent agent) {
        tankAgent = agent;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Target")) {
            tankAgent.RegisterHit(); // Notify the agent of a hit
            Destroy(gameObject); // Destroy the projectile
        } else if (other.CompareTag("OutOfBounds")) {
            tankAgent.RegisterMiss(); // Notify the agent of a miss
            Destroy(gameObject); // Destroy the projectile
        }
    }
}
