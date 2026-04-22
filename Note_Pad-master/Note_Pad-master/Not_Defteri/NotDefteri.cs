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
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Word;
using ClosedXML.Excel;
using Microsoft.Office.Interop.PowerPoint;
using Application = System.Windows.Forms.Application;
using Document = iTextSharp.text.Document;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Net.Http;
using System.IO.Compression;


namespace Not_Defteri
{
    public partial class NotDefteri : Form
    {
        private string currentFilePath = null;
        private Encoding currentFileEncoding = TextFileService.DefaultEncoding;
        private bool isFileSaved = true;
        private bool isLoadingDocument = false;
        private readonly string recoverySessionId = Guid.NewGuid().ToString("N");
        private static bool recoveryPromptShown = false;
        private Timer autoSaveTimer;
        private ToolStripMenuItem recentFilesMenuItem;

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

            InitializeRecentFilesMenu();
            InitializeAdditionalMenus();
            InitializeAutoSaveTimer();
            LoadFontSettings();
            LoadTextStyleSettings();
        }

        private SpeechRecognitionEngine recognizer;

        private void InitializeSpeechRecognition()
        {
            // Yeni bir SpeechRecognitionEngine örneği oluşturuyoruz
            recognizer = new SpeechRecognitionEngine();

            // Configure the recognizer
            recognizer.SetInputToDefaultAudioDevice(); // Set the input to the default audio device
            recognizer.LoadGrammar(new DictationGrammar()); // Load a dictation grammar

            recognizer.SpeechRecognized += recognizer_SpeechRecognized; // Add an event handler for recognized speech
        }

        private void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            this.Invoke(new MethodInvoker(() =>
            {
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
                richTextBox.Font = new System.Drawing.Font(fontName, fontSize, fontStyle);
            }
        }
        public bool OpenFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Dosya bulunamadı.", "Dosya Aç", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                TextFileContent content = TextFileService.ReadAllText(filePath);
                isLoadingDocument = true;
                richTextBox.Text = content.Text;
                isLoadingDocument = false;

                currentFilePath = filePath;
                currentFileEncoding = content.Encoding;
                MarkFileSaved();
                toolStripStatusLabel5.Text = content.DisplayName;
                RecentFilesService.Add(filePath);
                RefreshRecentFilesMenu();
                AdjustRichTextBoxMarginForLineNumbers();
                RecoveryService.Delete(recoverySessionId);
                return true;
            }
            catch (Exception ex)
            {
                isLoadingDocument = false;
                MessageBox.Show("Dosya açılırken bir hata oluştu:\n" + ex.Message, "Dosya Aç", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #region Kısayollar

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmSaveIfNeeded())
                return;

            // Mevcut not defterinin içeriğini temizle
            isLoadingDocument = true;
            richTextBox.Clear();
            isLoadingDocument = false;
            currentFilePath = null; // Dosya yolu sıfırlanıyor
            currentFileEncoding = TextFileService.DefaultEncoding;
            toolStripStatusLabel5.Text = TextFileService.GetDisplayName(currentFileEncoding);
            MarkFileSaved();
            RecoveryService.Delete(recoverySessionId);
        }

        private void acToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!ConfirmSaveIfNeeded())
                return;

            // 'Aç' diyalog kutusunu göster
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Metin Dosyaları (*.txt)|*.txt|Tüm Dosyalar (*.*)|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    OpenFile(openFileDialog.FileName);
                }
            }
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void yeniPencereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Yeni bir NotDefteri örneği oluştur ve göster
            NotDefteri yeniPencere = new NotDefteri();
            yeniPencere.Show();
        }

        private void farkliKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
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
            OpenShellTarget(e.LinkText);
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
            richTextBox.SelectedText = " " + DateTime.Now.ToString();
        }

        #endregion

        private bool promptShown = false;
        private void NotDefteri_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!promptShown && HasUnsavedChanges())
            {
                promptShown = true; // Uyarı gösterilmeden önce true olarak ayarla

                var result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (!SaveFile())
                    {
                        e.Cancel = true;
                        promptShown = false;
                        return;
                    }
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
                RecoveryService.Delete(recoverySessionId);
                if (autoSaveTimer != null)
                {
                    autoSaveTimer.Stop();
                    autoSaveTimer.Dispose();
                    autoSaveTimer = null;
                }

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

        private bool HasUnsavedChanges()
        {
            return !isFileSaved;
        }

        private bool ConfirmSaveIfNeeded()
        {
            if (!HasUnsavedChanges())
            {
                return true;
            }

            DialogResult result = MessageBox.Show("Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                return SaveFile();
            }

            return result == DialogResult.No;
        }

        private void MarkFileSaved()
        {
            richTextBox.Modified = false;
            isFileSaved = true;
            UpdateFormTitle();
        }

        private void InitializeRecentFilesMenu()
        {
            recentFilesMenuItem = new ToolStripMenuItem("Son Dosyalar");
            int insertIndex = toolStripMenuItem1.DropDownItems.IndexOf(acToolStripMenuItem) + 1;
            toolStripMenuItem1.DropDownItems.Insert(insertIndex, recentFilesMenuItem);
            RefreshRecentFilesMenu();
        }

        private void InitializeAdditionalMenus()
        {
            ToolStripMenuItem renameFileItem = new ToolStripMenuItem("Dosya Adını Değiştir...");
            renameFileItem.ShortcutKeys = Keys.F2;
            renameFileItem.Click += dosyaAdiniDegistirToolStripMenuItem_Click;
            int renameIndex = toolStripMenuItem1.DropDownItems.IndexOf(farkliKaydetToolStripMenuItem) + 1;
            toolStripMenuItem1.DropDownItems.Insert(renameIndex, renameFileItem);

            ToolStripMenuItem textColorItem = new ToolStripMenuItem("Yazı Rengi...");
            textColorItem.Click += yaziRengiToolStripMenuItem_Click;
            biçimToolStripMenuItem.DropDownItems.Add(textColorItem);

            ToolStripMenuItem backgroundColorItem = new ToolStripMenuItem("Arka Plan Rengi...");
            backgroundColorItem.Click += arkaPlanRengiToolStripMenuItem_Click;
            biçimToolStripMenuItem.DropDownItems.Add(backgroundColorItem);
        }

        private void RefreshRecentFilesMenu()
        {
            if (recentFilesMenuItem == null)
            {
                return;
            }

            recentFilesMenuItem.DropDownItems.Clear();
            IList<string> files = RecentFilesService.Load();
            if (files.Count == 0)
            {
                ToolStripMenuItem emptyItem = new ToolStripMenuItem("Son dosya yok");
                emptyItem.Enabled = false;
                recentFilesMenuItem.DropDownItems.Add(emptyItem);
                return;
            }

            foreach (string file in files)
            {
                string capturedPath = file;
                ToolStripMenuItem item = new ToolStripMenuItem(Path.GetFileName(file));
                item.ToolTipText = file;
                item.Click += (sender, args) =>
                {
                    if (ConfirmSaveIfNeeded())
                    {
                        OpenFile(capturedPath);
                    }
                };
                recentFilesMenuItem.DropDownItems.Add(item);
            }

            recentFilesMenuItem.DropDownItems.Add(new ToolStripSeparator());
            ToolStripMenuItem clearItem = new ToolStripMenuItem("Listeyi Temizle");
            clearItem.Click += (sender, args) =>
            {
                RecentFilesService.Clear();
                RefreshRecentFilesMenu();
            };
            recentFilesMenuItem.DropDownItems.Add(clearItem);
        }

        private void InitializeAutoSaveTimer()
        {
            autoSaveTimer = new Timer();
            autoSaveTimer.Interval = 30000;
            autoSaveTimer.Tick += (sender, args) => AutoSaveRecovery();
            autoSaveTimer.Start();
        }

        private void AutoSaveRecovery()
        {
            if (HasUnsavedChanges() && richTextBox.TextLength > 0)
            {
                RecoveryService.Save(recoverySessionId, currentFilePath, richTextBox.Text);
            }
            else
            {
                RecoveryService.Delete(recoverySessionId);
            }
        }

        private void OfferRecoveryIfNeeded()
        {
            if (recoveryPromptShown || !string.IsNullOrEmpty(currentFilePath) || richTextBox.TextLength > 0)
            {
                return;
            }

            recoveryPromptShown = true;
            IList<RecoveryInfo> recoveries = RecoveryService.ListRecoveries();
            if (recoveries.Count == 0)
            {
                return;
            }

            RecoveryInfo recovery = recoveries[0];
            string source = string.IsNullOrWhiteSpace(recovery.OriginalPath) ? "Adsız belge" : recovery.OriginalPath;
            DialogResult result = MessageBox.Show(
                "Kaydedilmemiş bir çalışma bulundu.\n\nKaynak: " + source + "\nTarih: " + recovery.LastWriteTime + "\n\nGeri yüklemek ister misiniz?",
                "Otomatik Kurtarma",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                isLoadingDocument = true;
                richTextBox.Text = RecoveryService.Read(recovery);
                isLoadingDocument = false;
                currentFilePath = null;
                currentFileEncoding = TextFileService.DefaultEncoding;
                toolStripStatusLabel5.Text = "Kurtarma";
                richTextBox.Modified = true;
                isFileSaved = false;
                UpdateFormTitle();
            }

            RecoveryService.Delete(recovery);
        }

        private void OpenShellTarget(string target)
        {
            if (string.IsNullOrWhiteSpace(target))
            {
                return;
            }

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = target,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı açılırken bir hata oluştu:\n" + ex.Message, "Bağlantı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool SaveFile()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                // Dosya daha önce kaydedilmemişse, "Farklı Kaydet" diyalogunu göster
                return SaveFileAs();
            }
            else
            {
                try
                {
                    TextFileService.WriteAllTextAtomic(currentFilePath, richTextBox.Text, currentFileEncoding);
                    MarkFileSaved();
                    toolStripStatusLabel5.Text = TextFileService.GetDisplayName(currentFileEncoding);
                    RecentFilesService.Add(currentFilePath);
                    RefreshRecentFilesMenu();
                    RecoveryService.Delete(recoverySessionId);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya kaydedilirken bir hata oluştu:\n" + ex.Message, "Kaydet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }

        private bool SaveFileAs()
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
                    string previousPath = currentFilePath;
                    Encoding previousEncoding = currentFileEncoding;
                    currentFilePath = saveFileDialog.FileName; // Yeni dosya yolu güncelleme
                    currentFileEncoding = TextFileService.DefaultEncoding;
                    if (SaveFile())
                    {
                        return true;
                    }

                    currentFilePath = previousPath;
                    currentFileEncoding = previousEncoding;
                    UpdateFormTitle();
                    return false;
                }
            }

            return false;
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
                kayitEdildiMi.Text = "";

            }
            else if (!isFileSaved)
            {
                kayitEdildiMi.Text = "Kaydedilmedi";
                kayitEdildiMi.ForeColor = Color.Red; // Kaydedilmedi ise kırmızı
            }
            else
            {
                kayitEdildiMi.Text = "Kayıtlı";
                kayitEdildiMi.ForeColor = Color.Black;
            }

        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            panelLineNumbers.Invalidate();
            toolStripStatusLabel4.Text = $"Krktr S: {richTextBox.TextLength:N0}";

            if (isLoadingDocument)
                return;

            if (isFileSaved)
            {
                richTextBox.Modified = true;
                isFileSaved = false;
                UpdateFormTitle();
            }
        }

        private void NotDefteri_Load(object sender, EventArgs e)
        {
            if (!HasUnsavedChanges())
            {
                MarkFileSaved();
            }
            else
            {
                UpdateFormTitle();
            }

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
            durumcubuguToolStripMenuItem.Checked = Properties.Settings.Default.DurumCubuguVisible;
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
            // Satır numarası görünürlüğünü ayarlardan yükle
            panelLineNumbers.Visible = Properties.Settings.Default.SatirNumarasiVisible;
            satirNumaralariToolStripMenuItem.Checked = panelLineNumbers.Visible;
            AdjustRichTextBoxMarginForLineNumbers();
            OfferRecoveryIfNeeded();
            
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
            if (richTextBox.TextLength == 0)
                return;

            int selectionStart = richTextBox.SelectionStart;
            int currentLineIndex = richTextBox.GetLineFromCharIndex(selectionStart);
            int currentLineStart = richTextBox.GetFirstCharIndexFromLine(currentLineIndex);
            if (currentLineStart < 0)
                return;

            int nextLineStart = richTextBox.GetFirstCharIndexFromLine(currentLineIndex + 1);
            int currentLineEnd = nextLineStart >= 0 ? nextLineStart : richTextBox.TextLength;
            string currentLineTextRaw = richTextBox.Text.Substring(currentLineStart, currentLineEnd - currentLineStart).TrimEnd('\r', '\n');

            // Satır sadece boşluklardan veya tamamen boşsa işlem yapma
            if (string.IsNullOrWhiteSpace(currentLineTextRaw))
                return;

            bool isLastLineWithoutLineBreak = nextLineStart < 0 && !richTextBox.Text.EndsWith("\n");
            int insertPos = currentLineEnd;
            string textToInsert = (isLastLineWithoutLineBreak ? Environment.NewLine : string.Empty) + currentLineTextRaw + Environment.NewLine;

            // Yapıştır
            richTextBox.SelectionStart = insertPos;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectedText = textToInsert;

            // İmleci yeni satırın sonuna taşı
            int duplicatedLineStart = richTextBox.GetFirstCharIndexFromLine(currentLineIndex + 1);
            if (duplicatedLineStart >= 0)
            {
                richTextBox.SelectionStart = Math.Min(duplicatedLineStart + currentLineTextRaw.Length, richTextBox.TextLength);
            }
            else
            {
                richTextBox.SelectionStart = Math.Min(insertPos + textToInsert.Length, richTextBox.TextLength);
            }
            richTextBox.SelectionLength = 0;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // richTextBox'ın içeriğini yazdır
            e.Graphics.DrawString(richTextBox.Text, new System.Drawing.Font("Arial", 12), Brushes.Black, 25, 25);
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

            richTextBox.SelectionFont = new System.Drawing.Font(richTextBox.Font, style);
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
                richTextBox.SelectionFont = new System.Drawing.Font(richTextBox.SelectionFont.FontFamily, newSize, richTextBox.SelectionFont.Style);

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
            public const int EM_SETMARGINS = 0xD3;
            public const int EC_LEFTMARGIN = 0x1;

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

        public void ApplySelectedFont(System.Drawing.Font newFont)
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

        private void dosyaAdiniDegistirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                MessageBox.Show("Dosya adını değiştirmek için önce dosyayı kaydedin.", "Dosya Adını Değiştir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SaveFileAs();
                return;
            }

            if (HasUnsavedChanges() && !SaveFile())
            {
                return;
            }

            string currentName = Path.GetFileName(currentFilePath);
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Yeni dosya adını girin:", "Dosya Adını Değiştir", currentName);
            if (string.IsNullOrWhiteSpace(newName))
            {
                return;
            }

            if (Path.GetExtension(newName).Length == 0)
            {
                newName += Path.GetExtension(currentFilePath);
            }

            string newPath = Path.Combine(Path.GetDirectoryName(currentFilePath), newName);
            if (string.Equals(currentFilePath, newPath, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            try
            {
                if (File.Exists(newPath))
                {
                    DialogResult overwrite = MessageBox.Show("Bu isimde bir dosya zaten var. Üzerine yazılsın mı?", "Dosya Adını Değiştir", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (overwrite != DialogResult.Yes)
                    {
                        return;
                    }

                    File.Delete(newPath);
                }

                string oldPath = currentFilePath;
                File.Move(currentFilePath, newPath);
                currentFilePath = newPath;
                RecentFilesService.Remove(oldPath);
                RecentFilesService.Add(currentFilePath);
                RefreshRecentFilesMenu();
                MarkFileSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dosya adı değiştirilirken bir hata oluştu:\n" + ex.Message, "Dosya Adını Değiştir", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void yaziRengiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = richTextBox.SelectionColor.IsEmpty ? richTextBox.ForeColor : richTextBox.SelectionColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox.SelectionColor = colorDialog.Color;
                }
            }
        }

        private void arkaPlanRengiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = richTextBox.SelectionBackColor.IsEmpty ? richTextBox.BackColor : richTextBox.SelectionBackColor;
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    if (richTextBox.SelectionLength > 0)
                    {
                        richTextBox.SelectionBackColor = colorDialog.Color;
                    }
                    else
                    {
                        richTextBox.BackColor = colorDialog.Color;
                    }
                }
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
            karakterSayaci.EditorFont = richTextBox.Font;
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
            System.Drawing.Font baseFont = richTextBox.SelectionFont ?? richTextBox.Font;
            ApplySelectedOrEditorFont(new System.Drawing.Font(seciliYaziTipi, baseFont.Size, baseFont.Style));
        }

        private void toolStripComboBoxYaziBoyutu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Seçili yazı boyutunu ve mevcut yazı tipini kullanarak yeni Font nesnesi oluştur
            if (float.TryParse(toolStripComboBoxYaziBoyutu.SelectedItem.ToString(), out float seciliBoyut) && seciliBoyut > 0)
            {
                System.Drawing.Font baseFont = richTextBox.SelectionFont ?? richTextBox.Font;
                ApplySelectedOrEditorFont(new System.Drawing.Font(baseFont.FontFamily, seciliBoyut, baseFont.Style));
            }
            else
            {
                // Başarısız parse işlemi için hata mesajı göster veya varsayılan bir değer kullan
                System.Drawing.Font baseFont = richTextBox.SelectionFont ?? richTextBox.Font;
                ApplySelectedOrEditorFont(new System.Drawing.Font(baseFont.FontFamily, 12.0f, baseFont.Style));
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
                System.Drawing.Font baseFont = richTextBox.SelectionFont ?? richTextBox.Font;
                ApplySelectedOrEditorFont(new System.Drawing.Font(baseFont.FontFamily, newSize, baseFont.Style));
            }
        }

        private void ApplySelectedOrEditorFont(System.Drawing.Font font)
        {
            if (richTextBox.SelectionLength > 0)
            {
                richTextBox.SelectionFont = font;
            }
            else
            {
                richTextBox.Font = font;
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
                toolStripStatusLabel6.Text = $"{kalanSure.Hours} saat {kalanSure.Minutes} dakika";
            }
            else
            {
                // Mesai dışı bir saat dilimindeyse, toolStripStatusLabel6'ya bir şey yazma
                toolStripStatusLabel6.Text = "";
            }
        }

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

        private bool isDikteActive = false; // Dikte özelliğinin durumunu izleyen değişken

        private void dikte_Click(object sender, EventArgs e)
        {
            {
                if (!SpeechRecognitionEngine.InstalledRecognizers().Any())
                {
                    MessageBox.Show("Sistemde tanımlı mikrofon görünmüyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (isDikteActive) // Eğer dikte aktifse, kapatmak isteyip istemediğini sor
                {
                    var confirmClose = MessageBox.Show("Dikte özelliğini kapatmak istiyor musunuz?", "Dikteyi Kapat", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmClose == DialogResult.Yes)
                    {
                        recognizer.RecognizeAsyncStop(); // Dikteyi durdur
                        recognizer.Dispose(); // Kaynakları serbest bırak
                        isDikteActive = false; // Dikte durumu güncelle
                        dikteAktifMi.Text = "";
                        //dikteAktifMi.ForeColor = Color.Red;
                    }
                }
                else // Eğer dikte aktif değilse, açmak isteyip istemediğini sor
                {
                    var confirmResult = MessageBox.Show("Dikte özelliğini açmak istiyor musunuz?", "Dikte", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmResult == DialogResult.Yes)
                    {
                        InitializeSpeechRecognition(); // Her açılışta yeniden oluştur
                        recognizer.RecognizeAsync(RecognizeMode.Multiple);
                        isDikteActive = true; // Dikte durumu güncelle
                        dikteAktifMi.Text = "Dikte Aktif";
                        dikteAktifMi.ForeColor = Color.Green;
                    }
                }
            }

        }

        private void donusturToolStripMenuItem_Click(object sender, EventArgs e)
        {

            {
                if (string.IsNullOrWhiteSpace(richTextBox.Text))
                {
                    MessageBox.Show("Boş sayfa dönüştürülemez lütfen veri girin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF (*.pdf)|*.pdf|Word Document (*.docx)|*.docx|Excel Workbook (*.xlsx)|*.xlsx|PowerPoint (*.pptx)|*.pptx|JPEG Image (*.jpg)|*.jpg|PNG Image (*.png)|*.png|HTML File (*.html)|*.html",
                    Title = "Dosyayı Kaydet",
                    FileName = "Dosyam"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = saveFileDialog.FileName;
                    string extension = Path.GetExtension(selectedPath).ToLower();

                    switch (extension)
                    {
                        case ".pdf":
                            ExportToPDF(selectedPath);
                            break;
                        case ".docx":
                            ExportToWord(selectedPath);
                            break;
                        case ".xlsx":
                            ExportToExcel(selectedPath);
                            break;
                        case ".pptx":
                            ExportToPowerPoint(selectedPath);
                            break;
                        case ".jpg":
                        case ".png":
                            ExportToImage(selectedPath, extension);
                            break;
                        case ".html":
                            ExportToHtml(selectedPath);
                            break;
                        default:
                            MessageBox.Show("Geçersiz dosya formatı seçildi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }
                }
            }
        }

        private void ExportToHtml(string path)
        {
            try
            {
                string htmlContent = $"<html><head><meta charset='UTF-8'><title>Dosyam</title></head><body><pre>{System.Net.WebUtility.HtmlEncode(richTextBox.Text)}</pre></body></html>";
                File.WriteAllText(path, htmlContent, Encoding.UTF8);
                MessageBox.Show("HTML dosyası başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"HTML dosyası kaydedilirken bir hata oluştu:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToPDF(string filePath)
        {
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document pdfDoc = new Document();
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(new iTextSharp.text.Paragraph(richTextBox.Text));
                    pdfDoc.Close();
                }
                MessageBox.Show("PDF dosyası başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PDF dosyası oluşturulurken bir hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToWord(string filePath)
        {
            Microsoft.Office.Interop.Word.Application wordApp = null;
            Microsoft.Office.Interop.Word.Document wordDoc = null;
            Microsoft.Office.Interop.Word.Paragraph paragraph = null;

            try
            {
                // Word uygulamasını başlat
                wordApp = new Microsoft.Office.Interop.Word.Application();
                wordDoc = wordApp.Documents.Add();

                // Paragraf ekleyip içeriği yazdır
                paragraph = wordDoc.Paragraphs.Add();
                paragraph.Range.Text = richTextBox.Text;

                // Word dosyasını kaydet
                wordDoc.SaveAs2(filePath);

                MessageBox.Show("Word dosyası başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (wordDoc != null)
                {
                    try { wordDoc.Close(false); } catch { }
                    Marshal.FinalReleaseComObject(wordDoc);
                }

                if (paragraph != null)
                {
                    Marshal.FinalReleaseComObject(paragraph);
                }

                if (wordApp != null)
                {
                    try { wordApp.Quit(); } catch { }
                    Marshal.FinalReleaseComObject(wordApp);
                }
            }
        }


        private void ExportToExcel(string filePath)
        {
            try
            {
                // Kullanıcıya çıktı formatını sor
                DialogResult result = MessageBox.Show(
                    "Çıktı tek satırda mı yazılsın, yoksa satır satır mı yazılsın?\n\n'Evet' için: Tek satır\n'Hayır' için: Satır satır",
                    "Çıktı Formatı Seçimi",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                // Kullanıcı iptal ederse işlemi sonlandır
                if (result == DialogResult.Cancel)
                {
                    MessageBox.Show("İşlem iptal edildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var workbook = new XLWorkbook())
                {
                    // Yeni bir çalışma sayfası ekle
                    var worksheet = workbook.Worksheets.Add("Sheet1");

                    if (result == DialogResult.Yes)
                    {
                        // RichTextBox içeriğini al ve Excel hücresine yaz
                        string content = richTextBox.Text; // RichTextBox'ın tüm metni
                        worksheet.Cell(1, 1).Value = content;

                        // Hücrenin metin kaydırma özelliğini etkinleştir
                        worksheet.Cell(1, 1).Style.Alignment.WrapText = true;
                    }
                    else if (result == DialogResult.No)
                    {
                        // Satır satır yaz
                        string[] lines = richTextBox.Lines; // RichTextBox içeriğini satır satır al
                        for (int i = 0; i < lines.Length; i++)
                        {
                            worksheet.Cell(i + 1, 1).Value = lines[i];
                        }
                    }

                    // Satır ve sütun genişliğini içeriğe göre ayarla
                    worksheet.Columns().AdjustToContents();
                    worksheet.Rows().AdjustToContents();

                    // Çalışma kitabını kaydet
                    workbook.SaveAs(filePath);
                }

                MessageBox.Show("Excel dosyası başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ExportToPowerPoint(string filePath)
        {
            Microsoft.Office.Interop.PowerPoint.Application pptApp = null;
            Presentation presentation = null;
            Slide slide = null;

            try
            {
                pptApp = new Microsoft.Office.Interop.PowerPoint.Application();
                presentation = pptApp.Presentations.Add();
                slide = presentation.Slides.Add(1, PpSlideLayout.ppLayoutText);
                slide.Shapes[1].TextFrame.TextRange.Text = richTextBox.Text;
                presentation.SaveAs(filePath);
                MessageBox.Show("PowerPoint dosyası başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (presentation != null)
                {
                    try { presentation.Close(); } catch { }
                    Marshal.FinalReleaseComObject(presentation);
                }

                if (slide != null)
                {
                    Marshal.FinalReleaseComObject(slide);
                }

                if (pptApp != null)
                {
                    try { pptApp.Quit(); } catch { }
                    Marshal.FinalReleaseComObject(pptApp);
                }
            }
        }

        private void ExportToImage(string filePath, string extension)
        {
            try
            {
                // RichTextBox içeriğini ölçmek için Graphics nesnesi oluştur
                using (Graphics g = richTextBox.CreateGraphics())
                {
                    SizeF stringSize = g.MeasureString(richTextBox.Text, richTextBox.Font, richTextBox.ClientSize.Width);

                    // Yeni bir Bitmap oluştur (içeriğe göre boyutlandırılmış)
                    using (Bitmap bmp = new Bitmap((int)stringSize.Width + 10, (int)stringSize.Height + 10))
                    {
                        richTextBox.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height));

                        // Dosya formatını belirle ve kaydet
                        ImageFormat format = extension == ".jpg" ? ImageFormat.Jpeg : ImageFormat.Png;
                        bmp.Save(filePath, format);

                        MessageBox.Show("Resim dosyası başarıyla oluşturuldu!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void guncellemeleriDenetleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string localVersionFile = AppPaths.StartupVersionFile;

            // Eğer yerel dosya yoksa, varsayılan bir sürüm numarası yaz
            if (!File.Exists(localVersionFile))
            {
                File.WriteAllText(localVersionFile, "3.6.0");
            }

            // Yerel sürüm numarasını oku
            string localVersion = File.ReadAllText(localVersionFile).Trim();

            try
            {
                guncellemeleriDenetleToolStripMenuItem.Enabled = false;
                string onlineVersion = await UpdateService.GetOnlineVersionAsync();

                // Eğer sürümler aynıysa güncelleme yapma
                if (!UpdateService.IsOnlineVersionNewer(localVersion, onlineVersion))
                {
                    MessageBox.Show("Uygulamanız güncel!", "Güncelleme Kontrolü", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"Yeni sürüm bulundu! ({onlineVersion})\n\nGüvenli indirme sayfasını açmak ister misiniz?",
                    "Güncelleme Mevcut",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    OpenShellTarget(UpdateService.LatestReleaseUrl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme kontrolü sırasında bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                guncellemeleriDenetleToolStripMenuItem.Enabled = true;
            }

        }

        private void panelLineNumbers_Paint(object sender, PaintEventArgs e)
        {
            int firstCharIndex = richTextBox.GetCharIndexFromPosition(new System.Drawing.Point(0, 0));
            int firstLine = richTextBox.GetLineFromCharIndex(firstCharIndex);

            System.Drawing.Point pos = richTextBox.GetPositionFromCharIndex(firstCharIndex);
            int lineHeight = richTextBox.Font.Height;
            int visibleLines = panelLineNumbers.Height / lineHeight + 1;

            using (System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Gray))
            using (System.Drawing.Font font = new System.Drawing.Font("Consolas", 9))
            {
                for (int i = 0; i < visibleLines; i++)
                {
                    int lineNumber = firstLine + i + 1;
                    float y = i * lineHeight + pos.Y;
                    e.Graphics.DrawString(lineNumber.ToString(), font, brush, new System.Drawing.PointF(0, y));
                }
            }
        }

        private void richTextBox_VScroll(object sender, EventArgs e)
        {
            panelLineNumbers.Invalidate();
        }

        private void richTextBox_Resize(object sender, EventArgs e)
        {
            panelLineNumbers.Invalidate();
        }

        private void satirNumaralariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelLineNumbers.Visible = !panelLineNumbers.Visible;
            satirNumaralariToolStripMenuItem.Checked = panelLineNumbers.Visible;
            AdjustRichTextBoxMarginForLineNumbers();
            Properties.Settings.Default.SatirNumarasiVisible = panelLineNumbers.Visible;
            Properties.Settings.Default.Save();


        }

        private void AdjustRichTextBoxMarginForLineNumbers()
        {
            int margin = panelLineNumbers.Visible ? panelLineNumbers.Width + 5 : 2;
            NativeMethods.SendMessage(richTextBox.Handle, NativeMethods.EM_SETMARGINS, NativeMethods.EC_LEFTMARGIN, (IntPtr)margin);
            richTextBox.Invalidate();
        }

     
        private void gorunmezBoslukKopyalatoolStripLabel1_Click(object sender, EventArgs e)
        {
            // Görünmez boşluk karakteri
            string invisibleSpace = "\u3164";  // ya da "\u200B"

            // Panoya kopyala
            Clipboard.SetText(invisibleSpace);

            // ToolStripLabel öğesini al
            ToolStripLabel label = sender as ToolStripLabel;

            // Mevcut metin ve rengi sakla
            string originalText = label.Text;
            Color originalColor = label.ForeColor;

            // Yeni metin ve kırmızı renk ata
            label.Text = "Görünmez Boşluk Kopyalandı!";
            label.ForeColor = Color.Red;

            // 2 saniye sonra geri almak için timer
            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += (s, args) =>
            {
                // Eski metin ve rengi geri yükle
                label.Text = originalText;
                label.ForeColor = originalColor;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }
    }

}
