// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Markdig.Syntax.Inlines;
using CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements;

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.Renderers.ObjectRenderers.Inlines;

internal class EmphasisInlineRenderer : UWPObjectRenderer<EmphasisInline>
{
    protected override void Write(WinUIRenderer renderer, EmphasisInline obj)
    {
        if (renderer == null) throw new ArgumentNullException(nameof(renderer));
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        EmphasisInlineElement? span = null;

        switch (obj.DelimiterChar)
        {
            case '*':
            case '_':
                span = new EmphasisInlineElement(obj);
                if (obj.DelimiterCount == 2) { span.SetBold(); } else { span.SetItalic(); }
                break;
            case '~':
                span = new EmphasisInlineElement(obj);
                if (obj.DelimiterCount == 2) { span.SetStrikeThrough(); } else { span.SetSubscript(); }
                break;
            case '^':
                span = new EmphasisInlineElement(obj);
                span.SetSuperscript();
                break;
        }

        if (span != null)
        {
            renderer.Push(span);
            renderer.WriteChildren(obj);
            renderer.Pop();
        }
        else
        {
            renderer.WriteChildren(obj);
        }
    }
}
