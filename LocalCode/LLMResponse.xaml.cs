using Markdig;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LocalCode
{
    /// <summary>
    /// Interaction logic for LLMResponse.xaml.
    /// </summary>
    [ProvideToolboxControl("LocalCode.LLMResponse", true)]
    public partial class LLMResponse : UserControl
    {
        StringBuilder _markdownBuilder = new StringBuilder();

        public LLMResponse()
        {
            InitializeComponent();
        }

        public void SetResponseText(string responseText)
        {
            try
            {
                _markdownBuilder.Append(responseText);
                Markdownview.Markdown = _markdownBuilder.ToString();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
