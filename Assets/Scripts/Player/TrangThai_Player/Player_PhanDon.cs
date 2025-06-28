using UnityEngine;

public class Player_PhanDon : TrangThaiPlayer
{
    private Player_Combat combat;
    private bool daphandonAiDo;
    public Player_PhanDon(Player player, StateMachine mayTrangThai, string TenBoolanim) : base(player, mayTrangThai, TenBoolanim)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        tgianTrangThai = combat.tgHoiPhanDon();
        daphandonAiDo = combat.ThucHienPhanDon();

        anim.SetBool("PhanDonDaThucHien",  daphandonAiDo);
    }
    public override void Update()
    {
        base.Update();
        player.SetVelocity(0, rb.linearVelocity.y);


        if (triggerDuocGoi)
            mayTrangThai.thayDoiTrangThai(player.DungYen);


        if (tgianTrangThai < 0 && daphandonAiDo == false)
            mayTrangThai.thayDoiTrangThai(player.DungYen);
    }
}
