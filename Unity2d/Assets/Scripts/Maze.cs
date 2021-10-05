using System.Collections.Generic;
using System;
using UnityEngine;

namespace Procedural
{
  public class Maze
  {

    // the number of rows and columns
    private int mRows;
    private int mCols;

    // the 2d array of cells
    private Cell[,] mCells;

    public int NumRows { get { return mRows; } }
    public int NumCols { get { return mCols; } }

    // constructor
    public Maze(int rows, int cols)
    {
      mRows = rows;
      mCols = cols;

      // initiaze all the cells.
      mCells = new Cell[mCols, mRows];
      for (var i = 0; i < mCols; i++)
      {
        for (var j = 0; j < mRows; j++)
        {
          mCells[i, j] = new Cell(i, j, this);
        }
      }
    }

    public Cell GetCell(int i, int j)
    {
      return mCells[i, j];
    }

    public int GetCellCount()
    {
      return mRows * mCols;
    }

    public List<Tuple<Directions, Cell>> 
      GetNeighboursNotVisited(
      int cx, 
      int cy)
    {
      List<Tuple<Directions, Cell>> neighbours = 
        new List<Tuple<Directions, Cell>>();

      foreach (Directions dir in Enum.GetValues(
        typeof(Directions)))
      {
        int x = cx;
        int y = cy;

        switch (dir)
        {
          case Directions.UP:
            if (y < mRows - 1)
            {
              ++y;
              if (!mCells[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Cell>(
                  Directions.UP,
                  mCells[x, y])
                );
              }
            }
            break;
          case Directions.RIGHT:
            if (x < mCols - 1)
            {
              ++x;
              if (!mCells[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Cell>(
                  Directions.RIGHT,
                  mCells[x, y])
                );
              }
            }
            break;
          case Directions.DOWN:
            if (y > 0)
            {
              --y;
              if (!mCells[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Cell>(
                  Directions.DOWN,
                  mCells[x, y])
                );
              }
            }
            break;
          case Directions.LEFT:
            if (x > 0)
            {
              --x;
              if (!mCells[x, y].visited)
              {
                neighbours.Add(new Tuple<Directions, Cell>(
                  Directions.LEFT,
                  mCells[x, y])
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

    public void RemoveCellWall(
      int x, 
      int y, 
      Directions dir)
    {
      if (dir != Directions.NONE)
      {
        Cell cell = GetCell(x, y);
        cell.SetDirFlag(dir, false);
      }

      Directions opp = Directions.NONE;
      switch (dir)
      {
        case Directions.UP:
          if (y < mRows - 1)
          {
            opp = Directions.DOWN;
            ++y;
          }
          break;
        case Directions.RIGHT:
          if (x < mCols - 1)
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
        Cell cell1 = GetCell(x, y);
        cell1.SetDirFlag(opp, false);
      }
    }
  }
}