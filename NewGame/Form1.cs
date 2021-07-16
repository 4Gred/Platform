using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace NewGame
{
    public partial class Form1 : Form
    {

        Image playerImageRun, playerImageIdle, playerImageJump, enemyOneImageRun, enemyTwoImageRun;
        private bool LeftSide, enemyOneLeft = true, enemyOneRight = false, enemyTwoLeft = true, enemyTwoRight = false;
        bool goLeft, goRight, jumping, isGameOver;
        private int PressedKey = 0;
        private int currFrame = 0;
        private int currAnimation = 0;
        private int currFrameIdle = 0;
        int jumpSpeed;
        int force;
        int score = 0;
        int key = 0;
        int playerSpeed = 7;
        int backgroundSpeed = 8;
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;
        int gilTopSpeed = 1;
        int gilBottomSpeed = 1;
        private int currFrameEnemy = 0;

        public class Save
        {
            public int X;
            public int Y;

            public Save(int _x, int _y)
            {
                X = _x;
                Y = _y;
            }
        }
        public class InvSave
        {
            public int KEY;
            public int COIN;

            public InvSave(int _key, int _coin)
            {
                KEY = _key;
                COIN = _coin;
            }
        }
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            playerImageRun = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_run.png");
            enemyOneImageRun = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\SteamMan\\SteamMan_run.png");
            enemyTwoImageRun = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\GraveRobber\\GraveRobber_run.png");
            playerImageIdle = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_idle.png");
            playerImageJump = new Bitmap("D:\\Programs\\Platform\\Platform\\Sprite\\Woodcutter\\Woodcutter_jump.png");
            timer1.Interval = 20;
            timer1.Tick += new EventHandler(update);
            timer1.Start();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            player.BackColor = Color.Transparent;
            enemyOne.BackColor = Color.Transparent;
            enemyTwo.BackColor = Color.Transparent;

        }
        private void update(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                playAnimation(5);
                i = 0;
            }
            timer1.Interval = 20;
            if (PressedKey == 1)
            {
                currFrameIdle = 0;
                playAnimation(1);
                if (currAnimation == 3)
                {
                    playAnimation(2);
                }
                if (currFrame > 3)
                    currFrame = 0;
            }
            else
            {
                playAnimation(0);
                if (currFrameIdle == 2)
                    currFrameIdle = 0;
            }
            currFrame++;
            currFrameIdle++;
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            List<Save> saves = new List<Save>();
            List<InvSave> invSave = new List<InvSave>();
            string pathSave = @"D:\Programs\Platform\Platform\NewGame\Saves\saves.bin";
            string pathInvSave = @"D:\Programs\Platform\Platform\NewGame\Saves\invSaves.bin";
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
            if (e.KeyCode == Keys.F1)
            {
                saves.Add(new Save(player.Location.X, player.Location.Y));
                saves.Add(new Save(enemyOne.Location.X, enemyOne.Location.Y));
                saves.Add(new Save(enemyTwo.Location.X, enemyTwo.Location.Y));
                saves.Add(new Save(gil1.Location.X, gil1.Location.Y));
                saves.Add(new Save(gil2.Location.X, gil2.Location.Y));
                saves.Add(new Save(gil3.Location.X, gil3.Location.Y));
                saves.Add(new Save(gil4.Location.X, gil4.Location.Y));
                saves.Add(new Save(gil5.Location.X, gil5.Location.Y));
                saves.Add(new Save(gil6.Location.X, gil6.Location.Y));
                invSave.Add(new InvSave(key, score));
                BinaryWriter writer = new BinaryWriter(new FileStream(pathSave, FileMode.Open));
                BinaryWriter invWriter = new BinaryWriter(new FileStream(pathInvSave, FileMode.Open));
                for (int i = 0; i < saves.Count; i++)
                {
                    writer.Write(saves[i].X);
                    writer.Write(saves[i].Y);
                }
                for (int i = 0; i < invSave.Count; i++)
                {
                    invWriter.Write(invSave[i].KEY);
                    invWriter.Write(invSave[i].COIN);
                }
                writer.Flush();
                writer.Close();
                invWriter.Flush();
                invWriter.Close();
            }
            if (e.KeyCode == Keys.F2)
            {
                BinaryReader Savereader = new BinaryReader(new FileStream(pathSave, FileMode.OpenOrCreate));
                BinaryReader InvSavereader = new BinaryReader(new FileStream(pathInvSave, FileMode.OpenOrCreate));
                while (Savereader.BaseStream.Position < Savereader.BaseStream.Length)
                {
                    int X = Savereader.ReadInt32();
                    int Y = Savereader.ReadInt32();
                    Save loads = new Save(X, Y);
                    saves.Add(loads);
                }
                while (InvSavereader.BaseStream.Position < InvSavereader.BaseStream.Length)
                {
                    int KEY = InvSavereader.ReadInt32();
                    int COIN = InvSavereader.ReadInt32();
                    InvSave invLoad = new InvSave(KEY, COIN);
                    invSave.Add(invLoad);
                }
                Savereader.Close();
                InvSavereader.Close();
                player.Location = new Point(saves[0].X, saves[0].Y);
                enemyOne.Location = new Point(saves[1].X, saves[1].Y);
                enemyTwo.Location = new Point(saves[2].X, saves[2].Y);
                gil1.Location = new Point(saves[3].X, saves[3].Y);
                gil2.Location = new Point(saves[4].X, saves[4].Y);
                gil3.Location = new Point(saves[5].X, saves[5].Y);
                gil4.Location = new Point(saves[6].X, saves[6].Y);
                gil5.Location = new Point(saves[7].X, saves[7].Y);
                gil6.Location = new Point(saves[8].X, saves[8].Y);
                key = invSave[0].KEY;
                score = invSave[0].COIN;
                if (key == 1)
                {
                    pictureBox15.Visible = false;
                }
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
            playAnimation(0);
            player.Top += jumpSpeed;
            if (goLeft == true && player.Left > 0)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true && player.Left + (player.Width) < this.ClientSize.Width)
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
            if (goLeft && background.Left < 0)
            {
                background.Left += backgroundSpeed;
                MoveGameElements("forward");
            }
            if (goRight && background.Left > -641)
            {
                background.Left -= backgroundSpeed;
                MoveGameElements("back");
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
                    if ((string)x.Tag == "fake")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                        }
                    }
                    if ((string)x.Tag == "key")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {
                            x.Visible = false;
                            key++;
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
                }
            }
            /* horizontalPlatform.Left -= horizontalSpeed;

             if (horizontalPlatform.Left < 0 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
             {
                 horizontalSpeed = -horizontalSpeed;
             }*/

            verticalPlatform.Top += verticalSpeed;
            if (verticalPlatform.Top < 467 || verticalPlatform.Top > 647)
            {
                verticalSpeed = -verticalSpeed;
            }
            gil1.Top += gilTopSpeed;
            gil3.Top += gilTopSpeed;
            gil6.Top += gilTopSpeed;
            if (gil1.Location.Y > 65)
            {
                gilTopSpeed = -gilTopSpeed;
                if (gil1.Location.Y > 65)
                {
                    gil1.Visible = false;
                    gil3.Visible = false;
                    gil6.Visible = false;

                }
            }
            if (gil1.Location.Y < 150)
            {
                gilTopSpeed = -gilTopSpeed;
                if (gil1.Location.Y < 85)
                {
                    gil1.Visible = true;
                    gil3.Visible = true;
                    gil6.Visible = true;

                }
            }
            gil2.Top += gilBottomSpeed;
            gil4.Top += gilBottomSpeed;
            gil5.Top += gilBottomSpeed;
            if (gil2.Top > 10 || gil2.Top < -20)
            {
                gilBottomSpeed = -gilBottomSpeed;
            }
            playAnimation(3);
            enemyOne.Left -= enemyOneSpeed;
            if (enemyOne.Left < pictureBox3.Left || enemyOne.Left + enemyOne.Width > pictureBox3.Left + pictureBox3.Width)
            {
                playAnimation(3);
                enemyOneLeft = false;
                enemyOneRight = true;
                currFrameEnemy++;
                enemyOneSpeed = -enemyOneSpeed;
            }
            playAnimation(4);
            enemyTwo.Left += enemyTwoSpeed;
            if (enemyTwo.Left < pictureBox19.Left || enemyTwo.Left + enemyTwo.Width > pictureBox19.Left + pictureBox19.Width)
            {
                playAnimation(4);
                enemyTwoLeft = false;
                enemyTwoRight = true;
                enemyTwoSpeed = -enemyTwoSpeed;
            }
            if (currFrameEnemy == 4)
            {
                currFrameEnemy = 0;
            }
            if (player.Top + player.Height > this.ClientSize.Height + 60)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + MessageBox.Show("Вы мертвы!");
            }
            if (player.Bounds.IntersectsWith(door.Bounds) && score >= 1 && key == 1)
            {
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + MessageBox.Show("Миссия выполнена!");
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
            Form1 newWindow = new Form1();
            newWindow.Show();
            this.Hide();
        }
        private void MoveGameElements(string direction)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "platform" || x is PictureBox && (string)x.Tag == "fake" || x is PictureBox && (string)x.Tag == "coin" || x is PictureBox && (string)x.Tag == "door" || x is PictureBox && (string)x.Tag == "enemy" || x is PictureBox && (string)x.Tag == "environment" || x is PictureBox && (string)x.Tag == "key")
                {
                    this.DoubleBuffered = true;
                    if (direction == "back")
                    {
                        x.Left -= backgroundSpeed;
                    }
                    if (direction == "forward")
                    {
                        x.Left += backgroundSpeed;
                    }
                    if (direction == "Top")
                    {
                        x.Left -= backgroundSpeed;
                    }
                    if (direction == "Bottom")
                    {
                        x.Left -= backgroundSpeed;
                    }
                }
            }
        }
        private void playAnimation(int x)
        {
            this.DoubleBuffered = true;
            Image part = new Bitmap(48, 48);
            Graphics g = Graphics.FromImage(part);
            switch (x)
            {
                case 0: //idle
                    player.Size = new Size(48, 48);
                    player.Image = part;
                    if (currAnimation == 0)
                    {
                        g.DrawImage(playerImageIdle, 0, 0, new Rectangle(new Point(48 * currFrameIdle, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                    }
                    if (currAnimation == 0 && LeftSide)
                    {

                        g.DrawImage(playerImageIdle, 0, 0, new Rectangle(new Point(48 * currFrameIdle, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                        part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    break;
                case 1: //run
                    player.Size = new Size(48, 48);
                    player.Image = part;
                    if (goRight)
                    {
                        g.DrawImage(playerImageRun, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                    }
                    if (goLeft)
                    {
                        g.DrawImage(playerImageRun, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                        part.RotateFlip(RotateFlipType.RotateNoneFlipX);

                    }
                    break;
                case 2: //jump
                    player.Size = new Size(48, 48);
                    player.Image = part;
                    if (currAnimation == 3)
                    {
                        g.DrawImage(playerImageJump, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                    }
                    if (jumping && LeftSide)
                    {

                        g.DrawImage(playerImageJump, 0, 0, new Rectangle(new Point(48 * currFrame, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                        part.RotateFlip(RotateFlipType.RotateNoneFlipX);

                    }
                    break;
                case 3: //enemyOne
                    enemyOne.Size = new Size(48, 48);
                    enemyOne.Image = part;
                    if (enemyOneLeft)
                    {
                        g.DrawImage(enemyOneImageRun, 0, 0, new Rectangle(new Point(48 * currFrameEnemy, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                    }
                    if (enemyOneRight)
                    {
                        g.DrawImage(enemyOneImageRun, 0, 0, new Rectangle(new Point(48 * currFrameEnemy, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                        part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    break;
                case 4://enemyTwo
                    enemyTwo.Size = new Size(48, 48);
                    enemyTwo.Image = part;
                    if (enemyTwoLeft)
                    {
                        g.DrawImage(enemyTwoImageRun, 0, 0, new Rectangle(new Point(48 * currFrameEnemy, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                    }
                    if (enemyTwoRight)
                    {

                        g.DrawImage(enemyTwoImageRun, 0, 0, new Rectangle(new Point(48 * currFrameEnemy, 0), new Size(48, 48)), GraphicsUnit.Pixel);
                        part.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    break;
                case 5://bigSaw
                    Image flipImage = saw.Image;
                    flipImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    saw.Image = flipImage;
                    break;
            }
        }
    }
}
