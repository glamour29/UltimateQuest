using UnityEngine;

public class Enemy_Chet : TrangThaiEnemy

{
    private Collider2D col;

    public Enemy_Chet(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(enemy, mayTrangThai, TenBoolanim)
    {
        col = enemy.GetComponent<Collider2D>();

    }

    public override void Enter()
    {
        anim.enabled = false;
        col.enabled = false ;
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        mayTrangThai.TatMayTrangThai();
    }
}
