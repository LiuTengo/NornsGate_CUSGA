using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textLabel;
    public Image CharacterL;
    public Image CharacterM;
    public Image CharacterR;
    
    [Header("Sprite and Layer")]
    // A-旁白 B-葛麦斯 C-罗莎琳德 D-艾达 E—芙芙 F—？？？
    public Sprite aSprite;
    public Sprite blSprite;
    public Sprite brSprite;
    public Sprite clSprite;
    public Sprite crSprite;
    public Sprite dlSprite;
    public Sprite drSprite;
    public Sprite elSprite;
    public Sprite erSprite;
    public Sprite fSprite;
    // Sprite层级
    public int hierarchyLayer;

    [Header("Text")]
    public TextAsset textFile;
    public int index;
    public bool textFinished;
    public float textSpeed = .2f;
    List<string> textList = new List<string>();

    [Header("VFX")] 
    public SpriteVFXController spriteVFXController;

    // Start is called before the first frame update
    void Awake()
    {
        GetTextFormFile(textFile);
        textFinished = true;
        index = 0;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
            // 写场景转换，或者连接场景转换的脚本。
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {

            if (!textFinished)
            {
                textLabel.text = textList[index];
                index++;
                textFinished = true;
            }
            else
            {
                StartCoroutine(SetTextUI());
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            spriteVFXController.ClearSpriteVFX(CharacterR);
        }
    }

    
    private void OnEnable()
    {
        textLabel.text = textList[index];
        index++;
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineDate = file.text.Split("\n");

        foreach (var line in lineDate)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        // 出人物图的部分 直接显示空格
        textLabel.text = " ";

        switch (textList[index].Trim())
        {
            //case "Clear":
            // 可以定义一些新的sprite操作 有需求再说
                
            case "A":
                CharacterL.sprite = aSprite;
                CharacterM.sprite = aSprite;
                CharacterR.sprite = aSprite;
                spriteVFXController.ClearAllSpritesVFX();
                index++;
                break;
            case "BL":
                CharacterL.sprite = blSprite;
                CharacterL.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterL);
                spriteVFXController.UseSpriteVFX(CharacterR,"strongtint");
                index++;
                break;
            case "BR":
                CharacterR.sprite = brSprite;
                CharacterR.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterR);
                spriteVFXController.UseSpriteVFX(CharacterL,"strongtint");
                index++;
                break;
            case "CL":
                CharacterL.sprite = clSprite;
                CharacterL.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterL);
                spriteVFXController.UseSpriteVFX(CharacterR,"strongtint");
                index++;
                break;
            case "CR":
                CharacterR.sprite = crSprite;
                CharacterR.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterR);
                spriteVFXController.UseSpriteVFX(CharacterL,"strongtint");
                index++;
                break;
            case "DL":
                CharacterL.sprite = dlSprite;
                CharacterL.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterL);
                spriteVFXController.UseSpriteVFX(CharacterR,"strongtint");
                index++;
                break;
            case "DR":
                CharacterR.sprite = drSprite;
                CharacterR.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterR);
                spriteVFXController.UseSpriteVFX(CharacterL,"strongtint");
                index++;
                break;
            case "EL":
                CharacterL.sprite = elSprite;
                CharacterL.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterL);
                spriteVFXController.UseSpriteVFX(CharacterR,"strongtint");
                index++;
                break;
            case "ER":
                CharacterR.sprite = erSprite;
                CharacterR.transform.SetSiblingIndex(hierarchyLayer);
                spriteVFXController.ClearSpriteVFX(CharacterR);
                spriteVFXController.UseSpriteVFX(CharacterL,"strongtint");
                index++;
                break;
            case "F":
                CharacterL.sprite = fSprite;
                CharacterM.sprite = fSprite;
                CharacterR.sprite = fSprite;
                spriteVFXController.ClearAllSpritesVFX();
                index++;
                break;
        }

        for (int i = 0; i < textList[index].Length; i++)
        {

            textLabel.text += textList[index][i];

            yield return new WaitForSeconds(textSpeed);
        }

        if (textFinished != true)
        {
            textFinished = true;
            index++;
        }
    }

    public void Skip()
    {
        index = textList.Count - 3;
    }
}
