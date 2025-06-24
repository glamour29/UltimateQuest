using UnityEngine;

public class Player_DiChuyen : Player_DungDat
{
    public Player_DiChuyen(Player player, StateMachine MayTrangThai, string tenTrangThai) : base(player, MayTrangThai, tenTrangThai)
    {
    }

    public override void Update()
    {
        base.Update();

        if (player.dichuyenInput.x == 0 || player.daChamTuong) //Kiểm tra người chơi KHÔNG bấm phím sang trái/phải
            mayTrangThai.thayDoiTrangThai(player.DungYen);

        // Thiết lập vận tốc cho nhân vật theo hướng người chơi đang điều khiển
        player.SetVelocity(player.dichuyenInput.x * player.tocDoDiChuyen,// Tính vận tốc theo trục X: trái (-), phải (+)
            rb.linearVelocity.y);// Giữ nguyên vận tốc theo trục Y (ví dụ: đang rơi hoặc nhảy)
    }
} 
