using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Defteri
{
	public partial class NotDefteri : Form
	{
		public NotDefteri()
		{
			InitializeComponent();
		}

		

		private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 'Yeni' menü öğesi için kodlar
			richTextBox.Clear();
		}

		private void acToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 'Aç' menü öğesi için kodlar
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					richTextBox.Text = File.ReadAllText(openFileDialog.FileName);
				}
			}
		}

		private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// 'Kaydet' menü öğesi için kodlar
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				// Kaydedilebilecek dosya türlerini belirleme
				saveFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar|*.*";
				saveFileDialog.DefaultExt = "txt"; // Varsayılan uzantı
				saveFileDialog.AddExtension = true; // Uzantıyı otomatik ekle

				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					// Dosya olarak kaydetme işlemi
					File.WriteAllText(saveFileDialog.FileName, richTextBox.Text);
				}
			}
		}
	}
}
