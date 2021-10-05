using Procedural;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
  [SerializeField]
  GameObject[] Walls;

  [SerializeField]
  SpriteRenderer Highlight;

  public Cell Cell { get; set; }

  void Start()
  {
    SetHighlight(false);
  }

  public void SetHighlight(bool flag)
  {
    Highlight.gameObject.SetActive(flag);
  }

  public void SetHighColor(Color color)
  {
    Highlight.color = color;
  }

  public void SetActive(Directions dir, bool flag)
  {
    Walls[(int)dir].SetActive(flag);
  }
}
