using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine.Events;
using Unity.MLAgents.Sensors;

enum ActionType {
    None,
    MoveLeft,
    MoveRight,
    MoveBack,
    MoveForward,
    RotateRight,
    RotateLeft,
}

public class AgentPusher : Agent
{
    public UnityEvent WallCollided;

    float m_LateralSpeed = 0.1f;
    float m_ForwardSpeed = 0.75f;

    [HideInInspector]
    public Rigidbody agentRb;

    public override void Initialize()
    {
        agentRb = GetComponent<Rigidbody>();
        agentRb.maxAngularVelocity = 500;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //sensor.AddObservation(transform.localPosition);
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        ActionType action = (ActionType)act[0];

        switch (action)
        {
            case ActionType.MoveLeft:
                dirToGo = transform.right * -m_LateralSpeed;
                break;
            case ActionType.MoveRight:
                dirToGo = transform.right * m_LateralSpeed;
                break;
            case ActionType.MoveBack:
                dirToGo = transform.forward * -m_ForwardSpeed;
                break;
            case ActionType.MoveForward:
                dirToGo = transform.forward * m_ForwardSpeed;
                break;
            case ActionType.RotateLeft:
                rotateDir = transform.up * -1f;
                break;
            case ActionType.RotateRight:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        agentRb.AddForce(dirToGo * 3, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveAgent(actionBuffers.DiscreteActions);
        AddReward(-1f / MaxStep);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = (int)ActionType.None;
        //right
        // if (Input.GetKey(KeyCode.A))
        // {
        //     discreteActionsOut[0] = (int)ActionType.MoveLeft;
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     discreteActionsOut[0] = (int)ActionType.MoveRight;
        // }
        //forward
        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = (int)ActionType.MoveForward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = (int)ActionType.MoveBack;
        }
        //rotate
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = (int)ActionType.RotateLeft;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = (int)ActionType.RotateRight;
        }
    }

    // public override void OnEpisodeBegin()
    // {
    //     AddReward(-1f / MaxStep);
    // }

    private void OnCollisionEnter(Collision collision)
    {
        return;
        if (collision.gameObject.CompareTag("wall")) {
            // AddReward(-1f);
            WallCollided?.Invoke();
        }
    }

}
