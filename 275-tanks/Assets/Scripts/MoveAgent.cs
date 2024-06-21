using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

// goal of the move agent will be to keep a distance X away from the target and try to avoid being shot
public class MoveAgent : Agent
{
    [SerializeField] private Transform agent;
    [SerializeField] private Transform Target;
    [SerializeField] private ShootAgent TargetAgent;
    [SerializeField] private Enemy enemy;
    [SerializeField] private float fightDistance = 4f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private Renderer floorRenderer;

    private Rigidbody rb;

    GameObject trackedBullet;

    public override void Initialize()
    {
        rb = agent.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Move the agent to a random position
        agent.localPosition = new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));
        rb.velocity = Vector3.zero;
        agent.rotation = Quaternion.identity;
        rb.angularVelocity = Vector3.zero;

        // Move the target to a random position
        Target.localPosition = new Vector3(Random.Range(-15f, 15f), 0, Random.Range(-15f, 15f));

        // Make sure target is not too close to the agent
        while (Vector3.Distance(agent.localPosition, Target.localPosition) < 8f)
        {
            Target.localPosition = new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(-15f, 15f));
        }

        //StartCoroutine(EndPractice());
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add the agent's position
        sensor.AddObservation(new Vector2(agent.localPosition.x, agent.localPosition.z) / 2f);
        // Add the target's position
        sensor.AddObservation(new Vector2(Target.localPosition.x, Target.localPosition.z) / 2f);

        // Add the agent's velocity
        //sensor.AddObservation(new Vector2(rb.velocity.x, rb.velocity.z));

        // Add the agent's rotation
        sensor.AddObservation(agent.transform.forward);

        // get position of target's bullet
        //Projectile targetProjectile = TargetAgent.GetProjectile();
        if (trackedBullet != null)
        {
            sensor.AddObservation(new Vector2(trackedBullet.transform.localPosition.x, trackedBullet.transform.localPosition.z) / 2f);
            sensor.AddObservation(new Vector2(trackedBullet.GetComponent<Rigidbody>().velocity.x, trackedBullet.GetComponent<Rigidbody>().velocity.z) / 2f);
        }
        else
        {
            sensor.AddObservation(new Vector2(0, 0));
            sensor.AddObservation(new Vector2(0, 0));
        }

    }

    public void SetTrackedBullet(GameObject newBullet)
    {
        trackedBullet = newBullet;
        //Debug.Log("tracking bullet");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the action values
        float rotate = actions.ContinuousActions[0];
        float move = actions.ContinuousActions[1];

        // Apply movement
        // tank can only move forwards and backwards, or rotate left and right
        rb.velocity = agent.forward * move * moveSpeed;
        //Debug.Log(rb.velocity);
        agent.Rotate(new Vector3(0, rotate * rotateSpeed * Time.deltaTime, 0));
        //Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        //Debug.Log("Running");
        //agent.localPosition += move;

        // Calculate distance to target
        float distanceToTarget = Vector3.Distance(agent.localPosition, Target.localPosition);

        // Reward for maintaining a specific distance from the target
        float distanceDifference = Mathf.Abs(distanceToTarget - fightDistance);
        AddReward(1.0f / (distanceDifference + 1.0f) * 0.02f); // closer to fightDistance gives higher reward

        // Penalty for being too close or too far from the target
        if (distanceToTarget < fightDistance / 1.5 || distanceToTarget > fightDistance * 1.5)
        {
            AddReward(-0.01f);
        }

        // reward for moving and rotating
        if(Mathf.Abs(move) > 0.8f)
        {
            AddReward(0.2f);
        }

        // Check for bullet dodging
        //Projectile targetProjectile = TargetAgent.GetProjectile();
        if (trackedBullet != null)
        {
            float distanceToBullet = Vector3.Distance(agent.localPosition, trackedBullet.transform.position);
            if (distanceToBullet < 0.5f) // Arbitrary close distance, adjust as necessary
            {
                //AddReward(-5.0f); // Penalty for being hit or close to the bullet
            }
        }

        if(agent.localPosition.y < -3) {
            AddReward(-2.0f);
            EndEpisode();
        }
    }

    public void HitEnemy()
    {
        AddReward(10f);
        EndEpisode();
        floorRenderer.material.color = Color.blue;
    }

    public void GotHit()
    {
        AddReward(-10f);
        EndEpisode();
        floorRenderer.material.color = Color.red;
    }

    private IEnumerator EndPractice() {
        yield return new WaitForSeconds(20f);
        EndEpisode();
    }
}
