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

        #region RendererXP
        /// <summary>
        /// Mimics Windows XP appearance.
        /// </summary>
        public class RendererXP : ImageListView.ImageListViewRenderer
        {
            /// <summary>
            /// Returns item size for the given view mode.
            /// </summary>
            /// <param name="view">The view mode for which the item measurement should be made.</param>
            public override Size MeasureItem(Manina.Windows.Forms.View view)
            {
                Size itemSize = new Size();

                // Reference text height
                int textHeight = ImageListView.Font.Height;

                if (view == Manina.Windows.Forms.View.Details)
                    return base.MeasureItem(view);
                else
                {
                    // Calculate item size
                    Size itemPadding = new Size(4, 4);
                    itemSize = ImageListView.ThumbnailSize + itemPadding + itemPadding;
                    itemSize.Height += textHeight + System.Math.Max(4, textHeight / 3) + itemPadding.Height; // textHeight / 3 = vertical space between thumbnail and text
                    return itemSize;
                }
            }
            /// <summary>
            /// Draws the specified item on the given graphics.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="item">The ImageListViewItem to draw.</param>
            /// <param name="state">The current view state of item.</param>
            /// <param name="bounds">The bounding rectangle of item in client coordinates.</param>
            public override void DrawItem(System.Drawing.Graphics g, ImageListViewItem item, ItemState state, System.Drawing.Rectangle bounds)
            {
                // Paint background
                using (Brush bItemBack = new SolidBrush(item.BackColor))
                {
                    g.FillRectangle(bItemBack, bounds);
                }

                if (ImageListView.View != Manina.Windows.Forms.View.Details)
                {
                    Size itemPadding = new Size(4, 4);

                    // Draw the image
                    Image img = item.ThumbnailImage;
                    if (img != null)
                    {
                        Rectangle border = new Rectangle(bounds.Location + itemPadding, ImageListView.ThumbnailSize);
                        Rectangle pos = Utility.GetSizedImageBounds(img, border);

                        //g.DrawImage(img, pos);
                        DrawThumbnail(g, item, img, pos);

                        // Draw image border
                        if (ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                        {
                            using (Pen pen = new Pen(SystemColors.Highlight, 3))
                            {
                                g.DrawRectangle(pen, border);
                            }
                        }
                        else if (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                        {
                            using (Pen pen = new Pen(SystemColors.GrayText, 3))
                            {
                                pen.Alignment = PenAlignment.Center;
                                g.DrawRectangle(pen, border);
                            }
                        }
                        else
                        {
                            using (Pen pGray128 = new Pen(Color.FromArgb(128, SystemColors.GrayText)))
                            {
                                g.DrawRectangle(pGray128, border);
                            }
                        }
                    }

                    // Draw item text
                    SizeF szt = TextRenderer.MeasureText(item.Text, ImageListView.Font);
                    RectangleF rt;
                    using (StringFormat sf = new StringFormat())
                    {
                        rt = new RectangleF(bounds.Left + itemPadding.Width, bounds.Top + 3 * itemPadding.Height + ImageListView.ThumbnailSize.Height, ImageListView.ThumbnailSize.Width, szt.Height);
                        sf.Alignment = StringAlignment.Center;
                        sf.FormatFlags = StringFormatFlags.NoWrap;
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Trimming = StringTrimming.EllipsisCharacter;
                        rt.Width += 1;
                        rt.Inflate(1, 2);
                        if (ImageListView.Focused && ((state & ItemState.Focused) != ItemState.None))
                            rt.Inflate(-1, -1);
                        if (ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                        {
                            g.FillRectangle(SystemBrushes.Highlight, rt);
                        }
                        else if (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                        {
                            g.FillRectangle(SystemBrushes.GrayText, rt);
                        }
                        if (((state & ItemState.Selected) != ItemState.None))
                        {
                            g.DrawString(item.Text, ImageListView.Font, SystemBrushes.HighlightText, rt, sf);
                        }
                        else
                        {
                            using (Brush bItemFore = new SolidBrush(item.ForeColor))
                            {
                                g.DrawString(item.Text, ImageListView.Font, bItemFore, rt, sf);
                            }
                        }
                    }

                    if (ImageListView.Focused && ((state & ItemState.Focused) != ItemState.None))
                    {
                        Rectangle fRect = Rectangle.Round(rt);
                        fRect.Inflate(1, 1);
                        ControlPaint.DrawFocusRectangle(g, fRect);
                    }
                }
                else
                {
                    if (ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                    {
                        g.FillRectangle(SystemBrushes.Highlight, bounds);
                    }
                    else if (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                    {
                        g.FillRectangle(SystemBrushes.GrayText, bounds);
                    }

                    Size offset = new Size(2, (bounds.Height - ImageListView.Font.Height) / 2);
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.FormatFlags = StringFormatFlags.NoWrap;
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Trimming = StringTrimming.EllipsisCharacter;
                        // Sub text
                        List<ImageListView.ImageListViewColumnHeader> uicolumns = ImageListView.Columns.GetDisplayedColumns();
                        RectangleF rt = new RectangleF(bounds.Left + offset.Width, bounds.Top + offset.Height, uicolumns[0].Width - 2 * offset.Width, bounds.Height - 2 * offset.Height);
                        foreach (ImageListView.ImageListViewColumnHeader column in uicolumns)
                        {
                            rt.Width = column.Width - 2 * offset.Width;
                            using (Brush bItemFore = new SolidBrush(item.ForeColor))
                            {
                                if ((state & ItemState.Selected) == ItemState.None)
                                    g.DrawString(item.GetSubItemText(column.Type), ImageListView.Font, bItemFore, rt, sf);
                                else
                                    g.DrawString(item.GetSubItemText(column.Type), ImageListView.Font, SystemBrushes.HighlightText, rt, sf);
                            }
                            rt.X += column.Width;
                        }
                    }

                    if (ImageListView.Focused && ((state & ItemState.Focused) != ItemState.None))
                        ControlPaint.DrawFocusRectangle(g, bounds);
                }
            }
            /// <summary>
            /// Draws the large preview image of the focused item in Gallery mode.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="item">The ImageListViewItem to draw.</param>
            /// <param name="image">The image to draw.</param>
            /// <param name="bounds">The bounding rectangle of the preview area.</param>
            public override void DrawGalleryImage(Graphics g, ImageListViewItem item, Image image, Rectangle bounds)
            {
                // Calculate image bounds
                Size itemMargin = MeasureItemMargin(mImageListView.View);
                Rectangle pos = Utility.GetSizedImageBounds(image, new Rectangle(bounds.Location + itemMargin, bounds.Size - itemMargin - itemMargin));
                // Draw image
                g.DrawImage(image, pos);

                // Draw image border
                if (Math.Min(pos.Width, pos.Height) > 32)
                {
                    using (Pen pBorder = new Pen(Color.Black))
                    {
                        g.DrawRectangle(pBorder, pos);
                    }
                }
            }
        }
        #endregion

    }
}
