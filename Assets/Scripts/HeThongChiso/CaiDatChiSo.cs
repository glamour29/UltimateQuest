using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Stat Setup", fileName = " Default Stat Setup ")]

public class CaiDatChiSo : ScriptableObject
{
    [Header("Tài Nguyên")]
    public float mauToiDa = 100;
    public float hoiMau;

    [Header("Tấn Công - Vật Lý")]
    public float tocDoDanh = 1;
    public float satThuong = 10;
    public float tiLeChiMang;
    public float sucManhChiMang = 150;
    public float giamGiap;

    [Header("Tấn Công - Nguyên Tố")]
    public float satThuongLua;
    public float satThuongBang;
    public float satThuongSet;

    [Header("Phòng Thủ - Vật Lý")]
    public float giap;
    public float neTranh;

    [Header("Phòng Thủ - Nguyên Tố")]
    public float khangLua;
    public float khangBang;
    public float khangSet;

    [Header("Chỉ số chính")]
    public float sucManh;         
    public float nhanhNhen;       
    public float triTue;          
    public float theLuc;          

}
