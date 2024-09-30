using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace LocalCode
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("1e9495a3-6117-469d-9896-7cd480f9299e")]
    public class CodeWithLocalLLM : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeWithLocalLLM"/> class.
        /// </summary>
        public CodeWithLocalLLM() : base(null)
        {
            this.Caption = "Code With Local AI";

            this.Content = new CodeWithLocalLLMControl();
            InitLocalLLMInLocalControlAsync();
        }

        public async Task InitLocalLLMInLocalControlAsync()
        {
            await ((CodeWithLocalLLMControl)this.Content).InitLocalLLMAsync();
        }

    }
}
