using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        private List<Rectangle> snake = new List<Rectangle>(); // Liste qui contient le corps du serpent
        private Rectangle food; // Nourriture
        private int directionX = 20, directionY = 0; // Direction du serpent
        private int snakeSpeed = 100; // Vitesse du serpent
        private Timer gameTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            this.Width = 640;
            this.Height = 480;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.KeyDown += new KeyEventHandler(OnKeyDown);
            InitializeGame();
            gameTimer.Interval = snakeSpeed;
            gameTimer.Tick += new EventHandler(Update);
            gameTimer.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.ResumeLayout(false);
        }

        private void InitializeGame()
        {
            // Initialiser la taille et la position du serpent
            snake.Clear();
            snake.Add(new Rectangle(100, 100, 20, 20));
            snake.Add(new Rectangle(80, 100, 20, 20));
            snake.Add(new Rectangle(60, 100, 20, 20));

            // Initialiser la nourriture à une position aléatoire
            Random rand = new Random();
            food = new Rectangle(rand.Next(0, this.Width / 20) * 20, rand.Next(0, this.Height / 20) * 20, 20, 20);
        }

        private void Update(object sender, EventArgs e)
        {
            // Déplacer le serpent
            for (int i = snake.Count - 1; i > 0; i--)
            {
                snake[i] = snake[i - 1];
            }

            snake[0] = new Rectangle(snake[0].X + directionX, snake[0].Y + directionY, 20, 20);

            // Vérifier si le serpent a mangé la nourriture
            if (snake[0].IntersectsWith(food))
            {
                snake.Add(new Rectangle(snake[snake.Count - 1].X, snake[snake.Count - 1].Y, 20, 20));

                Random rand = new Random();
                food.X = rand.Next(0, this.Width / 20) * 20;
                food.Y = rand.Next(0, this.Height / 20) * 20;
            }

            // Vérifier les collisions avec les bords de la fenêtre
            if (snake[0].X < 0 || snake[0].X >= this.Width || snake[0].Y < 0 || snake[0].Y >= this.Height)
            {
                GameOver();
            }

            // Vérifier les collisions avec le corps du serpent
            for (int i = 1; i < snake.Count; i++)
            {
                if (snake[0].IntersectsWith(snake[i]))
                {
                    GameOver();
                }
            }

            Invalidate(); // Repeindre la fenêtre
        }

        private void GameOver()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over! Score: " + (snake.Count - 3).ToString());
            InitializeGame();
            gameTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Dessiner le serpent
            foreach (Rectangle rect in snake)
            {
                g.FillRectangle(Brushes.Green, rect);
            }

            // Dessiner la nourriture
            g.FillRectangle(Brushes.Red, food);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (directionY == 0)
                    {
                        directionX = 0;
                        directionY = -20;
                    }
                    break;
                case Keys.Down:
                    if (directionY == 0)
                    {
                        directionX = 0;
                        directionY = 20;
                    }
                    break;
                case Keys.Left:
                    if (directionX == 0)
                    {
                        directionX = -20;
                        directionY = 0;
                    }
                    break;
                case Keys.Right:
                    if (directionX == 0)
                    {
                        directionX = 20;
                        directionY = 0;
                    }
                    break;
            }
        }
    }
}
