using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;

namespace DatamatrixCode
{
	public class BitmapsFormatter
	{
		private readonly List<BitmapModel> bitmapModels;

		public BitmapsFormatter(List<BitmapModel> bitmapModels)
		{
			this.bitmapModels = bitmapModels;
		}

		public List<BitmapModel> FormatBitmaps()
		{
			var formattedBitmapModels = new List<BitmapModel>();

			foreach(var bitmapModel in bitmapModels)
			{
				var formattedBitmap = new Bitmap(bitmapModel.bitmap.Width * 2, bitmapModel.bitmap.Height);

				using (Graphics g = Graphics.FromImage(formattedBitmap))
				{
					g.Clear(Color.Transparent);

					g.DrawImage(bitmapModel.bitmap, Point.Empty);

					var rightAlligned = new StringFormat();
					rightAlligned.Alignment = StringAlignment.Far;

					g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
					g.DrawString("PC: ", new Font("Tahoma", 14), Brushes.Black, new PointF((formattedBitmap.Width / 2) + 5, 5));
					g.DrawString(bitmapModel.ProductCode, new Font("Tahoma", 14), Brushes.Black, new PointF(formattedBitmap.Width - 5, 5), rightAlligned);

					g.DrawString("Ch.-B.: ", new Font("Tahoma", 14), Brushes.Black, new PointF((formattedBitmap.Width / 2) + 5, 55));
					g.DrawString(bitmapModel.Batch, new Font("Tahoma", 14), Brushes.Black, new PointF(formattedBitmap.Width - 5, 55), rightAlligned);

					g.DrawString("Verw.bis.: ", new Font("Tahoma", 14), Brushes.Black, new PointF((formattedBitmap.Width / 2) + 5, 105));
					g.DrawString(bitmapModel.Exp, new Font("Tahoma", 14), Brushes.Black, new PointF(formattedBitmap.Width - 5, 105), rightAlligned);

					g.DrawString("SN: ", new Font("Tahoma", 14), Brushes.Black, new PointF((formattedBitmap.Width / 2) + 5, 155));
					g.DrawString(bitmapModel.SerialNumber, new Font("Tahoma", 14), Brushes.Black, new PointF(formattedBitmap.Width - 5, 155), rightAlligned);

					// PC:
					// Ch.-B.:
					// Verw.bis:
					// SN:

					g.Flush();
				}

				formattedBitmapModels.Add(
					new BitmapModel()
					{
						bitmap = formattedBitmap,
						ProductCode = bitmapModel.ProductCode,
						Batch = bitmapModel.Batch,
						SerialNumber = bitmapModel.SerialNumber,
						Exp = bitmapModel.Exp,
						RunningNumber = bitmapModel.RunningNumber
					});
			}

			return formattedBitmapModels;
		}
	}
}
