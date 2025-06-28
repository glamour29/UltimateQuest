using Unity.VisualScripting;
using UnityEngine;

public class Player_DungDat : TrangThaiPlayer
{
    public Player_DungDat(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }

    public override void Update()
    {
        
        base.Update();

        if (rb.linearVelocity.y < 0 && player.daChamDat == false)// Nếu đang rơi xuống thì chuyển sang trạng thái Rơi
            mayTrangThai.thayDoiTrangThai(player.RoiXuong);

        if (input.Player.Jump.WasPressedThisFrame()) //lenh unity,// Nếu vừa nhấn nút nhảy thì chuyển sang trạng thái Nhảy
            mayTrangThai.thayDoiTrangThai(player.Nhay);

        if(input.Player.TanCong.WasPressedThisFrame())
            mayTrangThai.thayDoiTrangThai(player.DanhThuong);

        if(input.Player.PhanDon.WasPressedThisFrame())
            mayTrangThai.thayDoiTrangThai(player.PhanDon);

    }
}
