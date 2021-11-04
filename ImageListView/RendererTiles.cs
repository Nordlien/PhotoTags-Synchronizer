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

        #region RendererTiles
        /// <summary>
        /// Displays items with large tiles.
        /// </summary>
        public class RendererTiles : ImageListView.ImageListViewRenderer
        {
            private Font mCaptionFont;
            private int mTileWidth;
            private int mTextHeight;

            /// <summary>
            /// Gets or sets the width of the tile.
            /// </summary>
            public int TileWidth { get { return mTileWidth; } set { mTileWidth = value; } }

            private Font CaptionFont
            {
                get
                {
                    if (mCaptionFont == null)
                        mCaptionFont = new Font(ImageListView.Font, FontStyle.Bold);
                    return mCaptionFont;
                }
            }

            /// <summary>
            /// Initializes a new instance of the RendererTiles class.
            /// </summary>
            public RendererTiles()
                : this(150)
            {
                ;
            }

            /// <summary>
            /// Initializes a new instance of the RendererTiles class.
            /// </summary>
            /// <param name="tileWidth">Width of tiles in pixels.</param>
            public RendererTiles(int tileWidth)
            {
                mTileWidth = tileWidth;
            }

            /// <summary>
            /// Releases managed resources.
            /// </summary>
            public override void Dispose()
            {
                if (mCaptionFont != null)
                    mCaptionFont.Dispose();

                base.Dispose();
            }
            /// <summary>
            /// Returns item size for the given view mode.
            /// </summary>
            /// <param name="view">The view mode for which the item measurement should be made.</param>
            public override Size MeasureItem(Manina.Windows.Forms.View view)
            {
                if (view == Manina.Windows.Forms.View.Thumbnails)
                {
                    Size itemSize = new Size();
                    mTextHeight = (int)(5.8f * (float)CaptionFont.Height);

                    // Calculate item size
                    Size itemPadding = new Size(4, 4);
                    itemSize.Width = ImageListView.ThumbnailSize.Width + 4 * itemPadding.Width + mTileWidth;
                    itemSize.Height = Math.Max(mTextHeight, ImageListView.ThumbnailSize.Height) + 2 * itemPadding.Height;
                    return itemSize;
                }
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
                    Size itemPadding = new Size(4, 4);

                    // Paint background
                    using (Brush bItemBack = new SolidBrush(item.BackColor))
                    {
                        g.FillRectangle(bItemBack, bounds);
                    }
                    if ((ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None)) ||
                        (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None) && ((state & ItemState.Hovered) != ItemState.None)))
                    {
                        using (Brush bSelected = new LinearGradientBrush(bounds, Color.FromArgb(16, SystemColors.Highlight), Color.FromArgb(64, SystemColors.Highlight), LinearGradientMode.Vertical))
                        {
                            Utility.FillRoundedRectangle(g, bSelected, bounds, 4);
                        }
                    }
                    else if (!ImageListView.Focused && ((state & ItemState.Selected) != ItemState.None))
                    {
                        using (Brush bGray64 = new LinearGradientBrush(bounds, Color.FromArgb(16, SystemColors.GrayText), Color.FromArgb(64, SystemColors.GrayText), LinearGradientMode.Vertical))
                        {
                            Utility.FillRoundedRectangle(g, bGray64, bounds, 4);
                        }
                    }
                    if (((state & ItemState.Hovered) != ItemState.None))
                    {
                        using (Brush bHovered = new LinearGradientBrush(bounds, Color.FromArgb(8, SystemColors.Highlight), Color.FromArgb(32, SystemColors.Highlight), LinearGradientMode.Vertical))
                        {
                            Utility.FillRoundedRectangle(g, bHovered, bounds, 4);
                        }
                    }

                    // Draw the image
                    Image img = item.ThumbnailImage;
                    if (img != null)
                    {
                        Rectangle pos = Utility.GetSizedImageBounds(img, new Rectangle(bounds.Location + itemPadding, ImageListView.ThumbnailSize), 0.0f, 50.0f);
                        g.DrawImage(img, pos);
                        // Draw image border
                        if (Math.Min(pos.Width, pos.Height) > 32)
                        {
                            using (Pen pGray128 = new Pen(Color.FromArgb(128, Color.Gray)))
                            {
                                g.DrawRectangle(pGray128, pos);
                            }
                            using (Pen pWhite128 = new Pen(Color.FromArgb(128, Color.White)))
                            {
                                g.DrawRectangle(pWhite128, Rectangle.Inflate(pos, -1, -1));
                            }
                        }

                        // Draw item text
                        int lineHeight = CaptionFont.Height;
                        RectangleF rt;
                        using (StringFormat sf = new StringFormat())
                        {
                            rt = new RectangleF(bounds.Left + 2 * itemPadding.Width + ImageListView.ThumbnailSize.Width,
                                bounds.Top + itemPadding.Height + (Math.Max(ImageListView.ThumbnailSize.Height, mTextHeight) - mTextHeight) / 2,
                                mTileWidth, lineHeight);
                            sf.Alignment = StringAlignment.Near;
                            sf.FormatFlags = StringFormatFlags.NoWrap;
                            sf.LineAlignment = StringAlignment.Center;
                            sf.Trimming = StringTrimming.EllipsisCharacter;

                             
                            using (Brush bItemFore = new SolidBrush(item.ForeColor))
                            {
                                g.DrawString(item.GetSubItemText(ImageListView.TitleLine1), CaptionFont, bItemFore, rt, sf);
                            }

                            using (Brush bItemDetails = new SolidBrush(Color.Gray))
                            {
                                //Line 2 : 
                                rt.Offset(0, 1.5f * lineHeight);

                                string line;

                                try
                                {
                                    line = item.GetSubItemText(ImageListView.TitleLine2);
                                    g.DrawString(line, ImageListView.Font, bItemDetails, rt, sf);
                                    rt.Offset(0, 1.1f * lineHeight);

                                    line = item.GetSubItemText(ImageListView.TitleLine3);
                                    g.DrawString(line, ImageListView.Font, bItemDetails, rt, sf);
                                    rt.Offset(0, 1.1f * lineHeight);


                                    line = item.GetSubItemText(ImageListView.TitleLine4);
                                    g.DrawString(line, ImageListView.Font, bItemDetails, rt, sf);
                                    rt.Offset(0, 1.1f * lineHeight);

                                    line = item.GetSubItemText(ImageListView.TitleLine5);
                                    g.DrawString(line, ImageListView.Font, bItemDetails, rt, sf);
                                    rt.Offset(0, 1.1f * lineHeight);
                                } catch (Exception ex) {
                                    Console.WriteLine(ex.Message); //For debug reason
                                    //Error can occure when ImageListView.Font
                                }
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

                    // Focus rectangle
                    if (ImageListView.Focused && ((state & ItemState.Focused) != ItemState.None))
                    {
                        ControlPaint.DrawFocusRectangle(g, bounds);
                    }
                }
                else
                    base.DrawItem(g, item, state, bounds);
            }
        }
        #endregion

    }
}
