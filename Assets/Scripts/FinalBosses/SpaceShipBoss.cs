using UnityEngine;

public abstract class SpaceShipBossBase
{
    public abstract void EnterState(BossManager boss);
    public abstract void UpdateState(BossManager boss);
    public abstract void ExitState(BossManager boss);
}
