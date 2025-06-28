using UnityEngine;

public class Enemy_DiChuyen : Enemy_DungTrenDat
{
    public Enemy_DiChuyen(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(enemy, mayTrangThai, TenBoolanim)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        if (enemy.daChamDat == false || enemy.daChamTuong)
            enemy.Lat();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.tocDoDiChuyen * enemy.huongQuay, rb.linearVelocity.y);

        if (enemy.daChamDat == false || enemy.daChamTuong)
            mayTrangThai.thayDoiTrangThai(enemy.DungYen);
    }
}
