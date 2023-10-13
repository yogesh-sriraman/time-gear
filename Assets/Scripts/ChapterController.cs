using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChapterController : MonoBehaviour
{
    public TextMeshProUGUI TitleText;
    public Image artSprite;
    public List<GameObject> nodes;
 
    // Start is called before the first frame update
    void Start()
    {
        AddListenerToButtons();
    }

    public void AddListenerToButtons()
    {
        if(nodes.Count>0)
        {
            foreach (var item in nodes)
            {
                Button button = item.GetComponent<Button>();
                //Debug.Log(item.name);
                if (button!=null)
                {
                    button.onClick.AddListener(()=>LoadLevel(item.name));
                    button.interactable = false;
                    Transform orbTransform = button.transform.parent.GetChild(0);
                    int totalStarObjs = orbTransform.childCount;
                    button.transform.GetChild(0).gameObject.SetActive(false);
                    orbTransform.gameObject.SetActive(false);
                    int currChapterID = GameController.instance.playerLevelData[item.name].chapterID;
                    bool isLevelUnlocked = GameController.instance.playerLevelData[item.name].isUnlocked;
                    bool isChapterUnlocked = GameController.instance.playerChapterData[currChapterID].isUnlocked;
                    int numOfOrbs = GameController.instance.playerLevelData[item.name].numberOfOrbs;
                    for(int j = 0; j < totalStarObjs; j++)
                    {
                        Image fillImg = orbTransform.GetChild(j).GetChild(0).GetComponent<Image>();
                        fillImg.fillAmount = 0;
                        if (j < numOfOrbs)
                        {
                            fillImg.fillAmount = 1;
                        }
                    }
                    if (isChapterUnlocked && isLevelUnlocked)
                    {
                        button.interactable = true;
                        button.transform.GetChild(0).gameObject.SetActive(true);
                        orbTransform.gameObject.SetActive(true);
                    }

                    LevelController.instance.AddButtons(button);
                }
            }
        }
    }

    public void LoadLevel(string LevelName)
    {
        Debug.Log("Loading scene "+LevelName);
        GameController.instance.loadLevel(LevelName);
        AudioManager.instance.PlaySound("Button Click1");
    }

    public void RemoveListenerFromButtons()
    {
        if (nodes.Count > 0)
        {
            foreach (var item in nodes)
            {
                Button button = item.GetComponent<Button>();
                if (button != null)
                {
                    button.onClick.RemoveListener(() => LoadLevel(TitleText.text + item.name));
                    LevelController.instance.RemoveButton(button);
                }
            }
        }
    }


}
