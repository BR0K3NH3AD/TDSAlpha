using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace TDS.Scripts.Fifteen
{
    public class FifteenPuzzle : MonoBehaviour
    {
        public GameObject buttonPrefab; // Префаб кнопки
        public GameObject skipButtonPrefab;
        public int gridSize = 4; // Размер игровой доски (4x4 в примере)
        private Button[,] buttons; // Двумерный массив кнопок
        private int[,] solution; // Массив для хранения правильного порядка клеток
        private int emptyX, emptyY; // Координаты пустой клетки
        private bool gameFinished = false; // Флаг для проверки завершения игры

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

                    // Настройка текста на кнопке
                    TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                    int number = y * gridSize + x + 1;
                    if (number < gridSize * gridSize)
                    {
                        buttonText.text = number.ToString();
                    }
                    else
                    {
                        buttonText.text = ""; // Последняя клетка пустая
                        emptyX = x;
                        emptyY = y;
                    }

                    // Назначение функции кнопке
                    button.onClick.AddListener(() => ButtonClicked(button));

                    // Настраиваем размеры и выравнивание кнопок
                    RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(100, 100); // Пример размеров кнопки
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
            rectTransform.sizeDelta = new Vector2(200, 50); // Пример размеров кнопки "Skip"
            rectTransform.anchoredPosition = new Vector2(654, 425); // Пример позиции кнопки "Skip"
        }

        private void SkipGame()
        {
            if (gameFinished) return;

            gameFinished = true;
            Debug.Log("Игра пропущена!");

            // Вызываем метод OnPuzzleSolved из PuzzleManager
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
                        solution[x, y] = 0; // Последняя клетка пустая
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
                Debug.Log("Игра завершена!");
                return;
            }

            int clickedX = -1;
            int clickedY = -1;

            // Находим координаты нажатой кнопки в массиве кнопок
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

            // Проверяем, можно ли переместить клетку
            if (CanMove(clickedX, clickedY))
            {
                // Меняем местами клетки
                SwapButtons(clickedX, clickedY);

                // Проверяем, завершена ли игра
                if (CheckVictory())
                {
                    Debug.Log("Поздравляю, вы выиграли!");
                    gameFinished = true;

                    // Вызываем метод OnPuzzleSolved из PuzzleManager
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
            // Проверяем возможность перемещения клетки в указанное место
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
            // Меняем местами тексты кнопок
            string tempText = buttons[x, y].GetComponentInChildren<TextMeshProUGUI>().text;
            buttons[x, y].GetComponentInChildren<TextMeshProUGUI>().text = buttons[emptyX, emptyY].GetComponentInChildren<TextMeshProUGUI>().text;
            buttons[emptyX, emptyY].GetComponentInChildren<TextMeshProUGUI>().text = tempText;

            // Обновляем координаты пустой клетки
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

                    // Проверяем текст кнопки
                    if (number == gridSize * gridSize)
                    {
                        // Последняя клетка должна быть пустой
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

