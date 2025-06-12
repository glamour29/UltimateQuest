using System;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour //Định nghĩa một class tên là Player, kế thừa từ MonoBehaviour, để có thể gắn vào GameObject trong Unity.
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("DiChuyen")]
    [SerializeField] private float TocDoDiChuyen = 3.5f;
    [SerializeField] private float LucNhay = 8;
    private float xInput; //là giá trị nhập từ bàn phím, ví dụ -1, 0, hoặc 1 (khi nhấn trái, không nhấn gì, hoặc nhấn phải).
    private bool QuayMatSangPhai = true;

    [Header("VaCham")]
    [SerializeField] private float KhoangCachKiemTraDat;
    [SerializeField] private LayerMask LopMatDat;
    private bool DaChamDat;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); /// Giúp ta điều khiển vật lý như trọng lực, vận tốc cho đối tượng 2D.
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        XuLyVaCham();
        XuLyInput();
        XuLyDiChuyen();
        XuLyHoatAnh();
        XuLyLatHuong();
    }

    private void XuLyHoatAnh()
    {
        anim.SetFloat("vtX", rb.linearVelocity.x);
        // Gửi trạng thái đó vào Animator để chuyển animation
        anim.SetFloat("vtY", rb.linearVelocity.y);
        anim.SetBool("DaChamDat", DaChamDat);
    }

    private void XuLyInput()
    {
        xInput = Input.GetAxisRaw("Horizontal"); //Lấy giá trị điều khiển trục ngang (phím trái/phải hoặc A/D).

        if (Input.GetKeyDown(KeyCode.Space))
            Nhay();
    }

    private void XuLyDiChuyen()
    {
        rb.linearVelocity = new Vector2(xInput * TocDoDiChuyen, rb.linearVelocity.y);//tạo ra một vận tốc mới,trục Y: giữ nguyên rb.linearVelocity.y (để không ảnh hưởng đến nhảy/rơi...)
    }

    private void Nhay()
    {
        if(DaChamDat)
         rb.linearVelocity = new Vector2(rb.linearVelocity.x, LucNhay);

    }
    private void XuLyVaCham()
    {
        // Kiểm tra xem có chạm đất không bằng cách bắn tia xuống dưới
        DaChamDat = Physics2D.Raycast(transform.position,Vector2.down, KhoangCachKiemTraDat, LopMatDat);
    }
    private void XuLyLatHuong()
    {
        //→ nhan vat đang di chuyển sang phải (trục X dương).nhân vật quay mặt sang trái → lật hướng lại
        if (rb.linearVelocity.x > 0 && QuayMatSangPhai == false)
            LatHuong();
        //→ nhan vat đang di chuyển sang trai (trục X am).nhân vật quay mặt sang phai → lật hướng lại
        else if (rb.linearVelocity.x < 0 && QuayMatSangPhai == true)
            LatHuong();

    }

    private void LatHuong()
    {
        transform.Rotate(0, 180, 0); //xoay đối tượng 180 độ theo trục Y, giúp lật trái ↔ phải.
        QuayMatSangPhai = !QuayMatSangPhai; // đảo ngược trạng thái: nếu đang quay phải thì thành quay trái và ngược lại.
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * KhoangCachKiemTraDat);
    }
}

