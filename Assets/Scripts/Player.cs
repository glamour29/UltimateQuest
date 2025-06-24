using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;


public class Player : MonoBehaviour //Định nghĩa một class tên là Player, kế thừa từ MonoBehaviour, để có thể gắn vào GameObject trong Unity.
{

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerInputSet input { get; private set; } 

    private StateMachine mayTrangThai;


    public Player_DungYen DungYen { get; private set; }
    public Player_DiChuyen DiChuyen { get; private set; }
    public Player_Nhay Nhay { get; private set; }
    public Player_RoiXuong RoiXuong { get; private set; }
    public Player_TruotTuong TruotTuong { get; private set; }
    public Player_NhayTuong NhayTuong { get; private set; }
    public Player_Luot Luot { get; private set; }
    public Player_DanhThuong DanhThuong { get; private set; }
    public Player_NhayDanh NhayDanh { get; private set; }


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

    [Range(0,1)]
    public float heSoO2 = .7f; // should be from 0 to 1;
    [Range(0, 1)]
    public float heSoTruotTuong = .7f;
    [Space]
    public float tgianLuot = .25f;
    public float tocDoLuot = 20;

    private bool quaySangPhai = true;
    public int huongQuay { get; private set; } = 1;
    public Vector2 dichuyenInput { get; private set; } //Biến dichuyenInput có kiểu Vector2


    [Header("Phát hiện va chạm")]
    [SerializeField] private float kcKiemTraDat;
    [SerializeField] private float kcKiemTraTuong;
    [SerializeField] private LayerMask MatDat;
    [SerializeField] Transform diemTuong;
    [SerializeField] Transform diemTuongPhu;
    public bool daChamDat {  get; private set; }
    public bool daChamTuong { get; private set; }



    // Hàm Awake được Unity gọi đầu tiên khi GameObject được khởi tạo (trước Start)
    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();

        // Khởi tạo máy trạng thái
        mayTrangThai = new StateMachine();
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

    }

    private void OnEnable()
    {
        input.Enable();  // Bật hệ thống điều khiển khi object được kích hoạt


        input.Player.DiChuyen.performed += ctx => dichuyenInput = ctx.ReadValue<Vector2>(); // gán giá trị từ bàn phím:Khi người chơi bấm phím điều hướng (← → ↑ ↓) hoặc di chuyển joystick, thì sự kiện performed được gọi.Unity sẽ gán giá trị Vector2 tương ứng vào dichuyenInput.
        input.Player.DiChuyen.canceled += ctx => dichuyenInput = Vector2.zero;

    }

    private void OnDisable()
    {
        input.Disable();// Tắt hệ thống điều khiển khi object bị vô hiệu hóa
    }

    private void Start()
    {
        // Thiết lập trạng thái ban đầu cho Player là "Đứng yên"
        mayTrangThai.KhoiTao(DungYen);
    }

    // Hàm Update được gọi mỗi khung hình (frame), dùng để xử lý logic thay đổi theo thời gian
    private void Update()
    {
        XuLyPhatHienVaCham();
        mayTrangThai.CapNhatTrangThai();// => Cho phép mỗi trạng thái tự xử lý logic riêng của nó khi đang hoạt động
    }

    public void chuyenSangTanCongSauDelay ()
    {
        if (tanCongDangCho != null)
            StopCoroutine(tanCongDangCho);

        tanCongDangCho = StartCoroutine(chuyenSangTanCongSauDelayCo());
    }

    private IEnumerator chuyenSangTanCongSauDelayCo ()
    {
        yield return new WaitForEndOfFrame();
        mayTrangThai.thayDoiTrangThai(DanhThuong);
    }    

    public void GoiAnimationTrigger()
    {
        mayTrangThai.TrangThaiHienTai.GoiAnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity); // tạo một vector 2 chiều 
        XuLyLatHuong(xVelocity);
    }

    private void XuLyLatHuong(float xVelocity)
    {
        if (xVelocity > 0 && quaySangPhai == false)    // Nếu đang di chuyển sang phải mà nhân vật chưa quay phải → lật lại

            Lat();
        else if (xVelocity <  0 && quaySangPhai)    // Nếu đang di chuyển sang trái mà nhân vật đang quay phải → lật lại

            Lat();
    }

    public void Lat ()
    {
        transform.Rotate(0, 180, 0);    // Xoay nhân vật 180 độ quanh trục Y (lật mặt)
        quaySangPhai = !quaySangPhai;    // Đảo ngược trạng thái quay (nếu đang quay phải thì thành quay trái và ngược lại)
        huongQuay = huongQuay * -1;

    }

    private void XuLyPhatHienVaCham()

    {
        // Tạo một tia raycast bắn xuống dưới từ vị trí của nhân vật
        // Nếu tia này chạm trúng lớp mặt đất (MatDat), thì biến daChamDat = true
        daChamDat = Physics2D.Raycast(transform.position, Vector2.down, kcKiemTraDat, MatDat);
        daChamTuong = Physics2D.Raycast(diemTuong.position, Vector2.right * huongQuay, kcKiemTraTuong, MatDat)
                   && Physics2D.Raycast(diemTuongPhu.position, // Từ vị trí kiểm tra tường phụ
                   Vector2.right * huongQuay, // Theo hướng nhân vật đang quay mặt
                   kcKiemTraTuong, MatDat);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, // Điểm bắt đầu: vị trí hiện tại của nhân vật
            transform.position + new Vector3 (0, -kcKiemTraDat));// Vẽ một đường thẳng từ vị trí hiện tại đi xuống dưới một đoạn bằng KhoangCachKiemTraDat

        Gizmos.DrawLine(diemTuong.position,
            diemTuong.position + new Vector3(kcKiemTraTuong,huongQuay * 0));// Điểm kết thúc: lùi/tiến một đoạn theo trục X (tùy hướng quay)

        Gizmos.DrawLine(diemTuongPhu.position,
            diemTuongPhu.position + new Vector3(kcKiemTraTuong * huongQuay, 0));
    }
}

