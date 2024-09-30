using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LocalCode
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("637b48db-e25d-4cb8-9912-4d3805e929df")]
    public class CodeWithLocalLLMOptionsPage : DialogPage
    {
        private string apiUrl = "http://localhost:11434";
        private string authToken = "ollama";

        [Category("Code with Local AI Settings")]
        [DisplayName("Ollama URL")]
        [Description("The URL of your local OLLAMA API endpoint.")]
        public string ApiUrl
        {
            get { return apiUrl; }
            set { apiUrl = value; }
        }

        [Category("Code with Local AI Settings")]
        [DisplayName("Authentication Token")]
        [Description("The authentication token for API requests. This is not really used with local LLM")]
        public string AuthToken
        {
            get { return authToken; }
            set { authToken = value; }
        }
    }
}
