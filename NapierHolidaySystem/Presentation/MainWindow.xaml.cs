using System;
using System.Collections.Generic;
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
using System.IO;
using Business;
using Data;

namespace Presentation
{
    /*
    * Author name: Andrea Silvestro Ortino
    * Presentation.MainWindow is a WPF used for logging in an existing customer or creating a new one.
    * Date last modified: 23/11/2017 09:49
    * This class makes use of the SingletonStorage class instance (Singleton Design Patter).
    */ 
    public partial class MainWindow : Window
    {
        private SingletonStorage data = SingletonStorage.Instance;

        // This constructor initialize the window and create or load a binary file, if exists, containing data for the system.
        public MainWindow()
        {
            InitializeComponent();

            if(File.Exists(data.Filename))
            {
                data.deserializeDict();
            }
            else
                data.serializeDict();
            
        }

        // This method is called on SIGN IN button click. It checks if the customer reference number inputed is the data and open the Customer page window, passing the correct data.
        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txbCustRefNo.Text.Length == 0)
                {
                    throw new ArgumentException("Please, insert a Customer Reference Number.");
                }

                int custRefNo = Int32.Parse(txbCustRefNo.Text);

                if (!data.CustDict.StoreCustomers.ContainsKey(custRefNo))
                {
                    throw new ArgumentException("Customer Reference Number invalid. Try again");
                }
                else
                    MessageBox.Show("Sign In successfull!");
                    Customer custPage = new Customer(data,custRefNo);
                    custPage.Show();
            }
            catch(Exception excep)
            {
                MessageBox.Show(excep.Message);
            }
        }

        // This method is called on NEW CUSTOMER button click. It opens a new Customer page window.
        private void btnNewCust_Click(object sender, RoutedEventArgs e)
        {
            Customer custPage = new Customer(data);
            custPage.Show();
        }

    }
}
