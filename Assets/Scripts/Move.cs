using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]
public class Move : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private float speed = 10f;
    public int Damage = 20;
    public int k = 0;

    private MazeConstructor generator;

    public override void OnEpisodeBegin()
    {
        generator = GetComponent<MazeConstructor>();
        if (k == 0)
        {
            k = 1;
            generator.DisposeOldMaze();
            generator.GenerateNewMaze(11, 13);
        }
        
        targetTransform.localPosition = new Vector3(40f, 1.5f, 33f);
        transform.localPosition = new Vector3(3f, 1.5f, 3f);
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
        float moveforward = actions.ContinuousActions[0];
        float moveRotate = actions.ContinuousActions[1];

        rb.MovePosition(transform.position + transform.forward * moveRotate * speed * 0.5f * Time.deltaTime);
        transform.Rotate(0f, moveforward * speed, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            SetReward(-1f);

        }

        if (other.TryGetComponent<Player>(out Player player))
        {
            SetReward(+2f);
            player.TakeDamage(Damage);

        }
    }
}
