using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
public class SnakeController : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private GameObject tailPrefab;
    [SerializeField] private GameObject food;
    [SerializeField] private TextMeshPro textScore;
    [SerializeField] private TextMeshPro textGameOver;
    private Vector2 _direction = Vector2.down;
    private Vector2 AreaLimit = new Vector2(13, 24);
    private List<Transform> snake = new List<Transform>();
    private int _score = 0;
    private bool grow;

    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            textScore.text = _score.ToString();
        }
    }
    private void Start()
    {
        Score = 0;
        textGameOver.enabled = false;
        ChangePositionOfFood();
        StartCoroutine(Move());
        snake.Add(transform);
    }
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && _direction != Vector2.right)
        {
            _direction = Vector2.left;

        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && _direction != Vector2.left)
        {
            _direction = Vector2.right;

        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && _direction != Vector2.down)
        {
            _direction = Vector2.up;

        }
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && _direction != Vector2.up)
        {
            _direction = Vector2.down;

        }

    }
    IEnumerator Move()
    {
        while (true)
        {
            if (grow)
            {
                grow = false;
                Grow();

            }
            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i].position = snake[i - 1].position;
            }
            var position = transform.position;
            position += (Vector3)_direction;
            transform.position = position;

            yield return new WaitForSeconds(speed);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("food"))
        {
            // Grow();
            grow = true;
        }
        else if (other.CompareTag("wall"))
        {
            Dead();
        }
    }
    private void Grow()
    {
        Score++;
        var tail = Instantiate(tailPrefab);
        snake.Add(tail.transform);
        snake[snake.Count - 1].position = snake[snake.Count - 2].position;
        ChangePositionOfFood();
    }
    private void Dead()
    {
        textGameOver.enabled = true;
        StopAllCoroutines();
    }

    private void ChangePositionOfFood()
    {
        Vector2 newPosition;
        do
        {

            var x = (int)Random.Range(1, AreaLimit.x);
            var y = (int)Random.Range(1, AreaLimit.y);
            newPosition = new Vector3(x, y, 0);

        } while (!CanSpanFood(newPosition));
        food.transform.position = newPosition;

    }
    private bool CanSpanFood(Vector2 position)
    {
        foreach (var tail in snake)
        {
            if ((Vector2)tail.transform.position == position)
            {
                return false;
            }
        }
        return true;
    }
}
