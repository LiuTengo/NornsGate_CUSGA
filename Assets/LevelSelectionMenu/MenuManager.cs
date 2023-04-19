using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button[] levels;
    public int levelReach;
    private int formerLevelReach = -1;

    private void Start()
    {



    }

    private void Update()
    {
        if(levelReach != formerLevelReach)
        {
            //Ĭ��Ϊ1����Ҫÿ��ͨ�غ��1
            for (int i = 0; i < levels.Length; i++)
            {
                levels[i].gameObject.SetActive(false);
                if (i <= levelReach - 1)
                {
                    levels[i].gameObject.SetActive(true);
                }
            }
            formerLevelReach = levelReach;
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
