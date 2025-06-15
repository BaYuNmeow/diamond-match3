using System.Collections;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public int targetX;
    public int targetY;
    public int offSet = 0;
    public bool isMatched = false;

    private EndGameManager endGameManager;
    private HintManager hintManager;
    private FindMatches findMatches;
    private Board board;
    public GameObject otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;

    [Header("Swipe Stuff")]
    public float swipeAngle = 0;
    public float swipeResist = 0.3f;

    [Header("Powerup Stuff")]
    public bool isColorBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isAdjacentBomb;
    public GameObject adjacentMarker;
    public GameObject rowArrow;
    public GameObject columnArrow;
    public GameObject colorBomb;

    private void Start()
    {
        isColumnBomb = false;
        isRowBomb = false;
        isColorBomb = false;
        isAdjacentBomb = false;

        endGameManager = FindAnyObjectByType<EndGameManager>();
        hintManager = FindAnyObjectByType<HintManager>();
        board = GameObject.FindWithTag("Board").GetComponent<Board>();
        findMatches = FindAnyObjectByType<FindMatches>();

    }

    private void Update()
    {
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
        findMatches.FindAllMatches();
    }

    public IEnumerator CheckMoveCo()
    {
        if (isColorBomb && otherDot != null)
        {
            findMatches.MatchPiecesOfColour(otherDot.tag);
            isMatched = true;

            board.currentState = GameState.Wait;

            board.DestroyMatches();

            yield return new WaitForSeconds(board.refillDelay);

            board.currentState = GameState.Move;

            yield break;
        }
        else if (otherDot != null && otherDot.TryGetComponent<Dot>(out Dot otherDotScript))
        {
            if (otherDotScript.isColorBomb)
            {
                findMatches.MatchPiecesOfColour(this.gameObject.tag);
                otherDotScript.isMatched = true;
            }

            yield return new WaitForSeconds(.5f);

            if (!isMatched && !otherDotScript.isMatched)
            {
                otherDotScript.row = row;
                otherDotScript.column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.5f);
                board.currentDot = null;
                board.currentState = GameState.Move;
            }
            else
            {
                if (endGameManager != null && endGameManager.requirements.gameType == GameType.Moves)
                {
                    endGameManager.DecreaseCounterValue();
                }
                board.DestroyMatches();
            }
        }
        else
        {
            board.currentState = GameState.Move;
        }
    }

    private void OnMouseDown()
    {
        if (hintManager != null)
        {
            hintManager.DestroyHint();
        }
        if (board.currentState == GameState.Move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.Move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        Vector2 swipeVector = finalTouchPosition - firstTouchPosition;
        if (swipeVector.magnitude > swipeResist)
        {
            swipeVector.Normalize();
            if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
            {
                if (swipeVector.x > 0 && column < board.width - 1)
                    MovePiecesActual(Vector2.right);
                else if (column > 0)
                    MovePiecesActual(Vector2.left);
            }
            else
            {
                if (swipeVector.y > 0 && row < board.height - 1)
                    MovePiecesActual(Vector2.up);
                else if (row > 0)
                    MovePiecesActual(Vector2.down);
            }
        }

    }

    void MovePiecesActual(Vector2 direction)
    {
        int newColumn = column + (int)direction.x;
        int newRow = row + (int)direction.y;

        if (newColumn >= 0 && newColumn < board.width && newRow >= 0 && newRow < board.height)
        {
            otherDot = board.allDots[newColumn, newRow];
            previousRow = row;
            previousColumn = column;

            if (otherDot != null)
            {
                otherDot.GetComponent<Dot>().column += -1 * (int)direction.x;
                otherDot.GetComponent<Dot>().row += -1 * (int)direction.y;
                column = newColumn;
                row = newRow;

                board.currentDot = this;

                StartCoroutine(CheckMoveCo());
            }
            else
            {
                board.currentState = GameState.Move;
            }
        }
        else
        {
            board.currentState = GameState.Move;
        }
    }

    void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }

    public void MakeRowBomb()
    {
        if (isColumnBomb) return;
        isRowBomb = true;
        GameObject arrow = Instantiate(rowArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }

    public void MakeColumnBomb()
    {
        if (isRowBomb) return;
        isColumnBomb = true;
        GameObject arrow = Instantiate(columnArrow, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    }

    public void MakeColorBomb()
    {
        isColorBomb = true;
        GameObject color = Instantiate(colorBomb, transform.position, Quaternion.identity);
        color.transform.parent = this.transform;
        this.gameObject.tag = "Rainbow Bomb";
    }

    public void MakeAdjacentBomb()
    {
        isAdjacentBomb = true;
        GameObject marker = Instantiate(adjacentMarker, transform.position, Quaternion.identity);
        marker.transform.parent = this.transform;
    }
}