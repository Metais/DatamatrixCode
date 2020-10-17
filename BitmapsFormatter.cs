using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DatamatrixCode
{
	public class BitmapsFormatter
	{
		private readonly List<Bitmap> bitmaps;

		public BitmapsFormatter(List<Bitmap> bitmaps)
		{
			this.bitmaps = bitmaps;
		}

		public Bitmap FormatBitmaps(bool isHorizontal, bool textInSamePath, int pixelMargin = 10)
		{
			var bitmapSize = this.bitmaps.First().Size;
			Bitmap finalBitmap = null;
			/*
			 * Width = number of bitmaps * width of bitmaps, add 50 to the end to make room for label text for last bitmap
			 * Height = Half the height of a bitmap due to the margin, only leave the important code part, and add some margin for printer
			 */
			if (isHorizontal)
			{
				finalBitmap = new Bitmap(bitmapSize.Width * this.bitmaps.Count, (bitmapSize.Height / 2) + pixelMargin);
			}
			/*
			 * Width = Half the width of a bitmap due to the margin, only leave the important code part, and add some margin for printer
			 * Height = number of bitmaps * height of bitmaps, add 50 to the end to make room for label text for last bitmap
			 */
			else
			{
				finalBitmap = new Bitmap((bitmapSize.Width / 2) + pixelMargin, bitmapSize.Height * this.bitmaps.Count);
			}
			

			using (Graphics g = Graphics.FromImage(finalBitmap))
			{
				if(isHorizontal)
				{
					foreach (var bitmap in this.bitmaps)
					{
						if(this.bitmaps.Last() == bitmap)
						{
							var newBitmap = new Bitmap(bitmap, new Size(bitmap.Width + 200, bitmap.Height));
							g.DrawImage(newBitmap,
							new Point(
								(bitmap.Width / 4) + bitmap.Width * this.bitmaps.IndexOf(bitmap),
							-((this.bitmaps.First().Height / 4) - (pixelMargin / 2))));
						}
						else
						{
							g.DrawImage(bitmap,
							new Point(
								(bitmap.Width / 4) + bitmap.Width * this.bitmaps.IndexOf(bitmap),
							-((this.bitmaps.First().Height / 4) - (pixelMargin / 2))));
						}
					}
				}
				else
				{
					foreach(var bitmap in this.bitmaps)
					{
						if (this.bitmaps.Last() == bitmap)
						{
							var newBitmap = new Bitmap(bitmap, new Size(bitmap.Width, bitmap.Height + 200));
							g.DrawImage(newBitmap,
							new Point(
								(bitmap.Width / 4) + bitmap.Width * this.bitmaps.IndexOf(bitmap),
							-((this.bitmaps.First().Height / 4) - (pixelMargin / 2))));
						}
						else
						{
							g.DrawImage(bitmap,
							new Point(
								-((this.bitmaps.First().Width / 4) - (pixelMargin / 2)),
							(bitmap.Height / 4) + bitmap.Height * this.bitmaps.IndexOf(bitmap)));
						}
					}
				}
			}

			return finalBitmap;
		}
	}
}
