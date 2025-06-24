using System.Runtime.CompilerServices;
using UnityEngine;

public class Player_TruotTuong : TrangThaiThucThe
{
    public Player_TruotTuong(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }

    public override void Update()
    {
        base.Update();
        XuLyTruotTuong();

        if (input.Player.Jump.WasPressedThisFrame())
            mayTrangThai.thayDoiTrangThai(player.NhayTuong);


        if (player.daChamTuong == false)// Nếu **KHÔNG** còn chạm tường nữa (tức đã rời khỏi bề mặt tường)
            mayTrangThai.thayDoiTrangThai(player.RoiXuong);// Chuyển trạng thái sang "Rơi xuống"

        // Kiểm tra nếu nhân vật đã chạm đất → chuyển về trạng thái "Đứng Yên"
        if (player.daChamDat)
        {
            mayTrangThai.thayDoiTrangThai(player.DungYen);
            player.Lat();
        }
    }
        private void XuLyTruotTuong ()
    {
        if (player.dichuyenInput.y < 0) // Nếu người chơi đang nhấn phím đi xuống (tức là muốn rơi nhanh)
            player.SetVelocity(player.dichuyenInput.x, rb.linearVelocity.y);    // Giữ nguyên vận tốc rơi (Y), cho phép di chuyển ngang (X) như bình thường
        else
            player.SetVelocity(player.dichuyenInput.x, rb.linearVelocity.y * player.heSoTruotTuong);// Nếu KHÔNG nhấn xuống thì làm giảm tốc độ rơi lại để tạo cảm giác bám tường (slide chậm),Vẫn cho di chuyển ngang (X), nhưng vận tốc rơi (Y) giảm còn 30%


    }
}