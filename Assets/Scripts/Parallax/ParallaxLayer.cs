using UnityEngine;

[System.Serializable]
public class ParallaxLayer 
{
    [SerializeField] private Transform background;// Transform của lớp nền
    [SerializeField] private float HeSoparallax;// Hệ số thị sai – xác định tốc độ di chuyển của nền so với camera
    [SerializeField] private float dolechChieuRongAnh =10 ;

    private float imageFullChieuRong;
    private float imageNuaChieuRong;

    public void tinhChieuRongAnh ()
    {
        imageFullChieuRong = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageNuaChieuRong = imageFullChieuRong / 2;
    }

    public void DiChuyen(float KhoangCachDiChuyen)
    {
        // Vector3.right là (1, 0, 0) → hướng sang phải trên trục X
        // Nhân với khoảng cách di chuyển * hệ số parallax để tính độ lệch của nền
        // Sau đó cộng vào vị trí hiện tại của nền để nó di chuyển
        background.position += Vector3.right * (KhoangCachDiChuyen * HeSoparallax);
    }

    public void nenLapLai (float cameraCanhTrai, float CameraCanhPhai)
    {
        //tinh toạ độ mép trái và phải
        float imageCanhPhai = (background.position.x + imageNuaChieuRong) - dolechChieuRongAnh;
        float imageCanhTrai = (background.position.x - imageNuaChieuRong) + dolechChieuRongAnh;

        // Nếu mép phải của nền đã đi ra khỏi màn hình bên trái của camera
        if (imageCanhPhai < cameraCanhTrai)
            background.position += Vector3.right * imageFullChieuRong;// Di chuyển nền sang bên phải để nó lặp lại
        else if (imageCanhTrai > CameraCanhPhai)
            background.position += Vector3.right * -imageFullChieuRong;
    }

}
