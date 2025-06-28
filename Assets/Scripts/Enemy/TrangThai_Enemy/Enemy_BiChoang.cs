using UnityEngine;

public class Enemy_BiChoang : TrangThaiEnemy
{
    private Enemy_VFX vfx;

    public Enemy_BiChoang(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(enemy, mayTrangThai, TenBoolanim)
    {
        vfx= enemy.GetComponent<Enemy_VFX>();
    }

    public override void Enter()
    {
        base.Enter();

        vfx.BatCanhBaoTanCong(false);
        enemy.BatPhanDon(false);

        tgianTrangThai = enemy.tgianBiChoang;// Đặt thời gian choáng
        rb.linearVelocity = new Vector2 (
            enemy.vanTocBiChoang.x * -enemy.huongQuay, // Đẩy ngược hướng quay
            enemy.vanTocBiChoang.y);// Thêm lực bật lên

    }

    public override void Update()
    {
        base.Update();

        if (tgianTrangThai < 0)
            mayTrangThai.thayDoiTrangThai(enemy.DungYen);
    }
}
