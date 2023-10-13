using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool cameraRotating;
    public float panDir;

    public GameObject camTarget;
    public Transform currTT;
    public Transform nextTT;

    public void init()
    {
        currTT = null;
        nextTT = null;
        cameraRotating = false;
        setCamPosition();
        setCamTarget();

        //minZoom = 20;
        //maxZoom = 40;
        //Camera.main.transform.eulerAngles = new Vector3(45, -45, 0);
        //Camera.main.fieldOfView = (minZoom + maxZoom) / 2;
    }
    private void setCamPosition()
    {
        transform.position = GameController.instance.cameraPath.points[0].position;
    }

    private void setCamTarget()
    {
        Transform targetTransform;

        if (camTarget.transform.childCount > 1)
        {
            Vector3 camPos = transform.position;
            targetTransform = camTarget.transform.GetChild(0);
            float dist = Vector3.Distance(camTarget.transform.GetChild(0).position, camPos);
            for(int i = 1; i < camTarget.transform.childCount; i++)
            {
                float currDist = Vector3.Distance(camTarget.transform.GetChild(i).position, camPos);

                if (currDist < dist)
                {
                    dist = currDist;
                    targetTransform = camTarget.transform.GetChild(i);
                }
            }
        }
        else
        {
            targetTransform = camTarget.transform.GetChild(0);
        }

        GameController.instance.cameraPath.target = targetTransform;
        transform.LookAt(targetTransform);
    }

    //public void updateCameraPosition()
    //{
    //    Vector3 camPos = cameraPosition;
    //    camPos.x = GameController.instance.characterController.transform.position.x + cameraPosition.x;
    //    camPos.z = GameController.instance.characterController.transform.position.z + cameraPosition.z;
    //    transform.position = camPos;
    //}

    public void rotateCamera(float multiplier)
    {
        GameController.instance.inputController.canInteract = false;
        panDir = multiplier;
        StartCoroutine(moveCam());
    }

    public IEnumerator moveCam()
    {
        bool isPaused = GameController.instance.cameraPath.IsPaused();
        bool isPlaying = GameController.instance.cameraPath.IsPlaying();

        int currSeg = GameController.instance.cameraPath.GetCurrentWayPoint();
        int nextSeg = GameController.instance.cameraPath.GetNextIndex(currSeg);

        findChangeOfTarget(currSeg, nextSeg);

        if (!isPaused && !isPlaying)
        {
            GameController.instance.cameraPath.PlayPath(5);
        }
        else
        {
            if (isPaused)
                GameController.instance.cameraPath.ResumePath();
        }

        while(GameController.instance.cameraPath.GetCurrentWayPoint() != nextSeg)
        {
            yield return null;
        }
        if (GameController.instance.cameraPath.GetCurrentWayPoint() == nextSeg)
        {
            if (nextTT != currTT)
            {
                GameObject newObj = GameController.instance.cameraPath.target.gameObject;
                GameController.instance.cameraPath.target = nextTT;
                currTT = null;
                nextTT = null;
                Destroy(newObj);
            }
            GameController.instance.cameraPath.PausePath();
            transform.LookAt(GameController.instance.cameraPath.target);

            if (TutorialController.instance != null)
            {
                int tID = PlayerPrefs.GetInt("tutorialID", -1);

                if (tID == 7)
                {
                    TutorialController.instance.playTutorial("tut_08");
                    PlayerPrefs.SetInt("tutorialID", tID + 1);
                }
            }

            GameController.instance.inputController.canInteract = true;
        }
    }
    private void findChangeOfTarget(int currentSeg, int nextSeg)
    {
        if(camTarget.transform.childCount > 1)
        {
            currTT = GameController.instance.cameraPath.target;
            nextTT = currTT;
            Vector3 nextSegPos = GameController.instance.cameraPath.points[nextSeg].position;
            float currDist = Vector3.Distance(currTT.position, nextSegPos);
            for(int i = 0; i < camTarget.transform.childCount; i++)
            {
                float dist = Vector3.Distance(nextSegPos, camTarget.transform.GetChild(i).position);

                if(dist < currDist)
                {
                    currDist = dist;
                    nextTT = camTarget.transform.GetChild(i);
                }
            }

            if(currTT != nextTT)
            {
                GameObject newObj = new GameObject();
                newObj.transform.position = currTT.position;
                GameController.instance.cameraPath.target = newObj.transform;
                moveTargetToNextPos(newObj);
            }
        }
    }

    private void moveTargetToNextPos(GameObject obj)
    {
        Hashtable hash = new Hashtable();
        hash.Add("position", nextTT.position);
        hash.Add("time", 1);
        hash.Add("easetype", iTween.EaseType.easeInOutSine);
        iTween.MoveTo(obj, hash);
    }
}
