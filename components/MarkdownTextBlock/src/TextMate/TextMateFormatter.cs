// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using TextMateSharp.Grammars;
using TextMateSharp.Registry;
using TextMateSharp.Themes;

namespace CommunityToolkit.WinUI.Controls.MarkdownTextBlockRns.TextMate;
public class TextMateFormatter
{
    private IGrammar grammar;
    private Theme theme;
#if !WINAPPSDK
    private FontFamily fontFamily;
#else
    private FontFamily fontFamily;
#endif

    public TextMateFormatter(ThemeName themeName, string extention, FontFamily font)
    {
        fontFamily = font;
        RegistryOptions options = new RegistryOptions(themeName);
        Registry registry = new Registry(options);
        theme = registry.GetTheme();
        grammar = registry.LoadGrammar(options.GetScopeByExtension(extention));
    }

    public void FormatRichTextBlock(List<string> codeText, RichTextBlock textBlock)
    {
        IStateStack? ruleStack = null;
        foreach (string line in codeText)
        {
#if !WINAPPSDK
            var paragraph = new Windows.UI.Xaml.Documents.Paragraph();
#else
            var paragraph = new Microsoft.UI.Xaml.Documents.Paragraph();
#endif

            ITokenizeLineResult result = grammar.TokenizeLine(line, ruleStack, TimeSpan.MaxValue);
            ruleStack = result.RuleStack;
            foreach (IToken token in result.Tokens)
            {
                int startIndex = (token.StartIndex > line.Length) ?
                    line.Length : token.StartIndex;
                int endIndex = (token.EndIndex > line.Length) ?
                    line.Length : token.EndIndex;

                int foreground = -1;

                TextMateSharp.Themes.FontStyle fontStyle = TextMateSharp.Themes.FontStyle.NotSet;

                foreach (var themeRule in theme.Match(token.Scopes))
                {
                    if (foreground == -1 && themeRule.foreground > 0)
                        foreground = themeRule.foreground;

                    if (fontStyle == TextMateSharp.Themes.FontStyle.NotSet && themeRule.fontStyle > 0)
                        fontStyle = themeRule.fontStyle;
                }

                paragraph.Inlines.Add(WriteToken(line.SubstringAtIndexes(startIndex, endIndex), foreground, fontStyle));
            }
            textBlock.Blocks.Add(paragraph);
        }
    }

    private Run WriteToken(string text, int foreground, TextMateSharp.Themes.FontStyle fontStyle)
    {
#if !WINAPPSDK
        Windows.UI.Xaml.Documents.Run run = new Run
        {
            Text = text,
        };
        run.FontFamily = fontFamily;
#else
        Microsoft.UI.Xaml.Documents.Run run = new Run
        {
            Text = text,
        };
        run.FontFamily = fontFamily;
#endif
        if (foreground == -1)
        {
            return run;
        }

        if (fontStyle == TextMateSharp.Themes.FontStyle.Bold)
        {
#if !WINAPPSDK
            run.FontWeight = Windows.UI.Text.FontWeights.Bold;
#else
            run.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
#endif
        }
        if (fontStyle == TextMateSharp.Themes.FontStyle.Italic)
        {
            run.FontStyle = Windows.UI.Text.FontStyle.Italic;
        }

        if (foreground != -1)
        {
            string hexString = theme.GetColor(foreground);
            run.Foreground = hexString.GetSolidColorBrush();
        }
        // BackGround RichTextBlock not Surport;
        return run;
    }
}
