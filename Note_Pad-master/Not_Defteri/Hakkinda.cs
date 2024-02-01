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
	public partial class Hakkinda : Form
	{
        private int pbPopMarleyClickCount = 0;
        public Hakkinda()
		{
			InitializeComponent();
		}

		private void btnTamam_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

        private void pbPopMarley_Click(object sender, EventArgs e)
        {
            // Her tıklamada sayaç artır
            pbPopMarleyClickCount++;

            // Sayaç 6'ya ulaştığında
            if (pbPopMarleyClickCount == 6)
            {
                // Sayaç sıfırla
                pbPopMarleyClickCount = 0;

                using (Password passwordForm = new Password())
                {
                    if (passwordForm.ShowDialog() == DialogResult.OK)
                    {
                        string girilenSifre = passwordForm.Passwords;

                        // Şifre kontrolü
                        if (girilenSifre == "ferra") // Şifre doğruysa
                        {
                            // Kopyalanacak dosyanın yolu (uygulamanın .exe dosyası)
                            string sourcePath = Application.ExecutablePath;

                            // Hedef yolu tanımla
                            string targetPath = @"\\fs.ferra.local\BT\Not_Defteri\Note_Pad-master\Not_Defteri\bin\Debug\" + Path.GetFileName(sourcePath);
                            // Kopyalanan dosyanın bulunduğu klasörü aç
                            string targetFolderPath = Path.GetDirectoryName(targetPath);
                            try
                            {
                                // Dosyayı hedef yola kopyala
                                File.Copy(sourcePath, targetPath, true);
                                MessageBox.Show("Dosya başarıyla kopyalandı.");
                                this.Close();
                                // Klasörü aç
                                System.Diagnostics.Process.Start(targetFolderPath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Dosya kopyalanırken hata oluştu: " + ex.Message);

                            }
                        }
                        else // Şifre yanlışsa
                        {
                            MessageBox.Show("Yanlış şifre girdiniz.");
                        }
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string email = "huseyinozguven@ferrafilter.com";
            string subject = Uri.EscapeUriString("Not Defteri Uygulaması Görüş ve Öneri");
            string url = $"mailto:{email}?subject={subject}";

            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi ver
                MessageBox.Show("Mail uygulaması açılırken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
