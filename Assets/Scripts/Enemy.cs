using UnityEngine;

public class Enemy : ThucThe
{
    public Enemy_DungYen DungYen;
    public Enemy_DiChuyen DiChuyen;


    [Header("Di chuyển")]
    public float DungYenTime = 2;
    public float tocDoDiChuyen = 1.4f;
    [Range(0, 2)]
    public float heSoTocDoDiChuyen = 1;
}
