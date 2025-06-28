using UnityEngine;

public class Enemy_DungTrenDat : TrangThaiEnemy
{
    public Enemy_DungTrenDat(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(enemy, mayTrangThai, TenBoolanim)
    {
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PhatHienPlayer() == true)
            mayTrangThai.thayDoiTrangThai(enemy.DanhNhau);

    }
}
