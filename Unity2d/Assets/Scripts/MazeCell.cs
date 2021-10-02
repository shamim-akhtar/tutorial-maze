using Procedural;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
  [SerializeField]
  GameObject[] Walls;

  [SerializeField]
  GameObject Highlight;

  void Start()
  {
    SetHighlight(false);
  }

  public void SetHighlight(bool flag)
  {
    Highlight.SetActive(flag);
  }

  public void SetActive(Directions dir, bool flag)
  {
    Walls[(int)dir].SetActive(flag);
  }
}
