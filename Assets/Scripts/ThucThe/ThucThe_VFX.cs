using System.Collections;
using UnityEngine;

public class ThucThe_VFX : MonoBehaviour
{

    private SpriteRenderer sr;
    private ThucThe thucthe;

    [Header("Hiệu ứng trúng đòn")]
    [SerializeField] private Material HieuUngTrungDon;
    [SerializeField] private float tgHieuUngTrungDon = .2f;
    private Material ChatLieuGoc;
    private Coroutine CoVfxTrungDon;

    [Header("Hiệu ứng VFX khi đánh")]
    [SerializeField] private Color mauVfxTrungDon = Color.white;
    [SerializeField] private GameObject danhVfx;
    [SerializeField] private GameObject BaoKichVfx;

    [Header("Màu sắc các nguyên tố")]
    [SerializeField] private Color DongBangVfx = Color.cyan;
    [SerializeField] private Color DotChayVfx = Color.red;
    [SerializeField] private Color DienGiatVfx = Color.yellow;
    private Color mauGocVfxTrungDon;
    private Coroutine CoroutineTrangThaiVfx;

    private void Awake()
    {
        thucthe = GetComponent<ThucThe>();

        sr = GetComponentInChildren<SpriteRenderer>(); // Lấy SpriteRenderer từ đối tượng con
        ChatLieuGoc = sr.material;// Lưu lại material ban đầu để sau này khôi phục
        mauGocVfxTrungDon = mauVfxTrungDon; 
    }

    public void ChayVFXTrangThai(float tgian, LoaiNguyenTo nguyento)
    {
        if (nguyento == LoaiNguyenTo.Bang)
            StartCoroutine(CoroutineChayVFXTrangThai(tgian, DongBangVfx));

        if ( nguyento == LoaiNguyenTo.Lua)
            StartCoroutine(CoroutineChayVFXTrangThai(tgian, DotChayVfx));

        if ( nguyento == LoaiNguyenTo.Set)
            StartCoroutine(CoroutineChayVFXTrangThai(tgian, DienGiatVfx));
    }

    public void DungLaiTatCaVfx ()
    {
        StopAllCoroutines();
        sr.color = Color.white;
        sr.material = ChatLieuGoc;

    }

    private IEnumerator CoroutineChayVFXTrangThai( float tgian, Color hieuUngMauSac)
    {
        float thoiGianMotLanTick = .25f;    // Thời gian giữa mỗi lần đổi màu (nhấp nháy)
        float thoigianDaQua = 0;    // Biến lưu tổng thời gian đã trôi qua

        // Tạo hai màu: màu sáng hơn và màu tối hơn dựa trên màu gốc
        Color mauSang = hieuUngMauSac * 1.2f;
        Color mauToi = hieuUngMauSac * .9f;

        // Biến kiểm soát bật/tắt (đổi giữa màu sáng và màu tối)
        bool batTat = false;

        // Vòng lặp chạy cho đến khi thời gian trôi qua đạt giới hạn tgian
        while (thoigianDaQua < tgian )
        {
            sr.color = batTat ? mauSang : mauToi;// Nếu batTat là true → đổi sang màu sáng, ngược lại màu tối
            batTat = !batTat;// Đảo ngược trạng thái (bật → tắt, hoặc tắt → bật)

            // Đợi một khoảng thời gian trước khi đổi màu lần tiếp theo
            yield return new WaitForSeconds(thoiGianMotLanTick);

            // Cộng dồn tổng thời gian đã trôi qua
            thoigianDaQua = thoigianDaQua + thoiGianMotLanTick;
        }

        sr.color = Color.white;
    }    

    public void TaoHieuUngVFXKhiDanh (Transform target, bool BaoKich)
    {
        GameObject PrefabKhiTrung = BaoKich ? BaoKichVfx : danhVfx;
        // Tạo một bản sao của hiệu ứng "danhVfx" tại vị trí của mục tiêu (target)
        // với góc quay mặc định (Quaternion.identity)
        GameObject vfx = Instantiate(PrefabKhiTrung,target.position, Quaternion.identity);
            vfx.GetComponentInChildren<SpriteRenderer>().color = mauVfxTrungDon;

        if (thucthe.huongQuay == -1 && BaoKich)
            vfx.transform.Rotate(0, 180, 0);
    }

    public void capNhatMauKhiTrungDon(LoaiNguyenTo nguyento)
    {
        if (nguyento == LoaiNguyenTo.Bang)
            mauVfxTrungDon = DongBangVfx;

        if (nguyento == LoaiNguyenTo.KhongCo)
            mauVfxTrungDon = mauGocVfxTrungDon;
    }

    public void ChayVfxTrungDon()
    {

        // Nếu hiệu ứng đang chạy, dừng lại để tránh đè nhau
        if ( CoVfxTrungDon != null)
            StopCoroutine(CoVfxTrungDon );

        // Bắt đầu hiệu ứng trúng đòn
        CoVfxTrungDon = StartCoroutine (ChayHeSoVfxTrungDon());
    }
    private IEnumerator ChayHeSoVfxTrungDon()
    {
        sr.material = HieuUngTrungDon;// Gán material hiệu ứng

        yield return new WaitForSeconds (tgHieuUngTrungDon);// Chờ một khoảng thời gian
        sr.material = ChatLieuGoc;// Trả lại material gốc
    }    
}
