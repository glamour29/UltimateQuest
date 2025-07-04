﻿using UnityEngine;

public abstract class TrangThaiThucThe
{
    protected StateMachine mayTrangThai;
    protected string TenBoolanim;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected ChiSo_ThucThe chiSo;

    protected float tgianTrangThai;
    protected bool triggerDuocGoi;

    public TrangThaiThucThe(StateMachine mayTrangThai, string TenBoolanim)
    {
        this.mayTrangThai = mayTrangThai;
        this.TenBoolanim = TenBoolanim;
    }
    public virtual void Enter()// Được gọi khi trạng thái bắt đầu hoạt động

    {
        anim.SetBool(TenBoolanim, true);
        triggerDuocGoi = false;
    }

    public virtual void Update()// Được gọi mỗi khung hình khi trạng thái đang hoạt động

    {
        tgianTrangThai -= Time.deltaTime;
    }

    public virtual void Exit() // Được gọi khi trạng thái kết thúc hoặc chuyển sang trạng thái khác

    {
        anim.SetBool(TenBoolanim, false);
    }

    public void AnimationTrigger()
    {
        triggerDuocGoi = true;
    }

    public virtual void capNhatThamSoAnimation ()
    {

    }

    public void dongBoHoaTocDoDanh ()
    {
        float tocDoTanCong = chiSo.congTanCong.tocDoDanh.layDuLieu();
        anim.SetFloat("HeSoTocDoTanCong", tocDoTanCong);
    }
}
