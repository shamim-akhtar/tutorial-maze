using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Procedural;
using GameAI.PathFinding;
using System.Collections.Generic;

public class MazePathfinder : MonoBehaviour
{
  public Transform mDestination;
  public NPC mNpc;
  public MazeGenerator mMazeGenerator;

  public Color COLOR_WALKABLE = new Color(42 / 255.0f, 99 / 255.0f, 164 / 255.0f, 1.0f);
  public Color COLOR_CURRENT_NODE = new Color(0.5f, 0.4f, 0.1f, 1.0f);
  public Color COLOR_ADD_TO_OPEN_LIST = new Color(0.2f, 0.7f, 0.5f, 1.0f);
  public Color COLOR_ADD_TO_CLOSED_LIST = new Color(0.5f, 0.5f, 0.5f, 1.0f);

  Cell mStart;
  Cell mGoal;

  LineRenderer mPathViz;

  AStarPathFinder<Vector2Int> mPathFinder = new AStarPathFinder<Vector2Int>();

  // Start is called before the first frame update
  void Start()
  {
    mDestination.gameObject.SetActive(false);

    // wait for maze generation to complete.
    StartCoroutine(Coroutine_WaitForMazeGeneration());
  }

  IEnumerator Coroutine_WaitForMazeGeneration()
  {
    while (!mMazeGenerator.MazeGenerationCompleted)
      yield return null;

    // maze generation completed.
    // Set the NPC to cell 0, 0
    mNpc.transform.position = mMazeGenerator.mMazeCells[0, 0].transform.position;
    mStart = mMazeGenerator.mMazeCells[0, 0].Cell;
    mGoal = mStart;

    // create the line renderer to show the path.
    // We create a line renderer to show the path.
    LineRenderer lr = mNpc.gameObject.AddComponent<LineRenderer>();
    lr.startWidth = 0.1f;
    lr.endWidth = 0.1f;
    lr.startColor = Color.magenta;
    lr.endColor = Color.magenta;

    mPathViz = lr;

    // set the pathfinder cost functions.

    mPathFinder.HeuristicCost = GetManhattanCost;
    mPathFinder.NodeTraversalCost = GetEuclideanCost;
    // to show pathfinding progress.

    mPathFinder.onChangeCurrentNode = OnChangeCurrentNode;
    mPathFinder.onAddToClosedList = OnAddToClosedList;
    mPathFinder.onAddToOpenList = OnAddToOpenList;
  }

  public void OnChangeCurrentNode(PathFinder<Vector2Int>.PathFinderNode node)
  {
    int x = node.Location.Value.x;
    int y = node.Location.Value.y;
    MazeCell mazeCell = mMazeGenerator.mMazeCells[x, y];
    mazeCell.SetHighColor(COLOR_CURRENT_NODE);
    mazeCell.SetHighlight(true);
  }
  public void OnAddToOpenList(PathFinder<Vector2Int>.PathFinderNode node)
  {
    int x = node.Location.Value.x;
    int y = node.Location.Value.y;
    MazeCell mazeCell = mMazeGenerator.mMazeCells[x, y];
    mazeCell.SetHighColor(COLOR_ADD_TO_OPEN_LIST);
    mazeCell.SetHighlight(true);
  }
  public void OnAddToClosedList(PathFinder<Vector2Int>.PathFinderNode node)
  {
    int x = node.Location.Value.x;
    int y = node.Location.Value.y;
    MazeCell mazeCell = mMazeGenerator.mMazeCells[x, y];
    mazeCell.SetHighColor(COLOR_ADD_TO_CLOSED_LIST);
    mazeCell.SetHighlight(true);
  }

  // Update is called once per frame
  void Update()
  {
    if (!mMazeGenerator.MazeGenerationCompleted)
      return;

    if (Input.GetMouseButtonDown(0))
    {
      RayCastAndSetDestination();
    }
  }

  void RayCastAndSetDestination()
  {
    // disable picking if we hit the UI.
    if (EventSystem.current.IsPointerOverGameObject() || enabled == false)
    {
      return;
    }

    Vector2 rayPos = new Vector2(
        Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
        Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

    if (hit)
    {
      GameObject obj = hit.transform.gameObject;
      MazeCell mazeCell = obj.GetComponent<MazeCell>();
      if (mazeCell == null) return;

      Vector3 pos = mDestination.position;
      pos.x = mazeCell.transform.position.x;
      pos.y = mazeCell.transform.position.y;
      mDestination.position = pos;

      mDestination.gameObject.SetActive(true);

      //mNpc.AddWayPoint(new Vector2(obj.transform.position.x, obj.transform.position.y));

      mStart = mGoal;
      mGoal = mazeCell.Cell;
      FindPath();
    }
  }
  public void FindPath()
  {
    mPathFinder.Initialize(mStart, mGoal);
    StartCoroutine(Coroutine_FindPathSteps());

    // reset the line.
    mPathViz.positionCount = 0;
    for(int i = 0; i < mMazeGenerator.maze.NumCols; ++i)
    {
      for (int j = 0; j < mMazeGenerator.maze.NumRows; ++j)
      {
        mMazeGenerator.mMazeCells[i, j].SetHighColor(COLOR_WALKABLE);
        mMazeGenerator.mMazeCells[i, j].SetHighlight(false);
      }
    }
  }
  IEnumerator Coroutine_FindPathSteps()
  {
    while (mPathFinder.Status == PathFinderStatus.RUNNING)
    {
      mPathFinder.Step();
      yield return null;
    }

    if (mPathFinder.Status == PathFinderStatus.SUCCESS)
    {
      OnPathFound();
    }
    else if (mPathFinder.Status == PathFinderStatus.FAILURE)
    {
      OnPathNotFound();
    }
  }
  public void OnPathFound()
  {
    PathFinder<Vector2Int>.PathFinderNode node = null;

    node = mPathFinder.CurrentNode;

    List<Vector3> reverse_positions = new List<Vector3>();

    while (node != null)
    {
      Vector3 pos = mMazeGenerator.mMazeCells[
        node.Location.Value.x, 
        node.Location.Value.y].transform.position;
      reverse_positions.Add(pos);
      node = node.Parent;
    }

    LineRenderer lr = mPathViz;
    lr.positionCount = reverse_positions.Count;
    for (int i = reverse_positions.Count - 1; i >= 0; i--)
    {
      Vector3 p = reverse_positions[i];
      mNpc.AddWayPoint(new Vector2(
        p.x,
        p.y));

      lr.SetPosition(i, new Vector3(
        p.x,
        p.y,
        -2.0f));
    }

    // now change the start cell
    // for the pathfinder to current
    // goal.
    mStart = mGoal;
  }

  void OnPathNotFound()
  {
    Debug.Log("Cannot find path to destination");
  }

  public float GetManhattanCost(
    Vector2Int a,
    Vector2Int b)
  {
    return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
  }

  public float GetEuclideanCost(
    Vector2Int a,
    Vector2Int b)
  {
    return GetCostBetweenTwoCells(a, b);
  }

  public float GetCostBetweenTwoCells(
    Vector2Int a,
    Vector2Int b)
  {
    return (mMazeGenerator.mMazeCells[a.x, a.y].transform.position -
      mMazeGenerator.mMazeCells[b.x, b.y].transform.position).magnitude;
  }
}
