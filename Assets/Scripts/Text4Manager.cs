using PlayTextSupport;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Text4Manager : MonoBehaviour
{
    public GameObject Dialogue;
    public GameObject[] CharacterList;
    public CanvasGroup _canvasGroup;
    public GameObject BlackBack;

    public string SceneName;

    bool alf = false;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("PlayText.NextDialogue", NextDialogue);
        EventCenter.GetInstance().AddEventListener("PlayText.Aida.None", PeopleNv);
        EventCenter.GetInstance().AddEventListener("PlayText.GeMaiSi.None", PeopleNan);
        EventCenter.GetInstance().AddEventListener("PlayText.PeopleX.None", PeopleX);
        EventCenter.GetInstance().AddEventListener("PlayText.End.None", End);
    }
    void PeopleNv() => CharacterList[0].SetActive(true);
    void PeopleNan() => CharacterList[1].SetActive(true);
    void PeopleX() => CharacterList[2].SetActive(true);
    void NextDialogue()
    {
        Debug.Log("111");
        for (int i = 0; i < CharacterList.Length; i++)
        {
            CharacterList[i].SetActive(false);
        }
    }
    void End()
    {
        BlackBack.SetActive(true);
        alf = true;
        StartCoroutine(LoadThisScene());
    }
    IEnumerator LoadThisScene()
    {
        EventCenter.GetInstance().RemoveEventListener("PlayText.NextDialogue", NextDialogue);
        EventCenter.GetInstance().RemoveEventListener("PlayText.Aida.None", PeopleNv);
        EventCenter.GetInstance().RemoveEventListener("PlayText.GeMaiSi.None", PeopleNan);
        EventCenter.GetInstance().RemoveEventListener("PlayText.PeopleX.None", PeopleX);
        EventCenter.GetInstance().RemoveEventListener("PlayText.End.None", End);
        //StopAllCoroutines();
        Dialogue.SetActive(false);

        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if (alf)
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, 0.3f);
    }
}
