using SoftGrid.Core;
using SoftGrid.Views;
using Xamarin.Forms;

namespace SoftGrid.Controls
{
    public partial class LabelSection : ContentView, IXamarinView
    {
        public LabelSection()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(LabelSection), string.Empty);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == TextProperty.PropertyName)
            {
                SectionLabel.Text = Device.RuntimePlatform == Device.iOS ? Text.ToUpperInvariant() : Text;
            }
        }
    }
}