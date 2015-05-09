using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

public class Shared
{
    private Random random = new Random((int)DateTime.Now.Ticks);

    private List<int> numbers()
    {
        int number;
        List<int> numbers = new List<int>();
        while ((numbers.Count < 6)) // Select 6 Numbers
        {
            number = random.Next(1, 50); // Random Number 1 - 49
            if ((!numbers.Contains(number)) || (numbers.Count < 1))
            {
                numbers.Add(number); // Add if number not Chosen or None selected
            }
        }
        return numbers;
    }

    public void Draw(ref StackPanel Stack)
    {
        Stack.Children.Clear();
        foreach (int number in numbers())
        {
            Canvas container = new Canvas();
            Ellipse ball = new Ellipse();
            TextBlock text = new TextBlock();
            container.Margin = new Thickness(2);
            container.Width = 62;
            container.Height = 62;
            ball.Width = container.Width;
            ball.Height = container.Height;
            if (number >= 1 && number <= 9)
            {
                ball.Fill = new SolidColorBrush(Colors.White);
            }
            else if (number >= 10 && number <= 19)
            {
                // Sky Blue
                ball.Fill = new SolidColorBrush(Color.FromArgb(255, 112, 200, 236));
            }
            else if (number >= 20 && number <= 29)
            {
                ball.Fill = new SolidColorBrush(Colors.Magenta);
            }
            else if (number >= 30 && number <= 39)
            {
                // Lawn Green
                ball.Fill = new SolidColorBrush(Color.FromArgb(255, 112, 255, 0));
            }
            else if (number >= 40 && number <= 49)
            {
                ball.Fill = new SolidColorBrush(Colors.Yellow);
            }
            container.Children.Add(ball);
            text.Foreground = new SolidColorBrush(Colors.Black);
            text.FontSize = 24;
            text.Text = number.ToString();
            text.Margin = new Thickness(18);
            container.Children.Add(text);
            Stack.Children.Add(container);
        }
    }
}
