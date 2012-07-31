using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Mathematics.Algebra;

namespace MasterOfCentauri.UIControls.Base
{
    class SpacedStackPanel: StackPanel
    {
        /// <inheritdoc/>
        protected override void OnArrange(Vector2F position, Vector2F size)
        {
            // Get extreme positions of arrange area.
            float left = position.X;
            float top = position.Y;
            float right = left + size.X;
            float bottom = top + size.Y;

            if (Orientation == DigitalRune.Game.UI.Orientation.Horizontal)
            {
                // ----- Horizontal: 
                // Each child gets its desired width or the rest of the available space.
                foreach (var child in VisualChildren)
                {
                    float availableSize = Math.Max(0.0f, right - left);
                    float sizeX = Math.Min(availableSize, child.DesiredWidth);
                    float sizeY = size.Y;
                    child.Arrange(new Vector2F(left, top), new Vector2F(sizeX, sizeY));
                    left += sizeX + 10;
                }
            }
            else
            {
                // ----- Vertical
                // Each child gets its desired height or the rest of the available space.
                foreach (var child in VisualChildren)
                {
                    float sizeX = size.X;
                    float availableSize = Math.Max(0.0f, bottom - top);
                    float sizeY = Math.Min(availableSize, child.DesiredHeight);
                    child.Arrange(new Vector2F(left, top), new Vector2F(sizeX, sizeY));
                    top += sizeY + 10;
                }
            }
        }
    }
}
