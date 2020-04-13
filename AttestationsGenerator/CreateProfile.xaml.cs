using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AttestationsGenerator
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class CreateProfile : Window
    {
        
        public CreateProfile()
        {
            InitializeComponent();
        }

        private List<TextBox>  get_all_textbox()
        {
            return new List<TextBox>()
            {
                FullName,
                DateBirth,
                CityBirth,
                Address,
                City
            };
        }

        private void Create_Profile(object sender, RoutedEventArgs e)
        {
            foreach (TextBox tb in get_all_textbox())
            {
                if (string.IsNullOrEmpty(tb.Text) || string.IsNullOrWhiteSpace(tb.Text))
                {
                    _ = MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Profile p = new Profile(FullName.Text, DateBirth.Text, CityBirth.Text, Address.Text, City.Text);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configPath = Path.Combine(path, "AttestationsGenerator");
            if (!File.Exists(configPath + "\\profiles.json"))
            {
                List<Profile> templateProfiles = new List<Profile>
                {
                    p
                };
                File.WriteAllText(configPath + "\\profiles.json", JsonConvert.SerializeObject(templateProfiles, Formatting.Indented));
                _ = MessageBox.Show("Votre fichier profiles.json à été généré.", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                List<Profile> profiles = JsonConvert.DeserializeObject<List<Profile>>(File.ReadAllText(configPath + "\\profiles.json"));
                profiles.Add(p);
                File.WriteAllText(configPath + "\\profiles.json", JsonConvert.SerializeObject(profiles, Formatting.Indented));
            }
            this.Close();
        }
    }
}
