using UnityEngine;

public abstract class TrangThaiThucThe
{
    protected Player player;
    protected StateMachine mayTrangThai;
    protected string TenBoolanim;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

    protected float thoiGianTrangThai;
    protected bool triggerDuocGoi;

    public TrangThaiThucThe(Player player ,StateMachine MayTrangThai, string TenBoolanim)
    {
        this.player = player;
        this.mayTrangThai = MayTrangThai;
        this.TenBoolanim = TenBoolanim;// Gán máy trạng thái truyền vào cho biến thành viên

        anim = player.anim;
        rb = player.rb;
        input = player.input;

    }

    public virtual void Enter()// Được gọi khi trạng thái bắt đầu hoạt động

    {
        anim.SetBool(TenBoolanim, true);
        triggerDuocGoi = false;
    }

    public virtual void Update ()// Được gọi mỗi khung hình khi trạng thái đang hoạt động

    {
        thoiGianTrangThai -= Time.deltaTime;
        anim.SetFloat("vtY", rb.linearVelocity.y);

        if(input.Player.Dash.WasPressedThisFrame() && choPhepLuot())
            mayTrangThai.thayDoiTrangThai(player.Luot);
    }

    public virtual void Exit() // Được gọi khi trạng thái kết thúc hoặc chuyển sang trạng thái khác

    {
        anim.SetBool(TenBoolanim, false);
    }

    public void GoiAnimationTrigger ()
    {
        triggerDuocGoi = true;
    }

    private bool choPhepLuot()
    {// Nếu đang chạm tường thì không cho phép lướt
        if (player.daChamTuong)
            return false;
        if (mayTrangThai.TrangThaiHienTai == player.Luot)    // Nếu đang ở trạng thái lướt thì không cho phép lướt tiếp
            return false;

        return true;    // Nếu không rơi vào 2 điều kiện trên thì được phép lướt


    }
}
