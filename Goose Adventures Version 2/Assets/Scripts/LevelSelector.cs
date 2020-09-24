using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField]
    GameObject selector;

    [SerializeField]
    GameObject[] row1;

    [SerializeField]
    GameObject[] row2;

    [SerializeField]
    GameObject[] row3;

    //Constant number of columns and rows
    const int cols = 3;
    const int rows = 3;

    //Keep track of the position on the level selection grid
    Vector2 positionIndex;
    //current Slot selected
    GameObject currentSlot;

    bool Moving = false;

    //Create 2D grid
    public GameObject[,] grid = new GameObject[cols, rows];


    //Start is called before the first frame update
    void Start()
    {
        AddRowToGrid(0, row1);
        AddRowToGrid(1, row2);
        AddRowToGrid(2, row3);

        positionIndex = new Vector2(1, 1);
        currentSlot = grid[1, 1];
    }

    void AddRowToGrid(int index, GameObject[] row)
    {
        for (int i = 0; i < 3; i++)
        {
            grid[index, i] = row[i];
        }
    }

    //Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");

        if (xAxis > 0)
        {
            //inputright
            MoveSelector("right");
        }
        else if (xAxis < 0)
        {
            //inputleft
            MoveSelector("left");
        }

        else if (yAxis > 0)
        {
            //inputup
            MoveSelector("up");
        }
        else if (yAxis < 0)
        {
            //inputdown
            MoveSelector("down");
        }
    }

    void MoveSelector(string direction)
    {
        if (Moving == false)
        {
            Moving = true;

            if(direction == "right")
            {
                if(positionIndex.x < cols - 1)
                {
                    positionIndex.x += 1;
                }
            }

            else if(direction == "left")
            {
                if(positionIndex.x > 0)
                {
                    positionIndex.x -= 1;
                }
            }

            else if(direction == "up")
            {
                if(positionIndex.y > 0)
                {
                    positionIndex.y -= 1;
                }
            }

            else if(direction == "down")
            {
                if(positionIndex.y < rows - 1)
                {
                    positionIndex.y += 1;
                }
            }


            currentSlot = grid[(int)positionIndex.y, (int)positionIndex.x];
            selector.transform.position = currentSlot.transform.position;

            Invoke("ResetMovement", 0.1f);
        }
    }
    void ResetMovement()
    {
        Moving = false;
    }
}