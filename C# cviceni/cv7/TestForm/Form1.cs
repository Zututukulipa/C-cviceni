using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".pcx";
            openFileDialog.Filter = "Image data (.pcx)|*.pcx";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PCX_Parser parser = new PCX_Parser();
                Bitmap image = parser.parsePCX(openFileDialog.FileName);

                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}

//using System;
//using System.Drawing;
//using System.IO;
//using System.Windows.Forms;

//namespace TestForm
//{
//    public partial class Form1 : Form
//    {
//        public Form1()
//        {
//            InitializeComponent();
//        }

//        private void btOpen_Click(object sender, EventArgs e)
//        {
//            OpenFileDialog openFileDialog = new OpenFileDialog();
//            openFileDialog.DefaultExt = ".pcx";
//            openFileDialog.Filter = "Image data (.pcx)|*.pcx";

//            if (openFileDialog.ShowDialog() == DialogResult.OK)
//            {
//                using (BinaryReader br = new BinaryReader(File.OpenRead(openFileDialog.FileName)))
//                {

//                    br.BaseStream.Seek(3, SeekOrigin.Begin);
//                    byte bitsPerPixel = br.ReadByte();

//                    ushort xPixelsStart = br.ReadUInt16();
//                    ushort yPixelsStart = br.ReadUInt16();
//                    ushort xPixelsEnd = br.ReadUInt16();
//                    ushort yPixelsEnd = br.ReadUInt16();

//                    int imageWidth = xPixelsEnd - xPixelsStart + 1;
//                    int imageHeight = yPixelsEnd - yPixelsStart + 1;

//                    br.BaseStream.Seek(4, SeekOrigin.Current);
//                    byte[] egaPalette = new byte[48];
//                    br.ReadBytes(egaPalette.Length);

//                    br.BaseStream.Seek(1, SeekOrigin.Current);

//                    byte numBitPlanes = br.ReadByte();
//                    ushort bytesPerLine = br.ReadUInt16();

//                    br.BaseStream.Seek(60, SeekOrigin.Current);

//                    int scanLineLength = numBitPlanes * bytesPerLine;


//                    int readBytes = 128;
//                    byte[,] buffer = new byte[imageHeight, scanLineLength];

//                    for (int l = 0; l < imageHeight; l++)
//                    {
//                        int i = 0;
//                        do
//                        {
//                            byte b = br.ReadByte();
//                            readBytes++;

//                            byte runByte = b;
//                            int runLength = 1;

//                            if ((b & 0xC0) == 0xC0)
//                            {
//                                runLength = b & 0x3F;
//                                runByte = br.ReadByte();
//                                readBytes++;
//                            }

//                            for (int r = 0; r < runLength; r++)
//                                buffer[l, i++] = runByte;

//                        } while (i < scanLineLength);
//                    }

//                    Color[] palette = null;

//                    if (!(bitsPerPixel == 8 && numBitPlanes == 3))
//                    {
//                        long paletteOffset = br.BaseStream.Position - readBytes;

//                        if (paletteOffset > 769)
//                        {
//                            br.BaseStream.Seek((int)(paletteOffset - 769), SeekOrigin.End);
//                        }

//                        palette = new Color[256];
//                        for (int i = 0; i < 768; i += 3)
//                        {
//                            int r = br.ReadByte() & 0xFF;
//                            int g = br.ReadByte() & 0xFF;
//                            int b = br.ReadByte() & 0xFF;

//                            palette[i / 3] = Color.FromArgb(r, g, b);
//                        }
//                    }

//                    Bitmap pcx = new Bitmap(imageWidth, imageHeight);

//                    for (int y = 0; y < imageHeight; y++)
//                    {
//                        for (int x = 0; x < imageWidth; x++)
//                        {
//                            if (bitsPerPixel == 8 && numBitPlanes == 1)
//                            {
//                                int colorIndex = buffer[y, x] & 0xFF;
//                                pcx.SetPixel(x, y, palette[colorIndex]);
//                            }
//                            else if (bitsPerPixel == 8 && numBitPlanes == 3)
//                            {
//                                int r = buffer[y, 0 + x] & 0xFF;
//                                int g = buffer[y, bytesPerLine + x] & 0xFF;
//                                int b = buffer[y, bytesPerLine * 2 + x] & 0xFF;
//                                pcx.SetPixel(x, y, Color.FromArgb(r, g, b));
//                            }
//                        }
//                    }

//                    pictureBox1.Image = pcx;
//                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
//                }
//            }
//        }
//    }
//}