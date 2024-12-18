// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Markdig.Syntax;

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements;

internal class BlockContainerElement : ITextElement
{
    private ContainerBlock _containerBlock;
    private InlineUIContainer _inlineUIContainer;
    private FlowDocumentElement _flowDocument;
    private Paragraph _paragraph;

    public TextElement TextElement
    {
        get => _paragraph;
    }

    public BlockContainerElement(ContainerBlock containerBlock)
    {
        _containerBlock = containerBlock;
        _inlineUIContainer = new InlineUIContainer();
        _flowDocument = new FlowDocumentElement(containerBlock);
        _inlineUIContainer.Child = _flowDocument.RichTextBlock;
        _paragraph = new Paragraph();
        _paragraph.Inlines.Add(_inlineUIContainer);
    }

    public void AddChild(ITextElement child)
    {
        _flowDocument.AddChild(child);
    }
}
