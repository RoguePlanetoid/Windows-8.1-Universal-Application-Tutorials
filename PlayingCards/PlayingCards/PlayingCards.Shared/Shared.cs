using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

public class Shared
{
    private const string clubs = "♣";
    private const string diamonds = "♦";
    private const string hearts = "♥";
    private const string spades = "♠";
    private const string ace = "A";
    private const string jack = "J";
    private const string queen = "Q";
    private const string king = "K";

    private List<int> deckOne = new List<int>();
    private List<int> deckTwo = new List<int>();
    private int cardOne, cardTwo;
    private int first, second;
    private int score, counter;
    private Random random = new Random((int)DateTime.Now.Ticks);

    public void Show(string content, string title)
    {
        try
        {
            IAsyncOperation<IUICommand> command = new MessageDialog(content, title).ShowAsync();
        }
        catch
        {

        }
    }

    private TextBlock pip(string content, Color foreground, double fontSize, string fontFamily, double left, double top)
    {
        TextBlock pip = new TextBlock();
        pip.FontSize = fontSize;
        pip.FontFamily = new FontFamily(fontFamily);
        pip.Text = content;
        pip.Foreground = new SolidColorBrush(foreground);
        pip.SetValue(Canvas.LeftProperty, left);
        pip.SetValue(Canvas.TopProperty, top);
        return pip;
    }

    private Canvas deck(ref int card, ref Color back)
    {
        Canvas canvas = new Canvas();
        Color foreground = Colors.Black;
        string suite = "";
        string display = "";
        int value;
        if (card >= 1 && card <= 13)
        {
            foreground = Colors.Black;
            suite = clubs;
            display = card.ToString();
        }
        else if (card >= 14 && card <= 26)
        {
            foreground = Colors.Red;
            suite = diamonds;
            display = (card - 13).ToString();
        }
        else if (card >= 27 && card <= 39)
        {
            foreground = Colors.Red;
            suite = hearts;
            display = (card - 26).ToString();
        }
        else if (card >= 40 && card <= 52)
        {
            foreground = Colors.Black;
            suite = spades;
            display = (card - 39).ToString();
        }
        value = int.Parse(display);
        canvas.Width = 170;
        canvas.Height = 250;
        canvas.Background = new SolidColorBrush(back);
        switch (value)
        {
            case 1:
                display = ace;
                canvas.Children.Add(pip(suite, foreground, 100, "Times New Roman", 50, 60)); // Centre
                break;
            case 2:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 20)); // Middle Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 160)); // Middle Bottom
                break;
            case 3:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 20)); // Middle Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 90)); // Centre
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 160)); // Middle Bottom
                break;
            case 4:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 20)); // Top Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 20)); // Top Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 160)); // Bottom Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 160)); // Bottom Right
                break;
            case 5:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 20)); // Top Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 20)); // Top Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 90)); // Centre
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 160)); // Bottom Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 160)); // Bottom Right
                break;
            case 6:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 20)); // Top Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 20)); // Top Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 90)); // Centre Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 90)); // Centre Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 160)); // Bottom Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 160)); // Bottom Right
                break;
            case 7:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 20)); // Top Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 20)); // Top Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 90)); // Centre Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 55)); // Centre Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 90)); // Centre Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 160)); // Bottom Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 160)); // Bottom Right
                break;
            case 8:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 20)); // Top Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 20)); // Top Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 90)); // Centre Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 55)); // Centre Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 125)); // Centre Bottom
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 90)); // Centre Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 160)); // Bottom Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 160)); // Bottom Right
                break;
            case 9:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 20)); // Top Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 20)); // Top Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 65)); // Centre Left Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 110)); // Centre Left Bottom
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 90)); // Centre
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 65)); // Centre Right Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 110)); // Centre Right Bottom
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 160)); // Bottom Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 160)); // Bottom Right
                break;
            case 10:
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 20)); // Top Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 20)); // Top Right
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 65)); // Centre Left Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 110)); // Centre Left Bottom
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 55)); // Centre Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 65, 125)); // Centre Bottom
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 65)); // Centre Right Top
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 110)); // Centre Right Bottom
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 20, 160)); // Bottom Left
                canvas.Children.Add(pip(suite, foreground, 60, "Arial", 110, 160)); // Bottom Right
                break;
            case 11:
                display = jack;
                canvas.Children.Add(pip(display, foreground, 100, "Times New Roman", 65, 65)); // Centre
                break;
            case 12:
                display = queen;
                canvas.Children.Add(pip(display, foreground, 100, "Times New Roman", 50, 65)); // Centre
                break;
            case 13:
                display = king;
                canvas.Children.Add(pip(display, foreground, 100, "Times New Roman", 50, 65)); // Centre
                break;
        }
        if ((display.Length == 1))
        {
            canvas.Children.Add(pip(display, foreground, 20, "Times New Roman", 10, 4));
            canvas.Children.Add(pip(display, foreground, 20, "Times New Roman", 150, 204));
        }
        else
        {
            canvas.Children.Add(pip(display, foreground, 20, "Times New Roman", 5, 4));
            canvas.Children.Add(pip(display, foreground, 20, "Times New Roman", 145, 204));
        }
        canvas.Children.Add(pip(suite, foreground, 30, "Arial", 7, 16));
        canvas.Children.Add(pip(suite, foreground, 30, "Arial", 145, 215));
        return canvas;
    }

    private Canvas card(int value, bool facing, Color back)
    {
        if (facing)
        {
            return deck(ref value, ref back);
        }
        else
        {
            Canvas card = new Canvas();
            card.Width = 170;
            card.Height = 250;
            card.Background = new SolidColorBrush(back);
            return card;
        }
    }

    private void match(ref Canvas pileone, ref Canvas piletwo)
    {
        if ((cardOne < 52) && (cardTwo < 52))
        {
            first = deckOne[cardOne];
            pileone.Children.Clear();
            pileone.Children.Add(card(first, true, Colors.White));
            cardOne++;
            second = deckTwo[cardTwo];
            piletwo.Children.Clear();
            piletwo.Children.Add(card(second, true, Colors.White));
            cardTwo++;
            if ((first % 13) == (second % 13)) // Ignore Suite for Match
            {
                score++;
                Show("Match!", "Playing Cards");
            }
            counter++;
        }
        else
        {
            Show("Game Over! Matched " + score + " out of " + counter + " cards!", "Playing Cards");
        }
    }

    private List<int> select()
    {
        int number;
        List<int> numbers = new List<int>();
        while ((numbers.Count < 52)) // Select 52 Numbers
        {
            number = random.Next(1, 53); // Seeded Random Number
            if ((!numbers.Contains(number)) || (numbers.Count < 1))
            {
                numbers.Add(number); // Add if number Chosen or None
            }
        }
        return numbers;
    }

    public void New(Canvas pileone, Canvas piletwo)
    {
        score = 0;
        counter = 0;
        cardOne = 0;
        cardTwo = 0;
        deckOne = select();
        deckTwo = select();
        pileone.Children.Clear();
        pileone.Children.Add(card(1, false, Colors.Red));
        pileone.Tapped += (object sender, TappedRoutedEventArgs e) =>
        {
            if (((Canvas)sender).Children.Count > 0)
            {
                match(ref pileone, ref piletwo);
            }
        };
        piletwo.Children.Clear();
        piletwo.Children.Add(card(1, false, Colors.Blue));
        piletwo.Tapped += (object sender, TappedRoutedEventArgs e) =>
        {
            if (((Canvas)sender).Children.Count > 0)
            {
                match(ref pileone, ref piletwo);
            }
        };
    }
}
