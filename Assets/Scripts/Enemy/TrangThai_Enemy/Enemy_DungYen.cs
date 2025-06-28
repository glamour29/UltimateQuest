using UnityEngine;

public class Enemy_DungYen : Enemy_DungTrenDat
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Enemy_DungYen(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(enemy, mayTrangThai, TenBoolanim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        tgianTrangThai = enemy.DungYenTime;

    }

    public override void Update()
    {
        base.Update();

        if (tgianTrangThai < 0)
            mayTrangThai.thayDoiTrangThai(enemy.DiChuyen);
    }
}
