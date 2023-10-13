using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearController : MonoBehaviour
{
    public GameObject gearParent;
    public List<Gear> gears = new List<Gear>();
    public int moveUpDir;
    public int moveLRDir;
    public bool canToggleMovUpDir = false;
    public int flag;
    public bool moveX;
    public bool moveZ;
    public bool canToggleMoveLR = false;
    public int gearDir;

    public void init()
    {
        GameController.instance.inputController.canInteract = true;
        for(int i = 0; i < gearParent.transform.childCount; i++)
        {
            Gear gearObject = gearParent.transform.GetChild(i).GetComponent<Gear>();
            gearObject.gearDir = 1;
            gears.Add(gearObject);
        };
    }

    public void updatePathTiles(GameObject gearObject, Hashtable gearHash)
    {
        GameController.instance.inputController.canInteract = false;
        Gear gear = gearObject.GetComponent<Gear>();
        int toggle = 1;
        if (gear.canMoveUpDown)
        {
            Vector3 pObjectPos = gear.pathTile[0].transform.position;
            if (pObjectPos.y < gear.attachedPath.transform.position.y)
            {
                gear.gearDir = 1;
                moveUpDir = 1;
            }

            if (pObjectPos.y > gear.attachedPath.transform.position.y)
            {
                gear.gearDir = -1;
                moveUpDir = -1;
            }

            gearHash.Add("amount", Vector3.up * 90 * gear.gearDir);
            iTween.RotateAdd(gearObject, gearHash);

            for (int i = 0; i < gear.pathTile.Length; i++)
            {
                GameObject pathObj = gear.pathTile[i];
                int offset;
                if(Mathf.Approximately(pathObj.transform.position.y, gear.attachedPath.transform.position.y))
                {
                    canToggleMovUpDir = true;
                    offset = 2;
                    offset *= toggle * gear.gearDir;
                    toggle *= -1;
                }
                else if (pathObj.transform.position.y < gear.attachedPath.transform.position.y)
                {
                    offset = 2;
                    canToggleMovUpDir = false;
                }
                else
                {
                    offset = -2;
                    canToggleMovUpDir = false;
                }

                Hashtable hash = new Hashtable();
                hash.Add("amount", Vector3.up * offset);
                hash.Add("speed", 4);
                hash.Add("easetype", iTween.EaseType.linear);
                hash.Add("oncomplete", "onMoveUpDownComplete");
                hash.Add("oncompletetarget", gameObject);
                hash.Add("oncompleteparams", pathObj);

                iTween.MoveBy(pathObj, hash);
            }
            if (canToggleMovUpDir == true)
                moveUpDir *= -1;
        }
        if(!gear.canMoveUpDown && !gear.canMoveLeftRight)
        {
            gearHash.Add("amount", Vector3.up * 90 * gear.gearDir);
            iTween.RotateAdd(gearObject, gearHash);

            for (int i = 0; i < gear.pathTile.Length; i++)
            {
                GameObject pathObj = gear.pathTile[i];

                Hashtable hash = new Hashtable();
                hash.Add("amount", Vector3.up * 90);
                hash.Add("time", 0.5f);
                hash.Add("easetype", iTween.EaseType.linear);
                hash.Add("looptype", iTween.LoopType.none);
                hash.Add("oncomplete", "onRotComplete");
                hash.Add("oncompletetarget", gameObject);
                hash.Add("oncompleteparams", pathObj);

                iTween.RotateAdd(pathObj, hash);
            }
        }
        if(gear.canMoveLeftRight)
        {
            GameController.instance.inputController.canInteract = false;

            gearHash.Add("amount", Vector3.up * 90 * gear.gearDir);
            iTween.RotateAdd(gearObject, gearHash);

            for(int i = 0; i < gear.pathTile.Length; i++)
            {
                GameObject go = gear.pathTile[i];
                Path po = GameController.instance.pathController.getPathFromObject(go);

                Hashtable hash = new Hashtable();
                Hashtable param = new Hashtable();
                param.Add("arg1", gear);
                param.Add("arg2", po);

                if (po.moveZ)
                {
                    if (!Mathf.Approximately(go.transform.position.z, po.zPos))
                    {
                        if (go.transform.position.z < po.zPos)
                            po.moveZOffset = 2;

                        if (go.transform.position.z > po.zPos)
                            po.moveZOffset = -2;
                    }

                    hash.Add("amount", Vector3.forward * po.moveZOffset);
                    float targetZ = go.transform.position.z + po.moveZOffset;
                    param.Add("arg3", targetZ);
                }
                else if (po.moveX)
                {
                    if (!Mathf.Approximately(go.transform.position.x, po.xPos))
                    {
                        if (go.transform.position.x < po.xPos)
                            po.moveXOffset = 2;

                        if (go.transform.position.x > po.xPos)
                            po.moveXOffset = -2;
                    }

                    hash.Add("amount", Vector3.right * po.moveXOffset);
                    float targetX = go.transform.position.x + po.moveXOffset;
                    param.Add("arg3", targetX);
                }

                hash.Add("speed", 4);
                hash.Add("easetype", iTween.EaseType.linear);
                hash.Add("space", Space.World);
                hash.Add("oncomplete", "onMoveLeftRightComplete");
                hash.Add("oncompletetarget", gameObject);
                hash.Add("oncompleteparams", param);

                iTween.MoveBy(go, hash);
            }
        }
        AudioManager.instance.PlaySound("Tap Gear");
        //AudioManager.instance.PlaySound("Tile Slide");
    }

    private void onMoveUpDownComplete(GameObject pathObject)
    {
        float roundVal = Mathf.Round(pathObject.transform.position.y) - pathObject.transform.position.y;
        pathObject.transform.position += new Vector3(0, roundVal, 0);
        GameController.instance.inputController.canInteract = true;

        if(PlayerPrefs.GetInt("tutorialID", -1) <= 1 && TutorialController.instance != null)
        {
            int tID = PlayerPrefs.GetInt("tutorialID", -1);
            TutorialController.instance.playTutorial(tID);
            PlayerPrefs.SetInt("tutorialID", tID + 1);
        }
    }

    private void onRotComplete(GameObject pathObject)
    {
        float roundVal = Mathf.Round(pathObject.transform.eulerAngles.y) - pathObject.transform.eulerAngles.y;
        pathObject.transform.eulerAngles += new Vector3(0, roundVal, 0);
        GameController.instance.inputController.canInteract = true;
        if (PlayerPrefs.GetInt("tutorialID", -1) <= 5 && TutorialController.instance != null)
        {
            int tID = PlayerPrefs.GetInt("tutorialID", -1);
            TutorialController.instance.playTutorial(tID);
            PlayerPrefs.SetInt("tutorialID", tID + 1);
        }
    }

    private void onMoveLeftRightComplete(Hashtable param)
    {
        Gear gear = (Gear)param["arg1"];
        Path po = (Path)param["arg2"];
        float val = (float)param["arg3"];

        if(po.moveX)
        {
            float errorVal = val - po.pathObject.transform.position.x;
            po.pathObject.transform.position += new Vector3(errorVal, 0, 0);
        }
        if (po.moveZ)
        {
            float errorVal = val - po.pathObject.transform.position.z;
            po.pathObject.transform.position += new Vector3(0, 0, errorVal);
        }

        GameController.instance.inputController.canInteract = true;
        if (PlayerPrefs.GetInt("tutorialID", -1) <= 3 && TutorialController.instance != null)
        {
            int tID = PlayerPrefs.GetInt("tutorialID", -1);
            TutorialController.instance.playTutorial(tID);
            PlayerPrefs.SetInt("tutorialID", tID + 1);
        }
    }
}
