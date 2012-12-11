using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using YAMP;

namespace SL.YAMP
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ParseQueryButtonOnClick(object sender, RoutedEventArgs e)
        {
            var query = QueryTextBox.Text;

            try
            {
                var parser = Parser.Parse(query);
                var value = parser.Execute();

                if (value != null)
                {
                    var queryHeader = new Run
                    {
                        Foreground = new SolidColorBrush(Colors.Black),
                        FontWeight = FontWeights.Bold,
                        Text = "Query:  "
                    };
                    var queryText = new Run { Foreground = new SolidColorBrush(Colors.Green), Text = query };

                    var resultHeader = new Run
                    {
                        Foreground = new SolidColorBrush(Colors.Black),
                        FontWeight = FontWeights.Bold,
                        Text = "Result: "
                    };
                    var resultText = new Run { Foreground = new SolidColorBrush(Colors.Green), Text = value.ToString() };
                    
                    var parserText = new Run { Foreground = new SolidColorBrush(Colors.Blue), Text = parser.ToString() };

                    var paragraph = new Paragraph();
                    paragraph.Inlines.Add(queryHeader);
                    paragraph.Inlines.Add(queryText);
                    paragraph.Inlines.Add(new LineBreak());
                    paragraph.Inlines.Add(resultHeader);
                    paragraph.Inlines.Add(resultText);
                    paragraph.Inlines.Add(new LineBreak());
                    paragraph.Inlines.Add(parserText);

                    ResultTextBox.Blocks.Add(paragraph);

                    QueryTextBox.Text = String.Empty;
                }
            }
            catch (Exception ex)
            {
                var messageText = new Run { Foreground = new SolidColorBrush(Colors.Red), Text = ex.Message };

                var paragraph = new Paragraph();
                paragraph.Inlines.Add(messageText);
                paragraph.Inlines.Add(new LineBreak());

                ResultTextBox.Blocks.Add(paragraph);
            }
        }
    }
}
