using UnityEngine;

public class Player_Luot : TrangThaiPlayer
{
    private float trongLucGoc;
    private float huongLuot;
    public Player_Luot(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }
    public override void Enter()
    {
        base.Enter();

        huongLuot = player.dichuyenInput.x != 0 ? ((int)player.dichuyenInput.x) : player.huongQuay;
        tgianTrangThai = player.tgianLuot; 

        trongLucGoc = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        huyLuot();
        player.SetVelocity(player.tocDoLuot * huongLuot, 0); // Lướt ngang theo hướng đang quay với tốc độ tocDoLuot, không di chuyển lên xuống


        if (tgianTrangThai < 0)
        {
            if (player.daChamDat)
                mayTrangThai.thayDoiTrangThai(player.DungYen);
            else
                mayTrangThai.thayDoiTrangThai(player.RoiXuong);
        
        } 
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, 0);
        rb.gravityScale = trongLucGoc;
    }

    private void huyLuot()
    {
        if (player.daChamTuong)
        {
            if (player.daChamDat)
                mayTrangThai.thayDoiTrangThai(player.DungYen);
            else
                mayTrangThai.thayDoiTrangThai(player.TruotTuong);
        }
    }
}
