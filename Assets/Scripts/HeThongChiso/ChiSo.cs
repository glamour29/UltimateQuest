using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ChiSo
{
    [SerializeField] private float DuLieuNen;
    [SerializeField] private List<BoSuaDoiChiSo> cacTrinhSuaDoi = new List<BoSuaDoiChiSo>();

    private bool canDuocTinhToanLai = true;
    private float duLieuCuoiCung;

    public float layDuLieu()
    {
        if (canDuocTinhToanLai)
        {
            duLieuCuoiCung = layDuLieuCuoiCung();
            canDuocTinhToanLai = false;
        }    

        return duLieuCuoiCung;
    }

    public void themBoSuaDoi(float dulieu, string tainguyen)
    {
        BoSuaDoiChiSo boSuaDoiDeThemVao = new BoSuaDoiChiSo(dulieu, tainguyen);
        cacTrinhSuaDoi.Add(boSuaDoiDeThemVao);
        canDuocTinhToanLai = true;
    }
    public void xoaBoSuaDoi(string tainguyen)
    {
        cacTrinhSuaDoi.RemoveAll(trinhSuaDoi => trinhSuaDoi.tainguyen == tainguyen);
        canDuocTinhToanLai = true;
    }

    private float layDuLieuCuoiCung()
    {
        float duLieuCuoiCung = DuLieuNen;
        foreach (var trinhSuaDoi in cacTrinhSuaDoi)
        {
            duLieuCuoiCung = duLieuCuoiCung + trinhSuaDoi.dulieu;
        }
        return duLieuCuoiCung;
    }
    public void GanGiaTriNen(float giatri) => DuLieuNen = giatri;
}


[System.Serializable]
public class BoSuaDoiChiSo
{
    public float dulieu;
    public string tainguyen;
    public BoSuaDoiChiSo(float dulieu, string tainguyen)
    {
        this.dulieu = dulieu;
        this.tainguyen = tainguyen;
    }

}
