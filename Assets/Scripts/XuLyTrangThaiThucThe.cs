using System.Collections;
using UnityEngine;

public class XuLyTrangThaiThucThe : MonoBehaviour
{
    private ThucThe thucthe;
    private ThucThe_VFX thuctheVfx;
    private ChiSo_ThucThe chiSoThucThe;
    private ThucThe_SucKhoe thucTheSucKhoe;
    private LoaiNguyenTo hieuUngHienTai = LoaiNguyenTo.KhongCo;

    [Header("Hiệu ứng sấm sét")]
    [SerializeField] private GameObject samSetVfx;
    [SerializeField] private float nangLuongHienTai;
    [SerializeField] private float nangLuongToiDa = 1;
    private Coroutine CoroutineSamSet;

    private void Awake()
    {
        thucthe = GetComponent<ThucThe>();
        thucTheSucKhoe= GetComponent <ThucThe_SucKhoe>();
        chiSoThucThe = GetComponent<ChiSo_ThucThe>();
        thuctheVfx = GetComponent<ThucThe_VFX>();
    }

    public void ApDungVfxSamSet(float thoiGianKeoDai, float satthuong, float napNangLuong)
    {
        // Lấy chỉ số kháng nguyên tố sét của thực thể
        float KhangSet = chiSoThucThe.LayKhangNguyenTo(LoaiNguyenTo.Set);

        // Tính tổng năng lượng sau khi bị trừ bởi kháng sét
        float tongNangLuong = napNangLuong * (1- KhangSet);

        // Tăng năng lượng hiện tại lên theo lượng nạp vào
        nangLuongHienTai = nangLuongHienTai + tongNangLuong;

        // Nếu đã đạt hoặc vượt mức năng lượng tối đa
        if (nangLuongHienTai >= nangLuongToiDa)
        {
            GaySetDanh(satthuong);
            NgatSamSetVFX();
            return;
        }
        // Nếu Coroutine sét đang chạy rồi, dừng nó để không chạy chồng chéo
        if (CoroutineSamSet != null)
            StopCoroutine(CoroutineSamSet);

        // Bắt đầu lại Coroutine hiệu ứng sét với thời gian kéo dài thoiGianKeoDai
        CoroutineSamSet = StartCoroutine(CoroutineVfxSamset(thoiGianKeoDai));
    }

    private void NgatSamSetVFX()
    {
        hieuUngHienTai = LoaiNguyenTo.KhongCo;
        nangLuongHienTai = 0;
        thuctheVfx.DungLaiTatCaVfx();
    }    

    private void GaySetDanh(float satthuong)
    {
        // Tạo hiệu ứng sét đánh (VFX) tại vị trí của đối tượng
        Instantiate(samSetVfx, transform.position, Quaternion.identity);

        // Gây sát thương cho thực thể (trừ máu với lượng sát thương của sét)
        thucTheSucKhoe.MatMau(satthuong);
    }

    private IEnumerator CoroutineVfxSamset (float thoiGianKeoDai)
    {
        hieuUngHienTai = LoaiNguyenTo.Set;
        thuctheVfx.ChayVFXTrangThai(thoiGianKeoDai, LoaiNguyenTo.Set);

        yield return new WaitForSeconds(thoiGianKeoDai);
        NgatSamSetVFX();
    }


    public void ApDungVfxDotChay (float thoiGianKeoDai, float satThuongDotChay)
    {
        float khangLua = chiSoThucThe.LayKhangNguyenTo(LoaiNguyenTo.Lua);
        float satThuongCuoiCung = satThuongDotChay * (1 - khangLua);

        StartCoroutine (CoroutineDotChayVfx(thoiGianKeoDai,satThuongCuoiCung));
    }

    // Coroutine chạy hiệu ứng DOT (Damage Over Time) cho cháy (burn)
    private IEnumerator CoroutineDotChayVfx (float thoiGianKeoDai, float tongSatThuong)
    {
        hieuUngHienTai = LoaiNguyenTo.Lua;// Gán trạng thái hiệu ứng hiện tại là Lửa
        // Kích hoạt hiệu ứng VFX (nhấp nháy, đổi màu...) trong suốt thời gian cháy
        thuctheVfx.ChayVFXTrangThai(thoiGianKeoDai, LoaiNguyenTo.Lua);

        int soLanTickMoiGiay = 2;// Cấu hình: số lần tick sát thương trong mỗi giây
        // Tính tổng số lần tick trong suốt thời gian kéo dài
        int soLanTick = Mathf.RoundToInt (soLanTickMoiGiay * thoiGianKeoDai);

        float satThuongMoiLanTick = tongSatThuong / soLanTick;// Tính sát thương mỗi lần tick
        float nhipTick = 1f/ soLanTickMoiGiay;// Khoảng thời gian giữa mỗi lần tick

        for ( int i = 0; i < soLanTick; i++)// Vòng lặp gây sát thương theo từng tick
        {
            thucTheSucKhoe.MatMau(satThuongMoiLanTick);// Gây sát thương cho đối tượng
            yield return new WaitForSeconds (nhipTick);// Đợi tới lần tick tiếp theo
        }
        hieuUngHienTai = LoaiNguyenTo.KhongCo;// Kết thúc hiệu ứng, reset lại trạng thái không còn lửa
    }

    public void apDungVfxDongBangNhe(float tgian, float heSoLamCham)
    {
        float KhangBang = chiSoThucThe.LayKhangNguyenTo(LoaiNguyenTo.Bang);
        float thoiGianKeoDaiCuoiCung = tgian * (1 - KhangBang);

        StartCoroutine(CoroutineDongBangThucThe(thoiGianKeoDaiCuoiCung, heSoLamCham));
    }

    private IEnumerator CoroutineDongBangThucThe (float tgian, float heSoLamCham)
    {
        thucthe.lamChamThucThe(tgian, heSoLamCham);
        hieuUngHienTai = LoaiNguyenTo.Bang;
        thuctheVfx.ChayVFXTrangThai(tgian, LoaiNguyenTo.Bang);

        yield return new WaitForSeconds(tgian);
        hieuUngHienTai = LoaiNguyenTo.KhongCo; 
    }
    public bool coTheApDung(LoaiNguyenTo nguyento)
    {
        if (nguyento == LoaiNguyenTo.Set && hieuUngHienTai == LoaiNguyenTo.Set)
            return true;

        return hieuUngHienTai == LoaiNguyenTo.KhongCo;
    }

}
