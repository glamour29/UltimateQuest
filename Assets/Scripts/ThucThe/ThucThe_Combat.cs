using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class ThucThe_Combat : MonoBehaviour
{
    private ThucThe_VFX vfx;
    private ChiSo_ThucThe ChiSo;

    [Header("Mục tiêu")]
    [SerializeField] private Transform ktMucTieu;
    [SerializeField] private float banKinhKiemTraMucTieu = 1;
    [SerializeField] private LayerMask XacDinhMucTieu;

    [Header("Hiệu ứng trạng thái")]
    [SerializeField] private float tgianKeoDaiMacDinh = 3;
    [SerializeField] private float heSoLamChamTuVfxLanh = .2f;
    [SerializeField] private float tichLuyNangLuongDienGiat = .4f;
    [Space]
    [SerializeField] private float tiLeLua = .8f;
    [SerializeField] private float tiLeSamSet = 2.5f;


    private void Awake()
    {
        vfx = GetComponent<ThucThe_VFX>();
        ChiSo = GetComponent<ChiSo_ThucThe>();
    }
    // Hàm thực hiện tấn công các mục tiêu trong vùng
    public void ThucHienTanCong()
    {
        LayVaCham();// Gọi để lấy các va chạm (có thể bỏ nếu không cần gọi dư)
        foreach (var target in LayVaCham())// Duyệt qua từng mục tiêu trong vùng
        {
            IBiThuong bithuong = target.GetComponent<IBiThuong>();

            if (bithuong == null) //skip muc tieu, qua muc tieu tiep theo
                continue;

            float satThuongNguyenTo = ChiSo.LaySatThuongNguyenTo(out LoaiNguyenTo nguyento, .6f);
            float satthuong = ChiSo.TinhSatThuongVatLy(out bool BaoKich);
            bool MucTieuBiDanh = bithuong.GaySatThuong(satthuong, satThuongNguyenTo, nguyento, transform);

            // Nếu nguyên tố hiện tại KHÔNG phải là 'Không Có' (tức là có nguyên tố như Lửa, Băng, Sét...)
            if (nguyento != LoaiNguyenTo.KhongCo)
                // Gọi hàm để áp dụng hiệu ứng trạng thái nguyên tố lên mục tiêu
                // - target.transform: là vị trí hoặc thực thể bị ảnh hưởng
                // - nguyento: loại nguyên tố sẽ gây hiệu ứng tương ứng
                apDungVfxTrangThai(target.transform, nguyento);

            if (MucTieuBiDanh)
            {
                vfx.capNhatMauKhiTrungDon(nguyento);
                vfx.TaoHieuUngVFXKhiDanh(target.transform, BaoKich); // Hiển thị hiệu ứng đánh trúng
            }

        }
    }

    // Hàm áp dụng hiệu ứng VFX trạng thái nguyên tố (băng, lửa, sét) cho mục tiêu
    public void apDungVfxTrangThai(Transform target, LoaiNguyenTo nguyento, float heSoTiLe = 1)
    {
        XuLyTrangThaiThucThe xuLyTrangThai = target.GetComponent<XuLyTrangThaiThucThe>();

        if (xuLyTrangThai == null)
            return;

        if (nguyento == LoaiNguyenTo.Bang && xuLyTrangThai.coTheApDung(LoaiNguyenTo.Bang))
            xuLyTrangThai.apDungVfxDongBangNhe(tgianKeoDaiMacDinh, heSoLamChamTuVfxLanh);

        if (nguyento == LoaiNguyenTo.Lua && xuLyTrangThai.coTheApDung(LoaiNguyenTo.Lua))
        {
            heSoTiLe = tiLeLua;
            float satThuongLua = ChiSo.congTanCong.SatThuongLua.layDuLieu() * heSoTiLe ;
            xuLyTrangThai.ApDungVfxDotChay(tgianKeoDaiMacDinh, satThuongLua);
        }
        
        if (nguyento == LoaiNguyenTo.Set && xuLyTrangThai.coTheApDung(LoaiNguyenTo.Set))
        {
            heSoTiLe = tiLeSamSet;
            float satThuongSamSet = ChiSo.congTanCong.SatThuongSet.layDuLieu() * heSoTiLe;
            xuLyTrangThai.ApDungVfxSamSet(tgianKeoDaiMacDinh, satThuongSamSet, tichLuyNangLuongDienGiat);
        }
    }

    // Hàm kiểm tra va chạm trong bán kính xung quanh một vị trí
    protected Collider2D[] LayVaCham()
    {
        return Physics2D.OverlapCircleAll(ktMucTieu.position, banKinhKiemTraMucTieu, XacDinhMucTieu);

    }
    // Vẽ vòng tròn kiểm tra mục tiêu trong editor (chỉ để debug)
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(ktMucTieu.position, banKinhKiemTraMucTieu);
    }
}
