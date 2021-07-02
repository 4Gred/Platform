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
