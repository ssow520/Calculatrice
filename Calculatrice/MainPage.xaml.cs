using System;
using Microsoft.Maui.Controls;

namespace Calculatrice
{
    public partial class MainPage : ContentPage
    {
        // Variables pour stocker l'état de la calculatrice
        private double premierNombre = 0;
        private double resultat = 0;
        private string operateurActuel = "";
        private bool nouvelleEntree = true;

        public MainPage()
        {
            InitializeComponent();
        }

        // Gestion des boutons numériques (0-9)
        private void OnNumberButton_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string nombre = button.Text;

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

            AjusterTaillePolice(Affichage.Text);
        }

        // Gestion du point décimal
        private void OnDecimalButton_Clicked(object sender, EventArgs e)
        {
            if (nouvelleEntree)
            {
                Affichage.Text = "0.";
                nouvelleEntree = false;
            }
            else if (!Affichage.Text.Contains("."))
            {
                Affichage.Text += ".";
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
            if (!nouvelleEntree && operateurActuel != "")
            {
                // Si on change d'opérateur sans calculer
                CalculerResultat();
            }

            if (double.TryParse(Affichage.Text, out double valeur))
            {
                premierNombre = valeur;
                operateurActuel = operateur;

                // Afficher l'opération en cours en haut
                OperationLabel.Text = $"{FormatNombre(premierNombre)} {operateur}";

                nouvelleEntree = true;
            }
        }

        // Bouton Égal (=)
        private void OnEqualsButton_Clicked(object sender, EventArgs e)
        {
            CalculerResultat();
            OperationLabel.Text = "";
            operateurActuel = "";
            nouvelleEntree = true;
        }

        // Calcul du résultat
        private void CalculerResultat()
        {
            if (operateurActuel == "" || !double.TryParse(Affichage.Text, out double deuxiemeNombre))
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
                        Affichage.Text = "Erreur";
                        AjusterTaillePolice("Erreur");
                        nouvelleEntree = true;
                        return;
                    }
                    break;
            }

            // Formater le résultat
            string resultatFormate = FormatNombre(resultat);
            Affichage.Text = resultatFormate;
            AjusterTaillePolice(resultatFormate);

            premierNombre = resultat;
        }

        // Bouton AC
        private void OnClearButton_Clicked(object sender, EventArgs e)
        {
            Affichage.Text = "0";
            premierNombre = 0;
            resultat = 0;
            operateurActuel = "";
            nouvelleEntree = true;
            OperationLabel.Text = "";
            AjusterTaillePolice("0");
        }

        // Bouton +/-
        private void OnPlusMinusButton_Clicked(object sender, EventArgs e)
        {
            if (double.TryParse(Affichage.Text, out double valeur))
            {
                valeur = -valeur;
                Affichage.Text = FormatNombre(valeur);
                AjusterTaillePolice(Affichage.Text);
            }
        }

        // Bouton %
        private void OnPercentButton_Clicked(object sender, EventArgs e)
        {
            if (double.TryParse(Affichage.Text, out double valeur))
            {
                valeur = valeur / 100;
                Affichage.Text = FormatNombre(valeur);
                AjusterTaillePolice(Affichage.Text);
            }
        }

        // Bouton 1/x
        private void OnInverseButton_Clicked(object sender, EventArgs e)
        {
            if (double.TryParse(Affichage.Text, out double valeur))
            {
                if (valeur != 0)
                {
                    valeur = 1 / valeur;
                    Affichage.Text = FormatNombre(valeur);
                    AjusterTaillePolice(Affichage.Text);
                }
                else
                {
                    Affichage.Text = "Erreur";
                    AjusterTaillePolice("Erreur");
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
            if (nombre == Math.Floor(nombre))
                return nombre.ToString("0");
            else
            {
                // Sinon, afficher avec les décimales nécessaires
                string resultat = nombre.ToString("G12"); // Max 12 chiffres significatifs

                // Limiter à 12 caractères maximum pour éviter débordement
                if (resultat.Length > 12)
                    resultat = nombre.ToString("0.#########");

                return resultat;
            }
        }
    }
}
