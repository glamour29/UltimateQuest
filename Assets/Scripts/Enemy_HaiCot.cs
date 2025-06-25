using UnityEngine;

public class Enemy_HaiCot : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        DungYen = new Enemy_DungYen(this, mayTrangThai, "dungyen");
        DiChuyen = new Enemy_DiChuyen(this, mayTrangThai, "dichuyen");
    }

    protected override void Start()
    {
        base.Start();

        mayTrangThai.KhoiTao(DungYen);
    }
}
