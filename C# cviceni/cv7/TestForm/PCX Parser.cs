using System.Drawing;
using System.IO;

namespace TestForm
{
    class PCX_Parser
    {
        private ushort xPixelsStart;
        private ushort yPixelsStart;
        private ushort xPixelsEnd;
        private ushort yPixelsEnd;

        private int imageWidth;
        private int imageHeight;

        private byte numBitPlanes;
        private ushort bytesPerLine;

        public Bitmap parsePCX(string fileName)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fileName)))
            {
                byte bitsPerPixel = loadHeader(br);

                imageWidth = calculateImageWidth();
                imageHeight = calculateImageHeight();

                int scanLineLength = calculateLineLength();
                
                int readBytes = 128;  // Velikost hlavičky
                
                byte[,] buffer = new byte[imageHeight, scanLineLength];

                readBytes = loadRows(br, scanLineLength, readBytes, buffer);

                Color[] palette = calculateImageColors(br, bitsPerPixel, readBytes);

                return createImage(bitsPerPixel, buffer, palette);
            }
        }

        private int calculateImageHeight()
        {
            return yPixelsEnd - yPixelsStart + 1;
        }

        private int calculateImageWidth()
        {
            return xPixelsEnd - xPixelsStart + 1;
        }

        private int calculateLineLength()
        {
            return numBitPlanes * bytesPerLine;
        }

        private Color[] calculateImageColors(BinaryReader br, byte bitsPerPixel, int readBytes)
        {
            Color[] palette = null;

            if (!(bitsPerPixel == 8 && numBitPlanes == 3))
            {
                long paletteOffset = br.BaseStream.Position - readBytes;

                if (paletteOffset > 769)
                {
                    br.BaseStream.Seek((int)(paletteOffset - 769), SeekOrigin.End);
                }

                palette = new Color[256];
                for (int i = 0; i < 768; i += 3)
                {
                    int r = br.ReadByte() & 0xFF;
                    int g = br.ReadByte() & 0xFF;
                    int b = br.ReadByte() & 0xFF;

                    palette[i / 3] = Color.FromArgb(r, g, b);
                }
            }

            return palette;
        }

        private int loadRows(BinaryReader br, int scanLineLength, int readBytes, byte[,] buffer)
        {
            byte bajt, runByte;
            int runLength;

            for (int line = 0; line < imageHeight; line++)
            {
                int i = 0;
                do
                {
                    bajt = br.ReadByte();
                    readBytes++;

                    runByte = bajt;
                    runLength = 1;

                    if ((bajt & 0xC0) == 0xC0)
                    {
                        runLength = bajt & 0x3F;
                        runByte = br.ReadByte();
                        readBytes++;
                    }

                    for (int r = 0; r < runLength; r++)
                        buffer[line, i++] = runByte;

                } while (i < scanLineLength);
            }

            return readBytes;
        }

        private byte loadHeader(BinaryReader br)
        {
            br.BaseStream.Seek(3, SeekOrigin.Begin);
            byte bitsPerPixel = br.ReadByte();

            xPixelsStart = br.ReadUInt16();
            yPixelsStart = br.ReadUInt16();
            xPixelsEnd = br.ReadUInt16();
            yPixelsEnd = br.ReadUInt16();

            br.BaseStream.Seek(53, SeekOrigin.Current);

            numBitPlanes = br.ReadByte();
            bytesPerLine = br.ReadUInt16();

            br.BaseStream.Seek(60, SeekOrigin.Current);

            return bitsPerPixel;
        }

        private Bitmap createImage(byte bitsPerPixel, byte[,] buffer, Color[] palette)
        {
            Bitmap image = new Bitmap(imageWidth, imageHeight);

            for (int y = 0; y < imageHeight; y++)
            {
                for (int x = 0; x < imageWidth; x++)
                {
                    if (bitsPerPixel == 8 && numBitPlanes == 1)
                    {
                        int colorIndex = buffer[y, x] & 0xFF;
                        image.SetPixel(x, y, palette[colorIndex]);
                    }
                    else if (bitsPerPixel == 8 && numBitPlanes == 3)
                    {
                        int r = buffer[y, 0 + x] & 0xFF;
                        int g = buffer[y, bytesPerLine + x] & 0xFF;
                        int b = buffer[y, bytesPerLine * 2 + x] & 0xFF;
                        image.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }

            return image;
        }
    }
}
