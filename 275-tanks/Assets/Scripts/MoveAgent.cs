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
    [SerializeField] private float fightDistance = 4f;
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody rb;

    GameObject trackedBullet;

    public override void Initialize()
    {
        rb = agent.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        // Move the agent to a random position
        agent.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
        rb.velocity = Vector3.zero;
        agent.rotation = Quaternion.identity;

        // Move the target to a random position
        Target.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));

        StartCoroutine(EndPractice());
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
        sensor.AddObservation(new Vector2(agent.localPosition.x, agent.localPosition.z));

        // Add the target's position
        sensor.AddObservation(new Vector2(Target.localPosition.x, Target.localPosition.z));

        // Add the agent's velocity
        sensor.AddObservation(new Vector2(rb.velocity.x, rb.velocity.z));

        // get position of target's bullet
        Projectile targetProjectile = TargetAgent.GetProjectile();
        if (targetProjectile != null)
        {
            sensor.AddObservation(new Vector2(targetProjectile.transform.localPosition.x, targetProjectile.transform.localPosition.z));
            sensor.AddObservation(new Vector2(targetProjectile.GetComponent<Rigidbody>().velocity.x, targetProjectile.GetComponent<Rigidbody>().velocity.z));
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
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the action values
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        // Apply movement
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        Debug.Log("Running");
        agent.localPosition += move;

        // Calculate distance to target
        float distanceToTarget = Vector3.Distance(agent.localPosition, Target.localPosition);

        // Reward for maintaining a specific distance from the target
        float distanceDifference = Mathf.Abs(distanceToTarget - fightDistance);
        AddReward(1.0f / (distanceDifference + 1.0f) * 0.01f); // closer to fightDistance gives higher reward

        // Penalty for being too close or too far from the target
        if (distanceToTarget < fightDistance / 1.5 || distanceToTarget > fightDistance * 1.5)
        {
            AddReward(-0.1f);
        }

        // Check for bullet dodging
        //Projectile targetProjectile = TargetAgent.GetProjectile();
        if (trackedBullet != null)
        {
            float distanceToBullet = Vector3.Distance(agent.localPosition, trackedBullet.transform.position);
            if (distanceToBullet < 0.5f) // Arbitrary close distance, adjust as necessary
            {
                AddReward(-5.0f); // Penalty for being hit or close to the bullet
            }
        }

        if(agent.localPosition.y < -3) {
            AddReward(-2.0f);
            EndEpisode();
        }
    }

    private IEnumerator EndPractice() {
        yield return new WaitForSeconds(20f);
        EndEpisode();
    }
}
