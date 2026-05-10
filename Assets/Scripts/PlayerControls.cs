using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [SerializeField]
    private PlayerInput playerInputActions;
    private InputAction move, act;
    
    [SerializeField]
    private Rigidbody2D body;

    [SerializeField]
    private float speed, stunTime;

    [SerializeField]
    private GameObject picker;
 
    void Awake()
    {
        SetInputActions();
        EnableInputActions();
        SetEvents();
    }

    private void OnDestroy()
    {
        DisableInputActions();
        RemoveEvents();
    }

    private void SetInputActions()
    {
        act = playerInputActions.actions.FindAction("Attack");
        move = playerInputActions.actions.FindAction("Move");
    }

    private void EnableInputActions()
    {
        act?.Enable();
        move?.Enable();
    }

    private void SetEvents()
    {
        move.performed += HandleMovement;
        move.canceled += HandleMovement;
        act.started += HandleAction;
    }

    private void DisableInputActions()
    {
        act?.Disable();
        move?.Disable();
    }

    private void RemoveEvents()
    {
        move.performed -= HandleMovement;
        move.canceled -= HandleMovement;
        act.started -= HandleAction;
    }

    private void HandleMovement(InputAction.CallbackContext callbackContext)
    {
        body.linearVelocity = move.ReadValue<Vector2>() * speed;
    }

    private void HandleAction(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine("PickUpAll");
    }

    private IEnumerator PickUpAll()
    {
        picker.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        picker.SetActive(false);
    }

    public void GetDamaged(Vector3 position, float kickStrength)
    {
        body.AddForce((transform.position - position).normalized * kickStrength);
        StartCoroutine("GetStunned");
    }

    private IEnumerator GetStunned()
    {
        DisableInputActions();
        yield return new WaitForSeconds(stunTime);
        EnableInputActions();
        body.linearVelocity = Vector2.zero;
    }
}
