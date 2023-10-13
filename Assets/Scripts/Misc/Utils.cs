using System.Collections;
using UnityEngine;

public class Utils: MonoBehaviour
{
    public static void animateHUD(GameObject source, GameObject destination, bool isSourceCanvas, bool isDestinationCanvas, GameObject prefab, float duration)
    {
        Canvas[] c;
        if (isSourceCanvas == true)
            c = source.GetComponentsInParent<Canvas>();
        else
            c = destination.GetComponentsInParent<Canvas>();

        if (c.Length <= 0)
        {
            Debug.LogError("Source or Destination, one of them has to be Canvas element");
            return;
        }

        Canvas topmost = c[c.Length - 1];

        GameObject animatedObj = prefab;
        animatedObj.transform.parent = topmost.transform;

        Vector3 sourcePos = source.transform.position;
        if (isSourceCanvas == false)
        {
            var sourceScreenPoint = Camera.main.WorldToScreenPoint(sourcePos);
            sourcePos = new Vector3(sourceScreenPoint.x, sourceScreenPoint.y, 0); // As we are tweening in Canvas, make 'z-axis' zero.
        }

        Vector3 destPosition = destination.transform.position;
        if (isDestinationCanvas == false)
        {
            var destinationScreenPoint = Camera.main.WorldToScreenPoint(destPosition);
            destPosition = new Vector3(destinationScreenPoint.x, destinationScreenPoint.y, 0); // As we are tweening in Canvas, make 'z-axis' zero.
        }

        animatedObj.transform.position = sourcePos;

        //Debug.LogError("Source1: " + sourcePos);
        //Debug.LogError("Dest1: " + destPosition);
        ////destPosition = Camera.main.WorldToScreenPoint(destPosition);
        //Debug.LogError("Dest2: " + destPosition);



        //Vector2 midPoint = new Vector2((sourcePos.x - destPosition.x) / 4.0f, destPosition.y);
        Vector3 midPoint = new Vector3((sourcePos.x + destPosition.x) / 2f, (sourcePos.y + destPosition.y) / 2f, (sourcePos.z + destPosition.z) / 2f);
        if (isSourceCanvas == false || isDestinationCanvas == false)
        {
            midPoint.x = Mathf.Clamp(midPoint.x + 500, 0, Screen.width - 50);
            midPoint.y = Mathf.Clamp(midPoint.y + 300, 0, Screen.height - 50);
        }
        else
            midPoint.x = midPoint.x - 200;
        
        Vector3[] curvePath = new Vector3[2];
        curvePath[0] = midPoint;
        curvePath[1] = destPosition;

        Hashtable moveHash = new Hashtable
        {
            { "path", curvePath},
            { "time", duration },
            { "islocal", false },
            { "easetype", iTween.EaseType.easeInOutSine }
        };
        iTween.MoveTo(animatedObj, moveHash);
        Hashtable scaleHash = new Hashtable
        {
            { "scale", Vector3.one * 1.5f},
            { "time", (duration - 0.2f) },
            { "delay", 0.2f },
            { "easetype", iTween.EaseType.linear }
        };
        iTween.ScaleTo(animatedObj, scaleHash);
    }
}