# Implement Mazes in Unity2D
![](https://img.shields.io/badge/Unity-2021.1.21f1-green) ![C#](https://img.shields.io/badge/%20-C%23-blue) ![Visitors](https://visitor-badge.glitch.me/badge?page_id=tutorial-maze.visitor-badge) 

![Maze Generation](https://github.com/shamim-akhtar/tutorial-maze/blob/main/MazeGeneration.gif)

This repository comprises the source codes for my tutorial on how to implement mazes in Unity2D by applying the backtracking algorithm with an explicit stack. Read the complete tutorial [Implement Mazes in Unity2D](https://faramira.com/implement-mazes-in-unity2d/)

## Introduction to Mazes
Not too far ago, mazes were somewhat central to video games. Arguably, the original first-person shooter game, literally named Maze, was created by students on Imlac computers at a NASA laboratory in 1974 – source [Wikipedia](https://en.wikipedia.org/wiki/Maze_War).

A maze is a kind of game where a player moves in pathways with many branches to find a way
out or reach a specific target.

The maze-generation process is the process of designing the position of paths and walls in the Maze.
There are many methods/algorithms to generate a maze

We can create a maze by starting with a predetermined arrangement of cells (a rectangular grid in our case) with walls between them. The maze generator will traverse these cells iteratively and remove walls in four possible directions, thus forming a maze.
