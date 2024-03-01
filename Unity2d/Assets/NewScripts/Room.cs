using UnityEngine;

namespace Procedural
{
  public class Room : MonoBehaviour
  {
    [SerializeField]
    GameObject[] walls;

    [SerializeField]
    SpriteRenderer highlight;

    public Vector2Int Index { get; set; }
    public bool visited = false;
    bool[] dirflag =
    {
      true,
      true,
      true,
      true
    };

    public void SetHighlight(bool flag)
    {
      highlight.gameObject.SetActive(flag);
    }

    public void SetHighColor(Color color)
    {
      highlight.color = color;
    }

    public void SetActive(Directions dir, bool flag)
    {
      walls[(int)dir].SetActive(flag);
    }

    public void SetDirFlag(
      Directions dir,
      bool f)
    {
      dirflag[(int)dir] = f;
      SetActive(dir, f);
    }
  }
}