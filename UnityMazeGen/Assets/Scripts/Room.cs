using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum Directions
    {
        TOP,
        RIGHT,
        BOTTOM,
        LEFT,
        NONE,
    }

    [SerializeField]
    GameObject topWall;
    [SerializeField]
    GameObject rightWall;
    [SerializeField]
    GameObject bottomWall;
    [SerializeField]
    GameObject leftWall;

    [SerializeField]
    SpriteRenderer floor;
    [SerializeField]
    SpriteRenderer marker;
    private Color NORMAL_COLOR = new Color(100.0f / 255.0f, 100.0f / 255.0f, 150.0f / 255.0f);
    //private Color NORMAL_COLOR = Color.gray;
    //private Color NORMAL_COLOR = new Color(200 / 255.0f, 200 / 255.0f, 200.0f / 255.0f);

    Dictionary<Directions, GameObject> walls =
      new Dictionary<Directions, GameObject>();

    public Vector2Int Index
    {
        get;
        set;
    }

    public bool visited { get; set; } = false;

    Dictionary<Directions, bool> dirflags =
      new Dictionary<Directions, bool>();

    private void Start()
    {
        walls[Directions.TOP] = topWall;
        walls[Directions.RIGHT] = rightWall;
        walls[Directions.BOTTOM] = bottomWall;
        walls[Directions.LEFT] = leftWall;

        SetDirFlag(Directions.LEFT, true);
        SetDirFlag(Directions.RIGHT, true);
        SetDirFlag(Directions.BOTTOM, true);
        SetDirFlag(Directions.TOP, true);

        ResetColor();
    }

    private void SetActive(Directions dir, bool flag)
    {
        walls[dir].SetActive(flag);
    }

    public void SetDirFlag(Directions dir, bool flag)
    {
        dirflags[dir] = flag;
        SetActive(dir, flag);
    }

    public bool GetDirFlag(Directions dir)
    {
        return dirflags[dir];
    }

    public void SetColor(Color color)
    {
        floor.color = color;
    }

    public void SetMarkerAsStart()
    {
        marker.gameObject.SetActive(true);
        marker.color = Color.green;
    }

    public void SetMarkerAsDestination()
    {
        marker.gameObject.SetActive(true);
        marker.color = Color.red;
    }

    public void ResetMarker()
    {
        marker.gameObject.SetActive(false);
    }
    public void ResetColor()
    {
        floor.color = NORMAL_COLOR;
    }
}
