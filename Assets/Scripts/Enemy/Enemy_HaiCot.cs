using UnityEngine;

public class Enemy_HaiCot : Enemy, IBiPhanDon
{
    public bool CoTheDaBiPhanDon { get => coTheBiChoang; }

    protected override void Awake()
    {
        base.Awake();

        DungYen = new Enemy_DungYen(this, mayTrangThai, "dungyen");
        DiChuyen = new Enemy_DiChuyen(this, mayTrangThai, "dichuyen");
        TanCong = new Enemy_TanCong(this, mayTrangThai, "tancong");
        DanhNhau = new Enemy_DanhNhau(this, mayTrangThai, "danhnhau");
        Chet = new Enemy_Chet(this, mayTrangThai, "dungyen");
        BiChoang = new Enemy_BiChoang(this, mayTrangThai, "bichoang");
    }

    protected override void Start()
    {
        base.Start();

        mayTrangThai.KhoiTao(DungYen);
    }

    public void XuLyPhanDon()
    {
        if (CoTheDaBiPhanDon == false)
            return;
        mayTrangThai.thayDoiTrangThai(BiChoang);
    }

}