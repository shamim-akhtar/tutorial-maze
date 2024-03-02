using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{

  public class MazeGrid : MonoBehaviour
  {
    [SerializeField]
    GameObject roomPrefab;

    Room[,] rooms = null;
    [SerializeField]
    int numX = 10;
    int numY = 10;

    float roomWidth;
    float roomHeight;

    Stack<Room> stack = new Stack<Room>();

    private void GetRoomSize()
    {
      // Get all child sprites
      SpriteRenderer[] spriteRenderers = roomPrefab.GetComponentsInChildren<SpriteRenderer>();

      // Initialize variables to store min and max bounds
      Vector3 minBounds = Vector3.positiveInfinity;
      Vector3 maxBounds = Vector3.negativeInfinity;

      // Iterate through all child sprites and update min and max bounds
      foreach (SpriteRenderer spriteRenderer in spriteRenderers)
      {
        minBounds = Vector3.Min(minBounds, spriteRenderer.bounds.min);
        maxBounds = Vector3.Max(maxBounds, spriteRenderer.bounds.max);
      }

      // Calculate width and height
      roomWidth = maxBounds.x - minBounds.x;
      roomHeight = maxBounds.y - minBounds.y;

      //return new Tuple<float, float>(width, height);

    }

    private void Start()
    {
      GetRoomSize();

      rooms = new Room[numX, numY];
      for (int i = 0; i < numX; ++i)
      {
        for (int j = 0; j < numY; ++j)
        {
          GameObject room = Instantiate(roomPrefab,
            new Vector3(i * roomWidth, j * roomHeight, 0.0f),
            Quaternion.identity);

          room.name = "Room_" + i.ToString() + "_" + j.ToString();
          rooms[i, j] = room.GetComponent<Room>();
          rooms[i, j].Index = new Vector2Int(i, j);
        }
      }

      SetCamera();

      CreateNewMaze();
    }

    private void SetCamera()
    {
      Camera.main.transform.position = new Vector3(
        numX * (roomWidth - 1) / 2,
        numY * (roomHeight - 1) / 2,
        -100.0f);
      float min_value = Mathf.Min(numX * roomWidth, numY * roomHeight);
      Camera.main.orthographicSize = min_value * 0.75f;
    }

    public void RemoveRoomWall(
      int x,
      int y,
      Directions dir)
    {
      if (dir != Directions.NONE)
      {
        rooms[x, y].SetDirFlag(dir, false);
      }

      Directions opp = Directions.NONE;
      switch (dir)
      {
        case Directions.UP:
          if (y < numY - 1)
          {
            opp = Directions.DOWN;
            ++y;
          }
          break;
        case Directions.RIGHT:
          if (x < numX - 1)
          {
            opp = Directions.LEFT;
            ++x;
          }
          break;
        case Directions.DOWN:
          if (y > 0)
          {
            opp = Directions.UP;
            --y;
          }
          break;
        case Directions.LEFT:
          if (x > 0)
          {
            opp = Directions.RIGHT;
            --x;
          }
          break;
      }

      if (opp != Directions.NONE)
      {
        rooms[x, y].SetDirFlag(opp, false);
      }
    }

    public void CreateNewMaze()
    {
      // Remove the left wall from 
      // the bottom left cell.
      RemoveRoomWall(
        0,
        0,
        Directions.DOWN);

      // Remove the right wall from 
      // the top right cell.
      RemoveRoomWall(
        numX - 1,
        numY - 1,
        Directions.RIGHT);

      // Push the first cell into the stack.
      stack.Push(rooms[0, 0]);

      // Generate the maze in a coroutine 
      // so that we can see the progress of the
      // maze generation in progress.
      StartCoroutine(Coroutine_Generate());
    }

    public void HighlightRoom(int i, int j, bool flag)
    {
      rooms[i, j].SetHighlight(flag);
    }

    public void RemoveAllHightlights()
    {

      for (int i = 0; i < numX; ++i)
      {
        for (int j = 0; j < numY; ++j)
        {
          rooms[i, j].SetHighlight(false);
        }
      }
    }

    public void OnRoomSetDirFlag(
      int x,
      int y,
      Directions dir,
      bool f)
    {
      rooms[x, y].SetActive(dir, f);
    }

    public List<Tuple<Directions, Room>>
      GetNeighboursNotVisited(
      int cx,
      int cy)
    {
      List<Tuple<Directions, Room>> neighbours =
        new List<Tuple<Directions, Room>>();

      foreach (Directions dir in Enum.GetValues(
        typeof(Directions)))
      {
        int x = cx;
        int y = cy;

        switch (dir)
        {
          case Directions.UP:
            if (y < numY - 1)
            {
              ++y;
              if (!rooms[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Room>(
                  Directions.UP,
                  rooms[x, y])
                );
              }
            }
            break;
          case Directions.RIGHT:
            if (x < numX - 1)
            {
              ++x;
              if (!rooms[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Room>(
                  Directions.RIGHT,
                  rooms[x, y])
                );
              }
            }
            break;
          case Directions.DOWN:
            if (y > 0)
            {
              --y;
              if (!rooms[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Room>(
                  Directions.DOWN,
                  rooms[x, y])
                );
              }
            }
            break;
          case Directions.LEFT:
            if (x > 0)
            {
              --x;
              if (!rooms[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Room>(
                  Directions.LEFT,
                  rooms[x, y])
                );
              }
            }
            break;
          default:
            break;
        }
      }
      return neighbours;
    }

    bool GenerateStep()
    {
      if (stack.Count == 0) return true;

      Room c = stack.Peek();
      var neighbours = GetNeighboursNotVisited(c.Index.x, c.Index.y);

      if (neighbours.Count != 0)
      {
        var index = 0;
        if (neighbours.Count > 1)
        {
          index = UnityEngine.Random.Range(0, neighbours.Count);
        }
        var item = neighbours[index];
        Room neighbour = item.Item2;
        neighbour.visited = true;
        RemoveRoomWall(c.Index.x, c.Index.y, item.Item1);

        rooms[c.Index.x, c.Index.y].SetHighlight(true);

        stack.Push(neighbour);
      }
      else
      {
        stack.Pop();
        rooms[c.Index.x, c.Index.y].SetHighlight(false);
      }
      return false;
    }

    IEnumerator Coroutine_Generate()
    {
      bool flag = false;
      while (!flag)
      {
        flag = GenerateStep();
        //yield return null;
        yield return new WaitForSeconds(0.01f);
      }
      //MazeGenerationCompleted = true;
    }
  }
}