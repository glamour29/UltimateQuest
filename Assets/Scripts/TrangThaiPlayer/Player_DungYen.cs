using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player_DungYen : Player_DungDat
{
    public Player_DungYen(Player player, StateMachine MayTrangThai, string tenTrangThai) : base(player, MayTrangThai, tenTrangThai)
    {
    }


    public override void Update()
    {
        base.Update();

        // Nếu đang di chuyển đúng hướng quay mặt và bị đụng tường → không làm gì cả (thoát khỏi hàm)
        if (player.dichuyenInput.x == player.huongQuay && player.daChamTuong)
            return;


        //player.rb.velocity = new vector2(moveinput.x * movespeed, rb.velocity.y);

        if (player.dichuyenInput.x !=0) //Kiểm tra người chơi CÓ bấm phím sang trái hoặc phải,player.dichuyenInput là một biến kiểu Vector2 — do chính mình khai báo ở trong class Player
            mayTrangThai.thayDoiTrangThai(player.DiChuyen);


    }

 
}
