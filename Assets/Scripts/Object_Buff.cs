using System.Collections;
using UnityEngine;

[System.Serializable]
public class Buff
{
    public LoaiChiSo Loai;
    public float giaTri;

}

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    private ChiSo_ThucThe cacChiSoCanSuaDoi;

    [Header("Thông tin buff")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string tenBuff;
    [SerializeField] private float tgianKeoDaiBuff = 4;
    [SerializeField] private bool coTheSuDungKhong = true;

    [Header("Bay trên không")]
    [SerializeField] private float tocDo = 1f;
    [SerializeField] private float phamVi = .1f;
    private Vector3 viTriBatDau;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        viTriBatDau = transform.position;
    }

    private void Update()
    {
        float yDoLech = Mathf.Sin(Time.time * tocDo) * phamVi;
        transform.position = viTriBatDau + new Vector3(0, yDoLech);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coTheSuDungKhong == false)
            return;

        cacChiSoCanSuaDoi = collision.GetComponent<ChiSo_ThucThe>();
        StartCoroutine(CoroutineBuff(tgianKeoDaiBuff));
    }
    private IEnumerator CoroutineBuff(float thoiGianKeoDai)
    {
        coTheSuDungKhong = false;
        sr.color = Color.clear;
        ApDungBuff(true);

        yield return new WaitForSeconds(thoiGianKeoDai);
        ApDungBuff(false);
        Destroy(gameObject);
    }

    private void ApDungBuff(bool apdung)
    {

        foreach (var buff in buffs)
        {
            if (apdung)
                cacChiSoCanSuaDoi.layChiSoTheoLoai(buff.Loai).themBoSuaDoi(buff.giaTri, tenBuff);
            else
                cacChiSoCanSuaDoi.layChiSoTheoLoai(buff.Loai).xoaBoSuaDoi(tenBuff);
        }
    }
}
