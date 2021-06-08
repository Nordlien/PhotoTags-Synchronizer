// ImageListView - A listview control for image files
// Copyright (C) 2009 Ozgur Ozcitak
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Ozgur Ozcitak (ozcitak@yahoo.com)

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

        #region RendererNoir
        /// <summary>
        /// A renderer with a dark theme.
        /// </summary>
        public class RendererNoir : ImageListView.ImageListViewRenderer
        {
            private int padding;
            private int mReflectionSize;

            /// <summary>
            /// Gets or sets the size of image reflections.
            /// </summary>
            public int ReflectionSize { get { return mReflectionSize; } set { mReflectionSize = value; } }

            /// <summary>
            /// Initializes a new instance of the RendererNoir class.
            /// </summary>
            public RendererNoir()
                : this(20)
            {
                ;
            }
            /// <summary>
            /// Initializes a new instance of the RendererNoir class.
            /// </summary>
            /// <param name="reflectionSize">Size of image reflections.</param>
            public RendererNoir(int reflectionSize)
            {
                mReflectionSize = reflectionSize;
                padding = 4;
            }

            /// <summary>
            /// Draws the background of the control.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="bounds">The client coordinates of the item area.</param>
            public override void DrawBackground(Graphics g, Rectangle bounds)
            {
                g.Clear(Color.FromArgb(16, 16, 16));
            }
            /// <summary>
            /// Returns item size for the given view mode.
            /// </summary>
            /// <param name="view">The view mode for which the measurement should be made.</param>
            /// <returns></returns>
            public override Size MeasureItem(View view)
            {
                if (view == View.Details)
                    return base.MeasureItem(view);
                else
                    return new Size(ImageListView.ThumbnailSize.Width + 2 * padding,
                        ImageListView.ThumbnailSize.Height + 2 * padding + mReflectionSize);
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
                // Fill with item background color
                if (item.BackColor != Color.Transparent)
                {
                    using (Brush brush = new SolidBrush(item.BackColor))
                    {
                        g.FillRectangle(brush, bounds);
                    }
                }

                if (ImageListView.View == View.Details)
                {
                    // Item background
                    if ((state & ItemState.Selected) == ItemState.Selected)
                    {
                        using (Brush brush = new LinearGradientBrush(bounds,
                            Color.FromArgb(64, 96, 160), Color.FromArgb(64, 64, 96, 160), LinearGradientMode.Horizontal))
                        {
                            g.FillRectangle(brush, bounds);
                        }
                    }
                    else if ((state & ItemState.Hovered) == ItemState.Hovered)
                    {
                        using (Brush brush = new LinearGradientBrush(bounds,
                            Color.FromArgb(64, Color.White), Color.FromArgb(16, Color.White), LinearGradientMode.Horizontal))
                        {
                            g.FillRectangle(brush, bounds);
                        }
                    }

                    // Shade sort column
                    List<ImageListView.ImageListViewColumnHeader> uicolumns = mImageListView.Columns.GetDisplayedColumns();
                    int x = mImageListView.layoutManager.ColumnHeaderBounds.Left;
                    foreach (ImageListView.ImageListViewColumnHeader column in uicolumns)
                    {
                        if (mImageListView.SortColumn == column.Type && mImageListView.SortOrder != SortOrder.None &&
                            (state & ItemState.Hovered) == ItemState.None && (state & ItemState.Selected) == ItemState.None)
                        {
                            Rectangle subItemBounds = bounds;
                            subItemBounds.X = x;
                            subItemBounds.Width = column.Width;
                            using (Brush brush = new SolidBrush(Color.FromArgb(32, 128, 128, 128)))
                            {
                                g.FillRectangle(brush, subItemBounds);
                            }
                            break;
                        }
                        x += column.Width;
                    }
                    // Separators 
                    x = mImageListView.layoutManager.ColumnHeaderBounds.Left;
                    foreach (ImageListView.ImageListViewColumnHeader column in uicolumns)
                    {
                        x += column.Width;
                        if (!ReferenceEquals(column, uicolumns[uicolumns.Count - 1]))
                        {
                            using (Pen pen = new Pen(Color.FromArgb(64, 128, 128, 128)))
                            {
                                g.DrawLine(pen, x, bounds.Top, x, bounds.Bottom);
                            }
                        }
                    }

                    // Item texts
                    Size offset = new Size(2, (bounds.Height - mImageListView.Font.Height) / 2);
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.FormatFlags = StringFormatFlags.NoWrap;
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Trimming = StringTrimming.EllipsisCharacter;
                        // Sub text
                        RectangleF rt = new RectangleF(bounds.Left + offset.Width, bounds.Top + offset.Height, uicolumns[0].Width - 2 * offset.Width, bounds.Height - 2 * offset.Height);
                        foreach (ImageListView.ImageListViewColumnHeader column in uicolumns)
                        {
                            rt.Width = column.Width - 2 * offset.Width;
                            using (Brush bItemFore = new SolidBrush(Color.White))
                            {
                                g.DrawString(item.GetSubItemText(column.Type), mImageListView.Font, bItemFore, rt, sf);
                            }
                            rt.X += column.Width;
                        }
                    }

                    // Border
                    if ((state & ItemState.Hovered) == ItemState.Hovered)
                    {
                        using (Pen pen = new Pen(Color.FromArgb(128, Color.White)))
                        {
                            g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                        }
                    }
                    else if ((state & ItemState.Selected) == ItemState.Hovered)
                    {
                        using (Pen pen = new Pen(Color.FromArgb(96, 144, 240)))
                        {
                            g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                        }
                    }
                }
                else
                {
                    // Align images to bottom of bounds
                    Image img = item.ThumbnailImage;
                    Rectangle pos = Utility.GetSizedImageBounds(img,
                        new Rectangle(bounds.X + padding, bounds.Y + padding, bounds.Width - 2 * padding, bounds.Height - 2 * padding - mReflectionSize),
                        50.0f, 100.0f);

                    int x = pos.X;
                    int y = pos.Y;

                    // Item background
                    if ((state & ItemState.Selected) == ItemState.Selected)
                    {
                        using (Brush brush = new LinearGradientBrush(
                            new Point(x - padding, y - padding), new Point(x - padding, y + pos.Height + 2 * padding),
                            Color.FromArgb(64, 96, 160), Color.FromArgb(16, 16, 16)))
                        {
                            g.FillRectangle(brush, x - padding, y - padding, pos.Width + 2 * padding, pos.Height + 2 * padding);
                        }
                    }
                    else if ((state & ItemState.Hovered) == ItemState.Hovered)
                    {
                        using (Brush brush = new LinearGradientBrush(
                            new Point(x - padding, y - padding), new Point(x - padding, y + pos.Height + 2 * padding),
                            Color.FromArgb(64, Color.White), Color.FromArgb(16, 16, 16)))
                        {
                            g.FillRectangle(brush, x - padding, y - padding, pos.Width + 2 * padding, pos.Height + 2 * padding);
                        }
                    }

                    // Border
                    if ((state & ItemState.Hovered) == ItemState.Hovered)
                    {
                        using (Brush brush = new LinearGradientBrush(
                            new Point(x - padding, y - padding), new Point(x - padding, y + pos.Height + 2 * padding),
                            Color.FromArgb(128, Color.White), Color.FromArgb(16, 16, 16)))
                        using (Pen pen = new Pen(brush))
                        {
                            g.DrawRectangle(pen, x - padding, y - padding + 1, pos.Width + 2 * padding - 1, pos.Height + 2 * padding - 1);
                        }
                    }
                    else if ((state & ItemState.Selected) == ItemState.Selected)
                    {
                        using (Brush brush = new LinearGradientBrush(
                            new Point(x - padding, y - padding), new Point(x - padding, y + pos.Height + 2 * padding),
                            Color.FromArgb(96, 144, 240), Color.FromArgb(16, 16, 16)))
                        using (Pen pen = new Pen(brush))
                        {
                            g.DrawRectangle(pen, x - padding, y - padding + 1, pos.Width + 2 * padding - 1, pos.Height + 2 * padding - 1);
                        }
                    }

                    // Draw item image
                    DrawImageWithReflection(g, img, pos, mReflectionSize);

                    // Highlight
                    using (Pen pen = new Pen(Color.FromArgb(160, Color.White)))
                    {
                        g.DrawLine(pen, pos.X, pos.Y + 1, pos.X + pos.Width - 1, pos.Y + 1);
                        g.DrawLine(pen, pos.X, pos.Y + 1, pos.X, pos.Y + pos.Height);
                    }
                }
            }
            /// <summary>
            /// Draws the column headers.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="column">The ImageListViewColumnHeader to draw.</param>
            /// <param name="state">The current view state of column.</param>
            /// <param name="bounds">The bounding rectangle of column in client coordinates.</param>
            public override void DrawColumnHeader(Graphics g, ImageListView.ImageListViewColumnHeader column, ColumnState state, Rectangle bounds)
            {
                // Paint background
                if (mImageListView.Focused && ((state & ColumnState.Hovered) == ColumnState.Hovered))
                {
                    using (Brush bHovered = new LinearGradientBrush(bounds,
                        Color.FromArgb(64, 96, 144, 240), Color.FromArgb(196, 96, 144, 240), LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(bHovered, bounds);
                    }
                }
                else
                {
                    using (Brush bNormal = new LinearGradientBrush(bounds,
                        Color.FromArgb(32, 128, 128, 128), Color.FromArgb(196, 128, 128, 128), LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(bNormal, bounds);
                    }
                }
                using (Brush bBorder = new LinearGradientBrush(bounds,
                    Color.FromArgb(96, 128, 128, 128), Color.FromArgb(128, 128, 128), LinearGradientMode.Vertical))
                using (Pen pBorder = new Pen(bBorder))
                {
                    g.DrawLine(pBorder, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom);
                    g.DrawLine(pBorder, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                }
                using (Pen pen = new Pen(Color.FromArgb(16, Color.White)))
                {
                    g.DrawLine(pen, bounds.Left + 1, bounds.Top + 1, bounds.Left + 1, bounds.Bottom - 2);
                    g.DrawLine(pen, bounds.Right - 1, bounds.Top + 1, bounds.Right - 1, bounds.Bottom - 2);
                }

                // Draw the sort arrow
                int textOffset = 4;
                if (mImageListView.SortOrder != SortOrder.None && mImageListView.SortColumn == column.Type)
                {
                    Image img = null;
                    if (mImageListView.SortOrder == SortOrder.Ascending)
                        img = ImageListViewResources.SortAscending;
                    else if (mImageListView.SortOrder == SortOrder.Descending)
                        img = ImageListViewResources.SortDescending;
                    g.DrawImageUnscaled(img, bounds.X + 4, bounds.Top + (bounds.Height - img.Height) / 2);
                    textOffset += img.Width;
                }

                // Text
                bounds.X += textOffset;
                bounds.Width -= textOffset;
                if (bounds.Width > 4)
                {
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.FormatFlags = StringFormatFlags.NoWrap;
                        sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Trimming = StringTrimming.EllipsisCharacter;
                        using (Brush brush = new SolidBrush(Color.White))
                        {
                            g.DrawString(column.Text,
                                (mImageListView.HeaderFont == null ? mImageListView.Font : mImageListView.HeaderFont),
                                brush, bounds, sf);
                        }
                    }
                }
            }
            /// <summary>
            /// Draws the extender after the last column.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="bounds">The bounding rectangle of extender column in client coordinates.</param>
            public override void DrawColumnExtender(Graphics g, Rectangle bounds)
            {
                using (Brush bNormal = new LinearGradientBrush(bounds,
                    Color.FromArgb(32, 128, 128, 128), Color.FromArgb(196, 128, 128, 128), LinearGradientMode.Vertical))
                {
                    g.FillRectangle(bNormal, bounds);
                }
                using (Brush bBorder = new LinearGradientBrush(bounds,
                    Color.FromArgb(96, 128, 128, 128), Color.FromArgb(128, 128, 128), LinearGradientMode.Vertical))
                using (Pen pBorder = new Pen(bBorder))
                {
                    g.DrawLine(pBorder, bounds.Left, bounds.Top, bounds.Left, bounds.Bottom);
                    g.DrawLine(pBorder, bounds.Left, bounds.Bottom - 1, bounds.Right, bounds.Bottom - 1);
                }
                using (Pen pen = new Pen(Color.FromArgb(16, Color.White)))
                {
                    g.DrawLine(pen, bounds.Left + 1, bounds.Top + 1, bounds.Left + 1, bounds.Bottom - 2);
                    g.DrawLine(pen, bounds.Right - 1, bounds.Top + 1, bounds.Right - 1, bounds.Bottom - 2);
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
                if (item != null && image != null)
                {
                    Size itemMargin = MeasureItemMargin(ImageListView.View);
                    Rectangle pos = Utility.GetSizedImageBounds(image, new Rectangle(bounds.X + itemMargin.Width, bounds.Y + itemMargin.Height, bounds.Width - 2 * itemMargin.Width, bounds.Height - 2 * itemMargin.Height - mReflectionSize), 50.0f, 100.0f);
                    DrawImageWithReflection(g, image, pos, mReflectionSize);
                }
            }
            /// <summary>
            /// Draws the left pane in Pane view mode.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="item">The ImageListViewItem to draw.</param>
            /// <param name="image">The image to draw.</param>
            /// <param name="bounds">The bounding rectangle of the pane.</param>
            public override void DrawPane(Graphics g, ImageListViewItem item, Image image, Rectangle bounds)
            {
                // Draw resize handle
                using (Brush bBorder = new SolidBrush(Color.FromArgb(64, 64, 64)))
                {
                    g.FillRectangle(bBorder, bounds.Right - 2, bounds.Top, 2, bounds.Height);
                }
                bounds.Width -= 2;

                if (item != null && image != null)
                {
                    // Calculate image bounds
                    Size itemMargin = MeasureItemMargin(ImageListView.View);
                    Rectangle pos = Utility.GetSizedImageBounds(image, new Rectangle(bounds.Location + itemMargin, bounds.Size - itemMargin - itemMargin), 50.0f, 0.0f);
                    // Draw image
                    g.DrawImage(image, pos);

                    bounds.X += itemMargin.Width;
                    bounds.Width -= 2 * itemMargin.Width;
                    bounds.Y = pos.Height + 16;
                    bounds.Height -= pos.Height + 16;

                    // Item text
                    if (mImageListView.Columns[ColumnType.Name].Visible && bounds.Height > 0)
                    {
                        int y = Utility.DrawStringPair(g, bounds, "", item.Text, mImageListView.Font,
                            Brushes.White, Brushes.White);
                        bounds.Y += 2 * y;
                        bounds.Height -= 2 * y;
                    }

                    // File type
                    if (mImageListView.Columns[ColumnType.FileType].Visible && bounds.Height > 0 && !string.IsNullOrWhiteSpace(item.FileType))
                    {
                        using (Brush bCaption = new SolidBrush(Color.FromArgb(196, 196, 196)))
                        using (Brush bText = new SolidBrush(Color.White))
                        {
                            int y = Utility.DrawStringPair(g, bounds, mImageListView.Columns[ColumnType.FileType].Text + ": ",
                                item.FileType, mImageListView.Font, bCaption, bText);
                            bounds.Y += y;
                            bounds.Height -= y;
                        }
                    }

                    // Metatada
                    foreach (ImageListView.ImageListViewColumnHeader column in mImageListView.Columns)
                    {
                        if (column.Type == ColumnType.MediaDescription)
                        {
                            bounds.Y += 8;
                            bounds.Height -= 8;
                        }

                        if (bounds.Height <= 0) break;

                        if (column.Visible &&
                            column.Type != ColumnType.FileType &&
                            column.Type != ColumnType.DateAccessed &&
                            column.Type != ColumnType.FileFullPath &&
                            column.Type != ColumnType.FileDirectory &&
                            column.Type != ColumnType.Name)
                        {
                            string caption = column.Text;
                            string text = item.GetSubItemText(column.Type);
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                using (Brush bCaption = new SolidBrush(Color.FromArgb(196, 196, 196)))
                                using (Brush bText = new SolidBrush(Color.White))
                                {
                                    int y = Utility.DrawStringPair(g, bounds, caption + ": ", text,
                                        mImageListView.Font, bCaption, bText);
                                    bounds.Y += y;
                                    bounds.Height -= y;
                                }
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Draws the selection rectangle.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="selection">The client coordinates of the selection rectangle.</param>
            public override void DrawSelectionRectangle(Graphics g, Rectangle selection)
            {
                using (Brush brush = new LinearGradientBrush(selection,
                    Color.FromArgb(160, 96, 144, 240), Color.FromArgb(32, 96, 144, 240),
                    LinearGradientMode.ForwardDiagonal))
                {
                    g.FillRectangle(brush, selection);
                }
                using (Brush brush = new LinearGradientBrush(selection,
                    Color.FromArgb(96, 144, 240), Color.FromArgb(128, 96, 144, 240),
                    LinearGradientMode.ForwardDiagonal))
                using (Pen pen = new Pen(brush))
                {
                    g.DrawRectangle(pen, selection);
                }
            }
            /// <summary>
            /// Draws the insertion caret for drag and drop operations.
            /// </summary>
            /// <param name="g">The System.Drawing.Graphics to draw on.</param>
            /// <param name="bounds">The bounding rectangle of the insertion caret.</param>
            public override void DrawInsertionCaret(Graphics g, Rectangle bounds)
            {
                using (Brush b = new SolidBrush(Color.FromArgb(96, 144, 240)))
                {
                    g.FillRectangle(b, bounds);
                }
            }

            /// <summary>
            /// Draws an image with a reflection effect at the bottom.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="img">The image to draw.</param>
            /// <param name="x">The x coordinate of the upper left corner of the image.</param>
            /// <param name="y">The y coordinate of the upper left corner of the image.</param>
            /// <param name="reflection">Height of the reflection.</param>
            private void DrawImageWithReflection(Graphics g, Image img, int x, int y, int reflection)
            {
                // Draw the image
                g.DrawImageUnscaled(img, x, y + 1);

                // Draw the reflection
                if (img.Width > 32 && img.Height > 32)
                {
                    int reflectionHeight = img.Height / 2;
                    if (reflectionHeight > reflection) reflectionHeight = reflection;

                    Region prevClip = g.Clip;
                    g.SetClip(new Rectangle(x, y + img.Height + 1, img.Width, reflectionHeight));
                    g.DrawImage(img, x, y + img.Height + img.Height / 2 + 1, img.Width, -img.Height / 2);
                    g.Clip = prevClip;

                    using (Brush brush = new LinearGradientBrush(
                        new Point(x, y + img.Height + 1), new Point(x, y + img.Height + reflectionHeight + 1),
                        Color.FromArgb(128, 16, 16, 16), Color.FromArgb(16, 16, 16)))
                    {
                        g.FillRectangle(brush, x, y + img.Height + 1, img.Width, reflectionHeight);
                    }
                }
            }
            /// <summary>
            /// Draws an image with a reflection effect at the bottom.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="img">The image to draw.</param>
            /// <param name="x">The x coordinate of the upper left corner of the image.</param>
            /// <param name="y">The y coordinate of the upper left corner of the image.</param>
            /// <param name="width">Width of the drawn image.</param>
            /// <param name="height">Height of the drawn image.</param>
            /// <param name="reflection">Height of the reflection.</param>
            private void DrawImageWithReflection(Graphics g, Image img, int x, int y, int width, int height, int reflection)
            {
                // Draw the image
                g.DrawImage(img, x, y + 1, width, height);

                // Draw the reflection
                if (img.Width > 32 && img.Height > 32)
                {
                    int reflectionHeight = height / 2;
                    if (reflectionHeight > reflection) reflectionHeight = reflection;

                    Region prevClip = g.Clip;
                    g.SetClip(new Rectangle(x, y + height + 1, width, reflectionHeight));
                    g.DrawImage(img, x, y + height + height / 2 + 1, width, -height / 2);
                    g.Clip = prevClip;

                    using (Brush brush = new LinearGradientBrush(
                        new Point(x, y + height + 1), new Point(x, y + height + reflectionHeight + 1),
                        Color.FromArgb(128, 16, 16, 16), Color.FromArgb(16, 16, 16)))
                    {
                        g.FillRectangle(brush, x, y + height + 1, width, reflectionHeight);
                    }
                }
            }
            /// <summary>
            /// Draws an image with a reflection effect at the bottom.
            /// </summary>
            /// <param name="g">The graphics to draw on.</param>
            /// <param name="img">The image to draw.</param>
            /// <param name="bounds">The target bounding rectangle.</param>
            /// <param name="reflection">Height of the reflection.</param>
            private void DrawImageWithReflection(Graphics g, Image img, Rectangle bounds, int reflection)
            {
                DrawImageWithReflection(g, img, bounds.X, bounds.Y, bounds.Width, bounds.Height, reflection);
            }
        }
        #endregion

    }
}
