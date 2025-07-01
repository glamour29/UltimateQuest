using UnityEngine;

public abstract class TrangThaiPlayer : TrangThaiThucThe
{
    protected Player player;
    protected PlayerInputSet input;

    public TrangThaiPlayer(Player player ,StateMachine mayTrangThai, string TenBoolanim) : base (mayTrangThai, TenBoolanim)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
        chiSo = player.chiSo;

    }
    public override void Update()
    {
        base.Update();
        anim.SetFloat("vtY", rb.linearVelocity.y);

        if (input.Player.Dash.WasPressedThisFrame() && choPhepLuot())
            mayTrangThai.thayDoiTrangThai(player.Luot);
    }



    private bool choPhepLuot()
    {// Nếu đang chạm tường thì không cho phép lướt
        if (player.daChamTuong)
            return false;
        if (mayTrangThai.TrangThaiHienTai == player.Luot)    // Nếu đang ở trạng thái lướt thì không cho phép lướt tiếp
            return false;

        return true;    // Nếu không rơi vào 2 điều kiện trên thì được phép lướt


    }
}
