using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float viTrilastCameraX;
    private float motNuaChieuRongCamera;

    [SerializeField] private ParallaxLayer[] backgroundLayers;


    private void Awake()
    {
        mainCamera = Camera.main;
        motNuaChieuRongCamera = mainCamera.orthographicSize * mainCamera.aspect;
        tinhChieuDaiAnh();
    }

    private void FixedUpdate()
    {
        float viTriCameraXHientai = mainCamera.transform.position.x; // Lấy vị trí X hiện tại của camera

        // Tính khoảng cách mà camera đã di chuyển kể từ lần trước (mới - cũ)
        float KhoangCachDeDiChuyen = viTriCameraXHientai - viTrilastCameraX;

        // Cập nhật lại vị trí X cũ thành vị trí hiện tại, để dùng trong lần sau
        viTrilastCameraX = viTriCameraXHientai;

        float cameraCanhTrai = viTriCameraXHientai - motNuaChieuRongCamera;
        float cameraCanhPhai = viTriCameraXHientai + motNuaChieuRongCamera;


        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.DiChuyen(KhoangCachDeDiChuyen);// Di chuyển nền theo camera
            layer.nenLapLai(cameraCanhTrai, cameraCanhPhai);// Nếu nền đi ra khỏi tầm nhìn thì lặp lại nền
        }
    }

    private void tinhChieuDaiAnh()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
            layer.tinhChieuRongAnh();
    }

}
