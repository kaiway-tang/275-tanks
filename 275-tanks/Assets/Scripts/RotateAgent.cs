using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RotateAgent : Agent
{
    //[SerializeField]


    [SerializeField] Vector3 distToTarget;
    [SerializeField] float angleDiff;

// ----------------------

    [SerializeField] private Transform agent;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Renderer floorRenderer;

    public override void OnEpisodeBegin() {
        // Reset the tank's rotation and the target's position
        agent.rotation = Quaternion.identity;
        
        // Move the agent to a random position
        // agent.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));

        // Move the target to a random position
        // target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));

        // Make sure target is not too close to the agent
        // while (Vector3.Distance(agent.localPosition, target.localPosition) < 2f) {
        //     target.localPosition = new Vector3(Random.Range(-4f, 4f), 0f, Random.Range(-4f, 4f));
        // }

        // StartCoroutine(EndPractice());

    }

    public override void CollectObservations(VectorSensor sensor) {
        // Add the tank's barrel rotation and target's position as observations
        sensor.AddObservation(barrelTransform.right);
        Vector3 targetLocal = new Vector3(target.localPosition.x, 0, target.localPosition.z);
        Vector3 transformLocal = new Vector3(agent.localPosition.x, 0, agent.localPosition.z);
        sensor.AddObservation((targetLocal - transformLocal).normalized);
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

        // Calculate the reward based on the alignment of the barrel with the target

        // Calculate the angle between the barrel's forward direction and the direction to the target
        Vector3 targetLocal = new Vector3(target.localPosition.x, 0, target.localPosition.z);
        Vector3 transformLocal = new Vector3(agent.localPosition.x, 0, agent.localPosition.z);
        distToTarget = (targetLocal - transformLocal).normalized;
        angleDiff = Vector3.Angle(barrelTransform.right, distToTarget);
        SetReward(-angleDiff / 180f * 0.1f); // Penalize based on how off the angle is, scaled to [-1, 0]

        // Give a small positive reward for good alignment
        if (angleDiff < 10f) {
            SetReward(0.1f);
            floorRenderer.material.color = Color.green;
        } else {
            floorRenderer.material.color = Color.white;
        }
    }

    private IEnumerator EndPractice() {
        yield return new WaitForSeconds(5f);
        EndEpisode();
    }
}
