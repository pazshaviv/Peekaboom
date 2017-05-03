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
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Data;

namespace Peekaboom.Pages
{
    /// <summary>
    /// Interaction logic for Boom.xaml
    /// </summary>
    public partial class Boom : UserControl
    {
        Socket sck;
        String ip;
        String remoteIP;
        byte[] buffer;
        EndPoint epLocal, epRemote;
        int level;
        Boolean canvasClickEnabled;
        Rectangle rect;
        const int gameType = 1;  //1 for p&c || 2 for np&c  || 3 for p&nc  || 4 for np&nc
        Boolean clicked = true;
        Ellipse el;
        String DBpath = @"Data Source=proj-1217;Initial Catalog=ExperimentDB;Integrated Security=True";


        public Boom()
        {
            InitializeComponent();
            level = 0;
            //==============Defining communication================//
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            startConnection();
            //====================================================//
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //HELLO TEST!!!!!
            clicked = true;
            if (! clicked)
            {
                clicked = true;
                System.Windows.Point p = Mouse.GetPosition(canvas);
                el = new Ellipse();
                switch (gameType)       //only the clarity has influence
                {
                    case 1:
                        el.Width = 30;
                        el.Height = 30;
                        break;
                    case 2:
                        el.Width = 30;
                        el.Height = 30;
                        break;
                    case 3:
                        el.Width = 80;
                        el.Height = 80;
                        break;
                    case 4:
                        el.Width = 80;
                        el.Height = 80; 
                        break;
                }
                el.Stroke = Brushes.Red;
                Canvas.SetLeft(el, p.X);
                Canvas.SetBottom(el, p.X);
                Canvas.SetTop(el, p.Y);
                canvas.Children.Add(el);

                this.Dispatcher.Invoke(() =>
                {
                    //convert string message to byte[]
                    ASCIIEncoding aEncoding = new ASCIIEncoding();
                    byte[] sendingMessage = new byte[600];
                    //sending the encoded message
                    sendingMessage = aEncoding.GetBytes("4" + p.ToString());
                    sck.Send(sendingMessage);
                });
            }
		}
        private void initialization(string message)
        {
            level++;
            wordLabel.Content = message.Substring(1);
            feedBox.IsEnabled = false;
            canvas.Children.Clear();
            canvasClickEnabled = true;

            lguess.Text = "";
            hintBox.Items.Clear();
            feedBox.Items.Clear();
            hintBox.Items.Add("אנא בחר רמז");
            feedBox.Items.Add("אנא העבר לשותפך משוב על הניחוש");
            instructionLabel.Content = "אנא המתן ששותפך יסיים את תורו";
            double start_left = 0, start_top = 0;
            int curr = 0;
            double sqrWidth = 30;
            double sqrHeight = 30;
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 150; j++)
                {
                    rect = new Rectangle();
                    if (canvasClickEnabled)
                        rect.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
                    
                    rect.Fill = System.Windows.Media.Brushes.Black;
                    rect.Width = sqrWidth;
                    rect.Height = sqrHeight;

                    Canvas.SetLeft(rect, start_left);
                    Canvas.SetTop(rect, start_top);

                    canvas.Children.Add(rect);

                    start_left += sqrWidth;
                    curr++;
                }
                start_top += sqrHeight;
                start_left = 0;
            }
            string url = "/Images/image" + message.Substring(0, 1) + ".jpg";

            image_left.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            image_right.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));


            hintBox.SelectedIndex = 0;
            loadHints(message.Substring(0, 1));
            feedBox.SelectedIndex = 0;
            loadFeed(message.Substring(0, 1));
        }

     
        private void loadFeed(string message)
        {
            try
            {
                int num = Int32.Parse(message);
                SqlConnection thisConnection = new SqlConnection(DBpath);
                thisConnection.Open();
                string s = "select * from feed";
                SqlCommand cmd = new SqlCommand(s, thisConnection);
                SqlDataReader DR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DR);
                num--;
                DataRow row = dt.Rows[num];
                int count = dt.Columns.Count;
                for (int i = 1; i < count; i++)
                    feedBox.Items.Add(row[i].ToString());
            }
            catch
            {
                MessageBox.Show("feed error");
            }
        }

        private void loadHints(string message)
        {
            try
            {
                int num = Int32.Parse(message);
                SqlConnection thisConnection = new SqlConnection(DBpath);
                thisConnection.Open();
                string s = "select * from hints";
                SqlCommand cmd = new SqlCommand(s, thisConnection);
                SqlDataReader DR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DR);
                num--;
                DataRow row = dt.Rows[num];
                int count = dt.Columns.Count;

                for (int i = 1; i < count; i++)
                    hintBox.Items.Add(row[i].ToString());
            }
            catch
            {
                MessageBox.Show("hints error");
            }
        }

        private void MessageCallBack(IAsyncResult ar)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    byte[] receivedData = new byte[600];
                    receivedData = (byte[])ar.AsyncState;

                    Encoding hebrewEncoding = Encoding.GetEncoding(862);

                    String receivedMessage = hebrewEncoding.GetString(receivedData);
                    string cleanedMessage = receivedMessage.Replace("\0", string.Empty);

                    string type = receivedMessage.Substring(0, 1);
                    string content = cleanedMessage.Substring(1);

                    switch (type)
                    {
                        case "0":
                            exposePic(content);
                            break;
                        case "1":
                            initialization(content);
                            break;
                        case "2":
                            lguess.Text = content;
                            feedBox.IsEnabled = true;
                            instructionLabel.Content = "אנא העבר ל-PEEK משוב על הניחוש שלו";
                            break;
                        case "3":
                            hintConfirmation(type);
                            break;
                        case "4":
                            System.Windows.MessageBox.Show("PEEK גילה את המילה! כל הכבוד!!");
                            break;
                        case "5":
                            System.Windows.MessageBox.Show("סיימת חלק זה של הניסוי, מיד תעבור לשאלון סיום");
                            break;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                buffer = new byte[600];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
            });
        }

        private void hintConfirmation(string receivedMessage)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("השותף שלך מעוניין לקבל רמז. תסכים להעביר לו?", "אישור בקשת רמז", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (level > 3)
                {
                    hintBox.Visibility = System.Windows.Visibility.Visible;
                    b_sendHint.Visibility = System.Windows.Visibility.Visible;
                    instructionLabel.Content = "אנא בחר רמז להעביר ל-Peek";
                    b_feed.IsEnabled = false;
                    hintBox.IsEnabled = true;
                }
                else
                {
                    clicked = false;
                    instructionLabel.Content = "אנא לחץ על איזור חשוף בתמונה שברצונך למקד בו את שותפך";
                    b_feed.IsEnabled = false;
                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    string theMessageToSend = "3";
                    Encoding hebrewEncoding = Encoding.GetEncoding(862);
                    byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);
                    sck.Send(msg);
                });
            }
        }


    private void exposePic(string message)
    {
        canvas.Children.Remove(el);
        this.Dispatcher.Invoke(() =>
        {
            //get coordinates from receivedMessage
            string[] coordinates = message.Split(',');
            double x = Convert.ToDouble(coordinates[0]);
            double y = Convert.ToDouble(coordinates[1]);
            System.Windows.Point myXY = new System.Windows.Point(x, y);

            //reveal the slice sent from PEEK
            foreach (FrameworkElement nextElement in canvas.Children)
            {
                double left = Canvas.GetLeft(nextElement);
                double top = Canvas.GetTop(nextElement);
                double right = Canvas.GetRight(nextElement);
                double bottom = Canvas.GetBottom(nextElement);
                if (double.IsNaN(left))
                {
                    if (double.IsNaN(right) == false)
                        left = right - nextElement.Width;
                    else
                        continue;
                }
                if (double.IsNaN(top))
                {
                    if (double.IsNaN(bottom) == false)
                        top = bottom - nextElement.Height;
                    else
                        continue;
                }
                Rect eleRect = new Rect(left, top, nextElement.Width, nextElement.Height);
                if (myXY.X >= eleRect.X && myXY.Y >= eleRect.Y && myXY.X <= eleRect.Right && myXY.Y <= eleRect.Bottom)
                {
                    nextElement.Opacity = 0;
                    break;
                }
            }
        });
    }

        private void startConnection()
        {
            //binding socket
            ip = getLocalIP();
            remoteIP = getLocalIP();
            epLocal = new IPEndPoint(IPAddress.Parse(ip), 50091);
            sck.Bind(epLocal);

            //Connecting to remote ip
            epRemote = new IPEndPoint(IPAddress.Parse(remoteIP), 50092);
            sck.Connect(epRemote);

            //listening the specific port
            buffer = new byte[600];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
        }

        private string getLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "127.0.0.1";
        }

        private void sendFeedback(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                string theMessageToSend = "1"+feedBox.SelectedValue;
                Encoding hebrewEncoding = Encoding.GetEncoding(862);
                byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);
                sck.Send(msg);
                feedBox.SelectedIndex = 0;
                b_feed.IsEnabled = false;
                feedBox.IsEnabled = false;
                instructionLabel.Content = "אנא המתן ששותפך יסיים את תורו";
            });
       }

        private void hintBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (hintBox.SelectedIndex != 0)
                b_sendHint.IsEnabled = true;
            if (hintBox.SelectedIndex == 0)
                b_sendHint.IsEnabled = false;
        }

        private void feedBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (feedBox.SelectedIndex != 0)
                b_feed.IsEnabled = true;
            if (feedBox.SelectedIndex == 0)
                b_feed.IsEnabled = false;
        }

        private void b_sendHint_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                string theMessageToSend = "2"+hintBox.SelectedValue;
                Encoding hebrewEncoding = Encoding.GetEncoding(862);
                byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);
                sck.Send(msg);
                hintBox.IsEnabled = false;
                hintBox.SelectedIndex = 0;
                hintBox.Visibility = System.Windows.Visibility.Hidden;
                b_sendHint.Visibility = System.Windows.Visibility.Hidden;
            });
        }

    }
}
