using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour
{

    public GameObject Video;
    //public GameObject BlackBack;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedDialogue(62f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DelayedDialogue(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //BlackBack.SetActive(false);
        Video.SetActive(false);
        Destroy(this.gameObject);
    }
}
