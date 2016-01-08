﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NAPS2.Scan.Images.Transforms
{
    [Serializable]
    public class CropTransform : Transform
    {
        public CropTransform()
        {
        }

        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public override Bitmap Perform(Bitmap bitmap)
        {
            var result = new Bitmap(Math.Max(bitmap.Width - Left - Right, 1), Math.Max(bitmap.Height - Top - Bottom, 1));
            result.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);
            using (var g = Graphics.FromImage(result))
            {
                g.DrawImage(bitmap, new Rectangle(-Left, -Top, bitmap.Width, bitmap.Height));
            }
            OptimizePixelFormat(bitmap, ref result);
            bitmap.Dispose();
            return result;
        }

        public override bool CanSimplify(Transform other)
        {
            return other is CropTransform;
        }

        public override Transform Simplify(Transform other)
        {
            var other2 = (CropTransform)other;
            return new CropTransform
            {
                Left = Left + other2.Left,
                Right = Right + other2.Right,
                Top = Top + other2.Top,
                Bottom = Bottom + other2.Bottom
            };
        }

        public override bool IsNull
        {
            get { return Left == 0 && Right == 0 && Top == 0 && Bottom == 0; }
        }
    }
}
