using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class HintManager : MonoBehaviour
{
    private Board board;
    public float hintDelay = 5f;
    private float hintDelayTimer;

    private GameObject firstHintDot;
    private GameObject secondHintDot;
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    private Dictionary<GameObject, Tween> activeTweens = new Dictionary<GameObject, Tween>();

    private void Start()
    {
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        hintDelayTimer = hintDelay;
    }

    private void Update()
    {
        hintDelayTimer -= Time.deltaTime;

        if (hintDelayTimer <= 0 && firstHintDot == null && secondHintDot == null)
        {
            MarkHint();
            hintDelayTimer = hintDelay;
        }
    }

    private List<(GameObject, GameObject)> FindAllHintPairs()
    {
        List<(GameObject, GameObject)> possibleMoves = new List<(GameObject, GameObject)>();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject dot = board.allDots[i, j];
                if (dot == null) continue;

                if (i < board.width - 1)
                {
                    if (board.SwitchAndCheck(i, j, Vector2.right))
                    {
                        possibleMoves.Add((dot, board.allDots[i + 1, j]));
                    }
                }
                if (j < board.height - 1)
                {
                    if (board.SwitchAndCheck(i, j, Vector2.up))
                    {
                        possibleMoves.Add((dot, board.allDots[i, j + 1]));
                    }
                }
            }
        }

        return possibleMoves;
    }

    private (GameObject, GameObject)? PickOnePairRandomly()
    {
        var possibleMoves = FindAllHintPairs();
        if (possibleMoves.Count == 0)
            return null;

        int index = Random.Range(0, possibleMoves.Count);
        return possibleMoves[index];
    }

    private void MarkHint()
    {
        var pair = PickOnePairRandomly();
        if (pair.HasValue)
        {
            firstHintDot = pair.Value.Item1;
            secondHintDot = pair.Value.Item2;

            AnimateDot(firstHintDot);
            AnimateDot(secondHintDot);
        }
    }

    private void AnimateDot(GameObject dot)
    {
        if (dot == null) return;

        if (activeTweens.TryGetValue(dot, out Tween existingTween))
        {
            if (existingTween.IsActive()) existingTween.Kill();
            activeTweens.Remove(dot);
        }

        if (!originalScales.ContainsKey(dot))
        {
            originalScales[dot] = dot.transform.localScale;
        }

        Vector3 originalScale = originalScales[dot];

        Tween pulse = dot.transform.DOScale(originalScale * 1.2f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);

        activeTweens[dot] = pulse;
    }

    public void DestroyHint()
    {
        foreach (var kvp in activeTweens)
        {
            Tween tween = kvp.Value;
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }

            GameObject dot = kvp.Key;
            if (dot != null && originalScales.TryGetValue(dot, out Vector3 originalScale))
            {
                dot.transform.localScale = originalScale;
            }
        }

        activeTweens.Clear();
        firstHintDot = null;
        secondHintDot = null;

        hintDelayTimer = hintDelay;
    }

    private void OnDestroy()
    {
        foreach (var tween in activeTweens.Values)
        {
            if (tween != null && tween.IsActive())
            {
                tween.Kill();
            }
        }
        activeTweens.Clear();
    }
}
