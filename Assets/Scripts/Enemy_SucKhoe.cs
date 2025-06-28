using UnityEngine;

public class Enemy_SucKhoe : ThucThe_SucKhoe
{
    // Lấy component Enemy từ GameObject hiện tại
    private Enemy enemy => GetComponent<Enemy>();

    // Gây sát thương cho thực thể này
    public override void GaySatThuong(float satthuong, Transform KeGaySatThuong)
    {
        base.GaySatThuong(satthuong, KeGaySatThuong);

        if (DaChet)
            return;

        // Nếu kẻ gây sát thương là Player thì chuyển Enemy vào trạng thái đánh nhau
        if (KeGaySatThuong.GetComponent<Player>() != null) 
            enemy.VaoTrangThaiDanhNhau(KeGaySatThuong);

      
    }
}
