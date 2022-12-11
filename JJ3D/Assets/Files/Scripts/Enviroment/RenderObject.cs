using System.Collections.Generic;
using UnityEngine;

public class RenderObject : MonoBehaviour
{
    [Header("Meshs")]
    [SerializeField] Mesh[] meshRock;
    [SerializeField] Mesh[] meshWood;
    [SerializeField] Mesh[] meshGrass;
    [SerializeField] Mesh[] meshPlant;

    [Header("Materials")]
    [SerializeField] Material matRock;
    [SerializeField] Material matWood;
    [SerializeField] Material matPlant;
    [SerializeField] Material matMashroom;
    [SerializeField] Material matTreeGreen;

    // Scale
    private Vector3 grassScale;

    // GrassPos
    private List<List<Matrix4x4>> grass1Pos;
    private List<List<Matrix4x4>> grass2Pos;
    private List<List<Matrix4x4>> grass3Pos;
    private List<List<Matrix4x4>> grass4Pos;

    // RockPos
    private List<List<Matrix4x4>> rock1Pos;
    private List<List<Matrix4x4>> rock2Pos;
    private List<List<Matrix4x4>> rock3Pos;
    private List<List<Matrix4x4>> rock4Pos;
    private List<List<Matrix4x4>> rock5Pos;
    private List<List<Matrix4x4>> rock6Pos;
    private List<List<Matrix4x4>> rock7Pos;

    // Plant Pos
    private List<List<Matrix4x4>> plant1Pos;
    private List<List<Matrix4x4>> plant2Pos;
    private List<List<Matrix4x4>> plant3Pos;
    private List<List<Matrix4x4>> plant4Pos;
    private List<List<Matrix4x4>> plant5Pos;

    // Wood Pos
    private List<List<Matrix4x4>> wood1Pos;
    private List<List<Matrix4x4>> wood2Pos;



    private void Start()
    {
        InitializeGrass();
        InitializeRocks();
        InitializePlants();
        InitializeWoods();
    }

    private void Update()
    {
        RenderGrass();
        RenderRock();
        RenderPlant();
        RenderWoods();
    }


    private void InitializeGrass()
    {
        grassScale = new Vector3(2, 5, 2);

        grass1Pos = new List<List<Matrix4x4>>();
        grass1Pos.Add(new List<Matrix4x4>());

        grass2Pos = new List<List<Matrix4x4>>();
        grass2Pos.Add(new List<Matrix4x4>());

        grass3Pos = new List<List<Matrix4x4>>();
        grass3Pos.Add(new List<Matrix4x4>());

        grass4Pos = new List<List<Matrix4x4>>();
        grass4Pos.Add(new List<Matrix4x4>());
    }

    private void AddGrassItem(Vector3 pos)
    {
        switch (Random.Range(1, 5))
        {
            case 1:
                if (grass1Pos[grass1Pos.Count - 1].Count >= 1000) grass1Pos.Add(new List<Matrix4x4>());
                grass1Pos[grass1Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, grassScale * Random.Range(0.7f, 1.4f)));
                break;
            case 2:
                if (grass2Pos[grass2Pos.Count - 1].Count >= 1000) grass2Pos.Add(new List<Matrix4x4>());
                grass2Pos[grass2Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, grassScale * Random.Range(0.7f, 1.4f)));
                break;
            case 3:
                if (grass3Pos[grass3Pos.Count - 1].Count >= 1000) grass3Pos.Add(new List<Matrix4x4>());
                grass3Pos[grass3Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, grassScale * Random.Range(0.7f, 1.4f)));
                break;
            case 4:
                if (grass4Pos[grass4Pos.Count - 1].Count >= 1000) grass4Pos.Add(new List<Matrix4x4>());
                grass4Pos[grass4Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, grassScale * Random.Range(0.7f, 1.4f)));
                break;
        }

        // Debug.Log("Grass Added");
    }

    private void RenderGrass()
    {
        foreach (var item in grass1Pos)
        {
            Graphics.DrawMeshInstanced(meshGrass[0], 0, matPlant, item);
        }

        foreach (var item in grass2Pos)
        {
            Graphics.DrawMeshInstanced(meshGrass[1], 0, matPlant, item);
        }

        foreach (var item in grass3Pos)
        {
            Graphics.DrawMeshInstanced(meshGrass[2], 0, matPlant, item);
        }

        foreach (var item in grass4Pos)
        {
            Graphics.DrawMeshInstanced(meshGrass[3], 0, matPlant, item);
        }
    }

    public void AddGrass(Vector3 pos)
    {
        AddGrassItem(pos);
        AddGrassItem(pos + new Vector3(Random.Range(0, 2), 0, 0));
        AddGrassItem(pos + new Vector3(Random.Range(0, 2), 0, Random.Range(0, 2)));
        AddGrassItem(pos + new Vector3(Random.Range(0, 2), 0, Random.Range(0, -2)));
        AddGrassItem(pos + new Vector3(Random.Range(0, -2), 0, 0));
        AddGrassItem(pos + new Vector3(Random.Range(0, -2), 0, Random.Range(0, 2)));
        AddGrassItem(pos + new Vector3(Random.Range(0, -2), 0, Random.Range(0, -2)));
    }





    private void InitializeRocks()
    {
        rock1Pos = new List<List<Matrix4x4>>();
        rock1Pos.Add(new List<Matrix4x4>());

        rock2Pos = new List<List<Matrix4x4>>();
        rock2Pos.Add(new List<Matrix4x4>());

        rock3Pos = new List<List<Matrix4x4>>();
        rock3Pos.Add(new List<Matrix4x4>());

        rock4Pos = new List<List<Matrix4x4>>();
        rock4Pos.Add(new List<Matrix4x4>());

        rock5Pos = new List<List<Matrix4x4>>();
        rock5Pos.Add(new List<Matrix4x4>());

        rock6Pos = new List<List<Matrix4x4>>();
        rock6Pos.Add(new List<Matrix4x4>());

        rock7Pos = new List<List<Matrix4x4>>();
        rock7Pos.Add(new List<Matrix4x4>());
    }

    private void RenderRock()
    {
        foreach (var item in rock1Pos)
        {
            Graphics.DrawMeshInstanced(meshRock[0], 0, matRock, item);
        }

        foreach (var item in rock2Pos)
        {
            Graphics.DrawMeshInstanced(meshRock[1], 0, matRock, item);
        }

        foreach (var item in rock3Pos)
        {
            Graphics.DrawMeshInstanced(meshRock[2], 0, matRock, item);
        }

        foreach (var item in rock4Pos)
        {
            Graphics.DrawMeshInstanced(meshRock[3], 0, matRock, item);
        }

        foreach (var item in rock5Pos)
        {
            Graphics.DrawMeshInstanced(meshRock[4], 0, matRock, item);
        }

        foreach (var item in rock6Pos)
        {
            Graphics.DrawMeshInstanced(meshRock[5], 0, matRock, item);
        }

        foreach (var item in rock7Pos)
        {
            Graphics.DrawMeshInstanced(meshRock[6], 0, matRock, item);
        }
    }

    public void AddRock(Vector3 pos)
    {
        switch (Random.Range(1, 8))
        {
            case 1:
                if (rock1Pos[rock1Pos.Count - 1].Count >= 1000) rock1Pos.Add(new List<Matrix4x4>());
                rock1Pos[rock1Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
                break;
            case 2:
                if (rock2Pos[rock2Pos.Count - 1].Count >= 1000) rock2Pos.Add(new List<Matrix4x4>());
                rock2Pos[rock2Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
                break;
            case 3:
                if (rock3Pos[rock3Pos.Count - 1].Count >= 1000) rock3Pos.Add(new List<Matrix4x4>());
                rock3Pos[rock3Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
                break;
            case 4:
                if (rock4Pos[rock4Pos.Count - 1].Count >= 1000) rock4Pos.Add(new List<Matrix4x4>());
                rock4Pos[rock4Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * Random.Range(0.2f, 0.5f)));
                break;
            case 5:
                if (rock5Pos[rock5Pos.Count - 1].Count >= 1000) rock5Pos.Add(new List<Matrix4x4>());
                rock5Pos[rock5Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
                break;
            case 6:
                if (rock6Pos[rock6Pos.Count - 1].Count >= 1000) rock6Pos.Add(new List<Matrix4x4>());
                rock6Pos[rock6Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * Random.Range(0.2f, 0.5f)));
                break;
            case 7:
                if (rock7Pos[rock7Pos.Count - 1].Count >= 1000) rock7Pos.Add(new List<Matrix4x4>());
                rock7Pos[rock7Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
                break;
        }

        // Debug.Log("Rock Added");
    }





    private void InitializePlants()
    {
        plant1Pos = new List<List<Matrix4x4>>();
        plant1Pos.Add(new List<Matrix4x4>());

        plant2Pos = new List<List<Matrix4x4>>();
        plant2Pos.Add(new List<Matrix4x4>());

        plant3Pos = new List<List<Matrix4x4>>();
        plant3Pos.Add(new List<Matrix4x4>());

        plant4Pos = new List<List<Matrix4x4>>();
        plant4Pos.Add(new List<Matrix4x4>());

        plant5Pos = new List<List<Matrix4x4>>();
        plant5Pos.Add(new List<Matrix4x4>());
    }

    private void RenderPlant()
    {
        foreach (var item in plant1Pos)
        {
            Graphics.DrawMeshInstanced(meshPlant[0], 0, matPlant, item);
        }

        foreach (var item in plant2Pos)
        {
            Graphics.DrawMeshInstanced(meshPlant[1], 0, matPlant, item);
        }

        foreach (var item in plant3Pos)
        {
            Graphics.DrawMeshInstanced(meshPlant[2], 0, matPlant, item);
        }

        foreach (var item in plant4Pos)
        {
            Graphics.DrawMeshInstanced(meshPlant[3], 0, matTreeGreen, item);
        }

        foreach (var item in plant5Pos)
        {
            Graphics.DrawMeshInstanced(meshPlant[4], 0, matMashroom, item);
        }
    }

    public void AddPlant(Vector3 pos)
    {
        switch (Random.Range(1, 6))
        {
            case 1:
                if (plant1Pos[plant1Pos.Count - 1].Count >= 1000) plant1Pos.Add(new List<Matrix4x4>());
                plant1Pos[plant1Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
                break;
            case 2:
                if (plant2Pos[plant2Pos.Count - 1].Count >= 1000) plant2Pos.Add(new List<Matrix4x4>());
                plant2Pos[plant2Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one));
                break;
            case 3:
                if (plant3Pos[plant3Pos.Count - 1].Count >= 1000) plant3Pos.Add(new List<Matrix4x4>());
                plant3Pos[plant3Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * 4));
                break;
            case 4:
                if (plant4Pos[plant4Pos.Count - 1].Count >= 1000) plant4Pos.Add(new List<Matrix4x4>());
                plant4Pos[plant4Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * 4));
                break;
            case 5:
                if (plant5Pos[plant5Pos.Count - 1].Count >= 1000) plant5Pos.Add(new List<Matrix4x4>());
                plant5Pos[plant5Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * 4));
                break;
        }

        // Debug.Log("Plant Added");
    }





    private void InitializeWoods()
    {
        wood1Pos = new List<List<Matrix4x4>>();
        wood1Pos.Add(new List<Matrix4x4>());

        wood2Pos = new List<List<Matrix4x4>>();
        wood2Pos.Add(new List<Matrix4x4>());
    }

    private void RenderWoods()
    {
        foreach (var item in wood1Pos)
        {
            Graphics.DrawMeshInstanced(meshWood[0], 0, matWood, item);
        }

        foreach (var item in wood2Pos)
        {
            Graphics.DrawMeshInstanced(meshWood[1], 0, matWood, item);
        }
    }

    public void AddWood(Vector3 pos)
    {
        switch (Random.Range(1, 3))
        {
            case 1:
                if (wood1Pos[wood1Pos.Count - 1].Count >= 1000) wood1Pos.Add(new List<Matrix4x4>());
                wood1Pos[wood1Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * Random.Range(1.5f, 4f)));
                break;
            case 2:
                if (wood2Pos[wood2Pos.Count - 1].Count >= 1000) wood2Pos.Add(new List<Matrix4x4>());
                wood2Pos[wood2Pos.Count - 1].Add(Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * Random.Range(1.5f, 4f)));
                break;
        }

        // Debug.Log("Wood Added");
    }
}