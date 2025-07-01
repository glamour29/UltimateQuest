using Unity.VisualScripting;
using UnityEngine;

public class Player_DanhThuong : TrangThaiPlayer
{

    private float dongHoTocDoTanCong;
    private float lanDanhCuoi;

    private bool comboTanCongDangCho;
    private int huongDanh;
    private int chiSoCombo = 1;
    private int gioiHanCombo = 3;
    private const int chiSoComboDauTien = 1;


    // Hàm khởi tạo cho trạng thái "Đánh thường"
    // Kiểm tra nếu giới hạn combo không trùng với độ dài mảng tốc độ tấn công,
    // thì sẽ tự động điều chỉnh lại cho khớp và cảnh báo trong console.
    public Player_DanhThuong(Player player, StateMachine MayTrangThai, string TenBoolanim) : base(player, MayTrangThai, TenBoolanim)
    {
        if (gioiHanCombo != player.tocDoTanCong.Length)
        {
            Debug.LogWarning("i've adjusted combo limit, according to attack velocity array!");
            gioiHanCombo = player.tocDoTanCong.Length;
        }

    }

    

    public override void Enter()
    {
        base.Enter();
        comboTanCongDangCho = false;// Đánh dấu là chưa bấm combo kế tiếp
        resetComboNeuCan();// Nếu qua lâu chưa đánh, reset lại combo về đầu
        dongBoHoaTocDoDanh();


        // Nếu đang bấm phím trái/phải → đánh theo hướng di chuyển
        // Ngược lại → đánh theo hướng quay mặt
        huongDanh = player.dichuyenInput.x != 0 ? ((int)player.dichuyenInput.x) : player.huongQuay;

            anim.SetInteger("chiSoDanhThuong", chiSoCombo);// Cập nhật chỉ số combo cho Animator
            apDungVanTocTanCong();// Áp dụng vận tốc cho cú đánh này (theo combo)

   }

    public override void Update()
    {
        base.Update();
        XuLyTocDoTanCong();// Giảm dần vận tốc tấn công sau khi đánh

        if (input.Player.TanCong.WasPressedThisFrame())    // Nếu người chơi nhấn nút tấn công lần nữa
            hangTanCongTiepTheo();// Đánh dấu chuẩn bị combo kế tiếp

        if (triggerDuocGoi)// Nếu animation đã kết thúc (trigger được gọi)
            xuLyKhiThoatTrangThai();
    }

    public override void Exit()
    {
        base.Exit();
        chiSoCombo++;// Tăng combo lên để chuẩn bị cho đòn kế tiếp
        lanDanhCuoi = Time.time;// Ghi lại thời điểm đánh cuối để tính reset combo
    }

    private void xuLyKhiThoatTrangThai ()
    {
        if (comboTanCongDangCho)
        {
            anim.SetBool(TenBoolanim, false);// Tắt animation hiện tại
            player.chuyenSangTanCongSauDelay();// Chờ rồi chuyển sang đòn combo tiếp theo
        }
        else
            mayTrangThai.thayDoiTrangThai(player.DungYen);// Không combo → về trạng thái đứng yên
    }


    private void hangTanCongTiepTheo()
    {
        if (chiSoCombo < gioiHanCombo)
            comboTanCongDangCho = true;
    }

    private void XuLyTocDoTanCong()
    {
        dongHoTocDoTanCong -= Time.deltaTime;// Giảm thời gian còn lại của vận tốc tấn công

        // Khi hết thời gian, dừng vận tốc tấn công theo chiều ngang
        if (dongHoTocDoTanCong < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void apDungVanTocTanCong()
    {
        Vector2 tocDoTanCong = player.tocDoTanCong[chiSoCombo -1 ];    // Lấy vận tốc tấn công của chiêu hiện tại trong combo


        dongHoTocDoTanCong = player.thoiGianTocDoTanCong;    // Đặt thời gian hiệu lực của vận tốc tấn công
        player.SetVelocity(tocDoTanCong.x * huongDanh, tocDoTanCong.y);    // Di chuyển nhân vật theo hướng đang quay và vận tốc tấn công
    }

    private void resetComboNeuCan()
    {
        if (Time.time > lanDanhCuoi + player.comboResetTime)// Nếu đã quá thời gian cho phép thực hiện đòn đánh tiếp theo trong chuỗi combo
            chiSoCombo = chiSoComboDauTien;// Đặt lại combo về đòn đầu tiên

        if (chiSoCombo > gioiHanCombo)// Nếu combo hiện tại vượt quá giới hạn cho phép
            chiSoCombo = chiSoComboDauTien;// Cũng đặt lại về combo đầu tiên
    }


}
