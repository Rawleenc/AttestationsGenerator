using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private List<TextBox>  Get_All_Textbox()
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
            CreateProfileProcess();
        }

        private void CreateProfileProcess()
        {
            foreach (TextBox tb in Get_All_Textbox())
            {
                if (string.IsNullOrEmpty(tb.Text) || string.IsNullOrWhiteSpace(tb.Text))
                {
                    _ = MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Profile p = MainWindow.GetFromFullName(FullName.Text);
            Boolean newProfile = false;
            if (p == null)
            {
                newProfile = true;
                p = new Profile(FullName.Text, DateBirth.Text, CityBirth.Text, Address.Text, City.Text);
            }
            else
            {
                p.BirthDate = DateBirth.Text;
                p.BirthPlace = CityBirth.Text;
                p.Address = Address.Text;
                p.City = City.Text;
            }
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
            }
            else
            {
                if (newProfile)
                {
                    MainWindow.profiles.Add(p);
                }
                File.WriteAllText(configPath + "\\profiles.json", JsonConvert.SerializeObject(MainWindow.profiles, Formatting.Indented));
            }
            this.Close();
        }

        private void OnKeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CreateProfileProcess();
            }
        }
    }
}
