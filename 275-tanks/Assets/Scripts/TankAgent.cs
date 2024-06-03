using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class TankAgent : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Renderer floorRenderer;
    private float lastShotTime;
    private bool canShoot = true;

    public override void OnEpisodeBegin() {
        // Reset the tank's rotation and the target's position
        transform.rotation = Quaternion.identity;
        
        // Move the agent to a random position
        transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));

        // Move the target to a random position
        target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));

        // make sure target is not too close to the agent
        while (Vector3.Distance(transform.localPosition, target.localPosition) < 2f) {
            target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
        }

        lastShotTime = Time.time;
        canShoot = true;
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Add the tank's barrel rotation and target's position as observations
        sensor.AddObservation(barrelTransform.rotation.eulerAngles.y);
        sensor.AddObservation(target.localPosition - transform.localPosition);
    }
    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
    public override void OnActionReceived(ActionBuffers actions) {
        // Get the continuous action value for rotation
        float rotate = actions.ContinuousActions[0];

        // Rotate the barrel
        barrelTransform.Rotate(0, rotate * rotationSpeed * Time.deltaTime, 0);

        int shootAction = actions.DiscreteActions[0];
        if (shootAction == 1 && Time.time - lastShotTime > 2f && canShoot)
        {
            Shoot();
            lastShotTime = Time.time;
            canShoot = false;
            StartCoroutine(ResetShootCooldown());
        }

        SetReward(-0.001f);
    }


    private void Shoot() {
        // Instantiate and shoot the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = projectileSpawnPoint.right * 10f; // Adjust the speed as needed

        // Set the agent as the projectile's agent
        projectile.GetComponent<Projectile>().SetAgent(this);

        // Destroy the projectile after a few seconds
        Destroy(projectile, 4f);
    }

    private IEnumerator ResetShootCooldown() {
        yield return new WaitForSeconds(2f);
        canShoot = true;
    }

    public void RegisterHit() {
        SetReward(2f);
        floorRenderer.material.color = Color.green;
        EndEpisode();
    }

    public void RegisterMiss() {
        SetReward(-1f);
        floorRenderer.material.color = Color.red;
        EndEpisode();
    }
}
