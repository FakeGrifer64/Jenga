using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JengaManager : MonoBehaviour
{

    [Header("Tower Settings")]
    public int layers = 3;
    public int pieceslayer = 3;
    public List<GameObject> pieceprefab;

    [Header("Piece size settings")]
    public float pieceSpacing = 0f;
    public float pieceHeight = 0.5f;
    public float pieceLenght = 05f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildTower();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BuildTower()
    {
        for (int layerIndex = 0; layerIndex < layers; layerIndex++) 
        {
            GeneralLayer(layerIndex);
        }
    }

    void GeneralLayer(int layerIndex)
    {
        Vector3 basePosition = transform.position;
        basePosition.y += layerIndex * (pieceHeight + pieceSpacing);

        bool isOdd = isOddLayer(layerIndex);
        Quaternion rotation = isOdd ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
        Vector3 direction = isOdd ? Vector3.right : Vector3.forward;

        float totalWidth = (pieceslayer - 1) * (pieceHeight + pieceSpacing);
        Vector3 startOffset = -direction * (totalWidth / 2f);

        for (int i = 0; i < pieceslayer; i++)
        {
            GameObject prefab = pieceprefab[Random.Range(0, pieceprefab.Count)];
            Vector3 offset = direction * i * (pieceLenght + pieceSpacing);
            Vector3 spawnPosition = basePosition + startOffset + offset;

            Instantiate(prefab, spawnPosition, rotation);
        }
        
    }

    bool isOddLayer(int layerIndex)
    {
        return layerIndex % 2 != 0;
    }
}
