using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace Calculatrice
{
    public partial class MainPage : ContentPage
    {
        // Variables pour stocker l'état de la calculatrice
        private double premierNombre = 0;
        private double deuxiemeNombre = 0;
        private double resultat = 0;
        private string operateurActuel = "";
        private bool nouvelleEntree = true;
        private bool derniereActionEgal = false;

        public MainPage()
        {
            InitializeComponent();
        }

        // Gestion des boutons numériques (0-9)
        private void OnNumberButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string nombre = button.Text;

            // Si on vient d'appuyer sur =, on recommence un nouveau calcul
            if (derniereActionEgal)
            {
                Affichage.Text = "";
                OperationLabel.Text = "";
                derniereActionEgal = false;
                nouvelleEntree = true;
            }

            if (nouvelleEntree)
            {
                Affichage.Text = nombre;
                nouvelleEntree = false;
            }
            else
            {
                if (Affichage.Text == "0")
                    Affichage.Text = nombre;
                else
                    Affichage.Text += nombre;
            }

            // Mettre à jour l'affichage de l'opération pendant la saisie du 2e nombre
            if (operateurActuel != "")
            {
                OperationLabel.Text = $"{FormatNombre(premierNombre)} {operateurActuel} {Affichage.Text}";
            }

            AjusterTaillePolice(Affichage.Text);
        }

        // Gestion du point décimal
        private void OnDecimalButton_Clicked(object sender, EventArgs e)
        {
            // Si on vient d'appuyer sur =, on recommence
            if (derniereActionEgal)
            {
                Affichage.Text = "0";
                OperationLabel.Text = "";
                derniereActionEgal = false;
                nouvelleEntree = false;
            }

            if (nouvelleEntree)
            {
                Affichage.Text = "0.";
                nouvelleEntree = false;
            }
            else if (!Affichage.Text.Contains("."))
            {
                Affichage.Text += ".";
            }

            // Mettre à jour l'affichage de l'opération
            if (operateurActuel != "")
            {
                OperationLabel.Text = $"{FormatNombre(premierNombre)} {operateurActuel} {Affichage.Text}";
            }

            AjusterTaillePolice(Affichage.Text);
        }

        // Bouton Addition
        private void OnPlusButton_Clicked(object sender, EventArgs e)
        {
            GererOperateur("+");
        }

        // Bouton Soustraction
        private void OnMinusButton_Clicked(object sender, EventArgs e)
        {
            GererOperateur("−");
        }

        // Bouton Multiplication
        private void OnMultiplyButton_Clicked(object sender, EventArgs e)
        {
            GererOperateur("×");
        }

        // Bouton Division
        private void OnDivideButton_Clicked(object sender, EventArgs e)
        {
            GererOperateur("÷");
        }

        // Méthode générique pour gérer les opérateurs
        private void GererOperateur(string operateur)
        {
            derniereActionEgal = false;  // Réinitialiser le flag

            if (!nouvelleEntree && operateurActuel != "")
            {
                // Si on change d'opérateur sans calculer
                CalculerResultat();
            }

            // Enlever les parenthèses et utiliser InvariantCulture
            // Gère à la fois (5) et (-5)
            string texteAParsser = Affichage.Text.Replace("(", "").Replace(")", "").Replace(',', '.');

            if (double.TryParse(texteAParsser, NumberStyles.Any, CultureInfo.InvariantCulture, out double valeur))
            {
                premierNombre = valeur;
                operateurActuel = operateur;

                // Afficher l'opération en cours en haut (sans le 2e nombre encore)
                OperationLabel.Text = $"{FormatNombre(premierNombre)} {operateur}";

                nouvelleEntree = true;
            }
        }

        // Bouton Égal =
        private void OnEqualsButton_Clicked(object sender, EventArgs e)
        {
            if (operateurActuel != "")
            {
                double premierNombreOriginal = premierNombre;  // Sauvegarder le premier nombre original

                // Si on vient de cliquer sur =, on répète la même opération
                if (derniereActionEgal)
                {
                    // Répéter : 7 × 2 = 14, puis 14 × 2 = 28, puis 28 × 2 = 56...
                    premierNombre = resultat;
                    premierNombreOriginal = resultat;  // Pour l'affichage
                }
                else
                {
                    // Première fois qu'on clique sur =
                    // Enlever les parenthèses si présentes
                    string texteAParsser = Affichage.Text.Replace("(", "").Replace(")", "").Replace(',', '.');

                    if (double.TryParse(texteAParsser, NumberStyles.Any, CultureInfo.InvariantCulture, out double valeur))
                    {
                        deuxiemeNombre = valeur;
                    }
                }

                CalculerResultat();

                // Afficher l'opération complète : "5 + 4 = 9" (pas "9 + 4 = 9")
                OperationLabel.Text = $"{FormatNombre(premierNombreOriginal)} {operateurActuel} {FormatNombre(deuxiemeNombre)} = {FormatNombre(resultat)}";

                derniereActionEgal = true;  // Marquer qu'on vient de cliquer sur =
                nouvelleEntree = true;
            }
        }

        // Calcul du résultat
        private void CalculerResultat()
        {
            if (operateurActuel == "")
                return;

            switch (operateurActuel)
            {
                case "+":
                    resultat = premierNombre + deuxiemeNombre;
                    break;
                case "−":
                    resultat = premierNombre - deuxiemeNombre;
                    break;
                case "×":
                    resultat = premierNombre * deuxiemeNombre;
                    break;
                case "÷":
                    if (deuxiemeNombre != 0)
                        resultat = premierNombre / deuxiemeNombre;
                    else
                    {
                        Affichage.Text = "Non défini";
                        OperationLabel.Text = "Division par zéro";
                        AjusterTaillePolice("Erreur");
                        nouvelleEntree = true;
                        operateurActuel = "";
                        return;
                    }
                    break;
            }

            // Afficher le résultat en gros
            Affichage.Text = FormatNombre(resultat);
            AjusterTaillePolice(Affichage.Text);

            premierNombre = resultat;
        }

        // Bouton AC (All Clear)
        private void OnClearButton_Clicked(object sender, EventArgs e)
        {
            Affichage.Text = "0";
            premierNombre = 0;
            deuxiemeNombre = 0;
            resultat = 0;
            operateurActuel = "";
            nouvelleEntree = true;
            derniereActionEgal = false;
            OperationLabel.Text = "";
            AjusterTaillePolice("0");
        }

        // Bouton +/- (changement de signe)
        private void OnPlusMinusButton_Clicked(object sender, EventArgs e)
        {
            // Enlever les parenthèses si présentes pour parser correctement
            string texteAParsser = Affichage.Text.Replace("(", "").Replace(")", "").Replace(',', '.');

            if (double.TryParse(texteAParsser, NumberStyles.Any, CultureInfo.InvariantCulture, out double valeur))
            {
                valeur = -valeur;

                // Formater avec parenthèses si négatif
                string nombreFormate;
                if (valeur < 0)
                    nombreFormate = $"(-{FormatNombre(Math.Abs(valeur))})";
                else
                    nombreFormate = FormatNombre(valeur);

                Affichage.Text = nombreFormate;

                // Mettre à jour l'affichage de l'opération si on est en train de saisir le 2e nombre
                if (operateurActuel != "")
                {
                    // Utiliser la valeur réelle (avec signe) pour l'opération
                    OperationLabel.Text = $"{FormatNombre(premierNombre)} {operateurActuel} {nombreFormate}";
                }

                AjusterTaillePolice(Affichage.Text);
            }
        }

        // Bouton % (pourcentage)
        private void OnPercentButton_Clicked(object sender, EventArgs e)
        {
            // Enlever les parenthèses si présentes
            string texteAParsser = Affichage.Text.Replace("(", "").Replace(")", "").Replace(',', '.');

            if (double.TryParse(texteAParsser, NumberStyles.Any, CultureInfo.InvariantCulture, out double valeur))
            {
                valeur = valeur / 100;
                Affichage.Text = FormatNombre(valeur);

                // Mettre à jour l'affichage de l'opération si on est en train de saisir le 2e nombre
                if (operateurActuel != "")
                {
                    OperationLabel.Text = $"{FormatNombre(premierNombre)} {operateurActuel} {Affichage.Text}";
                }

                AjusterTaillePolice(Affichage.Text);
            }
        }

        // Bouton 1/x (inverse)
        private void OnInverseButton_Clicked(object sender, EventArgs e)
        {
            // Enlever les parenthèses si présentes
            string texteAParsser = Affichage.Text.Replace("(", "").Replace(")", "").Replace(',', '.');

            if (double.TryParse(texteAParsser, NumberStyles.Any, CultureInfo.InvariantCulture, out double valeur))
            {
                if (valeur != 0)
                {
                    valeur = 1 / valeur;
                    Affichage.Text = FormatNombre(valeur);

                    // Mettre à jour l'affichage de l'opération si on est en train de saisir le 2e nombre
                    if (operateurActuel != "")
                    {
                        OperationLabel.Text = $"{FormatNombre(premierNombre)} {operateurActuel} {Affichage.Text}";
                    }

                    AjusterTaillePolice(Affichage.Text);
                }
                else
                {
                    Affichage.Text = "Non défini";
                    AjusterTaillePolice("Non défini");
                    nouvelleEntree = true;
                }
            }
        }

        // Ajuster la taille de la police en fonction de la longueur du texte
        private void AjusterTaillePolice(string texte)
        {
            int longueur = texte.Length;

            if (longueur <= 6)
                Affichage.FontSize = 48;
            else if (longueur <= 9)
                Affichage.FontSize = 36;
            else if (longueur <= 12)
                Affichage.FontSize = 28;
            else
                Affichage.FontSize = 20;
        }

        // Formater le nombre pour l'affichage
        private string FormatNombre(double nombre)
        {
            // Si c'est un entier, afficher sans décimales
            if (nombre == Math.Floor(nombre) && nombre >= -999999999 && nombre <= 999999999)
                return nombre.ToString("0");
            else
            {
                // Sinon, afficher avec les décimales nécessaires
                string resultat = nombre.ToString("G12", CultureInfo.InvariantCulture);

                // Limiter à 12 caractères maximum pour éviter débordement
                if (resultat.Length > 12)
                    resultat = nombre.ToString("0.#########", CultureInfo.InvariantCulture);

                return resultat;
            }
        }
    }
}