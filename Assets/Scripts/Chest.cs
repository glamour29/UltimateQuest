using UnityEngine;
using UnityEngine.XR;

public class Chest : MonoBehaviour, IBiThuong
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();    // Lấy Rigidbody2D con (dùng để đẩy rương)
    private Animator anim => GetComponentInChildren<Animator>();    // Lấy Animator con (để mở rương)
    private ThucThe_VFX fx => GetComponent<ThucThe_VFX>();    // Lấy hiệu ứng trúng đòn

    [Header("Mở rương")]
    [SerializeField] private Vector2 daylui;

    public void GaySatThuong(float satthuong, Transform KeGaySatThuong)
    {
        fx.ChayHieuUngTrungDon();
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = daylui ;
        rb.angularVelocity = Random.Range(-200f, 200f);

    }
}
