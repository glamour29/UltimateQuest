using UnityEngine;

public class Player_TrenKhong : TrangThaiPlayer
{
    public Player_TrenKhong(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }

    public override void Update()
    {
        base.Update();

        if(player.dichuyenInput.x !=0)    // Kiểm tra xem người chơi có đang nhấn phím trái hoặc phải không (x ≠ 0 nghĩa là có di chuyển ngang)

                                          // Nếu có di chuyển, đặt lại vận tốc (velocity) cho nhân vật:
                                          // - trục X: tốc độ di chuyển (hướng × tốc độ)
                                          // - trục Y: giữ nguyên vận tốc dọc (để không ảnh hưởng việc rơi, nhảy...)
            player.SetVelocity(player.dichuyenInput.x * (player.tocDoDiChuyen* player.heSoO2), rb.linearVelocity.y);// Cập nhật vận tốc theo hướng di chuyển, có tính hệ số O2


        if (input.Player.TanCong.WasPressedThisFrame())
            mayTrangThai.thayDoiTrangThai(player.NhayDanh);
    }
}
