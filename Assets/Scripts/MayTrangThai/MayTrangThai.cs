using UnityEngine;

public class StateMachine
{
    // Thuộc tính trạng thái hiện tại của thực thể (chỉ có thể đọc từ bên ngoài)
    public TrangThaiThucThe TrangThaiHienTai {  get; private set; }
    public bool ChuyenTrangThai =true ;

    // Hàm khởi tạo trạng thái ban đầu cho máy trạng thái
    public void KhoiTao(TrangThaiThucThe BatDauTrangThai)
    {
        TrangThaiHienTai = BatDauTrangThai;// Gán trạng thái hiện tại là trạng thái bắt đầu
        TrangThaiHienTai.Enter(); // Gọi hàm Enter của trạng thái bắt đầu
    }

    public void thayDoiTrangThai (TrangThaiThucThe TrangThaiMoi)
    {
        if (ChuyenTrangThai == false)
            return;

        TrangThaiHienTai.Exit();// Gọi hàm Exit để thoát trạng thái hiện tại
        TrangThaiHienTai = TrangThaiMoi;// Cập nhật trạng thái hiện tại thành trạng thái mới
        TrangThaiHienTai.Enter();// Gọi hàm Enter của trạng thái mới
    }
    public void CapNhatTrangThai()
    {
        TrangThaiHienTai?.Update(); // Nếu trạng thái hiện tại khác null thì gọi hàm Update
    }

    public void TatMayTrangThai() => ChuyenTrangThai = false;
}
