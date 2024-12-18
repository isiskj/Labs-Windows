// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using HtmlAgilityPack;
using Markdig.Syntax.Inlines;

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements;

internal class HyperlinkElement : ITextElement
{
    private Hyperlink _hyperlink;
    private LinkInline? _linkInline;
    private HtmlNode? _htmlNode;
    private string? _baseUrl;

    public bool IsHtml => _htmlNode != null;

    public TextElement TextElement
    {
        get => _hyperlink;
    }

    public HyperlinkElement(LinkInline linkInline, string? baseUrl)
    {
        _baseUrl = baseUrl;
        var url = linkInline.GetDynamicUrl != null ? linkInline.GetDynamicUrl() ?? linkInline.Url : linkInline.Url;
        _linkInline = linkInline;
        _hyperlink = new Hyperlink()
        {
            NavigateUri = Extensions.GetUri(url, baseUrl),
        };
    }

    public HyperlinkElement(HtmlNode htmlNode, string? baseUrl)
    {
        _baseUrl = baseUrl;
        var url = htmlNode.GetAttributeValue("href", "#");
        _htmlNode = htmlNode;
        _hyperlink = new Hyperlink()
        {
            NavigateUri = Extensions.GetUri(url, baseUrl),
        };
    }

    public void AddChild(ITextElement child)
    {
#if !WINAPPSDK
        if (child.TextElement is Windows.UI.Xaml.Documents.Inline inlineChild)
        {
            try
            {
                _hyperlink.Inlines.Add(inlineChild);
                // TODO: Add support for click handler
            }
            catch { }
        }
#else
        if (child.TextElement is Microsoft.UI.Xaml.Documents.Inline inlineChild)
        {
            try
            {
                _hyperlink.Inlines.Add(inlineChild);
                // TODO: Add support for click handler
            }
            catch { }
        }
#endif
    }
}
