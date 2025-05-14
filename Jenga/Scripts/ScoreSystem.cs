using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreSystem : MonoBehaviour
{
    [System.Serializable]
    public class PieceSettings
    {
        public GameObject prefab;
        public int scoreValue = 10;
    }

    [Header("Configuration")]
    public List<PieceSettings> pieceConfigs = new List<PieceSettings>();
    public float minHeight = 3f;
    public float requiredStillTime = 3f;

    [Header("UI")]
    public TMP_Text scoreText;

    private int currentScore = 0;
    private Dictionary<GameObject, PieceSettings> settingsLookup = new Dictionary<GameObject, PieceSettings>();
    private Dictionary<Rigidbody, TrackedPiece> trackedPieces = new Dictionary<Rigidbody, TrackedPiece>();

    private class TrackedPiece
    {
        public float stillTime;
        public bool scored;
    }

    void Start()
    {
        // Initialize lookup dictionary
        foreach (var config in pieceConfigs)
        {
            if (config.prefab != null)
            {
                settingsLookup[config.prefab] = config;
            }
        }

        UpdateScoreDisplay();
    }

    void FixedUpdate()
    {
        CheckPieces();
    }

    void CheckPieces()
    {
        var piecesToRemove = new List<Rigidbody>();

        foreach (var kvp in trackedPieces)
        {
            var rb = kvp.Key;
            var piece = kvp.Value;

            if (rb == null || piece.scored)
            {
                piecesToRemove.Add(rb);
                continue;
            }

            if (IsPieceValid(rb))
            {
                piece.stillTime += Time.fixedDeltaTime;

                if (piece.stillTime >= requiredStillTime)
                {
                    AddScore(rb.gameObject);
                    piece.scored = true;
                }
            }
            else
            {
                piece.stillTime = 0f;
            }
        }

        // Cleanup
        foreach (var rb in piecesToRemove)
        {
            trackedPieces.Remove(rb);
        }
    }

    bool IsPieceValid(Rigidbody rb)
    {
        return rb != null &&
               rb.transform.position.y > minHeight &&
               IsStill(rb);
    }

    bool IsStill(Rigidbody rb)
    {

        return rb.IsSleeping() ||
              (rb.linearVelocity.sqrMagnitude < 0.01f &&
               rb.angularVelocity.sqrMagnitude < 0.01f);
    }

    void AddScore(GameObject piece)
    {
        if (settingsLookup.TryGetValue(piece, out var settings))
        {
            currentScore += settings.scoreValue;
            UpdateScoreDisplay();
            Debug.Log($"Scored {settings.scoreValue} points! Total: {currentScore}");
        }
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }

    public void RegisterPiece(GameObject newPiece)
    {
        if (newPiece == null) return;

        var rb = newPiece.GetComponent<Rigidbody>();
        if (rb != null && settingsLookup.ContainsKey(newPiece))
        {
            trackedPieces[rb] = new TrackedPiece();
        }
    }

}