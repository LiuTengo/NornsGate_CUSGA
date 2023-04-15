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
        //Ĭ��Ϊ1����Ҫÿ��ͨ�غ��1
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
        //���ص�Ŀ�곡������Ҫ�ڶ�Ӧ��ť�¼���λ�ô���дĿ�곡��������
    }

    public void QuitMenu()
    {
        SceneManager.LoadScene("Begining");
    }

}
