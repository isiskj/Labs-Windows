// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Markdig.Syntax.Inlines;
using Windows.UI.Text;

namespace CommunityToolkit.Labs.WinUI.MarkdownTextBlock.TextElements;

internal class EmphasisInlineElement : ITextElement
{
    private Span _span;
    private EmphasisInline _markdownObject;

    private bool _isBold;
    private bool _isItalic;
    private bool _isStrikeThrough;

    public TextElement TextElement
    {
        get => _span;
    }

    public EmphasisInlineElement(EmphasisInline emphasisInline)
    {
        _span = new Span();
        _markdownObject = emphasisInline;
    }

    public void AddChild(ITextElement child)
    {
        try
        {
            if (child is InlineTextElement inlineText)
            {
                _span.Inlines.Add((Run)inlineText.TextElement);
            }
            else if (child is EmphasisInlineElement emphasisInline)
            {
                if (emphasisInline._isBold) { SetBold(); }
                if (emphasisInline._isItalic) { SetItalic(); }
                if (emphasisInline._isStrikeThrough) { SetStrikeThrough(); }
                _span.Inlines.Add(emphasisInline._span);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error in {nameof(EmphasisInlineElement)}.{nameof(AddChild)}: {ex.Message}");
        }
    }

    public void SetBold()
    {
        #if WINUI3
        _span.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
        #elif WINUI2
        _span.FontWeight = Windows.UI.Text.FontWeights.Bold;
        #endif

        _isBold = true;
    }

    public void SetItalic()
    {
        _span.FontStyle = FontStyle.Italic;
        _isItalic = true;
    }

    public void SetStrikeThrough()
    {
        #if WINUI3
        _span.TextDecorations = Windows.UI.Text.TextDecorations.Strikethrough;
        #elif WINUI2
        _span.TextDecorations = Windows.UI.Text.TextDecorations.Strikethrough;
        #endif

        _isStrikeThrough = true;
    }

    public void SetSubscript()
    {
        _span.SetValue(Typography.VariantsProperty, FontVariants.Subscript);
    }

    public void SetSuperscript()
    {
        _span.SetValue(Typography.VariantsProperty, FontVariants.Superscript);
    }
}