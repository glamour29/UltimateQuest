using UnityEngine;
using UnityEngine.UI;

public class ThucThe_SucKhoe : MonoBehaviour, IBiThuong
{
    private Slider thanhMau;
    private ThucThe thucthe;
    private ThucThe_VFX thuctheVfx;
    private ChiSo_ThucThe chiSoThucThe;

    [SerializeField] private float HpHientai;
    [SerializeField] protected bool DaChet;
    [Header("Hồi máu")]
    [SerializeField] private float khoangTgianHoiMau = 1;
    [SerializeField] private bool coTheHoiMauHayKhong = true;


    [Header("Đẩy lùi khi trúng đòn")]
    [SerializeField] private Vector2 dayLuiTrungDon = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 dayLuiTrungDonManh = new Vector2(7, 7);
    [SerializeField] private float tgianDayLui = .2f;
    [SerializeField] private float tgianDayManh = .5f;

    [Space]
    [Header("Trúng đòn mạnh")]
    [Range(0, 1)]
    [SerializeField] private float nguonLucDayManh = .3f;


    protected virtual void Awake()// Gọi khi đối tượng được khởi tạo (trước Start)
    {
        // Lấy các component cần thiết từ đối tượng hiện tại
        thucthe = GetComponent<ThucThe>();
        thuctheVfx = GetComponent<ThucThe_VFX>();
        chiSoThucThe = GetComponent<ChiSo_ThucThe>();
        thanhMau = GetComponentInChildren<Slider>();

        // Gán máu hiện tại bằng máu tối đa tính được từ chỉ số
        HpHientai = chiSoThucThe.layHpToiDa();
        CapNhatThanhMau();    // Cập nhật thanh máu hiển thị UI

        InvokeRepeating(nameof(hoiPhucMau), 0, khoangTgianHoiMau);

    }

    public virtual bool GaySatThuong(float satthuong, float satThuongTheoNguyenTo, LoaiNguyenTo nguyento, Transform KeGaySatThuong)
    {
        // Nếu đối tượng đã chết thì không nhận sát thương nữa
        if (DaChet)
            return false;

        if (DaNeDon())
        {
            Debug.Log($"{gameObject.name} da ne duoc don tan cong!");
            return false;
        }

        ChiSo_ThucThe chiSoKeTanCong = KeGaySatThuong.GetComponent<ChiSo_ThucThe>();
        float GiaTriGiamSatThuong = chiSoKeTanCong != null ? chiSoKeTanCong.layGiaTriGiamSatThuongTuGiap() : 0;


        // Tính tỉ lệ giảm sát thương từ giáp
        float tiLeGiamSatThuongg = chiSoThucThe.layTiLeGiamSatThuongTuGiap(GiaTriGiamSatThuong);
        float satThuongVatLyPhaiChiu = satthuong * (1 - tiLeGiamSatThuongg);

        float khang = chiSoThucThe.LayKhangNguyenTo(nguyento);
        float luongSatThuongNguyenToNhanVao = satThuongTheoNguyenTo * (1 - khang);

        NhanDayLui(KeGaySatThuong, satThuongVatLyPhaiChiu);
        MatMau(satThuongVatLyPhaiChiu);

        return true;
    }


    // Trả về true nếu né đòn thành công dựa trên tỉ lệ né đòn hiện tại
    private bool DaNeDon() => Random.Range(0, 100) < chiSoThucThe.LayGiaTriNeDon();

    private void hoiPhucMau ()
    {
        if (coTheHoiMauHayKhong == false)    // Nếu không được phép hồi máu thì thoát hàm
            return;
        float luongHoiMau = chiSoThucThe.nguonTaiNguyen.hoiMau.layDuLieu();    // Lấy lượng máu cần hồi từ chỉ số tài nguyên

        TangMau(luongHoiMau);// Thực hiện hồi máu
    }

    public void TangMau(float luongHoiMau)// Hàm tăng máu cho thực thể
    {
        if (DaChet)// Nếu đã chết thì không thể tăng máu, thoát hàm
            return;

        float mauMoi = HpHientai + luongHoiMau;// Tính lượng máu mới sau khi hồi
        float mauToiDa = chiSoThucThe.layHpToiDa();// Lấy giới hạn máu tối đa

        HpHientai = Mathf.Min(mauMoi, mauToiDa);// Cập nhật máu hiện tại, không vượt quá máu tối đa
        CapNhatThanhMau();// Cập nhật thanh máu hiển thị
    }    


    // Gọi khi bị mất máu, trừ máu hiện tại và kiểm tra nếu đã chết
    public void MatMau(float satthuong)
    {
        thuctheVfx?.ChayVfxTrungDon();
        HpHientai = HpHientai- satthuong;
        CapNhatThanhMau();

        if (HpHientai <= 0)
            Chet();
    }
    // Đánh dấu thực thể đã chết và gọi hàm tiêu diệt (destroy)
    public void Chet()
    {
        DaChet = true;
        thucthe?.ThucTheBiTieuDiet();
    }

    // Cập nhật thanh máu hiển thị trên UI theo phần trăm máu hiện tại
    private void CapNhatThanhMau()
    {
        if (thanhMau == null)
            return;

        thanhMau.value = HpHientai / chiSoThucThe.layHpToiDa();
    }
    private void NhanDayLui(Transform KeGaySatThuong, float satThuongCuoiCung)
    {
        Vector2 daylui = TinhToanDayLui(satThuongCuoiCung, KeGaySatThuong);
        float tgian = TinhThoiGianKeoDai(satThuongCuoiCung);

        thucthe?.NhanDaylui(daylui, tgian);
    }

    // Tính toán lực đẩy lùi dựa trên sát thương và vị trí kẻ tấn công
    private Vector2 TinhToanDayLui(float satthuong, Transform KeGaySatThuong)
    {
        int huong = transform.position.x > KeGaySatThuong.position.x ? 1 : -1;

        Vector2 daylui = SatThuongManh(satthuong) ? dayLuiTrungDonManh : dayLuiTrungDon;
        daylui.x = daylui.x * huong;

        return daylui;
    }

    private float TinhThoiGianKeoDai(float satthuong)
    {
        return SatThuongManh(satthuong) ? tgianDayManh : tgianDayLui;
    }

    private bool SatThuongManh(float satthuong)
    {
        return satthuong / chiSoThucThe.layHpToiDa() > nguonLucDayManh;
    }

}
