using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace TDS.Scripts.Fifteen
{
    public class FifteenPuzzle : MonoBehaviour
    {
        public GameObject buttonPrefab; // ������ ������
        public GameObject skipButtonPrefab;
        public int gridSize = 4; // ������ ������� ����� (4x4 � �������)
        private Button[,] buttons; // ��������� ������ ������
        private int[,] solution; // ������ ��� �������� ����������� ������� ������
        private int emptyX, emptyY; // ���������� ������ ������
        private bool gameFinished = false; // ���� ��� �������� ���������� ����

        private void Start()
        {
            GenerateBoard();
            ShuffleButtons();
            CreateSolution();
            CreateSkipButton();
        }

        private void GenerateBoard()
        {
            buttons = new Button[gridSize, gridSize];

            GridLayoutGroup gridLayout = GetComponentInChildren<GridLayoutGroup>();

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    GameObject buttonObj = Instantiate(buttonPrefab, gridLayout.transform);
                    Button button = buttonObj.GetComponent<Button>();
                    buttons[x, y] = button;

                    // ��������� ������ �� ������
                    TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                    int number = y * gridSize + x + 1;
                    if (number < gridSize * gridSize)
                    {
                        buttonText.text = number.ToString();
                    }
                    else
                    {
                        buttonText.text = ""; // ��������� ������ ������
                        emptyX = x;
                        emptyY = y;
                    }

                    // ���������� ������� ������
                    button.onClick.AddListener(() => ButtonClicked(button));

                    // ����������� ������� � ������������ ������
                    RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(100, 100); // ������ �������� ������
                }
            }
        }

        private void CreateSkipButton()
        {
            GameObject skipButtonObj = Instantiate(skipButtonPrefab, transform);
            Button skipButton = skipButtonObj.GetComponent<Button>();
            skipButton.onClick.AddListener(SkipGame);

            RectTransform rectTransform = skipButtonObj.GetComponent<RectTransform>();
            rectTransform.SetParent(GetComponentInChildren<Canvas>().transform, false);
            rectTransform.sizeDelta = new Vector2(200, 50); // ������ �������� ������ "Skip"
            rectTransform.anchoredPosition = new Vector2(654, 425); // ������ ������� ������ "Skip"
        }

        private void SkipGame()
        {
            if (gameFinished) return;

            gameFinished = true;
            Debug.Log("���� ���������!");

            // �������� ����� OnPuzzleSolved �� PuzzleManager
            PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
            if (puzzleManager != null)
            {
                puzzleManager.OnPuzzleSolved();
            }
        }
        private void ShuffleButtons()
        {
            List<int> numbers = new List<int>();

            for (int i = 1; i < gridSize * gridSize; i++)
            {
                numbers.Add(i);
            }

            System.Random random = new System.Random();

            for (int i = numbers.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int temp = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = temp;
            }

            int index = 0;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (buttons[x, y] != null)
                    {
                        if (index < numbers.Count)
                        {
                            buttons[x, y].GetComponentInChildren<TextMeshProUGUI>().text = numbers[index].ToString();
                        }
                        else
                        {
                            buttons[x, y].GetComponentInChildren<TextMeshProUGUI>().text = "";
                            emptyX = x;
                            emptyY = y;
                        }
                        index++;
                    }
                }
            }
        }

        private void CreateSolution()
        {
            solution = new int[gridSize, gridSize];
            int number = 1;

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (number == gridSize * gridSize)
                    {
                        solution[x, y] = 0; // ��������� ������ ������
                    }
                    else
                    {
                        solution[x, y] = number;
                        number++;
                    }
                }
            }
        }

        private void ButtonClicked(Button clickedButton)
        {
            if (gameFinished)
            {
                Debug.Log("���� ���������!");
                return;
            }

            int clickedX = -1;
            int clickedY = -1;

            // ������� ���������� ������� ������ � ������� ������
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    if (buttons[x, y] == clickedButton)
                    {
                        clickedX = x;
                        clickedY = y;
                        break;
                    }
                }
                if (clickedX != -1)
                    break;
            }

            // ���������, ����� �� ����������� ������
            if (CanMove(clickedX, clickedY))
            {
                // ������ ������� ������
                SwapButtons(clickedX, clickedY);

                // ���������, ��������� �� ����
                if (CheckVictory())
                {
                    Debug.Log("����������, �� ��������!");
                    gameFinished = true;

                    // �������� ����� OnPuzzleSolved �� PuzzleManager
                    PuzzleManager puzzleManager = FindObjectOfType<PuzzleManager>();
                    if (puzzleManager != null)
                    {
                        puzzleManager.OnPuzzleSolved();
                    }
                }
            }
        }


        private bool CanMove(int x, int y)
        {
            // ��������� ����������� ����������� ������ � ��������� �����
            if (x > 0 && buttons[x - 1, y].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return true;
            if (x < gridSize - 1 && buttons[x + 1, y].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return true;
            if (y > 0 && buttons[x, y - 1].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return true;
            if (y < gridSize - 1 && buttons[x, y + 1].GetComponentInChildren<TextMeshProUGUI>().text == "")
                return true;

            return false;
        }

        private void SwapButtons(int x, int y)
        {
            // ������ ������� ������ ������
            string tempText = buttons[x, y].GetComponentInChildren<TextMeshProUGUI>().text;
            buttons[x, y].GetComponentInChildren<TextMeshProUGUI>().text = buttons[emptyX, emptyY].GetComponentInChildren<TextMeshProUGUI>().text;
            buttons[emptyX, emptyY].GetComponentInChildren<TextMeshProUGUI>().text = tempText;

            // ��������� ���������� ������ ������
            emptyX = x;
            emptyY = y;
        }

        private bool CheckVictory()
        {
            int number = 1;
            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    string buttonText = buttons[x, y].GetComponentInChildren<TextMeshProUGUI>().text;

                    // ��������� ����� ������
                    if (number == gridSize * gridSize)
                    {
                        // ��������� ������ ������ ���� ������
                        if (buttonText != "")
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (buttonText != number.ToString())
                        {
                            return false;
                        }
                    }
                    number++;
                }
            }
            return true;
        }
    }

}

