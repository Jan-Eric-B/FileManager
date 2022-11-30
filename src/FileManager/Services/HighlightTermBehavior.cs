using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace FileManager.Services
{
    public static class HighlightTermBehavior
    {
        public static readonly DependencyProperty TermToBeHighlightedProperty = DependencyProperty.RegisterAttached(
            "TermToBeHighlighted",
            typeof(string),
            typeof(HighlightTermBehavior),
            new FrameworkPropertyMetadata("", OnTextChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
                    "Text",
            typeof(string),
            typeof(HighlightTermBehavior),
            new FrameworkPropertyMetadata("", OnTextChanged));

        public static string GetTermToBeHighlighted(FrameworkElement frameworkElement)
        {
            return (string)frameworkElement.GetValue(TermToBeHighlightedProperty);
        }

        public static string GetText(FrameworkElement frameworkElement) => (string)frameworkElement.GetValue(TextProperty);

        public static void SetTermToBeHighlighted(FrameworkElement frameworkElement, string value)
        {
            frameworkElement.SetValue(TermToBeHighlightedProperty, value);
        }

        public static void SetText(FrameworkElement frameworkElement, string value) => frameworkElement.SetValue(TextProperty, value);

        public static List<string> SplitTextIntoTermAndNotTermParts(string text, string term)
        {
            return string.IsNullOrEmpty(text)
                ? new List<string>() { string.Empty }
                : Regex.Split(text, $"({Regex.Escape(term)})")
                        .Where(p => p != string.Empty)
                        .ToList();
        }

        private static void AddHighlightedPartToTextBlock(TextBlock textBlock, string part)
        {
            textBlock.Inlines.Add(new Run { Text = part, Background = new SolidColorBrush(WIndowsAccentColorsService.AccentLight1()) });
        }

        private static void AddPartToTextBlock(TextBlock textBlock, string part)
        {
            textBlock.Inlines.Add(new Run { Text = part });
        }

        private static void AddPartToTextBlockAndHighlightIfNecessary(TextBlock textBlock, string termToBeHighlighted, string textPart)
        {
            if (textPart == termToBeHighlighted)
            {
                AddHighlightedPartToTextBlock(textBlock, textPart);
            }
            else
            {
                AddPartToTextBlock(textBlock, textPart);
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                SetTextBlockTextAndHighlightTerm(textBlock, GetText(textBlock), GetTermToBeHighlighted(textBlock));
            }
        }

        private static void SetTextBlockTextAndHighlightTerm(TextBlock textBlock, string text, string termToBeHighlighted)
        {
            textBlock.Text = string.Empty;

            if (TextIsEmpty(text))
            {
                return;
            }

            if (TextIsNotContainingTermToBeHighlighted(text, termToBeHighlighted))
            {
                AddPartToTextBlock(textBlock, text);
                return;
            }

            foreach (string textPart in SplitTextIntoTermAndNotTermParts(text, termToBeHighlighted))
            {
                AddPartToTextBlockAndHighlightIfNecessary(textBlock, termToBeHighlighted, textPart);
            }
        }

        private static bool TextIsEmpty(string text)
        {
            return text.Length == 0;
        }

        private static bool TextIsNotContainingTermToBeHighlighted(string text, string termToBeHighlighted)
        {
            return !text.Contains(termToBeHighlighted, StringComparison.Ordinal);
        }
    }
}