using EnvDTE;
using Markdig;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Newtonsoft.Json;
using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using TextSelection = EnvDTE.TextSelection;

namespace LocalCode
{
    /// <summary>
    /// Interaction logic for CodeWithLocalLLMControl.
    /// </summary>
    public partial class CodeWithLocalLLMControl : UserControl
    {
        public static AsyncPackage _package;
        private StringBuilder _markdownBuilder = new StringBuilder();
        OllamaApiClient ollama;
        Chat chat;

        int intGridRow = 1;

        public string MarkdownDoc { get; set; } = string.Empty;

        public async Task InitLocalLLMAsync()
        {
            try
            {
                var optionsPage = (CodeWithLocalLLMOptionsPage)_package.GetDialogPage(typeof(CodeWithLocalLLMOptionsPage));

                // Retrieve the options page from the package
                optionsPage = _package.GetDialogPage(typeof(CodeWithLocalLLMOptionsPage)) as CodeWithLocalLLMOptionsPage;

                // Use the configured URL and authentication token
                string apiUrl = optionsPage.ApiUrl;
                string authToken = optionsPage.AuthToken;

                QueryTextBox.Text = "Ask question here!";
                SubmitButton.IsEnabled = true;
                QueryTextBox.IsEnabled = true;
                cmbModels.IsEnabled = true;

                ollama = new OllamaApiClient(apiUrl);

                chat = new Chat(ollama, "You are an expert software developer engineer");

                var models = await ollama.ListLocalModels();

                if (models.Any())
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    //await this.Dispatcher.InvokeAsync(() =>
                    //{
                    foreach (var m in models)
                    {
                        cmbModels.Items.Add(m.Name);
                    }

                    cmbModels.SelectedIndex = 1;
                    //});
                }
                else
                {
                    var result = MessageBox.Show("Could not find local models, please open command prompt and run following \n `ollama pull [Model Name]`", "Code With Local LLM", System.Windows.MessageBoxButton.YesNo);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeWithLocalLLMControl"/> class.
        /// </summary>
        /// 
        public CodeWithLocalLLMControl()
        {
            this.InitializeComponent();
            InitLocalLLMAsync();
        }

        private async void OnSubmitClickAsync(object sender, RoutedEventArgs e)
        {
            if (SubmitButton.Content.ToString() == "Submit")
            {
                await ExecuteQueryAsync();
            }
        }

        private async void QueryTextBox_KeyDownAsync(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && SubmitButton.Content.ToString() == "Submit")
            {
                await ExecuteQueryAsync();
            }
        }

        private async Task ExecuteQueryAsync()
        {
            SubmitButton.Content = "Generating...";
            SubmitButton.IsEnabled = false;
            ollama.SelectedModel = cmbModels.SelectedItem.ToString();
            chat.Model = cmbModels.SelectedItem.ToString();
            string selectedCode = GetSelectedText();

            string query = QueryTextBox.Text;

            QueryTextBox.Text = string.Empty;

            QueryTextBox.Focus();

            if (string.IsNullOrWhiteSpace(query))
            {
                SubmitButton.Content = "Submit";
                SubmitButton.IsEnabled = true;
                return;
            }

            LLMResponse responseBlock = new LLMResponse();
            responseStackPanel.Margin = new Thickness(10, 30, 10, 10);
            responseStackPanel.Children.Add(responseBlock);


            var askAI = string.Empty;

            if (string.IsNullOrEmpty(selectedCode))
            {
                askAI = query;
            }
            else
            {
                askAI = "In the following context of a code \n" + selectedCode + "\n" + "can you please answer the following question \n" + query;
            }



            await CallApiAsync(askAI, responseBlock);
        }

        private async Task CallApiAsync(string queryToAskAI, LLMResponse responseControl)
        {
            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        await foreach (var stream in chat.Send(queryToAskAI))
                        {
                            await AppendMarkdownAsync(stream, responseControl);
                            //await AppendMarkdownAsync(stream.Response, responseControl);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                        SubmitButton.Content = "Submit";
                        SubmitButton.IsEnabled = true;
                    }
                });
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as API call failures
                await Dispatcher.InvokeAsync(() =>
                {
                    MarkdownDoc += $"\nError: {ex.Message}";
                });
            }
        }

        public async Task AppendMarkdownAsync(string markdownChunk, LLMResponse responseControl)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            responseControl.SetResponseText(markdownChunk);
            responseScrollViewer.ScrollToBottom();
        }

        private string GetSelectedText()
        {
            var dte = Package.GetGlobalService(typeof(DTE)) as DTE;

            var selectedText = dte?.ActiveDocument?.Selection as TextSelection;

            if (selectedText != null)
            {
                return selectedText.Text;
            }

            return string.Empty;
        }

        public class LicenseKeyVarificationResult
        {
            public string result { get; set; }
            public string message { get; set; }
            public string license_key { get; set; }
            public string status { get; set; }
            public string max_allowed_domains { get; set; }
            public string email { get; set; }
            public List<object> registered_domains { get; set; }
            public string date_created { get; set; }
            public string date_renewed { get; set; }
            public string date_expiry { get; set; }
            public string date { get; set; }
            public string product_ref { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string company_name { get; set; }
            public string txn_id { get; set; }
            public string subscr_id { get; set; }

            public string error_code { get; set; }
        }


    }
}