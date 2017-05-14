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
    /// Interaction logic for Peek.xaml
    /// </summary>
    public partial class Peek : UserControl
    {
        const int whenHintEnable = 0;
        Socket sck;
        String ip;
        String remoteIP;
        int localPort = 50092;
        int remotePort = 50091;
        byte[] buffer;
        EndPoint epLocal, epRemote;
        Boolean peekTurn;
        Boolean canvasClickEnabled;
        string guess;
        Button b;
        Rectangle rect;
        int iterations;
        int hints;
        string word;
        List<Button> buttonList;
        int level;
        Boolean exposed;
		const int gameType = 1;  //1 for p&c || 2 for np&c  || 3 for p&nc  || 4 for np&nc
        Ellipse el;
        //String DBpath = @"Data Source=proj-1217;Initial Catalog=ExperimentDB;Integrated Security=True";
        String DBpath = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\pazsh\OneDrive\Documents\GitHub\Peekaboom\Peekaboom\Database1.mdf;Integrated Security=True";

        public Peek()
        {
            InitializeComponent();
            //==============Defining communication================//
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            startConnection();
            //====================================================//
        
            initialization();

            level = 0;      
        }

        private void initialization()
        {
            guess = "";
            canvas.Children.Clear();
            b = new Button();
            exposed = false;

            buttonList = new List<Button>();
            buttonList.Add(button20);
            buttonList.Add(button1);
            buttonList.Add(button2);
            buttonList.Add(button3);
            buttonList.Add(button4);
            buttonList.Add(button5);
            buttonList.Add(button6);
            buttonList.Add(button7);
            buttonList.Add(button8);
            buttonList.Add(button9);
            buttonList.Add(button10);
            buttonList.Add(button11);
            buttonList.Add(button12);
            buttonList.Add(button13);
            buttonList.Add(button14);
            buttonList.Add(button15);
            buttonList.Add(button16);
            buttonList.Add(button17);
            buttonList.Add(button18);
            buttonList.Add(button19);

            foreach (Button bt in buttonList)
            {
                bt.Background = Brushes.WhiteSmoke;
                bt.MouseEnter += new MouseEventHandler(sendButton_MouseEnter);
            }

            instructionLabel.Content = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
            guide.Text = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";

            RandomizePic();
            sendButton.IsEnabled = false;
            peekTurn = true;
            canvasClickEnabled = true;
            hints = 3;

            feed.Text = "";
            double start_left = 0, start_top = 0;
            int curr = 0;
            double sqrWidth = 55;
            double sqrHeight = 55;
            for (int i = 0; i/*width*/ < 150; i++)
            {
                for (int j = 0; j/*height*/ < 150; j++)
                {
                    rect = new Rectangle();
                    if (canvasClickEnabled)
                    {
                        rect.MouseEnter += new MouseEventHandler(rect_MouseEnter);
                        rect.MouseLeave += new MouseEventHandler(rect_MouseLeave);
                        rect.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
                    }

                    rect.Fill = Brushes.Black;
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
            sendButton.MouseEnter += new MouseEventHandler(sendButton_MouseEnter);
        }

        private void RandomizePic()
        {
            Random rnd = new Random();
            int num = rnd.Next(1, 5);
            string ran = num.ToString();
            string url = "/Images/image" + ran + ".jpg";
            pic.Source = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));

            string s = loadGuesess(num);

            this.Dispatcher.Invoke(() =>
            {
                string theMessageToSend = "1" + num + s;
                Encoding hebrewEncoding = Encoding.GetEncoding(862);
                byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);

                sck.Send(msg);
            });
        }

        private string loadGuesess(int num)
        {
            try
            {
                SqlConnection thisConnection = new SqlConnection(DBpath);
                thisConnection.Open();
                string s = "select * from guesses";
                SqlCommand cmd = new SqlCommand(s, thisConnection);
                SqlDataReader DR = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(DR);
                num--;
                DataRow row = dt.Rows[num];
                num++;
                word = row[num].ToString();                        //this is the true guess
                int i = 1;
                foreach (Button b in buttonList)
                { 
                    b.Content = row[i].ToString();
                    i++;            
                }
            }
            catch
            {
                MessageBox.Show("guess error");
            }
            return word;
        }

        private void sendButton_MouseEnter(object sender, MouseEventArgs e)
        {
            sendButton.Cursor = Cursors.Hand;
            for (int i = 0; i < buttonList.Count(); i++)
            {
                buttonList[i].Cursor = Cursors.Hand;
            }
        }

        private void startConnection()
        {
            //binding socket
            ip = getLocalIP();
            remoteIP = getLocalIP();
            //remoteIP = "132.72.66.86";
            epLocal = new IPEndPoint(IPAddress.Parse(ip), localPort);
            sck.Bind(epLocal);

            //Connecting to remote ip
            epRemote = new IPEndPoint(IPAddress.Parse(remoteIP), remotePort );
            sck.Connect(epRemote);

            //listening the specific port
            buffer = new byte[600];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
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
                        case "1":
                            instructionLabel.Content = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
                            guide.Text = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
                            canvasClickEnabled = true;
                            peekTurn = true;
                            feed.Text = content;
                            exposed = false;
                            if ( (hints > 0) && (whenHintEnable < iterations) )
                            {
                                b_hint.Visibility = System.Windows.Visibility.Visible;
                                b_hint.IsEnabled = true;
                                l_hints.Visibility = System.Windows.Visibility.Visible;
                                l_hints.Content = " נותרו " + hints + " רמזים.";
                            }
                            break;
                        case "2":
                            getTextHint(content);
                            b_hint.IsEnabled = false;
                            break;
                        case "3":
                            System.Windows.MessageBox.Show("השותף שלך סירב להעביר לך רמז", "אישור בקשת רמז");
                            peekTurn = true;
                            b_hint.IsEnabled = false;
                            instructionLabel.Content = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
                            guide.Text = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";

                            break;
						case "4":
							getPing(content);
                            b_hint.IsEnabled = false;
                            feed.Text = "שים לב למיקוד בתמונה ששלחתי לך.";
                            guide.Text = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
                            //feed.Text = "";
							break;
                    }
                }
                catch
                {

                }
            });

            buffer = new byte[600];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), buffer);
        }

		private void getPing(string content)
		{
			string[] coordinates = content.Split(',');
			double x = Convert.ToDouble(coordinates[0]);
			double y = Convert.ToDouble(coordinates[1]);
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
			Canvas.SetLeft(el, x-15);
			Canvas.SetTop(el, y-15);
			canvas.Children.Add(el);

            hints--;
            l_hints.Content = " נותרו " + hints + " רמזים.";
            peekTurn = true;
            if (hints == 0)
                b_hint.IsEnabled = false;
            instructionLabel.Content = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
            guide.Text = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
        }

        private void getTextHint(string receivedMessage)
        {
            hints--;
            l_hints.Content = " נותרו " + hints + " רמזים.";
            //System.Windows.MessageBox.Show(receivedMessage, "הרמז שלך");
            feed.Text = "הרמז שלך: " + receivedMessage;
            peekTurn = true;
            if (hints == 0)
                b_hint.IsEnabled = false;
            instructionLabel.Content = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";
            guide.Text = "העבר את העכבר על הרקע השחור ובחר את האיזור אותו ברצונך לחשוף";

        }

        private void hint_Click(object sender, RoutedEventArgs e)
        {
            if ((peekTurn) && (hints > 0) && (! exposed))
            {
                this.Dispatcher.Invoke(() =>
                {
                    string theMessageToSend = "3";
                    Encoding hebrewEncoding = Encoding.GetEncoding(862);
                    byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);
                    sck.Send(msg);
                    peekTurn = false;
                    instructionLabel.Content = "אנא המתן לתשובתו של BOOM לגבי הרמז";
                    guide.Text = "אנא המתן לתשובתו של שותפך";
                    b_hint.IsEnabled = false;
                });
            }
        }

        private void rect_MouseLeave(object sender, MouseEventArgs e)
        {
            if (peekTurn && canvasClickEnabled)
            {
                Rectangle rect = (Rectangle)sender;
                rect.Fill = Brushes.Black;
            }
        }

        private void rect_MouseEnter(object sender, MouseEventArgs e)
        {
            if (peekTurn && canvasClickEnabled)
            {
                Rectangle rect = (Rectangle)sender;
                rect.Fill = Brushes.Khaki;
                rect.Cursor = Cursors.Hand;
            }
        }


        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (peekTurn && canvasClickEnabled)
            {
                if (e.OriginalSource is Rectangle)
                {
                    Rectangle ClickedRectangle = (Rectangle)e.OriginalSource;
                    ClickedRectangle.Opacity = 0;
                    ClickedRectangle.MouseEnter -= rect_MouseEnter;
                    ClickedRectangle.MouseLeave -= rect_MouseLeave;
                    ClickedRectangle.MouseLeftButtonDown -= canvas_MouseLeftButtonDown;
                    canvas.Children.Remove(ClickedRectangle);
                }

                Point p = Mouse.GetPosition(canvas);
                //convert string message to byte[]
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                byte[] sendingMessage = new byte[600];
                //sending the encoded message
                sendingMessage = aEncoding.GetBytes("0" + p.ToString());
                sck.Send(sendingMessage);
                canvasClickEnabled = false;
                instructionLabel.Content = @"אנא בחר את המילה שברצונך לשלוח, ולאחר מכן לחץ על ""שלח ניחוש""";
                guide.Text = @"אנא בחר את המילה שברצונך לשלוח ממחסן המילים למטה, ולאחר מכן לחץ על ""שלח ניחוש""";

                exposed = true;
                b_hint.IsEnabled = false;
                for (int i = 0; i < buttonList.Count(); i++)
                {
                    buttonList[i].IsEnabled = true;
                }
                canvas.Children.Remove(el);
            }
        }

        private void sendGuess(object sender, RoutedEventArgs e)
        {

            if (peekTurn)
            {
                this.Dispatcher.Invoke(() =>
                {
                    string theMessageToSend = "2" + guess;
                    Encoding hebrewEncoding = Encoding.GetEncoding(862);
                    byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);
                    sck.Send(msg);
                });
                iterations++;
                peekTurn = false;

                sendButton.IsEnabled = false;
                for (int i = 0; i < buttonList.Count(); i++)
                {
                    buttonList[i].IsEnabled = false;
                }
                buttonList.Remove(b);
                b.Background = Brushes.Black;
                guessTextBlock.Text = "";
                instructionLabel.Content = "אנא המתן לסיום תורו של שותפך";
                guide.Text = "אנא המתן לסיום תורו של שותפך";
                if (guess.Equals(word))
                {
                    level++;
                    System.Windows.MessageBox.Show("ניחוש נכון!! כל הכבוד!!!");
                    this.Dispatcher.Invoke(() =>
                    {
                        string theMessageToSend = "4";
                        Encoding hebrewEncoding = Encoding.GetEncoding(862);
                        byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);
                        sck.Send(msg);
                    });
                    if (level < 3)
                    {
                        initialization();
                    }
                    else {
                            this.Dispatcher.Invoke(() =>
                            {
                                string theMessageToSend = "5";
                                Encoding hebrewEncoding = Encoding.GetEncoding(862);
                                byte[] msg = hebrewEncoding.GetBytes(theMessageToSend);
                                sck.Send(msg);
                            });
                            System.Windows.MessageBox.Show("סיימת חלק זה של הניסוי, מיד תעבור לשאלון סיום");
                        }
                }
            }
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


        //Send guess clicked
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (peekTurn)
            {
                foreach (Button bt in buttonList)
                {
                    bt.Background = Brushes.WhiteSmoke;
                }
                b = e.Source as Button;
                guess = b.Content.ToString();
                guessBlock.Text = guess;
                sendButton.IsEnabled = true;
                b.Background = Brushes.Aqua;
                guessTextBlock.Text = "המילה שברצוני לשלוח היא: " + guess;
            }
        }
    }
}
