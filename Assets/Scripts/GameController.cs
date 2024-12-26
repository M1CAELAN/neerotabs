using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]               // 1

public class GameController : MonoBehaviour
{
    private MazeConstructor generator;

    public int[,] data
    {
        get; private set;
    }

    void Start()
    {

    }
}