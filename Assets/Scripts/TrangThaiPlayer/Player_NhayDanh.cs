using UnityEngine;

public class Player_NhayDanh : TrangThaiPlayer
{

    private bool dangDungTrenDat;// Đánh dấu đã đứng trên đất hay chưa
    public Player_NhayDanh(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dangDungTrenDat = false;// Khi mới vào trạng thái này → chưa chạm đất

        player.SetVelocity(player.tocDoNhayDanh.x * player.huongQuay, player.tocDoNhayDanh.y);
    }

    public override void Update()
    {
        base.Update();

        // Nếu player đã chạm đất nhưng biến chưa cập nhật
        if (player.daChamDat && dangDungTrenDat == false)
        {
            dangDungTrenDat = true;// Cập nhật đã đứng trên đất
            anim.SetTrigger("NhayDanhTrigger");// Gọi animation đánh khi chạm đất
            player.SetVelocity(0, rb.linearVelocity.y);// Dừng chuyển động ngang

        }
        // Nếu animation kết thúc và đã chạm đất → chuyển về trạng thái đứng yên
        if (triggerDuocGoi && player.daChamDat)
            mayTrangThai.thayDoiTrangThai(player.DungYen); 
    }
}
