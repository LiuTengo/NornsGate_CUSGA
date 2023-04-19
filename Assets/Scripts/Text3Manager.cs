using PlayTextSupport;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Text3Manager : MonoBehaviour
{

    public GameObject Dialogue;
    bool _isWin = false;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("PlayText.End.None", End);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void End()
    {
        if(_isWin)
            Dialogue.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Dialogue.SetActive(true);
        _isWin = true;

    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Dialogue.SetActive(false);
        EventCenter.GetInstance().RemoveEventListener("PlayText.End.None", End);
        this.gameObject.SetActive(false);
        _isWin = true;
    }
}
