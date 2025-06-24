using UnityEngine;

public class Player_NhayTuong : TrangThaiThucThe
{
    public Player_NhayTuong(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(player.lucNhayTuong.x * -player.huongQuay, player.lucNhayTuong.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
            mayTrangThai.thayDoiTrangThai(player.RoiXuong);

        if(player.daChamTuong)
            mayTrangThai.thayDoiTrangThai(player.TruotTuong);
    }
}
