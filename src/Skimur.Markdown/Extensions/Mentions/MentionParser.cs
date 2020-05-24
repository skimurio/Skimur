using System;
using Markdig.Helpers;
using Markdig.Parsers;

namespace Skimur.Markdown.Extensions.Mentions
{
    public class MentionParser : InlineParser
    {
        private static readonly char[] _openingCharacters =
        {
            '@'
        };

        public MentionParser()
        {
            this.OpeningCharacters = _openingCharacters;
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            bool matchFound;
            char previous;

            matchFound = false;
            previous = slice.PeekCharExtra(-1);

            if (previous.IsWhiteSpaceOrZero() || previous == '(' || previous == '[')
            {
                char current;
                int start;
                int end;

                slice.NextChar();

                current = slice.CurrentChar;
                start = slice.Start;
                end = start;

                while (current.IsDigit())
                {
                    end = slice.Start;
                    current = slice.NextChar();
                }

                if (current.IsWhiteSpaceOrZero() || current == ')' || current == ']')
                {
                    int inlineStart;

                    inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

                    processor.Inline = new Mention
                    {
                        Span =
                              {
                                Start = inlineStart,
                                End = inlineStart + (end - start) + 1
                              },
                        Line = line,
                        Column = column,
                        Username = new StringSlice(slice.Text, start, end)
                    };

                    matchFound = true;
                }
            }

            return matchFound;
        }

    }
}
