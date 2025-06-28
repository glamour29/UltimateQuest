using UnityEngine;

public class Player_Chet : TrangThaiPlayer
{ 
    public Player_Chet(Player player, StateMachine mayTrangThai, string TenBoolanim) : base(player, mayTrangThai, TenBoolanim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.simulated =false;
    }
}
    
