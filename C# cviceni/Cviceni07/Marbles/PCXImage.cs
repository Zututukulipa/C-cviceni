using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Marbles
{
    public class PCXImage
    {
        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }
        public Bitmap Bitmap { get; private set; }
        /// <summary>
        /// Identifier 10 indicates PCX file
        /// </summary>
        private byte Identifier { get; set; }  /* PCX Id Number (Always 0x0A) */
        /// <summary>
        /// Value
        /// <para>0 - Version 2.5 with fixed EGA palette information</para>
        ///
        /// <para>2 - Version 2.8 with modifiable EGA palette information</para>
        ///
        /// <para>3 - Version 2.8 without palette information</para>
        ///
        /// <para>4 - PC Paintbrush for Windows</para>
        ///
        /// <para>5 - Version 3.0 of PC Paintbrush, PC Paintbrush Plus,
        ///     PC Paintbrush Plus for Windows, Publisher's Paintbrush, 
        ///     and all 24 - bit image files</para>
        /// </summary>
        private byte Version { get; set; }  /* Version Number */
        /// <summary>
        /// The only encoding algorithm currently supported by the PCX specification is a
        /// simple byte-wise run-length encoding(RLE) scheme indicated by a value of 1 in this byte.
        /// <para>[...] however, always contain encoded image data, and currently the only valid value for the encoding field is 1.</para>
        /// </summary>
        private byte Encoding { get; set; }  /* Encoding Format */
        /// <summary>
        /// The possible values are 1, 2, 4, and 8 for 2-, 4-, 16-, and 256-color images.
        /// </summary>
        private byte BitsPerPixel { get; set; }  /* Bits per Pixel */
        /// <summary>
        /// The upper-left corner of the screen is considered to be at location [0,0]
        /// and any PCX image with an XStart of 0 will start displaying at this location
        /// </summary>
        private ushort XStart { get; set; }    /* Left of image */
        /// <summary>
        /// The upper-left corner of the screen is considered to be at location [0,0]
        /// and any PCX image with an YStart of 0 will start displaying at this location
        /// </summary>
        private ushort YStart { get; set; }    /* Top of Image */
        /// <summary>
        /// The largest XEnd PCX image that can be stored is 65,535 pixels in size.
        /// </summary>
        private ushort XEnd { get; set; }    /* Right of Image*/
        /// <summary>
        /// The largest YEnd PCX image that can be stored is 65,535 pixels in size.
        /// </summary>
        private ushort YEnd { get; set; }     /* Bottom of image */
        /// <summary>
        ///  HorzRes is the horizontal size of the stored image in pixels per line or dots per inch (DPI).
        ///  <para>[...]<strong>However, this value is not used when decoding image data. </strong></para>
        /// </summary>
        private ushort HorzRes { get; set; }    /* Horizontal Resolution */
        /// <summary>
        ///  VertRes is the vertical size of the stored image in pixels per line or dots per inch (DPI).
        ///  <para>[...]<strong>However, this values is not used when decoding image data. </strong></para>
        /// </summary>
        private ushort VertRes { get; set; }    /* Vertical Resolution */

        /// <summary>
        ///  Palette is a 48-byte array of 8-bit values that make up a 16-color EGA color palette. 
        ///  <para>[...] Subsequent versions have allowed the use of a modifiable palette enabling a PCX image
        ///  file writer to choose which 16 (or fewer) of the 64 colors available to the EGA to use.</para>
        /// </summary>
        private byte[] PaletteArr = new byte[48]; /* 16-Color EGA Palette */
        /// <summary>
        /// Wrapper to Palette Array hiding undeneath it
        /// </summary>
        public IReadOnlyCollection<byte> Palette
        {
            get
            {
                return PaletteArr;
            }
        }
        /// <summary>
        /// Reserved1 is not currently used and should have a value of 00h.
        /// </summary>
        private byte Reserved1 { get; set; }
        /// <summary>
        /// NumBitPlanes is the number of color planes that contains the image data. The number of planes is usually 1, 3, or 4 and 
        /// is used in conjunction with the BitsPerPixel value to determine the proper video mode in which to display the image.
        /// </summary>
        private byte NumBitPlanes { get; set; }  /* Number of Bit Planes */
        private ushort BytesPerLine { get; set; }    /* Bytes per Scan-line */
        private ushort PaletteType { get; set; }    /* Palette Type */
        private ushort HorzScreenSize { get; set; }    /* Horizontal Screen Size */
        private ushort VertScreenSize { get; set; }    /* Vertical Screen Size */
        private byte[] Reserved2 { get; set; } = new byte[54];  /* Reserved (Always 0) */
        private const int HeaderSize = 128;
        
        /// <summary>
        /// Returns Width of image in pixels 
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="xEnd"></param>
        /// <returns></returns>
        private int GetImageWidth()
        {
            return XEnd - XStart + 1;
        }

        /// <summary>
        /// Returns Height of image in pixels
        /// </summary>
        /// <param name="yStart"></param>
        /// <param name="yEnd"></param>
        /// <returns></returns>
        private int GetImageHeight()
        {
            return YEnd - YStart + 1;
        }

        /// <summary>
        /// Loads amount of bytes needed
        /// </summary>
        /// <param name="br"></param>
        /// <param name="ScanLineLength"></param>
        /// <param name="ImageHeight"></param>
        /// <param name="ReadBytes"></param>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private int LoadRows(BinaryReader br, int ScanLineLength, byte[,] Buffer)
        {
            byte b;
            byte runByte;
            int runLength;
            int bytesRead = HeaderSize;

            /* Any PCX image may contain extra bytes of padding at the end of each scan line
            or extra scan lines added to the bottom of the image.
            To prevent this extra data from becoming visible,
            only the image data within the picture dimension window coordinates is displayed.*/
            for (int line = 0; line < ImageHeight; ++line)
            {
                int i = 0;
                do
                {
                    b = br.ReadByte();
                    bytesRead++;

                    runByte = b;
                    runLength = 1;

                    if ((b & 0xC0) == 0xC0)
                    {
                        runLength = b & 0x3F;
                        runByte = br.ReadByte();
                        bytesRead++;
                    }

                    for (int row = 0; row < runLength; ++row) { 
                        Buffer[line, i++] = runByte;
                    }

                } while (i < ScanLineLength);
            }

            return bytesRead;
        }
        internal PCXImage() { }
        public PCXImage(string path)
        {
            LoadPCX(path);
        }
        public void LoadPCX(string path)
        {
            using BinaryReader br = ReadImageHeader(path);
            ImageWidth = GetImageWidth();
            ImageHeight = GetImageHeight();
            int scanLineLength = GetLineLength();
            int linePaddingSize = GetLinePadding();
            //TODO
            //Possible bug in buffer, buffer values are 0 while trying to write into bitmap
            //Maybe thats why black and white values?
            byte[,] buffer = new byte[ImageHeight, scanLineLength];
            int paletteByteValue = LoadRows(br, scanLineLength, buffer);
            Color[] palette = CalculateImageColours(br, paletteByteValue);
            br.BaseStream.Close();
            br.Close();
            Bitmap = CreateImage(buffer, palette);
        }

        private Bitmap CreateImage(byte[,] Buffer, Color[] palette)
        {
            Bitmap image = new Bitmap(ImageWidth, ImageHeight);

            for (int y = 0; y < ImageHeight; y++)
            {
                for (int x = 0; x < ImageWidth; x++)
                {
                    if (BitsPerPixel == 8 && NumBitPlanes == 1)
                    {
                        int colorIndex = Buffer[y, x] * 3;
                        image.SetPixel(x, y, palette[colorIndex]);
                    }
                    else if (BitsPerPixel == 8 && NumBitPlanes == 3)
                    {
                        int r = Buffer[y, 0 + x] & 0xFF;
                        int g = Buffer[y, BytesPerLine + 1] & 0xFF;
                        int b = Buffer[y, BytesPerLine + 2] & 0xFF;
                        image.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }

            return image;
        }
        private Color[] CalculateImageColours(BinaryReader br, int PaletteByteValue)
        {
            Color[] palette = null;

            if (!(BitsPerPixel == 8 && NumBitPlanes == 3))
            {
                long paletteOffset = br.BaseStream.Position - PaletteByteValue;

                if (paletteOffset > 769)
                {
                    br.BaseStream.Seek((int)(paletteOffset - 769), SeekOrigin.End);
                }

                palette = new Color[256];
                for (int i = 0; i < 768; i += 3)
                {
                    int red = br.ReadByte() & 0xFF;
                    int green = br.ReadByte() & 0xFF;
                    int blue = br.ReadByte() & 0xFF;

                    palette[i/3] = Color.FromArgb(red, green, blue);
                }
            }

            return palette;
        }
        /// <summary>
        /// Calculates Padding in image.
        /// </summary>
        /// <returns></returns>
        private int GetLinePadding()
        {
            return ((BytesPerLine * NumBitPlanes) * (8 / BitsPerPixel)) - ((XEnd - XStart) + 1);
        }

        /// <summary>
        /// Returns Length of Line
        /// </summary>
        /// <returns></returns>
        private int GetLineLength()
        {
            return NumBitPlanes * BytesPerLine;
        }

        private BinaryReader ReadImageHeader(string path)
        {
            StringBuilder builder = new StringBuilder();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            if (!ValidityCheck(br))
                throw new InvalidDataException("PCX file is not valid!");

            //TODO
            //Dimensions of the result are so far too narrow
            //Maybe reading of dimensions is not done right
            //Possible skipped bit?
            ReadDimensions(br);
            ReadPalette(br);
            ReadResolution(br);
            Reserved1 = br.ReadByte();
            NumBitPlanes = br.ReadByte();
            BytesPerLine = br.ReadUInt16();
            PaletteType = br.ReadUInt16();
            ReadScreenSize(br);
            ReadReserved2(br);
            return br;
        }

        /// <summary>
        /// Reads 54 reserved bits from binary stream.
        /// </summary>
        /// <param name="br"></param>
        private void ReadReserved2(BinaryReader br)
        {
            for (int j = 0; j < 54; j++)
            {
                Reserved2[j] = br.ReadByte();
            }
        }

        /// <summary>
        /// Reads screen size from binary stream.
        /// </summary>
        /// <param name="br"></param>
        private void ReadScreenSize(BinaryReader br)
        {
            HorzScreenSize = br.ReadUInt16();
            VertScreenSize = br.ReadUInt16();
        }

        /// <summary>
        /// Checks if crucial header values are valid from binary stream.
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        private bool ValidityCheck(BinaryReader br)
        {
            Identifier = br.ReadByte();
            if (Identifier != 10)
                return false;

            Version = br.ReadByte();
            Encoding = br.ReadByte();
            if (Encoding != 1)
                return false;

            BitsPerPixel = br.ReadByte();
            if (BitsPerPixel != 1 && BitsPerPixel != 2 && BitsPerPixel != 4 && BitsPerPixel != 8)
                return false;


            return true;
        }

        /// <summary>
        /// Reads X and Y dimensions from binary stream
        /// </summary>
        /// <param name="br"></param>
        private void ReadDimensions(BinaryReader br)
        {
            XStart = br.ReadUInt16();
            YStart = br.ReadUInt16();

            XEnd = br.ReadUInt16();
            YEnd = br.ReadUInt16();
        }

        /// <summary>
        /// Reads DPI values from binary stream
        /// </summary>
        /// <param name="br"></param>
        private void ReadResolution(BinaryReader br)
        {
            HorzRes = br.ReadUInt16();
            VertRes = br.ReadUInt16();
        }

        private void ReadPalette(BinaryReader br)
        {
            for (int j = 0; j < 48; j++)
            {
                PaletteArr[j] = br.ReadByte();
            }
        }
    }
}
