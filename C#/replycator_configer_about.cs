using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NAudio.Wave;

namespace ReplicatorConfiger
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load icon
            Icon icon = new Icon("src/icon.ico");

            // Create window
            Form window = new Form();
            window.Text = "About Replycator";
            window.Width = 360;
            window.Height = 500;
            window.Icon = icon;

            // Load background animation
            var backgroundAnimation = new Gif("src/backround_about.gif");
            var backgroundSprite = new PictureBox
            {
                Location = new System.Drawing.Point(0, -35),
                Size = new System.Drawing.Size(360, 500),
                Image = backgroundAnimation.GetNextFrame()
            };
            window.Controls.Add(backgroundSprite);

            // Create rectangle
            var square = new Panel
            {
                Location = new System.Drawing.Point(40, -20),
                Size = new System.Drawing.Size(280, 150),
                BackColor = System.Drawing.Color.FromArgb(10, 10, 10)
            };
            window.Controls.Add(square);

            // Play audio
            var audioSource = new AudioFileReader("src/exampler.ogg");
            var audioPlayer = new WaveOutEvent();
            audioPlayer.Init(audioSource);
            audioPlayer.Play();
            audioPlayer.Loop = true;

            // Add text labels
            var version = 0.01;
            var replycatorText = new Label
            {
                Text = $"Replycator {version}",
                Font = new System.Drawing.Font("Ubuntu", 15f),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 360,
                Location = new System.Drawing.Point(0, 108)
            };
            window.Controls.Add(replycatorText);

            var developerConfiger = new Label
            {
                Text = "Developer configer by exampler",
                Font = new System.Drawing.Font("Ubuntu", 13f),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 360,
                Location = new System.Drawing.Point(0, 88)
            };
            window.Controls.Add(developerConfiger);

            var developerBashScripter = new Label
            {
                Text = "Developer bash scriptes by exampler",
                Font = new System.Drawing.Font("Ubuntu", 12f),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 360,
                Location = new System.Drawing.Point(0, 68)
            };
            window.Controls.Add(developerBashScripter);

            var developerTelegramBot = new Label
            {
                Text = "Developer telegram bot by exampler",
                Font = new System.Drawing.Font("Ubuntu", 13f),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 360,
                Location = new System.Drawing.Point(0, 48)
            };
            window.Controls.Add(developerTelegramBot);

            var designerConfiger = new Label
            {
                Text = "Design configer by exampler",
                Font = new System.Drawing.Font("Ubuntu", 13f),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 360,
                Location = new System.Drawing.Point(0, 28)
            };
            window.Controls.Add(designerConfiger);

            var walm2024 = new Label
            {
                Text = "",
                Font = new System.Drawing.Font("Ubuntu", 15f),
                TextAlign = ContentAlignment.MiddleCenter,
                Width = 360,
                Location = new System.Drawing.Point(0, 4)
            };
            window.Controls.Add(walm2024);

            // Run the application
            Application.Run(window);
        }
    }

    class Gif
    {
        private System.Drawing.Image[] frames;
        private int currentFrame = 0;

        public Gif(string filePath)
        {
            using (var gifImage = System.Drawing.Image.FromFile(filePath))
            {
                var frameCount = gifImage.GetFrameCount(new System.Drawing.Imaging.FrameDimension(gifImage.FrameDimensionsList[0]));
                frames = new System.Drawing.Image[frameCount];

                for (int i = 0; i < frameCount; i++)
                {
                    gifImage.SelectActiveFrame(new System.Drawing.Imaging.FrameDimension(gifImage.FrameDimensionsList[0]), i);
                    frames[i] = new Bitmap(gifImage);
                }
            }
        }

        public System.Drawing.Image GetNextFrame()
        {
            var frame = frames[currentFrame];
            currentFrame = (currentFrame + 1) % frames.Length;
            return frame;
        }
    }
}

