using UnityEngine;

public class MazeConstructor : MonoBehaviour
{
    private MazeMeshGenerator meshGenerator;
    private MazeMeshGenerator2 meshGenerator2;
    private MazeDataGenerator dataGenerator;

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
    public int[,] data
    {
        get; private set;
    }
    public float hallWidth
    {
        get; private set;
    }
    public float hallHeight
    {
        get; private set;
    }
    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }
    void Awake()
    {
        meshGenerator = new MazeMeshGenerator();
        meshGenerator2 = new MazeMeshGenerator2();
        dataGenerator = new MazeDataGenerator();

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
        FindStartPosition();
        FindGoalPosition();
        DisplayMaze();
    }
    void OnGUI()
    {
        if (!showDebug)
        {
            return;
        }

        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";

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

        GUI.Label(new Rect(20, 20, 500, 500), msg);
    }
    public void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
        GameObject[] objects2 = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject go2 in objects2)
        {
            Destroy(go2);
        }
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
    private void FindGoalPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        // loop top to bottom, right to left
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = cMax; j >= 0; j--)
            {
                if (maze[i, j] == 0)
                {
                    goalRow = i;
                    goalCol = j;
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

