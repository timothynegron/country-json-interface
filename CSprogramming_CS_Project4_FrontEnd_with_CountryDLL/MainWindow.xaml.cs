//***************************************************************************
//File: MainWindow.xaml.cs
//
//Purpose: To add logic to the xaml graphical user interface. Helps open a
//  JSON file using a file dialog object. Searches for country data by storing 
//  JSON data into a list of country objects and comparing the target while
//  looping through the list.
//
//Written By: Timothy Negron
//
//Compiler: Visual Studio C# 2017
//
//Update Information
//------------------
//
//***************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CSprogramming_CS_DLL_for_Country_Data; // My Imported DLL
using Microsoft.Win32; // For File Dialog
using System.Runtime.Serialization.Json; // For reading JSON files

namespace CSprogramming_CS_Project4_FrontEnd_with_CountryDLL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Member Variables
        // The purpose of this variable is to help easily set the initial directory when the FileDialog opens
        string initialDirectory = @"C:\Users\Tim\source\repos\CSprogramming_CS_Project4_FrontEnd_with_CountryDLL\CSprogramming_CS_Project4_FrontEnd_with_CountryDLL\bin\Debug";
        
        // Create a  list of countries using the generics list data type
        List<Country> ListOfCountries = new List<Country>();
        #endregion

        #region Main Method
        //***************************************************************************
        //Method: Main
        //
        //Purpose: To display a graphical user interface and execute logic that is related
        //  to the purpose of the program.
        //
        //***************************************************************************
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Button Methods
        //***************************************************************************
        //Method: ButtonOpenCountriesJSONFile_Click(object sender, RoutedEvenArgs e)
        //
        //Purpose: To display a file dialog and allow the user to open a JSON file.
        //  If a file is selected, it will then store the data inside the file to the 
        //  listOfCountries.
        //
        //***************************************************************************
        private void ButtonOpenCountriesJSONFile_Click(object sender, RoutedEventArgs e)
        {
            // Create an OpenFileDialog Object
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Initialize the initial directory to the current directory
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;

            // Set the initial director to the current working directory
            openFileDialog.InitialDirectory = initialDirectory;

            // Set the title of the File Dialog
            openFileDialog.Title = "Get Countries Data from JSON file";

            // Filter visible files in the File Dialog to only json files
            openFileDialog.Filter = "Json files(*.json)| *.json";

            // Check if a file was selected
            if (openFileDialog.ShowDialog() == true)
            {
                // Set the text on the text boxes
                TextBoxCountryFilename.Text = openFileDialog.FileName;
                TextBoxTargetCountryName.Text = string.Empty;
                TextBoxCapital.Text = string.Empty;
                TextBoxName.Text = string.Empty;
                TextBoxRegion.Text = string.Empty;
                TextBoxSubregion.Text = string.Empty;
                TextBoxPopulation.Text = string.Empty;
                ListViewCurrencies.Items.Clear();
                ListViewLanguages.Items.Clear();

                // Put the filename in a string
                string filename = openFileDialog.FileName;

                // Create new Filstream Object to help read from file
                FileStream readFILE = new FileStream(filename, FileMode.Open, FileAccess.Read);

                // Read as UTF-8 from file into string
                StreamReader streamReader = new StreamReader(readFILE, Encoding.UTF8);
                string jsonString = streamReader.ReadToEnd();

                // Once the data is in the JSON string then put it in a MemoryStream
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);
                MemoryStream stream = new MemoryStream(byteArray);

                // Create new DCJS Object to help read json data
                DataContractJsonSerializer serListOfCountryData = new DataContractJsonSerializer(typeof(List<Country>));

                // Store data from stream to List
                ListOfCountries = (List<Country>)serListOfCountryData.ReadObject(stream);

                readFILE.Close();
            }
        }

        //***************************************************************************
        //Method: ButtonOpenCountriesJSONFile_Click(object sender, RoutedEvenArgs e)
        //
        //Purpose: To search through the list for a target country and, if found,
        //  display the data on the GUI.
        //
        //***************************************************************************
        private void ButtonFindCountryByName_Click(object sender, RoutedEventArgs e)
        {
            // Clear the list view
            ListViewCurrencies.Items.Clear();
            ListViewLanguages.Items.Clear();

            // Store target in string variable to make it easier to read the code
            string Target = TextBoxTargetCountryName.Text;

            // If target is not found textboxes will be set to empty
            Boolean found = false;

            // Start searching through the list
            foreach (Country c in ListOfCountries)
            {
                // Check if the name of each country object matches with the target
                if (c.Name == Target)
                {
                    // Set found to true to that the text boxes do not get cleared of the data that is going to be stored being stored
                    found = true;

                    // Display the data in the textboxes
                    TextBoxName.Text = c.Name;
                    TextBoxCapital.Text = c.Capital;
                    TextBoxRegion.Text = c.Region;
                    TextBoxSubregion.Text = c.Subregion;
                    TextBoxPopulation.Text = Convert.ToString(c.Population);

                    // Loop through each currency object in the country object
                    foreach (Currency curr in c.Currencies)
                    {
                        // Add the names of the currencies to the list view
                        ListViewCurrencies.Items.Add(curr.Name);
                    }

                    // Loop through each language object in the country object
                    foreach (Language l in c.Languages)
                    {
                        // Add the names of the languages to the list view
                        ListViewLanguages.Items.Add(l.Name);
                    }

                    break;
                }
            }

            // Check if target was found
            if(found == false)
            {
                // Remove data from text boxes and list view if target wasn't found to eliminate any user confusion
                TextBoxCapital.Text = string.Empty;
                TextBoxName.Text = string.Empty;
                TextBoxRegion.Text = string.Empty;
                TextBoxSubregion.Text = string.Empty;
                TextBoxPopulation.Text = string.Empty;
                ListViewCurrencies.Items.Clear();
                ListViewLanguages.Items.Clear();
            }
        }
        #endregion
    }
}
