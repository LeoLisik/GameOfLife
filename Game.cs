using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Praktika11
{
    public partial class Game : Form
    {
        private Graphics Grafica;  
        private int CellSize = 10;  
        private bool[,] RedCells;  
        private bool[,] BlueCells; 
        private int Rows; 
        private int Columns;
        private List<bool[,]> ArraysForCheckEquals = new List<bool[,]>() { null, null }; // Список для хранения предыдущих состояний
        private Brush RedCellsColor = Brushes.IndianRed;
        private Brush BlueCellsColor = Brushes.BlueViolet;
        private bool IsMinimized = false; // Свернуто ли окно

        public Game() //создание формы
        {
            InitializeComponent();
            ResizeComponents(null, null);
        }

        private void StartGame()  //При начале игры запустить таймер
        {
            if (Timer.Enabled)
            {
                return;
            }
            Timer.Start();
            // Запретить изменение размеров во время игры
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            // Сохранение начального состояния
            ArraysForCheckEquals[0] = RedCells;
            ArraysForCheckEquals[1] = BlueCells;
            // Разрешить паузу и очистку после начала игры
            PauseButton.Enabled = true;
            CleanButton.Enabled = true;
        }

        private bool ClickPositionCheck(int MousePosX, int MousePosY)  //Проверка нахождение клика в границах поля
        {
            return MousePosX >= 0 && MousePosY >= 0 && MousePosX < Columns && MousePosY < Rows;  
        }

        private void Start(object sender, EventArgs e) //Запустить игру при нажатии кнопки
        {
            StartGame(); 
        }

        private void Exit(object sender, EventArgs e) //Закрыть приложение
        {
            Application.Exit(); 
        }

        private void ResizeComponents(object sender, EventArgs e) // Расчет размеров и позиций
        {
            if (WindowState == FormWindowState.Minimized) // Если окно свернули
            {
                IsMinimized = true;
                PauseClick(null, null);
                return;
            } else
            {
                if (IsMinimized) // При выходе из свернутого состояния
                {
                    IsMinimized = false;
                    PauseClick(null, null);
                    return;
                }
            }
            
            int XButtonsLoc = this.Width / 50; // Позиция кнопок по X
            int YBetweenSpace = 20; // Пространство между элементами по Y
            Size ButtonSizes = new Size(this.Width / 10, this.Height / 15); // Размер кнопок
            int FontSize = this.Width / 110 + 1;
            Font ButtonFont = new Font("Microsoft Sans Serif", FontSize); // Шрифт кнопок

            StartButton.Location = new Point(XButtonsLoc, 0 * (ButtonSizes.Height + YBetweenSpace) + YBetweenSpace);
            StartButton.Size = ButtonSizes;
            StartButton.Font = ButtonFont;

            PauseButton.Location = new Point(XButtonsLoc, 1 * (ButtonSizes.Height + YBetweenSpace) + YBetweenSpace);
            PauseButton.Size = ButtonSizes;
            PauseButton.Font = ButtonFont;

            CleanButton.Location = new Point(XButtonsLoc, 2 * (ButtonSizes.Height + YBetweenSpace) + YBetweenSpace);
            CleanButton.Size = ButtonSizes;
            CleanButton.Font = ButtonFont;

            ExitButton.Location = new Point(XButtonsLoc, 3 * (ButtonSizes.Height + YBetweenSpace) + YBetweenSpace);
            ExitButton.Size = ButtonSizes;
            ExitButton.Font = ButtonFont;

            GamePlace.Location = new Point(ButtonSizes.Width + XButtonsLoc * 2, YBetweenSpace); // Ширина кнопки + два расстояния между дают позицию игрового поля
            GamePlace.Size = new Size(this.Width - GamePlace.Location.X - XButtonsLoc * 2, this.Height - YBetweenSpace * 4); // Множители подобраны путем перебора

            CellSize = GamePlace.Height / 60; // По вертикали всегда будет 60 клеток

            StartCalcVariable(null, null); 
        }

        private int CountNeighbors(int x, int y, bool[,] a)  //Подсчет кол-ва соседей клетки
        {
            int CountNeighbors = 0;
            for (int i = -1; i < 2; i++)  //В циклах обрабатываем всех соседей
            {
                for (int j = -1; j < 2; j++)
                {
                    int NeighboringCol = (x + i + Columns) % Columns; //Нахождение соседних столбцов
                    int NeighboringRow = (y + j + Rows) % Rows; //Нахождение соседних строк
                    bool Samoproverka = NeighboringCol == x && NeighboringRow == y; //является ли проверка соседа самопроверкой
                    bool IsAlive = a[NeighboringCol, NeighboringRow];
                    if (IsAlive && !Samoproverka)  //Если клетка имеет жизнь и не самопроверка, увеличить кол-во соседей
                    {
                        CountNeighbors++;
                    }
                }
            }
            return CountNeighbors;
        }

        private void NextGeneration()  //Просчет следующего поколения
        {
            Grafica.Clear(Color.White);  //Очистить поле
            var NewRedCells = new bool[Columns, Rows];
            var NewBlueCells = new bool[Columns, Rows];
            for (int x = 0; x < Columns; x++)  //2 цикла for для прохода по всем клеткам массивов 
            {
                for (int y = 0; y < Rows; y++)
                {
                    int NeighborsRed = CountNeighbors(x, y, RedCells);
                    bool IsAliveRed = RedCells[x, y];
                    int NeighborsBlue = CountNeighbors(x, y, BlueCells);
                    bool IsAliveBlue = BlueCells[x, y];
                    if (IsAliveRed && !IsAliveBlue && NeighborsBlue == 3) //если синяя клетка появляется там, где жива красная
                    {
                        NewRedCells[x, y] = false;
                        NewBlueCells[x, y] = true;
                        Grafica.FillRectangle(BlueCellsColor, x * CellSize, y * CellSize, CellSize - 1, CellSize - 1);
                        continue;
                    }
                    if (IsAliveBlue && !IsAliveRed && NeighborsRed == 3)  //если красная клетка появляется там, где жива синяя 
                    {
                        NewRedCells[x, y] = true;
                        NewBlueCells[x, y] = false;
                        Grafica.FillRectangle(RedCellsColor, x * CellSize, y * CellSize, CellSize - 1, CellSize - 1);
                        continue;
                    }
                    if ((!IsAliveRed && !IsAliveBlue) && (NeighborsRed == 3 && NeighborsBlue == 3))  //если синяя и красная на пустой
                    {
                        NewBlueCells[x, y] = true;
                        Grafica.FillRectangle(RedCellsColor, x * CellSize, y * CellSize, CellSize - 1, CellSize - 1);
                        continue;
                    }
                    /////////////////////////////////////////////////
                    if (!IsAliveRed && NeighborsRed == 3)  //если клетка пуста и 3 соседа красных 
                    {
                        NewRedCells[x, y] = true;
                    }
                    else if (IsAliveRed && (NeighborsRed < 2 || NeighborsRed > 3)) //если красная жива и рядом меньше 2 или больше 3 соседей
                    {
                        NewRedCells[x, y] = false;
                    }
                    else //Если ни одно условие не сработало, клетка остаётся такой же
                    {
                        NewRedCells[x, y] = RedCells[x, y];
                    }
                    ////////////////////////////////////////////////
                    if (!IsAliveBlue && NeighborsBlue == 3) //если клетка пуста и имеет 3 соседа синих
                    {
                        NewBlueCells[x, y] = true;
                    }
                    else if (IsAliveBlue && (NeighborsBlue < 2 || NeighborsBlue > 3))  //если синяя клетка жива и рядом меньше 2 или больше 3 соседей
                    {
                        NewBlueCells[x, y] = false;
                    }
                    else //Если ни одно условие не сработало, клетка остаётся такой же
                    {
                        NewBlueCells[x, y] = BlueCells[x, y];
                    }
                    /////////////////////////////////////////////
                    if (NewRedCells[x, y])  //Если клетка по координатам красная, то покрасить клетку
                    {
                        Grafica.FillRectangle(RedCellsColor, x * CellSize, y * CellSize, CellSize - 1, CellSize - 1);
                    }
                    else if (NewBlueCells[x, y])  //Если клетка по координатам синяя, то покрасить клетку
                    {
                        Grafica.FillRectangle(BlueCellsColor, x * CellSize, y * CellSize, CellSize - 1, CellSize - 1);
                    }
                }
            }
            RedCells = NewRedCells; //Новые поколения становятся текущими
            BlueCells = NewBlueCells;
            GamePlace.Refresh();  //Отрисовываются изменения
        }

        private void TimerTick(object sender, EventArgs e)  //Каждый тик расчитывать новое поколение
        {
            NextGeneration();
            if (new Random().Next(10) == 1) // С 10% вероятностью сохранять текущее состояние для проверки
            {
                ArraysForCheckEquals[0] = RedCells;
                ArraysForCheckEquals[1] = BlueCells;
            }
            else
            {
                if (IsStateEquals()) // Проверка совпадения состояний с сохраненным
                {
                    PauseClick(null, null);
                    MessageBox.Show("Состояние повторилось. Игра окончена."); // TODO: Почему-то срабатывает 2 раза, я хз почему, пофиксить когда нибудь
                }
            }
        }

        private bool IsStateEquals() // Проверка текущего состояния с сохраненным
        {
            for (int x = 0; x < RedCells.GetLength(0); x++) // Сравнение красных
            {
                for (int y = 0; y < RedCells.GetLength(1); y++)
                {
                    if (RedCells[x, y] != ArraysForCheckEquals[0][x, y]) // Если состояние клетки не совпало, значит есть различия
                    {
                        return false;
                    }
                }
            }
            for (int x = 0; x < BlueCells.GetLength(0); x++) // Сравнение синих
            {
                for (int y = 0; y < BlueCells.GetLength(1); y++)
                {
                    if (BlueCells[x, y] != ArraysForCheckEquals[1][x, y]) // Если состояние клетки не совпало, значит есть различия
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void PauseClick(object sender, EventArgs e)  //Пауза
        {
            Timer.Enabled = !Timer.Enabled;
            if (Timer.Enabled) // Изменение возможности менять размер
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
            } else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.MaximizeBox = true;
            }
            Bitmap bitmap = (Bitmap)GamePlace.Image;
            GamePlace.Image = bitmap;  //Создаём игровое поле
            Grafica = Graphics.FromImage(GamePlace.Image);  //Переносим поле в изображение
            for (int i = CellSize - 1; i < bitmap.Width; i += CellSize) // Рисуем клетки
            {
                Grafica.DrawLine(Pens.Black, i, 0, i, bitmap.Height);
            }
            for (int i = CellSize - 1; i < bitmap.Height; i += CellSize)
            {
                Grafica.DrawLine(Pens.Black, 0, i, bitmap.Width, i);
            }
        }

        private void PrintColor(object sender, MouseEventArgs e)  //Закрасить клетку
        {
            if (Timer.Enabled)  //Если таймер включен, то рисовать нельзя
            {
                return;
            }
            int MousePosX = e.Location.X / CellSize;  //координаты клика
            int MousePosY = e.Location.Y / CellSize;

            if (ClickPositionCheck(MousePosX, MousePosY))  //если координата клетки в пределах поля
            {
                if (e.Button == MouseButtons.Left)  //если нажата ЛКМ
                {
                    if (RedCells[MousePosX, MousePosY])  //если клетка уже существует - стереть
                    {
                        RedCells[MousePosX, MousePosY] = false;
                        Grafica.FillRectangle(Brushes.White, MousePosX * CellSize, MousePosY * CellSize, CellSize - 1, CellSize - 1);
                    }
                    else  //если клетки ещё не существует - создать
                    {
                        RedCells[MousePosX, MousePosY] = true;
                        Grafica.FillRectangle(RedCellsColor, MousePosX * CellSize, MousePosY * CellSize, CellSize - 1, CellSize - 1);
                    }
                }
                if (e.Button == MouseButtons.Right)  //если нажата ПКМ
                {
                    if (BlueCells[MousePosX, MousePosY])  //если клетка уже существует - стереть
                    {
                        BlueCells[MousePosX, MousePosY] = false;
                        Grafica.FillRectangle(Brushes.White, MousePosX * CellSize, MousePosY * CellSize, CellSize - 1, CellSize - 1);
                    }
                    else  //если клетки ещё не существует - создать
                    {
                        BlueCells[MousePosX, MousePosY] = true;
                        Grafica.FillRectangle(BlueCellsColor, MousePosX * CellSize, MousePosY * CellSize, CellSize - 1, CellSize - 1);
                    }
                }
            }
            GamePlace.Refresh();  //Отобразить изменения
        }

        private void StartCalcVariable(object sender, EventArgs e)
        {
            Rows = GamePlace.Height / CellSize; 
            Columns = GamePlace.Width / CellSize; 
            RedCells = new bool[Columns, Rows];  //Инициализируем новый размер массивов для 2-х цветов
            BlueCells = new bool[Columns, Rows];
            Bitmap bitmap = new Bitmap(GamePlace.Width, GamePlace.Height);
            GamePlace.Image = bitmap;  //Создаём сетку игры 
            Grafica = Graphics.FromImage(GamePlace.Image);  //Переносим сетку в изображение
            Grafica.Clear(Color.White);  //Заполняем графику белым цветом
            for (int i = CellSize - 1; i < bitmap.Width; i += CellSize) // Рисуем клетки
            {
                Grafica.DrawLine(Pens.Black, i, 0, i, bitmap.Height);
            }
            for (int i = CellSize - 1; i < bitmap.Height; i += CellSize)
            {
                Grafica.DrawLine(Pens.Black, 0, i, bitmap.Width, i);
            }
        }

        private void ClearColors(object sender, EventArgs e)
        {
            PauseClick(null, null);
            Bitmap bitmap = new Bitmap(GamePlace.Width, GamePlace.Height);
            GamePlace.Image = bitmap;  //Создать новое поле 
            Grafica = Graphics.FromImage(GamePlace.Image);  //Инициализировать переменную Grafica полем 
            Grafica.Clear(Color.White);  //заполнение поля белым цветом
            for (int i = CellSize - 1; i < bitmap.Width; i += CellSize) // Рисуем клетки
            {
                Grafica.DrawLine(Pens.Black, i, 0, i, bitmap.Height);
            }
            for (int i = CellSize - 1; i < bitmap.Height; i += CellSize)
            {
                Grafica.DrawLine(Pens.Black, 0, i, bitmap.Width, i);
            }
            for (int i = 0; i < Columns; i++)  //В двойном цикле пройтись по всему массиву и очистить его.
            {
                for (int j = 0; j < Rows; j++) 
                {
                   RedCells[i, j] = false; 
                   BlueCells[i, j] = false; 
                }
            }
        }
    }
}
