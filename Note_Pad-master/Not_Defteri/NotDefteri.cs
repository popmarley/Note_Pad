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
using System.Speech.Recognition;

namespace Not_Defteri
{
    public partial class NotDefteri : Form
    {
        private string savedContent = "";
        private string currentFilePath = null;
        private bool isFileSaved = true;

        private Bul bulForm = null;
        private Degistir degistirForm = null;

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

            // RichTextBox için ContextMenuStrip oluştur
            ContextMenuStrip richtextBoxContextMenu = new ContextMenuStrip();
            ToolStripMenuItem geriAlMenuItem = new ToolStripMenuItem("Geri Al");
            ToolStripMenuItem kesMenuItem = new ToolStripMenuItem("Kes");
            ToolStripMenuItem kopyalaMenuItem = new ToolStripMenuItem("Kopyala");
            ToolStripMenuItem yapistirMenuItem = new ToolStripMenuItem("Yapıştır");
            ToolStripMenuItem silMenuItem = new ToolStripMenuItem("Sil"); // Sil menü öğesi

            // Menü öğelerini ContextMenuStrip'e ekle
            richtextBoxContextMenu.Items.AddRange(new ToolStripItem[] { geriAlMenuItem, kesMenuItem, kopyalaMenuItem, yapistirMenuItem, silMenuItem });

            // Menü öğelerine tıklama olaylarını ekle
            geriAlMenuItem.Click += (sender, e) => richTextBox.Undo();
            kesMenuItem.Click += (sender, e) => richTextBox.Cut();
            kopyalaMenuItem.Click += (sender, e) => richTextBox.Copy();
            yapistirMenuItem.Click += (sender, e) => richTextBox.Paste();
            silMenuItem.Click += (sender, e) => richTextBox.SelectedText = ""; // Seçili metni siler

            // RichTextBox'ın ContextMenuStrip özelliğini ayarla
            richTextBox.ContextMenuStrip = richtextBoxContextMenu;

            LoadFontSettings();
            LoadTextStyleSettings();
        }

        private SpeechRecognitionEngine recognizer;

        private void InitializeSpeechRecognition()
        {
            recognizer = new SpeechRecognitionEngine();

            // Configure the recognizer
            recognizer.SetInputToDefaultAudioDevice(); // Set the input to the default audio device
            recognizer.LoadGrammar(new DictationGrammar()); // Load a dictation grammar

            recognizer.SpeechRecognized += recognizer_SpeechRecognized; // Add an event handler for recognized speech
            recognizer.RecognizeAsync(RecognizeMode.Multiple); // Start asynchronous recognition
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() => {
                richTextBox.AppendText(e.Result.Text + " "); // Append recognized text to the richTextBox
            }));
        }
        private void LoadTextStyleSettings()
        {
            isBoldActive = Properties.Settings.Default.isBoldActive;
            isItalicActive = Properties.Settings.Default.isItalicActive;
            isUnderlineActive = Properties.Settings.Default.isUnderlineActive;
            UpdateFontStyle(); // Mevcut yazı stilini ayarlar

            // Butonların Checked durumlarını güncelle
            KalinStripButton6.Checked = isBoldActive;
            İtalicStripButton7.Checked = isItalicActive;
            AltiCizgiliStripButton8.Checked = isUnderlineActive;
        }

        private void LoadFontSettings()
        {
            string fontName = Properties.Settings.Default.FontName;
            float fontSize = Properties.Settings.Default.FontSize;
            FontStyle fontStyle = (FontStyle)Properties.Settings.Default.FontStyle;

            if (!string.IsNullOrEmpty(fontName) && fontSize > 0)
            {
                richTextBox.Font = new Font(fontName, fontSize, fontStyle);
            }
        }
        public void OpenFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                // Dosyayı UTF-8 encoding'i ile oku
                richTextBox.Text = File.ReadAllText(filePath, Encoding.UTF8);
                currentFilePath = filePath; // Dosya yolu güncelleme
                savedContent = richTextBox.Text; // Kaydedilmiş içerik güncelleme
                isFileSaved = true;
                UpdateFormTitle(); // Başlık güncelleme
            }
        }

        #region Kısayollar

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {// Mevcut pencerede yapılan değişiklikleri kontrol et
            if (richTextBox.Modified && !isFileSaved)
            {
                var result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile(); // Kaydetme işlemi
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Kullanıcı iptal ederse, yeni pencere açmayı durdur
                }
                // Kullanıcı "Hayır" derse, hiçbir şey yapmadan yeni pencere aç
            }
            // Mevcut not defterinin içeriğini temizle
            richTextBox.Clear();
            currentFilePath = null; // Dosya yolu sıfırlanıyor
            isFileSaved = true; // Dosya kaydedildi olarak işaretle
            UpdateFormTitle(); // Başlık güncelleme
        }

        private void acToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Mevcut içerik değiştirildiyse ve kaydedilmediyse, kullanıcıya kaydetme seçeneği sun
            if (richTextBox.Modified && !isFileSaved)
            {
                var result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile(); // Kaydetme işlemi
                }
                else if (result == DialogResult.Cancel)
                {
                    return; // Kullanıcı iptal ederse, dosya açmayı durdur
                }
                // Kullanıcı "Hayır" derse, hiçbir şey yapmadan devam et
            }

            // 'Aç' diyalog kutusunu göster
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
            // Mevcut pencerede yapılan değişiklikleri kontrol et
            if (richTextBox.Modified && !isFileSaved)
            {
                var result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile(); // Kaydetme işlemi
                }
                else if (result == DialogResult.Cancel)
                {

                    return; // Kullanıcı iptal ederse, yeni pencere açmayı durdur
                }

                // Kullanıcı "Hayır" derse, hiçbir şey yapmadan yeni pencere aç
            }

            // Yeni bir NotDefteri örneği oluştur ve göster
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
            hakkinda.ShowDialog();
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
            // İmlecin bulunduğu konumu al
            int cursorPosition = richTextBox.SelectionStart;

            // Eğer metin varsa ve imleç metnin sonunda değilse, bir karakter sil
            if (richTextBox.Text.Length > 0 && cursorPosition < richTextBox.Text.Length)
            {
                // Metni, imleçten önceki ve sonraki kısmı birleştirerek güncelle
                richTextBox.Text = richTextBox.Text.Substring(0, cursorPosition) + richTextBox.Text.Substring(cursorPosition + 1);

                // İmleci eski konumuna geri getir
                richTextBox.SelectionStart = cursorPosition;
            }
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
            Properties.Settings.Default.isBoldActive = isBoldActive;
            Properties.Settings.Default.Save();
            KalinStripButton6.Checked = isBoldActive;
        }

        private void İtalicStripButton7_Click(object sender, EventArgs e)
        {
            isItalicActive = !isItalicActive;
            UpdateFontStyle();
            Properties.Settings.Default.isItalicActive = isItalicActive;
            Properties.Settings.Default.Save();
            İtalicStripButton7.Checked = isItalicActive;
        }

        private void AltiCizgiliStripButton8_Click(object sender, EventArgs e)
        {
            isUnderlineActive = !isUnderlineActive;
            UpdateFontStyle();
            Properties.Settings.Default.isUnderlineActive = isUnderlineActive;
            Properties.Settings.Default.Save();
            AltiCizgiliStripButton8.Checked = isUnderlineActive;

        }

        private void BuyutStripButton11_Click(object sender, EventArgs e)
        {
            ChangeFontSize(1); // Font boyutunu 1 birim artır
        }

        private void KucultStripButton12_Click(object sender, EventArgs e)
        {
            ChangeFontSize(-1); // Font boyutunu 1 birim azalt
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

        private void sayfaAsagiStripButton2_Click(object sender, EventArgs e)
        {
            NativeMethods.SendMessage(richTextBox.Handle, NativeMethods.WM_VSCROLL, NativeMethods.SB_PAGEUP, IntPtr.Zero);
        }

        private void sayfaYukariStripButton1_Click(object sender, EventArgs e)
        {
            NativeMethods.SendMessage(richTextBox.Handle, NativeMethods.WM_VSCROLL, NativeMethods.SB_PAGEDOWN, IntPtr.Zero);
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
                this.FormClosed += (s, args) => bulForm.Close(); // Ana form kapatıldığında Bul formunu kapat
            }

            // Bul formunu modaless olarak göster
            bulForm.Show();
            bulForm.Focus(); // Bul formuna odaklan
        }

        private void degistirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Eğer Bul formu daha önce oluşturulmadıysa veya kapandıysa, yeni bir örnek oluştur
            if (degistirForm == null || degistirForm.IsDisposed)
            {
                degistirForm = new Degistir();
                degistirForm.TextBoxReferans = this.richTextBox; // richTextBox referansını geçir
                this.FormClosed += (s, args) => degistirForm.Close(); // Ana form kapatıldığında Bul formunu kapat
            }

            // Degistir formunu modaless olarak göster
            degistirForm.Show();
            degistirForm.Focus(); // Degistir formuna odaklan
        }

        private void tumunuSecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.SelectAll();
        }

        private void saatTarihToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Saat/Tarih bilgisini metin kutusunun mevcut konumuna ekler.
            richTextBox.SelectedText = " "+DateTime.Now.ToString();
        }

        #endregion

        private bool promptShown = false;
        private void NotDefteri_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!promptShown && richTextBox.Modified && !isFileSaved)
            {
                promptShown = true; // Uyarı gösterilmeden önce true olarak ayarla

                var result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // Kapatmayı iptal et
                    promptShown = false; // İptal edildiğinde promptShown'ı tekrar false yap
                    return; // Döngüden çık
                }
                else if (result == DialogResult.No)
                {
                    // "Hayır" seçeneği için ekstra işlem yapmaya gerek yok
                    // Form kapatılacak ve değişiklikler kaydedilmeyecek
                }
            }

            if (!e.Cancel) // Eğer form kapanmıyorsa, kaydetme işlemini yapma
            {
                // Yazı tipi ve boyutu ayarlarını kaydet
                SaveFontSettings();

                if (Application.OpenForms.Count == 1)
                {
                    Application.Exit(); // Eğer bu son form ise, uygulamayı kapat
                }
            }

            if (recognizer != null)
            {
                recognizer.RecognizeAsyncStop();
                recognizer.Dispose();
            }

            Properties.Settings.Default.MenulerVisible = menulerToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();

            Properties.Settings.Default.DurumCubuguVisible = durumcubuguToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void SaveFontSettings()
        {
            Properties.Settings.Default.FontName = toolStripComboBoxYaziTipi.Text;
            if (float.TryParse(toolStripComboBoxYaziBoyutu.Text, out float fontSize))
            {
                Properties.Settings.Default.FontSize = fontSize;
            }
            else
            {
                Properties.Settings.Default.FontSize = 12.0f; // Örnek varsayılan boyut
            }
            Properties.Settings.Default.Save();
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
                // Dosyayı UTF-8 encoding'i ile kaydet
                File.WriteAllText(currentFilePath, richTextBox.Text, Encoding.UTF8);
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
                    // Dosyayı UTF-8 encoding'i ile kaydet
                    File.WriteAllText(saveFileDialog.FileName, richTextBox.Text, Encoding.UTF8);
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

            // Kaydedilme durumu için toolStripStatusLabel'ı güncelle
            if (string.IsNullOrEmpty(richTextBox.Text))
            {
                kayitedildiMİ.Text = " ";
                
            }
            else if (!isFileSaved)
            {
                kayitedildiMİ.Text = "Kaydedilmedi";
                kayitedildiMİ.ForeColor = Color.Red; // Kaydedilmedi ise kırmızı
            }
            else
            {
                kayitedildiMİ.Text = "Kayıtlı";
                kayitedildiMİ.ForeColor = Color.Black; 
            }
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

            foreach (FontFamily font in FontFamily.Families)
            {
                toolStripComboBoxYaziTipi.Items.Add(font.Name);
            }

            // Mevcut yazı tipini ayarla
            toolStripComboBoxYaziTipi.SelectedItem = richTextBox.Font.FontFamily.Name;

            // Sık kullanılan yazı boyutlarını yükle
            int[] boyutlar = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int boyut in boyutlar)
            {
                toolStripComboBoxYaziBoyutu.Items.Add(boyut.ToString());
            }

            // Kaydedilen yazı tipi ve boyutunu yükle
            string fontName = Properties.Settings.Default.FontName;
            string fontSize = Properties.Settings.Default.FontSize.ToString();

            if (!string.IsNullOrEmpty(fontName))
            {
                if (!toolStripComboBoxYaziTipi.Items.Contains(fontName))
                {
                    toolStripComboBoxYaziTipi.Items.Add(fontName);
                }
                toolStripComboBoxYaziTipi.SelectedItem = fontName;
            }

            if (!string.IsNullOrEmpty(fontSize))
            {
                if (!toolStripComboBoxYaziBoyutu.Items.Contains(fontSize))
                {
                    toolStripComboBoxYaziBoyutu.Items.Add(fontSize);
                }
                toolStripComboBoxYaziBoyutu.SelectedItem = fontSize;
            }

            // Kaydedilen menü görünürlüğünü yükle ve uygula
            menulerToolStripMenuItem.Checked = Properties.Settings.Default.MenulerVisible;
            toolStrip1.Visible = menulerToolStripMenuItem.Checked;
            toolStrip2.Visible = menulerToolStripMenuItem.Checked;


            // Kaydedilen durum cubugu görünürlüğünü yükle ve uygula
            durumcubuguToolStripMenuItem.Checked = Properties.Settings.Default.MenulerVisible;
            statusStrip1.Visible = durumcubuguToolStripMenuItem.Checked;

            string themeMode = Properties.Settings.Default["ThemeMode"].ToString();
            if (themeMode == "Dark")
            {
                ToggleDarkMode();
            }
            else
            {
                ToggleLightMode();
            }
        } 

        private void NotDefteri_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveFile();
                e.Handled = true; // Klavye olayını işlendi olarak işaretle
            }
            else if (e.KeyCode == Keys.F12) // F12 tuşu kontrolü
            {
                SaveFileAs(); // Farklı Kaydet metodunu çağır
                e.Handled = true; // Klavye olayını işlendi olarak işaretle
            }
            else if (e.Control && e.KeyCode == Keys.D) // Ctrl+D tuş kombinasyonunu kontrol et
            {
                DuplicateCurrentLineToNextLine();
                e.Handled = true; // Klavye olayını işlendi olarak işaretle
            }
        }

        private void DuplicateCurrentLineToNextLine()
        {
            if (richTextBox.Text.Length == 0 || richTextBox.Lines.Length == 0)
            {
                // Metin kutusu boşsa veya satır yoksa, işlemi yapma ve metottan çık
                return;
            }
            int selectionStart = richTextBox.SelectionStart;
            int currentLineIndex = richTextBox.GetLineFromCharIndex(selectionStart); // Mevcut satır numarasını al
            int lineStart = richTextBox.GetFirstCharIndexOfCurrentLine(); // Mevcut satırın başlangıç indeksini al
            int lineLength = richTextBox.Lines[currentLineIndex].Length; // Mevcut satırın uzunluğunu al

            string currentLineText = richTextBox.Lines[currentLineIndex] + Environment.NewLine; // Mevcut satır metni ve yeni satır karakteri

            // Kopyalama yapılacak konum hesaplama
            int insertPosition;
            if (currentLineIndex < richTextBox.Lines.Length - 1)
            {
                // Eğer mevcut satır son satır değilse, bir sonraki satırın başlangıcını hesapla
                insertPosition = richTextBox.GetFirstCharIndexFromLine(currentLineIndex + 1);
            }
            else
            {
                // Eğer mevcut satır son satırsa, mevcut satırın sonuna yeni satır ekle
                insertPosition = lineStart + lineLength;
                currentLineText += Environment.NewLine; // Son satıra ekstra yeni satır eklenir
            }

            richTextBox.SelectionStart = insertPosition; // Yapıştırma konumunu ayarla
            richTextBox.SelectedText = currentLineText; // Kopyalanan satırı yapıştır

            // İmleci kopyalanan içeriğin sonuna taşı
            if (currentLineIndex < richTextBox.Lines.Length - 1)
            {
                richTextBox.SelectionStart = insertPosition + currentLineText.Length - Environment.NewLine.Length;
            }
            else
            {
                richTextBox.SelectionStart = insertPosition + currentLineText.Length;
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

            // Satır ve sütun numarasını formatlayarak gösterme
            toolStripStatusLabel1.Text = $"St: {line + 1:N0}, Stn: {column + 1:N0}";

            // Karakter sayısını formatlayarak gösterme
            int textLength = richTextBox.Text.Length;
            toolStripStatusLabel4.Text = $"Krktr S: {textLength:N0}";
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


        private void ChangeFontSize(float change)
        {
            if (richTextBox.SelectionFont != null)
            {
                // Mevcut boyutu al ve değişikliği uygula
                float currentSize = richTextBox.SelectionFont.Size;
                int newSize = (int)(currentSize + change);
                newSize = Math.Max(1, newSize); // Font boyutunu 1'den küçük olmamasını sağlar

                // Font boyutunu güncelle
                richTextBox.SelectionFont = new Font(richTextBox.SelectionFont.FontFamily, newSize, richTextBox.SelectionFont.Style);

                // toolStripComboBoxYaziBoyutu'nu güncelle
                UpdateComboBoxFontSize(newSize);
            }
        }

        private void UpdateComboBoxFontSize(float newSize)
        {
            string newSizeString = newSize.ToString();

            // Eğer yeni boyut zaten listede varsa, onu seç
            if (toolStripComboBoxYaziBoyutu.Items.Contains(newSizeString))
            {
                toolStripComboBoxYaziBoyutu.SelectedItem = newSizeString;
            }
            else
            {
                // Değilse, yeni değeri listeye ekle ve seç
                toolStripComboBoxYaziBoyutu.Items.Add(newSizeString);
                toolStripComboBoxYaziBoyutu.SelectedItem = newSizeString;
            }
        }

        class NativeMethods
        {
            public const int WM_VSCROLL = 0x115;
            public const int SB_PAGEUP = 2;
            public const int SB_PAGEDOWN = 3;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, IntPtr lParam);
        }



        #endregion

        private void yazimBicimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YaziTipi yaziTipiFormu = new YaziTipi(richTextBox.Font);
            yaziTipiFormu.FontChanged += ApplySelectedFont;
            yaziTipiFormu.ShowDialog();
        }

        public void ApplySelectedFont(Font newFont)
        {
            if (richTextBox.SelectionLength > 0)
            {
                richTextBox.SelectionFont = newFont;
            }
            else
            {
                richTextBox.Font = newFont;
            }
        }

        private void toolStripComboBoxYaziTipi_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBoxYaziBoyutu_Click(object sender, EventArgs e)
        {

        }

        private void metinKarsilastiriciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MetinKarsilastirici karakterSayaci = new MetinKarsilastirici();
            karakterSayaci.Show();
        }

        private void yeniStripButton_Click(object sender, EventArgs e)
        {
            yeniToolStripMenuItem_Click(sender, e);
        }

        private void yeniPencereStripButton_Click(object sender, EventArgs e)
        {
            yeniPencereToolStripMenuItem_Click(sender, e);
        }

        private void AcStripButton_Click(object sender, EventArgs e)
        {
            acToolStripMenuItem_Click(sender, e);
        }

        private void kaydetStripButton_Click(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem_Click(sender, e);
        }

        private void farkliKaydetStripButton_Click(object sender, EventArgs e)
        {
            farkliKaydetToolStripMenuItem_Click(sender, e);
        }

        private void yazdirStripButton_Click(object sender, EventArgs e)
        {
            yazdırToolStripMenuItem_Click(sender, e);
        }

        private void bulStripButton_Click(object sender, EventArgs e)
        {
            bulToolStripMenuItem_Click(sender, e);
        }

        private void saatZamanStripButton_Click(object sender, EventArgs e)
        {
            saatTarihToolStripMenuItem_Click(sender, e);
        }

        private void metinKarsilastiriciStripButton_Click(object sender, EventArgs e)
        {
            metinKarsilastiriciToolStripMenuItem_Click(sender, e);
        }

        private void hesapMakinesiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HesapMakinasi hesapMakinasi = new HesapMakinasi();
            hesapMakinasi.Show();
        }

        private void toolStripComboBoxYaziTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçili yazı tipini ve mevcut boyutu kullanarak yeni Font nesnesi oluştur
            string seciliYaziTipi = toolStripComboBoxYaziTipi.SelectedItem.ToString();
            float mevcutBoyut = richTextBox.Font.Size;
            richTextBox.Font = new Font(seciliYaziTipi, mevcutBoyut, richTextBox.Font.Style);
        }

        private void toolStripComboBoxYaziBoyutu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçili yazı boyutunu ve mevcut yazı tipini kullanarak yeni Font nesnesi oluştur
            if (float.TryParse(toolStripComboBoxYaziBoyutu.SelectedItem.ToString(), out float seciliBoyut) && seciliBoyut > 0)
            {
                richTextBox.Font = new Font(richTextBox.Font.FontFamily, seciliBoyut, richTextBox.Font.Style);
            }
            else
            {
                // Başarısız parse işlemi için hata mesajı göster veya varsayılan bir değer kullan
                richTextBox.Font = new Font(richTextBox.Font.FontFamily, 12.0f, richTextBox.Font.Style);
            }
        }

        private void toolStripComboBoxYaziBoyutu_Validating(object sender, CancelEventArgs e)
        {
            // Kullanıcının girdiği değeri doğrula
            if (!float.TryParse(toolStripComboBoxYaziBoyutu.Text, out float newSize))
            {
                MessageBox.Show("Geçersiz yazı boyutu girdiniz. Lütfen geçerli bir sayı girin.");
                e.Cancel = true; // Geçersiz girişi kabul etme
            }
            else
            {
                // Geçerli ise, yazı boyutunu ayarla
                string mevcutYaziTipi = richTextBox.Font.FontFamily.Name;
                FontStyle mevcutStil = richTextBox.Font.Style;
                richTextBox.Font = new Font(mevcutYaziTipi, newSize, mevcutStil);
            }
        }

        private void richTextBox_FontChanged(object sender, EventArgs e)
        {
            // Yazı tipi değiştiğinde toolStripComboBoxYaziTipi ve toolStripComboBoxYaziBoyutu'nu güncelle
            toolStripComboBoxYaziTipi.SelectedItem = richTextBox.Font.FontFamily.Name;
            toolStripComboBoxYaziBoyutu.SelectedItem = richTextBox.Font.Size.ToString();
        }

        private void menulerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Menü öğesinin 'Checked' durumunu tersine çevir
            menulerToolStripMenuItem.Checked = !menulerToolStripMenuItem.Checked;

            // Durum çubuğunun görünürlüğünü menü öğesinin 'Checked' durumuna bağla
            toolStrip1.Visible = menulerToolStripMenuItem.Checked;
            toolStrip2.Visible = menulerToolStripMenuItem.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // menulerToolStripMenuItem'nin Checked durumunu kontrol et
            if (!menulerToolStripMenuItem.Checked)
            {
                // Eğer menüler etkin değilse, herhangi bir uyarı gösterme
                pbUyari.Visible = false;
                lblUyari.Visible = false;
                return; // Ve metodu erken bitir
            }
            // CapsLock veya Insert tuşunun etkin olup olmadığını kontrol et
            bool capsLockActive = Control.IsKeyLocked(Keys.CapsLock);
            bool insertActive = Control.IsKeyLocked(Keys.Insert);

            // Uyarıları ayarla
            if (capsLockActive && insertActive)
            {
                lblUyari.Text = "CapsLock ve Insert AÇIK!";
                pbUyari.Visible = true;
                lblUyari.Visible = true;
            }
            else if (capsLockActive)
            {
                lblUyari.Text = "CapsLock AÇIK!";
                pbUyari.Visible = true;
                lblUyari.Visible = true;
            }
            else if (insertActive)
            {
                lblUyari.Text = "Insert AÇIK!";
                pbUyari.Visible = true;
                lblUyari.Visible = true;
            }
            else
            {
                // Eğer CapsLock ve Insert kapalıysa, uyarıyı gizle
                pbUyari.Visible = false;
                lblUyari.Visible = false;
            }

            // Tarih ve saati kontrol et
            DateTime simdi = DateTime.Now;
            DateTime mesaiBaslangic = new DateTime(simdi.Year, simdi.Month, simdi.Day, 8, 0, 0);
            DateTime mesaiBitis = new DateTime(simdi.Year, simdi.Month, simdi.Day, 18, 1, 0);

            if (simdi >= mesaiBaslangic && simdi <= mesaiBitis)
            {
                // Mesai bitimine kalan süreyi hesapla
                TimeSpan kalanSure = mesaiBitis - simdi;
                // Kalan süreyi "saat:dakika" formatında yazdır
                //toolStripStatusLabel6.Text = $"{kalanSure.Hours} saat {kalanSure.Minutes} dakika";// mesai saati gerekirse yorum satırını kaldır
                toolStripStatusLabel6.Text = "";
            }
            else
            {
                // Mesai dışı bir saat dilimindeyse, toolStripStatusLabel6'ya bir şey yazma
                toolStripStatusLabel6.Text = "";
            }
        }

        private bool isDarkModeEnabled = false; // Koyu mod durumunu takip eden değişken

        private void ToggleDarkMode()
        {
            
            
                // Koyu modu etkinleştir
                this.BackColor = Color.FromArgb(45, 45, 48); // Koyu arka plan rengi
                richTextBox.BackColor = Color.FromArgb(30, 30, 30); // RichTextBox için koyu arka plan
                richTextBox.ForeColor = Color.WhiteSmoke; // Açık metin rengi
                menuStrip.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                menuStrip.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                toolStrip1.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                toolStrip1.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                toolStrip2.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                toolStrip2.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                statusStrip1.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                statusStrip1.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                toolStripMenuItem1.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                toolStripMenuItem1.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                düzenToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                düzenToolStripMenuItem.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                biçimToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                biçimToolStripMenuItem.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                görünümToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                görünümToolStripMenuItem.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                uygulamalarToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                uygulamalarToolStripMenuItem.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi
                yardımToolStripMenuItem.BackColor = Color.FromArgb(37, 37, 38); // Menü strip için koyu arka plan
                yardımToolStripMenuItem.ForeColor = Color.WhiteSmoke; // Menü strip için açık metin rengi


            isDarkModeEnabled = true;

            Properties.Settings.Default["ThemeMode"] = "Dark";
            Properties.Settings.Default.Save(); // Ayarı kaydet
            UpdateMenuChecks("Dark");
        }
        private void koyuModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Koyu modu etkinleştir veya devre dışı bırak
            ToggleDarkMode();
        }

        private void acikMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Açık modu etkinleştir
            ToggleLightMode();
        }

        private void ToggleLightMode()
        {
            // Koyu modu devre dışı bırak
            this.BackColor = SystemColors.Control; // Formun arka plan rengi
            richTextBox.BackColor = Color.White; // RichTextBox'ın arka plan rengi
            richTextBox.ForeColor = Color.Black; // RichTextBox'ın metin rengi
            menuStrip.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            menuStrip.ForeColor = Color.Black; // Menü strip'in metin rengi
            toolStrip1.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            toolStrip1.ForeColor = Color.Black; // Menü strip'in metin rengi
            toolStrip2.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            toolStrip2.ForeColor = Color.Black; // Menü strip'in metin rengi
            statusStrip1.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            statusStrip1.ForeColor = Color.Black; // Menü strip'in metin rengi
            toolStripMenuItem1.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            toolStripMenuItem1.ForeColor = Color.Black; // Menü strip'in metin rengi
            düzenToolStripMenuItem.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            düzenToolStripMenuItem.ForeColor = Color.Black; // Menü strip'in metin rengi
            biçimToolStripMenuItem.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            biçimToolStripMenuItem.ForeColor = Color.Black; // Menü strip'in metin rengi
            görünümToolStripMenuItem.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            görünümToolStripMenuItem.ForeColor = Color.Black; // Menü strip'in metin rengi
            uygulamalarToolStripMenuItem.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            uygulamalarToolStripMenuItem.ForeColor = Color.Black; // Menü strip'in metin rengi
            yardımToolStripMenuItem.BackColor = SystemColors.Control; // Menü strip'in arka plan rengi
            yardımToolStripMenuItem.ForeColor = Color.Black; // Menü strip'in metin rengi

            // Koyu mod durumunu false yap (açık mod aktif)
            isDarkModeEnabled = false;

            Properties.Settings.Default["ThemeMode"] = "Light";
            Properties.Settings.Default.Save(); // Ayarı kaydet
            UpdateMenuChecks("Light");
        }

        private void UpdateMenuChecks(string mode)
        {
            // Menü öğelerindeki işaretleri güncelle
            acikMToolStripMenuItem.Checked = (mode == "Light");
            koyuModToolStripMenuItem.Checked = (mode == "Dark");
        }

        private void dikte_Click(object sender, EventArgs e)
        {
            if (!SpeechRecognitionEngine.InstalledRecognizers().Any())
            {
                MessageBox.Show("Sistemde tanımlı mikrofon görünmüyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var confirmResult = MessageBox.Show("Dikte özelliğini açmak istiyor musunuz?", "Dikte", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                if (recognizer == null)
                {
                    InitializeSpeechRecognition();
                }
                else
                {
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);
                }
            }
        }
    }

}