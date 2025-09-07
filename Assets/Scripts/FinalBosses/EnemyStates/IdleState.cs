using UnityEngine;

public class IdleState : SpaceShipBossBase
{
    public override void EnterState(BossManager boss)
    {
    }

    public override void ExitState(BossManager boss)
    {
    }

    public override void UpdateState(BossManager boss)
    {
        boss.transform.Translate(boss.movedir*1*Time.deltaTime);
    }
}
