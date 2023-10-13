using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;

public class LevelController : MonoBehaviour
{
    public List<LevelTemplate> levels;
    public GameObject chapterTemplate;
    public Transform parentTransfrom;
    public HorizontalScrollSnap hss;
    public Image bgImage;
    public Transform settingsPanel;
    public Button settingsBtn;

    //Levelcontroller is keeping track of all buttons in each level. This will change once more levels are added. 
    private List<Button> listButtons;

    private ChapterController chapterController;

    private static LevelController _instance;
    public static LevelController instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        listButtons = new List<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        listButtons = new List<Button>();
        CreateAndLoadLevels();
    }

    void CreateAndLoadLevels()
    {
        if(levels.Count>0)
        {
            foreach (var levelItem in levels)
            {
                GameObject chapterObject = Instantiate(chapterTemplate,Vector3.zero,Quaternion.identity);
                chapterObject.transform.SetParent(parentTransfrom);
                chapterObject.GetComponent<ChapterController>().TitleText.text = levelItem.levelName;
                chapterObject.GetComponent<ChapterController>().artSprite.sprite = levelItem.icon;

                GameObject nodes = Instantiate(levelItem.Nodes, levelItem.Nodes.transform.position, levelItem.Nodes.transform.rotation);
                List<GameObject> subLevels = GetSubLevels(levelItem.levelName, nodes);
            
                nodes.name = levelItem.levelName+"_Nodes";
                nodes.transform.SetParent(chapterObject.transform);
                nodes.transform.localScale = Vector3.one;

              
                chapterObject.GetComponent<ChapterController>().nodes = subLevels;
                chapterObject.name=levelItem.levelName;
                chapterObject.transform.localScale = Vector3.one;

                if(levelItem.animModel != null)
                {
                    GameObject imgObj = Instantiate(levelItem.animModel, chapterObject.GetComponent<ChapterController>().artSprite.transform);
                    imgObj.transform.localPosition = levelItem.animPos;
                }
            }
            hss.enabled = true;//enabling horizontal scrollbar so that the created tiles will get added 
        }

    }
    List<GameObject> GetSubLevels(string levelName,GameObject templateNodes)
    {
        int i = 0;
        List<GameObject> nodesList = new List<GameObject>();
        Button[] allChilds = templateNodes.GetComponentsInChildren<Button>();
        foreach (Button item in allChilds)
        {
            item.gameObject.name = levelName + "_" + ++i;
            item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
            nodesList.Add(item.gameObject);
        }
        return nodesList;
    }

    public void PlaySwipeSound()
    {
        AudioManager.instance.PlaySound("Slide");
    }
    #region Remove Later
    public void ChangeBackgroundColor()
    {
        bgImage.color = GetLevelThemeColor(hss.CurrentPage);
    }
    
    Color GetLevelThemeColor(int levelNumber)
    {
        Color color = Color.white;
        if(levelNumber>=0&&levelNumber<=levels.Count)
        {
            color = levels[levelNumber].backgroundColor;
        }
        return color;
    }

    public void AddButtons(Button button)
    {
        listButtons.Add(button);
    }
    public void RemoveButton(Button button)
    {
        listButtons.Remove(button);
    }

    public Button GetButtonByName(string levelname)
    {
        foreach (var item in listButtons)
        {
            if(item.name.Equals(levelname))
            {
                return item;
            }
        }
        return null;
    }
    #endregion
}
