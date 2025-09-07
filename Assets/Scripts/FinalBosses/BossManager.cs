using Unity.VisualScripting;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    SpaceShipBossBase currentState;

    public IdleState idleState = new IdleState();
    public AttackState attackState = new AttackState();
    public DeadState deadState = new DeadState();
    public EnragedState enragedState = new EnragedState();

    public LayerMask layer;

    private void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }

    public Vector2 movedir = Vector2.right;

    private readonly Vector2[] directions = new Vector2[]
    {
        Vector2.right,Vector2.left,
    };

    private void FixedUpdate()
    {
        currentState.UpdateState(this);
        CheckForEdgeOfScreen();
    }

    void ShootRays()
    {
        //foreach (var dir in directions)
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir,1,layer);
        //    if (hit.collider != null)
        //    {
        //        ChangeDirection();
        //    }
        //}
    }

    void CheckForEdgeOfScreen()
    {
        var result = Camera.main.WorldToViewportPoint(transform.position);
        if (result.x <= 0.1f || result.x >= 0.9f)
        {
            movedir = movedir == Vector2.right ? Vector2.left : Vector2.right;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var dir in directions)
        {
            Vector2 start = transform.position;
            Vector2 end = start + (Vector2)dir * 1f;
            Gizmos.DrawLine(start, end);
        }

    }

    public void SwitchState(SpaceShipBossBase state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
