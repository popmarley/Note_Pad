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
    public partial class YaziTipi : Form
    {
        public Font SecilenYaziTipi { get; private set; }

        public YaziTipi(Font mevcutYaziTipi)
        {
            InitializeComponent();
            SecilenYaziTipi = mevcutYaziTipi;
            ListeleYaziTipleri();
            ListeleYaziTipiBoyutlari();
            SetInitialFontSelections();
        }

        private void ListeleYaziTipleri()
        {
            foreach (var fontFamily in System.Drawing.FontFamily.Families)
            {
                txtboxYaziTipi.Items.Add(fontFamily.Name);
            }
        }

        private void ListeleYaziTipiBoyutlari()
        {
            for (int i = 8; i <= 72; i += 2)
            {
                txtboxYaziTipiBoyutu.Items.Add(i.ToString());
            }
        }

        private void SetInitialFontSelections()
        {
            // Yazı tipini seç
            txtboxYaziTipi.SelectedItem = SecilenYaziTipi.FontFamily.Name;

            // Yazı tipi stilini seç
            txtboxYaziTipiStili.SelectedItem = SecilenYaziTipi.Style.ToString();

            // Yazı tipi boyutunu seç
            var yaziTipiBoyutuStr = SecilenYaziTipi.Size.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var enYakinBoyut = txtboxYaziTipiBoyutu.Items.Cast<string>()
                .OrderBy(item => Math.Abs(float.Parse(item) - SecilenYaziTipi.Size))
                .FirstOrDefault();

            if (enYakinBoyut != null)
            {
                txtboxYaziTipiBoyutu.SelectedItem = enYakinBoyut;
            }
        }

        private void txtboxYaziTipi_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtboxYaziTipiStili.Items.Clear();
            var selectedFontFamily = new FontFamily(txtboxYaziTipi.SelectedItem.ToString());
            foreach (FontStyle style in Enum.GetValues(typeof(FontStyle)))
            {
                if (selectedFontFamily.IsStyleAvailable(style))
                {
                    txtboxYaziTipiStili.Items.Add(style.ToString());
                }
            }
            GuncelleSecilenYaziTipi();
        }

        private void txtboxYaziTipiStili_SelectedIndexChanged(object sender, EventArgs e)
        {
            GuncelleSecilenYaziTipi();
        }

        private void txtboxYaziTipiBoyutu_SelectedIndexChanged(object sender, EventArgs e)
        {
            GuncelleSecilenYaziTipi();
        }

        private void GuncelleSecilenYaziTipi()
        {
            if (txtboxYaziTipi.SelectedItem != null && txtboxYaziTipiStili.SelectedItem != null && txtboxYaziTipiBoyutu.SelectedItem != null)
            {
                var yaziTipi = txtboxYaziTipi.SelectedItem.ToString();
                var yaziTipiStili = (FontStyle)Enum.Parse(typeof(FontStyle), txtboxYaziTipiStili.SelectedItem.ToString());
                var yaziTipiBoyutu = float.Parse(txtboxYaziTipiBoyutu.SelectedItem.ToString());

                SecilenYaziTipi = new Font(yaziTipi, yaziTipiBoyutu, yaziTipiStili);

                // Seçilen yazı tipini göster
                txtboxsecilenYaziTipi.Text = yaziTipi;
                txtboxsecilenYaziTipiStili.Text = yaziTipiStili.ToString();
                txtboxsecilenYaziTipiBoyutu.Text = yaziTipiBoyutu.ToString();

                // Örnek label'ı güncelle
                lblOrnek.Font = SecilenYaziTipi;
            }
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            // Bu formu kapat
            this.Close();
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            // İptal işlemi
            // Bu formu kapat
            this.Close();
        }
    }
}