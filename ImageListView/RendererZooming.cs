using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Manina.Windows.Forms
{
    public static partial class ImageListViewRenderers
    {
        #region RendererZooming
        /// <summary>
        /// Zooms items on mouse over.
        /// </summary>
        public class RendererZooming : ImageListView.ImageListViewRenderer
        {
            private float mZoomRatio;

            /// <summary>
            /// Gets or sets the relative zoom ratio.
            /// </summary>
            public float ZoomRatio
            {
                get
                {
                    return mZoomRatio;
                }
                set
                {
                    mZoomRatio = value;
                    if (mZoomRatio < 0.0f) mZoomRatio = 0.0f;
                }
            }

            /// <summary>
            /// Initializes a new instance of the RendererZooming class.
            /// </summary>
            public RendererZooming()
                : this(0.5f)
            {
                ;
            }

            /// <summary>
            /// Initializes a new instance of the RendererZooming class.
            /// </summary>
            /// <param name="zoomRatio">Relative zoom ratio.</param>
            public RendererZooming(float zoomRatio)
            {
                if (zoomRatio < 0.0f) zoomRatio = 0.0f;
                mZoomRatio = zoomRatio;
            }

            /// <summary>
            /// Initializes the System.Drawing.Graphics used to draw
            /// control elements.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            public override void InitializeGraphics(System.Drawing.Graphics g)
            {
                base.InitializeGraphics(g);

                Clip = false;

                ItemDrawOrder = ItemDrawOrder.NormalSelectedHovered;
            }
            /// <summary>
            /// Returns item size for the given view mode.
            /// </summary>
            /// <param name="view">The view mode for which the item measurement should be made.</param>
            /// <returns></returns>
            public override Size MeasureItem(Manina.Windows.Forms.View view)
            {
                if (view == Manina.Windows.Forms.View.Thumbnails)
                    return ImageListView.ThumbnailSize + new Size(8, 8);
                else
                    return base.MeasureItem(view);
            }
            /// <summary>
            /// Draws the specified item on the given graphics.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="item">The ImageListViewItem to draw.</param>
            /// <param name="state">The current view state of item.</param>
            /// <param name="bounds">The bounding rectangle of item in client coordinates.</param>
            public override void DrawItem(Graphics g, ImageListViewItem item, ItemState state, Rectangle bounds)
            {
                if (ImageListView.View == Manina.Windows.Forms.View.Thumbnails)
                {
                    // Zoom on mouse over
                    if ((state & ItemState.Hovered) != ItemState.None)
                    {
                        bounds.Inflate((int)(bounds.Width * mZoomRatio), (int)(bounds.Height * mZoomRatio));
                        if (bounds.Bottom > ItemAreaBounds.Bottom)
                            bounds.Y = ItemAreaBounds.Bottom - bounds.Height;
                        if (bounds.Top < ItemAreaBounds.Top)
                            bounds.Y = ItemAreaBounds.Top;
                        if (bounds.Right > ItemAreaBounds.Right)
                            bounds.X = ItemAreaBounds.Right - bounds.Width;
                        if (bounds.Left < ItemAreaBounds.Left)
                            bounds.X = ItemAreaBounds.Left;
                    }

                    // Get item image
                    Image img = null;
                    
                    if ((state & ItemState.Hovered) != ItemState.None)
                        img = GetImageAsync(item, new Size(bounds.Width - 8, bounds.Height - 8));

                    if (img == null) img = item.ThumbnailImage;

                    // Calculate image bounds
                    Rectangle pos = Utility.GetSizedImageBounds(img, Rectangle.Inflate(bounds, -4, -4));
                    int imageWidth = pos.Width;
                    int imageHeight = pos.Height;
                    int imageX = pos.X;
                    int imageY = pos.Y;

                    // Allocate space for item text
                    if ((state & ItemState.Hovered) != ItemState.None &&
                        (bounds.Height - imageHeight) / 2 < ImageListView.Font.Height + 8)
                    {
                        int delta = (ImageListView.Font.Height + 8) - (bounds.Height - imageHeight) / 2;
                        bounds.Height += 2 * delta;
                        imageY += delta;
                    }

                    // Paint background
                    using (Brush bBack = new SolidBrush(ImageListView.BackColor))
                    {
                        Utility.FillRoundedRectangle(g, bBack, bounds, 5);
                    }
                    using (Brush bItemBack = new SolidBrush(item.BackColor))
                    {
                        Utility.FillRoundedRectangle(g, bItemBack, bounds, 5);
                    }
                    if ((ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None)) ||
                        (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None) && ((state & ItemState.Hovered) != ItemState.None)))
                    {
                        using (Brush bSelected = new LinearGradientBrush(bounds, Color.FromArgb(16, SystemColors.Highlight), Color.FromArgb(64, SystemColors.Highlight), LinearGradientMode.Vertical))
                        {
                            Utility.FillRoundedRectangle(g, bSelected, bounds, 5);
                        }
                    }
                    else if (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                    {
                        using (Brush bGray64 = new LinearGradientBrush(bounds, Color.FromArgb(16, SystemColors.GrayText), Color.FromArgb(64, SystemColors.GrayText), LinearGradientMode.Vertical))
                        {
                            Utility.FillRoundedRectangle(g, bGray64, bounds, 5);
                        }
                    }
                    if (((state & ItemState.Hovered) != ItemState.None))
                    {
                        using (Brush bHovered = new LinearGradientBrush(bounds, Color.FromArgb(8, SystemColors.Highlight), Color.FromArgb(32, SystemColors.Highlight), LinearGradientMode.Vertical))
                        {
                            Utility.FillRoundedRectangle(g, bHovered, bounds, 5);
                        }
                    }

                    // Draw the image
                    //g.DrawImage(img, pos);
                    DrawThumbnai(g, item, img, pos);

                    // Draw image border
                    if (Math.Min(imageWidth, imageHeight) > 32)
                    {
                        using (Pen pGray128 = new Pen(Color.FromArgb(128, Color.Gray)))
                        {
                            g.DrawRectangle(pGray128, imageX, imageY, imageWidth, imageHeight);
                        }
                        if (System.Math.Min(imageWidth, imageHeight) > 32)
                        {
                            using (Pen pWhite128 = new Pen(Color.FromArgb(128, Color.White)))
                            {
                                g.DrawRectangle(pWhite128, imageX + 1, imageY + 1, imageWidth - 2, imageHeight - 2);
                            }
                        }
                    }

                    // Draw item text
                    if ((state & ItemState.Hovered) != ItemState.None)
                    {
                        RectangleF rt;
                        using (StringFormat sf = new StringFormat())
                        {
                            rt = new RectangleF(bounds.Left + 4, bounds.Top + 4, bounds.Width - 8, (bounds.Height - imageHeight) / 2 - 8);
                            sf.Alignment = StringAlignment.Center;
                            sf.FormatFlags = StringFormatFlags.NoWrap;
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Trimming = StringTrimming.EllipsisCharacter;
                            using (Brush bItemFore = new SolidBrush(item.ForeColor))
                            {
                                g.DrawString(item.Text, ImageListView.Font, bItemFore, rt, sf);
                            }
                            rt.Y = bounds.Bottom - (bounds.Height - imageHeight) / 2 + 4;
                            string details = "";
                            if (item.Dimensions != Size.Empty)
                                details += item.GetSubItemText(ColumnType.MediaDimensions) + " pixels ";
                            if (item.FileSize != 0)
                                details += item.GetSubItemText(ColumnType.FileSize);
                            using (Brush bGrayText = new SolidBrush(Color.Gray))
                            {
                                g.DrawString(details, ImageListView.Font, bGrayText, rt, sf);
                            }
                        }
                    }

                    // Item border
                    using (Pen pWhite128 = new Pen(Color.FromArgb(128, Color.White)))
                    {
                        Utility.DrawRoundedRectangle(g, pWhite128, bounds.Left + 1, bounds.Top + 1, bounds.Width - 3, bounds.Height - 3, 4);
                    }
                    if (ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                    {
                        using (Pen pHighlight128 = new Pen(Color.FromArgb(128, SystemColors.Highlight)))
                        {
                            Utility.DrawRoundedRectangle(g, pHighlight128, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1, 4);
                        }
                    }
                    else if (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                    {
                        using (Pen pGray128 = new Pen(Color.FromArgb(128, SystemColors.GrayText)))
                        {
                            Utility.DrawRoundedRectangle(g, pGray128, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1, 4);
                        }
                    }
                    else if ((state & ItemState.Selected) == ItemState.None)
                    {
                        using (Pen pGray64 = new Pen(Color.FromArgb(64, SystemColors.GrayText)))
                        {
                            Utility.DrawRoundedRectangle(g, pGray64, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1, 4);
                        }
                    }

                    if (ImageListView.Focused && ((state & ItemState.Hovered) != ItemState.None))
                    {
                        using (Pen pHighlight64 = new Pen(Color.FromArgb(64, SystemColors.Highlight)))
                        {
                            Utility.DrawRoundedRectangle(g, pHighlight64, bounds.Left, bounds.Top, bounds.Width - 1, bounds.Height - 1, 4);
                        }
                    }
                }
                else
                    base.DrawItem(g, item, state, bounds);
            }
        }
        #endregion
    }
}
