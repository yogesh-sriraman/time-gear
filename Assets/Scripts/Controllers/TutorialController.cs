using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public List<TutorialInfo> tutorialData = new List<TutorialInfo>();
    public Image spotLight;

    public GameObject arrowHolder;
    public GameObject arrow;
    public TMP_Text speechText;

    //public Button nextBtn;

    private Sprite spotLightSprite;
    private Texture2D spotLightTexture;
    private Rect lastDrawnTextureSize;
    private TutorialInfo.Shapes lastDrawnShape;

    public TutorialInfo.Shapes shape = TutorialInfo.Shapes.RECTANGLE;

    public TutorialInfo tutorialInfo = null;
    public int currentTutId;

    private Dictionary<int, TutorialInfo> tutorialDict = new Dictionary<int, TutorialInfo>();

    private void Awake()
    {
        _instance = this;
        hideTutorial();
        spotLight.alphaHitTestMinimumThreshold = 0.1f;
    }
    protected void Start()
    {

        //StartTutorial();
    }
    private void drawTransparentTexture()
    {
        Canvas tutorialCanvas = spotLight.GetComponentInParent<Canvas>();
        int textureWidth = Screen.currentResolution.width;
        int textureHeight = Screen.currentResolution.height;
        if (tutorialCanvas != null)
        {
            textureWidth = (int)(tutorialCanvas.transform as RectTransform).sizeDelta.x;
            textureHeight = (int)(tutorialCanvas.transform as RectTransform).sizeDelta.y;
        }

        spotLightTexture = new Texture2D(textureWidth, textureHeight);
        Color color = new Color(0, 0, 0);

        for (int r = 0; r < spotLightTexture.width; r++)
        {
            for (int c = 0; c < spotLightTexture.height; c++)
            {
                color.a = 1.0f;
                spotLightTexture.SetPixel(r, c, color);
            }
        }
        spotLightTexture.Apply();
    }

    #region Singleton
    private static TutorialController _instance;
    public static TutorialController instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            return null;
        }
        #endregion
    }

    public void init()
    {
        drawTransparentTexture();

        //nextBtn.onClick.AddListener(gotoNextTutorial);
    }
    private void gotoNextTutorial()
    {
        if (tutorialInfo != null)
        {
            if (tutorialInfo.next > 0)
                playTutorial(tutorialData[tutorialInfo.next].uid);
        }
    }

    public void playTutorial(string tutorialUID)
    {
        currentTutId = getIDBasedOnUID(tutorialUID);
        if (currentTutId >= 0)
        {
            tutorialInfo = tutorialData[currentTutId];
            //if (tutorialInfo.next > 0 || (tutorialInfo.completion != null && tutorialInfo.completion != ""))
            //    nextBtn.gameObject.SetActive(true);
            //else
            //    nextBtn.gameObject.SetActive(false);

            parseTutorialEventName();
        }
    }
    public void playTutorial(int tutorialID)
    {
        currentTutId = tutorialID;
        if (currentTutId >= 0)
        {
            tutorialInfo = tutorialData[currentTutId];
            //if (tutorialInfo.next > 0 || (tutorialInfo.completion != null && tutorialInfo.completion != ""))
            //    nextBtn.gameObject.SetActive(true);
            //else
            //    nextBtn.gameObject.SetActive(false);

            parseTutorialEventName();
        }
    }
    public void hideTutorial()
    {
        spotLight.gameObject.SetActive(false);
        arrowHolder.gameObject.SetActive(false);
    }

    private void StartTutorial()
    {
        tutorialInfo = tutorialData[0];
        parseTutorialEventName();
    }
    private void parseTutorialEventName()
    {
        parseTutorialEventTarget();
    }
    private void parseTutorialEventTarget()
    {
        if (tutorialInfo.event_name == null || tutorialInfo.event_name == "null")
        {
            spotLight.gameObject.SetActive(false);
            arrowHolder.gameObject.SetActive(false);
            placeAndShowSpeech();
        }
        else
        {
            string targetName = tutorialInfo.event_target;
            GameObject target = GameObject.Find(targetName);
            if (target != null)
            {
                spotLight.gameObject.SetActive(true);
                shape = tutorialInfo.shape;
                highlightTarget(target);
            }
        }
    }

    private void highlightTarget(GameObject target)
    {
        Canvas canvas = target.GetComponentInParent<Canvas>();
        if (canvas != null)
            drawTransparentArea(target);
        else
            drawTransparentAreaFor3D(target);
    }

    private void drawTransparentArea(GameObject target)
    {
        RectTransform rectTransform = target.transform as RectTransform;

        // Converts target's world position to screen space.. Takes care of Anchor point as well
        float startX = Camera.main.WorldToScreenPoint(target.transform.position).x;
        float startY = Camera.main.WorldToScreenPoint(target.transform.position).y;
        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;

        // Converts screenspace to actual screen space based on Pivot point
        Rect rect = RectTransformUtility.PixelAdjustRect(rectTransform, target.GetComponentInParent<Canvas>());
        startX = startX + rect.x;
        startY = startY + rect.y;

        Rect spotLightRect = new Rect(startX, startY, width, height);
        drawTexture(spotLightRect);
    }
    private void drawTransparentAreaFor3D(GameObject target)
    {
        MeshRenderer mesh = target.GetComponentInChildren<MeshRenderer>();
        if (mesh == null)
        {
            Debug.LogError("MeshRenderer is not available to target");
            return;
        }

        Vector3 posStart = Camera.main.WorldToScreenPoint(new Vector3(mesh.bounds.min.x, mesh.bounds.min.y, mesh.bounds.min.z));
        Vector3 posEnd = Camera.main.WorldToScreenPoint(new Vector3(mesh.bounds.max.x, mesh.bounds.max.y, mesh.bounds.min.z));

        float depth = Mathf.Abs((posEnd.z - posStart.z));
        float width = Mathf.Abs((posEnd.x - posStart.x)) * depth;
        float height = Mathf.Abs((posEnd.y - posStart.y)) * depth;
        
        float startX = Camera.main.WorldToScreenPoint(target.transform.position).x - width / 2;
        float startY = Camera.main.WorldToScreenPoint(target.transform.position).y - height / 2;
        Debug.LogError(startX + " :: " + startY + " :: " + width + " :: " + height + " :: " + depth);
        Rect spotLightRect = new Rect(startX, startY, width, height);
        drawTexture(spotLightRect);
    }
    private void drawTexture(Rect rect)
    {
        switch (shape)
        {
            case TutorialInfo.Shapes.RECTANGLE:
                applySpriteToImageComponent(convertTextureToSprite(drawRectangle(rect)));
                break;
            case TutorialInfo.Shapes.CIRCLE:
                applySpriteToImageComponent(convertTextureToSprite(drawCircle(rect)));
                break;
        }
    }
    private void placeAndShowArrow(Rect rect)
    {
        switch (tutorialInfo.arrowpos)
        {
            case 0:
                rect.x = rect.x + (rect.width / 2);
                rect.y = rect.y + rect.height;
                break;
            case 1:
                rect.x = rect.x;
                rect.y = rect.y + rect.height + 41.4f;
                break;
            case 2:
                rect.x = rect.x - 50.0f;
                rect.y = rect.y + rect.height / 2;
                break;
            case 6:
                rect.x = rect.x + rect.width;
                rect.y = rect.y + rect.height / 2;
                break;
            case 7:
                rect.x = rect.x + rect.width;
                rect.y = rect.y + rect.height;
                break;
            default:
                break;
        }
        arrowHolder.SetActive(true);
        arrow.transform.localEulerAngles = new Vector3(0.0f, 0.0f, (tutorialInfo.arrowpos * 45.0f));
        arrowHolder.transform.localPosition = new Vector3(rect.x, rect.y, 0.0f);
    }
    private void placeAndShowSpeech()
    {
        speechText.transform.GetComponent<RectTransform>().rotation = Quaternion.identity;
        speechText.text = tutorialInfo.speech;

        if (tutorialInfo.hideSpotLight)
            speechText.transform.GetComponent<TextMeshProUGUI>().color = new Color(37/255f, 37/255f, 37/255f, 1);
        else
            speechText.transform.GetComponent<TextMeshProUGUI>().color = Color.white;
    }
    private Texture2D drawRectangle(Rect rect)
    {
        rect.x = ((float)spotLightTexture.width / (float)Screen.currentResolution.width) * rect.x;
        rect.y = ((float)spotLightTexture.height / (float)Screen.currentResolution.height) * rect.y;

        resetLastPixels();

        Color color = new Color(0, 0, 0);
        for (int r = (int)rect.x; r <= (int)(rect.x + rect.width); r++)
        {
            for (int c = (int)rect.y; c <= (int)(rect.y + rect.height); c++)
            {
                color.a = 0.0f;
                spotLightTexture.SetPixel(r, c, color);
            }
        }
        spotLightTexture.Apply();
        lastDrawnShape = TutorialInfo.Shapes.RECTANGLE;

        if (tutorialInfo.arrowpos >= 0)
            placeAndShowArrow(rect);
        else
            arrowHolder.SetActive(false);

        if (tutorialInfo.speech == "")
            speechText.text = "";
        else
            placeAndShowSpeech();

        if (tutorialInfo.hideSpotLight == true)
            spotLight.gameObject.SetActive(false);
        else
            spotLight.gameObject.SetActive(true);

        lastDrawnTextureSize = rect;
        return spotLightTexture;
    }
    private Texture2D drawCircle(Rect rect)
    {
        rect.x = ((float)spotLightTexture.width / (float)Screen.currentResolution.width) * rect.x;
        rect.y = ((float)spotLightTexture.height / (float)Screen.currentResolution.height) * rect.y;

        resetLastPixels();

        int radius = (int)Mathf.Sqrt(Mathf.Pow(rect.width, 2) + Mathf.Pow(rect.height, 2)) / 2;

        int centerX = (int)(rect.x + rect.width / 2);
        int centerY = (int)(rect.y + rect.height / 2);

        Circle(spotLightTexture, centerX, centerY, radius, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        spotLightTexture.Apply();
        lastDrawnShape = TutorialInfo.Shapes.CIRCLE;

        if (tutorialInfo.arrowpos >= 0)
            placeAndShowArrow(rect);
        else
            arrowHolder.SetActive(false);

        if (tutorialInfo.speech == "")
            speechText.text = "";
        else
            placeAndShowSpeech();

        return spotLightTexture;
    }
    public void Circle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        int x, y, px, nx, py, ny, d;

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                if (px >= 0 && py >= 0)
                    tex.SetPixel(px, py, col);
                if (nx >= 0 && py >= 0)
                    tex.SetPixel(nx, py, col);

                if (px >= 0 && ny >= 0)
                    tex.SetPixel(px, ny, col);
                if (nx >= 0 && ny >= 0)
                    tex.SetPixel(nx, ny, col);

            }
        }
    }
    private Sprite convertTextureToSprite(Texture2D texture)
    {
        spotLightSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.0f, 0.0f));
        spotLightSprite.name = "Spotlight";
        return spotLightSprite;
    }
    private void applySpriteToImageComponent(Sprite sprite)
    {
        spotLight.sprite = sprite;
        spotLight.SetNativeSize();
    }

    /*********************** UTILS ***********************/
    private int getIDBasedOnUID(string tutorialUID)
    {
        int id = -1;
        //foreach (KeyValuePair<int, TutorialInfo> tutorial in tutorialDict)
        //{
        //    TutorialInfo info = tutorial.Value;
        //    if (info.uid == tutorialUID)
        //        return tutorial.Key;
        //}

        for(int i = 0; i < tutorialData.Count; i++)
        {
            if(tutorialData[i].uid == tutorialUID)
            {
                id = i;
                break;
            }
        }

        return id;
    }

    private void resetLastPixels()
    {
        Color color = new Color(0, 0, 0, 1.0f);
        if (lastDrawnTextureSize != null)
        {
            if (lastDrawnShape == TutorialInfo.Shapes.RECTANGLE)
            {
                for (int r = (int)lastDrawnTextureSize.x; r <= (int)(lastDrawnTextureSize.x + lastDrawnTextureSize.width); r++)
                {
                    for (int c = (int)lastDrawnTextureSize.y; c <= (int)(lastDrawnTextureSize.y + lastDrawnTextureSize.height); c++)
                    {
                        spotLightTexture.SetPixel(r, c, color);
                    }
                }
            }
            else if (lastDrawnShape == TutorialInfo.Shapes.CIRCLE)
            {
                for (int r = 0; r < spotLightTexture.width; r++)
                {
                    for (int c = 0; c < spotLightTexture.height; c++)
                    {
                        spotLightTexture.SetPixel(r, c, color);
                    }
                }
            }
        }
        spotLightTexture.Apply();
    }
}