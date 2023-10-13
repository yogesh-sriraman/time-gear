using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    public TextAsset pathData;
    public TextAsset playerData;
    public TextAsset levelData;

    public CameraFollow cameraFollow;
    public InputController inputController;
    public GearController gearController;
    public PathController pathController;
    public CharacterController characterController;
    public CPC_CameraPath cameraPath;
    public PlayerProfile playerProfile;
    public PlayerLevels playerLevels;
    public MainCanvasController mainCanvasController;

    public GameObject canvasObject;
    public GameObject activePopup;
    public GameObject quitInfoPrefab;
    public GameObject uiQuitInfo;

    public ParticleSystem touchDisplay;
    public List<Image> starDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI timeDisplay;

    public Dictionary<int, GameChapters> playerChapterData = new Dictionary<int, GameChapters>();
    public Dictionary<string, PlayerLevel> playerLevelData = new Dictionary<string, PlayerLevel>();

    public string currScene;
    public string configData;

    public int currSceneIndex;
    public int startTime;
    public int timeTaken;
    public int starsEarned;
    private int totalScore;
    public int totalStarsEarned;

    public bool checkCharInBounds;
    public bool isLevelCompleted;

    public int backButtonCounter;

    public static GameController instance { get; private set; }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        init();
    }

    private void updateGOs()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraPath = FindObjectOfType<CPC_CameraPath>().GetComponent<CPC_CameraPath>();
        touchDisplay = GameObject.Find("BasicZoneYellow").GetComponent<ParticleSystem>();

        inputController = FindObjectOfType<InputController>().GetComponent<InputController>();
        gearController = FindObjectOfType<GearController>().GetComponent<GearController>();
        pathController = FindObjectOfType<PathController>().GetComponent<PathController>();
        characterController = FindObjectOfType<CharacterController>().GetComponent<CharacterController>();

        canvasObject = GameObject.FindGameObjectWithTag("MainCanvas");
        mainCanvasController = canvasObject.GetComponent<MainCanvasController>();

        scoreDisplay = mainCanvasController.victoryPopup.Find("ScoreBG").GetChild(0).GetComponent<TextMeshProUGUI>();
        timeDisplay = mainCanvasController.topHUD.Find("TimeDisplay").GetComponent<TextMeshProUGUI>();

        mainCanvasController.topHUD.Find("PauseBtn").GetComponent<Button>().onClick.AddListener(delegate { pauseGameAndShowPopup(); });

        mainCanvasController.victoryPopup.Find("HomeBtn").GetComponent<Button>().onClick.AddListener(delegate { openHomeScene(); });
        mainCanvasController.victoryPopup.Find("PlayAgainBtn").GetComponent<Button>().onClick.AddListener(delegate { restartLevel(); });
        mainCanvasController.victoryPopup.Find("NextLevelBtn").GetComponent<Button>().onClick.AddListener(delegate { nextLevel(); });

        mainCanvasController.failPopup.Find("HomeBtn").GetComponent<Button>().onClick.AddListener(delegate { openHomeScene(); });
        mainCanvasController.failPopup.Find("PlayAgainBtn").GetComponent<Button>().onClick.AddListener(delegate { restartLevel(); });

        mainCanvasController.pausePopup.Find("HomeBtn").GetComponent<Button>().onClick.AddListener(delegate { openHomeScene(); });
        mainCanvasController.pausePopup.Find("PlayAgainBtn").GetComponent<Button>().onClick.AddListener(delegate { restartLevel(); });
        mainCanvasController.pausePopup.Find("CloseButton").GetComponent<Button>().onClick.AddListener(delegate { resumeGame(); });

        updatePausePopup();

        starDisplay.Clear();
        Transform orbsParent = mainCanvasController.victoryPopup.Find("Orbs");
        for(int i = 0; i < orbsParent.childCount; i++)
        {
            starDisplay.Add(orbsParent.GetChild(i).GetChild(0).GetComponent<Image>());
        }
    }

    public void init()
    {
        currScene = SceneManager.GetActiveScene().name;
        currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        backButtonCounter = 0;
        activePopup = null;

        if (playerLevelData.Count == 0)
        {
            initPlayerData();
            Timer.instance.init();
            EconomyController.instance.init();
        }
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            updateSettings();
        }
        if (SceneManager.GetActiveScene().buildIndex >= 2)
        {
            totalScore = 1000;
            checkCharInBounds = false;
            isLevelCompleted = false;
            updateGOs();

            gearController.init();
            configData = pathData.ToString();
            pathController.updateFromJson(configData, currScene);
            characterController.init();

            canvasObject.SetActive(true);
            mainCanvasController.pausePopup.gameObject.SetActive(false);
            mainCanvasController.victoryPopup.gameObject.SetActive(false);
            mainCanvasController.failPopup.gameObject.SetActive(false);
            mainCanvasController.popupBG.gameObject.SetActive(false);
            cameraFollow.init();

            EconomyController.instance.updateAllEcoUI();

            if (TutorialController.instance != null)
            {
                TutorialController.instance.init();
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    TutorialController.instance.playTutorial("tut_01");
                    PlayerPrefs.SetInt("tutorialID", 1);
                }
                if (SceneManager.GetActiveScene().buildIndex == 3)
                {
                    TutorialController.instance.playTutorial("tut_07");
                    PlayerPrefs.SetInt("tutorialID", 7);
                }
            }
            startTime = (int)Timer.instance.CurrentTime;
            starsEarned = 0;
            StartCoroutine(updateTimer());
        }
    }

    private IEnumerator updateTimer()
    {
        int currT = (int)Timer.instance.CurrentTime;
        int timePassed = currT - startTime;

        int timeLeft = playerLevelData[currScene].minimumTime - timePassed;
        if (timeLeft < 0)
            timeLeft = 0;

        timeDisplay.text = timeLeft.ToString();

        yield return new WaitForSeconds(1);

        StartCoroutine(updateTimer());
    }

    private void updateSettings()
    {
        LevelController.instance.settingsPanel.gameObject.SetActive(false);
        LevelController.instance.settingsBtn.onClick.AddListener(delegate { openSettingsPopup(); });

        LevelController.instance.settingsPanel.GetChild(0).Find("CloseButton").GetComponent<Button>()
            .onClick.AddListener(delegate { closeSettingsPopup(); });

        updateMusicFX(LevelController.instance.settingsPanel.GetChild(0).Find("MusicFX"));
        updateSoundFX(LevelController.instance.settingsPanel.GetChild(0).Find("SoundFX"));
    }

    private void updatePausePopup()
    {
        updateMusicFX(mainCanvasController.pausePopup.Find("MusicFX"));
        updateSoundFX(mainCanvasController.pausePopup.Find("SoundFX"));
    }

    private void updateMusicFX(Transform musicParent)
    {
        Slider musicSlider = musicParent.Find("MusicSlider").GetComponent<Slider>();
        musicSlider.value = AudioManager.instance.getVolume("Theme Music");
        GameObject crossSymbol = musicParent.Find("MusicBG").GetChild(1).gameObject;
        if (musicSlider.value == 0)
        {
            crossSymbol.SetActive(true);
        }
        else
        {
            crossSymbol.SetActive(false);
        }

        musicSlider.onValueChanged.AddListener(delegate { changeMusicVolume(musicSlider, crossSymbol); });
    }

    private void updateSoundFX(Transform soundParent)
    {
        Slider soundSlider = soundParent.Find("SoundSlider").GetComponent<Slider>();
        soundSlider.value = AudioManager.instance.getSoundVolume();
        GameObject crossSymbol = soundParent.Find("SoundBG").GetChild(1).gameObject;
        if (soundSlider.value == 0)
        {
            crossSymbol.SetActive(true);
        }
        else
        {
            crossSymbol.SetActive(false);
        }

        soundSlider.onValueChanged.AddListener(delegate { changeSoundVolume(soundSlider, crossSymbol); });
    }

    private void openSettingsPopup()
    {
        AudioManager.instance.PlaySound("Button Click1");
        backButtonCounter = 0;
        LevelController.instance.settingsPanel.gameObject.SetActive(true);
        activePopup = LevelController.instance.settingsPanel.gameObject;
    }
    private void closeSettingsPopup()
    {
        AudioManager.instance.PlaySound("Button Click1");
        LevelController.instance.settingsPanel.gameObject.SetActive(false);
        activePopup = null;
    }

    private void initPlayerData()
    {
        string pathDirectory = System.IO.Path.Combine(Application.persistentDataPath, "data");
        checkAndCreateDirectory(pathDirectory);


        string playerdataPath = System.IO.Path.Combine(pathDirectory, "PlayerProfile.json");
        configData = checkFileAndReadString(playerdataPath);
        if(configData == "")
        {
            Debug.Log("File Not Exists");
            configData = playerData.ToString();
        }
        playerProfile = JsonConvert.DeserializeObject<PlayerProfile>(configData);
        totalStarsEarned = playerProfile.numberOfStars;
        playerChapterData.Clear();
        for (int i = 0; i < playerProfile.gameChapters.Length; i++)
        {
            int chapterID = playerProfile.gameChapters[i].chapterID;
            playerChapterData.Add(chapterID, playerProfile.gameChapters[i]);
        }


        string playerLevelPath = System.IO.Path.Combine(pathDirectory, "PlayerLevels.json");
        configData = checkFileAndReadString(playerLevelPath);
        if (configData == "")
        {
            Debug.Log("File Not Exists");
            configData = levelData.ToString();
        }
        playerLevels = JsonConvert.DeserializeObject<PlayerLevels>(configData);
        playerLevelData.Clear();
        for (int i = 0; i < playerLevels.playerLevels.Length; i++)
        {
            string key = playerLevels.playerLevels[i].levelName;
            if (!playerChapterData[playerLevels.playerLevels[i].chapterID].isUnlocked)
                playerLevels.playerLevels[i].isUnlocked = false;

            playerLevelData.Add(key, playerLevels.playerLevels[i]);
        }
    }

    private void checkAndCreateDirectory(string pathDir)
    {
        if (!Directory.Exists(pathDir))
            Directory.CreateDirectory(pathDir);
    }

    public string checkFileAndReadString(string fileDataPath)
    {
        string readData = "";
        if (File.Exists(fileDataPath))
        {
            Debug.Log("File Exists");

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(fileDataPath, FileMode.Open);
            readData = (string)bf.Deserialize(fs);
            fs.Close();
        }
        return readData;
    }

    public void writePlayerData()
    {
        PlayerProfile pp = new PlayerProfile();
        pp.playerID = playerProfile.playerID;
        pp.numberOfStars = totalStarsEarned;
        pp.gameChapters = new GameChapters[playerChapterData.Count];
        for(int i = 0; i < pp.gameChapters.Length; i++)
        {
            pp.gameChapters[i] = playerChapterData.ElementAt(i).Value;
        }
        string playerdataPath = System.IO.Path.Combine(Application.persistentDataPath, "data", "PlayerProfile.json");
        string convertedJson = JsonConvert.SerializeObject(pp, Formatting.Indented);
        writeToFile(playerdataPath, convertedJson);


        PlayerLevels pl = new PlayerLevels();
        pl.playerID = playerLevels.playerID;
        pl.playerLevels = new PlayerLevel[playerLevelData.Count];
        for(int i = 0; i < pl.playerLevels.Length; i++)
        {
            pl.playerLevels[i] = playerLevelData.ElementAt(i).Value;
        }
        string playerLevelPath = System.IO.Path.Combine(Application.persistentDataPath, "data", "PlayerLevels.json");
        convertedJson = JsonConvert.SerializeObject(pl, Formatting.Indented);
        writeToFile(playerLevelPath, convertedJson);
    }

    public void writeToFile(string dataPath, string jsonToWrite)
    {
        if (!File.Exists(dataPath))
            File.Create(dataPath).Dispose();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(dataPath, FileMode.OpenOrCreate);
        bf.Serialize(fs, jsonToWrite);
        fs.Close();
        Debug.Log(jsonToWrite);
    }

    public void calculateStarsEarned()
    {
        timeTaken = (int)Timer.instance.CurrentTime - startTime;

        int localScore = 0;
        localScore +=  (characterController.health * 100);
        localScore += starsEarned * 100;
        localScore += (timeTaken <= playerLevelData[currScene].minimumTime) ? 400 : 0;

        scoreDisplay.text = "Score : " + localScore;

        int totalStars = 1;
        if(localScore == 1000)
        {
            totalStars = 3;
        }
        else if(localScore > 300)
        {
            totalStars = 2;
        }
        if (playerLevelData[currScene].numberOfOrbs < totalStars)
        {
            totalStarsEarned += totalStars - playerLevelData[currScene].numberOfOrbs;
            playerLevelData[currScene].numberOfOrbs = totalStars;
        }

        for(int i = 0; i < starDisplay.Count; i++)
        {
            starDisplay[i].fillAmount = 0;
            if (i <= totalStars - 1)
                starDisplay[i].fillAmount = 1;
        }
    }

    public void showVictoryPopup()
    {
        inputController.canInteract = false;
        canvasObject.SetActive(true);
        mainCanvasController.popupBG.gameObject.SetActive(true);
        mainCanvasController.failPopup.gameObject.SetActive(false);
        mainCanvasController.victoryPopup.gameObject.SetActive(true);
        mainCanvasController.pausePopup.gameObject.SetActive(false);
        activePopup = mainCanvasController.victoryPopup.gameObject;
        AudioManager.instance.PlaySound("Level Complete");
        playerLevelData[currScene].isCompleted = true;
        unlockNextLevel();
    }

    private void unlockNextLevel()
    {
        string prevSceneName = currScene;
        int sceneIndex = currSceneIndex;

        if (sceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            string nextSceneName = getNextSceneName(sceneIndex);
            while (playerLevelData[nextSceneName].isUnlocked == true)
            {
                sceneIndex += 1;
                if(sceneIndex + 1 >= SceneManager.sceneCountInBuildSettings)
                {
                    sceneIndex = -1;
                    break;
                }
                prevSceneName = nextSceneName;
                nextSceneName = getNextSceneName(sceneIndex);
            }

            if (sceneIndex != -1)
            {
                bool isSameCurrScene = prevSceneName == currScene;
                bool isSameChapter = playerLevelData[currScene].chapterID == playerLevelData[nextSceneName].chapterID;

                if (isSameChapter)
                {
                    if (isSameCurrScene)
                        playerLevelData[nextSceneName].isUnlocked = true;
                }
                else
                {
                    if (!playerChapterData[playerLevelData[nextSceneName].chapterID].isUnlocked)
                    {
                        if (playerLevelData[prevSceneName].isCompleted)
                        {
                            if (totalStarsEarned >= playerChapterData[playerLevelData[nextSceneName].chapterID].starToUnlock)
                            {
                                playerChapterData[playerLevelData[nextSceneName].chapterID].isUnlocked = true;
                                playerLevelData[nextSceneName].isUnlocked = true;
                            }
                        }
                    }
                }
            }
        }
        writePlayerData();
        EconomyController.instance.writeEconomyData();
    }

    private string getNextSceneName(int sceneIndex)
    {
        string nextScenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex + 1);
        string nextSceneName = nextScenePath.Substring(nextScenePath.LastIndexOf('/') + 1);
        return nextSceneName.Split('.')[0];
    }

    public void showFailPopup()
    {
        inputController.canInteract = false;
        canvasObject.SetActive(true);
        mainCanvasController.popupBG.gameObject.SetActive(true);
        mainCanvasController.failPopup.gameObject.SetActive(true);
        mainCanvasController.victoryPopup.gameObject.SetActive(false);
        mainCanvasController.pausePopup.gameObject.SetActive(false);
        activePopup = mainCanvasController.failPopup.gameObject;
        AudioManager.instance.PlaySound("Death");

        if (currScene == "Hylia_1")
            PlayerPrefs.DeleteKey("tutorialID");
    }

    public void pauseGameAndShowPopup()
    {
        AudioManager.instance.PlaySound("Button Click1");
        Time.timeScale = 0;
        inputController.canInteract = false;
        mainCanvasController.popupBG.gameObject.SetActive(true);
        mainCanvasController.failPopup.gameObject.SetActive(false);
        mainCanvasController.victoryPopup.gameObject.SetActive(false);
        mainCanvasController.pausePopup.gameObject.SetActive(true);
        activePopup = mainCanvasController.pausePopup.gameObject;
    }

    public void resumeGame()
    {
        mainCanvasController.failPopup.gameObject.SetActive(false);
        mainCanvasController.victoryPopup.gameObject.SetActive(false);
        mainCanvasController.pausePopup.gameObject.SetActive(false);
        mainCanvasController.popupBG.gameObject.SetActive(false);
        activePopup = null;
        Time.timeScale = 1;
        AudioManager.instance.PlaySound("Button Click1");
        inputController.canInteract = true;
    }

    public void closeActivePopup()
    {
        AudioManager.instance.PlaySound("Button Click1");
        activePopup.SetActive(false);
        mainCanvasController.popupBG.gameObject.SetActive(false);
    }

    public void changeMusicVolume(Slider slider, GameObject cross)
    {
        AudioManager.instance.setVolume("Theme Music", slider.value);
        if(slider.value == 0)
        {
            cross.SetActive(true);
        }
        else
        {
            cross.SetActive(false);
        }
    }

    public void changeSoundVolume(Slider slider, GameObject cross)
    {
        AudioManager.instance.setSoundVolume(slider.value);
        if (slider.value == 0)
        {
            cross.SetActive(true);
        }
        else
        {
            cross.SetActive(false);
        }
    }


    #region SceneManagements
    public void openHomeScene()
    {
        loadLevel("LevelSelection");
        AudioManager.instance.PlaySound("Button Click1");
    }

    public void restartLevel()
    {
        loadLevel(currScene);
        //SceneManager.LoadSceneAsync(currScene, LoadSceneMode.Single).completed += onLoadComplete;
        AudioManager.instance.PlaySound("Button Click1");
    }

    public void nextLevel()
    {
        int index = SceneManager.GetSceneByName(currScene).buildIndex;

        if (index + 1 >= SceneManager.sceneCountInBuildSettings)
            index = -1;

        string nextLevelName = getNextSceneName(index);

        if (!playerLevelData.ContainsKey(nextLevelName) || (playerLevelData.ContainsKey(nextLevelName) && playerLevelData[nextLevelName].isUnlocked))
        {
            loadLevel(nextLevelName);
            AudioManager.instance.PlaySound("Button Click1");
        }
    }

    public void loadLevel(string levelName = null)
    {
        if (levelName != null)
        {
            SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single).completed += onLoadComplete;
            // do code to show loading screen
        }
    }

    public void onLoadComplete(AsyncOperation async)
    {
        //do code to quit loading screen
        if (Time.timeScale == 0)
            Time.timeScale = 1;

        init();
    }
    #endregion


    #region backButtonDetection
    // GameController is always active from the start of the game.
    // Hence this input detection is done here instead of input controller.

    private bool isBackButtonTouched()
    {
        return Input.GetKeyDown(KeyCode.Escape);
    }

    private void inLevelFunctions()
    {
        if (activePopup == mainCanvasController.pausePopup.gameObject)
        {
            resumeGame();
        }
        else if(activePopup == null)
        {
            pauseGameAndShowPopup();
        }
    }

    private void homeScreenFunctions()
    {
        if (activePopup != null)
        {
            closeSettingsPopup();
        }
        else
        {
            backButtonCounter += 1;
            StartCoroutine(showQuitInfo());
            if (backButtonCounter == 2)
            {
                Application.Quit();
            }
        }
    }

    private IEnumerator showQuitInfo()
    {
        Transform parentTransformBG;

        if (currSceneIndex == 0)
            parentTransformBG = LevelChanger.instance.backgroundBG.transform;
        else
            parentTransformBG = LevelController.instance.bgImage.transform;

        uiQuitInfo = Instantiate(quitInfoPrefab, parentTransformBG);
        yield return new WaitForSeconds(3);
        if(uiQuitInfo != null)
            Destroy(uiQuitInfo);
    }


    private void Update()
    {
        if(isBackButtonTouched())
        {
            if(currSceneIndex >= 2)
            {
                inLevelFunctions();
            }
            else
            {
                homeScreenFunctions();
            }
        }
        else
        {
            if (Input.anyKeyDown && !isBackButtonTouched())
            {
                backButtonCounter = 0;
                if (uiQuitInfo != null)
                    Destroy(uiQuitInfo);
            }
        }
    }
    #endregion
}
