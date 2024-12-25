using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    private MazeMeshGenerator meshGenerator;
    private MazeMeshGenerator2 meshGenerator2;
    private MazeDataGenerator dataGenerator;
    //1
    public bool showDebug;

    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;

    public int startRow
    {
        get; private set;
    }
    public int startCol
    {
        get; private set;
    }
    //2
    public int[,] data
    {
        get; private set;
    }

    //3
    void Awake()
    {
        meshGenerator = new MazeMeshGenerator();
        meshGenerator2 = new MazeMeshGenerator2();
        dataGenerator = new MazeDataGenerator();
        // default to walls surrounding a single empty cell
        data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size.");
        }

        data = dataGenerator.FromDimensions(sizeRows, sizeCols);
        DisplayMaze();

    }
    void OnGUI()
    {
        //1
        if (!showDebug)
        {
            return;
        }

        //2
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

        //3
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    msg += "....";
                }
                else
                {
                    msg += "==";
                }
            }
            msg += "\n";
        }

        //4
        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }

    private void FindStartPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    startRow = i;
                    startCol = j;
                    return;
                }
            }
        }
    }
    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        GameObject go2 = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";
        go.layer = 3;

        go2.transform.position = Vector3.zero;
        go2.name = "Procedural Maze2";
        go2.tag = "Wall";

        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mazeMat1, mazeMat2 };

        MeshFilter mf2 = go2.AddComponent<MeshFilter>();
        mf2.mesh = meshGenerator2.FromData(data);

        MeshCollider mc2 = go2.AddComponent<MeshCollider>();
        mc2.sharedMesh = mf2.mesh;

        MeshRenderer mr2 = go2.AddComponent<MeshRenderer>();
        mr2.materials = new Material[2] { mazeMat1, mazeMat2 };
    }
}

