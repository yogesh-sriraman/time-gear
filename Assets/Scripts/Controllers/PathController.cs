using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PathController : MonoBehaviour
{
    public GameObject pathParent;

    [SerializeField]
    public List<GameObject> resultPath = new List<GameObject>();

    public List<GameObject> pathVisited = new List<GameObject>();

    [SerializeField]
    public List<Path> paths;

    public void updateFromJson(string pathConfig, string currentSceneName)
    {
        clearPaths();
        Levels levelData = JsonConvert.DeserializeObject<Levels>(pathConfig);
        Paths p = null;

        //int level = int.Parse(currentSceneName.Split('_')[currentSceneName.Split('_').Length - 1]);
        for(int i = 0; i < levelData.paths.Length; i++)
        {
            if(currentSceneName == levelData.paths[i].levelID)
            {
                p = levelData.paths[i];
                break;
            }
        }

        if(p == null)
        {
            Debug.Log("Not configured!!!!");
            return;
        }

        for(int i = 0; i < p.pathData.Length; i++)
        {
            addToPathList(p.pathData[i]);
        }
    }

    public void highlightPath(GameObject touchedPath)
    {
        GameController.instance.touchDisplay.transform.position = touchedPath.transform.position + new Vector3(0, 0.3f, 0);

        GameController.instance.touchDisplay.Play();
    }

    private void addToPathList(PathData pData)
    {
        Path path = new Path();
        path.pathObject = GameObject.Find(pData.pathObjName);
        path.pathPosition = path.pathObject.transform.position;
        path.pathRotation = path.pathObject.transform.eulerAngles;
        path.adjacentPaths = getAdjacentPaths(pData);
        updateCosts(path);
        checkMoveDirection(path, pData);
        path.xPos = pData.xPos;
        path.zPos = pData.zPos;
        paths.Add(path);
    }

    private void checkMoveDirection(Path path, PathData pData)
    {
        path.moveX = false;
        path.moveZ = false;
        if (Mathf.Approximately(pData.xPos, path.pathPosition.x))
        {
            path.moveZ = true;
            if (pData.zPos < path.pathPosition.z)
                path.moveZOffset = -2;
            else
                path.moveZOffset = 2;
        }
        if (Mathf.Approximately(pData.zPos, path.pathPosition.z))
        {
            path.moveX = true;
            if (pData.xPos < path.pathPosition.x)
                path.moveXOffset = -2;
            else
                path.moveXOffset = 2;
        }
    }

    private List<GameObject> getAdjacentPaths(PathData pData)
    {
        List<GameObject> adj = new List<GameObject>();

        for(int i = 0; i < pData.adjacentPathNames.Length; i++)
        {
            adj.Add(GameObject.Find(pData.adjacentPathNames[i].pathName));
        }

        return adj;
    }

    public void clearPaths()
    {
        paths.Clear();
    }

    public void savePaths()
    {
        Levels levels = new Levels();
        levels.paths = new Paths[1];

        levels.paths[0] = new Paths();
        levels.paths[0].levelID = "Hello";
        levels.paths[0].pathData = new PathData[paths.Count];
        for(int i = 0; i < paths.Count; i++)
        {
            Path path = paths[i];
            levels.paths[0].pathData[i] = new PathData();
            levels.paths[0].pathData[i].pathObjName = path.pathObject.name;
            levels.paths[0].pathData[i].xPos = path.pathPosition.x;
            levels.paths[0].pathData[i].zPos = path.pathPosition.z;
            levels.paths[0].pathData[i].adjacentPathNames = new AdjPaths[path.adjacentPaths.Count];
            for (int j = 0; j < path.adjacentPaths.Count; j++)
            {
                levels.paths[0].pathData[i].adjacentPathNames[j] = new AdjPaths();
                levels.paths[0].pathData[i].adjacentPathNames[j].pathName = path.adjacentPaths[j].name;
            }
        }
        string configData = JsonConvert.SerializeObject(levels, Formatting.Indented);
        Debug.Log(configData);
    }

    public void updatePathTiles()
    {
        clearPaths();
        paths = new List<Path>();
        for (int i = 0; i < pathParent.transform.childCount; i++)
        {
            Transform pathObj = pathParent.transform.GetChild(i);
            Path path = new Path();
            path.pathObject = pathObj.gameObject;
            path.pathPosition = pathObj.position;
            path.adjacentPaths = updateAdjacentPaths(pathObj);
            paths.Add(path);
        }
    }

    public void updateCosts(Path path)
    {
        path.gCost = Mathf.Infinity;
        path.fCost = Mathf.Infinity;
        path.parentPath = null;
    }

    public List<GameObject> updateAdjacentPaths(Transform pObject)
    {
        List<GameObject> adjObjs = new List<GameObject>();
        float boundOffset = 0.5f;
        RaycastHit hit; ;

        Collider objectCollider = pObject.GetComponent<Collider>();
        Vector3 centrePos = objectCollider.bounds.center;
        Vector3 boundExtents = objectCollider.bounds.extents;

        float xBoundPositive = centrePos.x + boundExtents.x + boundOffset;
        float xBoundNegative = centrePos.x - boundExtents.x - boundOffset;

        float zBoundPositive = centrePos.z + boundExtents.z + boundOffset;
        float zBoundNegative = centrePos.z - boundExtents.z - boundOffset;

        Vector3 posXDir = new Vector3(xBoundPositive, 10f, centrePos.z);
        Vector3 negXDir = new Vector3(xBoundNegative, 10f, centrePos.z);
        Vector3 posZDir = new Vector3(centrePos.x, 10f, zBoundPositive);
        Vector3 negZDir = new Vector3(centrePos.x, 10f, zBoundNegative);

        if(Physics.Raycast(posXDir, Vector3.down, out hit))
        {
            if(hit.collider.tag == "Path")
            {
                adjObjs.Add(hit.collider.gameObject);
            }
        }
        if (Physics.Raycast(negXDir, Vector3.down, out hit))
        {
            if (hit.collider.tag == "Path")
            {
                adjObjs.Add(hit.collider.gameObject);
            }
        }
        if (Physics.Raycast(posZDir, Vector3.down, out hit))
        {
            if (hit.collider.tag == "Path")
            {
                adjObjs.Add(hit.collider.gameObject);
            }
        }
        if (Physics.Raycast(negZDir, Vector3.down, out hit))
        {
            if (hit.collider.tag == "Path")
            {
                adjObjs.Add(hit.collider.gameObject);
            }
        }

        return adjObjs;
    }

    public List<GameObject> findPath(GameObject fromTile, GameObject toTile)
    {
        if (fromTile == null || toTile == null)
            return null;

        if (fromTile == toTile)
            return null;

        pathVisited.Clear();
        resultPath.Clear();

        Path fromPath = null;
        Path toPath = null;

        for (int i = 0; i < paths.Count; i++)
        {
            if(paths[i].pathObject == fromTile)
            {
                fromPath = paths[i];
            }
            if (paths[i].pathObject == toTile)
            {
                toPath = paths[i];
            }
            if (fromPath != null && toPath != null)
                break;
        }

        for (int i = 0; i < pathParent.transform.childCount; i++)
        {
            Transform pathObj = pathParent.transform.GetChild(i);
            Path pathObject = getPathFromObject(pathObj.gameObject);
            updateCosts(pathObject);
        }
        resultPath.Clear();
        resultPath = findPathToDest(fromPath, toPath);
        resultPath.Add(fromPath.pathObject);
        resultPath.Reverse();
        return resultPath;
    }

    public List<GameObject> findPathToDest(Path sourcePath, Path destPath)
    {
        List<Path> openSet = new List<Path>();
        List<Path> closeSet = new List<Path>();

        openSet.Add(sourcePath);
        sourcePath.gCost = 0;
        sourcePath.hCost = calculateDistance(sourcePath.pathObject, destPath.pathObject);
        sourcePath.fCost = calculateFCost(sourcePath);

        while(openSet.Count > 0)
        {
            Path currentPath = getLowestFCostNode(openSet);
            if(currentPath == destPath)
            {
                return calculatePath(currentPath);
            }

            openSet.Remove(currentPath);
            closeSet.Add(currentPath);

            for(int i = 0; i < currentPath.adjacentPaths.Count; i++)
            {
                Path adjacentTile = getPathFromObject(currentPath.adjacentPaths[i]);
                if (closeSet.Contains(adjacentTile))
                    continue;

                float tempGCost = currentPath.gCost + calculateDistance(currentPath.pathObject, adjacentTile.pathObject);
                float tempHCost = calculateDistance(adjacentTile.pathObject, destPath.pathObject);
                float tempFCost = tempGCost + tempHCost;
                if(tempGCost < adjacentTile.gCost && tempFCost < adjacentTile.fCost)
                {
                    adjacentTile.parentPath = currentPath;
                    adjacentTile.gCost = tempGCost;
                    adjacentTile.hCost = calculateDistance(adjacentTile.pathObject, destPath.pathObject);
                    adjacentTile.fCost = calculateFCost(adjacentTile);

                    if(!openSet.Contains(adjacentTile))
                    {
                        openSet.Add(adjacentTile);
                    }
                }
            }
        }
        return null;
    }

    public List<GameObject> calculatePath(Path endTile)
    {
        List<GameObject> result = new List<GameObject>();
        Path currentPath = endTile;
        while(currentPath.parentPath != null)
        {
            result.Add(currentPath.pathObject);
            currentPath = currentPath.parentPath;
        }

        return result;
    }

    public float calculateDistance(GameObject object1, GameObject object2)
    {
        float xVal = Mathf.Abs(object1.transform.position.x - object2.transform.position.x);
        float zVal = Mathf.Abs(object1.transform.position.z - object2.transform.position.z);
        float dist = Mathf.Sqrt(Mathf.Pow(xVal, 2) + Mathf.Pow(zVal, 2));

        return dist;
    }

    public float calculateFCost(Path pathTile)
    {
        return pathTile.gCost + pathTile.hCost;
    }

    public Path getLowestFCostNode(List<Path> oList)
    {
        Path lowestNode = oList[0];
        for(int i = 1; i < oList.Count; i++)
        {
            if(oList[i].fCost < lowestNode.fCost)
            {
                lowestNode = oList[i];
            }
        }
        return lowestNode;
    }

    public Path getPathFromObject(GameObject pObject)
    {
        for(int i = 0; i < paths.Count; i++)
        {
            if (paths[i].pathObject == pObject)
                return paths[i];
        }
        return null;
    }

    public GameObject findObjectByPos(Vector3 charPosition)
    {
        for (int i = 0; i < paths.Count; i++)
        {
            float xDiff = Mathf.Abs(Mathf.Abs(paths[i].pathPosition.x) - Mathf.Abs(charPosition.x));
            float zDiff = Mathf.Abs(Mathf.Abs(paths[i].pathPosition.z) - Mathf.Abs(charPosition.z));
            if (0 <= xDiff && xDiff <= 1.5f && 0 <= zDiff && zDiff <= 1.5f)
            {
                return paths[i].pathObject;
            }
        }
        return null;
    }


    public bool isSameX(GameObject object1, GameObject object2)
    {
        return Mathf.Ceil(object1.transform.position.x - object2.transform.position.x) == 0;
    }

    public bool isSameZ(GameObject object1, GameObject object2)
    {
        return Mathf.Ceil(object1.transform.position.z - object2.transform.position.z) == 0;
    }
}

public class Levels
{
    public Paths[] paths;
}

public class Paths
{
    public string levelID;
    public PathData[] pathData;
}

[System.Serializable]
public class PathData
{
    public string pathObjName;
    public float xPos;
    public float zPos;
    public AdjPaths[] adjacentPathNames;
}

[System.Serializable]
public class AdjPaths
{
    public string pathName;
}

[System.Serializable]
public class Path
{
    public GameObject pathObject;
    public Vector3 pathPosition;
    public Vector3 pathRotation;
    public float xPos;
    public float zPos;
    public bool moveX;
    public bool moveZ;
    public int moveXOffset;
    public int moveZOffset;
    public float gCost;
    public float hCost;
    public float fCost;
    public Path parentPath;
    public List<GameObject> adjacentPaths;
}
