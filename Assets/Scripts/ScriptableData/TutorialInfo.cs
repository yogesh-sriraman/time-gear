using UnityEngine;

[CreateAssetMenu(fileName = "TutorialInfo", menuName = "Tutorial Info", order = 51)]
public class TutorialInfo: ScriptableObject
{
    public enum Shapes
    {
        CIRCLE,
        RECTANGLE
    }

	public int id;
	public string uid = "";
	public string event_name = "";
	public string event_target = "";
	public string speech = "";
	public int arrowpos;
	public int next = -1;
	public Shapes shape;
	public bool hideSpotLight;
}