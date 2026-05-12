using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private AudioSource bonk, oof;

    [SerializeField]
    private PlayerInput playerInputActions;
    private InputAction move, act;

    [SerializeField]
    private Animator animator, armAnimator;

    [SerializeField]
    private GameObject images;
    
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
        Flip();
        animator.SetBool("Move", body.linearVelocity.magnitude > 0);
    }

    private void Flip()
    {
        if (body.linearVelocity.x > 0)
            images.transform.eulerAngles = Vector3.zero;
        if (body.linearVelocity.x < 0)
            images.transform.eulerAngles = new Vector3(0, 180, 0);
    }

    private void HandleAction(InputAction.CallbackContext callbackContext)
    {
        StartCoroutine("PickUpAll");
    }

    private IEnumerator PickUpAll()
    {
        picker.SetActive(true);
        armAnimator.SetBool("Poke", true);
        yield return new WaitForSeconds(0.2f);
        armAnimator.SetBool("Poke", false);
        picker.SetActive(false);
    }

    public void GetDamaged(Vector3 position, float kickStrength)
    {
        body.AddForce((transform.position - position).normalized * kickStrength);
        bonk.Play();
        oof.Play();
        StartCoroutine("GetStunned");
    }

    private IEnumerator GetStunned()
    {
        DisableInputActions();
        animator.SetBool("Damaged", true);
        yield return new WaitForSeconds(stunTime);
        animator.SetBool("Damaged", false);
        EnableInputActions();
        body.linearVelocity = Vector2.zero;
    }
}
