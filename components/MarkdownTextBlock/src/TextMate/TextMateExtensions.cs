// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.Controls.MarkdownTextBlockRns.TextMate;
internal static class TextMateExtensions
{
    internal static string SubstringAtIndexes(this string str, int startIndex, int endIndex)
    {
        return str.Substring(startIndex, endIndex - startIndex);
    }

    internal static SolidColorBrush GetSolidColorBrush(this string hex)
    {
        hex = hex.Replace("#", string.Empty);

        byte a = 255;
        int index = 0;

        if (hex.Length == 8)
        {
            a = (byte)(Convert.ToUInt32(hex.Substring(index, 2), 16));
            index += 2;
        }

        byte r = (byte)(Convert.ToUInt32(hex.Substring(index, 2), 16));
        index += 2;
        byte g = (byte)(Convert.ToUInt32(hex.Substring(index, 2), 16));
        index += 2;
        byte b = (byte)(Convert.ToUInt32(hex.Substring(index, 2), 16));
        SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
        return myBrush;
    }
}
