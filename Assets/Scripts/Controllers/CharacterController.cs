using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public bool charReached;
    public bool canMove;
    public Animator charAnim;
    public Vector3 currPos;
    public Vector3 nextPos;
    private RaycastHit rayCastHit;
    public Vector3 charOffset;
    public bool failPopupShown;
    public bool movedOnce;

    public GameObject currCharTile;
    public CapsuleCollider capsuleCollider;

    [Range(0,3)]
    public int health;

    public void init()
    {
        charReached = true;
        canMove = true;
        failPopupShown = false;
        charAnim = GetComponent<Animator>();
        //charOffset = new Vector3(0, 0.2757799f, 0);
        health = 3;
        showHealth();

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            currCharTile = hit.collider.gameObject;
        }

        capsuleCollider = GetComponent<CapsuleCollider>();
        StartCoroutine(setCharOffset());
    }

    private IEnumerator setCharOffset()
    {
        yield return new WaitForSeconds(0.5f);
        charOffset = transform.position - currCharTile.transform.position;
        transform.position = currCharTile.transform.position + charOffset;
        charOffset.x = 0;
        charOffset.z = 0;
    }

    public void updateCharMotion(GameObject sourceObject, GameObject destObject)
    {
        if (sourceObject!= null && destObject != null)
        {
            if (sourceObject != destObject)
            {
                GameController.instance.pathController.highlightPath(destObject);
                List<GameObject> pathWay = GameController.instance.pathController.findPath(sourceObject, destObject);
                if (pathWay != null)
                {
                    charReached = false;
                    canMove = true;

                    charAnim.SetBool("canWalk", true);
                    StartCoroutine(moveCharacter(pathWay));
                }
            }
        }
    }

    private IEnumerator moveCharacter(List<GameObject> paths)
    {
        GameObject destTile = paths[paths.Count - 1];

        GameObject currentTile = paths[0];
        GameObject nextTile = paths[1];

        while(currentTile != destTile)
        {
            currPos = currentTile.transform.position;
            currPos.y += charOffset.y;

            nextPos = nextTile.transform.position;
            nextPos.y = currPos.y;

            float xDist = Mathf.Abs(currPos.x - nextPos.x);
            float zDist = Mathf.Abs(currPos.z - nextPos.z);

            if (xDist > zDist)
                nextPos.z = currPos.z;

            if (xDist < zDist)
                nextPos.x = currPos.x;

            while (currPos != nextPos && canMove)
            {
                if (Physics.Raycast(transform.position + charOffset, Vector3.down, out rayCastHit))
                {
                    if (rayCastHit.collider.tag == "Path")
                    {
                        movedOnce = false;
                        nextPos.y = rayCastHit.collider.gameObject.transform.position.y + charOffset.y;
                        if (currPos.y < nextPos.y)
                        {
                            nextPos.y = currPos.y;
                        }
                        transform.position = Vector3.MoveTowards(transform.position, nextPos, 5 * Time.deltaTime);

                        if (!charAnim.GetBool("canWalk"))
                            charAnim.SetBool("canWalk", true);
                    }

                    else
                    {
                        if (!movedOnce)
                        {
                            movedOnce = true;
                            transform.position = Vector3.MoveTowards(transform.position, nextPos, 10 * Time.deltaTime);
                        }
                        transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.up, 5 * Time.deltaTime);

                        if (charAnim.GetBool("canWalk"))
                            charAnim.SetBool("canWalk", false);
                    }
                }
                else
                {
                    if (!movedOnce)
                    {
                        movedOnce = true;
                        transform.position = Vector3.MoveTowards(transform.position, nextPos, 10 * Time.deltaTime);
                    }
                    else
                    {
                        canMove = false;
                    }
                }

                transform.LookAt(nextPos, Vector3.up);
                currPos = transform.position;
                yield return new WaitForEndOfFrame();
                //GameController.instance.cameraFollow.updateCameraPosition();
            }
            if (!canMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, nextPos, 10 * Time.deltaTime);
                transform.LookAt(nextPos, Vector3.up);
                charAnim.SetBool("canWalk", false);
                break;
            }
            currentTile = nextTile;
            currCharTile = currentTile;

            if (nextTile != destTile)
            {
                int index = paths.IndexOf(nextTile);
                nextTile = paths[index + 1];
            }
        }
        if(currentTile == destTile)
        {
            charReached = true;
            charAnim.SetBool("canWalk", false);

            int tID = PlayerPrefs.GetInt("tutorialID", -1);

            if (tID == 2 || tID == 4)
            {
                if (TutorialController.instance != null)
                {
                    TutorialController.instance.playTutorial(tID);
                    PlayerPrefs.SetInt("tutorialID", tID + 1);
                }
            }

            if (currentTile.name.Contains("EndTile"))
            {
                GameController.instance.isLevelCompleted = true;
                GameController.instance.calculateStarsEarned();
                GameController.instance.showVictoryPopup();
            }
        }
    }

    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        if(transform.position.y < -20f)
        {
            reduceHealth();
            respawnCharacter();
            canMove = false;
            charReached = true;
        }
        if (charReached && !GameController.instance.inputController.canInteract && !GameController.instance.cameraFollow.cameraRotating && !GameController.instance.isLevelCompleted)
        {
            transform.position = currCharTile.transform.position + charOffset;
        }
    }
    private void respawnCharacter()
    {
        transform.position = currCharTile.transform.position + new Vector3(0, 1.5f, 0);
    }

    private IEnumerator enableCollision(GameObject collision)
    {
        yield return new WaitForSeconds(0.5f);
        collision.GetComponent<Collider>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Obstacle")
        {
            collision.collider.enabled = false;
            StartCoroutine(enableCollision(collision.collider.gameObject));
            reduceHealth();
        }
        if(collision.collider.tag == "Collectible")
        {
            moveCollectible(collision.collider.gameObject);
            
        }
    }
    private void moveCollectible(GameObject sourceObj)
    {
        GameController.instance.starsEarned += 1;
        Destroy(sourceObj);

        Hashtable hash = new Hashtable();
        hash.Add("diamond", 1);
        AudioManager.instance.PlaySound("Coin Collect");
        EconomyController.instance.updateAllCurrencies(hash);
    }

    private void reduceHealth()
    {
        health -= 1;
        showHealth();
        if(health == 0)
        {
            canMove = false;
            GameController.instance.showFailPopup();
        }
    }

    private void showHealth()
    {
        Transform healthParent = GameController.instance.mainCanvasController.healthBarParent;
        for (int i = 0; i < healthParent.childCount; i++)
        {
            Image healthFillImg = healthParent.GetChild(i).GetChild(0).GetComponent<Image>();
            healthFillImg.fillAmount = 0;
            if(i < health)
            {
                healthFillImg.fillAmount = 1;
            }
        }
    }
}