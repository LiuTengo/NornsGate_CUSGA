using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginningController : MonoBehaviour
{

    [SerializeField] Button _startBtn;
    [SerializeField] Button _informationBtn;
    [SerializeField] Button _quitBtn;
    [SerializeField] Button _memberQuitBtn;

    public CanvasGroup _canvasGroup;
    public GameObject _canvasRaw;
    public GameObject _memberRaw;

    public string StartSceneName;
    
    bool _isStarted = false;


    private void Awake()
    {
        _startBtn.onClick.AddListener(() =>
        {
            _canvasRaw.SetActive(true);
            _isStarted = true;
            
            StartCoroutine(LoadThisScene());
        });
        _informationBtn.onClick.AddListener(() =>
        {
            _memberRaw.SetActive(true);
        });
        _quitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        _memberQuitBtn.onClick.AddListener(() =>
        {
            _memberRaw.SetActive(false);
        });
    }
    IEnumerator LoadThisScene()
    {
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene(StartSceneName);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isStarted)
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, 0.04f);
    }
}
