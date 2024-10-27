// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements;

internal class LineBreakElement : ITextElement
{
    private LineBreak _lineBreak;

    public TextElement TextElement
    {
        get => _lineBreak;
    }

    public LineBreakElement()
    {
        _lineBreak = new LineBreak();
    }

    public void AddChild(ITextElement child) {}
}
