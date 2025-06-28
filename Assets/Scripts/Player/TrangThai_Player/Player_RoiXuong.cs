using UnityEngine;

public class Player_RoiXuong : Player_TrenKhong
{
    // Constructor - nhận player, máy trạng thái và tên animation
    public Player_RoiXuong(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0, rb.linearVelocity.y);        // Ngưng di chuyển theo chiều ngang, giữ nguyên tốc độ rơi

    }

    public override void Update()    // Hàm được gọi liên tục mỗi frame trong trạng thái này
    {
        base.Update();

        // Nếu đã chạm đất thì chuyển sang trạng thái "Đứng Yên"
        if (player.daChamDat)
            mayTrangThai.thayDoiTrangThai(player.DungYen);

        if (player.daChamTuong)
            mayTrangThai.thayDoiTrangThai(player.TruotTuong);
    }
}
 