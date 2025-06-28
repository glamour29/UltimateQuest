using UnityEngine;

public class Player_Combat : ThucThe_Combat
{
    [Header("Phản đòn")]
    [SerializeField] private float HoiSucSauPhanDon = .1f;

  public bool ThucHienPhanDon()
    {
        bool daPhanDon = false;

        foreach (var target in LayVaCham())
        {
            IBiPhanDon phandon = target.GetComponent<IBiPhanDon>();

            if (phandon == null)
                continue;

            if (phandon.CoTheDaBiPhanDon)
            {
                phandon.XuLyPhanDon();
                daPhanDon = true;
            }
        }
        return daPhanDon;
    }

    public float tgHoiPhanDon() => HoiSucSauPhanDon;
}
