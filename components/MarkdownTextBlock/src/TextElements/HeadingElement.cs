// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using HtmlAgilityPack;
using Markdig.Syntax;

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements;

internal class HeadingElement : ITextElement
{
    private Paragraph _paragraph;
    private HeadingBlock? _headingBlock;
    private HtmlNode? _htmlNode;
    private MarkdownConfig _config;

    public bool IsHtml => _htmlNode != null;

    public TextElement TextElement
    {
        get => _paragraph;
    }

    public HeadingElement(HeadingBlock headingBlock, MarkdownConfig config)
    {
        _headingBlock = headingBlock;
        _paragraph = new Paragraph();
        _config = config;

        var level = headingBlock.Level;
        _paragraph.FontSize = level switch
        {
            1 => _config.Themes.H1FontSize,
            2 => _config.Themes.H2FontSize,
            3 => _config.Themes.H3FontSize,
            4 => _config.Themes.H4FontSize,
            5 => _config.Themes.H5FontSize,
            _ => _config.Themes.H6FontSize,
        };
        _paragraph.Foreground = _config.Themes.HeadingForeground;
        _paragraph.FontWeight = level switch
        {
            1 => _config.Themes.H1FontWeight,
            2 => _config.Themes.H2FontWeight,
            3 => _config.Themes.H3FontWeight,
            4 => _config.Themes.H4FontWeight,
            5 => _config.Themes.H5FontWeight,
            _ => _config.Themes.H6FontWeight,
        };
    }

    public HeadingElement(HtmlNode htmlNode, MarkdownConfig config)
    {
        _htmlNode = htmlNode;
        _paragraph = new Paragraph();
        _config = config;

        var align = _htmlNode.GetAttributeValue("align", "left");
        _paragraph.TextAlignment = align switch
        {
            "left" => TextAlignment.Left,
            "right" => TextAlignment.Right,
            "center" => TextAlignment.Center,
            "justify" => TextAlignment.Justify,
            _ => TextAlignment.Left,
        };

        var level = int.Parse(htmlNode.Name.Substring(1));
        _paragraph.FontSize = level switch
        {
            1 => _config.Themes.H1FontSize,
            2 => _config.Themes.H2FontSize,
            3 => _config.Themes.H3FontSize,
            4 => _config.Themes.H4FontSize,
            5 => _config.Themes.H5FontSize,
            _ => _config.Themes.H6FontSize,
        };
        _paragraph.Foreground = _config.Themes.HeadingForeground;
        _paragraph.FontWeight = level switch
        {
            1 => _config.Themes.H1FontWeight,
            2 => _config.Themes.H2FontWeight,
            3 => _config.Themes.H3FontWeight,
            4 => _config.Themes.H4FontWeight,
            5 => _config.Themes.H5FontWeight,
            _ => _config.Themes.H6FontWeight,
        };
    }

    public void AddChild(ITextElement child)
    {
        if (child.TextElement is Inline inlineChild)
        {
            _paragraph.Inlines.Add(inlineChild);
        }
    }
}
