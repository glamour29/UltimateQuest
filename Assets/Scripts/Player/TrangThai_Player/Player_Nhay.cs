using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Player_Nhay : Player_TrenKhong
{
    public Player_Nhay(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.linearVelocity.x, player.LucNhay);        // Trục X giữ nguyên, trục Y được đặt thành lực nhảy (bật lên)

    }

    public override void Update()
    {
        base.Update();


        // Nếu nhân vật đang rơi xuống (vận tốc Y âm) và KHÔNG phải đang ở trạng thái "Nhảy đánh"
        if (rb.linearVelocity.y < 0 && mayTrangThai.TrangThaiHienTai != player.NhayDanh)   
            mayTrangThai.thayDoiTrangThai(player.RoiXuong);        // Chuyển sang trạng thái rơi xuống

    }

}
