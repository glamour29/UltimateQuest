using UnityEngine;

public class ThucThe_AnimationTrigger : MonoBehaviour
{
private ThucThe thucthe;
    private ThucThe_Combat thuctheCombat;
    protected virtual void Awake()
    {
        thucthe = GetComponentInParent<ThucThe>();
        thuctheCombat = GetComponentInParent<ThucThe_Combat>();
    }

    private void TriggerHienTai()
    {
        thucthe.AnimationTriggerHienTai();
    }

    private void TanCongTrigger()
    {
        thuctheCombat.ThucHienTanCong();
    }
       
}
