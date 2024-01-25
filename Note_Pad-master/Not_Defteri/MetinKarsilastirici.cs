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
        public MetinKarsilastirici()
        {
            InitializeComponent();



            this.AcceptButton = compareButton;
            this.AcceptButton = btnTemizle;
            compareButton.Click += CompareTexts;
           

            Controls.Add(textBox1);
            Controls.Add(textBox2);
            Controls.Add(compareButton);
            Controls.Add(resultBox);

            Width = 450;
            Height = 300;
        }

        private void CompareTexts(object sender, EventArgs e)
        {
            string text1 = textBox1.Text;
            string text2 = textBox2.Text;

            resultBox.Clear();

            string[] lines1 = text1.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string[] lines2 = text2.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            int maxLines = Math.Max(lines1.Length, lines2.Length);
            bool isDifferentFound = false;

            for (int i = 0; i < maxLines; i++)
            {
                string currentLine1 = i < lines1.Length ? lines1[i] : "";
                string currentLine2 = i < lines2.Length ? lines2[i] : "";

                if (currentLine1 != currentLine2)
                {
                    isDifferentFound = true;
                }

                CompareAndDisplayLines(currentLine1, currentLine2);
            }

            if (!isDifferentFound)
            {
                AppendColoredText(resultBox, "\nHerhangi bir değişiklik bulunamadı.", Color.Firebrick);
            }
        }

        private void CompareAndDisplayLines(string line1, string line2)
        {
            var diffBuilder = new InlineDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(line1, line2);

            bool isDifferent = diff.Lines.Any(l => l.Type != ChangeType.Unchanged);
            bool addedBlankLine = false;

            foreach (var line in diff.Lines)
            {
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        // Farklı satırlar arasında boşluk ekleme
                        if (!addedBlankLine && isDifferent)
                        {
                            resultBox.AppendText(Environment.NewLine);
                            addedBlankLine = true;
                        }
                        AppendColoredText(resultBox, "+ " + line.Text, Color.Green);
                        break;
                    case ChangeType.Deleted:
                        if (!addedBlankLine && isDifferent)
                        {
                            resultBox.AppendText(Environment.NewLine);
                            addedBlankLine = true;
                        }
                        AppendColoredText(resultBox, "- " + line.Text, Color.Red);
                        break;
                    case ChangeType.Unchanged:
                        AppendColoredText(resultBox, "  " + line.Text, Color.Black);
                        break;
                }
            }

            // Farklılık varsa, karşılaştırma sonrası ekstra boşluk ekleyin
            if (isDifferent)
            {
                resultBox.AppendText(Environment.NewLine);
            }
        }




        private void AppendColoredText(RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text + Environment.NewLine);
            box.SelectionColor = box.ForeColor;
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            resultBox.Clear();
        }
    }

}

