using UnityEngine;

public class Enemy_AnimationTriggers : ThucThe_AnimationTrigger
{
    private Enemy enemy;
    private Enemy_VFX enemyVfx;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVfx = GetComponentInParent<Enemy_VFX>(); 

    }

    private void BatPhanDon()
    {
        enemyVfx.BatCanhBaoTanCong(true);
        enemy.BatPhanDon(true);
    }
     
    private void TatPhanDon()
    {
        enemyVfx.BatCanhBaoTanCong(false);
        enemy.BatPhanDon(false);
    }
}
