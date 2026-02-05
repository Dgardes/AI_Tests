using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector2 direction;
    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
    }

    void Start()
    {
        
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += ctx => direction = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => direction = Vector2.zero;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void FixedUpdate()
    {
        transform.Translate(direction * 0.10f);
    }
}
