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
		private string savedContent = "";
		private string currentFilePath = null;
		private bool isFileSaved = true;
		public NotDefteri()
		{
			InitializeComponent();
			this.KeyPreview = true;


			string[] args = Environment.GetCommandLineArgs();

			// Eğer argüman olarak bir dosya yolu verilmişse, bu dosyayı aç
			if (args.Length > 1)
			{
				string filePath = args[1]; // args[0], uygulamanın kendisinin yoludur, bu yüzden args[1] kullanılır
				OpenFile(filePath);
			}
		}

		public void OpenFile(string filePath)
		{
			if (File.Exists(filePath))
			{
				richTextBox.Text = File.ReadAllText(filePath);
				currentFilePath = filePath; // Dosya yolu güncelleme
				savedContent = richTextBox.Text; // Kaydedilmiş içerik güncelleme
				isFileSaved = true;
				UpdateFormTitle(); // Başlık güncelleme
			}
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
				openFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					richTextBox.Text = File.ReadAllText(openFileDialog.FileName);
					currentFilePath = openFileDialog.FileName; // Dosya yolu güncelleme
					savedContent = richTextBox.Text; // Kaydedilmiş içerik güncelleme
					isFileSaved = true;
					UpdateFormTitle(); // Başlık güncelleme
				}
			}
		}

		private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Eğer dosya zaten bir kere kaydedildi ise, mevcut dosya yoluna kaydet
			if (!string.IsNullOrEmpty(currentFilePath))
			{
				File.WriteAllText(currentFilePath, richTextBox.Text);
				savedContent = richTextBox.Text; // Kaydedilmiş içerik güncelleme
				isFileSaved = true;
				UpdateFormTitle(); // Başlık güncelleme
			}
			else
			{
				// Eğer dosya daha önce hiç kaydedilmediyse, "Farklı Kaydet" diyalogunu göster
				SaveFileAs();
			}
		}

		private void yeniPencereToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Yeni pencere açma işlemi
			NotDefteri yeniPencere = new NotDefteri();
			yeniPencere.Show();
		}

		private void farkliKaydetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Farklı kaydetme işlemi
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*";
				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					File.WriteAllText(saveFileDialog.FileName, richTextBox.Text);
				}
			}
		}

		private void notDefteriHakkindaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Hakkinda hakkinda = new Hakkinda();
			hakkinda.Show();
		}

		private void geriAlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Undo();
		}

		private void kesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Cut();
		}

		private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Copy();
		}

		private void yapıştırToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Paste();
		}

		private void silToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Clear();
		}

		private void bulToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (Bul bulForm = new Bul())
			{
				bulForm.TextBoxReferans = this.richTextBox; // Bu formdaki richTextBox kontrolünün referansını geçirin.
				bulForm.Show();
			}
		}

		private void degistirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string arananMetin = Microsoft.VisualBasic.Interaction.InputBox("Değiştirilecek metni girin:", "Bul", "", -1, -1);
			if (!string.IsNullOrEmpty(arananMetin))
			{
				string yeniMetin = Microsoft.VisualBasic.Interaction.InputBox("Yeni metni girin:", "Değiştir", "", -1, -1);
				richTextBox.Text = richTextBox.Text.Replace(arananMetin, yeniMetin);
			}
		}

		private void tumunuSecToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.SelectAll();
		}

		private void saatTarihToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Saat/Tarih bilgisini metin kutusunun mevcut konumuna ekler.
			richTextBox.SelectedText = DateTime.Now.ToString();
		}

		private void NotDefteri_FormClosing(object sender, FormClosingEventArgs e)
		{
			// richTextBox'ta değişiklik yapıldıysa ve içerik kaydedilmediyse kullanıcıya sor
			if (richTextBox.Modified && !isFileSaved)
			{
				var result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
				{
					// Kaydetme işlemi
					SaveFile();
				}
				else if (result == DialogResult.Cancel)
				{
					// Kapatmayı iptal et
					e.Cancel = true;
				}
				// No seçeneği için ekstra bir işlem yapmaya gerek yok. Form kapatılacak.
			}
		}

		private void SaveFile()
		{
			if (string.IsNullOrEmpty(currentFilePath))
			{
				// Dosya daha önce kaydedilmemişse, "Farklı Kaydet" diyalogunu göster
				SaveFileAs();
			}
			else
			{
				// Dosya daha önce kaydedilmişse, mevcut dosya yoluna kaydet
				File.WriteAllText(currentFilePath, richTextBox.Text);
				savedContent = richTextBox.Text; // Kaydedilmiş içerik güncelleme
				isFileSaved = true;
				UpdateFormTitle(); // Başlık güncelleme
			}
		}

		private void SaveFileAs()
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*";
				// Mevcut dosyanın adını ve dizinini SaveFileDialog'da önceden ayarla
				if (!string.IsNullOrEmpty(currentFilePath))
				{
					saveFileDialog.FileName = Path.GetFileName(currentFilePath);
					saveFileDialog.InitialDirectory = Path.GetDirectoryName(currentFilePath);
				}

				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					File.WriteAllText(saveFileDialog.FileName, richTextBox.Text);
					currentFilePath = saveFileDialog.FileName; // Yeni dosya yolu güncelleme
					savedContent = richTextBox.Text;
					isFileSaved = true;
					UpdateFormTitle(); // Başlığı güncelle
				}
			}
		}

		private void UpdateFormTitle()
		{
			string fileName = "Adsız";
			if (!string.IsNullOrEmpty(currentFilePath))
			{
				fileName = Path.GetFileNameWithoutExtension(currentFilePath);
			}
			this.Text = $"{fileName}{(isFileSaved ? "" : " *")} - Not Defteri";

		}

		private void richTextBox_TextChanged(object sender, EventArgs e)
		{
			if (richTextBox.Text == savedContent)
			{
				richTextBox.Modified = false;
				isFileSaved = true;
			}
			else
			{
				richTextBox.Modified = true;
				isFileSaved = false;
			}
			UpdateFormTitle();
		}

		private void NotDefteri_Load(object sender, EventArgs e)
		{
			savedContent = richTextBox.Text;
			UpdateFormTitle();
		}

		private void NotDefteri_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.S)
			{
				SaveFile();
				e.Handled = true; // Klavye olayını işlendi olarak işaretle
			}
		}
	}
}
