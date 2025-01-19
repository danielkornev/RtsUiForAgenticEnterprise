using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MdXaml;
using Mscc.GenerativeAI;
using MutliModalLiveCopilotWpfApp.Common;
using MutliModalLiveCopilotWpfApp.ViewModels;

namespace MutliModalLiveCopilotWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string _geminiApiKey = "AIzaSyBIiz2RsfBWaRo5le9R68PX06cvIy6Ricw";
        Markdown engine = new Markdown();

        private Point _startPoint;

        ObservableCollection<ShellItem> _items = new ObservableCollection<ShellItem>();

        ObservableCollection<List<Tuple<string, List<Message>>>> agentMessageHistory = new ObservableCollection<List<Tuple<string, List<Message>>>>();

        public MainWindow()
        {
            InitializeComponent();

            this.inputTextBox.KeyDown += InputTextBox_KeyDown;

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // maximizing the window
            this.WindowState = WindowState.Maximized;

            // changing style to None
            // this.WindowStyle = WindowStyle.None;

            // databinding
            this.richCanvas.ItemsSource = _items;

            // item templates
            var itemTemplates =
                new ObservableCollection<TemplateItem>();

            // adding item templates
            itemTemplates.Add(new TemplateItem()
            {
                Title = "Market Researcher",
                Description = "Description",
                Thumbnail = "https://via.placeholder.com/150",
                TemplateType = "TitleDescription"
            });

            itemTemplates.Add(new TemplateItem()
            {
                Title = "Market Researcher",
                Description = "Description",
                Thumbnail = "https://via.placeholder.com/150",
                TemplateType = "TitleDescription"
            });

            this.agentTemplates.ItemsSource = itemTemplates;
        }

        private void OnScrolling(object sender, RoutedEventArgs e)
        {
            //AttachedAdorner.OnScrolling();
        }

        // help me write code that if user entered "enter" button while typing in inputTextBox, would process the input
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Generate_Content_with_GoogleSearch_ResponseModalities(inputTextBox.Text);

                inputTextBox.Text = string.Empty;
            }
        }

        private void inputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (inputTextBox.Text.Length>0)
            {
                sendMessageButton.IsEnabled = true;
            }
            else
            {
                sendMessageButton.IsEnabled = false;
            }
        }

        // Ref: https://ai.google.dev/gemini-api/docs/models/gemini-v2#search-tool
        public async Task Generate_Content_with_GoogleSearch_ResponseModalities(string prompt)
        {
            // Arrange
            //var prompt = "When is the next total solar eclipse in Mauritius?";
            var genAi = new GoogleAI(_geminiApiKey);
            var model = genAi.GenerativeModel(Model.Gemini20FlashExperimental);
            model.UseGoogleSearch = true;
            

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: new GenerationConfig()
                {
                    ResponseModalities = [ResponseModality.Text]
                });

            // ok, great. Can we only show text in chatbot, but show structured data outside of it?


            var responseText = string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
                    //                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray());

            // write check if responeText is actually markdown 



            if (responseText.StartsWith("{"))
            {
                var structuredData = responseText;
                //this.debugMessagesListBox.Items.Add(structuredData);
            }

            else
            {
                // we suppose that anything coming is markdown
                FlowDocument document = engine.Transform(responseText);

                this.debugMessagesListBox.Items.Add(document);
            }

            this.debugMessagesListBox.Items.Add(responseText);

            //response.Candidates![0].GroundingMetadata!.GroundingChunks?
            //    .ForEach(c =>
            //        debugMessagesListBox.Items.Add($"{c!.Web!.Title} - {c!.Web!.Uri}"));

            //this.debugMessagesListBox.Items.Add(string.Join(Environment.NewLine,
            //    response.Candidates![0].GroundingMetadata!.WebSearchQueries!
            //        .Select(w => w)
            //        .ToArray()));

            //

            //this.debugMessagesListBox.Items.Add(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        // help me write code that would open or hide the copilot pane using Translation animation when user clicks on the button. take into account that I need to have Translation Animation (horizontal) and disappearance/appearance
        private void openOrHideCopilotPaneButton_Click(object sender, RoutedEventArgs e)
        {
            if (copilotPane.Visibility == Visibility.Visible)
            {
                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = -300,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                copilotPane.BeginAnimation(TranslateTransform.XProperty, animation);
                copilotPane.Visibility = Visibility.Collapsed;
            }
            else
            {
                var animation = new DoubleAnimation
                {
                    From = -300,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5)
                };
                copilotPane.BeginAnimation(TranslateTransform.XProperty, animation);
                copilotPane.Visibility = Visibility.Visible;
            }
        }

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            _startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null)
                    return;

                // Find the data behind the ListViewItem
                object data = listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                // Initialize the drag-and-drop operation
                DragDrop.DoDragDrop(listViewItem, data, DragDropEffects.Move);
            }
        }

        private void richCanvas_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TemplateItem)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void richCanvas_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TemplateItem)) || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void richCanvas_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TemplateItem)))
            {
                TemplateItem data = (TemplateItem)e.Data.GetData(typeof(TemplateItem));

                // Handle the drop action (e.g., add the data to the target surface)
                // Example: Add a new UI element to the Grid
                Grid dropTarget = sender as Grid;
                // Add your logic to handle the dropped data

                // Get the drop position relative to the RichCanvas
                Point dropPosition = e.GetPosition(richCanvas);

                _items.Add(new ShellItem()
                {
                    Title = data.Title,
                    Description = data.Description,
                    Thumbnail = data.Thumbnail,
                    Left = dropPosition.X,
                    Top = dropPosition.Y
                });
            }
        }


        #region Helpers
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
        #endregion
    } // class 
} // namespace