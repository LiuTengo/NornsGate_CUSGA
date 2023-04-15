using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button[] levels;
    public int levelReach;

    private void Start()
    {
        levelReach = PlayerPrefs.GetInt("levelReach", 1);
        //默认为1，需要每关通关后加1
        for(int i = 0; i < levels.Length; i++)
        {
            levels[i].gameObject.SetActive(false);
            if (i <= levelReach-1)
            {
                levels[i].gameObject.SetActive(true);
            }
        }
    }

    public void LoadLevelScene(string _LevelName)
    {
        //Debug.Log(_LevelName);
        SceneManager.LoadScene(_LevelName);
        //加载到目标场景，需要在对应按钮事件的位置处填写目标场景的名称
    }

    public void QuitMenu()
    {
        SceneManager.LoadScene("Begining");
    }

}
