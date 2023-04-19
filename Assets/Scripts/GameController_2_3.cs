using UnityEngine;

public class GameController_2_3 : MonoBehaviour
{

    public bool IsVertical;
    public float RotateSpeed;
    public GameObject _revolution;
    public Transform _revolutionPoint;

    public Teleport _teleport;
    public Vector3 _another;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsVertical)
        {
            Revolution();
            _teleport.destination = _another;
        }
    }

    void Revolution()
    {

        Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
        float angleDiff = Quaternion.Angle(_revolution.transform.rotation, targetRotation);

        if (angleDiff > .1f)
        {
            Quaternion newRotation = Quaternion.RotateTowards(_revolution.transform.rotation, targetRotation, RotateSpeed * Time.deltaTime);
            _revolution.transform.RotateAround(_revolutionPoint.position, -Vector3.up, RotateSpeed * Time.deltaTime);
        }
        else
        {
            _revolution.transform.rotation = targetRotation;
        }



    }
}
