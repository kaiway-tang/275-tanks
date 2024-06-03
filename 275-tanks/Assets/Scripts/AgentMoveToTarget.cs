using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentMoveToTarget : Agent
{
    [SerializeField] private Transform Target;
    [SerializeField] private Renderer floorRenderer;

    public override void OnEpisodeBegin() {
        // Move the agent to a random position
        transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Move the target to a random position
        Target.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add the agent's position
        sensor.AddObservation(new Vector2(transform.localPosition.x, transform.localPosition.z));

        // Add the target's position
        sensor.AddObservation(new Vector2(Target.localPosition.x, Target.localPosition.z));
    }
    public override void OnActionReceived(ActionBuffers actions) {

        // Get the action values
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 5f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Target")) {
            SetReward(2f);
            EndEpisode();
            floorRenderer.material.color = Color.green;
        } else if (other.CompareTag("OutOfBounds")) {
            SetReward(-1f);
            EndEpisode();
            floorRenderer.material.color = Color.red;
        }
    }

        private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Target")) {
            SetReward(2f);
            EndEpisode();
            floorRenderer.material.color = Color.green;
        } else if (other.CompareTag("OutOfBounds")) {
            SetReward(-1f);
            EndEpisode();
            floorRenderer.material.color = Color.red;
        }
    }
}
