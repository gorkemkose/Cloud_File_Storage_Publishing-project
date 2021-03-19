using System;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;
using System.Threading;

namespace CS408Project_client
{
    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;

        private static string shortFileName = "";
        private static string fileName = "";

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        //----------FOR FILE BROWSING WINDOW---------------------//
        private void button_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Client browsing";
            dlg.ShowDialog();
            FilePath_textBox.Text = dlg.FileName;
            fileName = dlg.FileName;
            shortFileName = dlg.SafeFileName;
        }

        private void button_Connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_IP.Text;
            string Username = textBox_Username.Text;

            int portNum;
            if ((Int32.TryParse(textBox_Port.Text, out portNum)) && (textBox_IP.Text != "") && (Username != ""))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    Byte[] buffer = new Byte[64];
                    buffer = Encoding.Default.GetBytes(Username);
                    clientSocket.Send(buffer);

                    //----TAKE AN ACK ON THE USERNAME, IF THERE IS SUCH A USER IN THE SERVER, DON'T CONNECT-------//
                    Byte[] usernameAckBuffer = new byte[4];
                    clientSocket.Receive(usernameAckBuffer);
                    string usernameAck = Encoding.Default.GetString(usernameAckBuffer);
                    usernameAck = usernameAck.Substring(0, usernameAck.IndexOf("\0"));
                    
                    if(usernameAck != "1")
                    {
                        logs.AppendText("There is a client with username " + Username + "who is already connected. Try another username!\n");
                    }
                    else //SAFE TO CONNECT
                    {
                        button_disconnect.BackColor = Color.IndianRed;
                        button_Connect.BackColor = Color.DimGray;
                        textBox_Port.BackColor = Color.MediumSeaGreen;
                        textBox_IP.BackColor = Color.MediumSeaGreen;
                        textBox_Username.BackColor = Color.MediumSeaGreen;

                        button_Connect.Enabled = false;
                        button_disconnect.Enabled = true;
                        connected = true;
                        button_Browse.Enabled = true;
                        button_Send.Enabled = true;
                        textBox_Port.Enabled = false;
                        textBox_IP.Enabled = false;
                        textBox_Username.Enabled = false;
                        button_disconnect.Enabled = true;
                        button1.Enabled = true;
                        button_rtrvPublic.Enabled = true;
                        fileName_textbox.Enabled = true;
                        button_download.Enabled = true;
                        button_public.Enabled = true;
                        button_copy.Enabled = true;
                        button_delete.Enabled = true;
                        button_folderBrowse.Enabled = true;
                        textBox_owner.Enabled = true;
                        button_publicDown.Enabled = true;

                        logs.AppendText("Connected to the server!\n");
                        
                    }
                    
                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                }
            }
            else //THEN THE INPUTS ARE NOT APPROPRIATE
            {
                textBox_Port.BackColor = Color.IndianRed;
                textBox_IP.BackColor = Color.IndianRed;
                textBox_Username.BackColor = Color.IndianRed; 
                logs.AppendText("Check the username, IP address and port number!\n");
            }
        }

        private void button_Send_Click(object sender, EventArgs e)
        {
            try
            {
                //-----------SEND THE FILENAME-------------------//
                Byte[] buffer_fileName = new Byte[64];
                buffer_fileName = Encoding.Default.GetBytes(shortFileName);
                clientSocket.Send(buffer_fileName);

                string data = System.IO.File.ReadAllText(FilePath_textBox.Text);
                int dataLength = data.Length;

                //---------DATA WILL BE SENT PACKET BY PACKET, EACH PACKET HAS 8192 BYTES-------------//
                //---------CALCULATE THE NUMBER OF PACKETS------------//
                int numOfPackets = dataLength / 8192;
                numOfPackets += 1;

                //----------------SEND THE NUMBER OF PACKETS INFORMATION TO THE SERVER---------------//
                Byte[] num_of_packets = new Byte[8192];
                num_of_packets = Encoding.Default.GetBytes(numOfPackets.ToString());
                clientSocket.Send(num_of_packets);

                //-----------RECEIVE THE ACK OF SERVER RECEIVED THE NUMBER OF PACKETS INFORMATION--------//
                Byte[] usernameAckBuffer = new Byte[8];
                clientSocket.Receive(usernameAckBuffer);
                Array.Clear(usernameAckBuffer, 0, usernameAckBuffer.Length);

                //-------------SEND THE SIZE OF THE WHOLE FILE TO SERVER---------------------------------//
                
                FileInfo fi = new FileInfo(@fileName);
                var size = fi.Length;
                string sizeStr = size.ToString();
                Byte[] fileSizeBuffer = Encoding.Default.GetBytes(sizeStr);
                clientSocket.Send(fileSizeBuffer);

                //-----------RECEIVE THE ACK OF SERVER RECEIVED THE NUMBER OF PACKETS INFORMATION--------//
                Byte[] sizeAckBuffer = new Byte[8];
                clientSocket.Receive(sizeAckBuffer);
                Array.Clear(sizeAckBuffer, 0, sizeAckBuffer.Length);

                int index = 0;
                logs.AppendText("Started to send file...\n");

                //-----SEND THE DATA PACKET BY PACKET IN A LOOP THAT ITERATES NUMBER OF PACKET TIMES-----------//
                for (int i = 1; i <= numOfPackets; i++)
                {
                    Byte[] data_packet = new Byte[8192];
                    string part;

                    if ((data.Length - (i-1)*8192)>= 8192)
                        part = data.Substring(index, 8192);

                    else
                        part = data.Substring(index, (data.Length - (i - 1) * 8192));

                    data_packet = Encoding.Default.GetBytes(part);
                    clientSocket.Send(data_packet);
                    Array.Clear(data_packet, 0, data_packet.Length);
                    
                    index += 8192;               
                }

                //--------WHOLE DATA IS SENT WHEN THE LOOP TERMINATES-------------------//
                logs.AppendText("File has been sent. \n");
            }
            catch
            {
                if (FilePath_textBox.Text == "")
                    logs.AppendText("Please select a file to upload! \n");

                else
                    logs.AppendText("Couldn't send the text file. \n");
            }

        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            connected = false;
            clientSocket.Close();
            button_disconnect.Enabled = false;
            button_Connect.Enabled = true;
            button_Browse.Enabled = false;
            textBox_IP.Enabled = true;
            textBox_Port.Enabled = true;
            textBox_Username.Enabled = true;
            button_Send.Enabled = false;
            button1.Enabled = false;
            button_rtrvPublic.Enabled = false;
            fileName_textbox.Enabled = false;
            textBox_downPath.Enabled = false;
            button_download.Enabled = false;
            button_public.Enabled = false;
            button_copy.Enabled = false;
            button_delete.Enabled = false;
            button_folderBrowse.Enabled = false;
            textBox_owner.Enabled = false;
            button_publicDown.Enabled = false;
            button_disconnect.BackColor = Color.DimGray;
            button_Connect.BackColor = Color.MediumSeaGreen;
            textBox_IP.BackColor = Color.White;
            textBox_Username.BackColor = Color.White;
            textBox_Port.BackColor = Color.White;

            logs.AppendText("Disconnected from server!\n");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //--------------------RETRIEVE FILE LIST BUTTON---------------//
        private void button1_Click(object sender, EventArgs e) 
        {
            try
            {
                //------------------SEND THE COMMAND RTRV----------------------//
                Byte[] requestBuffer = Encoding.Default.GetBytes("Rtrv");
                clientSocket.Send(requestBuffer);

                //-------------------RECEIVE THE LIST OF FILES-----------------//
                Byte[] fileNamesBuffer = new Byte[10000]; //this number can be optimized
                clientSocket.Receive(fileNamesBuffer);
                string fileNamesLongStr = Encoding.Default.GetString(fileNamesBuffer);
                fileNamesLongStr = fileNamesLongStr.Substring(1, fileNamesLongStr.IndexOf("\0"));

                if(fileNamesLongStr.Length > 1) //SUCCESSFUL
                {
                    logs.AppendText("The upload Dates, Names and the Sizes of the files that you have uploaded are as follows:\n" + fileNamesLongStr + "\n");
                }
                else //THE INITIAL VALUE OF WHAT CLIENT RECEIVE FROM THE SERVER IS "1". SO IF CLIENT PROCESSED ALL LINES BUT DIDN'T MAKE AN ADDITION, THEN THERE IS NO FILES UPLOADED
                {
                    logs.AppendText("You haven't uploaded any files yet!\n");
                }
                
            }
            catch
            {
                logs.AppendText("Something went wrong while retrieving the list of files!\n");
            }
        }

        private void button_public_Click(object sender, EventArgs e)
        {
            try
            {
                //------------------SEND THE PBLC REQUEST-------------//
                Byte[] requestBuffer = Encoding.Default.GetBytes("Pblc");
                clientSocket.Send(requestBuffer);

                //----------------SEND THE NAME OF THE FILE TO BE PUBLICISED-----------//
                string fileToPublic = fileName_textbox.Text;
                Byte[] fileNameBuffer = Encoding.Default.GetBytes(fileToPublic);
                clientSocket.Send(fileNameBuffer);

                //----------------RECEIVE THE RESULT OF THE PUBLICATION----------------//
                Byte[] publicAckBuffer = new Byte[100];
                clientSocket.Receive(publicAckBuffer);
                string messageAboutPublic = Encoding.Default.GetString(publicAckBuffer);
                messageAboutPublic = messageAboutPublic.Substring(0, messageAboutPublic.IndexOf("\0"));
                logs.AppendText(messageAboutPublic);

            }catch
            {
                logs.AppendText("Something went wrong while publicising the file!\n");
            }
        }

        private void button_rtrvPublic_Click(object sender, EventArgs e)
        {
            try
            {
                //--------------------SEND THE RTRVPBLC REQUEST--------------//
                Byte[] requestBuffer = Encoding.Default.GetBytes("RtrvPblc");
                clientSocket.Send(requestBuffer);

                //--------------RECEIVE THE LIST OF PUBLIC FILES---------------//
                Byte[] fileNamesBuffer = new Byte[10000]; //this number can be optimized
                clientSocket.Receive(fileNamesBuffer);
                string fileNamesLongStr = Encoding.Default.GetString(fileNamesBuffer);
                fileNamesLongStr = fileNamesLongStr.Substring(1, fileNamesLongStr.IndexOf("\0"));

                if (fileNamesLongStr.Length > 1) //SUCCESSFUL
                {
                    logs.AppendText("The upload times, names and the sizes of the files that are public are as following:\n" + fileNamesLongStr + "\n");
                }
                else //THE INITIAL VALUE OF WHAT CLIENT RECEIVE FROM THE SERVER IS "1". SO IF CLIENT PROCESSED ALL LINES BUT DIDN'T MAKE AN ADDITION, THEN THERE IS NO FILES UPLOADED
                {
                    logs.AppendText("There is no public files!\n");
                }
            }
            catch
            {
                logs.AppendText("Something went wrong while retrieving the list of public files!\n");
            }
        }


        // ----------  DELETE OPERATION
        private void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                //--------------------SEND THE deletion REQUEST--------------//
                Byte[] requestBuffer = Encoding.Default.GetBytes("dlt");
                clientSocket.Send(requestBuffer);
              

                //----------------SEND THE NAME OF THE FILE TO BE DELETED-----------//
                string fileToDelete = fileName_textbox.Text;
                Byte[] fileNameBuffer = Encoding.Default.GetBytes(fileToDelete);
                clientSocket.Send(fileNameBuffer);

                logs.AppendText("Requested to delete the file: " + fileToDelete +"\n");

                //----------------RECEIVE THE RESULT OF THE DELETION----------------//
                Byte[] deleteAckBuffer = new Byte[100];
                clientSocket.Receive(deleteAckBuffer);
                string messageAboutDelete = Encoding.Default.GetString(deleteAckBuffer);
                messageAboutDelete = messageAboutDelete.Substring(0, messageAboutDelete.IndexOf("\0"));
                logs.AppendText(messageAboutDelete);

            }

            catch
            {
                logs.AppendText("Something went wrong while deleting the file!\n");
            }
        }

        private void button_folderBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.RootFolder = Environment.SpecialFolder.Desktop;
            browser.Description = "Select a folder to download.";
            browser.ShowNewFolderButton = true;

            if (browser.ShowDialog() == DialogResult.OK)
                textBox_downPath.Text = browser.SelectedPath;
        }

        private void button_download_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_downPath.Text != "") { 
                //--------------------SEND THE DOWNLOAD REQUEST--------------//
                Byte[] requestBuffer = Encoding.Default.GetBytes("dwn");
                clientSocket.Send(requestBuffer);

                //----------------------SEND THE NAME OF THE FILE TO BE DOWNLOADED------------//
                Byte[] fileNameBuffer = Encoding.Default.GetBytes(fileName_textbox.Text);
                clientSocket.Send(fileNameBuffer);

                //----------------RECEIVE THE EXPECTED RESULT OF THE DOWNLOAD----------------//
                Byte[] publicAckBuffer = new Byte[200];
                clientSocket.Receive(publicAckBuffer);
                string messageAboutPublic = Encoding.Default.GetString(publicAckBuffer);
                messageAboutPublic = messageAboutPublic.Substring(0, messageAboutPublic.IndexOf("\0"));
                logs.AppendText(messageAboutPublic);
                string first_word = messageAboutPublic.Substring(0, 3);

                //if there is no such file the first 3 words of the ack is You, then return.

                if (first_word == "You")
                {
                    return;
                }

                //THE FOLDER AND FILE OPERATIONS
                fileName = fileName_textbox.Text;
                fileName = fileName.Substring(0, fileName.IndexOf(".")); //REMOVING THE .TXT EXTENSION FROM THE END

                string file_path = textBox_downPath.Text;
                string save_fileName = "\\" + fileName + ".txt";
                string write_path = file_path + save_fileName;
                var path = @write_path;

                string finalFileName;

                if (File.Exists(path)) //CHECK IF THE FILE IS DOWNLOADED BEFORE OR SOMEHOW EXIST IN THE SELECTED DOWNLOAD FOLDER, IF SO
                {
                    logs.AppendText("You already have a file as " + fileName + ".txt in the folder.\n");

                    int counter = 1;
                    while (File.Exists(path)) //INCREMENT AND ADD THE NEXT NUMBER TO THE NAME OF THE FILE TO GUARANTEE UNIQUENESS
                    {
                        save_fileName = "";
                        save_fileName = "\\" + fileName + "(" + counter + ").txt";
                        write_path = file_path + save_fileName;
                        path = @write_path;
                        counter++;
                    }

                    string resultFileName = save_fileName.Substring(1, save_fileName.Length - 1);
                    logs.AppendText("File will be downloaded as:" + resultFileName + "\n");

                    finalFileName = resultFileName;

                }
                else //IF THERE IS NO EXISTING FILE WITH THAT NAME, NEVER DOWNLOADED BEFORE OR IT IS NOT EXISTING 
                {
                    logs.AppendText(fileName_textbox.Text + " is saved.\n");

                    finalFileName = fileName;

                }

                // ----------- GET THE NUMBER OF PACKETS -------------------------//
                Byte[] packet_buffer = new Byte[8192];
                clientSocket.Receive(packet_buffer);

                string packets = Encoding.Default.GetString(packet_buffer);
                packets = packets.Substring(0, packets.IndexOf("\0"));

                int packet_amount = Int32.Parse(packets);
                logs.AppendText("The packet amount is: " + packet_amount + "\n");

                //-------------------------- SEND ACKNOWLEDGEMENT ------------------//
                Byte[] buffer = new Byte[8];
                buffer = Encoding.Default.GetBytes("OK");
                clientSocket.Send(buffer);

                //------RECEIVING THE SIZE OF THE FILE IS UNNECESSARY SINCE WE WONT STORE ANYTHING IN DB

                // -------------------------- RECEIVE FILE DATA -----------------//
                for (int i = 1; i <= packet_amount; i++)
                {
                    Byte[] data_buffer = new Byte[8192];
                    clientSocket.Receive(data_buffer);

                    string data_received = Encoding.Default.GetString(data_buffer);
                    if (data_received.Length == 8192)
                    {
                        data_received = data_received.Substring(0, data_received.Length);
                    }
                    else
                    {
                        data_received = data_received.Substring(0, data_received.IndexOf("\0"));
                    }


                    File.AppendAllText(path, data_received);

                    Array.Clear(data_buffer, 0, data_buffer.Length);
                }

                logs.AppendText("File is downloaded and added under folder: " + write_path + "\n");
            }
            else{
                logs.AppendText("You should select a valid folder path to download the file!\n");
            }
            }
            catch
            {
                logs.AppendText("Something went wrong while downloading the file!\n");
            }
        }

        private void button_publicDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox_downPath.Text == "")
                {
                    logs.AppendText("You should select a valid folder path to download the file!\n");
                }
                else if (textBox_owner.Text == "")
                {
                    logs.AppendText("You should enter a valid username to download his/her public file!\n");
                }
                else
                {
                    //--------------------SEND THE DOWNLOAD REQUEST--------------//
                    Byte[] requestBuffer = Encoding.Default.GetBytes("dwnpblc");
                    clientSocket.Send(requestBuffer);

                    //----------------------SEND THE NAME OF THE FILE TO BE DOWNLOADED------------//
                    string publicFileName = textBox_owner.Text + "_" + fileName_textbox.Text;
                    Byte[] fileNameBuffer = Encoding.Default.GetBytes(publicFileName);
                    clientSocket.Send(fileNameBuffer);

                    //----------------RECEIVE THE EXPECTED RESULT OF THE DOWNLOAD----------------//
                    Byte[] publicAckBuffer = new Byte[200];
                    clientSocket.Receive(publicAckBuffer);
                    string messageAboutPublic = Encoding.Default.GetString(publicAckBuffer);
                    messageAboutPublic = messageAboutPublic.Substring(0, messageAboutPublic.IndexOf("\0"));
                    logs.AppendText(messageAboutPublic);
                    string first_word = messageAboutPublic.Substring(0, 3);

                    if (first_word == "The") {
                        return;
                    }
                    //If the acknowledgement's first 3 letters is not The then we will proceed with downloading operation

                    //THE FOLDER AND FILE OPERATIONS
                    fileName = fileName_textbox.Text;
                    fileName = fileName.Substring(0, fileName.IndexOf(".")); //REMOVING THE .TXT EXTENSION FROM THE END

                    string file_path = textBox_downPath.Text;
                    string save_fileName = "\\" + fileName + ".txt";
                    string write_path = file_path + save_fileName;
                    var path = @write_path;

                    string finalFileName;

                    if (File.Exists(path)) //CHECK IF THE FILE IS DOWNLOADED BEFORE OR SOMEHOW EXIST IN THE SELECTED DOWNLOAD FOLDER, IF SO
                    {
                        logs.AppendText("You already have a file as " + fileName + ".txt in the folder.\n");

                        int counter = 1;
                        while (File.Exists(path)) //INCREMENT AND ADD THE NEXT NUMBER TO THE NAME OF THE FILE TO GUARANTEE UNIQUENESS
                        {
                            save_fileName = "";
                            save_fileName = "\\" + fileName + "(" + counter + ").txt";
                            write_path = file_path + save_fileName;
                            path = @write_path;
                            counter++;
                        }

                        string resultFileName = save_fileName.Substring(1, save_fileName.Length - 1);
                        logs.AppendText("File will be downloaded as:" + resultFileName + "\n");

                        finalFileName = resultFileName;

                    }
                    else //IF THERE IS NO EXISTING FILE WITH THAT NAME, NEVER DOWNLOADED BEFORE OR IT IS NOT EXISTING 
                    {
                        logs.AppendText(fileName_textbox.Text + " is saved.\n");

                        finalFileName = fileName;

                    }

                    // ----------- GET THE NUMBER OF PACKETS -------------------------//
                    Byte[] packet_buffer = new Byte[8192];
                    clientSocket.Receive(packet_buffer);

                    string packets = Encoding.Default.GetString(packet_buffer);
                    packets = packets.Substring(0, packets.IndexOf("\0"));

                    int packet_amount = Int32.Parse(packets);
                    logs.AppendText("The packet amount is: " + packet_amount + "\n");

                    //-------------------------- SEND ACKNOWLEDGEMENT ------------------//
                    Byte[] buffer = new Byte[8];
                    buffer = Encoding.Default.GetBytes("OK");
                    clientSocket.Send(buffer);

                    //------RECEIVING THE SIZE OF THE FILE IS UNNECESSARY SINCE WE WONT STORE ANYTHING IN DB

                    // -------------------------- RECEIVE FILE DATA -----------------//
                    for (int i = 1; i <= packet_amount; i++)
                    {
                        Byte[] data_buffer = new Byte[8192];
                        clientSocket.Receive(data_buffer);

                        string data_received = Encoding.Default.GetString(data_buffer);
                        if (data_received.Length == 8192)
                        {
                            data_received = data_received.Substring(0, data_received.Length);
                        }
                        else
                        {
                            data_received = data_received.Substring(0, data_received.IndexOf("\0"));
                        }


                        File.AppendAllText(path, data_received);

                        Array.Clear(data_buffer, 0, data_buffer.Length);
                    }

                    logs.AppendText("File is downloaded and added under folder: " + write_path + "\n");
                }
            }
            catch
            {
                logs.AppendText("Something went wrong while downloading the file!\n");
            }
        }

        private void button_copy_Click_1(object sender, EventArgs e)
        {
            try
            {
                //---------------SEND THE COPY REQUEST--------------//
                Byte[] requestBuffer = Encoding.Default.GetBytes("cpy");
                clientSocket.Send(requestBuffer);

                //----------------SEND THE NAME OF THE FILE TO BE COPIED-----------//
                string fileToCopy = fileName_textbox.Text;
                Byte[] fileNameBuffer = Encoding.Default.GetBytes(fileToCopy);
                clientSocket.Send(fileNameBuffer);

                logs.AppendText("Requested to copy the file: " + fileToCopy + "\n");

                //----------------RECEIVE THE RESULT OF THE COPYING----------------//
                Byte[] copyAckBuffer = new Byte[100];
                clientSocket.Receive(copyAckBuffer);
                string messageAboutCopy = Encoding.Default.GetString(copyAckBuffer);
                messageAboutCopy = messageAboutCopy.Substring(0, messageAboutCopy.IndexOf("\0"));
                logs.AppendText(messageAboutCopy);
            }
            catch
            {
                logs.AppendText("Something went wrong while copying the file!\n");
            }
        }
    }
}
