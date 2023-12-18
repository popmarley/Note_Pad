using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

		private Bul bulForm = null;

		public NotDefteri()
		{
			InitializeComponent();
			this.DoubleBuffered = true;

			this.KeyPreview = true;
			richTextBox.MouseWheel += new MouseEventHandler(richTextBox_MouseWheel);
			toolStripStatusLabel3.Text = "100%";

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

		#region Kısayollar

		private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Mevcut not defterinin içeriğini temizle
			richTextBox.Clear();
			currentFilePath = null; // Dosya yolu sıfırlanıyor
			isFileSaved = true; // Dosya kaydedildi olarak işaretle
			UpdateFormTitle(); // Başlık güncelleme
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
			richTextBox.Clear();
			currentFilePath = null; // Dosya yolu sıfırlanıyor
			isFileSaved = true; // Dosya kaydedildi olarak işaretle
			UpdateFormTitle(); // Başlık güncelleme
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

		private void ileriAlToolStripMenuItem_Click(object sender, EventArgs e)
		{
			richTextBox.Redo();
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

		private void richTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}

		private void GeriAlStripButton1_Click(object sender, EventArgs e)
		{
			richTextBox.Undo();

		}

		private void İleriAlStripButton2_Click(object sender, EventArgs e)
		{
			richTextBox.Redo();

		}

		private void KesStripButton3_Click(object sender, EventArgs e)
		{
			richTextBox.Cut();
		}

		private void KopyalaStripButton4_Click(object sender, EventArgs e)
		{
			richTextBox.Copy();

		}

		private void YapistirStripButton5_Click(object sender, EventArgs e)
		{
			richTextBox.Paste();
		}

		


		private void KalinStripButton6_Click(object sender, EventArgs e)
		{
			isBoldActive = !isBoldActive;
			UpdateFontStyle();
		}

		private void İtalicStripButton7_Click(object sender, EventArgs e)
		{
			isItalicActive = !isItalicActive;
			UpdateFontStyle();
		}

		private void AltiCizgiliStripButton8_Click(object sender, EventArgs e)
		{
			isUnderlineActive = !isUnderlineActive;
			UpdateFontStyle();
		}

		private void BuyutStripButton11_Click(object sender, EventArgs e)
		{
			richTextBox.SelectionFont = new Font(richTextBox.Font.FontFamily, richTextBox.Font.Size + 1);
		}

		private void KucultStripButton12_Click(object sender, EventArgs e)
		{
			richTextBox.SelectionFont = new Font(richTextBox.Font.FontFamily, richTextBox.Font.Size - 1);
		}

		private void MaddeleStripButton9_Click(object sender, EventArgs e)
		{
			isBulletedListActive = !isBulletedListActive;
			richTextBox.SelectionBullet = isBulletedListActive;
		}

		private void SiralaStripButton10_Click(object sender, EventArgs e)
		{
			isNumberedListActive = !isNumberedListActive;
			ApplyNumbering();
		}

		private void SolaHizalaStripButton13_Click(object sender, EventArgs e)
		{
			richTextBox.SelectionAlignment = HorizontalAlignment.Left;
		}

		private void OrtalaStripButton14_Click(object sender, EventArgs e)
		{
			richTextBox.SelectionAlignment = HorizontalAlignment.Center;
		}

		private void SagaHizalaStripButton15_Click(object sender, EventArgs e)
		{
			richTextBox.SelectionAlignment = HorizontalAlignment.Right;
		}


		private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Kullanıcıyı yazdırma işlemi hakkında uyar
			DialogResult dialogResult = MessageBox.Show("Varsayılan yazıcı üzerinden yazdırılma işlemi yapılacaktır. Devam etmek istiyor musunuz?", "Yazdırma Onayı", MessageBoxButtons.YesNo);

			// Eğer kullanıcı 'Evet' derse, yazdırma işlemine başla
			if (dialogResult == DialogResult.Yes)
			{
				printDocument1.Print();
			}
		}

		private void bulToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Eğer Bul formu daha önce oluşturulmadıysa veya kapandıysa, yeni bir örnek oluştur
			if (bulForm == null || bulForm.IsDisposed)
			{
				bulForm = new Bul();
				bulForm.TextBoxReferans = this.richTextBox; // richTextBox referansını geçir
			}

			// Bul formunu modaless olarak göster
			bulForm.Show();
			bulForm.Focus(); // Bul formuna odaklan
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

		#endregion

		private bool promptShown = false;
		private void NotDefteri_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (!promptShown && richTextBox.Modified && !isFileSaved)
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
				promptShown = true; // Set the flag after showing the prompt
			}
			else if (Application.OpenForms.Count == 1)
			{
				Application.Exit(); // Eğer bu son form ise, uygulamayı kapat
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

		private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			// richTextBox'ın içeriğini yazdır
			e.Graphics.DrawString(richTextBox.Text, new Font("Arial", 12), Brushes.Black, 25, 25);
		}

		private void richTextBox_SelectionChanged(object sender, EventArgs e)
		{
			int index = richTextBox.SelectionStart;
			int line = richTextBox.GetLineFromCharIndex(index);
			int column = index - richTextBox.GetFirstCharIndexFromLine(line);

			//satır ve sütun gösterme
			toolStripStatusLabel1.Text = $"St: {line + 1}, Stn: {column + 1}";

			//karakter sayısını gösterme
			int textLength = richTextBox.Text.Length;
			toolStripStatusLabel4.Text = $"Krktr S: {textLength}";
		}

		private void durumcubuguToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Menü öğesinin 'Checked' durumunu tersine çevir
			durumcubuguToolStripMenuItem.Checked = !durumcubuguToolStripMenuItem.Checked;

			// Durum çubuğunun görünürlüğünü menü öğesinin 'Checked' durumuna bağla
			statusStrip1.Visible = durumcubuguToolStripMenuItem.Checked;
		}

		private int zoomLevel = 100; // Default zoom level

		private void richTextBox_MouseWheel(object sender, MouseEventArgs e)
		{
			if (Control.ModifierKeys == Keys.Control)
			{
				// Zoom in
				if (e.Delta > 0 && zoomLevel < 500)
				{
					zoomLevel += 10;
				}
				// Zoom out
				else if (e.Delta < 0 && zoomLevel > 10)
				{
					zoomLevel -= 10;
				}

				// Apply zoom level
				richTextBox.ZoomFactor = zoomLevel / 100f;

				// Update status label
				toolStripStatusLabel3.Text = $"{zoomLevel}%";
			}
		}

		private void yakınlastirToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (zoomLevel < 500)
			{
				zoomLevel += 10;
				ApplyZoom();
			}
		}

		private void uzaklastirToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (zoomLevel > 10)
			{
				zoomLevel -= 10;
				ApplyZoom();
			}
		}

		private void varsayilanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			zoomLevel = 100;
			ApplyZoom();
		}

		private void ApplyZoom()
		{
			richTextBox.ZoomFactor = zoomLevel / 100f;
			toolStripStatusLabel3.Text = $"{zoomLevel}%";
		}

		public class CustomRichTextBox : RichTextBox
		{
			private const int WM_MOUSEWHEEL = 0x20A;
			private const int SB_LINEUP = 0;
			private const int SB_LINEDOWN = 1;
			private const int EM_LINESCROLL = 0x00B6;

			[DllImport("user32.dll")]
			private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WM_MOUSEWHEEL)
				{
					int wheelDelta = (int)m.WParam >> 16;
					int linesToScroll = 3; // Her tekerlek hareketi için kaydırılacak satır sayısı

					if (wheelDelta > 0)
					{
						// Yukarı kaydır
						SendMessage(this.Handle, EM_LINESCROLL, IntPtr.Zero, (IntPtr)(-linesToScroll));
					}
					else if (wheelDelta < 0)
					{
						// Aşağı kaydır
						SendMessage(this.Handle, EM_LINESCROLL, IntPtr.Zero, (IntPtr)linesToScroll);
					}
				}
				else
				{
					base.WndProc(ref m);
				}
			}
		}


		#region kısayollar metotlar

		bool isBoldActive = false;
		bool isItalicActive = false;
		bool isUnderlineActive = false;
		bool isBulletedListActive = false;
		bool isNumberedListActive = false;

		private void UpdateFontStyle()
		{
			FontStyle style = FontStyle.Regular;

			if (isBoldActive)
				style |= FontStyle.Bold;

			if (isItalicActive)
				style |= FontStyle.Italic;

			if (isUnderlineActive)
				style |= FontStyle.Underline;

			richTextBox.SelectionFont = new Font(richTextBox.Font, style);
		}

		private void ApplyNumbering()
		{
			int lineNumber = 1;
			string[] lines = richTextBox.Lines;
			for (int i = 0; i < lines.Length; i++)
			{
				if (isNumberedListActive)
				{
					if (!lines[i].StartsWith($"{lineNumber}. "))
					{
						lines[i] = $"{lineNumber}. {lines[i]}";
					}
					lineNumber++;
				}
				else
				{
					lines[i] = lines[i].Substring(lines[i].IndexOf(' ') + 1);
				}
			}
			richTextBox.Lines = lines;
		}

		#endregion


	}

}
