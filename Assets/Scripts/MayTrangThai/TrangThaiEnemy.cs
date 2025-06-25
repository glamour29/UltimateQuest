using UnityEngine;

public class TrangThaiEnemy : TrangThaiThucThe
{
    protected Enemy enemy;

    public TrangThaiEnemy(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(mayTrangThai, TenBoolanim)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
    }

    public override void Update()
    {
        base.Update();

        anim.SetFloat("heSoTocDoDiChuyen", enemy.heSoTocDoDiChuyen);
    }

}
