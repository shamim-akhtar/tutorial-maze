
namespace Procedural
{
  // The directions types for maze
  // propagation.
  public enum Directions
  {
    UP,
    RIGHT,
    DOWN,
    LEFT,
    NONE,
  }

  // A cell in the maze.
  public class Cell
  {
    public int x;
    public int y;

    // visited flag. 
    // it is used while creating the maze.
    public bool visited = false;
    public bool[] flag =
    {
      true,
      true,
      true,
      true
    };

    // a delegate that is called
    // when we set a direction flag 
    // to the cell.
    public delegate void
      DelegateSetDirFlag(
        int x,
        int y,
        Directions dir,
        bool f);

    public DelegateSetDirFlag onSetDirFlag;

    // constructor
    public Cell(int c, int r)
    {
      x = c;
      y = r;
    }

    // set direction flag for the cell.
    // a direction flag shows which 
    // dircetion the cell opens up to.
    public void SetDirFlag(
      Directions dir,
      bool f)
    {
      flag[(int)dir] = f;
      onSetDirFlag?.Invoke(x, y, dir, f);
    }
  }
}