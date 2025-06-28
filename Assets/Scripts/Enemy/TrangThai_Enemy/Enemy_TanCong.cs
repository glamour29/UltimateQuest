using UnityEngine;

public class Enemy_TanCong : TrangThaiEnemy
{
    public Enemy_TanCong(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(enemy, mayTrangThai, TenBoolanim)
    {
    }

    public override void Update()
    {
        base.Update();

        if (triggerDuocGoi)
            mayTrangThai.thayDoiTrangThai(enemy.DanhNhau);
    }
}
