using UnityEngine;

public class Enemy_VFX : ThucThe_VFX
{
    [Header("Tấn công phản đòn")]
    [SerializeField] private GameObject CanhBaoTanCong;


    public void BatCanhBaoTanCong(bool enable)
    {
        if (CanhBaoTanCong == null)
            return;
        
        CanhBaoTanCong.SetActive(enable);
    }


}
