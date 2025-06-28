using UnityEngine;

public class Enemy : ThucThe
{

    public Enemy_DungYen DungYen;
    public Enemy_DiChuyen DiChuyen;
    public Enemy_TanCong TanCong;
    public Enemy_DanhNhau DanhNhau;
    public Enemy_Chet Chet;
    public Enemy_BiChoang BiChoang;

    [Header("Đánh nhau")]
    public float tocDoDanhNhau = 3;
    public float KhoangCachTanCong = 2;
    public float ThoiGianGiaoChien = 5;
    public float kcLuiToiThieu = 1;
    public Vector2 tocDoLui;

    [Header("Bị choáng")]
    public float tgianBiChoang = 1;
    public Vector2 vanTocBiChoang = new Vector2(7, 7);
    [SerializeField] protected bool coTheBiChoang;


    [Header("Di chuyển")]
    public float DungYenTime = 2;
    public float tocDoDiChuyen = 1.4f;
    [Range(0, 2)]
    public float heSoTocDoDiChuyen = 1;

    [Header("Nhận diện player")]
    [SerializeField] private LayerMask NhanDienPlayer;
    [SerializeField] private Transform KiemTraPlayer;
    [SerializeField] private float khoangCachKiemTraPlayer = 10 ;
    public Transform player { get;private set; }

    public void BatPhanDon(bool enable) => coTheBiChoang = enable;

    public override void ThucTheBiTieuDiet()
    {
        base.ThucTheBiTieuDiet();
        mayTrangThai.thayDoiTrangThai(Chet);
    }

    private void XuLyPlayerChet()
    {
        mayTrangThai.thayDoiTrangThai(DungYen);
    }

    // Hàm này dùng để đưa enemy vào trạng thái đánh nhau (nếu chưa ở trạng thái đó)
    public void VaoTrangThaiDanhNhau(Transform player)
    {
        // Nếu trạng thái hiện tại đã là DanhNhau (battle), thì không làm gì nữa
        if (mayTrangThai.TrangThaiHienTai == DanhNhau)
            return;

        // Nếu đang ở trạng thái TanCong (attack), thì cũng không đổi trạng thái
        if (mayTrangThai.TrangThaiHienTai == TanCong)
            return;

        // Gán player làm mục tiêu hiện tại
        this.player = player;
        mayTrangThai.thayDoiTrangThai(DanhNhau);    // Chuyển sang trạng thái DanhNhau (battle state)
    }

    public Transform LayThamChieuPlayer()
    {
        if (player == null)
            player = PhatHienPlayer().transform;
        
        return player;
    }


    public RaycastHit2D PhatHienPlayer()
    { 
        RaycastHit2D hit = 
            Physics2D.Raycast(KiemTraPlayer.position, Vector2.right * huongQuay, khoangCachKiemTraPlayer, NhanDienPlayer | MatDat);

        // Kiểm tra xem tia raycast có va chạm gì không,
        // hoặc nếu có va chạm thì đối tượng va chạm đó không nằm trong layer chỉ định
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        return default;

        return hit;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(KiemTraPlayer.position, new Vector3(KiemTraPlayer.position.x + (huongQuay * khoangCachKiemTraPlayer), KiemTraPlayer.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(KiemTraPlayer.position, new Vector3(KiemTraPlayer.position.x + (huongQuay * KhoangCachTanCong), KiemTraPlayer.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(KiemTraPlayer.position, new Vector3(KiemTraPlayer.position.x + (huongQuay * kcLuiToiThieu), KiemTraPlayer.position.y));
    }


    private void OnEnable()
    {
        Player.KhiPlayerChet += XuLyPlayerChet;
    }

    private void OnDisable()
    {
        Player.KhiPlayerChet -= XuLyPlayerChet;
    }
}
