using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MazePathfinder : MonoBehaviour
{
  public Transform mDestination;
  public NPC mNpc;
  public MazeGenerator mMazeGenerator;

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
      //mGoal = sc.RectGridCell;
      mNpc.AddWayPoint(new Vector2(obj.transform.position.x, obj.transform.position.y));

      //FindPath();
    }
  }
  public void FindPath()
  {
    //mPathFinders[mPathFinderType][i].Initialize(mNPCStartPositions[i], mGoal);
    //StartCoroutine(Coroutine_FindPathSteps(i));
  }
  //IEnumerator Coroutine_FindPathSteps(int index)
  //{
  //  PathFinder<Vector2Int> pathFinder = mPathFinders[mPathFinderType][index];
  //  while (pathFinder.Status == PathFinderStatus.RUNNING)
  //  {
  //    pathFinder.Step();
  //    yield return null;
  //  }

  //  if (pathFinder.Status == PathFinderStatus.SUCCESS)
  //  {
  //    OnPathFound(index);
  //  }
  //  else if (pathFinder.Status == PathFinderStatus.FAILURE)
  //  {
  //    OnPathNotFound(index);
  //  }
  //}
  //public void OnPathFound(int index)
  //{
  //  if (mTextNotification)
  //  {
  //    mTextNotification.text = "Found path to destination";
  //  }
  //  PathFinder<Vector2Int>.PathFinderNode node = null;

  //  if (!mInteractive)
  //  {
  //    //ThreadedPathFinder<Vector2Int> tpf = mThreadedPool.GetThreadedPathFinder(index);
  //    //node = tpf.PathFinder.CurrentNode;
  //    node = mPathFinders[mPathFinderType][index].CurrentNode;
  //  }
  //  else
  //  {
  //    node = mPathFinders[mPathFinderType][index].CurrentNode;
  //  }

  //  SetFCost(node.Fcost);
  //  SetGCost(node.GCost);
  //  SetHCost(node.Hcost);

  //  List<Vector2Int> reverse_indices = new List<Vector2Int>();

  //  while (node != null)
  //  {
  //    reverse_indices.Add(node.Location.Value);
  //    node = node.Parent;
  //  }
  //  NPC Npc = mNPCs[index].GetComponent<NPC>();

  //  LineRenderer lr = mPathViz[index];
  //  lr.positionCount = reverse_indices.Count;
  //  for (int i = reverse_indices.Count - 1; i >= 0; i--)
  //  {
  //    Npc.AddWayPoint(new Vector2(
  //      reverse_indices[i].x,
  //      reverse_indices[i].y));

  //    lr.SetPosition(i, new Vector3(
  //      reverse_indices[i].x,
  //      reverse_indices[i].y,
  //      -2.0f));
  //  }
  //  // save these as the previous start positions.
  //  mNPCStartPositionsPrev[index] = mNPCStartPositions[index];
  //  mNPCStartPositions[index] = mGoal;
  //}

  //void OnPathNotFound(int i)
  //{
  //  Debug.Log(i + " - Cannot find path to destination");
  //  if (mTextNotification)
  //  {
  //    mTextNotification.text = "Cannot find path to destination";
  //  }
  //}

}
