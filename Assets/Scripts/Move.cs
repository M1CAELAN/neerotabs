using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Move : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer planeMeshRederer;
    [SerializeField] private float speed = 20f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-18, 18), 3f, Random.Range(-18, 18));
        targetTransform.localPosition = new Vector3(Random.Range(-18,18), 3f, Random.Range(-18, 18));
    }

    private Rigidbody rb;

    public override void Initialize()
    {
        rb  = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> ContinuousActions = actionsOut.ContinuousActions;

        ContinuousActions[0] = Input.GetAxisRaw("Horizontal");
        ContinuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

       
        rb.MovePosition(transform.position + transform.forward * moveZ * speed * Time.deltaTime);
        transform.Rotate(0f, moveX * speed, 0f, Space.Self);
      
        //transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);
            planeMeshRederer.material = loseMaterial;
            EndEpisode();
        }

        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            SetReward(-1f);
            planeMeshRederer.material = loseMaterial;
            EndEpisode();
        }

        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);
            planeMeshRederer.material = winMaterial;
            EndEpisode();
        }
    }
}
