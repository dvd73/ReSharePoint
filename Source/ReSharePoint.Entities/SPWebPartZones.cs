using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public class SPWebPartZone
        {
            public string TitleResoureseKey { get; set; }
            public string Title { get; set; }
        }

        public static List<SPWebPartZone> SPWebPartZones = new List<SPWebPartZone>()
        {
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Body", Title = "Body"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Bottom", Title = "Bottom Zone"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_BottomCenter", Title = "Bottom Center"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_BottomLeft", Title = "Bottom Left Zone"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_BottomRight", Title = "Bottom Right Zone"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Center", Title = "Center"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_CenterLeft", Title = "Center Left"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_CenterRight", Title = "Center Right"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Dynamic", Title = "Dynamic Content"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Footer", Title = "Footer"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Header", Title = "Header"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Left", Title = "Left"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_LeftColumn", Title = "Left Column"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_MiddleLeft", Title = "Middle Left Zone"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_MiddleRight", Title = "Middle Right Zone"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Right", Title = "Right"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_RightColumn", Title = "Right Column"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_Top", Title = "Top"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_TopLeft", Title = "Top Left"},
            new SPWebPartZone {TitleResoureseKey = "WebPartZoneTitle_TopRight", Title = "Top Right"}
        };
    }
}
