using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewGame
{
    public partial class Form1 : Form
    {
        Image playerImageRun, playerImageIdle, playerImageJump;
        private bool LeftSide;
        bool goLeft, goRight, jumping, isGameOver;
        private int PressedKey = 0;
        private int currFrame = 0;
        private int currAnimation = 0;
        private int currFrameIdle = 0;
        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;
        int backgroundSpeed = 8;
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        public Form1()
        {
            InitializeComponent();
            playerImageRun = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_run.png");
            playerImageIdle = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_idle.png");
            playerImageJump = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_jump.png");
            timer1.Interval = 50;
            timer1.Tick += new EventHandler(update);
            timer1.Start();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            player.BackColor = Color.Transparent;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            txtScore.BackColor = Color.Transparent;

        }

        private void update(object sender, EventArgs e)
        {
            if (PressedKey == 1)
            {
                timer1.Interval = 100;
                currFrameIdle = 0;
                playAnimationRun();
                if (currAnimation == 3)
                {
                    playAnimationJump();
                }
                if (currFrame >4)
                    currFrame = 0;
            }
            else
            {
                timer1.Interval = 100;
                playAnimationIdle();
                if (currFrameIdle == 2)
                    currFrameIdle = 0;
            }
            currFrame++;
            currFrameIdle++;
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            currFrameIdle = 0;
            PressedKey = 1;
            if (e.KeyCode == Keys.Left)
            {
                LeftSide = true;
                goLeft = true;
                currAnimation = 1;
            }
            if (e.KeyCode == Keys.Right)
            {
                LeftSide = false;
                goRight = true;
                currAnimation = 2;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
                currAnimation = 3;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            PressedKey = 0;
            if (e.KeyCode == Keys.Left)
            {
                LeftSide = true;
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                LeftSide = false;
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }
            currAnimation = 0;
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }


        }
        private void MainGame(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            playAnimationIdle();
            player.Top += jumpSpeed;
            if (goLeft == true)
            {              
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox)
                {


                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            player.Top = x.Top - player.Height;


                            if ((string)x.Name == "horizontalPlatform" && goLeft == false || (string)x.Name == "horizontalPlatform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }


                        }

                        x.BringToFront();

                    }

                    if ((string)x.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            score++;
                        }
                    }


                    if ((string)x.Tag == "enemy")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            gameTimer.Stop();
                            isGameOver = true;
                            txtScore.Text = "Score: " + score + MessageBox.Show("Вы убиты!");
                        }
                    }
                    if (player.Top + player.Height > this.ClientSize.Height + 60)
                    {
                        gameTimer.Stop();
                        MessageBox.Show("Вы мертвы!");
                    }
                    if (goRight && background.Left > 0)
                    {
                        background.Left -= backLeft;

                        foreach (Control x in this.Controls)
                        {
                            if (x is PictureBox && (string)x.Tag == "platform" || x is PictureBox && (string)x.Tag == "coin" || x is PictureBox && (string)x.Tag == "door")
                            {
                                x.Left -= backLeft;
                            }
                        }

                    }
                    if (goLeft && background.Left < 0)
                    {
                        background.Left += backLeft;


                        foreach (Control x in this.Controls)
                        {
                            if (x is PictureBox && (string)x.Tag == "platform" || x is PictureBox && (string)x.Tag == "coin" || x is PictureBox && (string)x.Tag == "door")
                            {
                                x.Left += backLeft;
                            }
                        }
                    }
                }

            }
            horizontalPlatform.Left -= horizontalSpeed;

            if (horizontalPlatform.Left < 0 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlatform.Top += verticalSpeed;

            if (verticalPlatform.Top < 195 || verticalPlatform.Top > 581)
            {
                verticalSpeed = -verticalSpeed;
            }


            enemyOne.Left -= enemyOneSpeed;

            if (enemyOne.Left < pictureBox5.Left || enemyOne.Left + enemyOne.Width > pictureBox5.Left + pictureBox5.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }

            enemyTwo.Left += enemyTwoSpeed;

            if (enemyTwo.Left < pictureBox2.Left || enemyTwo.Left + enemyTwo.Width > pictureBox2.Left + pictureBox2.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }


            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "Вы мертвы!";
            }

            if (player.Bounds.IntersectsWith(door.Bounds) && score > 1)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "Миссия выполнена!";
            }
            else
            {
                txtScore.Text = "Score: " + score + Environment.NewLine + "Нужно собрать все монеты";
            }


        }
        private void RestartGame()
        {

            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;

            txtScore.Text = "Score: " + score;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }          
            player.Left = 72;
            player.Top = 475;

            enemyOne.Left = 471;
            enemyTwo.Left = 360;

            horizontalPlatform.Left = 275;
            verticalPlatform.Top = 581;

            gameTimer.Start();


        }

        private void playAnimationIdle()
        {
            if (currAnimation == 0)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageIdle, 0, 0, new Rectangle(new Point(48 * currFrameIdle, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                player.Size = new Size(48, 48);
                player.Image = part;
            }
            if (currAnimation == 0 && LeftSide)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageIdle, 0, 0, new Rectangle(new Point(48 * currFrameIdle, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                player.Size = new Size(48, 48);
                player.Image = part;
            }
        }
        private void playAnimationRun()
        {
            if (goRight)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageRun, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                player.Size = new Size(48, 48);
                player.Image = part;
            }
            if (goLeft)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageRun, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                player.Size = new Size(48, 48);
                player.Image = part;

            }
        }
        private void playAnimationJump()
        {
            if (currAnimation == 3)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageJump, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                player.Size = new Size(48, 48);
                player.Image = part;
            }
            if (jumping && LeftSide)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageJump, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                player.Size = new Size(48, 48);
                player.Image = part;

            }
        }
    }
}
/*namespace NewGame
{
    public partial class Form1 : Form
    {
        Image playerImageWalk, playerImageRun, playerImageIdle, playerImageJump;
        private int currFrame = 0;
        private int currAnimation = 0;
        private int currFrameIdle = 0;
        private int ShiftRun = 0;
        private int PressedKey = 0;

        public Form1()
        {
            InitializeComponent();
            playerImageWalk = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_walk.png");
            playerImageRun = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_run.png");
            playerImageIdle = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_idle.png");
            playerImageJump = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_jump.png");
            timer1.Interval = 100;
            timer2.Interval = 40;
            timer1.Tick += new EventHandler(update);
            timer2.Tick += new EventHandler(AnimationMovement);
            timer2.Start();
            timer1.Start();
            this.KeyDown += new KeyEventHandler(keyboard);
            this.KeyUp += new KeyEventHandler(freeKey);

        }

        private void AnimationMovement(object sender, EventArgs e)
        {
            switch (currAnimation)
            {
                case 2:
                    pictureBox1.Location = new Point(pictureBox1.Location.X + 1, pictureBox1.Location.Y);
                    break;
                case 1:
                    pictureBox1.Location = new Point(pictureBox1.Location.X - 1, pictureBox1.Location.Y);
                    break;
                case 3:
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 10);
                    break;
                case 4:
                    currAnimation = 2;
                    pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 40);
                    break;

            }
            if (PressedKey == 2 && ShiftRun == 1)
            {
                switch (currAnimation)
                {
                    case 2:
                        pictureBox1.Location = new Point(pictureBox1.Location.X + 5, pictureBox1.Location.Y);
                        break;
                    case 1:
                        pictureBox1.Location = new Point(pictureBox1.Location.X - 5, pictureBox1.Location.Y);
                        break;
                    case 3:
                        pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + 20);
                        break;
                    case 4:
                        currAnimation = 2;
                        pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 40);
                        break;
                }
            }                                 
        }
        private void keyboard(object sender, KeyEventArgs e)
        {
            currFrameIdle = 0;
            PressedKey = 1;
            ShiftRun = 0;
            switch (e.KeyCode.ToString())
            {
                case "D":
                    currAnimation = 2;
                    break;
                case "A":
                    currAnimation = 1;
                    break;
                case "S":
                    currAnimation = 3;
                    break;
                case "Space":
                    currAnimation = 4;
                    break;

            }
            if (e.Shift)
            {
                ShiftRun = 1;
                PressedKey = 2;
                if (e.KeyCode == Keys.D && e.Shift)
                {
                    currAnimation = 2;
                }
                if (e.KeyCode == Keys.A && e.Shift)
                {
                    currAnimation = 1;
                }
                if (e.KeyCode == Keys.Space && e.Shift)
                {
                    currAnimation = 4;
                }
                if (e.KeyCode == Keys.S && e.Shift)
                {
                    currAnimation = 3;
                }
            }

        }
        private void freeKey(object sender, KeyEventArgs e)
        {
            currFrame = 0;
            PressedKey = 0;
            ShiftRun = 0;
            switch (e.KeyCode.ToString())
            {
                case "D":
                    currAnimation = 6;
                    break;
                case "A":
                    currAnimation = 5;
                    break;
                case "S":
                    currAnimation = 7;
                    break;
                case "Space":
                    currAnimation = 8;
                    break;

            }
        }
        private void update(object sender, EventArgs e)
        {
            if (PressedKey == 1)
            {
                timer1.Interval = 100;
                currFrameIdle = 0;
                playAnimationWalk();
                if (currAnimation == 4)
                {
                    playAnimationJump();
                }
                if (currFrame == 5)
                    currFrame = 0;
            }
            if (PressedKey == 2 && ShiftRun == 1)
            {
                timer1.Interval = 100;
                currFrameIdle = 0;
                playAnimationRun();
                if (currFrame == 5)
                    currFrame = 0;
            }
            else
            {
                timer1.Interval = 100;
                playAnimationIdle();
                if (currFrameIdle == 2)
                    currFrameIdle = 0;
            }
            currFrame++;
            currFrameIdle++;

        }
        private void playAnimationWalk()
        {
            if (currAnimation != 0)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageWalk, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;
            }
            if (currAnimation == 1)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageWalk, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;
            }
        }

        private void playAnimationIdle()
        {
            if (currAnimation > 5)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageIdle, 0, 0, new Rectangle(new Point(48 * currFrameIdle, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;
            }
            if (currAnimation == 5)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageIdle, 0, 0, new Rectangle(new Point(48 * currFrameIdle, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;
            }
        }
        private void playAnimationRun()
        {
            if (currAnimation > 1)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageRun, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;
            }
            if (currAnimation == 1)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageRun, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;

            }
        }
        private void playAnimationJump()
        {
            if (currAnimation == 4)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageJump, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;
            }
            if (currAnimation == 5 && currAnimation == 4)
            {
                Image part = new Bitmap(48, 48);
                Graphics g = Graphics.FromImage(part);
                g.DrawImage(playerImageJump, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox1.Size = new Size(48, 48);
                pictureBox1.Image = part;

            }
        }
    }
}
*/