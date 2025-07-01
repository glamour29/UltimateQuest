using UnityEngine;

public class Enemy_SucKhoe : ThucThe_SucKhoe
{
    // Lấy component Enemy từ GameObject hiện tại
    private Enemy enemy => GetComponent<Enemy>();

    // Gây sát thương cho thực thể này
    public override bool GaySatThuong(float satthuong,float satThuongNguyenTo,LoaiNguyenTo nguyento, Transform KeGaySatThuong)
    {
        bool dabiDanh = base.GaySatThuong(satthuong, satThuongNguyenTo,nguyento, KeGaySatThuong);

        if (dabiDanh == false)
            return false;

        // Nếu kẻ gây sát thương là Player thì chuyển Enemy vào trạng thái đánh nhau
        if (KeGaySatThuong.GetComponent<Player>() != null) 
            enemy.VaoTrangThaiDanhNhau(KeGaySatThuong);

        return true;
      
    }
}
