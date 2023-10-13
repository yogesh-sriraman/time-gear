using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;

public class InputController : MonoBehaviour
{
    public bool canInteract;
    public Vector2 initPosition;
    public Vector2 screenPos;
    public float mag;

    public bool twoFingersTouch;
    public bool touchOnCanvas;


    private void detectTouchOnCanvas(LeanFinger fing)
    {
        if (fing.Down)
        {
            if (fing.IsOverGui)
            {
                touchOnCanvas = true;
            }
            else
            {
                touchOnCanvas = false;
            }
            initPosition = fing.ScreenPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (LeanTouch.Fingers.Count == 0 && twoFingersTouch)
        {
            twoFingersTouch = false;
        }
        if(LeanTouch.Fingers.Count == 1)
        {
            LeanFinger finger = LeanTouch.Fingers[0];
            if (canInteract)
            {
                if (GameController.instance.characterController.charReached)
                {
                    detectTouchOnCanvas(finger);
                    if (finger.Up && !touchOnCanvas)
                    {
                        if (TutorialController.instance != null)
                        {
                            TutorialController.instance.hideTutorial();
                        }
                        screenPos = finger.ScreenPosition;
                        mag = (screenPos - initPosition).magnitude;
                        if (mag < 10)
                        {
                            Ray ray = Camera.main.ScreenPointToRay(initPosition);
                            RaycastHit raycastHit;
                            if (Physics.Raycast(ray, out raycastHit))
                            {
                                if (raycastHit.collider.tag == "Gear")
                                {
                                    Hashtable hash = new Hashtable();
                                    hash.Add("time", 0.5f);
                                    hash.Add("easetype", iTween.EaseType.linear);
                                    hash.Add("looptype", iTween.LoopType.none);

                                    GameController.instance.gearController.updatePathTiles(raycastHit.collider.gameObject, hash);
                                }
                                else
                                {
                                    GameObject sourceObject = null;
                                    GameObject destObject = raycastHit.collider.gameObject; ;
                                    Vector3 rayPos = GameController.instance.characterController.gameObject.transform.position + GameController.instance.characterController.charOffset;
                                    RaycastHit hit;
                                    if (Physics.Raycast(rayPos, Vector3.down, out hit))
                                    {
                                        if (hit.collider.tag == "Path")
                                        {
                                            sourceObject = hit.collider.gameObject;
                                        }
                                        if (hit.collider.tag == "Obstacle")
                                        {
                                            sourceObject = hit.collider.gameObject.GetComponent<ObstacleSpin>().pathTile;
                                        }
                                    }
                                    if (raycastHit.collider.tag == "Obstacle")
                                    {
                                        destObject = raycastHit.collider.gameObject.GetComponent<ObstacleSpin>().pathTile;
                                    }
                                    GameController.instance.characterController.updateCharMotion(sourceObject, destObject);
                                }
                            }
                        }
                        else
                        {
                            float val = screenPos.x - initPosition.x;
                            GameController.instance.cameraFollow.rotateCamera(val / Mathf.Abs(val));
                        }
                    }
                }
            }
        }
    }
}
