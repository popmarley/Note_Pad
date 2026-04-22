using DiffPlex.DiffBuilder.Model;
using DiffPlex.DiffBuilder;
using DiffPlex;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Defteri
{
    public partial class MetinKarsilastirici : Form
    {
        public Font EditorFont { get; set; }

        public MetinKarsilastirici()
        {
            InitializeComponent();
            compareButton.Click += CompareTexts;
            ConfigureLayout();
        }

        private void CompareTexts(object sender, EventArgs e)
        {
            string text1 = textBox1.Text;
            string text2 = textBox2.Text;

            resultBox.Clear();

            string[] lines1 = SplitLines(text1);
            string[] lines2 = SplitLines(text2);

            int maxLines = Math.Max(lines1.Length, lines2.Length);
            bool isDifferentFound = false;
            int differentLineCount = 0;

            for (int i = 0; i < maxLines; i++)
            {
                string currentLine1 = i < lines1.Length ? lines1[i] : "";
                string currentLine2 = i < lines2.Length ? lines2[i] : "";

                if (currentLine1 != currentLine2)
                {
                    isDifferentFound = true;
                    differentLineCount++;
                }

                CompareAndDisplayLines(i + 1, currentLine1, currentLine2);
            }

            if (!isDifferentFound)
            {
                AppendColoredText(resultBox, "Herhangi bir değişiklik bulunamadı.", Color.Firebrick);
            }

            label1.Text = isDifferentFound
                ? $"Farklı satır: {differentLineCount:N0} / {maxLines:N0}"
                : $"Farklı satır yok / {maxLines:N0}";
        }

        private void CompareAndDisplayLines(int lineNumber, string line1, string line2)
        {
            var diffBuilder = new InlineDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(line1, line2);

            bool isDifferent = diff.Lines.Any(l => l.Type != ChangeType.Unchanged);
            if (!isDifferent)
            {
                AppendColoredText(resultBox, $"{lineNumber,4}   {line1}", Color.FromArgb(64, 64, 64));
                return;
            }

            AppendColoredText(resultBox, $"Satır {lineNumber}", Color.FromArgb(30, 90, 160), true);

            foreach (var line in diff.Lines)
            {
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        AppendColoredText(resultBox, "+ " + line.Text, Color.FromArgb(24, 128, 56));
                        break;
                    case ChangeType.Deleted:
                        AppendColoredText(resultBox, "- " + line.Text, Color.FromArgb(190, 45, 45));
                        break;
                    case ChangeType.Unchanged:
                        AppendColoredText(resultBox, "  " + line.Text, Color.FromArgb(80, 80, 80));
                        break;
                }
            }

            resultBox.AppendText(Environment.NewLine);
        }

        private void AppendColoredText(RichTextBox box, string text, Color color, bool bold = false)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.SelectionFont = new Font(box.Font, bold ? FontStyle.Bold : FontStyle.Regular);
            box.AppendText(text + Environment.NewLine);
            box.SelectionColor = box.ForeColor;
            box.SelectionFont = box.Font;
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            resultBox.Clear();
            label1.Text = "Hazır";
        }

        private void MetinKarsilastirici_Load(object sender, EventArgs e)
        {
            Font editorFont = EditorFont ?? new Font("Consolas", 10F);
            textBox1.Font = editorFont;
            textBox2.Font = editorFont;
            resultBox.Font = editorFont;
        }

        private void ConfigureLayout()
        {
            SuspendLayout();
            Controls.Clear();

            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(820, 560);
            Size = new Size(920, 640);
            BackColor = Color.FromArgb(248, 249, 251);

            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Dock = DockStyle.Fill;
            textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.WordWrap = false;
            textBox1.AcceptsTab = true;

            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Dock = DockStyle.Fill;
            textBox2.Multiline = true;
            textBox2.ScrollBars = ScrollBars.Both;
            textBox2.WordWrap = false;
            textBox2.AcceptsTab = true;

            resultBox.BorderStyle = BorderStyle.FixedSingle;
            resultBox.Dock = DockStyle.Fill;
            resultBox.ReadOnly = true;
            resultBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            resultBox.WordWrap = false;
            resultBox.BackColor = Color.White;

            compareButton.Text = "Karşılaştır";
            compareButton.Width = 120;
            compareButton.Height = 32;

            btnTemizle.Text = "Temizle";
            btnTemizle.Width = 100;
            btnTemizle.Height = 32;

            label1.Text = "Hazır";
            label1.AutoSize = true;
            label1.TextAlign = ContentAlignment.MiddleLeft;
            label1.Margin = new Padding(12, 8, 0, 0);

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.Padding = new Padding(12);
            mainLayout.ColumnCount = 1;
            mainLayout.RowCount = 3;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 48F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 52F));

            TableLayoutPanel inputLayout = new TableLayoutPanel();
            inputLayout.Dock = DockStyle.Fill;
            inputLayout.ColumnCount = 2;
            inputLayout.RowCount = 1;
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            GroupBox leftGroup = CreateGroup("İlk Metin", textBox1);
            GroupBox rightGroup = CreateGroup("İkinci Metin", textBox2);
            inputLayout.Controls.Add(leftGroup, 0, 0);
            inputLayout.Controls.Add(rightGroup, 1, 0);

            FlowLayoutPanel commandPanel = new FlowLayoutPanel();
            commandPanel.Dock = DockStyle.Fill;
            commandPanel.FlowDirection = FlowDirection.LeftToRight;
            commandPanel.WrapContents = false;
            commandPanel.Padding = new Padding(0, 8, 0, 0);
            commandPanel.Controls.Add(compareButton);
            commandPanel.Controls.Add(btnTemizle);
            commandPanel.Controls.Add(label1);

            GroupBox resultGroup = CreateGroup("Sonuç", resultBox);

            mainLayout.Controls.Add(inputLayout, 0, 0);
            mainLayout.Controls.Add(commandPanel, 0, 1);
            mainLayout.Controls.Add(resultGroup, 0, 2);

            Controls.Add(mainLayout);
            ResumeLayout(true);
        }

        private GroupBox CreateGroup(string title, Control content)
        {
            GroupBox group = new GroupBox();
            group.Text = title;
            group.Dock = DockStyle.Fill;
            group.Padding = new Padding(10, 22, 10, 10);
            group.Margin = new Padding(6);
            group.BackColor = Color.White;
            content.Margin = new Padding(0);
            group.Controls.Add(content);
            return group;
        }

        private string[] SplitLines(string text)
        {
            return text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        }
    }

}

