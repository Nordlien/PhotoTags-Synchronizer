﻿#region BSD License
/*
 * 
 * Original BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
 *  © Component Factory Pty Ltd, 2006 - 2016, All rights reserved.
 * 
 *  New BSD 3-Clause License (https://github.com/Krypton-Suite/Standard-Toolkit/blob/master/LICENSE)
 *  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV), et al. 2017 - 2021. All rights reserved. 
 *  
 */
#endregion


namespace Krypton.Toolkit
{
    /// <summary>
    /// 
    /// </summary>
    public class PaletteOffice365BlackDarkMode : PaletteOffice365BlackThemeDarkModeBase
    {
        #region Static Fields
        private static readonly ImageList _checkBoxList;
        private static readonly ImageList _galleryButtonList;
        private static readonly Image[] _radioButtonArray;
        private static readonly Image _blackDropDownButton = Office2010Arrows._2010BlackDropDownButton;
        private static readonly Image _contextMenuSubMenu = Office2010Arrows._2010BlackContextMenuSub;
        private static readonly Image _formCloseNormal = Office2010ControlBoxResources._2010ButtonCloseHover;
        private static readonly Image _formCloseDisabled = Office2010ControlBoxResources._2010ButtonCloseBlackNormal;
        private static readonly Image _formMaximiseNormal = Office2010ControlBoxResources._2010ButtonMaxBlackNormal;
        private static readonly Image _formMaximiseDisabled = null;
        private static readonly Image _formMinimiseNormal = Office2010ControlBoxResources.Office2010BlackControlBoxButtonMinNormal;
        private static readonly Image _formMinimiseHover = Office2010ControlBoxResources.Office2010BlackControlBoxButtonMinHover;
        private static readonly Image _formMinimiseDisabled = Office2010ControlBoxResources.Office2010BlackControlBoxButtonMinDisabled;
        private static readonly Image _formRestoreNormal = Office2010ControlBoxResources._2010ButtonRestore;
        private static readonly Image _formRestoreDisabled = null;
        private static readonly Image _formHelpNormal = HelpIconResources.GenericOffice2010HelpIconBlack;
        private static readonly Image _formHelpHover = HelpIconResources.GenericOffice2010HelpIconHover;
        private static readonly Image _formHelpDisabled = HelpIconResources.GenericOffice365HelpIconDisabled;
        private static readonly Image _buttonSpecPendantClose = Office2010ControlBoxResources._2010ButtonMDICloseBlack;
        private static readonly Image _buttonSpecPendantMin = Office2010ControlBoxResources._2010ButtonMDIMinBlack;
        private static readonly Image _buttonSpecPendantRestore = Office2010ControlBoxResources._2010ButtonMDIRestoreBlack;
        private static readonly Image _buttonSpecRibbonMinimize = RibbonArrowImageResources.RibbonUp2010Black;
        private static readonly Image _buttonSpecRibbonExpand = RibbonArrowImageResources.RibbonDown2010Black;
        private static readonly Color _disabledRibbonText = Color.FromArgb(205, 205, 205);
        private static readonly Color[] _trackBarColors = { Color.FromArgb( 17,  17,  17),      // Tick marks
                                                                        Color.FromArgb( 37,  37,  37),      // Top track
                                                                        Color.FromArgb(174, 174, 174),      // Bottom track
                                                                        Color.FromArgb(131, 132, 132),      // Fill track
                                                                        Color.FromArgb(64, Color.White),    // Outside position
                                                                        Color.FromArgb(35, 35, 35)          // Border (normal) position
                                                                      };
        private static readonly Color[] _schemeColors = {             Color.White,             // TextLabelControl - Why is this used for  context menu normal & tracking text?
                                                                      Color.White,                      // TextButtonNormal - Normal button text
                                                                      Color.FromArgb(128, 128, 128),                      // TextButtonChecked
                                                                      Color.FromArgb(106, 106, 106),    // ButtonNormalBorder1
                                                                      Color.FromArgb( 32, 32, 32),    // ButtonNormalDefaultBorder
                                                                      Color.FromArgb(38, 38, 38),    // ButtonNormalBack1
                                                                      Color.FromArgb(34, 34, 34),    // ButtonNormalBack2
                                                                      Color.FromArgb(225, 225, 225),    // ButtonNormalDefaultBack1
                                                                      Color.FromArgb(185, 185, 185),    // ButtonNormalDefaultBack2
                                                                      Color.FromArgb( 32, 32, 32),    // ButtonNormalNavigatorBack1
                                                                      Color.FromArgb( 32, 32, 32),    // ButtonNormalNavigatorBack2
                                                                      Color.FromArgb(99, 99, 99),    // PanelClient - Panel, ribbon etc
                                                                      Color.FromArgb(61, 61, 61),    // PanelAlternative
                                                                      Color.FromArgb( 46,  46,  46),    // ControlBorder
                                                                      Color.FromArgb(172, 172, 172),    // SeparatorHighBorder1
                                                                      Color.FromArgb(111, 111, 111),    // SeparatorHighBorder2
                                                                      Color.FromArgb(139, 139, 139),    // HeaderPrimaryBack1
                                                                      Color.FromArgb( 72,  72,  72),    // HeaderPrimaryBack2
                                                                      Color.FromArgb(190, 190, 190),    // HeaderSecondaryBack1
                                                                      Color.FromArgb(145, 145, 145),    // HeaderSecondaryBack2
                                                                      Color.Black,                      // HeaderText
                                                                      Color.FromArgb(226, 226, 226),    // StatusStripText
                                                                      Color.FromArgb(236, 199,  87),    // ButtonBorder
                                                                      Color.FromArgb( 89,  89,  89),    // SeparatorLight
                                                                      Color.Black,                      // SeparatorDark
                                                                      Color.FromArgb( 89,  89,  89),    // GripLight
                                                                      Color.FromArgb( 27,  27,  27),    // GripDark
                                                                      Color.FromArgb(113, 113, 113),    // ToolStripBack
                                                                      Color.FromArgb( 75,  75,  75),    // StatusStripLight
                                                                      Color.FromArgb( 50,  50,  50),    // StatusStripDark
                                                                      Color.White,                      // ImageMargin
                                                                      Color.FromArgb( 75,  75,  75),    // ToolStripBegin
                                                                      Color.FromArgb( 50,  50,  50),    // ToolStripMiddle
                                                                      Color.FromArgb( 50,  50,  50),    // ToolStripEnd
                                                                      Color.FromArgb( 44,  44,  44),    // OverflowBegin
                                                                      Color.FromArgb(167, 167, 167),    // OverflowMiddle
                                                                      Color.FromArgb( 44,  44,  44),    // OverflowEnd
                                                                      Color.FromArgb( 44,  44,  44),    // ToolStripBorder
                                                                      Color.FromArgb(38, 38, 38),    // FormBorderActive
                                                                      Color.FromArgb(34, 34, 34),    // FormBorderInactive
                                                                      Color.FromArgb(113, 113, 113),    // FormBorderActiveLight
                                                                      Color.FromArgb(131, 131, 131),    // FormBorderActiveDark
                                                                      Color.FromArgb(158, 158, 158),    // FormBorderInactiveLight
                                                                      Color.FromArgb(158, 158, 158),    // FormBorderInactiveDark
                                                                      Color.FromArgb( 65,  65,  65),    // FormBorderHeaderActive
                                                                      Color.FromArgb(154, 154, 154),    // FormBorderHeaderInactive
                                                                      Color.FromArgb(121, 121, 121),    // FormBorderHeaderActive1
                                                                      Color.FromArgb(113, 113, 113),    // FormBorderHeaderActive2
                                                                      Color.FromArgb(158, 158, 158),    // FormBorderHeaderInctive1
                                                                      Color.FromArgb(158, 158, 158),    // FormBorderHeaderInctive2
                                                                      Color.FromArgb(226, 226, 226),    // FormHeaderShortActive
                                                                      Color.FromArgb(212, 212, 212),    // FormHeaderShortInactive
                                                                      Color.FromArgb(226, 226, 226),    // FormHeaderLongActive
                                                                      Color.FromArgb(212, 212, 212),    // FormHeaderLongInactive
                                                                      Color.FromArgb( 81,  81,  81),    // FormButtonBorderTrack
                                                                      Color.FromArgb(151, 151, 151),    // FormButtonBack1Track
                                                                      Color.FromArgb(116, 116, 116),    // FormButtonBack2Track
                                                                      Color.FromArgb( 81,  81,  81),    // FormButtonBorderPressed
                                                                      Color.FromArgb(113, 113, 113),    // FormButtonBack1Pressed
                                                                      Color.FromArgb( 93,  93,  93),    // FormButtonBack2Pressed
                                                                      Color.FromArgb( 70,  70,  70),    // TextButtonFormNormal
                                                                      Color.Black,                                   // TextButtonFormTracking - Button hover text
                                                                      Color.FromArgb(75, 75, 75),    // TextButtonFormPressed
                                                                      Color.Blue,                       // LinkNotVisitedOverrideControl
                                                                      Color.Purple,                     // LinkVisitedOverrideControl
                                                                      Color.Red,                        // LinkPressedOverrideControl
                                                                      Color.FromArgb(180, 210, 255),    // LinkNotVisitedOverridePanel
                                                                      Color.Violet,                     // LinkVisitedOverridePanel
                                                                      Color.FromArgb(255,  90,  90),    // LinkPressedOverridePanel
                                                                      Color.White,                      // TextLabelPanel
                                                                      //Color.FromArgb(226, 226, 226),    // RibbonTabTextNormal
                                                                      Color.FromArgb(128, 128, 128), // RibbonTabTextNormal - The unselected tab text colour
                                                                      Color.White,                      // RibbonTabTextChecked
                                                                      Color.FromArgb(99, 99, 99),    // RibbonTabSelected1
                                                                      Color.FromArgb(99, 99, 99),    // RibbonTabSelected2
                                                                      Color.FromArgb(99, 99, 99),    // RibbonTabSelected3
                                                                      Color.FromArgb(99, 99, 99),    // RibbonTabSelected4
                                                                      Color.FromArgb(99, 99, 99),    // RibbonTabSelected5
                                                                      Color.FromArgb( 32, 32, 32),    // RibbonTabTracking1
                                                                      Color.FromArgb(183, 183, 183),    // RibbonTabTracking2
                                                                      Color.FromArgb( 32, 32, 32),    // RibbonTabHighlight1
                                                                      Color.FromArgb(201, 201, 201),    // RibbonTabHighlight2
                                                                      Color.FromArgb(192, 192, 192),    // RibbonTabHighlight3
                                                                      Color.FromArgb(192, 192, 192),    // RibbonTabHighlight4
                                                                      Color.FromArgb(192, 192, 192),    // RibbonTabHighlight5
                                                                      Color.FromArgb( 54,  54,  54),    // RibbonTabSeparatorColor
                                                                      Color.FromArgb( 32, 32, 32),    // RibbonGroupsArea1
                                                                      Color.FromArgb( 50,  50,  50),    // RibbonGroupsArea2
                                                                      Color.FromArgb(32, 32, 32),    // RibbonGroupsArea3
                                                                      Color.FromArgb(50, 50,50),    // RibbonGroupsArea4
                                                                      Color.FromArgb(32, 32, 32),    // RibbonGroupsArea5
                                                                      Color.FromArgb(159, 159, 159),    // RibbonGroupBorder1
                                                                      Color.FromArgb(194, 194, 194),    // RibbonGroupBorder2
                                                                      Color.Empty,                      // RibbonGroupTitle1
                                                                      Color.Empty,                      // RibbonGroupTitle2
                                                                      Color.Empty,                      // RibbonGroupBorderContext1
                                                                      Color.Empty,                      // RibbonGroupBorderContext2
                                                                      Color.Empty,                      // RibbonGroupTitleContext1
                                                                      Color.Empty,                      // RibbonGroupTitleContext2
                                                                      Color.FromArgb(99, 99, 99),    // RibbonGroupDialogDark
                                                                      Color.FromArgb(61, 61, 61),    // RibbonGroupDialogLight
                                                                      Color.Empty,                      // RibbonGroupTitleTracking1
                                                                      Color.Empty,                      // RibbonGroupTitleTracking2
                                                                      Color.FromArgb( 78,  78,  78),    // RibbonMinimizeBarDark
                                                                      Color.FromArgb(110, 110, 110),    // RibbonMinimizeBarLight
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorder1
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorder2
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorder3
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorder4
                                                                      Color.Empty,                      // RibbonGroupCollapsedBack1
                                                                      Color.Empty,                      // RibbonGroupCollapsedBack2
                                                                      Color.Empty,                      // RibbonGroupCollapsedBack3
                                                                      Color.Empty,                      // RibbonGroupCollapsedBack4
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorderT1
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorderT2
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorderT3
                                                                      Color.Empty,                      // RibbonGroupCollapsedBorderT4
                                                                      Color.Empty,                      // RibbonGroupCollapsedBackT1
                                                                      Color.Empty,                      // RibbonGroupCollapsedBackT2
                                                                      Color.Empty,                      // RibbonGroupCollapsedBackT3
                                                                      Color.Empty,                      // RibbonGroupCollapsedBackT4
                                                                      Color.FromArgb(147, 147, 147),    // RibbonGroupFrameBorder1
                                                                      Color.FromArgb(139, 139, 139),    // RibbonGroupFrameBorder2
                                                                      Color.FromArgb(187, 187, 188),    // RibbonGroupFrameInside1
                                                                      Color.FromArgb(167, 167, 168),    // RibbonGroupFrameInside2
                                                                      Color.Empty,                      // RibbonGroupFrameInside3
                                                                      Color.Empty,                      // RibbonGroupFrameInside4
                                                                      Color.White,                                   // RibbonGroupCollapsedText - Ribbon group button condensed text colour
                                                                      Color.FromArgb(99, 99,99),    // AlternatePressedBack1
                                                                      Color.FromArgb(61, 61, 61),    // AlternatePressedBack2
                                                                      Color.FromArgb(124, 125, 125),    // AlternatePressedBorder1
                                                                      Color.FromArgb(186, 186, 186),    // AlternatePressedBorder2
                                                                      Color.FromArgb( 43,  55,  67),    // FormButtonBack1Checked
                                                                      Color.FromArgb(106, 122, 140),    // FormButtonBack2Checked
                                                                      Color.FromArgb( 18,  18,  18),    // FormButtonBorderCheck
                                                                      Color.FromArgb( 33,  45,  57),    // FormButtonBack1CheckTrack
                                                                      Color.FromArgb(136, 152, 170),    // FormButtonBack2CheckTrack
                                                                      Color.FromArgb( 55,  55,  55),    // RibbonQATMini1
                                                                      Color.FromArgb(100, 100, 100),    // RibbonQATMini2
                                                                      Color.FromArgb( 73,  73,  73),    // RibbonQATMini3
                                                                      Color.FromArgb(12, Color.White),  // RibbonQATMini4
                                                                      Color.FromArgb(14, Color.White),  // RibbonQATMini5
                                                                      Color.FromArgb(100, 100, 100),    // RibbonQATMini1I
                                                                      Color.FromArgb(170, 170, 170),    // RibbonQATMini2I
                                                                      Color.FromArgb(140, 140, 140),    // RibbonQATMini3I
                                                                      Color.FromArgb(12, Color.White),  // RibbonQATMini4I
                                                                      Color.FromArgb(14, Color.White),  // RibbonQATMini5I
                                                                      Color.FromArgb(132, 132, 132),    // RibbonQATFullbar1                                                      
                                                                      Color.FromArgb(121, 121, 121),    // RibbonQATFullbar2                                                      
                                                                      Color.FromArgb( 50,  49,  49),    // RibbonQATFullbar3                                                      
                                                                      Color.FromArgb( 90,  90,  90),    // RibbonQATButtonDark                                                      
                                                                      Color.FromArgb(174, 174, 175),    // RibbonQATButtonLight                                                      
                                                                      Color.FromArgb(161, 161, 161),    // RibbonQATOverflow1                                                      
                                                                      Color.FromArgb( 68,  68,  68),    // RibbonQATOverflow2                                                      
                                                                      Color.FromArgb( 82,  82,  82),    // RibbonGroupSeparatorDark                                                      
                                                                      Color.FromArgb(190, 190, 190),    // RibbonGroupSeparatorLight                                                      
                                                                      Color.FromArgb(210, 217, 219),    // ButtonClusterButtonBack1                                                      
                                                                      Color.FromArgb(214, 222, 223),    // ButtonClusterButtonBack2                                                      
                                                                      Color.FromArgb(179, 188, 191),    // ButtonClusterButtonBorder1                                                      
                                                                      Color.FromArgb(145, 156, 159),    // ButtonClusterButtonBorder2                                                      
                                                                      Color.FromArgb(235, 235, 235),    // NavigatorMiniBackColor                                                    
                                                                      Color.FromArgb(205, 205, 205),    // GridListNormal1                                                    
                                                                      Color.FromArgb(166, 166, 166),    // GridListNormal2                                                    
                                                                      Color.FromArgb(166, 166, 166),    // GridListPressed1                                                    
                                                                      Color.FromArgb(205, 205, 205),    // GridListPressed2                                                    
                                                                      Color.FromArgb(150, 150, 150),    // GridListSelected                                                    
                                                                      Color.FromArgb(220, 220, 220),    // GridSheetColNormal1                                                    
                                                                      Color.FromArgb(200, 200, 200),    // GridSheetColNormal2                                                    
                                                                      Color.FromArgb(255, 223, 107),    // GridSheetColPressed1                                                    
                                                                      Color.FromArgb(255, 252, 230),    // GridSheetColPressed2                                                    
                                                                      Color.FromArgb(255, 211,  89),    // GridSheetColSelected1
                                                                      Color.FromArgb(255, 239, 113),    // GridSheetColSelected2
                                                                      Color.FromArgb(205, 205, 205),    // GridSheetRowNormal                                                   
                                                                      Color.FromArgb(255, 223, 107),    // GridSheetRowPressed
                                                                      Color.FromArgb(245, 210,  87),    // GridSheetRowSelected
                                                                      Color.FromArgb(218, 220, 221),    // GridDataCellBorder
                                                                      Color.FromArgb(183, 219, 255),    // GridDataCellSelected
                                                                      Color.White,                                   // InputControlTextNormal - Combobox, textbox etc text colour
                                                                      Color.FromArgb(128, 128, 128),    // InputControlTextDisabled
                                                                      Color.FromArgb(132, 132, 132),    // InputControlBorderNormal
                                                                      Color.FromArgb(187, 187, 187),    // InputControlBorderDisabled
                                                                      Color.FromArgb(38, 38, 38),       // InputControlBackNormal
                                                                      Color.FromArgb(240, 240, 240),    // InputControlBackDisabled
                                                                      Color.FromArgb(192, 192, 192),    // InputControlBackInactive
                                                                      Color.FromArgb(38, 38, 38),       // InputDropDownNormal1
                                                                      Color.FromArgb(38, 38, 38),       // InputDropDownNormal2
                                                                      Color.FromArgb(172, 168, 153),    // InputDropDownDisabled1
                                                                      Color.Transparent,                // InputDropDownDisabled2
                                                                      Color.FromArgb(240, 242, 245),    // ContextMenuHeadingBack
                                                                      Color.Black,                      // ContextMenuHeadingText
                                                                      Color.FromArgb(91, 91, 91),       // ContextMenuImageColumn - Context menu margin
                                                                      Color.FromArgb( 70,  70,  70),    // AppButtonBack1
                                                                      Color.FromArgb( 70,  70,  70),    // AppButtonBack2
                                                                      Color.FromArgb( 50,  50,  50),    // AppButtonBorder
                                                                      Color.FromArgb( 70,  70,  70),    // AppButtonOuter1
                                                                      Color.FromArgb( 70,  70,  70),    // AppButtonOuter2
                                                                      Color.FromArgb( 70,  70,  70),    // AppButtonOuter3
                                                                      Color.Empty,                      // AppButtonInner1
                                                                      Color.FromArgb( 50,  50,  50),    // AppButtonInner2
                                                                      Color.FromArgb(32, 32, 32),                      // AppButtonMenuDocs
                                                                      Color.Black,                      // AppButtonMenuDocsText
                                                                      Color.FromArgb(172, 172, 172),    // SeparatorHighInternalBorder1
                                                                      Color.FromArgb(111, 111, 111),    // SeparatorHighInternalBorder2
                                                                      Color.FromArgb(132, 132, 132),    // RibbonGalleryBorder
                                                                      Color.FromArgb(99, 99, 99),    // RibbonGalleryBackNormal
                                                                      Color.FromArgb(193, 193, 193),    // RibbonGalleryBackTracking
                                                                      Color.FromArgb(99, 99, 99),    // RibbonGalleryBack1
                                                                      Color.FromArgb(150, 150, 150),    // RibbonGalleryBack2
                                                                      Color.FromArgb(148, 149, 151),    // RibbonTabTracking3
                                                                      Color.FromArgb(127, 127, 127),    // RibbonTabTracking4
                                                                      Color.FromArgb( 82,  82,  82),    // RibbonGroupBorder3
                                                                      Color.FromArgb(176, 176, 176),    // RibbonGroupBorder4
                                                                      Color.FromArgb(178, 178, 178),    // RibbonGroupBorder5
                                                                      Color.White,                                   // RibbonGroupTitleText - Ribbon group text colour
                                                                      Color.FromArgb(155, 157, 160),    // RibbonDropArrowLight
                                                                      Color.FromArgb( 27,  29,  40),    // RibbonDropArrowDark
                                                                      Color.FromArgb(137, 137, 137),    // HeaderDockInactiveBack1
                                                                      Color.FromArgb(125, 125, 125),    // HeaderDockInactiveBack2
                                                                      Color.FromArgb( 46,  46,  46),    // ButtonNavigatorBorder
                                                                      Color.White,                                   // ButtonNavigatorText
                                                                      Color.FromArgb( 76,  76,  76),    // ButtonNavigatorTrack1
                                                                      Color.FromArgb(147, 147, 143),    // ButtonNavigatorTrack2
                                                                      Color.FromArgb( 66,  66,  66),    // ButtonNavigatorPressed1
                                                                      Color.FromArgb(148, 148, 143),    // ButtonNavigatorPressed2
                                                                      Color.FromArgb( 91,  91,  91),    // ButtonNavigatorChecked1
                                                                      Color.FromArgb( 73,  73,  73),    // ButtonNavigatorChecked2
                                                                      Color.FromArgb(201, 201, 201) // ToolTipBottom                                                                      
        };
        #endregion

        #region Constructors
        static PaletteOffice365BlackDarkMode()
        {
            _checkBoxList = new ImageList
            {
                ImageSize = new Size(13, 13),
                ColorDepth = ColorDepth.Depth24Bit
            };

            _checkBoxList.Images.AddStrip(CheckBoxStripResources.CheckBoxStrip2010Black);

            _galleryButtonList = new ImageList
            {
                ImageSize = new Size(13, 7),
                ColorDepth = ColorDepth.Depth24Bit,
                TransparentColor = Color.Magenta
            };

            _galleryButtonList.Images.AddStrip(GalleryImageResources.Gallery2010);

            _radioButtonArray = new Image[]{Office2010BlueRadioButtonResources.RadioButton2010BlueD, 
                                            Office2010SilverRadioButtonResources.RadioButton2010SilverN, 
                                            Office2010BlueRadioButtonResources.RadioButton2010BlueT,
                                            Office2010BlueRadioButtonResources.RadioButton2010BlueP, 
                                            Office2010BlueRadioButtonResources.RadioButton2010BlueDC, 
                                            Office2010SilverRadioButtonResources.RadioButton2010SilverNC,
                                            Office2010SilverRadioButtonResources.RadioButton2010SilverTC, 
                                            Office2010SilverRadioButtonResources.RadioButton2010SilverPC };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaletteOffice365Black"/> class.
        /// </summary>
        public PaletteOffice365BlackDarkMode() : base(_schemeColors, _checkBoxList, _galleryButtonList, _radioButtonArray, _trackBarColors)
        {

        }
        #endregion

        #region Images        
        /// <summary>
        /// Gets a drop down button image appropriate for the provided state.
        /// </summary>
        /// <param name="state">PaletteState for which image is required.</param>
        /// <returns></returns>
        public override Image GetDropDownButtonImage(PaletteState state) => state != PaletteState.Disabled ? _blackDropDownButton : base.GetDropDownButtonImage(state);

        /// <summary>
        /// Gets an image indicating a sub-menu on a context menu item.
        /// </summary>
        /// <returns>
        /// Appropriate image for drawing; otherwise null.
        /// </returns>
        public override Image GetContextMenuSubMenuImage() => _contextMenuSubMenu;

        #endregion

        #region ButtonSpec        
        /// <summary>
        /// Gets the image to display for the button.
        /// </summary>
        /// <param name="style">Style of button spec.</param>
        /// <param name="state">State for which image is required.</param>
        /// <returns>
        /// Image value.
        /// </returns>
        public override Image GetButtonSpecImage(PaletteButtonSpecStyle style, PaletteState state)
        {
            return style switch
            {
                PaletteButtonSpecStyle.PendantClose => _buttonSpecPendantClose,
                PaletteButtonSpecStyle.PendantMin => _buttonSpecPendantMin,
                PaletteButtonSpecStyle.PendantRestore => _buttonSpecPendantRestore,
                PaletteButtonSpecStyle.FormClose => state switch
                {
                    PaletteState.Tracking or PaletteState.Pressed => _formCloseNormal,
                    _ => _formCloseDisabled
                },
                PaletteButtonSpecStyle.FormMin => state switch
                {
                    PaletteState.Normal => _formMinimiseNormal,
                    PaletteState.Tracking => _formMinimiseHover,
                    _ => _formMinimiseDisabled
                },
                PaletteButtonSpecStyle.FormMax => _formMaximiseNormal,
                PaletteButtonSpecStyle.FormRestore => _formRestoreNormal,
                PaletteButtonSpecStyle.FormHelp => state switch
                {
                    PaletteState.Tracking => _formHelpHover,
                    PaletteState.Normal => _formHelpNormal,
                    _ => _formHelpDisabled
                },
                PaletteButtonSpecStyle.RibbonMinimize => _buttonSpecRibbonMinimize,
                PaletteButtonSpecStyle.RibbonExpand => _buttonSpecRibbonExpand,
                _ => base.GetButtonSpecImage(style, state)
            };
        }
        #endregion
    }
}
