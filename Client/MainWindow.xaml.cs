using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using Tutorial1_Server;

namespace Client
{
    public partial class MainWindow : Window
    {
        private DataServerInterface foob;

        public MainWindow()
        {
            InitializeComponent();


            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/DataService";
            try
            {
                foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
                foob = foobFactory.CreateChannel();

                TotalNum.Text = foob.GetNumEntries().ToString();
            }
            catch
            {
                MessageBox.Show("Cannot connect to server, please restart the server and then start client again");
                return;
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "", image = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            string prefix = "";

            try
            {
                index = Int32.Parse(Index.Text);
            }
            catch
            {
                MessageBox.Show("Please Enter a valid number!");
                return;
            }



            try
            {
                if (index > foob.GetNumEntries() || index <= 0)

                {
                    MessageBox.Show("Index is out of range !");
                    return;
                }
                else
                {
                    foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out image);
                }


                FirstName.Text = fName;
                LastName.Text = lName;
                Balance.Text = bal.ToString("C");
                AcctNo.Text = acct.ToString();
                Pin.Text = pin.ToString("D4");
                
            }
            catch
            {
                MessageBox.Show("Cannot connect to the server !!!");
                return;
            }
            prefix = "https://i.ibb.co/";

            Uri prefixUri = new Uri(prefix);
            Uri suffixUri = new Uri(image, UriKind.Relative);
            Uri fullUri = new Uri(prefixUri, suffixUri);

            Console.WriteLine(fullUri.ToString());

            ImageCode.Source = new BitmapImage(fullUri);



        }

    }
}
