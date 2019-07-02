using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJController : MonoBehaviour
{
    Vector3 posicionInicial; 
    [SerializeField]
    public GameObject character;
    [SerializeField]
    GameObject CameraObj;

    // Start is called before the first frame update
    void Start()
    {
        posicionInicial = CameraObj.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
