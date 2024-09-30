using System;
using System.Globalization;
using System.Windows.Forms;

namespace LocalCode
{
    [ProvideToolboxControl("LocalCode.CodeWithLocalControl", false)]
    public partial class CodeWithLocalControl : UserControl
    {
        public CodeWithLocalControl()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
        }
    }
}
