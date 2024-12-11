using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Move2 : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer planeMeshRederer;
    [SerializeField] private float speed = 20f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(7, 3f, 0);
    }

    private Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
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
        float moveforward = actions.ContinuousActions[0];
        float moveRotate = actions.ContinuousActions[1];


        rb.MovePosition(transform.position + transform.forward * moveRotate * speed * Time.deltaTime);
        transform.Rotate(0f, moveforward * speed, 0f, Space.Self);

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

        if (other.TryGetComponent<Hant>(out Hant hant))
        {
            SetReward(-2f);
            planeMeshRederer.material = winMaterial;
            EndEpisode();
        }
    }
}