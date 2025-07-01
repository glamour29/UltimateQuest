using UnityEngine;

public class VFX_AutoController : MonoBehaviour
{
    [SerializeField] private bool TuHuy = true;
    [SerializeField] private float TuHuyDelay = 1;
    [Space]
    [SerializeField] private bool DoLechRandom = true;
    [SerializeField] private bool XoayRandom = true;

    [Header("Vị trí xoay ngẫu nhiên")]
    [SerializeField] private float minXoay = 0;
    [SerializeField] private float maxXoay = 360;


    [Header("Vị trí ngẫu nhiên")]
    [SerializeField] private float LechXMin = -.3f;
    [SerializeField] private float LechXMax = .3f;
    [Space]
    [SerializeField] private float LechYMin = -.3f;
    [SerializeField] private float LechYMax = .3f;



    private void Start()
    {
        ApDungLechRandom();
        ApDungXoayRandom();

        if(TuHuy)
        Destroy (gameObject,TuHuyDelay);
    }

    private void ApDungLechRandom()
    {
        if (DoLechRandom == false)
            return;

        float XLech = Random.Range(LechXMin, LechXMax);
        float YLech = Random.Range(LechYMin, LechYMax);

        transform.position = transform.position + new Vector3(XLech, YLech);
    }

    private void ApDungXoayRandom()
    {
        if (XoayRandom == false)
            return;

        float ZXoay = Random.Range(minXoay,maxXoay);
            transform.Rotate(0,0,ZXoay);

    }
}
