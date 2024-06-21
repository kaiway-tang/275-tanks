using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class ShootAgent : Agent
{
    [SerializeField] private Transform agent;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Renderer floorRenderer;
    private float lastShotTime;
    private bool canShoot = true;

    private bool projectileExists = false;
    private Projectile projectile;

    public override void OnEpisodeBegin() {
        // Reset the target's position
        // Move the agent to a random position
        agent.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));

        // Move the target to a random position
        target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));

        // Make sure target is not too close to the agent
        while (Vector3.Distance(agent.localPosition, target.localPosition) < 2f) {
            target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
        }

        lastShotTime = Time.time;
        canShoot = true;

        //StartCoroutine(EndPractice());
    }

    public override void CollectObservations(VectorSensor sensor) {
        // Add the relative position of the target as observations
        Vector3 targetLocal = new Vector3(target.localPosition.x, 0, target.localPosition.z);
        Vector3 transformLocal = new Vector3(agent.localPosition.x, 0, agent.localPosition.z);
        sensor.AddObservation(Vector3.Dot(barrelTransform.right, (targetLocal - transformLocal).normalized));
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    public override void OnActionReceived(ActionBuffers actions) {
        // Handle shooting action
        int shootAction = actions.DiscreteActions[0];
        if (shootAction == 1 && Time.time - lastShotTime > 2f && canShoot)
        {
            Shoot();
            lastShotTime = Time.time;
            canShoot = false;
            StartCoroutine(ResetShootCooldown());
        }

        //Debug.Log(GetCumulativeReward());

        if(!projectileExists) {
            // LosePoints();
        }
    }

    private void Shoot() {
        // Instantiate and shoot the projectile
        GameObject projectileGO = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
        rb.velocity = projectileSpawnPoint.right * 15f; // Adjust the speed as needed
        projectileExists = true;

        projectile = projectileGO.GetComponent<Projectile>();

        // Set the agent as the projectile's agent
        projectile.SetAgent(this);

        // Destroy the projectile after a few seconds
        Destroy(projectileGO, 4f);
    }

    public Projectile GetProjectile() {
        return projectile;
    }

    public void projectileDestroyed() {
        projectileExists = false;
    }

    private IEnumerator ResetShootCooldown() {
        yield return new WaitForSeconds(2f);
        canShoot = true;
    }

    private IEnumerator EndPractice() {
        yield return new WaitForSeconds(10f);
        EndEpisode();
    }

    public void RegisterHit() {
        SetReward(1f);
        //floorRenderer.material.color = Color.green;
        MoveTarget();
    }

    public void RegisterMiss() {
        SetReward(-2f);
        //floorRenderer.material.color = Color.red;
    }

    public void LosePoints() {
        SetReward(-0.01f);
    }

    private void MoveTarget() {
        // Move the target to a random position
        target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));

        // Make sure target is not too close to the agent
        while (Vector3.Distance(agent.localPosition, target.localPosition) < 2f) {
            target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
        }
    }
}
