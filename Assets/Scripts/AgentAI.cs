using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentAI : Agent
{
    public float moveSpeed = 5f;
    public Transform target;
    private bool isTraining = true;

    private InputSystem_Actions inputActions;
    private Vector2 direction;

    private void Awake()
    {
        AITrainer.Instance.agents.Add(this);
    }

    public override void Initialize()
    {
        // Инициализируем ввод
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        // Подписываемся на события здесь, а не в OnEnable
        inputActions.Player.Move.performed += ctx => direction = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => direction = Vector2.zero;

        Debug.Log("Agent Initialized!");
    }
    private void OnDisable()
{
    if (inputActions != null)
    {
        inputActions.Player.Disable();
    }
}

    public override void OnEpisodeBegin()
    {
        isTraining = true;
        gameObject.SetActive(true);
        inputActions.Player.Enable();
        transform.localPosition = new Vector3(-7f, 0f, -0.5f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (!isTraining) return;
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, moveY, 0) * Time.deltaTime * moveSpeed;

        AddReward(-0.001f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = direction.x;
        continuousActions[1] = direction.y;

        //Debug.Log($"Heuristic Input: {direction}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Agent collided with: {other.gameObject.name}");
        if (other.CompareTag("Aim"))
        {
            AddReward(2.0f);
            isTraining = false;
            gameObject.SetActive(false);
        }
        else if (other.CompareTag("Wall"))
        {
            SetReward(-1.0f);
            isTraining = false;
            gameObject.SetActive(false);
        }
    }

}
