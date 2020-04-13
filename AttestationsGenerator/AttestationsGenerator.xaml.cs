using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using Spire.Pdf;
using Spire.Pdf.Widget;
using Spire.Pdf.Fields;
using System.Diagnostics;

namespace AttestationsGenerator
{
    /// <summary>
    /// Logique d'interaction pour AttestationsGenerator.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Profile> profiles = new List<Profile>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles_ComboBox.Items.Count == 0)
            {
                _ = MessageBox.Show("Veuillez charger un fichier profiles.json avec des profiles remplis avant de générer une attestation.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "attestation-deplacement-fr-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".pdf",
                Filter = "Fichier PDF | *.pdf"
            };
            if (saveFileDialog.ShowDialog() ?? false)
            {
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("https://www.interieur.gouv.fr/attestation_de_deplacement_derogatoire", saveFileDialog.FileName);
                    PdfDocument doc = new PdfDocument();
                    doc.LoadFromFile(saveFileDialog.FileName);
                    PdfFormWidget formWidget = doc.Form as PdfFormWidget;
                    foreach (PdfField field in formWidget.FieldsWidget)// for (int i = 0; i < formWidget.FieldsWidget.List.Count; i++)
                    {
                        if (field is PdfTextBoxFieldWidget)
                        {
                            PdfTextBoxFieldWidget textBoxField = field as PdfTextBoxFieldWidget;
                            switch (textBoxField.Name)
                            {
                                case "Nom et prénom":
                                    textBoxField.Text = Profiles_ComboBox.Text;
                                    break;
                                case "Date de naissance":
                                    textBoxField.Text = GetFromFullName(Profiles_ComboBox.Text).BirthDate;
                                    break;
                                case "Lieu de naissance":
                                    textBoxField.Text = GetFromFullName(Profiles_ComboBox.Text).BirthPlace;
                                    break;
                                case "Adresse actuelle":
                                    textBoxField.Text = GetFromFullName(Profiles_ComboBox.Text).Address;
                                    break;
                                case "Ville":
                                    textBoxField.Text = GetFromFullName(Profiles_ComboBox.Text).City;
                                    break;
                                case "Date":
                                    textBoxField.Text = DateTime.Now.ToString("dd/MM/yyyy");
                                    break;
                                case "Heure":
                                    textBoxField.Text = DateTime.Now.ToString("HH");
                                    break;
                                case "Minute":
                                    textBoxField.Text = DateTime.Now.ToString("mm");
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (field is PdfCheckBoxWidgetFieldWidget)
                        {
                            PdfCheckBoxWidgetFieldWidget checkBoxField = field as PdfCheckBoxWidgetFieldWidget;
                            if (checkBoxField.Name == Exit_Reasons_ComboBox.Text)
                            {
                                checkBoxField.Checked = true;
                            }
                        }
                    }
                    doc.SaveToFile(saveFileDialog.FileName);
                    _ = Process.Start("explorer.exe", saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public static Profile GetFromFullName(string fullname) => profiles.Find(profil => profil.Fullname == fullname);

        private void Create_Profile_Click(object sender, RoutedEventArgs e)
        {
            new CreateProfile().ShowDialog();
            _ = LoadProfilesFile();
        }

        private void Generate_Profiles_Click(object sender, RoutedEventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string configPath = Path.Combine(path, "AttestationsGenerator");
            _ = Directory.CreateDirectory(configPath);
            if (!File.Exists(configPath + "\\profiles.json"))
            {
                List<Profile> templateProfiles = new List<Profile>
                {
                    new Profile("Fake 1", "01 Septembre 1901", "Fake Place 1", "Fake Address 1", "Fake City 1"),
                    new Profile("Fake 2", "02 Octobre 1902", "Fake Place 2", "Fake Address 2", "Fake City 2")
                };
                File.WriteAllText(configPath + "\\profiles.json", JsonConvert.SerializeObject(templateProfiles, Formatting.Indented));
                _ = MessageBox.Show("Votre fichier profiles.json à été généré.", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                _ = MessageBox.Show("Vous avez déjà un fichier profiles.json.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Load_Profiles_Click(object sender, RoutedEventArgs e)
        {
            if (LoadProfilesFile())
            {
                _ = MessageBox.Show("Votre fichier profiles.json à été correctement lu.", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = LoadProfilesFile();
        }

        private Boolean LoadProfilesFile()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string configPath = Path.Combine(path, "AttestationsGenerator");
                if (Directory.Exists(configPath))
                {
                    if (File.Exists(Path.Combine(configPath, "profiles.json")))
                    {
                        using (StreamReader r = new StreamReader(Path.Combine(configPath, "profiles.json")))
                        {
                            string json = r.ReadToEnd();
                            profiles = JsonConvert.DeserializeObject<List<Profile>>(json);
                        }
                    }
                    Profiles_ComboBox.Items.Clear();
                    foreach (Profile profile in profiles)
                    {
                        if (Profiles_ComboBox.Items.Contains(profile.Fullname))
                        {
                            Profiles_ComboBox.Items.Clear();
                            _ = MessageBox.Show("Vous ne pouvez pas avoir plusieurs profiles avec le même nom et le même prénom.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;
                        }
                        _ = Profiles_ComboBox.Items.Add(profile.Fullname);
                    }
                    if (Profiles_ComboBox.Items.Count > 0)
                    {
                        Profiles_ComboBox.SelectedIndex = 0;
                    }
                }
                else
                {
                    _ = Directory.CreateDirectory(configPath);
                }
                Exit_Reasons_ComboBox.SelectedIndex = 0;
                return true;
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Il y a eu un problème pendant la lecture du fichier profiles.json.\r\n\r\nVeuillez le vérifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Exit_Reasons_ComboBox.SelectedIndex = 0;
                return false;
            }

        }

        private void Delete_Profile_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles_ComboBox.Items.Count == 0)
            {
                _ = MessageBox.Show("Aucun profile à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (MessageBox.Show(String.Format("Vous vous apprêtez à supprimer le profile {0:s}, souhaitez vous vraiment continuer ?.", Profiles_ComboBox.SelectedItem), "Êtes-vous sûr ?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string configPath = Path.Combine(path, "AttestationsGenerator");
                profiles.Remove(GetFromFullName(Profiles_ComboBox.SelectedItem.ToString()));
                File.WriteAllText(configPath + "\\profiles.json", JsonConvert.SerializeObject(profiles, Formatting.Indented));
                _ = LoadProfilesFile();
                _ = MessageBox.Show("Profile supprimé.", "Réussite", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Modify_Profile_Click(object sender, RoutedEventArgs e)
        {
            if (Profiles_ComboBox.Items.Count == 0)
            {
                _ = MessageBox.Show("Aucun profile à modifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Profile profile = GetFromFullName(Profiles_ComboBox.SelectedItem.ToString());
            CreateProfile createProfile = new CreateProfile();
            createProfile.FullName.Text = profile.Fullname;
            createProfile.FullName.IsEnabled = false;
            createProfile.DateBirth.Text = profile.BirthDate;
            createProfile.CityBirth.Text = profile.BirthPlace;
            createProfile.Address.Text = profile.Address;
            createProfile.City.Text = profile.City;
            createProfile.CreateProfileButton.Content = "Modifier le profile";
            createProfile.ShowDialog();
            _ = LoadProfilesFile();
        }
    }
}
