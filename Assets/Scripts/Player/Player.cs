using System;
using System.Collections;
using UnityEngine;


public class Player : ThucThe
{
    public static event Action KhiPlayerChet;

    public PlayerInputSet input { get; private set; }

    public Player_DungYen DungYen { get; private set; }
    public Player_DiChuyen DiChuyen { get; private set; }
    public Player_Nhay Nhay { get; private set; }
    public Player_RoiXuong RoiXuong { get; private set; }
    public Player_TruotTuong TruotTuong { get; private set; }
    public Player_NhayTuong NhayTuong { get; private set; }
    public Player_Luot Luot { get; private set; }
    public Player_DanhThuong DanhThuong { get; private set; }
    public Player_NhayDanh NhayDanh { get; private set; }
    public Player_Chet Chet { get; private set; }
    public Player_PhanDon PhanDon { get; private set; }

    [Header("Tấn công")]
    public Vector2[] tocDoTanCong;
    public Vector2 tocDoNhayDanh;
    public float thoiGianTocDoTanCong = .1f;
    public float comboResetTime = 1;
    private Coroutine tanCongDangCho;


    [Header("Di chuyển")]
    public float tocDoDiChuyen;
    public float LucNhay = 5;
    public Vector2 lucNhayTuong;
    [Range(0, 1)]
    public float heSoO2 = .7f; // should be from 0 to 1;
    [Range(0, 1)]
    public float heSoTruotTuong = .7f;
    [Space]
    public float tgianLuot = .25f;
    public float tocDoLuot = 20;
    public Vector2 dichuyenInput { get; private set; } //Biến dichuyenInput có kiểu Vector2


    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        DungYen = new Player_DungYen(this, mayTrangThai, "dungyen"); //Khởi tạo đối tượng Player_DungYen, truyền vào máy trạng thái và tên trạng thái, dùng để quản lý và xác định đây là trạng thái gì.
        DiChuyen = new Player_DiChuyen(this, mayTrangThai, "dichuyen");
        Nhay = new Player_Nhay(this, mayTrangThai, "Nhay/Roi");
        RoiXuong = new Player_RoiXuong(this, mayTrangThai, "Nhay/Roi");
        TruotTuong = new Player_TruotTuong(this, mayTrangThai, "TruotTuong");
        NhayTuong = new Player_NhayTuong(this, mayTrangThai, "Nhay/Roi");
        Luot = new Player_Luot(this, mayTrangThai, "luot");
        DanhThuong = new Player_DanhThuong(this, mayTrangThai, "DanhThuong");
        NhayDanh = new Player_NhayDanh(this, mayTrangThai, "Nhay/Danh");
        Chet = new Player_Chet(this, mayTrangThai, "Chet");
        PhanDon = new Player_PhanDon(this, mayTrangThai, "PhanDon");
    }

    protected override void Start()
    {
        base.Start();
        mayTrangThai.KhoiTao(DungYen);
    }
    protected override IEnumerator CoroutinelamChamThucThe(float tgian, float heSoLamCham)
    {
        float tocDoDiChuyenGoc = tocDoDiChuyen;
        float tocDoLucNhayGoc = LucNhay;
        float tocDoAnimGoc = anim.speed;
        Vector2 NhayTuongGoc = lucNhayTuong;
        Vector2 NhayDanhGoc = tocDoNhayDanh;
        Vector2[] tocDoTanCongGoc = tocDoTanCong;

        float heSoTocDo = 1 - heSoLamCham;

        tocDoDiChuyen = tocDoDiChuyen * heSoTocDo;
        LucNhay = LucNhay * heSoTocDo;
        anim.speed = anim.speed * heSoTocDo;
        lucNhayTuong = lucNhayTuong * heSoTocDo;
        tocDoNhayDanh = tocDoNhayDanh * heSoTocDo;

        for (int i = 0; i < tocDoTanCong.Length; i++)
        {
            tocDoTanCong[i] = tocDoTanCong[i] * heSoTocDo;
        }

        yield return new WaitForSeconds(tgian);

        tocDoDiChuyen = tocDoDiChuyenGoc;
        LucNhay = tocDoLucNhayGoc;
        anim.speed = tocDoAnimGoc;
        lucNhayTuong = NhayTuongGoc;
        tocDoNhayDanh = NhayDanhGoc;

        for (int i = 0; i < tocDoTanCong.Length; i++)
        {
            tocDoTanCong[i] = tocDoTanCongGoc[i];
        }
    }
    public override void ThucTheBiTieuDiet()
    {
        base.ThucTheBiTieuDiet();

        KhiPlayerChet?.Invoke();
        mayTrangThai.thayDoiTrangThai(Chet);
    }

    public void chuyenSangTanCongSauDelay()
    {
        if (tanCongDangCho != null)
            StopCoroutine(tanCongDangCho);

        tanCongDangCho = StartCoroutine(chuyenSangTanCongSauDelayCo());
    }

    private IEnumerator chuyenSangTanCongSauDelayCo()
    {
        yield return new WaitForEndOfFrame();
        mayTrangThai.thayDoiTrangThai(DanhThuong);
    }


    private void OnEnable()
    {
        input.Enable();  // Bật hệ thống điều khiển khi object được kích hoạt


        input.Player.DiChuyen.performed += ctx => dichuyenInput = ctx.ReadValue<Vector2>(); // gán giá trị từ bàn phím:Khi người chơi bấm phím điều hướng (← → ↑ ↓) hoặc di chuyển joystick, thì sự kiện performed được gọi.Unity sẽ gán giá trị Vector2 tương ứng vào dichuyenInput.
        input.Player.DiChuyen.canceled += ctx => dichuyenInput = Vector2.zero;

    }

}

