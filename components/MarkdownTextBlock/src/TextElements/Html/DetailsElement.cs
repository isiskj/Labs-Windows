// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using HtmlAgilityPack;
using Microsoft.UI.Xaml.Controls;

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements.Html;

// block
internal class DetailsElement : ITextElement
{
    private HtmlNode _htmlNode;
    private InlineUIContainer _inlineUIContainer;
    private Expander _expander;
    private FlowDocumentElement _flowDocument;
    private Paragraph _paragraph;

    public TextElement TextElement
    {
        get => _paragraph;
    }

    public DetailsElement(HtmlNode details)
    {
        _htmlNode = details;

        var header = _htmlNode.ChildNodes
            .FirstOrDefault(
                x => x.Name == "summary" ||
                x.Name == "header");

        _inlineUIContainer = new InlineUIContainer();
        _expander = new Expander();
        _expander.HorizontalAlignment = HorizontalAlignment.Stretch;
        _flowDocument = new FlowDocumentElement(details);
        _flowDocument.RichTextBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
        _expander.Content = _flowDocument.RichTextBlock;
        var headerBlock = new TextBlock()
        {
            Text = header?.InnerText
        };
        headerBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
        _expander.Header = headerBlock;
        _inlineUIContainer.Child = _expander;
        _paragraph = new Paragraph();
        _paragraph.Inlines.Add(_inlineUIContainer);
    }

    public void AddChild(ITextElement child)
    {
        _flowDocument.AddChild(child);
    }
}
