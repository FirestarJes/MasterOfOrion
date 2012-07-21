using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalRune.Game.UI.Controls;
using DigitalRune.Game.Input;
using DigitalRune.Game;

namespace MasterOfCentauri.UIControls
{
    class MiniMapWindow: ContentControl 
    {
        /// <summary> 
        /// The ID of the <see cref="IsDown"/> game object property.
        /// </summary>
        [Browsable(false)]
        public static readonly int IsDownPropertyId = CreateProperty(
          typeof(ButtonBase), "IsDown", GamePropertyCategories.Default, null, false,
          UIPropertyOptions.AffectsRender);

        /// <summary>
        /// Gets a value indicating whether this button is currently pressed down. 
        /// This is a game object property.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this button is currently pressed down; otherwise, 
        /// <see langword="false"/>.
        /// </value>
        public bool IsDown
        {
            get { return GetValue<bool>(IsDownPropertyId); }
            private set { SetValue(IsDownPropertyId, value); }
        }

        public MiniMapWindow()
        {
            Style = "MiniMapWindow";
        }
        protected override void OnHandleInput(InputContext context)
        {
            base.OnHandleInput(context);
            var inputService = InputService;

            if (!IsDown)
            {
                // Check if button gets pressed down.
                if (IsMouseOver
                    && !inputService.IsMouseOrTouchHandled
                    && inputService.IsPressed(MouseButtons.Left, false))
                {
                    inputService.IsMouseOrTouchHandled = true;
                    IsDown = true;
                    //IsClicked = true;
                }

                if (IsFocusWithin && !inputService.IsKeyboardHandled)
                {
                    inputService.IsKeyboardHandled = true;
                    IsDown = true;
                    //IsClicked = true;
                }

            }
            else
            {
                if ((!inputService.IsMouseOrTouchHandled && inputService.IsDown(MouseButtons.Left)))
                {
                    // IsDown stays true.
                }
                else
                {
                    IsDown = false;
                }

                // Input is still captured for this frame.
                inputService.IsMouseOrTouchHandled = true;
                inputService.IsKeyboardHandled = true;

            }


        }
    }

    
}
