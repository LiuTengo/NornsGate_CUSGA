using PlayTextSupport;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Text1CharactorManager : MonoBehaviour
{

    public GameObject[] CharacterList;
    public CanvasGroup _canvasGroup;
    public GameObject BlackBack;
    public GameObject Dialogue;
    //public GameObject Video;

    public string SceneName;

    bool alf = false;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener("PlayText.NextDialogue", NextDialogue);
        EventCenter.GetInstance().AddEventListener("PlayText.PeopleA.None", PeopleA);
        EventCenter.GetInstance().AddEventListener("PlayText.PeopleB.None", PeopleB);
        EventCenter.GetInstance().AddEventListener("PlayText.PeopleC.None", PeopleC);
        EventCenter.GetInstance().AddEventListener("PlayText.PeopleX.None", PeopleX);
        EventCenter.GetInstance().AddEventListener("PlayText.End.None", End);
    }
    void NextDialogue()
    {
        Debug.Log("111");
        for (int i = 0; i < CharacterList.Length; i++)
        {
            CharacterList[i].SetActive(false);
        }
    }
    void PeopleA() => CharacterList[0].SetActive(true);
    void PeopleB() => CharacterList[1].SetActive(true);
    void PeopleC() => CharacterList[2].SetActive(true);
    void PeopleX() => CharacterList[3].SetActive(true);
    void End()
    {
        BlackBack.SetActive(true);
        alf = true;

        StartCoroutine(LoadThisScene());
    }

    IEnumerator LoadThisScene()
    {

        //DontDestroyOnLoad(this.gameObject);
        //Video.SetActive(false);
        //StopAllCoroutines();

        EventCenter.GetInstance().RemoveEventListener("PlayText.NextDialogue", NextDialogue);
        EventCenter.GetInstance().RemoveEventListener("PlayText.PeopleA.None", PeopleA);
        EventCenter.GetInstance().RemoveEventListener("PlayText.PeopleB.None", PeopleB);
        EventCenter.GetInstance().RemoveEventListener("PlayText.PeopleC.None", PeopleC);
        EventCenter.GetInstance().RemoveEventListener("PlayText.PeopleX.None", PeopleX);
        EventCenter.GetInstance().RemoveEventListener("PlayText.End.None", End);
        Dialogue.SetActive(false);

        yield return new WaitForSeconds(5f);

        StopAllCoroutines();
        this.gameObject.SetActive(false);

        SceneManager.LoadScene(SceneName);
    }

    // Update is called once per frame
    void Update()
    {
        if(alf)
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, 0.3f);
    }
}
