using UnityEngine;

public class UI_ThanhMau_Mini : MonoBehaviour
{

    private ThucThe thucthe;

    private void Awake()
    {
        thucthe= GetComponentInParent<ThucThe>();

    }
    private void OnEnable()
    {
        thucthe.KhiBiLat += XuLyLat;
    }

    // Update is called once per frame
    private void OnDisable()
    {
        thucthe.KhiBiLat -= XuLyLat;
    }

    private void XuLyLat() => transform.rotation = Quaternion.identity;
}
