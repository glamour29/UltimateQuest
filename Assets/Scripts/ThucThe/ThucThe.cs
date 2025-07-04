﻿using System;
using System.Collections;
using UnityEngine;

public class ThucThe : MonoBehaviour
{
    public event Action KhiBiLat;


    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public ChiSo_ThucThe chiSo { get; private set; }
    protected StateMachine mayTrangThai;



    private bool quaySangPhai = true;
    public int huongQuay { get; private set; } = 1;

    [Header("Phát hiện va chạm")]
    [SerializeField] protected LayerMask MatDat;
    [SerializeField] private float kcKiemTraDat;
    [SerializeField] private float kcKiemTraTuong;
    [SerializeField] private Transform ktDat;
    [SerializeField] Transform diemTuong;
    [SerializeField] Transform diemTuongPhu;
    public bool daChamDat { get; private set; }
    public bool daChamTuong { get; private set; }

    private bool biDayLui;
    private Coroutine heSoDayLui;
    private Coroutine coroutineLamCham;

    // Hàm Awake được Unity gọi đầu tiên khi GameObject được khởi tạo (trước Start)
    protected virtual void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rb = GetComponentInChildren<Rigidbody2D>();
        chiSo=GetComponent<ChiSo_ThucThe>();

        // Khởi tạo máy trạng thái
        mayTrangThai = new StateMachine();


    }

    protected virtual void Start ()
    {

    }

    // Hàm Update được gọi mỗi khung hình (frame), dùng để xử lý logic thay đổi theo thời gian
    protected virtual void Update()
    {
        XuLyPhatHienVaCham();
        mayTrangThai.CapNhatTrangThai();// => Cho phép mỗi trạng thái tự xử lý logic riêng của nó khi đang hoạt động

    }

    public virtual void ThucTheBiTieuDiet()
    {

    }

    public virtual void lamChamThucThe (float tgian, float heSoLamCham )
    {
        if (coroutineLamCham != null)
            StopCoroutine(coroutineLamCham);

        coroutineLamCham = StartCoroutine(CoroutinelamChamThucThe(tgian, heSoLamCham));
    }

    protected virtual IEnumerator CoroutinelamChamThucThe(float tgian, float heSoLamCham)
    {
        yield return null;
    }


    // Gọi hàm để bắt đầu hiệu ứng đẩy lùi
    public void NhanDaylui(Vector2 daylui, float tgian )
    {
        // Nếu đã có coroutine đẩy lùi đang chạy thì dừng lại
        if (heSoDayLui != null)
            StopCoroutine(heSoDayLui);
        // Bắt đầu coroutine mới để xử lý đẩy lùi
        heSoDayLui = StartCoroutine (CoroutineDayLui(daylui, tgian));
    }

    // Coroutine xử lý hiệu ứng đẩy lùi trong một khoảng thời gian
    private IEnumerator CoroutineDayLui(Vector2 daylui, float tgian)
    {
        biDayLui = true;// Đánh dấu là đang bị đẩy lùi
        rb.linearVelocity = Vector2.zero;// Dừng mọi chuyển động hiện tại

        // 👉 Apply pushback force
        rb.AddForce(daylui, ForceMode2D.Impulse);

        yield return new WaitForSeconds(tgian);// Chờ trong thời gian bị đẩy lùi

        rb.linearVelocity = Vector2.zero;// Đảm bảo không còn di chuyển sau khi hết đẩy lùi
        biDayLui = false;// Kết thúc trạng thái bị đẩy lùi
    }


    public void AnimationTriggerHienTai()
    {
        mayTrangThai.TrangThaiHienTai.AnimationTrigger();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (biDayLui)
            return;


        rb.linearVelocity = new Vector2(xVelocity, yVelocity); // tạo một vector 2 chiều 
        XuLyLatHuong(xVelocity);
    }

    public void XuLyLatHuong(float xVelocity)
    {
        if (xVelocity > 0 && quaySangPhai == false)    // Nếu đang di chuyển sang phải mà nhân vật chưa quay phải → lật lại

            Lat();
        else if (xVelocity < 0 && quaySangPhai)    // Nếu đang di chuyển sang trái mà nhân vật đang quay phải → lật lại

            Lat();
    }

    public void Lat()
    {
        transform.Rotate(0, 180, 0);    // Xoay nhân vật 180 độ quanh trục Y (lật mặt)
        quaySangPhai = !quaySangPhai;    // Đảo ngược trạng thái quay (nếu đang quay phải thì thành quay trái và ngược lại)
        huongQuay = huongQuay * -1;

        KhiBiLat?.Invoke();

    }



    private void XuLyPhatHienVaCham()

    {
        // Tạo một tia raycast bắn xuống dưới từ vị trí của nhân vật
        // Nếu tia này chạm trúng lớp mặt đất (MatDat), thì biến daChamDat = true
        daChamDat = Physics2D.Raycast(ktDat.position, Vector2.down, kcKiemTraDat, MatDat);

        if (diemTuongPhu != null)
        {
        daChamTuong = Physics2D.Raycast(diemTuong.position, Vector2.right * huongQuay, kcKiemTraTuong, MatDat)
                   && Physics2D.Raycast(diemTuongPhu.position, // Từ vị trí kiểm tra tường phụ
                   Vector2.right * huongQuay, // Theo hướng nhân vật đang quay mặt
                   kcKiemTraTuong, MatDat);
        }
        else
        {
            daChamTuong = Physics2D.Raycast(diemTuong.position, Vector2.right * huongQuay, kcKiemTraTuong, MatDat);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(ktDat.position, // Điểm bắt đầu: vị trí hiện tại của nhân vật
            ktDat.position + new Vector3(0, -kcKiemTraDat));// Vẽ một đường thẳng từ vị trí hiện tại đi xuống dưới một đoạn bằng KhoangCachKiemTraDat

        Gizmos.DrawLine(diemTuong.position,
            diemTuong.position + new Vector3(kcKiemTraTuong, huongQuay * 0));// Điểm kết thúc: lùi/tiến một đoạn theo trục X (tùy hướng quay)

        if (diemTuongPhu != null) 

        Gizmos.DrawLine(diemTuongPhu.position,
            diemTuongPhu.position + new Vector3(kcKiemTraTuong * huongQuay, 0));
    }
    

}
