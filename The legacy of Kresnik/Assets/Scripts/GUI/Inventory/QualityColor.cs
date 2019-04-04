using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Quality { Common, Uncommon, Rare, Epic, Legendary, Unique }
public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        { Quality.Common, "#ffffffff"},
           { Quality.Uncommon, "#56FF00"},
              { Quality.Rare, "#00ABFF"},
                 { Quality.Epic, "#B000FF"},
                    { Quality.Legendary, "#FFD700"},
                       { Quality.Unique, "#FF4D00"}
    };

    public static Dictionary<Quality, string> MyColors
    {
        get
        {
            return colors;
        }
    }
}