using System.Collections;
using UnityEngine;

public class ThucThe_VFX : MonoBehaviour
{

    private SpriteRenderer sr;

    [Header("Hiệu ứng trúng đòn")]
    [SerializeField] private Material HieuUngTrungDon;
    [SerializeField] private float tgHieuUngTrungDon = .2f;
    private Material ChatLieuGoc;
    private Coroutine CoHieuUngTrungDon;


    private void Awake()
    {
       sr = GetComponentInChildren<SpriteRenderer>(); // Lấy SpriteRenderer từ đối tượng con

        ChatLieuGoc = sr.material;// Lưu lại material ban đầu để sau này khôi phục
    }

    public void ChayHieuUngTrungDon()
    {

        // Nếu hiệu ứng đang chạy, dừng lại để tránh đè nhau
        if ( CoHieuUngTrungDon != null)
            StopCoroutine(CoHieuUngTrungDon );

        // Bắt đầu hiệu ứng trúng đòn
        CoHieuUngTrungDon = StartCoroutine (ChayHieuUng());
    }
    private IEnumerator ChayHieuUng()
    {
        sr.material = HieuUngTrungDon;// Gán material hiệu ứng

        yield return new WaitForSeconds (tgHieuUngTrungDon);// Chờ một khoảng thời gian
        sr.material = ChatLieuGoc;// Trả lại material gốc
    }    
}
