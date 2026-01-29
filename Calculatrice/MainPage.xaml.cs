using System;
using Microsoft.Maui.Controls;

namespace Calculatrice
{
    public partial class MainPage : ContentPage
    {
        // ======= Variables principales =======
        private double nombreCourant = 0;         
        private double nombrePrecedent = 0;    
        private string operationCourante = "";      
        private bool nouvelleSaisie = true;     

        // ======= Constructeur =======
        public MainPage()
        {
            InitializeComponent();
            UpdateAffichage("0");
        }

        // ======= Méthode pour mettre à jour l'affichage =======
        private void UpdateAffichage(string texte)
        {
            Affichaage.Text = texte;

            int longueur = texte.Length;

            if (longueur <= 6)
                Affichaage.FontSize = 48;
            else if (longueur <= 9)
                Affichaage.FontSize = 36;
            else if (longueur <= 12)
                Affichaage.FontSize = 28;
            else
                Affichaage.FontSize = 20;
        }

        // ======= Boutons numériques =======
        private void OnNumberButton_Clicked(object sender, EventArgs e)
        {
            Button bouton = (Button)sender;

            if (nouvelleSaisie)
            {
                UpdateAffichage(bouton.Text);
                nouvelleSaisie = false;
            }
            else
            {
                UpdateAffichage(Affichaage.Text + bouton.Text);
            }

            nombreCourant = double.Parse(Affichaage.Text);
        }

        // ======= Bouton décimal =======
        private void OnDecimalButton_Clicked(object sender, EventArgs e)
        {
            if (!Affichaage.Text.Contains("."))
            {
                UpdateAffichage(Affichaage.Text + ".");
                nouvelleSaisie = false;
            }
        }

        // ======= Boutons opérateurs =======
        private void OnPlusButton_Clicked(object sender, EventArgs e) => DefinirOperation("Addition");
        private void OnMinusButton_Clicked(object sender, EventArgs e) => DefinirOperation("Soustraction");
        private void OnMultiplyButton_Clicked(object sender, EventArgs e) => DefinirOperation("Multiplication");
        private void OnDivideButton_Clicked(object sender, EventArgs e) => DefinirOperation("Division");

        // Définit l'opération à effectuer
        private void DefinirOperation(string typeOperation)
        {
            if (!string.IsNullOrEmpty(operationCourante))
            {
                // Si une opération était déjà en cours, calculer le résultat avant
                CalculerResultat();
            }

            nombrePrecedent = nombreCourant;
            operationCourante = typeOperation;
            nouvelleSaisie = true;
        }

        // ======= Bouton égal =======
        private void OnEqualsButton_Clicked(object sender, EventArgs e)
        {
            CalculerResultat();
            operationCourante = ""; // Réinitialise l'opération après calcul
        }

        // Calcul du résultat selon l'opération courante
        private void CalculerResultat()
        {
            double resultat = nombreCourant;

            switch (operationCourante)
            {
                case "Addition":
                    resultat = nombrePrecedent + nombreCourant;
                    break;
                case "Soustraction":
                    resultat = nombrePrecedent - nombreCourant;
                    break;
                case "Multiplication":
                    resultat = nombrePrecedent * nombreCourant;
                    break;
                case "Division":
                    if (nombreCourant != 0)
                        resultat = nombrePrecedent / nombreCourant;
                    else
                    {
                        UpdateAffichage("Erreur");
                        nouvelleSaisie = true;
                        return;
                    }
                    break;
            }

            UpdateAffichage(resultat.ToString());
            nombreCourant = resultat;
            nouvelleSaisie = true;
        }

        // ======= Bouton AC =======
        private void OnClearButton_Clicked(object sender, EventArgs e)
        {
            nombreCourant = 0;
            nombrePrecedent = 0;
            operationCourante = "";
            nouvelleSaisie = true;
            UpdateAffichage("0");
        }

        // ======= Bouton ± =======
        private void OnPlusMinusButton_Clicked(object sender, EventArgs e)
        {
            nombreCourant = -nombreCourant;
            UpdateAffichage(nombreCourant.ToString());
        }

        // ======= Bouton % =======
        private void OnPercentButton_Clicked(object sender, EventArgs e)
        {
            nombreCourant /= 100;
            UpdateAffichage(nombreCourant.ToString());
        }

        // ======= Bouton 1/x =======
        private void OnInverseButton_Clicked(object sender, EventArgs e)
        {
            if (nombreCourant != 0)
            {
                nombreCourant = 1 / nombreCourant;
                UpdateAffichage(nombreCourant.ToString());
            }
            else
            {
                UpdateAffichage("Erreur");
            }
        }
    }
}
