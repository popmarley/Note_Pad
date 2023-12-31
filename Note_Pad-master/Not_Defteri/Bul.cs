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
	public partial class Bul : Form
	{

		public RichTextBox TextBoxReferans { get; set; }
		public Bul()
		{
			InitializeComponent();
		}

		private void sonrakiniBulButton_Click(object sender, EventArgs e)
		{
			string arananMetin = arananTextBox.Text;
			bool buyukKucukHarfEslestir = buyukKucukHarfCheckBox.Checked;
			RichTextBoxFinds options = buyukKucukHarfEslestir ? RichTextBoxFinds.MatchCase : RichTextBoxFinds.None;
			options |= yukariRadioButton.Checked ? RichTextBoxFinds.Reverse : RichTextBoxFinds.None;

			if (TextBoxReferans != null && !string.IsNullOrEmpty(arananMetin))
			{
				int startIndex, endIndex;
				if (yukariRadioButton.Checked)
				{
					startIndex = 0;
					endIndex = TextBoxReferans.SelectionStart;
				}
				else
				{
					startIndex = TextBoxReferans.SelectionStart + TextBoxReferans.SelectionLength;
					endIndex = TextBoxReferans.Text.Length;
				}

				int foundIndex = TextBoxReferans.Find(arananMetin, startIndex, endIndex, options);

				if (foundIndex != -1)
				{
					TextBoxReferans.Select(foundIndex, arananMetin.Length);
					TextBoxReferans.ScrollToCaret();
				}
				else
				{
					MessageBox.Show("Metin bulunamadı.", "Bul", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
