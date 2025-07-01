using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Thông tin buff")]
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
       transform.position = viTriBatDau+new Vector3(0,yDoLech);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (coTheSuDungKhong == false)
            return;
        StartCoroutine(CoroutineBuff(tgianKeoDaiBuff));
    }
    private IEnumerator CoroutineBuff(float thoiGianKeoDai)
    {
        coTheSuDungKhong = false;
        sr.color = Color.clear;
        Debug.Log("buff is applied for : " + thoiGianKeoDai + " seconds ");

        yield return new WaitForSeconds(thoiGianKeoDai);

        Debug.Log("buff is removed");

        Destroy(gameObject);
    }
}
