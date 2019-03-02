using System.Collections.Generic;
using System.Text;

namespace CryptoStringAPI.Functions
{
    public class TokenList : List<Token>
    {

        public void Add(string text, string replacement)
        {
            Add(new Token(text, replacement));
        }

        private Token GetFirstToken()
        {
            Token result = null;
            int index = int.MaxValue;
            foreach (Token token in this)
            {
                if (token.Index != -1 && token.Index < index)
                {
                    index = token.Index;
                    result = token;
                }
            }
            return result;
        }

        public string ReplaceWords(string text)
        {
            StringBuilder result = new StringBuilder();
            foreach (Token token in this)
            {
                token.Index = text.IndexOf(token.Text);
            }
            int index = 0;
            Token next;
            while ((next = GetFirstToken()) != null)
            {
                if (index < next.Index)
                {
                    result.Append(text, index, next.Index - index);
                    index = next.Index;
                }
                result.Append(next.Replacement);
                index += next.Text.Length;
                //next.Index = text.IndexOf(next.Text, index);
                foreach (Token token in this)
                {
                    if (token.Index != -1 && token.Index < index)
                    {
                        token.Index = text.IndexOf(token.Text, index);
                    }
                }
            }
            if (index < text.Length)
            {
                result.Append(text, index, text.Length - index);
            }
            return result.ToString();
        }

        public string ReplaceTokens(string text)
        {
            StringBuilder result = new StringBuilder();
            foreach (Token token in this)
            {
                token.Index = text.IndexOf(token.Replacement);
            }
            int index = 0;
            Token next;
            while ((next = GetFirstToken()) != null)
            {
                if (index < next.Index)
                {
                    result.Append(text, index, next.Index - index);
                    index = next.Index;
                }
                result.Append(next.Text);
                index += next.Replacement.Length;
                //next.Index = text.IndexOf(next.Replacement, index);
                foreach (Token token in this)
                {
                    if (token.Index != -1 && token.Index < index)
                    {
                        token.Index = text.IndexOf(token.Replacement, index);
                    }
                }
            }
            if (index < text.Length)
            {
                result.Append(text, index, text.Length - index);
            }
            return result.ToString();
        }
    }
}