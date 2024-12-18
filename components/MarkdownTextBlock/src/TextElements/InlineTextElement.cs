// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements;

internal class InlineTextElement : ITextElement
{
    private Run _run;

    public TextElement TextElement
    {
        get => _run;
    }

    public InlineTextElement(string text)
    {
        _run = new Run()
        {
            Text = text
        };
    }

    public void AddChild(ITextElement child) {}
}
