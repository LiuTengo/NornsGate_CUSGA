using PlayTextSupport;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Text2Manager : MonoBehaviour
{
    public GameObject Dialogue;
    // Start is called before the first frame update
    void Start()
    {
        //Dialogue.SetActive(true);
        EventCenter.GetInstance().AddEventListener("PlayText.End.None", End);
    }

    void End()
    {
        Dialogue.SetActive(false);
        EventCenter.GetInstance().RemoveEventListener("PlayText.End.None", End);
        Dialogue.SetActive(false);
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
