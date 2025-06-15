using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private void AddMatchesToCurrent(IEnumerable<GameObject> matches)
    {
        foreach (var dot in matches)
        {
            if (!currentMatches.Contains(dot))
            {
                currentMatches.Add(dot);
                dot.GetComponent<Dot>().isMatched = true;
            }
        }
    }

    private List<GameObject> IsAdjacentBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        var matchedDots = new List<GameObject>();

        if (dot1.isAdjacentBomb)
            matchedDots.AddRange(GetAdjacentPieces(dot1.column, dot1.row));

        if (dot2.isAdjacentBomb)
            matchedDots.AddRange(GetAdjacentPieces(dot2.column, dot2.row));

        if (dot3.isAdjacentBomb)
            matchedDots.AddRange(GetAdjacentPieces(dot3.column, dot3.row));

        return matchedDots.Distinct().ToList();
    }

    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        var matchedDots = new List<GameObject>();

        if (dot1.isRowBomb)
            matchedDots.AddRange(GetRowPieces(dot1.row));

        if (dot2.isRowBomb)
            matchedDots.AddRange(GetRowPieces(dot2.row));

        if (dot3.isRowBomb)
            matchedDots.AddRange(GetRowPieces(dot3.row));

        return matchedDots.Distinct().ToList();
    }

    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        var matchedDots = new List<GameObject>();

        if (dot1.isColumnBomb)
            matchedDots.AddRange(GetColumnPieces(dot1.column));

        if (dot2.isColumnBomb)
            matchedDots.AddRange(GetColumnPieces(dot2.column));

        if (dot3.isColumnBomb)
            matchedDots.AddRange(GetColumnPieces(dot3.column));

        return matchedDots.Distinct().ToList();
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (!currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
            dot.GetComponent<Dot>().isMatched = true;
        }
    }

    private void GetNearByPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);

        currentMatches.Clear();

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot == null) continue;

                Dot currentDotDot = currentDot.GetComponent<Dot>();

                // Проверка по горизонтали
                if (i > 0 && i < board.width - 1)
                {
                    GameObject leftDot = board.allDots[i - 1, j];
                    GameObject rightDot = board.allDots[i + 1, j];

                    if (leftDot != null && rightDot != null)
                    {
                        if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();

                            AddMatchesToCurrent(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));
                            AddMatchesToCurrent(IsColumnBomb(leftDotDot, currentDotDot, rightDotDot));
                            AddMatchesToCurrent(IsAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));
                            GetNearByPieces(leftDot, currentDot, rightDot);
                        }
                    }
                }

                // Проверка по вертикали
                if (j > 0 && j < board.height - 1)
                {
                    GameObject upDot = board.allDots[i, j + 1];
                    GameObject downDot = board.allDots[i, j - 1];

                    if (upDot != null && downDot != null)
                    {
                        if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                        {
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();

                            AddMatchesToCurrent(IsColumnBomb(upDotDot, currentDotDot, downDotDot));
                            AddMatchesToCurrent(IsRowBomb(upDotDot, currentDotDot, downDotDot));
                            AddMatchesToCurrent(IsAdjacentBomb(upDotDot, currentDotDot, downDotDot));
                            GetNearByPieces(upDot, currentDot, downDot);
                        }
                    }
                }
            }
        }
    }

    public void MatchPiecesOfColour(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject dot = board.allDots[i, j];
                if (dot != null && dot.tag == color)
                {
                    dot.GetComponent<Dot>().isMatched = true;
                }
            }
        }
    }

    List<GameObject> GetAdjacentPieces(int column, int row)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = column - 1; i <= column + 1; i++)
        {
            for (int j = row - 1; j <= row + 1; j++)
            {
                if (i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    GameObject dot = board.allDots[i, j];
                    if (dot != null)
                    {
                        dots.Add(dot);
                        dot.GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }

        return dots;
    }

    List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.height; i++)
        {
            GameObject dotGO = board.allDots[column, i];
            if (dotGO != null)
            {
                Dot dot = dotGO.GetComponent<Dot>();

                if (dot.isRowBomb)
                {
                    dots.AddRange(GetRowPieces(i));
                }

                dots.Add(dotGO);
                dot.isMatched = true;
            }
        }

        return dots.Distinct().ToList();
    }

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.width; i++)
        {
            GameObject dotGO = board.allDots[i, row];
            if (dotGO != null)
            {
                Dot dot = dotGO.GetComponent<Dot>();

                if (dot.isColumnBomb)
                {
                    dots.AddRange(GetColumnPieces(i));
                }

                dots.Add(dotGO);
                dot.isMatched = true;
            }
        }

        return dots.Distinct().ToList();
    }

    public void CheckBombs()
    {
        if (board.currentDot != null)
        {
            if (board.currentDot.isMatched)
            {
                board.currentDot.isMatched = false;

                if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                    || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135))
                {
                    board.currentDot.MakeRowBomb();
                }
                else
                {
                    board.currentDot.MakeColumnBomb();
                }
            }
            else if (board.currentDot.otherDot != null)
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                if (otherDot.isMatched)
                {
                    otherDot.isMatched = false;

                    if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45)
                        || (board.currentDot.swipeAngle < -135 || board.currentDot.swipeAngle >= 135))
                    {
                        otherDot.MakeRowBomb();
                    }
                    else
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
            }
        }
    }
}