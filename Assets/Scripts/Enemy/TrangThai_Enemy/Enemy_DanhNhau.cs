using UnityEngine;

public class Enemy_DanhNhau : TrangThaiEnemy
{
    private Transform player;
    private float lanCuoiTrongGiaoChien;
    public Enemy_DanhNhau(Enemy enemy, StateMachine mayTrangThai, string TenBoolanim) : base(enemy, mayTrangThai, TenBoolanim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CapNhatThoiGianDanhNhau();

        if (player == null)
            player = enemy.LayThamChieuPlayer();

        if (CoNenRutLui())
        {
            // Gán vận tốc để enemy lùi lại khỏi player
            rb.linearVelocity = new Vector2(
                enemy.tocDoLui.x * -KhoangCachDenPlayer(),// Trục X: lùi lại theo khoảng cách đến player (nhân âm để đi ngược)
                enemy.tocDoLui.y);// Trục Y: giữ nguyên hoặc đặt giá trị lùi theo phương thẳng đứng nếu cần
             enemy.XuLyLatHuong(HuongDenPlayer(  ));
        }

    }

    public override void Update()
    {
        base.Update();

        if (enemy.PhatHienPlayer() == true)
        CapNhatThoiGianDanhNhau();

        if (ThoiGianDanhNhauKetThuc())
            mayTrangThai.thayDoiTrangThai(enemy.DungYen);

        if (trongPhamViTanCong() && enemy.PhatHienPlayer() == true )
            mayTrangThai.thayDoiTrangThai(enemy.TanCong);
        else
            // Di chuyển enemy về phía player với tốc độ tấn công, giữ nguyên tốc độ Y
            enemy.SetVelocity(enemy.tocDoDanhNhau * HuongDenPlayer(), rb.linearVelocity.y);
    }

    private void CapNhatThoiGianDanhNhau() => lanCuoiTrongGiaoChien = Time.time;

    private bool ThoiGianDanhNhauKetThuc() => Time.time > lanCuoiTrongGiaoChien + enemy.ThoiGianGiaoChien;

    private bool trongPhamViTanCong() => KhoangCachDenPlayer() < enemy.KhoangCachTanCong;

    private bool CoNenRutLui() => KhoangCachDenPlayer() < enemy.kcLuiToiThieu;

    
        //return KhoangCachDenPlayer() < enemy.KhoangCachTanCong;
    
    private float KhoangCachDenPlayer ()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    // Trả về 1 nếu player ở bên phải, -1 nếu ở bên trái, 0 nếu không có player
    private int HuongDenPlayer ()
    {
        if (player == null)
            return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
