using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Newtonsoft.Json;

public class EconomyController : MonoBehaviour
{
    public TextAsset economyData;
    public Transform uiHUD;

    private Economies economies;

    public enum EconomyType { DIAMOND, GOLD }
    public EconomyType economyType;

    private Dictionary<EconomyType, Economy> inGameEconomy = new Dictionary<EconomyType, Economy>();

    private static EconomyController _instance;
    public static EconomyController instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void findHUD()
    {
        uiHUD = GameController.instance.mainCanvasController.topHUD;
    }

    public void init()
    {
        string pathDirectory = System.IO.Path.Combine(Application.persistentDataPath, "data");
        string ecoDataPath = System.IO.Path.Combine(pathDirectory, "EconomyInfo.json");

        string ecoConfigData = GameController.instance.checkFileAndReadString(ecoDataPath);
        if(ecoConfigData == "")
        {
            ecoConfigData = economyData.ToString();
        }

        economies = JsonConvert.DeserializeObject<Economies>(ecoConfigData);

        inGameEconomy.Clear();
        for(int i = 0; i < economies.economy.Length; i++)
        {
            economyType = (EconomyType)Enum.Parse(typeof(EconomyType), economies.economy[i].type.ToUpper());
            inGameEconomy.Add(economyType, economies.economy[i]);
        }
    }

    public void writeEconomyData()
    {
        Economies es = new Economies();
        es.economy = new Economy[inGameEconomy.Count];

        for(int i = 0; i < es.economy.Length; i++)
        {
            es.economy[i] = inGameEconomy.ElementAt(i).Value;
        }
        string ecoDataPath = System.IO.Path.Combine(Application.persistentDataPath, "data", "EconomyInfo.json");
        string convertedJson = JsonConvert.SerializeObject(es, Formatting.Indented);
        GameController.instance.writeToFile(ecoDataPath, convertedJson);
    }

    public void updateAllCurrencies(Hashtable ecoHash)
    {
        foreach(var key in ecoHash.Keys)
        {
            if (key.ToString() == "diamond")
                updateCurrency(EconomyType.DIAMOND, int.Parse(ecoHash[key].ToString()));
            if (key.ToString() == "gold")
                updateCurrency(EconomyType.GOLD, int.Parse(ecoHash[key].ToString()));
        }
    }

    public void updateCurrency(EconomyType eType, int amount)
    {
        inGameEconomy[eType].amount += amount;
        updateEcoUI(eType);
    }

    public void updateAllEcoUI()
    {
        foreach(var key in inGameEconomy.Keys)
        {
            updateEcoUI(key);
        }
    }

    public void updateEcoUI(EconomyType etype)
    {
        if(uiHUD == null)
        {
            findHUD();
        }

        string type = etype.ToString().ToLower();
        type = type.ElementAt(0).ToString().ToUpper() + type.Substring(1);

        Transform uiObj = uiHUD.Find(type);
        if(uiObj != null)
            uiObj.GetComponentInChildren<TextMeshProUGUI>().text = inGameEconomy[etype].amount.ToString();
    }
}

[Serializable]
public class Economies
{
    public Economy[] economy;
}



[Serializable]
public class Economy
{
    public string type;
    public int amount;
}
