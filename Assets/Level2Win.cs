using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2Win : MonoBehaviour
{

    bool _isWin = false;
    public CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isWin)
        {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, 0.3f);
            StartCoroutine(LoadThisScene());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerPrefs.SetInt("levelreach", 2);
        _isWin = true;
    }

    IEnumerator LoadThisScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LevelMenu");
    }
}
