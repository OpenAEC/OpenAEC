using System.Windows.Forms;

namespace OpenAEC.Utilities
{
    /// <summary>
    /// Progress Form Class
    /// </summary>
    public partial class ProgressForm : Form
    {
        private readonly string _format;

        /// <summary>
        /// Set up progress bar form and immediately display it modelessly.
        /// </summary>
        /// <param name="caption">Form caption</param>
        /// <param name="format">Progress message string</param>
        /// <param name="max">Number of elements to process</param>
        public ProgressForm(string caption, string format, int max)
        {
            _format = format;
            InitializeComponent();
            Text = caption;
            label1.Text = null == format ? caption : string.Format(format, 0);
            progressBar1.Minimum = 0;
            progressBar1.Maximum = max;
            progressBar1.Value = 0;
            Show();
            Application.DoEvents();
        }
        /// <summary>
        /// Progress Form increment method.
        /// </summary>
        public void Increment()
        {
            ++progressBar1.Value;
            if (null != _format)
            {
                label1.Text = string.Format(_format, progressBar1.Value);
            }
            Application.DoEvents();
        }
    }
}
