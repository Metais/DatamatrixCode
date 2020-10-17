using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace DatamatrixCode
{
	public class BitmapsFormatter
	{
		private readonly List<BitmapModel> bitmaps;

		public BitmapsFormatter(List<BitmapModel> bitmaps)
		{
			this.bitmaps = bitmaps;
		}

		public List<BitmapModel> FormatBitmaps()
		{
			return this.bitmaps;
		}
	}
}
