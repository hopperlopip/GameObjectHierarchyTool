# GameObjectHierarchyTool
Unity tool that allows you extract GameObject Hierarchy and import it in another assets/level file.

## You can:
1. Export full GameObject Hierarchy into one file with custom format `.gh` which means "GameObject Hierarchy". What does "full GameObject Hierarchy" mean? It means the program saves all GameObjects (with components) that inherit the selected GameObject. For example, you have GameObject with 2 GameObjects in it, the program will save all of them.
2. Easy change GameObject active state by changing checkbox state.
3. Easy change GameObject inheritance. For example, you have 2 GameObjects with 2 children so you want to move 1st child from 1st GameObject to 2nd GameObject, you can just Drag&Drop 1st child from 1st GameObject to 2nd GameObject.

## Used libraries:
[AssetsTools.NET](https://github.com/nesrak1/AssetsTools.NET/tree/main) from Nesrak.
