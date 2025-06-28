using System.Security;
using UnityEngine;

public class ThucThe_SucKhoe : MonoBehaviour, IBiThuong
{
    private ThucThe_VFX thuctheVfx;
    private ThucThe thucthe;

    [SerializeField]private float HpHientai;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] protected bool DaChet;


    [Header("Đẩy lùi khi trúng đòn")]
    [SerializeField] private Vector2 dayLuiTrungDon = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 dayLuiTrungDonManh = new Vector2(7, 7);
    [SerializeField] private float tgianDayLui = .2f;
    [SerializeField] private float tgianDayManh = .5f;

    [Space]
    [Header("Trúng đòn mạnh")]
    [Range(0, 1)]
    [SerializeField] private float nguonLucDayManh = .3f;


    protected virtual void Awake()
    {
        thucthe = GetComponent<ThucThe>();
        thuctheVfx = GetComponent<ThucThe_VFX>();
        HpHientai = maxHp;
    }

    public virtual void GaySatThuong (float satthuong, Transform KeGaySatThuong)
    {
        // Nếu đối tượng đã chết thì không nhận sát thương nữa
        if (DaChet)
            return;

        float tgian = TinhThoiGian(satthuong);
         Vector2 daylui = TinhToanDayLui(satthuong, KeGaySatThuong);

        // Nếu có hiệu ứng VFX thì kích hoạt hiệu ứng trúng đòn
        thuctheVfx?.ChayHieuUngTrungDon();
        thucthe?.NhanDaylui(daylui, tgian);

        MatMau(satthuong);    // Trừ máu (gọi hàm xử lý mất máu)
    }

    protected void MatMau (float satthuong)
    {
        HpHientai -= satthuong;

        if (HpHientai <= 0)
            Chet();
    }

    public void Chet()
    {
        DaChet = true;
        thucthe?.ThucTheBiTieuDiet(); 
    }

    // Tính toán lực đẩy lùi dựa trên sát thương và vị trí kẻ tấn công
    private Vector2 TinhToanDayLui (float satthuong, Transform KeGaySatThuong)
    {
        int huong = transform.position.x > KeGaySatThuong.position.x ? 1 : -1; // Xác định hướng đẩy lùi

        Vector2 daylui = SatThuongManh(satthuong) ? dayLuiTrungDonManh : dayLuiTrungDon; // Chọn lực đẩy phù hợp
        daylui.x = daylui.x * huong; // Áp hướng vào lực đẩy theo trục X

        return daylui;// Trả về vector đẩy lùi
    }    

    private float TinhThoiGian(float satthuong)
    {
        return SatThuongManh(satthuong) ? tgianDayManh : tgianDayLui; 
    }

    private bool SatThuongManh(float satthuong)
    {
        return satthuong / maxHp > nguonLucDayManh;
    }

}
