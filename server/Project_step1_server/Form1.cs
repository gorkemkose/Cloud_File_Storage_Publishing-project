using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_step1_server
{
    public partial class ServerForm : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<Socket> clientSockets = new List<Socket>(); //lIST FOR THE CLIENT SOCKETS
        List<string> clientNames = new List<string>(); //LIST FOR THE CLIENT NAMES TO GUARANTEE UNIQUENESS
        

        bool terminating = false;
        bool listening = false;
        string file_path;
        int maxNumClients = 16; //CAN BE CHANGED DEPENDING ON THE CURRENT EXPECTATIONS ON THE APPLICATION

        string pathForDB = "";
        string prevDB = ""; //IN ORDER TO MAINTAIN THE INFORMATION IN THE DATABASE

        DateTime now;

        public ServerForm()
        {
            Control.CheckForIllegalCrossThreadCalls = false;  //PREVENT THREAD ERRORS
            this.FormClosing += new FormClosingEventHandler(ServerForm_FormClosing); //FORM CLOSING METHOD IS ALSO IMPLEMENTED
            InitializeComponent();
        }
        
        //----------OPEN A FOLDER BROWSER TO SELECT A SAVING FOLDER-----------//
        private void browse_button_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.RootFolder = Environment.SpecialFolder.Desktop;
            browser.Description = "Select a folder to save the received files.";
            browser.ShowNewFolderButton = true;

            if (browser.ShowDialog() == DialogResult.OK)
                path_textBox.Text = browser.SelectedPath;
        }

        private void listen_button_Click(object sender, EventArgs e)
        {
            int serverPort;
            file_path = path_textBox.Text;
         
            //---------------CHECK IF PORT NUMBER AND FILEPATH IS PROVIDED CORRECTLY----------------//
            if (Int32.TryParse(port_textBox.Text, out serverPort) && (file_path != ""))
            {
                listening = true;

                //----------CREATE AN ENDPOINT OF GIVEN PORT AND IP TO BIND----------------------//
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                

                serverSocket.Bind(endPoint); //BIND
                serverSocket.Listen(maxNumClients); //PLACES SOCKETS IN THE LISTENING STATE, maxNumClients IS DEFINED GLOBALLY------//

                listen_button.Enabled = false;
                port_textBox.Enabled = false;
                path_textBox.Enabled = false;
                browse_button.Enabled = false;
                logs_richTextBox.Clear();
                
                logs_richTextBox.AppendText(DateTime.Now + " | Started listening port " + serverPort + ", files will be saved under folder " + file_path + ".\n");

                //--------DATABASE CREATION IF THIS IS THE FIRST TIME THAT THE SERVER IS EXECUTED, OTHERWISE THERE IS AN EXISTING DB--------//
                pathForDB = file_path + "\\DATABASE.txt";
                if (!File.Exists(@pathForDB))
                {
                    File.WriteAllText(@pathForDB, "Database is created\n\n");
                }
                

                //--------START A THREAD FOR ACCEPTING NEW CLIENTS--------//
                Thread acceptThread = new Thread(AcceptPort);
                acceptThread.Start();       
            }

            else 
            {
                logs_richTextBox.AppendText("Please check the port number and folder path!\n");    
            }
        }


        private void AcceptPort()
        {
            while (listening)
            {           
                try
                {
                    Socket newClient = serverSocket.Accept(); //CREATE A SOCKET FOR NEWLY CREATED CONNECTION
                                
                    Byte[] name_buffer = new Byte[1024];
                    newClient.Receive(name_buffer); //RECEIVES THE NAME OF THE CLIENT 

                    string receivedName = Encoding.Default.GetString(name_buffer);           
                    receivedName = receivedName.Substring(0, receivedName.IndexOf("\0"));

                    //-----MESSAGE FOR THE VALIDITY OF THE USERNAME------------//
                    Byte[] usernameAckBuffer = new Byte[4];
                    string usernameAckMessage = "";

                    if (clientNames.Contains(receivedName))
                    {
                        logs_richTextBox.AppendText(DateTime.Now +" | Client <" + receivedName + "> is already connected!\n");
                        logs_richTextBox.AppendText(DateTime.Now + " | Number of connected clients: " + clientNames.Count + "\n");

                        //------INFORM THE CLIENT IF THE USERNAME IS ALREADY IN USE BY ANOTHER CLIENT----//
                        usernameAckMessage = "0";
                        usernameAckBuffer = Encoding.Default.GetBytes(usernameAckMessage);
                        newClient.Send(usernameAckBuffer);

                        newClient.Close();
                    }

                    else
                    {
                        clientSockets.Add(newClient);  //ADD THAT CLIENT TO OUR CLIENT LIST
                        clientNames.Add(receivedName);

                        logs_richTextBox.AppendText(DateTime.Now + " | Client <" + receivedName + "> is connected now.\n");
                        logs_richTextBox.AppendText(DateTime.Now + " | Number of connected clients: " + clientNames.Count + "\n");

                        //-----INFORM THE CLIENT THAT THE USERNAME IS VALID, NO PROBLEMS WITH CONNECTION-------//
                        usernameAckMessage = "1";
                        usernameAckBuffer = Encoding.Default.GetBytes(usernameAckMessage);
                        newClient.Send(usernameAckBuffer);
                        
                        //-------START RECEIVING THE FILES FROM CLIENTS---------//
                        Thread receivedThread = new Thread(() => Receive(newClient,receivedName));
                        receivedThread.Start();

                        //-------THREAD FOR SENDING THE FILE LIST------------//
                        //Thread RetrieveFileListThread = new Thread(() => RetrieveFileList(newClient, receivedName, idxOfUser));
                        //RetrieveFileListThread.Start();
                    }             
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs_richTextBox.AppendText("\n" + DateTime.Now + " | Socket stopped working.\n");
                        path_textBox.Enabled = true;
                        port_textBox.Enabled = true;
                        browse_button.Enabled = true;
                        listen_button.Enabled = true;
                        listening = false;
                    }
                }
            }
        }

        private void Receive(Socket thisClient , string clientName)
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    //----------------------------- RECEIVE FILE NAME OR COMMANDS ------------------------//
                    Byte[] file_name_buffer = new Byte[64];
                    thisClient.Receive(file_name_buffer);

                    string fileName = Encoding.Default.GetString(file_name_buffer);
                    fileName = fileName.Substring(0, fileName.IndexOf("\0"));

                    if (fileName == "Rtrv") //THIS IS THE COMMAND FOR RETRIEVING THE LIST OF FILES THAT ARE UPLOADED BY THE REQUESTOR CLIENT
                    {
                        RetrieveFileList(thisClient, clientName, false);
                    }
                    else if(fileName == "Pblc") //THIS IS THE COMMAND FOR PUBLICISING THE FILE 
                    {
                        MakePublic(thisClient, clientName);
                    }
                    else if(fileName == "RtrvPblc") //THIS COMMAND IS FOR RETRIEVING THE LIST OF PUBLIC FILES
                    {
                        RetrieveFileList(thisClient, clientName, true);
                    }
                    else if(fileName == "dlt")  //THIS COMMAND IS FOR DELETING USERS FILES
                    {
                        deleteFile(thisClient,clientName);
                    }
                    else if(fileName == "dwn")
                    {
                        Download(thisClient, clientName);
                    }
                    else if(fileName == "dwnpblc")
                    {
                        DownloadPublic(thisClient, clientName);
                    }
                    else if (fileName == "cpy") // THIS COMMAND IS FOR COPYING FILES
                    {
                        copyFile(thisClient, clientName);
                    }
                    else { //THEN THE RECEIVED PIECE OF DATA FROM THE CLIENT IS NOT A COMMAND BUT THE FILENAME.
                        int idxOfDot = fileName.IndexOf(".");
                        fileName = fileName.Substring(0, idxOfDot);
                        fileName = clientName + "_" + fileName;

                        string save_fileName = "\\" + fileName + ".txt";
                        string write_path = file_path + save_fileName;
                        var path = @write_path;

                        prevDB = File.ReadAllText(@pathForDB);

                        string finalFileName;
                        bool txtExist;
                        if (File.Exists(path)) //CHECK IF THERE IS AN EXISTING FILE WITH USER PROVIDED FILENAME CURRENTLY, IF SO
                        {
                            logs_richTextBox.AppendText(DateTime.Now + " | Client <" + clientName + "> has already uploaded " + fileName + ".txt\n");

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
                            logs_richTextBox.AppendText(DateTime.Now + " | File will be saved as:" + resultFileName + "\n");

                            //-------NEW FILE IS ADDED, DATABASE SHOULD BE UPDATED--------//
                            finalFileName = resultFileName;
                            txtExist = true;
                            
                        }
                        else //IF THERE IS NO EXISTING FILE WITH THAT NAME
                        {
                            logs_richTextBox.AppendText(DateTime.Now + " | Client <" + clientName + "> wants to upload a file, it will be saved as: " + fileName + ".txt\n");

                            txtExist = false;
                            finalFileName = fileName;
                            
                        }
                  

                        // ----------- GET THE NUMBER OF PACKETS -------------------------//
                        Byte[] packet_buffer = new Byte[8192];
                        thisClient.Receive(packet_buffer);

                        string packets = Encoding.Default.GetString(packet_buffer);
                        packets = packets.Substring(0, packets.IndexOf("\0"));

                        int packet_amount = Int32.Parse(packets);

                        //-------------------------- SEND ACKNOWLEDGEMENT ------------------//
                        Byte[] buffer = new Byte[8];
                        buffer = Encoding.Default.GetBytes("OK");
                        thisClient.Send(buffer);
                      

                        //-------------------------RECEIVE THE FILE SIZE---------------------//
                        Byte[] sizeBuffer = new Byte[100];
                        thisClient.Receive(sizeBuffer);
                        string sizeStr = Encoding.Default.GetString(sizeBuffer);
                        sizeStr = sizeStr.Substring(0, sizeStr.IndexOf("\0"));
                        logs_richTextBox.AppendText(DateTime.Now + " | Started to receive a file of size " + sizeStr + " | Packets = " + packet_amount + "\n");

                        //-------------------------- SEND ACKNOWLEDGEMENT ------------------//
                        Byte[] lastAck = new Byte[8];
                        lastAck = Encoding.Default.GetBytes("OK");
                        thisClient.Send(lastAck);

                        //-------------------UPDATE DATABASE--------------------------------//
                        if (txtExist)
                        {
                            prevDB += DateTime.Now + " | username: " + clientName + " | filename: " + finalFileName + " | size: " + sizeStr + " | *private\n";
                            File.WriteAllText(@pathForDB, prevDB);
                        }
                        else
                        {
                            prevDB += DateTime.Now + " | username: " + clientName + " | filename: " + finalFileName + ".txt | size: " + sizeStr + " | *private\n";
                            File.WriteAllText(@pathForDB, prevDB);
                        }
                        

                        // -------------------------- RECEIVE FILE DATA -----------------//
                        for (int i = 1; i <= packet_amount; i++)
                        {
                            Byte[] data_buffer = new Byte[8192];
                            thisClient.Receive(data_buffer);

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
                        logs_richTextBox.AppendText(DateTime.Now + " | File is received and added under folder: " + write_path + "\n");
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | Client <" + clientName + "> has disconnected.\n");
                        logs_richTextBox.AppendText(DateTime.Now + " | Number of connected clients: " + (clientNames.Count-1) + "\n");
                    }
                    thisClient.Close(); //CLOSE SOCKET CONNECTION AND RELEASE RESOURCES
                    clientSockets.Remove(thisClient); //REMOVE THAT CLIENT FROM OUR SOCKET AND NAME LIST
                    clientNames.Remove(clientName);
                    connected = false;
                }
            }
        }

        public void DownloadPublic(Socket thisClient, string clientName)
        {
            if (!terminating)
            {
                try
                {
                    //----------------NAME OF THE FILE TO BE DOWNLOADED IS RECEIVED------------------------//
                    Byte[] fileNameBuffer = new Byte[100];
                    thisClient.Receive(fileNameBuffer);
                    string fileToDownload = Encoding.Default.GetString(fileNameBuffer);
                    fileToDownload = fileToDownload.Substring(0, fileToDownload.IndexOf("\0")); //IN THE FORM USERNAME_FILENAME.TXT

                    logs_richTextBox.AppendText(DateTime.Now + " | User <" + clientName + "> wants to download " + fileToDownload + "\n");

                    //----------------NECESSARY INITIALIZATIONS TO READ THE DATABASE LINE BY LINE---------------//
                    var lines = File.ReadLines(@pathForDB);
                    int lineNum = 0;
                    bool fileExist = false; //THERE IS A FILE EXIST IN THE DATABASE, EITHER PRIVATE OR PUBLIC
                    bool isPublic = false; //FILE IS PUBLIC IF isPublic == true
                    string longFileName = "";

                    foreach (var line in lines)
                    {
                        lineNum++;
                        if (lineNum > 2) //WE START PROCESSING LINES AFTER THE 2ND BECAUSE FIRST TWO LINES OF THE DB ARE DUMMY LINES
                        {
                            //------------FILENAME OF THE CURRENT ENTRY IS EXTRACTED----------//
                            string fileNameExtracted = line.Substring(line.IndexOf("filename") + 10);
                            fileNameExtracted = fileNameExtracted.Substring(0, fileNameExtracted.IndexOf(".") + 4);
                            longFileName =  fileNameExtracted;

                            if (fileToDownload == longFileName) //WE FIND THE FILE THAT IS REQUESTED TO BE DOWNLOADED
                            {
                                fileExist = true;
                                string publicInfo = line.Substring(line.IndexOf("*") + 1); //The value of publicInfo will be "public" if the file is public, otherwise "private"

                                if (publicInfo == "public")
                                {
                                    isPublic = true;
                                }
                                break;
                            }
                        }
                    }

                    Byte[] publicAckBuffer; //THIS BUFFER IS CREATED TO SEND A MESSAGE TO CLIENT SIDE ABOUT THE RESULT OF THE DOWNLOAD
                    if (fileExist == false) //THERE IS NO FILE UPLOADED BY THAT SPECIFIED CLIENT, NO ENTRY IN THE DATABASE AT ALL
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is no " + fileToDownload + " in the database!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("There is no " + fileToDownload + " uploaded in the server!\n");
                        thisClient.Send(publicAckBuffer);
                        return;
                    }
                    else if (isPublic == false) //IF THERE IS A FILE WITH THAT NAME SPECIFIED BUT NOT A PUBLIC FILE
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is a file " + fileToDownload + " in the database, but not a public file!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("The file is not public, you can't download!\n");
                        thisClient.Send(publicAckBuffer);
                        return;
                    }
                    else //THERE IS SUCH A PUBLIC FILE, DOWNLOAD IS AVAILABLE
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | The file " + fileToDownload + " is available for client <" + clientName + "> to download!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("Download is allowed.\n");
                        thisClient.Send(publicAckBuffer);
                    }

                    //IF WE DID NOT RETURN SO FAR, THEN DOWNLOAD OPERATION WILL BE PERFORMED
                    string storagePath = path_textBox.Text;
                    string pathAndFName = storagePath + "\\" + fileToDownload;
                    string data = System.IO.File.ReadAllText(pathAndFName);
                    int dataLength = data.Length;


                    //---------DATA WILL BE SENT PACKET BY PACKET, EACH PACKET HAS 8192 BYTES-------------//
                    //---------CALCULATE THE NUMBER OF PACKETS------------//

                    int numOfPackets = dataLength / 8192; //DATA LENGTH WILL ALWAYS BE A MULTIPLE OF 8192, MIN 8192. THIS IS HOW WE RECEIVE THE UPLOADED ONES BEFORE


                    //----------------SEND THE NUMBER OF PACKETS INFORMATION TO THE CLIENT---------------//
                    Byte[] num_of_packets = new Byte[8192];
                    num_of_packets = Encoding.Default.GetBytes(numOfPackets.ToString());
                    thisClient.Send(num_of_packets);

                    //-----------RECEIVE THE ACK OF CLIENT RECEIVED THE NUMBER OF PACKETS INFORMATION--------//
                    Byte[] usernameAckBuffer = new Byte[8];
                    thisClient.Receive(usernameAckBuffer);
                    Array.Clear(usernameAckBuffer, 0, usernameAckBuffer.Length);

                    //--------SENDING THE SIZE OF THE FILE IS UNNECESSARY BECAUSE CLIENT WONT STORE THAT IN ITS DB, DIRECTLY START SENDING THE PACKETS

                    int index = 0;
                    logs_richTextBox.AppendText(DateTime.Now + " | File is sending to client side...\n");

                    //-----SEND THE DATA PACKET BY PACKET IN A LOOP THAT ITERATES NUMBER OF PACKET TIMES-----------//
                    for (int i = 1; i <= numOfPackets; i++)
                    {
                        Byte[] data_packet = new Byte[8192];
                        string part;

                        if ((data.Length - (i - 1) * 8192) >= 8192)
                            part = data.Substring(index, 8192);

                        else
                            part = data.Substring(index, (data.Length - (i - 1) * 8192));

                        data_packet = Encoding.Default.GetBytes(part);
                        thisClient.Send(data_packet);
                        Array.Clear(data_packet, 0, data_packet.Length);

                        index += 8192;
                    }

                    //--------WHOLE DATA IS SENT WHEN THE LOOP TERMINATES-------------------//
                    logs_richTextBox.AppendText(DateTime.Now + " | File has been sent. \n");

                }
                catch
                {
                    logs_richTextBox.AppendText(DateTime.Now + " | Something went wrong while handling the public download request!\n");
                }
            }
        }

        public void Download(Socket thisClient, string clientName)
        {
            if (!terminating)
            {
                try
                {
                    //----------------NAME OF THE FILE TO BE DOWNLOADED IS RECEIVED------------------------//
                    Byte[] fileNameBuffer = new Byte[100];
                    thisClient.Receive(fileNameBuffer);
                    string fileToDownload = Encoding.Default.GetString(fileNameBuffer);
                    fileToDownload = fileToDownload.Substring(0, fileToDownload.IndexOf("\0"));

                    logs_richTextBox.AppendText(DateTime.Now + " | User <" + clientName + "> wants to download his/her" + fileToDownload + "\n");

                    string fNameDB = clientName + "_" + fileToDownload; //THIS IS HOW THE FILE MIGHT APPEAR IN THE DATABASE

                    //----------------NECESSARY INITIALIZATIONS TO READ THE DATABASE LINE BY LINE---------------//
                    var lines = File.ReadLines(@pathForDB);
                    int lineNum = 0;
                    bool fileFound = false; //THERE IS SUCH A FILE UPLOADED BY THE REQUESTOR CLIENT

                    foreach (var line in lines)
                    {
                        lineNum++;
                        if (lineNum > 2) //WE START PROCESSING LINES AFTER THE 2ND BECAUSE FIRST TWO LINES OF THE DB ARE DUMMY LINES
                        {
                            //------------FILENAME OF THE CURRENT ENTRY IS EXTRACTED----------//
                            string fileNameExtracted = line.Substring(line.IndexOf("filename") + 10);
                            fileNameExtracted = fileNameExtracted.Substring(0, fileNameExtracted.IndexOf(".") + 4);
                            
                            if(fileNameExtracted == fNameDB)
                            {
                                fileFound = true;
                                break;
                            }
                        }
                    }

                    Byte[] publicAckBuffer; //THIS BUFFER IS CREATED TO SEND A MESSAGE TO CLIENT SIDE ABOUT THE RESULT OF THE DOWNLOAD
                    if (fileFound) //DOWNLOAD WILL BE SUCCESSFUL
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is a file " + fileToDownload + " in the database uploaded by client <" + clientName + "> , download is available!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("Downloading your file " + fileToDownload + " is allowed!\n");
                        thisClient.Send(publicAckBuffer);
                    }
                    else //THERE IS NO FILE AVAILABLE FOR THE DOWNLOAD
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is not " + fileToDownload + " in the database uploaded by client <" + clientName + ">!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("You have not uploaded " + fileToDownload + ", can't download!\n");
                        thisClient.Send(publicAckBuffer);
                        return;
                    }

                    //IF WE DID NOT RETURN SO FAR, THEN EITHER THE CLIENT THAT MADE THE DOWNLOAD REQUEST UPLOADED SUCH A FILE BEFORE OR THE FILE IS UPLOADED BY ANYONE ELSE AND PUBLICISED
                    string storagePath = path_textBox.Text;
                    string pathAndFName = storagePath + "\\" + fNameDB;
                    string data = System.IO.File.ReadAllText(pathAndFName);
                    int dataLength = data.Length;


                    //---------DATA WILL BE SENT PACKET BY PACKET, EACH PACKET HAS 8192 BYTES-------------//
                    //---------CALCULATE THE NUMBER OF PACKETS------------//

                    int numOfPackets = dataLength / 8192; //DATA LENGTH WILL ALWAYS BE A MULTIPLE OF 8192, MIN 8192. THIS IS HOW WE RECEIVE THE UPLOADED ONES BEFORE

                    
                    //----------------SEND THE NUMBER OF PACKETS INFORMATION TO THE CLIENT---------------//
                    Byte[] num_of_packets = new Byte[8192];
                    num_of_packets = Encoding.Default.GetBytes(numOfPackets.ToString());
                    thisClient.Send(num_of_packets);

                    //-----------RECEIVE THE ACK OF CLIENT RECEIVED THE NUMBER OF PACKETS INFORMATION--------//
                    Byte[] usernameAckBuffer = new Byte[8];
                    thisClient.Receive(usernameAckBuffer);
                    Array.Clear(usernameAckBuffer, 0, usernameAckBuffer.Length);

                    //--------SENDING THE SIZE OF THE FILE IS UNNECESSARY BECAUSE CLIENT WONT STORE THAT IN ITS DB, DIRECTLY START SENDING THE PACKETS

                    int index = 0;
                    logs_richTextBox.AppendText(DateTime.Now + " | File is sending to client side...\n");

                    //-----SEND THE DATA PACKET BY PACKET IN A LOOP THAT ITERATES NUMBER OF PACKET TIMES-----------//
                    for (int i = 1; i <= numOfPackets; i++)
                    {
                        Byte[] data_packet = new Byte[8192];
                        string part;

                        if ((data.Length - (i - 1) * 8192) >= 8192)
                            part = data.Substring(index, 8192);

                        else
                            part = data.Substring(index, (data.Length - (i - 1) * 8192));

                        data_packet = Encoding.Default.GetBytes(part);
                        thisClient.Send(data_packet);
                        Array.Clear(data_packet, 0, data_packet.Length);

                        index += 8192;
                    }

                    //--------WHOLE DATA IS SENT WHEN THE LOOP TERMINATES-------------------//
                    logs_richTextBox.AppendText(DateTime.Now + " | File has been sent. \n");
                }
                catch
                {
                    logs_richTextBox.AppendText(DateTime.Now + " | Something went wrong while handling the download request!\n");
                }
            }
        }

        private void MakePublic(Socket thisClient, string clientName)
        {
            if (!terminating)
            {
                try
                {
                    //----------------NAME OF THE FILE TO BE PUBLICISED IS RECEIVED------------------------//
                    Byte[] fileNameBuffer = new Byte[100];
                    thisClient.Receive(fileNameBuffer);
                    string fileToPublic = Encoding.Default.GetString(fileNameBuffer);
                    fileToPublic = fileToPublic.Substring(0, fileToPublic.IndexOf("\0"));
                    fileToPublic = clientName + "_" + fileToPublic; //This is the form that the filename appears in the database

                    logs_richTextBox.AppendText(DateTime.Now + " | User <" + clientName + "> wants to publicise " + fileToPublic + "\n");

                    //----------------NECESSARY INITIALIZATIONS TO READ THE DATABASE LINE BY LINE---------------//
                    var lines = File.ReadLines(@pathForDB);
                    int lineNum = 0;
                    bool entryFound = false;
                    bool usernameExist = false;
                    string total_DB = "Database is created\n\n";

                    foreach (var line in lines)
                    {
                        lineNum++;
                        if (lineNum > 2) //WE START PROCESSING LINES AFTER THE 2ND BECAUSE FIRST TWO LINES OF THE DB ARE DUMMY LINES
                        {
                            //----------------USERNAME OF THE CURRENT ENTRY IS EXTRACTED -------------//
                            string afterVertical = line.Substring(line.IndexOf("|"));
                            string afterColumn = afterVertical.Substring(afterVertical.IndexOf(":") + 2);
                            string username = afterColumn.Substring(0, afterColumn.IndexOf("|") - 1); //username is extracted from the database
                            

                            if (username == clientName) //IF THE ENTRY BELONGS TO THE PARTICULAR CLIENT
                            {
                                usernameExist = true;

                                //------------FILENAME OF THE CURRENT ENTRY IS EXTRACTED----------//
                                string fileNameExtracted = line.Substring(line.IndexOf("filename") + 10);
                                fileNameExtracted = fileNameExtracted.Substring(0, fileNameExtracted.IndexOf(".") + 4);
                                
                                if (fileNameExtracted != fileToPublic) //IF THE FILENAME ON THAT ENTRY IS NOT THE FILE NAME TO BE PUBLICISED
                                {
                                        total_DB += line + "\n"; //DIRECTLY APPEND THAT LINE TO THE EMPTY DATABASE
                                }
                                else //WE FIND THE RELATED ENTRY
                                {
                                    entryFound = true;

                                    //------CHANGE THE PRIVATE FIELD OF THE ENTRY TO PUBLIC------------------//
                                    string woPublicInfo = line.Substring(0, line.IndexOf("*"));
                                    string PublicInfoAdded = woPublicInfo + "*public\n";

                                    total_DB += PublicInfoAdded; //ADD THAT LINE TO DATABASE
                                }
                            }
                            else //IF THE ENTRY BELONGS TO ANOTHER CLIENT
                            {
                                total_DB += line + "\n"; //DIRECTLY ADD THAT LINE BACK
                            }
                        }
                    }

                    Byte[] publicAckBuffer; //THIS BUFFER IS CREATED TO SEND A MESSAGE TO CLIENT SIDE ABOUT THE RESULT OF THE PUBLICATION
                    if(usernameExist == false) //IF THE USERNAME DOES NOT APPEAR IN THE DATABASE AT ALL
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is no entry in the database about user <" + clientName + "> currently!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("You haven't upload any file to the server yet!\n");
                    }
                    else if(entryFound == false) //IF THERE IS NO SUCH FILE TO BE PUBLICISED
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is no file named as " + fileToPublic + " uploaded by user <" + clientName + "> in the database!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("There is no such file in the server!\n");
                    }
                    else //IF EVERYTHING IS SUCCESSFUL
                    {
                        File.WriteAllText(@pathForDB, total_DB);
                        logs_richTextBox.AppendText(DateTime.Now + " | The file " + fileToPublic + " is publicised by the client <" + clientName + ">\n");
                        publicAckBuffer = Encoding.Default.GetBytes("The file is successfully publicised!\n");
                    }
                    thisClient.Send(publicAckBuffer);
                }
                catch
                {
                    logs_richTextBox.AppendText(DateTime.Now + " | Something went wrong while making the requested file public!\n");
                }
            }
        }

        private void RetrieveFileList(Socket thisClient, string clientName, bool isPublic)
        {
            if(!terminating)
            {
                try
                {
                    //----------NECESSARY INITIALIZATIONS TO READ THE DATABASE LINE BY LINE---------------//
                    var lines = File.ReadLines(@pathForDB);
                    int lineNum = 0;
                    string infoToBeSent = "1"; //IN ORDER TO DEAL WITH THE CASE THAT THERE IS NO FILES TO BE RETRIEVED, IF = "" THEN IT CRASHES
                    
                    foreach (var line in lines)
                    {
                        lineNum++;
                        if (lineNum > 2) //FIRST TWO LINES OF THE DATABASE ARE DUMMY LINES
                        {
                            //---------USERNAME IS EXTRACTED FROM THAT ENTRY-----------//
                            string afterVertical = line.Substring(line.IndexOf("|"));
                            string afterColumn = afterVertical.Substring(afterVertical.IndexOf(":") + 2);
                            string username = afterColumn.Substring(0, afterColumn.IndexOf("|") - 1);

                            //------------FILENAME IS EXTRACTED FROM THE LINE------------------//
                            string fileNameExtracted = line.Substring(line.IndexOf("filename") + 10);
                            fileNameExtracted = fileNameExtracted.Substring(fileNameExtracted.IndexOf("_") + 1);
                            fileNameExtracted = fileNameExtracted.Substring(0, fileNameExtracted.IndexOf(".") + 4);

                            //----------------DATE OF UPLOAD IS EXTRACTED FROM THE LINE------------//
                            string date = line.Substring(0, line.IndexOf(" |"));

                            //-----------------SIZE OF THE FILE IS EXTRACTED FROM THE LINE---------------//
                            string size = line.Substring(line.IndexOf("size: ") + 6);
                            size = size.Substring(0, size.IndexOf(" |"));
                            
                            //----------------PUBLIC/PRIVATE INFORMATION IS EXTRACTED FROM THE LINE---------------//
                            string priv_pub = line.Substring(line.IndexOf("*"));

                            if(isPublic == false) //THE REQUEST IS JUST FOR THE FILES THAT THE REQUESTOR CLIENT UPLOADED
                            {
                                if (username == clientName) //THE USERNAME ON THE LINE AND THE CLIENT WHO MAKES THE REQUEST MATCHES
                                {
                                    infoToBeSent += date + "\t" + fileNameExtracted + "\t" + size + "\n"; //THIS WILL BE SENT TO CLIENT
                                }
                            }
                            else //THE REQUEST IS FOR THE PUBLIC FILES UPLOADED BY ANY CLIENT
                            {
                                if (priv_pub == "*public")
                                {
                                    infoToBeSent += date + "\t" + username + "\t" + fileNameExtracted + "\t" + size + "\n"; //THIS WILL BE SENT TO CLIENT
                                }
                            }
                        }
                    }

                    //-------------SEND THE LIST OF FILES UPLOADED BY THE REQUESTOR CLIENT----------------//
                    Byte[] retrieveBuffer = Encoding.Default.GetBytes(infoToBeSent);
                    thisClient.Send(retrieveBuffer);
                    if (isPublic == false)
                        logs_richTextBox.AppendText(DateTime.Now + " | The list of files sent by client <" + clientName + "> has been sent to client <" + clientName + ">\n");
                    else logs_richTextBox.AppendText(DateTime.Now + " | The list of public files has been sent to client <" + clientName + ">\n");
                }
                catch(Exception e)
                {
                    logs_richTextBox.AppendText(DateTime.Now + " | Something went wrong while sending the list of filenames!\n");
                }
            }
        }

        private void deleteFile(Socket thisClient, string clientName)
        {
            if (!terminating)
            {
                try
                {
                    //----------------NAME OF THE FILE TO BE DELETED IS RECEIVED------------------------//
                    Byte[] fileNameBuffer = new Byte[100];
                    thisClient.Receive(fileNameBuffer);
                    string fileToDelete = Encoding.Default.GetString(fileNameBuffer);
                    fileToDelete = fileToDelete.Substring(0, fileToDelete.IndexOf("\0"));
                    fileToDelete = clientName + "_" + fileToDelete; //This is the form that the filename appears in the database

                    logs_richTextBox.AppendText(DateTime.Now + " | User <" + clientName + "> wants to delete " + fileToDelete + "\n");


                    //----------------NECESSARY INITIALIZATIONS TO READ THE DATABASE LINE BY LINE---------------//
                    var lines = File.ReadLines(@pathForDB);
                    int lineNum = 0;
                    bool entryFound = false;
                    bool usernameExist = false;
                    string total_DB = "Database is created\n\n";


                    foreach (var line in lines)
                    {
                        lineNum++;
                        if (lineNum > 2) //WE START PROCESSING LINES AFTER THE 2ND BECAUSE FIRST TWO LINES OF THE DB ARE DUMMY LINES
                        {
                            //----------------USERNAME OF THE CURRENT ENTRY IS EXTRACTED -------------//
                            string afterVertical = line.Substring(line.IndexOf("|"));
                            string afterColumn = afterVertical.Substring(afterVertical.IndexOf(":") + 2);
                            string username = afterColumn.Substring(0, afterColumn.IndexOf("|") - 1); //username is extracted from the database


                            if (username == clientName) //IF THE ENTRY BELONGS TO THE PARTICULAR CLIENT
                            {
                                usernameExist = true;

                                //------------FILENAME OF THE CURRENT ENTRY IS EXTRACTED----------//
                                string fileNameExtracted = line.Substring(line.IndexOf("filename") + 10);
                                fileNameExtracted = fileNameExtracted.Substring(0, fileNameExtracted.IndexOf(".") + 4);

                                if (fileNameExtracted != fileToDelete) //IF THE FILENAME ON THAT ENTRY IS NOT THE FILE NAME TO BE PUBLICISED
                                {
                                    total_DB += line + "\n"; //DIRECTLY APPEND THAT LINE TO THE EMPTY DATABASE
                                }
                                else //WE FIND THE RELATED ENTRY
                                {
                                    entryFound = true;

                                    //------NEGLECT THE ENTRY AND DELETE THE CORRESPONDING FILE ------------------//
                                    string delete_fileName = "\\" + fileToDelete;
                                    string delete_path = file_path + delete_fileName;
                                    var path = @delete_path;

                                    File.Delete(path);
                                }
                            }
                            else //IF THE ENTRY BELONGS TO ANOTHER CLIENT
                            {
                                total_DB += line + "\n"; //DIRECTLY ADD THAT LINE BACK
                            }
                        }
                    }

                    Byte[] publicAckBuffer; //THIS BUFFER IS CREATED TO SEND A MESSAGE TO CLIENT SIDE ABOUT THE RESULT OF THE DELETION
                    if (usernameExist == false) //IF THE USERNAME DOES NOT APPEAR IN THE DATABASE AT ALL
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is no entry in the database about user <" + clientName + "> currently!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("You haven't upload any file to the server yet!\n");
                    }
                    else if (entryFound == false) //IF THERE IS NO SUCH FILE TO BE DELETED
                    {
                        logs_richTextBox.AppendText(DateTime.Now + " | There is no file named as " + fileToDelete + " uploaded by user <" + clientName + "> in the database!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("There is no such file in the server!\n");
                    }
                    else //IF EVERYTHING IS SUCCESSFUL
                    {
                        File.WriteAllText(@pathForDB, total_DB);
                        logs_richTextBox.AppendText(DateTime.Now + " | The file " + fileToDelete + " deleted by the client <" + clientName + ">\n");
                        publicAckBuffer = Encoding.Default.GetBytes("The file successfully deleted!\n");
                    }
                    thisClient.Send(publicAckBuffer);

                }

                catch
                {
                    logs_richTextBox.AppendText(DateTime.Now + " | Something went wrong while deleting the requested file!\n");
                }

            }
        }

        private void copyFile(Socket thisClient, string clientName)
        {
            if (!terminating)
            {
                try
                {
                    //----------------NAME OF THE FILE TO BE COPIED IS RECEIVED------------------------//
                    Byte[] fileNameBuffer = new Byte[100];
                    thisClient.Receive(fileNameBuffer);
                    string fileToCopy = Encoding.Default.GetString(fileNameBuffer);
                    fileToCopy = fileToCopy.Substring(0, fileToCopy.IndexOf("\0"));
                    fileToCopy = clientName + "_" + fileToCopy; //This is the form that the filename appears in the database

                    logs_richTextBox.AppendText("User <" + clientName + "> wants to copy " + fileToCopy + "\n");

                    //----------------NECESSARY INITIALIZATIONS TO READ THE DATABASE LINE BY LINE---------------//
                    var lines = File.ReadLines(@pathForDB);
                    int lineNum = 0;
                    bool entryFound = false;
                    bool usernameExist = false;
                    string total_DB = "Database is created\n\n";

                    foreach (var line in lines)
                    {
                        lineNum++;
                        if (lineNum > 2) //WE START PROCESSING LINES AFTER THE 2ND BECAUSE FIRST TWO LINES OF THE DB ARE DUMMY LINES
                        {
                            //----------------USERNAME OF THE CURRENT ENTRY IS EXTRACTED -------------//
                            string afterVertical = line.Substring(line.IndexOf("|"));
                            string afterColumn = afterVertical.Substring(afterVertical.IndexOf(":") + 2);
                            string username = afterColumn.Substring(0, afterColumn.IndexOf("|") - 1); //username is extracted from the database

                            if (username == clientName)
                            {
                                usernameExist = true;

                                //------------FILENAME OF THE CURRENT ENTRY IS EXTRACTED----------//
                                string fileNameExtracted = line.Substring(line.IndexOf("filename") + 10);
                                fileNameExtracted = fileNameExtracted.Substring(0, fileNameExtracted.IndexOf(".") + 4);

                                if (fileNameExtracted != fileToCopy) //IF THE FILENAME ON THAT ENTRY IS NOT THE FILE NAME TO BE COPIED
                                {
                                    total_DB += line + "\n"; //DIRECTLY APPEND THAT LINE TO THE EMPTY DATABASE
                                }
                                else  //WE FIND THE RELATED ENTRY
                                {
                                    entryFound = true;

                                    string file_name = fileNameExtracted.Substring(0, fileNameExtracted.IndexOf("."));

                                    string save_fileName = "\\" + file_name + ".txt";
                                    string write_path = file_path + save_fileName;
                                    var path = @write_path;

                                    int counter = 1;
                                    while (File.Exists(path)) //INCREMENT AND ADD THE NEXT NUMBER TO THE NAME OF THE FILE TO GUARANTEE UNIQUENESS
                                    {
                                        save_fileName = "";
                                        save_fileName = "\\" + file_name + "(" + counter + ").txt";
                                        write_path = file_path + save_fileName;
                                        path = @write_path;
                                        counter++;
                                    }
                                    string remainingInfo = line.Substring(line.IndexOf("filename") + 10);
                                    remainingInfo = remainingInfo.Substring(remainingInfo.IndexOf(".txt") + 4);
                            
                                    string copy_fileName = save_fileName.Substring(1, save_fileName.Length - 1);
                                    string copy_line = DateTime.Now + " | username: " + clientName + " | filename: " + copy_fileName + remainingInfo;
                                    logs_richTextBox.AppendText(copy_fileName + "\n");
                                    logs_richTextBox.AppendText(remainingInfo + "\n");
                                    logs_richTextBox.AppendText(copy_line + "\n");


                                    string dest_Path = file_path + "\\" + copy_fileName;
                                    string source_Path = file_path + "\\" + fileToCopy;

                                    total_DB += line + "\n" + copy_line + "\n"; //ADD COPIED LINE TO DATABASE.

                                    File.Copy(source_Path, dest_Path); //COPIES FILE IN THE SOURCE PATH TO DESTINATON PATH


                                }
                            }

                            else //IF THE ENTRY BELONGS TO ANOTHER CLIENT
                            {
                                total_DB += line + "\n"; //DIRECTLY ADD THAT LINE BACK
                            }
                        }
                    }

                    Byte[] publicAckBuffer; //THIS BUFFER IS CREATED TO SEND A MESSAGE TO CLIENT SIDE ABOUT THE RESULT OF THE COPYING
                    if (usernameExist == false) //IF THE USERNAME DOES NOT APPEAR IN THE DATABASE AT ALL
                    {
                        logs_richTextBox.AppendText("There is no entry in the database about user <" + clientName + "> currently!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("You haven't upload any file to the server yet!\n");
                    }
                    else if (entryFound == false) //IF THERE IS NO SUCH FILE TO BE COPIED
                    {
                        logs_richTextBox.AppendText("There is no file named as " + fileToCopy + " uploaded by user <" + clientName + "> in the database!\n");
                        publicAckBuffer = Encoding.Default.GetBytes("There is no such file in the server!\n");
                    }
                    else //IF EVERYTHING IS SUCCESSFUL
                    {
                        File.WriteAllText(@pathForDB, total_DB);
                        logs_richTextBox.AppendText("The file " + fileToCopy + " copied by the client <" + clientName + ">\n");
                        publicAckBuffer = Encoding.Default.GetBytes("The file successfully copied!\n");
                    }
                    thisClient.Send(publicAckBuffer);

                }
                catch
                {
                    logs_richTextBox.AppendText("Something went wrong while copying the requested file!\n");
                }
            }
        }

        private void ServerForm_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            //Byte[] endingBuffer = Encoding.Default.GetBytes("-1");
            //for (int i = 0; i < clientSockets.Count(); i++)
            //{
            //    clientSockets[i].Send(endingBuffer);
            //    clientSockets[i].Close();
            //}
            Environment.Exit(0);
        } 
    }
}
