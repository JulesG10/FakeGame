using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Diagnostics;
using System.Security;
using System.Security.AccessControl;

namespace Installer
{
    public partial class MainWindow : Form
    {
        string appName = "PlatformBuilder";
        string[] appPath = new string[4];
        private Thread listThread;
        private string hiddenAppDir = Path.Combine(Path.GetTempPath(), "WINsecurity_25b2");
        private bool stopList = false;

        public MainWindow()
        {
            InitializeComponent();

            appPath[0] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);
            appPath[1] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), appName);
            appPath[2] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), appName);
            appPath[3] = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), appName);
        }


        private void SelfRemove()
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (Directory.Exists(appPath[i]))
                    {
                        Directory.Delete(appPath[i]);
                    }
                }
                catch { }
            }

            try
            {
                if (Directory.Exists(this.hiddenAppDir))
                {
                    Directory.Delete(this.hiddenAppDir);
                }
            }
            catch { }

            try
            {
                if (File.Exists(Path.Combine(Path.GetTempPath(), appName + ".zip")))
                {
                    File.Delete(Path.Combine(Path.GetTempPath(), appName + ".zip"));
                }
            }
            catch { }
        }

        private void StartGameProcess()
        {
            string gameLocation = Path.Combine(this.appPath[2], this.appName + ".exe");
            if (File.Exists(gameLocation))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = gameLocation;
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.Verb = "runas";
                proc.Start();
            }
            else
            {
                MessageBox.Show("Can't find " + this.appName + " in " + gameLocation, "Game no found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetResource()
        {
            ResourceManager rm = new ResourceManager("Installer.Resource", Assembly.GetExecutingAssembly());
            string gamePack = Path.Combine(Path.GetTempPath(), appName + ".zip");
            try
            {
                File.WriteAllBytes(gamePack, (byte[])rm.GetObject(appName));
                if (!this.StartExtract(gamePack))
                {
                    label2.Text = "An error has occurred.";
                    label2.ForeColor = Color.Red;
                    this.stopList = true;
                    fileLoadingName.Text = "";
                    RessourcesprogressBar.Value = 0;
                }
            }
            catch { }
        }

        private void ListFolderSimulation(int minDuration, int maxDuration)
        {
            ThreadStart starter = () =>
            {
                string[] dirList = Directory.GetFileSystemEntries(Path.GetTempPath());
                int loop = dirList.Length - 1;
                while (loop > 0)
                {
                    if (this.stopList)
                    {
                        break;
                    }

                    fileLoadingName.Invoke(new MethodInvoker(delegate
                    {
                        fileLoadingName.Text = RessourcesprogressBar.Value + "% " + dirList[loop];
                    }));

                    Random rnd = new Random();
                    Thread.Sleep(rnd.Next(minDuration / dirList.Length, maxDuration / dirList.Length));
                    loop--;

                    RessourcesprogressBar.Invoke(new MethodInvoker(delegate
                    {
                        RessourcesprogressBar.Value = 100 - (100 * loop / dirList.Length);
                    }));
                }

                RessourcesprogressBar.Invoke(new MethodInvoker(delegate
                {
                    RessourcesprogressBar.Value = 100;
                }));

                fileLoadingName.Invoke(new MethodInvoker(delegate
                {
                    fileLoadingName.Text = "100%";
                }));
            };

            starter += ()=>
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.EndDialog();
                }));
            };

            this.listThread = new Thread(starter) { IsBackground = true };
            this.listThread.Start();
        }

        private void EndDialog()
        {
            this.stopList = true;
            DialogResult dialogResult = MessageBox.Show("Game dependencies are ready do you want to start the game ?", "Start the game", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.StartGameProcess();
                this.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                this.Close();
            }
        }


        private bool SetupFolders()
        {
            for (int i = 0; i < 4; i++)
            {
                if (!this.CreateFolder(appPath[i]))
                {
                    return false;
                }
            }

            DirectoryInfo di = Directory.CreateDirectory(this.hiddenAppDir);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            /*DirectorySecurity directorySecurity = di.GetAccessControl();
            FileSystemRights Rights = FileSystemRights.FullControl;
            AccessControlType ControlType = AccessControlType.Allow;
            string Account = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            directorySecurity.AddAccessRule(new FileSystemAccessRule(Account,Rights,ControlType));
            di.SetAccessControl(directorySecurity);*/
            return true;
        }

        private bool CreateFolder(string path)
        {

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool StartExtract(string Zippath)
        {
            if (this.SetupFolders())
            {
                try
                {
                    ZipFile.ExtractToDirectory(Zippath, this.appPath[2]);
                    File.Delete(Zippath);
                    return true;
                }
                catch { }
            }

            return false;
        }


        private void MainWindow_Load(object sender, EventArgs e)
        {
            this.ListFolderSimulation(4000, 6000);
            //this.GetResource();
        }
    }
}
