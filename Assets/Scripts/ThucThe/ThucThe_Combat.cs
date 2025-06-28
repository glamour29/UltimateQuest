using UnityEngine;

public class ThucThe_Combat : MonoBehaviour
{
    public float satthuong = 10;

    [Header("Mục tiêu")]
    [SerializeField] private Transform ktMucTieu;
    [SerializeField] private float banKinhKiemTraMucTieu;
    [SerializeField] private LayerMask XacDinhMucTieu;

    public void ThucHienTanCong()
    {
        LayVaCham();
        foreach (var target in LayVaCham())
        {
            IBiThuong bithuong = target.GetComponent<IBiThuong>();
            bithuong?.GaySatThuong(satthuong,transform);

        }
    }

    protected Collider2D[] LayVaCham()
    {
        return  Physics2D.OverlapCircleAll(ktMucTieu.position, banKinhKiemTraMucTieu, XacDinhMucTieu);

}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(ktMucTieu.position, banKinhKiemTraMucTieu);
    }
}
