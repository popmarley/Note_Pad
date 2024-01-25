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

            // Sayaç 6'ya ulaştığında, dosyayı kopyala
            if (pbPopMarleyClickCount == 6)
            {
                // Sayaç sıfırla
                pbPopMarleyClickCount = 0;

                // Kopyalanacak dosyanın yolu (uygulamanın .exe dosyası)
                string sourcePath = Application.ExecutablePath;

                // Hedef yolu tanımla
                string targetPath = @"\\fs.ferra.local\BT\Not_Defteri\Note_Pad-master\Not_Defteri\bin\Debug\" + Path.GetFileName(sourcePath);

                try
                {
                    // Dosyayı hedef yola kopyala
                    File.Copy(sourcePath, targetPath, true);
                    MessageBox.Show("Dosya başarıyla kopyalandı.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Dosya kopyalanırken hata oluştu: " + ex.Message);
                }
            }
        }
    }
}
