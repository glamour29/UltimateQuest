using UnityEngine;

public class TrangThaiEnemy : TrangThaiThucThe
{
    protected Enemy enemy;

    public TrangThaiEnemy(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(mayTrangThai, TenBoolanim)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
        chiSo = enemy.chiSo;
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.F))
            mayTrangThai.thayDoiTrangThai(enemy.TanCong);

        float heSoTocDoDanhNhau = enemy.tocDoDanhNhau / enemy.tocDoDiChuyen;

        anim.SetFloat("heSoTocDoDanhNhau", heSoTocDoDanhNhau);
        anim.SetFloat("heSoTocDoDiChuyen", enemy.heSoTocDoDiChuyen);
        anim.SetFloat("vtX", rb.linearVelocity.x);
    }

}
