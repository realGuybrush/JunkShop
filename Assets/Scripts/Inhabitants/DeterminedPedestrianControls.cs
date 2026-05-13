using UnityEngine;

public class DeterminedPedestrianControls : PedestrianControls
{
    [SerializeField]
    private Vector3 goal;

    protected override void Awaking()
    {
        base.Awaking();
        goal = WorldManager.Instance.RandomDoor(transform.position);
    }

    protected override void DefaultWalkingPattern()
    {
        if ((goal - transform.position).magnitude < 0.1f)
        {
            Destroy(body.gameObject);
            return;
        }
        if (walkingTime > 0) return;
        body.linearVelocity = (goal - transform.position).normalized * speed;
        walkingTime = 1f;
        animator.SetBool(moveHash, true);
        Flip();
    }
}
